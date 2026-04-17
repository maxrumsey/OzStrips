using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
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

        _connection.On<StripDTO?>("StripUpdate", (StripDTO? scDTO) =>
        {
            AddMessage("s:StripUpdate: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (scDTO is not null)
            {
                InvokeOnGUI(() => _bayManager.StripRepository.UpdateStripData(scDTO, bayManager));
            }
        });

        _connection.On<StripDTO[]>("StripCache", (StripDTO[] scDTO) =>
        {
            AddMessage("s:StripCache: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (scDTO is not null && FreshClient)
            {
                InvokeOnGUI(() => _bayManager.StripRepository.LoadCache(scDTO ?? [], bayManager, this));
            }
        });

        _connection.On("UpdateCache", [], async _ =>
        {
            AddMessage("s:UpdateCache: ");
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

            return Task.CompletedTask;
        });

        _connection.On("UpdateBays", [], async _ =>
        {
            AddMessage("s:UpdateBays: ");
            if (!FreshClient)
            {
                InvokeOnGUI(async () =>
                {
                    try
                    {
                        foreach (var bay in bayManager.BayRepository.Bays)
                        {
                            if (bay is not null)
                            {
                                SyncBay(bay);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.LogError(ex);
                    }
                });
            }

            return Task.CompletedTask;
        });

        _connection.On("SendCDM", [], async (_) =>
        {
            AddMessage("s:SendCDM: ");
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

            return Task.CompletedTask;
        });

        _connection.On<string?>("Atis", (string? code) =>
        {
            if (code is not null)
            {
                InvokeOnGUI(() => mainForm.SetATISCode(code));
            }
        });

        _connection.On<string?>("Metar", (string? metar) =>
        {
            if (metar is not null)
            {
                InvokeOnGUI(() => mainForm.SetMetar(metar));
            }
        });

        _connection.On("BayUpdate", (BayDTO? bayDTO) =>
        {
            AddMessage("s:BayUpdate: " + System.Text.Json.JsonSerializer.Serialize(bayDTO));

            if (bayDTO is not null)
            {
                InvokeOnGUI(() => bayManager.BayRepository.UpdateOrder(bayDTO));
            }
        });

        _connection.On<StripKey?, RouteDTO[]?>("Routes", (StripKey? key, RouteDTO[]? routes) => // not functional.
        {
            if (key is null ||
                routes is null ||
                routes.Length == 0)
            {
                return;
            }

            try
            {
                AddMessage("s:Routes: " + System.Text.Json.JsonSerializer.Serialize(routes));

                if (MainFormValid)
                {
                    var sc = _bayManager.StripRepository.GetStrip(key);

                    if (sc is not null)
                    {
                        sc.ValidRoutes = routes;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogError(ex);
            }
        });

        _connection.On<string?>("GetStripStatus", (string? acid) =>
        {
            if (acid is not null)
            {
                InvokeOnGUI(() => _bayManager.StripRepository.GetStripStatus(acid, this));
            }
        });

        _connection.On<string?>("VersionInfo", (string? appversion) => // not functional.
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

        _connection.On<string?>("Message", (string? message) =>
        {
            if (message is not null)
            {
                InvokeOnGUI(() => Util.ShowWarnBox(message));
            }
        });

        _connection.On<AerodromeState>("AerodromeStateUpdate", (AerodromeState state) =>
        {
            if (state.AerodromeCode == _bayManager.AerodromeName && state.AerodromeCode.Length > 0)
            {
                AddMessage("s:State: " + System.Text.Json.JsonSerializer.Serialize(state));
                _bayManager.AerodromeState = state;
                InvokeOnGUI(() => AerodromeStateChanged?.Invoke(this, EventArgs.Empty));
            }
        });

        _connection.On<string[]?>("NewPDC", (string[]? pdcs) =>
        {
            AddMessage($"s:NewPDC: {JsonSerializer.Serialize(pdcs)}");
            if (pdcs is not null && pdcs.Length > 0)
            {
                InvokeOnGUI(() => NewPDCsReceived?.Invoke(this, pdcs));
            }
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
            if (!(Network.Me.IsRealATC || _isDebug))
            {
                AddMessage("c: DTO Rejected!");
            }

            return _connection.State == HubConnectionState.Connected && (Network.Me.IsRealATC || _isDebug);
        }
    }

    /// <summary>
    /// Syncs the strip controller.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public void SyncSC(Strip sc)
    {
        StripDTO scDTO = sc;
        AddMessage("c:sc_change: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

        if (CanSendDTO)
        {
            _connection.InvokeAsync("StripChange", scDTO);
        }
    }

    /// <summary>
    /// Sends strip status to the server.
    /// </summary>
    /// <param name="strip">Strip object.</param>
    /// <param name="acid">Strip callsign.</param>
    public void SendStripStatus(Strip? strip, string acid)
    {
        if (CanSendDTO && strip is not null)
        {
            _connection.InvokeAsync("StripStatus", (StripDTO)strip, acid);
        }
        else if (CanSendDTO)
        {
            _connection.InvokeAsync("StripStatus", null, acid);
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
            _connection.InvokeAsync("UplinkCDMAircraft", list);
        }
    }

    /// <summary>
    /// Requests routes for a given sc.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public void RequestRoutes(Strip sc)
    {
        AddMessage("c:GetRoutes: " + sc.FDR.Callsign);
        if (_connection.State == HubConnectionState.Connected)
        {
            _connection.InvokeAsync("GetRoutes", sc.StripKey);
        }
    }

    /// <summary>
    /// Requests strip data from the server.
    /// </summary>
    /// <param name="strip">Strip to fetch.</param>
    public void RequestStrip(Strip strip)
    {
        AddMessage("c:RequestStrip: " + strip.FDR.Callsign);

        if (_connection.State == HubConnectionState.Connected)
        {
            _connection.InvokeAsync("RequestStrip", strip.StripKey);
        }
    }

    /// <summary>
    /// Sends a Hoppies PDC to the server.
    /// </summary>
    /// <param name="strip">Strip.</param>
    /// <param name="text">PDC text.</param>
    public async Task SendPDC(Strip strip, string text)
    {
        AddMessage("c:SendPDC: " + strip.FDR.Callsign);

        if (CanSendDTO)
        {
            await _connection.InvokeAsync("SendPDC", (StripDTO)strip, text);
        }
    }

    /// <summary>
    /// Syncs the deletion of a controller.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public void SyncDeletion(Strip sc)
    {
        AddMessage("c:StripDelete: " + sc.FDR.Callsign);

        if (CanSendDTO)
        {
            _connection.InvokeAsync("StripDelete", sc.FDR.Callsign);
        }
    }

    /// <summary>
    /// Sync the bay to the socket.
    /// </summary>
    /// <param name="bay">The bay to sync.</param>
    public void SyncBay(Bay bay)
    {
        BayDTO bayDTO = bay;
        AddMessage("c:BayChange: " + System.Text.Json.JsonSerializer.Serialize(bayDTO));

        if (CanSendDTO)
        {
            _connection.InvokeAsync("BayChange", bayDTO);
        }
    }

    /// <summary>
    /// Sends circuit activity status to the server.
    /// </summary>
    /// <param name="status">Circuit enabled.</param>
    public void RequestCircuit(bool status)
    {
        AddMessage("c:RequestCircuit: " + status);
        if (CanSendDTO)
        {
            _connection.InvokeAsync("UpdateCircuitMode", status);
        }
    }

    /// <summary>
    /// Sends coordinator bay activity status to the server.
    /// </summary>
    /// <param name="status">Circuit enabled.</param>
    public void RequestCoordinator(bool status)
    {
        AddMessage("c:RequestCircuit: " + status);
        if (CanSendDTO)
        {
            _connection.InvokeAsync("UpdateCoordinatorMode", status);
        }
    }

    /// <summary>
    /// Sends new CDM parameters to the server.
    /// </summary>
    /// <param name="param">CDM Parameters.</param>
    public void SendCDMParameters(CDMParameters param)
    {
        AddMessage("c:ChangeCDMParameters: " + param);
        if (CanSendDTO)
        {
            _connection.InvokeAsync("ChangeCDMParameters", param);
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
            var response = await _connection.InvokeAsync<AerodromeSubscriptionResponse>("SubscribeToAerodrome", connmetadata);

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
                    InvokeOnGUI(() => _bayManager.BayRepository.UpdateOrder(bay));
                }
            });


            _aerodromeSubscriptionRegistered = DateTime.Now;
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
            if (_connection.State == HubConnectionState.Disconnected && MainFormController.ReadyForConnection)
            {
                await Connect();
            }
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
        AddMessage("c:StripCache: " + System.Text.Json.JsonSerializer.Serialize(cacheDTO));
        if (CanSendDTO)
        {
            await _connection.InvokeAsync("StripCache", cacheDTO);
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
            await _connection.SendAsync("UplinkCDMAircraft", cdmDTOs);
        }
    }

    /// <summary>
    /// Disconnects from the server.
    /// </summary>
    public void Close()
    {
        _connection.StopAsync();
    }

    /// <summary>
    /// Creates a connection to the server.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task Connect()
    {
        if (!CanConnectToCurrentServer())
        {
            return;
        }

        try
        {
            AddMessage("c: Attempting connection " + OzStripsConfig.socketioaddr);
            while (!_isDisposed)
            {
                try
                {
                    await _connection.StartAsync();
                    await ConnectionStateChanged(ConnectionState.CONNECTED);
                }
                catch (ObjectDisposedException)
                {
                }
                catch (Exception ex)
                {
                    Errors.Add(ex, "OzStrips - Server Connection Failed");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
        }
        catch (ObjectDisposedException)
        {
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
            // TODO: make sure settings isn't already open.
            var result = Util.ShowQuestionBox("Connection to OzStrips main server detected while connected to the Sweatbox.\n\n" +
                "Would you like to go to Settings and set Sweatbox mode?");

            if (result == DialogResult.Yes)
            {
                _mainForm.ShowSettings(this, new());
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
    }

    private async Task ConnectionStateChanged(ConnectionState newState, Exception? ex = null)
    {
        if (_isDisposed || !MainFormValid)
        {
            return;
        }

        if (ex is not null)
        {
            AddMessage($"Connection error: {ex.Message}");
        }

        if (newState == ConnectionState.CONNECTED && State is ConnectionState.RECONNECTING or ConnectionState.DISCONNECTED)
        {
            _aerodromeSubscriptionRegistered = DateTime.Now;
            State = ConnectionState.CONNECTED;

            if (!Network.IsConnected)
            {
                AddMessage("Connected, but network is not. Rejecting.");
                Disconnect();
                return;
            }

            AddMessage("Connected");

            _mainForm.Invoke(_mainForm.SetConnStatus);
            _bayManager.StripRepository.MarkAllStripsAsAwaitingRoutes();
        }
        else if (newState == ConnectionState.DISCONNECTED && State is ConnectionState.RECONNECTING or ConnectionState.CONNECTED)
        {
            _aerodromeSubscriptionRegistered = null;
            State = ConnectionState.DISCONNECTED;

            _mainForm.Invoke(_mainForm.SetConnStatus);

            if (Network.IsConnected)
            {
                await Connect();
            }
        }
        else if (newState == ConnectionState.RECONNECTING && State is ConnectionState.DISCONNECTED or ConnectionState.CONNECTED)
        {
            if (!Network.IsConnected)
            {
                AddMessage("Reconnecting, but network is not connected. Rejecting.");
                Disconnect();
                return;
            }

            _mainForm.Invoke(_mainForm.SetConnStatus);
        }
    }

    public enum ConnectionState
    {
        DISCONNECTED,
        CONNECTED,
        RECONNECTING,
    }
}
