using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui;
using Newtonsoft.Json;
using vatsys;
using vatsys.Plugin;

namespace MaxRumsey.OzStripsPlugin;

/// <summary>
/// The main VatSys plugin implementation.
/// </summary>
[Export(typeof(IPlugin))]
public sealed class OzStrips : IPlugin, IDisposable
{
    private const string _versionUrl = "https://raw.githubusercontent.com/maxrumsey/OzStrips/master/Version.json";
    private static readonly HttpClient _httpClient = new();
    private static readonly Version _version = new(OzStripsConfig.version);
    private readonly CustomToolStripMenuItem _ozStripsOpener;
    private MainForm? _gui;

    private System.Timers.Timer? _connectionTimer;
    private bool _readyForConnection;

    /// <summary>
    /// Initializes a new instance of the <see cref="OzStrips"/> class.
    /// </summary>
    public OzStrips()
    {
        try
        {
            _ = SendCrash();
        }
        catch (Exception ex)
        {
            Util.LogError(ex, "OzStrips Error Reporter");
        }

        try
        {
            SetAndCreateEnvVar();
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }

        Network.Connected += Connected;
        Network.Disconnected += Disconnected;
        _ozStripsOpener = new(CustomToolStripMenuItemWindowType.Main, CustomToolStripMenuItemCategory.Windows, new ToolStripMenuItem("OzStrips"));
        _ozStripsOpener.Item.Click += OpenGUI;
        MMI.AddCustomMenuItem(_ozStripsOpener);
        MMI.SelectedTrackChanged += SelectedAirTrackChanged;
        MMI.SelectedGroundTrackChanged += SelectedGroundTrackChanged;
        Network.OnlinePilotsChanged += Network_OnlinePilotsChanged;
        _ = CheckVersion();

        AppDomain.CurrentDomain.UnhandledException += ErrorHandler;
    }

    /// <summary>
    /// Gets the name of the plugin.
    /// </summary>
    public string Name => "OzStrips Connector";

    /// <summary>
    /// Happens when a FDR update happens.
    /// </summary>
    /// <param name="updated">The information about the update.</param>
    public void OnFDRUpdate(FDP2.FDR updated)
    {
        if (_gui?.IsHandleCreated == true)
        {
            MMI.InvokeOnGUI(() => _gui.UpdateFDR(updated));
        }
    }

    /// <summary>
    /// Updates the radar track.
    /// </summary>
    /// <param name="updated">The updated details about the radar track.</param>
    /// <remarks>
    /// Not needed for this plugin. But you could for instance, use the new position of the
    /// radar track or its change in state (cancelled, etc.) to do some processing.
    /// </remarks>
    public void OnRadarTrackUpdate(RDP.RadarTrack updated)
    {
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _httpClient.Dispose();
        _gui?.Dispose();
    }

