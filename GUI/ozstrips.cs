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
using MaxRumsey.OzStripsPlugin.GUI;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using Microsoft.Win32;
using Newtonsoft.Json;
using vatsys;
using vatsys.Plugin;

namespace MaxRumsey.OzStripsPlugin.GUI;

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
    private readonly AerodromeManager _aerodromeManager;
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

        EnsureDpiAwareness();

        _aerodromeManager = new();
        _aerodromeManager.OpenGUI += OpenGUI;

        Network.Connected += Connected;
        Network.Disconnected += Disconnected;
        _ozStripsOpener = new(CustomToolStripMenuItemWindowType.Main, CustomToolStripMenuItemCategory.Windows, new ToolStripMenuItem("OzStrips"));
        _ozStripsOpener.Item.Click += OpenGUI;
        MMI.AddCustomMenuItem(_ozStripsOpener);
        MMI.SelectedTrackChanged += SelectedAirTrackChanged;
        MMI.SelectedGroundTrackChanged += SelectedGroundTrackChanged;
        Network.OnlinePilotsChanged += Network_OnlinePilotsChanged;

        AppDomain.CurrentDomain.UnhandledException += ErrorHandler;

        _aerodromeManager.Initialize();

        _ = CheckVersion();
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
            MMI.InvokeOnGUI(() => _gui.Controller.UpdateFDR(updated));
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

            if (AerodromeManager.InhibitVersionCheck)
            {
                return;
            }

            Errors.Add(new("A new version of the plugin is available."), "OzStrips");
        }
        catch
        {
        }
    }

    private static async Task SendCrash()
    {
        try
        {
            if (File.Exists(Helpers.GetFilesFolder() + "ozstrips_log.txt"))
            {
                var str = File.ReadAllText(Helpers.GetFilesFolder() + "ozstrips_log.txt");
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
            }
        }
        catch (Exception ex)
        {
            Errors.Add(ex, "OzStrips Error Reporter");
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
            MMI.InvokeOnGUI(() => _gui.Controller.HandleDisconnect(e));
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
            MMI.InvokeOnGUI(() => _gui.Controller.MarkConnectionReadiness(_readyForConnection));
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
            MMI.InvokeOnGUI(() => _gui.Controller.DisconnectVATSIM());
            _gui.Controller.MarkConnectionReadiness(_readyForConnection);
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
            _gui = new(_readyForConnection, _aerodromeManager);
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
            MMI.InvokeOnGUI(() => _gui.Controller.SetSelectedTrack(fdr.Callsign, false));
        }
        else if (_gui?.IsDisposed == false)
        {
            MMI.InvokeOnGUI(() => _gui.Controller.SetSelectedTrack(null, false));
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
            MMI.InvokeOnGUI(() => _gui.Controller.SetSelectedTrack(MMI.SelectedGroundTrack.GetPilot().Callsign, true));
        }
        else if (_gui?.IsDisposed == false)
        {
            MMI.InvokeOnGUI(() => _gui.Controller.SetSelectedTrack(null, true));
        }
    }

    // Thanks Eoin!
    private void EnsureDpiAwareness()
    {
        try
        {
            var vatSysPath = GetVatSysExecutablePath();
            if (vatSysPath == null)
            {
                return;
            }

            const string registryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";
            const string dpiValue = "DPIUNAWARE";

            using var key = Registry.CurrentUser.OpenSubKey(registryPath, writable: false);
            var existingValue = key?.GetValue(vatSysPath) as string;

            // If already set, exit early
            if (existingValue != null && existingValue.Contains(dpiValue))
            {
                return;
            }

            // Set the registry key
            using var writableKey = Registry.CurrentUser.OpenSubKey(registryPath, writable: true)
                ?? Registry.CurrentUser.CreateSubKey(registryPath);

            writableKey.SetValue(vatSysPath, dpiValue, RegistryValueKind.String);

            // Restart vatSys to apply the DPI setting
            RestartVatSys();
        }
        catch (Exception ex)
        {
            Util.LogError(ex, Name);
        }
    }

    private void RestartVatSys()
    {
        try
        {
            var vatSysPath = GetVatSysExecutablePath();
            if (vatSysPath != null)
            {
                System.Diagnostics.Process.Start(vatSysPath);
                Environment.Exit(0);
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex, Name);
        }
    }

    private static string? GetVatSysExecutablePath()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Sawbe\vatSys");
            var installPath = key?.GetValue("Path") as string;

            if (string.IsNullOrEmpty(installPath))
            {
                return null;
            }

            var exePath = Path.Combine(installPath, "bin", "vatSys.exe");
            return File.Exists(exePath) ? exePath : null;
        }
        catch
        {
            return null;
        }
    }
}
