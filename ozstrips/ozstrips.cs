﻿using System;
using System.ComponentModel.Composition;
using System.Net.Http;
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
        Network.Connected += Connected;
        Network.Disconnected += Disconnected;
        _ozStripsOpener = new(CustomToolStripMenuItemWindowType.Main, CustomToolStripMenuItemCategory.Windows, new ToolStripMenuItem("OzStrips"));
        _ozStripsOpener.Item.Click += OpenGUI;
        MMI.AddCustomMenuItem(_ozStripsOpener);
        MMI.SelectedTrackChanged += SelectedAirTrackChanged;
        MMI.SelectedGroundTrackChanged += SelectedGroundTrackChanged;
        Network.OnlinePilotsChanged += Network_OnlinePilotsChanged;
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
        ////Errors.Add(new Exception("mew"));
        ////System.Diagnostics.Process.Start("http://google.com");
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
                Errors.Add(new("Could not get the version information from the OzStrips server. Cannot validate if latest version."), "OzStrips Connector");
                return;
            }

            var version = JsonConvert.DeserializeObject<Version>(response);

            if (version is null)
            {
                Errors.Add(new("Could not load the version information for OzStrips."), "OzStrips Connector");
                return;
            }

            if (version.Major == _version.Major && version.Minor == _version.Minor && version.Build == _version.Build)
            {
                return;
            }

            Errors.Add(new("A new version of the plugin is available."), "OzStrips Connector");
        }
        catch
        {
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

        _gui.Show(Form.ActiveForm);
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
            MMI.InvokeOnGUI(() => _gui.SetSelectedTrack(fdr.Callsign));
        }
        else if (_gui?.IsDisposed == false)
        {
            MMI.InvokeOnGUI(() => _gui.SetSelectedTrack(null));
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
            MMI.InvokeOnGUI(() => _gui.SetSelectedTrack(MMI.SelectedGroundTrack.GetPilot().Callsign));
        }
        else if (_gui?.IsDisposed == false)
        {
            MMI.InvokeOnGUI(() => _gui.SetSelectedTrack(null));
        }
    }
}
