using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using vatsys;
using static MaxRumsey.OzStripsPlugin.GUI.Shared.ConnectionMetadataDTO;
using static MaxRumsey.OzStripsPlugin.GUI.SocketConn;

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

    private bool _freshClient = true;
    private System.Timers.Timer? _oneMinTimer;
    private bool _versionShown;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="SocketConn"/> class.
    /// </summary>
    /// <param name="bayManager">The bay manager.</param>
    /// <param name="mainForm">The main form instance.</param>
    public SocketConn(BayManager bayManager, MainFormController mainForm)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(OzStripsConfig.socketioaddr + "OzStripsHub")
            .WithAutomaticReconnect()
            .Build();

        _bayManager = bayManager;
        _mainForm = mainForm;

        _connection.Closed += async (error) => await ConnectionLost(error);

        _connection.Reconnected += async (connId) => await MarkConnected();

        _connection.Reconnecting += async (error) => await ConnectionLost(error);

        _connection.On<StripDTO?>("StripUpdate", (StripDTO? scDTO) =>
        {
            AddMessage("s:StripUpdate: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (scDTO is not null)
            {
                InvokeOnGUI(() => _bayManager.StripRepository.UpdateStripData(scDTO, bayManager));
            }
        });

        _connection.On<List<StripDTO>?>("StripCache", (List<StripDTO>? scDTO) =>
        {
            AddMessage("s:StripCache: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (scDTO is not null && _freshClient)
            {
                InvokeOnGUI(() => _bayManager.StripRepository.LoadCache(scDTO, bayManager, this));
            }
        });

        _connection.On("UpdateCache", [], async _ =>
        {
            AddMessage("s:UpdateCache: ");
            if (!_freshClient)
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
            if (!_freshClient)
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
            if (!_freshClient)
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
                _bayManager.AerodromeState = state;
                InvokeOnGUI(() => AerodromeStateChanged?.Invoke(this, EventArgs.Empty));
            }
        });
    }

    /// <summary>
    /// An event called when the aerodrome state changes.
    /// </summary>
    public event EventHandler? AerodromeStateChanged;

    /// <summary>
    /// Gets the messages, used for debugging.
    /// </summary>
    public List<string> Messages { get; } = [];

    /// <summary>
    /// Gets or sets the current server type.
    /// </summary>
    public Servers Server { get; set; } = Servers.VATSIM;

    /// <summary>
    /// Gets or sets a value indicating whether the client is connected.
    /// </summary>
    public bool Connected { get; set; }

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
    /// Requests bay order data from server.
    /// </summary>
    /// <param name="force">Whether or not to force fetching of bay data.</param>
    public void ReadyForBayData(bool force = false)
    {
        if ((_freshClient || force) && Connected)
        {
            AddMessage("c:RequestBays:");
            _connection.InvokeAsync("RequestBays");
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
    public async Task SubscribeToAerodrome()
    {
        _freshClient = true;
        _oneMinTimer = new()
        {
            AutoReset = false,
            Interval = 60000,
        };
        _oneMinTimer.Elapsed += ToggleFresh;
        if (_connection.State == HubConnectionState.Connected) // was is io connected.
        {
            var connmetadata = new ConnectionMetadataDTO()
            {
                Version = OzStripsConfig.version,
                APIVersion = OzStripsConfig.apiversion,
                Server = Server,
                AerodromeName = _bayManager.AerodromeName,
                Callsign = Network.Me.Callsign,
            };

            await _connection.InvokeAsync("ProvideVersion", OzStripsConfig.version);
            await _connection.InvokeAsync("SubscribeToAerodrome", connmetadata);
            _oneMinTimer.Start();
        }
    }

    /// <summary>
    /// Sets the server type.
    /// </summary>
    /// <param name="type">Server connection type.</param>
    public async void SetServerType(Servers type)
    {
        Server = type;

        if (!CanConnectToCurrentServer())
        {
            return;
        }

        await SubscribeToAerodrome();
        if (_connection.State == HubConnectionState.Disconnected && MainFormController.ReadyForConnection)
        {
            Connect();
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
        MMI.InvokeOnGUI(() => _mainForm.SetAerodrome(_bayManager.AerodromeName));

        if (!CanConnectToCurrentServer())
        {
            return;
        }

        try
        {
            AddMessage("c: Attempting connection " + OzStripsConfig.socketioaddr);
            while (!_isDisposed && _connection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _connection.StartAsync();
                }
                catch (ObjectDisposedException)
                {
                }
                catch (Exception ex)
                {
                    Errors.Add(ex, "OzStrips - Server Connection Failed");
                }

                if (_connection.State == HubConnectionState.Connected)
                {
                    await MarkConnected();
                }
                else
                {
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
        _oneMinTimer?.Dispose();
        await _connection.DisposeAsync();
    }

    /// <summary>
    /// Creates the cache data transfer object.
    /// </summary>
    /// <returns>The cache data transfer object.</returns>
    private CacheDTO CreateCacheDTO()
    {
        return new() { strips = _bayManager.StripRepository.Strips.ConvertAll(x => (StripDTO)x), };
    }

    private void ToggleFresh(object sender, ElapsedEventArgs e)
    {
        try
        {
            _freshClient = false;
        }
        catch
        {
        }
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

    private async Task MarkConnected()
    {
        AddMessage("c: conn established");
        if (Network.IsConnected)
        {
            Connected = true;
            await SubscribeToAerodrome();

            if (MainFormValid)
            {
                _mainForm.Invoke(() => _mainForm.SetConnStatus());
            }

            _bayManager.StripRepository.MarkAllStripsAsAwaitingRoutes();

            await Task.Delay(TimeSpan.FromSeconds(60));
            _freshClient = false;
        }
        else
        {
            AddMessage("c: disconnecting as vatsys connection was lost");
            await _connection.StopAsync();
        }
    }

    private async Task ConnectionLost(Exception? error)
    {
        if (_isDisposed)
        {
            return;
        }

        if (Connected)
        {
            // prevent spamming of this func.
            Connected = false;
            _mainForm.Invoke(() => _mainForm.SetConnStatus());

            // Delete circuit bay, once.
            MMI.InvokeOnGUI(() => _mainForm.SetAerodrome(_bayManager.AerodromeName));
        }

        // This exists on the off chance we are connected but main form is not valid.
        Connected = false;

        if (error is not null)
        {
            AddMessage("server conn lost - " + error.Message);
        }

        // Don't try to connect if we are not connected to the network.
        if (_connection.State == HubConnectionState.Disconnected && Network.IsConnected)
        {
            await Connect();
        }
    }
}
