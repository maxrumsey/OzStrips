using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using vatsys;
using static MaxRumsey.OzStripsPlugin.GUI.Shared.ConnectionMetadataDTO;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// Handles communications by the sockets.
/// </summary>
public sealed class SocketConn : IDisposable
{
    private readonly MainFormController _mainForm;
    private readonly HubConnection _connection;
    private readonly BayManager _bayManager;
    private readonly bool _isDebug = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VisualStudioEdition"));
    private readonly SemaphoreSlim _connectionSemaphore = new(1, 1);

    private bool _serverPopupShown;
    private bool _enableAutoReconnect = true;
    private DateTime _lastDesyncResolution = DateTime.MinValue;
    private bool _synchronised;

    private bool FreshClient
    {
        get
        {
            return (DateTime.Now - (_aerodromeSubscriptionRegistered ?? DateTime.Now)) < TimeSpan.FromMinutes(1);
        }
    }

    private bool _versionShown;
    private bool _isDisposed;
    private DateTime? _aerodromeSubscriptionRegistered;

    /// <summary>
    /// Initializes a new instance of the <see cref="SocketConn"/> class.
    /// </summary>
    /// <param name="bayManager">The bay manager.</param>
    /// <param name="mainForm">The main form instance.</param>
    public SocketConn(BayManager bayManager, MainFormController mainForm)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(OzStripsConfig.socketioaddr + "ozstrips/hub/v2")
            .WithAutomaticReconnect()
            .Build();

        _bayManager = bayManager;
        _mainForm = mainForm;

        _connection.Closed += async (error) => await ConnectionStateChanged(ConnectionState.DISCONNECTED, error);
        _connection.Reconnected += async (connId) => await ConnectionStateChanged(ConnectionState.CONNECTED);
        _connection.Reconnecting += async (error) => await ConnectionStateChanged(ConnectionState.RECONNECTING, error);

        RegisterListener<StripDTO?>("StripUpdate", async scDTO =>
        {
            if (scDTO is not null)
            {
                InvokeOnGUI(() => _bayManager.StripRepository.UpdateStripData(scDTO, bayManager));
            }
        });

        RegisterListener<StripDTO[]>("StripCache", async scDTO =>
        {
            if (scDTO is not null && FreshClient)
            {
                InvokeOnGUI(() => _bayManager.StripRepository.LoadCache(scDTO ?? [], bayManager, this));
            }
        });

        RegisterListener("UpdateCache", async () =>
        {
            if (!FreshClient)
            {
                InvokeOnGUI(async () =>
                {
                    try
                    {
                        await SendCache();
                    }
                    catch (Exception ex)
                    {
                        Util.LogError(ex);
                    }
                });
            }
        });

        RegisterListener("SendCDM", async () =>
        {
            if (!FreshClient)
            {
                InvokeOnGUI(async () =>
                {
                    try
                    {
                        await SendCDMFull();
                    }
                    catch (Exception ex)
                    {
                        Util.LogError(ex);
                    }
                });
            }
        });

        RegisterListener<string?>("Atis", async code =>
        {
            if (code is not null)
            {
                InvokeOnGUI(() => mainForm.SetATISCode(code));
            }
        });

        RegisterListener<string?>("Metar", async metar =>
        {
            if (metar is not null)
            {
                InvokeOnGUI(() => mainForm.SetMetar(metar));
            }
        });

        RegisterListener<BayDTO?>("BayUpdate", async bayDTO =>
        {
            if (bayDTO is not null)
            {
                InvokeOnGUI(() => bayManager.BayRepository.UpdateOrder(bayDTO));
            }
        });

        RegisterListener<string?>("GetStripStatus", async acid =>
        {
            if (acid is not null)
            {
                InvokeOnGUI(() => _bayManager.StripRepository.GetStripStatus(acid, this));
            }
        });

        RegisterListener<string?>("Message", async message =>
        {
            if (message is not null)
            {
                InvokeOnGUI(() => Util.ShowWarnBox(message));
            }
        });

