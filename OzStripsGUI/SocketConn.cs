using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using vatsys;
using static MaxRumsey.OzStripsPlugin.Gui.SocketConn;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Handles communications by the sockets.
/// </summary>
public sealed class SocketConn : IDisposable
{
    private readonly SocketIOClient.SocketIO _io;
    private readonly BayManager _bayManager;
    private readonly bool _isDebug = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VisualStudioEdition"));
    private readonly MainForm _mainForm;
    private bool _versionShown;
    private bool _freshClient = true;
    private bool _connectionMade;
    private Timer? _oneMinTimer;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="SocketConn"/> class.
    /// </summary>
    /// <param name="bayManager">The bay manager.</param>
    /// <param name="mainForm">The main form instance.</param>
    public SocketConn(BayManager bayManager, MainForm mainForm)
    {
        _mainForm = mainForm;
        _bayManager = bayManager;
        _io = new(OzStripsConfig.socketioaddr);
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

        _io.OnConnected += async (_, _) =>
        {
            AddMessage("c: conn established");
            if (Network.IsConnected)
            {
                _freshClient = true;
                _connectionMade = true;
                await _io.EmitAsync("client:aerodrome_subscribe", bayManager.AerodromeName, Network.Me.RealName, Server);
                Connected = true;
                if (mainForm.Visible)
                {
                    mainForm.Invoke(() => mainForm.SetConnStatus());
                }

                _bayManager.StripRepository.MarkAllStripsAsAwaitingRoutes();

                await Task.Delay(TimeSpan.FromSeconds(60));
                _freshClient = false;
            }
            else
            {
                AddMessage("c: disconnecting as vatsys connection was lost");
                await _io.DisconnectAsync();
            }
        };

        _io.OnDisconnected += (_, _) =>
        {
            AddMessage("c: conn lost");
            Connected = false;
            if (mainForm.Visible)
            {
                mainForm.Invoke(() => mainForm.SetConnStatus());
            }
        };

        _io.OnError += (_, e) =>
        {
            AddMessage("c: error" + e);
            Connected = false;
            if (mainForm.Visible)
            {
                mainForm.Invoke(() =>
                {
                    mainForm.SetConnStatus();
                    Util.LogError(new(e));
                });
            }
        };

        _io.OnReconnected += (_, _) =>
        {
            if (_io.Connected)
            {
                _io.EmitAsync("client:aerodrome_subscribe", bayManager.AerodromeName, Network.Me.RealName, Server);
            }

            Connected = true;
            if (mainForm.Visible)
            {
                mainForm.Invoke(() => mainForm.SetConnStatus());
            }
        };

        _io.OnReconnectError += (_, _) => AddMessage("recon error");

        _io.On("server:sc_change", sc =>
        {
            var scDTO = sc.GetValue<StripControllerDTO>();
            AddMessage("s:sc_change: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (mainForm.Visible)
            {
                mainForm.Invoke(() => _bayManager.StripRepository.UpdateFDR(scDTO, bayManager));
            }
        });

        _io.On("server:sc_cache", sc =>
        {
            var scDTO = sc.GetValue<CacheDTO>();
            AddMessage("s:sc_cache: " + System.Text.Json.JsonSerializer.Serialize(scDTO));

            if (mainForm.Visible && _freshClient)
            {
                mainForm.Invoke(() => _bayManager.StripRepository.LoadCache(scDTO, bayManager, this));
            }
        });

        _io.On("server:order_change", bdto =>
        {
            var bayDTO = bdto.GetValue<BayDTO>();
            AddMessage("s:order_change: " + System.Text.Json.JsonSerializer.Serialize(bayDTO));

            if (mainForm.Visible)
            {
                mainForm.Invoke(() => bayManager.BayRepository.UpdateOrder(bayDTO));
            }
        });

        _io.On("server:routes", (data) =>
        {
            try
            {
                var acid = data.GetValue<string>();
                var routes = data.GetValue<RouteDTO[]>(1);

                AddMessage("s:routes: " + System.Text.Json.JsonSerializer.Serialize(data));

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

        _io.On("server:metar", metarRaw =>
        {
            var metar = metarRaw.GetValue<string>();

            if (mainForm.Visible)
            {
                mainForm.Invoke(() => _mainForm.SetMetar(metar));
            }
        });

        _io.On("server:atis", codeRaw =>
        {
            var code = codeRaw.GetValue<string>();

            if (mainForm.Visible)
            {
                mainForm.Invoke(() => _mainForm.SetATISCode(code));
            }
        });

        _io.On("server:update_cache", _ =>
        {
            AddMessage("s:update_cache: ");
            if (_io.Connected)
            {
                _io.EmitAsync("client:request_metar");
            }

            if (!_freshClient)
            {
                SendCache();
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

            return _io.Connected && (Network.Me.IsRealATC || _isDebug);
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
            _io.EmitAsync("client:sc_change", scDTO);
        }
    }

    /// <summary>
    /// Requests routes for a given sc.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public void RequestRoutes(Strip sc)
    {
        AddMessage("c:get_routes: " + sc.FDR.Callsign);
        if (_io.Connected)
        {
            _io.EmitAsync("client:get_routes", sc.FDR.DepAirport, sc.FDR.DesAirport, sc.FDR.Callsign);
        }
    }

    /// <summary>
    /// Requests bay order data from server.
    /// </summary>
    public void ReadyForBayData()
    {
        AddMessage("c:req_bays:");
        _io.EmitAsync("client:req_bays");
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
            _io.EmitAsync("client:sc_delete", scDTO);
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
            _io.EmitAsync("client:order_change", bayDTO);
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
        if (_io.Connected)
        {
            _io.EmitAsync("client:aerodrome_subscribe", _bayManager.AerodromeName, Network.Me.RealName, Server);
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
            await _io.EmitAsync("client:sc_cache", cacheDTO);
        }
    }

    /// <summary>
    /// Disconnects from the server.
    /// </summary>
    public void Close()
    {
        _io.DisconnectAsync();
        _io.Dispose();
    }

    /// <summary>
    /// Starts a fifteen second timer, ensures FDRs have loaded in before requesting SCs from server.
    /// </summary>
    public async void Connect()
    {
        MMI.InvokeOnGUI(() => _mainForm.SetAerodrome(_bayManager.AerodromeName));
        try
        {
            AddMessage("c: Attempting connection " + OzStripsConfig.socketioaddr);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _io.ConnectAsync().ConfigureAwait(false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await Task.Delay(TimeSpan.FromSeconds(15));
            if (!_connectionMade && !_disposed)
            {
                Util.ShowErrorBox("OzStrips Connection Failed.\n" +
                    "This may be an issue with an outstanding Navigraph connection within vatSys.\n" +
                    "Go to Help -> Documentation -> FAQ for more information.");
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
        _io.DisconnectAsync();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _oneMinTimer?.Dispose();
        _io.Dispose();
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
        catch (Exception)
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
}
