using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
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
    public SocketConn(BayManager bayManager, MainForm mainForm)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(OzStripsConfig.socketioaddr + "OzStripsHub")
            .WithAutomaticReconnect()
            .Build();

        _bayManager = bayManager;

        _connection.Closed += async (error) => await ConnectionLost(error);

        _connection.Reconnected += async (connId) => await MarkConnected();

        _connection.Reconnecting += async (error) => await ConnectionLost(error);

        _connection.On<StripDTO?>("StripUpdate", (StripDTO? scDTO) =>
        {
            AddMessage("s:StripUpdate: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (mainForm.Visible && scDTO is not null)
            {
                mainForm.Invoke(() => _bayManager.StripRepository.UpdateStripData(scDTO, bayManager));
            }
        });

        _connection.On<List<StripDTO>?>("StripCache", (List<StripDTO>? scDTO) =>
        {
            AddMessage("s:StripCache: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (mainForm.Visible && scDTO is not null && _freshClient)
            {
                mainForm.Invoke(() => _bayManager.StripRepository.LoadCache(scDTO, bayManager, this));
            }
        });

        _connection.On("UpdateCache", [], async _ =>
        {
            AddMessage("s:UpdateCache: ");
            if (!_freshClient)
            {
                await SendCache();
            }
        });

        _connection.On<string?>("Atis", (string? code) =>
        {
            if (MainFormValid && code is not null)
            {
                MainForm.MainFormInstance?.Invoke(() => MainForm.MainFormInstance.SetATISCode(code));
            }
        });

        _connection.On("ActivateWorldFlightMode", () => _bayManager.WorldFlightMode = true);

        _connection.On<string?>("Metar", (string? metar) =>
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

                if (mainForm.Visible)
                {
                    var sc = _bayManager.StripRepository.GetController(key);

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
            if (MainFormValid && acid is not null)
            {
                MainForm.MainFormInstance?.Invoke(() => _bayManager.StripRepository.GetStripStatus(acid, this));
            }
        });

        _connection.On<string?>("VersionInfo", (string? appversion) => // not functional.
        {
            if (appversion is null)
            {
                return;
            }

            if (!_versionShown && appversion != OzStripsConfig.version)
            {
                _versionShown = true;
                if (mainForm.Visible)
                {
                    mainForm.Invoke(() => Util.ShowInfoBox("New Update Available: " + appversion));
                }
            }
        });

        _connection.On<string?>("Message", (string? message) =>
        {
            if (!mainForm.IsDisposed && message is not null)
            {
                mainForm.Invoke(() => Util.ShowWarnBox(message));
            }
        });
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

    private static bool MainFormValid => MainForm.MainFormInstance?.IsDisposed == false && MainForm.MainFormInstance.Visible;

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
    /// Sets the aerodrome based on the bay manager.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task SetAerodrome()
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

        await SetAerodrome();
        if (_connection.State == HubConnectionState.Disconnected && MainForm.ReadyForConnection is not null && MainForm.ReadyForConnection == true)
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
        MMI.InvokeOnGUI(() => MainForm.MainFormInstance?.SetAerodrome(_bayManager.AerodromeName));

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
            var result = Util.ShowQuestionBox("Connection to OzStrips main server detected while connected to the Sweatbox.\n\n" +
                "Would you like to go to Settings and set Sweatbox mode?");

            if (result == DialogResult.Yes)
            {
                MainForm.MainFormInstance?.ShowSettings(this, new());
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

    private async Task MarkConnected()
    {
        AddMessage("c: conn established");
        if (Network.IsConnected)
        {
            Connected = true;
            await SetAerodrome();

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

    private async Task ConnectionLost(Exception? error)
    {
        Connected = false;
        if (MainFormValid)
        {
            MainForm.MainFormInstance?.Invoke(() => MainForm.MainFormInstance.SetConnStatus());
        }

        if (error is not null)
        {
            AddMessage("server conn lost - " + error.Message);
        }

        if (_connection.State == HubConnectionState.Disconnected)
        {
            await Connect();
        }
    }
}
