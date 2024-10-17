using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using vatsys;
using static MaxRumsey.OzStripsPlugin.Gui.SocketConn;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Handles communications by the sockets.
/// </summary>
public sealed class SocketConn : IDisposable
{
    private static bool MainFormValid => MainForm.MainFormInstance?.IsDisposed == false && MainForm.MainFormInstance.Visible;

    private readonly HubConnection _connection;
    private readonly BayManager _bayManager;
    private readonly bool _isDebug = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VisualStudioEdition"));

    // private bool _versionShown;
    private bool _freshClient = true;
    private Timer? _oneMinTimer;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="SocketConn"/> class.
    /// </summary>
    /// <param name="bayManager">The bay manager.</param>
    /// <param name="mainForm">The main form instance.</param>
    public SocketConn(BayManager bayManager, MainForm mainForm)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(OzStripsConfig.socketioaddr)
            .WithAutomaticReconnect()
            .Build();

        _bayManager = bayManager;

        _connection.Closed += async (error) => await ConnectionLost(error);

        _connection.Reconnected += async (connId) => await MarkConnected();

        _connection.Reconnecting += async (error) => await ConnectionLost(error, true);

        _connection.On<StripControllerDTO?>("StripUpdate", (StripControllerDTO? scDTO) =>
        {
            AddMessage("s:StripUpdate: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (mainForm.Visible && scDTO is not null)
            {
                mainForm.Invoke(() => _bayManager.StripRepository.UpdateFDR(scDTO, bayManager));
            }
        });

        _connection.On<List<StripControllerDTO>?>("StripCache", (List<StripControllerDTO>? scDTO) =>
        {
            AddMessage("s:StripCache: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (mainForm.Visible && scDTO is not null)
            {
                mainForm.Invoke(() => _bayManager.StripRepository.LoadCache(scDTO, bayManager, this));
            }
        });

        _connection.On("UpdateCache", [], async _ =>
        {
            AddMessage("s:UpdateCache: ");
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("RequestMetar");
            }

            if (!_freshClient)
            {
                SendCache();
            }
        });

        _connection.On<string?>("Atis", (string? code) => // not functional
        {
            if (MainFormValid && code is not null)
            {
                MainForm.MainFormInstance?.Invoke(() => MainForm.MainFormInstance.SetATISCode(code));
            }
        });

        _connection.On<string?>("Metar", (string? metar) => // not functional
        {
            if (MainFormValid && metar is not null)
            {
                 MainForm.MainFormInstance?.Invoke(() => MainForm.MainFormInstance.SetMetar(metar));
            }
        });

        _connection.On("BayUpdate", (BayDTO? bayDTO) =>
        {
            AddMessage("s:BayUpdate: " + System.Text.Json.JsonSerializer.Serialize(bayDTO));

            if (mainForm.Visible && bayDTO is not null)
            {
                mainForm.Invoke(() => bayManager.BayRepository.UpdateOrder(bayDTO));
            }
        });

        _connection.On<string?, RouteDTO[]?>("server:routes", (string? acid, RouteDTO[]? routes) => // not functional.
        {
            if (acid is null ||
                routes is null)
            {
                return;
            }

            try
            {
                AddMessage("s:routes: " + System.Text.Json.JsonSerializer.Serialize(routes));

                if (mainForm.Visible)
                {
                    var sc = _bayManager.StripRepository.GetController(acid);

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

        /*
        _io.OnAny((_, e) =>
        {
            var metaDTO = e.GetValue<MetadataDTO>(1);
            if (!string.IsNullOrEmpty(metaDTO.apiversion) && metaDTO.version != OzStripsConfig.version && !_versionShown)
            {
                _versionShown = true;
                if (mainForm.Visible)
                {
                    mainForm.Invoke(() => Util.ShowInfoBox("New Update Available: " + metaDTO.version));
                }
            }

            if (!string.IsNullOrEmpty(metaDTO.apiversion) && metaDTO.apiversion != OzStripsConfig.apiversion && mainForm.Visible)
            {
                mainForm.Invoke(() =>
                {
                    Util.ShowErrorBox("OzStrips incompatible with current API version! " + metaDTO.apiversion + " " + OzStripsConfig.apiversion + ".\nOzStrips will now close.");
                    mainForm.Close();
                    mainForm.Dispose();
                });
            }
        });

        

        */
    }

    /// <summary>
    /// Available server types.
    /// </summary>
    public enum Servers
    {
        /// <summary>
        /// Default connection.
        /// </summary>
        VATSIM,

        /// <summary>
        /// Sweatbox 1.
        /// </summary>
        SWEATBOX1,

        /// <summary>
        /// Sweatbox 2.
        /// </summary>
        SWEATBOX2,

        /// <summary>
        /// Sweatbox 3.
        /// </summary>
        SWEATBOX3,
    }

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
        StripControllerDTO scDTO = sc;
        AddMessage("c:sc_change: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

        if (string.IsNullOrEmpty(scDTO.acid))
        {
            return; // prevent bug
        }

        if (CanSendDTO)
        {
            _connection.InvokeAsync("StripChange", scDTO);
        }
    }

    /// <summary>
    /// Requests routes for a given sc.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public void RequestRoutes(Strip sc)
    {
        AddMessage("c:get_routes: " + sc.FDR.Callsign);
        if (_connection.State == HubConnectionState.Connected)
        {
            _connection.InvokeAsync("GetRoutes", sc.FDR.DepAirport, sc.FDR.DesAirport, sc.FDR.Callsign);
        }
    }

    /// <summary>
    /// Requests bay order data from server.
    /// </summary>
    public void ReadyForBayData()
    {
        AddMessage("c:req_bays:");
        _connection.InvokeAsync("RequestBays");
    }

    /// <summary>
    /// Syncs the deletion of a controller.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public void SyncDeletion(Strip sc)
    {
        SCDeletionDTO scDTO = sc;
        AddMessage("c:sc_delete: " + System.Text.Json.JsonSerializer.Serialize(scDTO));
        if (string.IsNullOrEmpty(scDTO.acid))
        {
            return; // prevent bug
        }

        if (CanSendDTO)
        {
            _connection.InvokeAsync("StripDelete", scDTO);
        }
    }

    /// <summary>
    /// Sync the bay to the socket.
    /// </summary>
    /// <param name="bay">The bay to sync.</param>
    public void SyncBay(Bay bay)
    {
        BayDTO bayDTO = bay;
        AddMessage("c:order_change: " + System.Text.Json.JsonSerializer.Serialize(bayDTO));

        if (CanSendDTO)
        {
            _connection.InvokeAsync("BayChange", bayDTO);
        }
    }

    /// <summary>
    /// Sets the aerodrome based on the bay manager.
    /// </summary>
    public void SetAerodrome()
    {
        _freshClient = true;
        _oneMinTimer = new()
        {
            AutoReset = false,
            Interval = 60000,
        };
        _oneMinTimer.Elapsed += ToggleFresh;
        _oneMinTimer.Start();
        if (_connection.State == HubConnectionState.Connected) // was is io connected.
        {
            _connection.InvokeAsync("SubscribeToAerodrome", _bayManager.AerodromeName, Network.Me.RealName, Server);
        }
    }

    /// <summary>
    /// Sets the server type.
    /// </summary>
    /// <param name="type">Server connection type.</param>
    public void SetServerType(Servers type)
    {
        Server = type;
        SetAerodrome();
    }

    /// <summary>
    /// Sends the cache to the server.
    /// </summary>
    public async void SendCache()
    {
        var cacheDTO = CreateCacheDTO();
        AddMessage("c:sc_cache: " + System.Text.Json.JsonSerializer.Serialize(cacheDTO));
        if (CanSendDTO)
        {
            await _connection.InvokeAsync("StripCache", cacheDTO);
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
    /// Starts a fifteen second timer, ensures FDRs have loaded in before requesting SCs from server.
    /// </summary>
    public async void Connect()
    {
        MMI.InvokeOnGUI(() => MainForm.MainFormInstance?.SetAerodrome(_bayManager.AerodromeName));
        try
        {
            AddMessage("c: Attempting connection " + OzStripsConfig.socketioaddr);
            while (_connection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _connection.StartAsync();
                }
                catch (Exception ex)
                {
                    Util.LogError(ex);
                }

                if (_connection.State == HubConnectionState.Connected)
                {
                    await MarkConnected();
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
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
        _oneMinTimer?.Dispose();
        await _connection.DisposeAsync();
        _disposed = true;
    }

    /// <summary>
    /// Creates the cache data transfer object.
    /// </summary>
    /// <returns>The cache data transfer object.</returns>
    private CacheDTO CreateCacheDTO()
    {
        return new() { strips = _bayManager.StripRepository.Controllers.ConvertAll(x => (StripControllerDTO)x), };
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

    private void AddMessage(string message)
    {
        lock (Messages)
        {
            Messages.Add(message);
        }
    }

    private async Task MarkConnected()
    {
        AddMessage("c: conn established");
        if (Network.IsConnected)
        {
            _freshClient = true;
            await _connection.SendAsync("SubscribeToAerodrome", _bayManager.AerodromeName, Network.Me.RealName, Server);

            Connected = true;
            if (MainFormValid)
            {
                MainForm.MainFormInstance?.Invoke(() => MainForm.MainFormInstance.SetConnStatus());
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

    private async Task ConnectionLost(Exception? error, bool reconnecting = false)
    {
        if (error is not null)
        {
            AddMessage("server conn lost - " + error.Message);
            if (!reconnecting)
            {
                await _connection.StartAsync();
            }
        }

        Connected = false;
        if (MainFormValid)
        {
            MainForm.MainFormInstance?.Invoke(() => MainForm.MainFormInstance.SetConnStatus());
        }
    }
}
