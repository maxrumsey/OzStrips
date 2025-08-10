using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.Controls;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui;

public class MainFormController : IDisposable
{
    private FormWindowState _lastState = FormWindowState.Minimized;
    private bool _postresizechecked = true;

    private MainForm _mainForm;
    private readonly Timer _timer;
    private BayManager _bayManager;
    private SocketConn _socketConn;
    private readonly List<string> _aerodromes = [];
    private string _metar = string.Empty;
    private bool _readyForConnection;

    public static MainFormController? Instance { get; private set; }

    /// <summary>
    /// Gets whether or not a connection can be made to the server.
    /// </summary>
    public static bool ReadyForConnection => Instance?._readyForConnection ?? false;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public MainFormController(MainForm form, bool readyToConnect)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
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

    }

    public void Initialize()
    {
        AddAerodrome("YBBN");
        AddAerodrome("YBCG");
        AddAerodrome("YBSU");
        AddAerodrome("YMEN");
        AddAerodrome("YMML");
        AddAerodrome("YPPH");
        AddAerodrome("YSCB");
        AddAerodrome("YSSY");

        _bayManager = new(_mainForm.MainFLP, AllToolStripMenuItem_Click);
        _socketConn = new(_bayManager, this);

        if (_readyForConnection)
        {
            _socketConn.Connect();
        }

        _bayManager.BayRepository.Resize();
    }

    /// <summary>
    /// Gets or sets the custom list of aerodromes.
    /// </summary>
    /// <param name="value">The list of aerodromes.</param>
    public void SetcustomAerodromeList(List<string> value)
    {
        _mainForm.AerodromeManager.ManuallySetAerodromes = value;
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
                _mainForm.AerodromeLabel.Text = name;
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
            _mainForm.MetarToolTip.RemoveAll();
            _mainForm.MetarToolTip.SetToolTip(_mainForm.AerodromeLabel, metar);
        }
    }

    /// <summary>
    /// Sets the connection status, green is connected, orange/red if not.
    /// </summary>
    public void SetConnStatus()
    {
        _mainForm.StatusPanel.BackColor = _socketConn.Connected ? Color.Green : Color.OrangeRed;
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

    /// <summary>
    /// Sets the ATIS code. Called from SocketConn.
    /// </summary>
    /// <param name="code">The ATIS code.</param>
    public void SetATISCode(string code)
    {
        _mainForm.ATISLabel.Text = code;
    }

    private void UpdateTimer(object sender, EventArgs e)
    {
        if (!_mainForm.Visible)
        {
            return;
        }

        if (!_postresizechecked)
        {
            _postresizechecked = true;
            _bayManager.BayRepository.Resize();
        }

        if (!_mainForm.IsDisposed)
        {
            _mainForm.Invoke(() =>
            {
                _mainForm.TimerTextBox.Text = DateTime.UtcNow.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                _bayManager.ForceRerender();
            });
        }
    }

    internal void MainFormSizeChanged(object sender, EventArgs e)
    {
        _postresizechecked = false;
        _bayManager?.BayRepository.Resize();
        _mainForm.SetControlBarScrollBar();
    }

    internal void AddAerodrome(string name)
    {
        _aerodromes.Add(name);

        var toolStripMenuItem = new ToolStripMenuItem
        {
            Text = name,
        };
        toolStripMenuItem.Click += (sender, e) => SetAerodrome(name);
        _mainForm.AerodromeListToolStrip.DropDownItems.Add(toolStripMenuItem);
    }

    internal void Bt_inhibit_Click(object sender, EventArgs e)
    {
        _bayManager.Inhibit();
    }

    internal void ACDToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.BayRepository.SetLayout(ACDToolStripMenuItem_Click);
        _bayManager.BayRepository.WipeBays();
        _bayManager.BayRepository.BayNum = 3;
        _ = new Bay([StripBay.BAY_PREA], _bayManager, _socketConn, "Preactive", 0);
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 1);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 2);
        _bayManager.BayRepository.Resize();
        _bayManager.BayRepository.ReloadStrips(_socketConn);
    }

    internal void SMCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.BayRepository.SetLayout(SMCToolStripMenuItem_Click);
        _bayManager.BayRepository.WipeBays();
        _bayManager.BayRepository.BayNum = 5;
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 0);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 0);
        _ = new Bay([StripBay.BAY_TAXI], _bayManager, _socketConn, "Taxi", 1);
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 2);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 2);
        _bayManager.BayRepository.Resize();
        _bayManager.BayRepository.ReloadStrips(_socketConn);
    }

    internal void SMCACDToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.BayRepository.SetLayout(SMCACDToolStripMenuItem_Click);
        _bayManager.BayRepository.WipeBays();
        _bayManager.BayRepository.BayNum = 6;
        _ = new Bay([StripBay.BAY_PREA], _bayManager, _socketConn, "Preactive", 0);
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 0);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 0);
        _ = new Bay([StripBay.BAY_TAXI], _bayManager, _socketConn, "Taxi", 1);
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 2);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 2);
        _bayManager.BayRepository.Resize();
        _bayManager.BayRepository.ReloadStrips(_socketConn);
    }

    internal void ADCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.BayRepository.SetLayout(ADCToolStripMenuItem_Click);
        _bayManager.BayRepository.WipeBays();
        _bayManager.BayRepository.BayNum = 4;
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 0);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 1);
        _ = new Bay([StripBay.BAY_OUT], _bayManager, _socketConn, "Departed", 2);
        _ = new Bay([StripBay.BAY_ARRIVAL], _bayManager, _socketConn, "Arrivals", 2);
        _bayManager.BayRepository.Resize();
        _bayManager.BayRepository.ReloadStrips(_socketConn);
    }

    internal void AllToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.BayRepository.SetLayout(AllToolStripMenuItem_Click);
        _bayManager.BayRepository.WipeBays();
        _bayManager.BayRepository.BayNum = 8;
        _ = new Bay([StripBay.BAY_PREA], _bayManager, _socketConn, "Preactive", 0);
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 0);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 1);
        _ = new Bay([StripBay.BAY_TAXI], _bayManager, _socketConn, "Taxi", 1);
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 1);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 2);
        _ = new Bay([StripBay.BAY_OUT], _bayManager, _socketConn, "Departed", 2);
        _ = new Bay([StripBay.BAY_ARRIVAL], _bayManager, _socketConn, "Arrivals", 2);
        _bayManager.BayRepository.Resize();
        _bayManager.BayRepository.ReloadStrips(_socketConn);
    }

    internal void ADCSMCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.BayRepository.SetLayout(ADCSMCToolStripMenuItem_Click);
        _bayManager.BayRepository.WipeBays();
        _bayManager.BayRepository.BayNum = 7;
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 0);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 0);
        _ = new Bay([StripBay.BAY_TAXI], _bayManager, _socketConn, "Taxi", 1);
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 1);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 2);
        _ = new Bay([StripBay.BAY_OUT], _bayManager, _socketConn, "Departed", 2);
        _ = new Bay([StripBay.BAY_ARRIVAL], _bayManager, _socketConn, "Arrivals", 2);
        _bayManager.BayRepository.Resize();
        _bayManager.BayRepository.ReloadStrips(_socketConn);
    }

    public void Bt_cross_Click(object sender, EventArgs e)
    {
        _bayManager.CrossStrip();
    }

    // socket.io log
    public void ShowMessageList_Click(object sender, EventArgs e)
    {
        var modalChild = new MsgListDebug(_socketConn);
        var bm = new BaseModal(modalChild, "Msg List");
        bm.Show(_mainForm);
    }

    public void MainForm_Load(object sender, EventArgs e)
    {
        SetConnStatus();
        _mainForm.SetSmartResizeCheckBox();
    }

    public void AerodromeSelectorKeyDown(object sender, KeyPressEventArgs e)
    {
        if (_bayManager != null && e.KeyChar == Convert.ToChar(Keys.Enter, CultureInfo.InvariantCulture))
        {
            SetAerodrome(_mainForm.EnteredAerodrome.ToUpper(CultureInfo.InvariantCulture));

            e.Handled = true;
        }
    }

    public void BarCreatorClick(object sender, EventArgs e)
    {
        var modalChild = new BarCreator(_bayManager);
        var bm = new BaseModal(modalChild, "Add Bar");
        bm.ReturnEvent += modalChild.ModalReturned;
        bm.Show(_mainForm);
    }

    public void MainForm_Resize(object sender, EventArgs e)
    {
        if (_mainForm.WindowState != _lastState)
        {
            _lastState = _mainForm.WindowState;
            _postresizechecked = false;
            _bayManager?.BayRepository.Resize();
            _mainForm.SetControlBarScrollBar();
        }
    }

    public void SetSmartResizeColumnMode(int cols)
    {
        Util.SetEnvVar("SmartResize", cols);
        _mainForm.SetSmartResizeCheckBox();
        _bayManager.BayRepository.ReloadStrips(_socketConn);
    }

    public void FlipFlopStrip(object sender, EventArgs e)
    {
        _bayManager.FlipFlopStrip();
    }

    /// <summary>
    /// Opens the settings window.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Args.</param>
    public void ShowSettings(object sender, EventArgs e)
    {
        var modalChild = new SettingsWindowControl(_socketConn, _aerodromes);
        var bm = new BaseModal(modalChild, "OzStrips Settings");
        bm.ReturnEvent += modalChild.ModalReturned;
        bm.Show(_mainForm);
    }

    public void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        _socketConn.Close();
        _socketConn.Dispose();
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public object Invoke(Action act)
    {
        return _mainForm.Invoke(act);
    }

    public bool Visible
    {
        get
        {
            return _mainForm.Visible;
        }
    }

    public bool IsDisposed
    {
        get
        {
            return _mainForm.IsDisposed;
        }
    }

    public IntPtr Handle
    {
        get
        {
            return _mainForm.Handle;
        }
    }
}
