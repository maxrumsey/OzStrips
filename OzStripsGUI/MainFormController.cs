using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui

public class MainFormController
{
    private MainForm _mainForm;
    private readonly Timer _timer;
    private readonly BayManager _bayManager;
    private readonly SocketConn _socketConn;
    private readonly List<string> _aerodromes = [];
    private string _metar = string.Empty;
    private bool _readyForConnection;

    public static MainFormController? Instance { get; private set; }

    /// <summary>
    /// Gets whether or not a connection can be made to the server.
    /// </summary>
    public static bool ReadyForConnection => Instance?._readyForConnection ?? false;

    public MainFormController(MainForm form, bool readyToConnect)
    {
        Instance = this;
        _mainForm = form;
        _readyForConnection = readyToConnect;

        _timer = new()
        {
            Interval = 100,
        };

        _timer.Tick += UpdateTimer;
        _timer.Start();

        _bayManager = new(flp_main, AllToolStripMenuItem_Click);
        _socketConn = new(_bayManager, this);

        AddAerodrome("YBBN");
        AddAerodrome("YBCG");
        AddAerodrome("YBSU");
        AddAerodrome("YMEN");
        AddAerodrome("YMML");
        AddAerodrome("YPPH");
        AddAerodrome("YSCB");
        AddAerodrome("YSSY");

        if (_readyForConnection)
        {
            _socketConn.Connect();
        }

        _bayManager.BayRepository.Resize();
    }

    /// <summary>
    /// Gets or sets the list of aerodromes.
    /// </summary>
    /// <param name="value">The list of aerodromes.</param>
    public void SetAerodromeList(List<string> value)
    {
        _aerodromes.Clear();

        foreach (var item in ts_ad.DropDownItems.OfType<ToolStripItem>().ToArray())
        {
            if (item.Tag is null)
            {
                ts_ad.DropDownItems.Remove(item);
            }
        }

        foreach (var item in value)
        {
            AddAerodrome(item);
        }
    }

    /// <summary>
    /// Marks whether or not a connection is ready to be established to the server.
    /// </summary>
    /// <param name="readyForConnection">Whether or not a connection can be made.</param>
    public void MarkConnectionReadiness(bool readyForConnection)
    {
        try
        {
            _readyForConnection = readyForConnection;
            if (_readyForConnection)
            {
                _socketConn.Connect();
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Forces a resize event to redraw stripbays.
    /// </summary>
    public void ForceResize()
    {
        _bayManager.BayRepository.Resize(true);
        _bayManager.BayRepository.Resize(); // double resize to take into account addition / subtraction of scroll bars.
    }

    /// <summary>
    /// Sets the current aerodrome. Called by the GUI, and subsequently calls SetAerodrome() for various managers.
    /// </summary>
    /// <param name="name">The aerodrome name.</param>
    public void SetAerodrome(string name)
    {
        try
        {
            if (_bayManager != null)
            {
                _bayManager.SetAerodrome(name, _socketConn);
                _socketConn.SetAerodrome();
                lb_ad.Text = name;
                SetATISCode("Z");
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Moves to the next/previous aerodrome.
    /// </summary>
    /// <param name="direction">Direction to move in.</param>
    public void MoveLateralAerodrome(int direction)
    {
        var index = 0;
        for (var i = 0; i < _aerodromes.Count; i++)
        {
            if (_aerodromes[i] == _bayManager.AerodromeName)
            {
                index = i;
            }
        }

        index += direction;
        if (index >= _aerodromes.Count)
        {
            index = 0;
        }

        if (index < 0)
        {
            index = _aerodromes.Count - 1;
        }

        SetAerodrome(_aerodromes[index]);
    }

    /// <summary>
    /// Sets the METAR information. Called from SocketConn.
    /// </summary>
    /// <param name="metar">The METAR.</param>
    public void SetMetar(string metar)
    {
        if (metar != _metar)
        {
            _metar = metar;
            tt_metar.RemoveAll();
            tt_metar.SetToolTip(lb_ad, metar);
        }
    }

    /// <summary>
    /// Sets the connection status, green is connected, orange/red if not.
    /// </summary>
    public void SetConnStatus()
    {
        pl_stat.BackColor = _socketConn.Connected ? Color.Green : Color.OrangeRed;
    }

    /// <summary>
    /// Sets the selected track from vatSys.
    /// </summary>
    /// <param name="callsign">Selected Callsign.</param>
    /// <param name="ground">Whether or not the track is a ground track.</param>
    public void SetSelectedTrack(string? callsign, bool ground)
    {
        try
        {
            _bayManager.SetPickedCallsignFromVatsys(callsign, ground);
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }


    /// <summary>
    /// Disconnects from VATSIM.
    /// </summary>
    public void DisconnectVATSIM()
    {
        try
        {
            _bayManager.WipeStrips();
            _bayManager.StripRepository.Strips.Clear();
            _socketConn.Disconnect();
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Forces a strip.
    /// </summary>
    /// <param name="sender">Sending object.</param>
    /// <param name="e">EventArgs.</param>
    public void ForceStrip(object? sender, EventArgs? e)
    {
        _bayManager.ForceStrip(_socketConn);
    }

    /// <summary>
    /// Updates the flight data record.
    /// </summary>
    /// <param name="fdr">The flgiht data record.</param>
    /// <remarks>Triggered from Connector plugin.</remarks>
    public void UpdateFDR(FDP2.FDR fdr)
    {
        try
        {
            _bayManager.StripRepository.UpdateFDR(fdr, _bayManager, _socketConn);
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Handles a pilot disconnecting from vatSys.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    public void HandleDisconnect(Network.PilotUpdateEventArgs args)
    {
        try
        {
            if (!args.Removed || args.UpdatedPilot is null)
            {
                return;
            }

            var strip = _bayManager.StripRepository.GetStrip(args.UpdatedPilot.Callsign);
            if (strip is not null)
            {
                _bayManager.BayRepository.DeleteStrip(strip);
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Overrides keypress event to capture all keypresses.
    /// </summary>
    /// <param name="msg">Sender.</param>
    /// <param name="keyData">Key.</param>
    /// <returns>Handled.</returns>
    public bool? ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Up | Keys.Control))
        {
            _bayManager.PositionToNextBar(1);
            return true;
        }
        else if (keyData == (Keys.Down | Keys.Control))
        {
            _bayManager.PositionToNextBar(-1);
            return true;
        }
        else if (keyData == (Keys.X | Keys.Alt))
        {
            // If we didnt't delete a crossing bar, add one.
            if (!_bayManager.DeleteBarByParams("Runway", 3, "XXX CROSSING XXX"))
            {
                _bayManager.AddBar("Runway", 3, "XXX CROSSING XXX");
            }

            return true;
        }

        switch (keyData)
        {
            case Keys.Up:
                _bayManager.PositionKey(1);
                return true;
            case Keys.Down:
                _bayManager.PositionKey(-1);
                return true;
            case Keys.Space:
                _bayManager.QueueUp();
                return true;
            case Keys.Enter:
                _bayManager.SidTrigger();
                return true;
            case Keys.Tab:
                _bayManager.CockStrip();
                return true;
            case Keys.OemOpenBrackets:
                MoveLateralAerodrome(-1);
                return true;
            case Keys.OemCloseBrackets:
                MoveLateralAerodrome(1);
                return true;
            case Keys.Back:
                _bayManager.Inhibit();
                return true;
            case Keys.X:
                _bayManager.CrossStrip();
                return true;
            case Keys.F:
                _bayManager.FlipFlopStrip();
                return true;
            default:
                break;
        }

        _bayManager.ForceRerender();

        return null;
    }
}