        RegisterListener<AerodromeState?>("AerodromeStateUpdate", async state =>
        {
            if (state is not null)
            {
                _bayManager.AerodromeState = state;
                InvokeOnGUI(() => AerodromeStateChanged?.Invoke(this, EventArgs.Empty));
            }
        });

        RegisterListener<string[]?>("NewPDC", async pdcs =>
        {
            if (pdcs is not null && pdcs.Length > 0)
            {
                InvokeOnGUI(() => NewPDCsReceived?.Invoke(this, pdcs));
            }
        });

        RegisterListener<string?>("VersionInfo", async appversion =>
        {
            if (appversion is null)
            {
                return;
            }

            if (!_versionShown && appversion != OzStripsConfig.version && !AerodromeManager.InhibitVersionCheck)
            {
                _versionShown = true;
                InvokeOnGUI(() => Util.ShowInfoBox("New Update Available: " + appversion));
            }
        });

        RegisterListener("OutOfSync", async () =>
        {
            InvokeOnGUI(async () =>
            {
                if (DateTime.Now - _lastDesyncResolution < TimeSpan.FromSeconds(5))
                {
                    return;
                }

                var res = Util.ShowQuestionBox("Client became desynchronised from server. Reconnect?");
                if (res == DialogResult.Yes)
                {
                    _lastDesyncResolution = DateTime.Now;
                    await SubscribeToAerodrome();
                }
                else
                {
                    _lastDesyncResolution = DateTime.Now;
                    _enableAutoReconnect = false;
                    Disconnect();
                }
            });
        });
    }

    /// <summary>
    /// An event called when the aerodrome state changes.
    /// </summary>
    public event EventHandler? AerodromeStateChanged;

    /// <summary>
    /// An event called when the server type changes.
    /// </summary>
    public event EventHandler? ServerTypeChanged;

    /// <summary>
    /// An event called when new PDCs are received from server.
    /// </summary>
    public event EventHandler<string[]>? NewPDCsReceived;

    /// <summary>
    /// Gets the messages, used for debugging.
    /// </summary>
    public List<string> Messages { get; } = [];

    /// <summary>
    /// Gets or sets the current server type.
    /// </summary>
    public Servers Server { get; set; } = Servers.VATSIM;

    /// <summary>
    /// Gets the connection state.
    /// </summary>
    public ConnectionState State { get; private set; } = SocketConn.ConnectionState.DISCONNECTED;

    /// <summary>
    /// Gets a value indicating whether we should be able to send strip and bay updates to the server.
    /// </summary>
    public bool HaveSendPerms
    {
        get
        {
            return Network.Me.IsRealATC || _isDebug;
        }
    }

    private static bool MainFormValid => MainFormController.Instance?.IsDisposed == false && MainFormController.Instance.Visible;

    /// <summary>
    /// Gets a value indicating whether the user has permission to send data to server.
    /// </summary>
    private bool CanSendDTO
    {
        get
        {
            return _connection.State == HubConnectionState.Connected && (Network.Me.IsRealATC || _isDebug) && _synchronised;
        }
    }

    /// <summary>
    /// Syncs the strip controller.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    /// <returns>Task.</returns>
    public async Task SyncSC(Strip sc)
    {
        StripDTO scDTO = sc;
        if (CanSendDTO)
        {
            LogMessageContent("StripChange", scDTO, false);
            await _connection.InvokeAsync("StripChange", scDTO, GetMessageMetadata());
        }
    }

    /// <summary>
    /// Sends strip status to the server.
    /// </summary>
    /// <param name="strip">Strip object.</param>
    /// <param name="acid">Strip callsign.</param>
    public void SendStripStatus(Strip? strip, string acid)
    {
        if (CanSendDTO)
        {
            LogMessageContent("StripStatus", strip is null ? null : (StripDTO)strip, false);
        }

        if (CanSendDTO && strip is not null)
        {
            _connection.InvokeAsync("StripStatus", (StripDTO)strip, acid, GetMessageMetadata());
        }
        else if (CanSendDTO)
        {
            _connection.InvokeAsync("StripStatus", null, acid, GetMessageMetadata());
        }
    }

    /// <summary>
    /// Sends a CDM update to the server.
    /// </summary>
    /// <param name="strip">Strip.</param>
    /// <param name="state">Current state.</param>
    public void SendCDMUpdate(Strip strip, CDMState state)
    {
        var dto = new CDMAircraftDTO()
        {
            Key = strip.StripKey,
            State = state,
            RWY = strip.RWY,
        };

        var list = new CDMAircraftList();
        list.CheckAndAddAircraft(dto, _bayManager);

        if (CanSendDTO && list.Count > 0)
        {
            LogMessageContent("UplinkCDMAircraft", list, false);
            _connection.InvokeAsync("UplinkCDMAircraft", list, GetMessageMetadata());
        }
    }

    /// <summary>
    /// Requests routes for a given sc.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    /// <returns>Task.</returns>
    public async Task RequestRoutes(Strip sc)
    {
        try
        {
            LogMessageContent("GetRoutes", sc.StripKey, false);

            if (_connection.State == HubConnectionState.Connected && _synchronised)
            {
                var routes = await _connection.InvokeAsync<RouteDTO[]?>("GetRoutes", sc.StripKey, GetMessageMetadata());

                if (
                    routes is null ||
                    routes.Length == 0)
                {
                    return;
                }

                sc.ValidRoutes = routes;
            }
        }
        catch (TimeoutException)
        {
        }
        catch (HubException)
        {
        }
        catch (System.Text.Json.JsonException)
        {
        }
    }

    /// <summary>
    /// Requests strip data from the server.
    /// </summary>
    /// <param name="strip">Strip to fetch.</param>
    /// <returns>Task.</returns>
    public async Task RequestStrip(Strip strip)
    {
        if (_connection.State == HubConnectionState.Connected && _synchronised)
        {
            LogMessageContent("RequestStrip", strip.StripKey, false);
            var dto = await _connection.InvokeAsync<StripDTO?>("RequestStrip", strip.StripKey, GetMessageMetadata());

            if (dto is not null)
            {
                _bayManager.StripRepository.UpdateStripData(dto, _bayManager);
            }
        }
    }

    /// <summary>
    /// Sends a Hoppies PDC to the server.
    /// </summary>
    /// <param name="strip">Strip.</param>
    /// <param name="text">PDC text.</param>
    /// <returns>Task.</returns>
    public async Task SendPDC(Strip strip, string text)
    {
        if (CanSendDTO)
        {
            LogMessageContent("SendPDC", text, false);
            await _connection.InvokeAsync("SendPDC", (StripDTO)strip, text, GetMessageMetadata());
        }
    }

    /// <summary>
    /// Sync the bay to the socket.
    /// </summary>
    /// <param name="bayChange">The bay to sync.</param>
    public void SyncBay(BayChange bayChange)
    {
        if (CanSendDTO)
        {
            LogMessageContent("BayChange", bayChange, false);
            _connection.InvokeAsync("BayChange", bayChange, GetMessageMetadata());
        }
    }

    /// <summary>
    /// Sends circuit activity status to the server.
    /// </summary>
    /// <param name="status">Circuit enabled.</param>
    public void RequestCircuit(bool status)
    {
        if (CanSendDTO)
        {
            LogMessageContent("UpdateCircuitMode", status, false);
            _connection.InvokeAsync("UpdateCircuitMode", status, GetMessageMetadata());
        }
    }

    /// <summary>
    /// Sends coordinator bay activity status to the server.
    /// </summary>
    /// <param name="status">Circuit enabled.</param>
    public void RequestCoordinator(bool status)
    {
        if (CanSendDTO)
        {
            LogMessageContent("UpdateCoordinatorMode", status, false);
            _connection.InvokeAsync("UpdateCoordinatorMode", status, GetMessageMetadata());
        }
    }

    /// <summary>
    /// Sends new CDM parameters to the server.
    /// </summary>
    /// <param name="param">CDM Parameters.</param>
    public void SendCDMParameters(CDMParameters param)
    {
        if (CanSendDTO)
        {
            LogMessageContent("ChangeCDMParameters", param, false);
            _connection.InvokeAsync("ChangeCDMParameters", param, GetMessageMetadata());
        }
    }

    /// <summary>
    /// Sets the aerodrome based on the bay manager.
    /// </summary>
    /// <returns>Task.</returns>
    /// <exception cref="Exception">Server error.</exception>
    /// <exception cref="ArgumentNullException">Connection data was not returned.</exception>
    /// <exception cref="ArgumentException">Connection data did not match our copy.</exception>
    public async Task SubscribeToAerodrome()
    {
        try
        {
            if (_connection.State == HubConnectionState.Connected) // was is io connected.
            {
                var connmetadata = new ConnectionMetadataDTO()
                {
                    Version = OzStripsConfig.version,
                    APIVersion = "2",
                    Server = Server,
                    AerodromeName = _bayManager.AerodromeName,
                    Callsign = Network.Me.Callsign,
                };
                LogMessageContent("SubscribeToAerodrome", connmetadata, false);

                _synchronised = false;
                var response = await _connection.InvokeAsync<AerodromeSubscriptionResponse>("SubscribeToAerodrome", connmetadata);
                _synchronised = true;

                if (response.Error is not null)
                {
                    throw response.Error;
                }
                else if (response is null)
                {
                    throw new ArgumentNullException("Subscription response was not included after aerodrome subscription.");
                }
                else if (response.AerodromeICAO != _bayManager.AerodromeName ||
                    response.Server != Server)
                {
                    throw new ArgumentException("Server details did not match retained details.");
                }

                InvokeOnGUI(() =>
                {
                    _bayManager.StripRepository.LoadCache(response.StripCache ?? [], _bayManager, this);

                    foreach (var bay in response.Bays ?? [])
                    {
                        if (bay is not null)
                        {
                            InvokeOnGUI(() => _bayManager.BayRepository.UpdateOrder(bay));
                        }
                    }
                });

                _aerodromeSubscriptionRegistered = DateTime.Now;
            }
            else
            {
                _enableAutoReconnect = true;
                await Connect();
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Sets the server type.
    /// </summary>
    /// <param name="type">Server connection type.</param>
    public async void SetServerType(Servers type)
    {
        try
        {
            Server = type;
            ServerTypeChanged?.Invoke(this, EventArgs.Empty);

            if (!CanConnectToCurrentServer())
            {
                return;
            }

            await SubscribeToAerodrome();
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Sends the cache to the server.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task SendCache()
    {
        var cacheDTO = CreateCacheDTO();
        if (CanSendDTO)
        {
            LogMessageContent("StripCache", cacheDTO, false);
            await _connection.InvokeAsync("StripCache", cacheDTO, GetMessageMetadata());
        }
    }

    /// <summary>
    /// Uplinks all CDM-active aircraft to the server.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task SendCDMFull()
    {
        var clearedBay = _bayManager.BayRepository.Bays.FirstOrDefault(x => x.BayTypes.Contains(StripBay.BAY_CLEARED));

        var activeStrips = new List<Strip>();
        var pushedStrips = _bayManager.StripRepository.Strips.Where(x => x.CurrentBay is >= StripBay.BAY_PUSHED and <= StripBay.BAY_RUNWAY);
        var depStrips = _bayManager.StripRepository.Strips.Where(x =>
        {
            var pilot = Network.GetOnlinePilots.Find(y => y.Callsign == x.FDR.Callsign);

            return pilot is not null && pilot.GroundSpeed > 50;
        });

        if (clearedBay is not null)
        {
            var barFound = false;

            foreach (var strip in clearedBay.Strips)
            {
                if (strip.Type == StripItemType.QUEUEBAR)
                {
                    barFound = true;
                    break;
                }
                else if (strip.Type == StripItemType.STRIP)
                {
                    activeStrips.Add(strip.Strip!);
                }
            }

            if (!barFound)
            {
                activeStrips.Clear();
            }
        }

        activeStrips.AddRange(_bayManager.StripRepository.Strips.Where(x => x.CurrentBay == StripBay.BAY_COORDINATOR));

        activeStrips = activeStrips.Distinct().ToList();

        var cdmDTOs = new CDMAircraftList(
        pushedStrips.Select(x => new CDMAircraftDTO()
        {
            Key = x.StripKey,
            State = CDMState.PUSHED,
            RWY = x.RWY,
        }).ToList(),
        _bayManager);

        cdmDTOs.OverwriteAndAddAiraftList(
        activeStrips.Select(x => new CDMAircraftDTO()
        {
            Key = x.StripKey,
            State = CDMState.ACTIVE,
            RWY = x.RWY,
        }).ToList(),
        _bayManager);

        cdmDTOs.OverwriteAndAddAiraftList(
        depStrips.Select(x => new CDMAircraftDTO()
        {
            Key = x.StripKey,
            State = CDMState.COMPLETE,
            RWY = x.RWY,
        }).ToList(),
        _bayManager);

        if (CanSendDTO)
        {
            LogMessageContent("UplinkCDMAircraft", cdmDTOs, false);
            await _connection.SendAsync("UplinkCDMAircraft", cdmDTOs);
        }
    }

    /// <summary>
    /// Creates a connection to the server.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task Connect()
    {
        AddMessage("#Attempting connection " + OzStripsConfig.socketioaddr);
        await _connectionSemaphore.WaitAsync();

        // try-catch to ensure semaphore is released.
        try
        {
            while (!_isDisposed)
            {
                if (State != ConnectionState.DISCONNECTED)
                {
                    return;
                }

                // Try to catch internet errors etc
                try
                {
                    if (!MainFormController.ReadyForConnection || !CanConnectToCurrentServer())
                    {
                        return;
                    }

                    await _connection.StartAsync();
                    break;
                }
                catch (Exception ex)
                {
                    Errors.Add(ex, "OzStrips - Server Connection Failed");
                    await Task.Delay(TimeSpan.FromSeconds(10 + ((new Random().NextDouble() * 4) - 2)));
                }
            }
        }
        finally
        {
            _connectionSemaphore.Release();
        }

        try
        {
            await ConnectionStateChanged(ConnectionState.CONNECTED);
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Disconnects the io.
    /// </summary>
    public void Disconnect()
    {
        _connection.StopAsync();
    }

    /// <summary>
    /// Marks the server as desynchronised when effecting an aerodrome change.
    /// </summary>
    public void MarkDesynchronised()
    {
        _synchronised = false;
    }

    /// <inheritdoc/>
    public async void Dispose()
    {
        _isDisposed = true;
        await _connection.DisposeAsync();
    }

    /// <summary>
    /// Creates the cache data transfer object.
    /// </summary>
    /// <returns>The cache data transfer object.</returns>
    private CacheDTO CreateCacheDTO()
    {
        return new() { strips = _bayManager.StripRepository.Strips.Select(x => (StripDTO)x).ToList(), };
    }

    private bool CanConnectToCurrentServer()
    {
        if (!Network.IsOfficialServer && Server == Servers.VATSIM)
        {
            if (!_serverPopupShown)
            {
                _serverPopupShown = true;
                var result = Util.ShowQuestionBox("Connection to OzStrips main server detected while connected to the Sweatbox.\n\n" +
                    "Would you like to go to Settings and set Sweatbox mode?");

                if (result == DialogResult.Yes)
                {
                    _mainForm.ShowSettings(this, new());
                }
            }

            return false;
        }

        return true;
    }

    private void AddMessage(string message)
    {
        lock (Messages)
        {
            Messages.Add(message);
        }
    }

    private void InvokeOnGUI(Action action)
    {
        if (MainFormValid)
        {
            try
            {
                _mainForm.Invoke(() =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        Util.LogError(ex);
                    }
                });
            }

            // This is really stupid.
            catch (ObjectDisposedException)
            {
            }
        }
    }

    private async Task ConnectionStateChanged(ConnectionState newState, Exception? ex = null)
    {
        if (_isDisposed || !MainFormValid)
        {
            return;
        }

        if (ex is not null)
        {
            AddMessage($"#Connection error: {ex.Message}");
        }

        if (newState == ConnectionState.CONNECTED && State is ConnectionState.RECONNECTING or ConnectionState.DISCONNECTED)
        {
            _aerodromeSubscriptionRegistered = DateTime.Now;
            State = ConnectionState.CONNECTED;

            if (!Network.IsConnected)
            {
                AddMessage("#Connected, but network is not. Rejecting.");
                Disconnect();
                return;
            }

            AddMessage("#Connected");

            await SubscribeToAerodrome();

            _mainForm.Invoke(_mainForm.SetConnStatus);
            _bayManager.StripRepository.MarkAllStripsAsAwaitingRoutes();
        }
        else if (newState == ConnectionState.DISCONNECTED && State is ConnectionState.RECONNECTING or ConnectionState.CONNECTED)
        {
            AddMessage("#Disconnected.");
            _aerodromeSubscriptionRegistered = null;
            State = ConnectionState.DISCONNECTED;

            _mainForm.Invoke(_mainForm.SetConnStatus);

            if (Network.IsConnected && _enableAutoReconnect)
            {
                await Connect();
            }
        }
        else if (newState == ConnectionState.RECONNECTING && State is ConnectionState.DISCONNECTED or ConnectionState.CONNECTED)
        {
            AddMessage("#Reconnecting.");
            if (!Network.IsConnected)
            {
                AddMessage("#Reconnecting, but network is not connected. Rejecting.");
                Disconnect();
                return;
            }

            _mainForm.Invoke(_mainForm.SetConnStatus);
        }
        else
        {
#if DEBUG
            Errors.Add(new($"Connection state changed in an unexpected manner {State} to {newState}."));
#endif
        }
    }

    /// <summary>
    /// SignalR connection status.
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// Client is disconnected.
        /// </summary>
        DISCONNECTED,

        /// <summary>
        /// Client is connected.
        /// </summary>
        CONNECTED,

        /// <summary>
        /// Client is reconnecting.
        /// </summary>
        RECONNECTING,
    }

    private void RegisterListener<T>(string name, Func<T, Task> func)
    {
        _connection.On(name, async (MessageMetadata metadata, T arg) =>
        {
            try
            {
                if (metadata.AerodromeICAO != _bayManager.AerodromeName ||
                    metadata.Server != Server)
                {
                    await SubscribeToAerodrome();
                    return;
                }

                LogMessageContent(name, arg);

                await func(arg);
            }
            catch (Exception ex)
            {
                Util.LogError(ex);
            }
        });
    }

    private void RegisterListener(string name, Func<Task> func)
    {
        _connection.On(name, async () =>
        {
            try
            {
                LogMessageContent(name);

                await func();
            }
            catch (Exception ex)
            {
                Util.LogError(ex);
            }
        });
    }

    private void LogMessageContent(string funcName, object? args = null, bool server = true)
    {
        var json = string.Empty;

        if (args is not null)
        {
            json = System.Text.Json.JsonSerializer.Serialize(args);
        }

        AddMessage($"{(server ? 's' : 'c')}-{funcName}: {json}");
    }

    private MessageMetadata GetMessageMetadata()
    {
        return new()
        {
            Server = Server,
            AerodromeICAO = _bayManager.AerodromeName,
        };
    }
}