    /// <summary>
    /// Checks the version of the plugin.
    /// If we are running or a old verison, or the version failed to load,
    /// it will add to VatSys error list and prompt the user to update.
    /// </summary>
    /// <returns>A task to monitor async operations.</returns>
    private static async Task CheckVersion()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_versionUrl).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(response))
            {
                Util.LogError(new("Could not get the version information from the OzStrips server. Cannot validate if latest version."));
                return;
            }

            var version = JsonConvert.DeserializeObject<Version>(response);

            if (version is null)
            {
                Util.LogError(new("Could not load the version information for OzStrips."));
                return;
            }

            if (version.Major == _version.Major && version.Minor == _version.Minor && version.Build == _version.Build)
            {
                return;
            }

            // Errors.Add(new("A new version of the plugin is available."), "OzStrips");
        }
        catch
        {
        }
    }

    private static async Task SendCrash()
    {
      if (File.Exists(Helpers.GetFilesFolder() + "ozstrips_log.txt"))
        {
            var str = File.ReadAllText(Helpers.GetFilesFolder() + "ozstrips_log.txt");
#pragma warning disable CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons
            if (str.ToLower(CultureInfo.InvariantCulture).Contains("ozstrips"))
            {
                var data = new Dictionary<string, string>
                {
                    { "error", str },
                };
                File.Delete(Helpers.GetFilesFolder() + "ozstrips_log.txt");
                var uri = (OzStripsConfig.socketioaddr + "/crash").Replace("//", "/").Replace(":/", "://");
                _ = await _httpClient.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json")).ConfigureAwait(false);
                }
#pragma warning restore CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons
        }
    }

    private static void SetAndCreateEnvVar()
    {
        var appdata_path = Util.SetAndReturnDLLVar();
        var assembly_folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        Directory.CreateDirectory(appdata_path);
        try
        {
            File.Copy(assembly_folder + @"\libSkiaSharp.adll", appdata_path + "libSkiaSharp.dll", true);
        }
        catch
        {
            Errors.Add(new("Failed to load an internal Ozstrips file. This may be because you have another vatSys window open, and in this case, can be disregarded."), "Ozstrips");
        }
    }

    private void Network_OnlinePilotsChanged(object sender, Network.PilotUpdateEventArgs e)
    {
        if (e.Removed && _gui?.IsDisposed == false)
        {
            MMI.InvokeOnGUI(() => _gui.HandleDisconnect(e));
        }
    }

    /// <summary>
    /// A callback if a connection is made to VATSIM.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">A empty event arguments.</param>
    private void Connected(object sender, EventArgs e)
    {
        _connectionTimer = new()
        {
            Interval = 10000,
            AutoReset = false,
        };
        _connectionTimer.Elapsed += HandleConnected;
        _connectionTimer.Start();
    }

    private void HandleConnected(object sender, ElapsedEventArgs e)
    {
        _readyForConnection = true;
        if (_gui?.IsHandleCreated == true)
        {
            MMI.InvokeOnGUI(() => _gui.MarkConnectionReadiness(_readyForConnection));
        }
    }

    /// <summary>
    /// A callback if a disconnection is made with VATSIM.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">A empty event arguments.</param>
    private void Disconnected(object sender, EventArgs e)
    {
        _readyForConnection = false;
        if (_connectionTimer?.Enabled == true)
        {
            _connectionTimer.Stop();
        }

        if (_gui?.IsHandleCreated == true)
        {
            MMI.InvokeOnGUI(() => _gui.DisconnectVATSIM());
            _gui.MarkConnectionReadiness(_readyForConnection);
        }
    }

    private void ErrorHandler(object sender, UnhandledExceptionEventArgs ex)
    {
        var error = ex.ExceptionObject as Exception;
        if (error is not null)
        {
            File.WriteAllText(Helpers.GetFilesFolder() + "ozstrips_log.txt", error.Message + "\n" + error.StackTrace);
        }
    }

    private void OpenGUI(object sender, EventArgs e)
    {
        MMI.InvokeOnGUI(OpenGUI);
    }

    private void OpenGUI()
    {
        if (_gui?.IsDisposed != false)
        {
            _gui = new(_readyForConnection);
        }
        else if (_gui.Visible)
        {
            if (_gui.WindowState == FormWindowState.Minimized)
            {
                _gui.WindowState = FormWindowState.Normal;
            }

            return;
        }

        var vatSysMainForm = Application.OpenForms["MainForm"];

        if (vatSysMainForm == null)
        {
            return;
        }

        _gui.Show(vatSysMainForm);
    }

    /// <summary>
    /// Fired on the last selected air track being sent.
    /// </summary>
    /// <param name="obj">Event obj.</param>
    /// <param name="args">Event Args.</param>
    private void SelectedAirTrackChanged(object obj, EventArgs args)
    {
        FDP2.FDR? fdr = null;
        if (MMI.SelectedTrack is not null)
        {
            fdr = MMI.SelectedTrack.GetFDR();
        }

        if (_gui?.IsDisposed == false && fdr is not null)
        {
            MMI.InvokeOnGUI(() => _gui.SetSelectedTrack(fdr.Callsign, false));
        }
        else if (_gui?.IsDisposed == false)
        {
            MMI.InvokeOnGUI(() => _gui.SetSelectedTrack(null, false));
        }
    }

    /// <summary>
    /// Fired on the last selected air track being sent.
    /// </summary>
    /// <param name="obj">Event obj.</param>
    /// <param name="args">Event Args.</param>
    private void SelectedGroundTrackChanged(object obj, EventArgs args)
    {
        if (_gui?.IsDisposed == false && MMI.SelectedGroundTrack is not null)
        {
            MMI.InvokeOnGUI(() => _gui.SetSelectedTrack(MMI.SelectedGroundTrack.GetPilot().Callsign, true));
        }
        else if (_gui?.IsDisposed == false)
        {
            MMI.InvokeOnGUI(() => _gui.SetSelectedTrack(null, true));
        }
    }
}
