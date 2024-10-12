using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.Controls;
using MaxRumsey.OzStripsPlugin.Gui.Properties;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// The main application form.
/// </summary>
public partial class MainForm : Form
{
    private readonly Timer _timer;
    private readonly BayManager _bayManager;
    private readonly SocketConn _socketConn;
    private readonly List<string> _aerodromes = new List<string>();
    private string _metar = string.Empty;

    private FormWindowState _lastState = FormWindowState.Minimized;

    private bool _readyForConnection;
    private bool _postresizechecked = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    /// <param name="readyForConnection">Whether the client can establish a server connection.</param>
    public MainForm(bool readyForConnection)
    {
        _readyForConnection = readyForConnection;
        MainFormInstance = this;

        InitializeComponent();
        _timer = new()
        {
            Interval = 100,
        };

        Util.SetAndReturnDLLVar();

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

        if (IsDebug)
        {
            var toolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Manual Comm",
            };
            toolStripMenuItem.Click += (_, _) => OpenManDebug();
            debugToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
        }

        SetStripSizeCheckBox();
    }

    /// <summary>
    /// Gets a singleton version of the main form.
    /// </summary>
    public static MainForm? MainFormInstance { get; private set; }

    /// <summary>
    /// Gets a value indicating whether we are currently in a Visual Studio debug session.
    /// </summary>
    public static bool IsDebug =>
        !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VisualStudioEdition"));

    /// <inheritdoc/>
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
            return cp;
        }
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
    /// Sets the ATIS code. Called from SocketConn.
    /// </summary>
    /// <param name="code">The ATIS code.</param>
    public void SetATISCode(string code)
    {
        lb_atis.Text = code;
    }

    /// <summary>
    /// Opens the Manual MSG debug menu.
    /// </summary>
    public void OpenManDebug()
    {
        var modalChild = new ManualMsgDebug(_bayManager);
        var bm = new BaseModal(modalChild, "Manual Message Editor");
        bm.Show(this);
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
    /// <param name="fdr">Selected FDR.</param>
    public void SetSelectedTrack(string? fdr)
    {
        try
        {
            _bayManager.PickedCallsign = fdr;
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
            _bayManager.StripRepository.Controllers.Clear();
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

            var strip = _bayManager.StripRepository.GetController(args.UpdatedPilot.Callsign);
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
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
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
            _bayManager.AddBar("Runway", 3, "XXX CROSSING XXX");
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
            default:
                break;
        }

        _bayManager.ForceRerender();

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /*
     * GUI Below
     */

    private void UpdateTimer(object sender, EventArgs e)
    {
        if (!Visible)
        {
            return;
        }

        if (!_postresizechecked)
        {
            _postresizechecked = true;
            _bayManager.BayRepository.Resize();
        }

        if (!IsDisposed)
        {
            Invoke(() =>
            {
                tb_Time.Text = DateTime.UtcNow.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                _bayManager.ForceRerender();
            });
        }
    }

    private void MainFormSizeChanged(object sender, EventArgs e)
    {
        _postresizechecked = false;
        if (_bayManager is not null)
        {
            _bayManager.BayRepository.Resize();
        }
    }

    private void AddAerodrome(string name)
    {
        _aerodromes.Add(name);

        var toolStripMenuItem = new ToolStripMenuItem
        {
            Text = name,
        };
        toolStripMenuItem.Click += (sender, e) => SetAerodrome(name);
        ts_ad.DropDownItems.Add(toolStripMenuItem);
    }

    private void Bt_inhibit_Click(object sender, EventArgs e)
    {
        _bayManager.Inhibit();
    }

    private void ACDToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.BayRepository.SetLayout(ACDToolStripMenuItem_Click);
        _bayManager.BayRepository.WipeBays();
        _bayManager.BayRepository.BayNum = 3;
        _ = new Bay([StripBay.BAY_PREA], _bayManager, _socketConn, "Preactive", 0);
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 1);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 2);
        _bayManager.BayRepository.Resize();
        _bayManager.BayRepository.ReloadStrips();
    }

    private void SMCToolStripMenuItem_Click(object sender, EventArgs e)
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
        _bayManager.BayRepository.ReloadStrips();
    }

    private void SMCACDToolStripMenuItem_Click(object sender, EventArgs e)
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
        _bayManager.BayRepository.ReloadStrips();
    }

    private void ADCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.BayRepository.SetLayout(ADCToolStripMenuItem_Click);
        _bayManager.BayRepository.WipeBays();
        _bayManager.BayRepository.BayNum = 4;
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 0);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 1);
        _ = new Bay([StripBay.BAY_OUT], _bayManager, _socketConn, "Departed", 2);
        _ = new Bay([StripBay.BAY_ARRIVAL], _bayManager, _socketConn, "Arrivals", 2);
        _bayManager.BayRepository.Resize();
        _bayManager.BayRepository.ReloadStrips();
    }

    private void AllToolStripMenuItem_Click(object sender, EventArgs e)
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
        _bayManager.BayRepository.ReloadStrips();
    }

    private void ADCSMCToolStripMenuItem_Click(object sender, EventArgs e)
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
        _bayManager.BayRepository.ReloadStrips();
    }

    private void Bt_cross_Click(object sender, EventArgs e)
    {
        _bayManager.CrossStrip();
    }

    // socket.io log
    private void ToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        var modalChild = new MsgListDebug(_socketConn);
        var bm = new BaseModal(modalChild, "Msg List");
        bm.Show(this);
    }

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var modalChild = new About();
        var bm = new BaseModal(modalChild, "About OzStrips");
        bm.Show(this);
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        _socketConn.Close();
        _socketConn.Dispose();
    }

    private void Bt_pdc_Click(object sender, EventArgs e)
    {
        _bayManager.SendPDC();
    }

    private void GitHubToolStripMenuItem_Click(object sender, EventArgs e)
    {
        System.Diagnostics.Process.Start("https://github.com/maxrumsey/OzStrips/");
    }

    private void DocumentationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        System.Diagnostics.Process.Start("https://maxrumsey.xyz/OzStrips/");
    }

    private void ChangelogToolStripMenuItem_Click(object sender, EventArgs e)
    {
        System.Diagnostics.Process.Start("https://maxrumsey.xyz/OzStrips/changelog");
    }

    private void NormalToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Util.SetEnvVar("StripSize", 2);
        _bayManager.BayRepository.ReloadStrips();
        SetStripSizeCheckBox();
    }

    private void SmallToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Util.SetEnvVar("StripSize", 1);
        _bayManager.BayRepository.ReloadStrips();
        SetStripSizeCheckBox();
    }

    private void SetStripSizeCheckBox()
    {
        normalToolStripMenuItem.Checked = false;
        smallToolStripMenuItem.Checked = false;
        tinyToolStripMenuItem.Checked = false;

        switch (OzStripsSettings.Default.StripSize)
        {
            case 0:
                tinyToolStripMenuItem.Checked = true;
                break;
            case 1:
                smallToolStripMenuItem.Checked = true;
                break;
            case 2:
                normalToolStripMenuItem.Checked = true;
                break;
        }
    }

    private void SetSmartResizeCheckBox()
    {
        colDisabledToolStripMenuItem.Checked = false;
        oneColumnToolStripMenuItem.Checked = false;
        twoColumnsToolStripMenuItem.Checked = false;
        threeColumnsToolStripMenuItem.Checked = false;

        switch (OzStripsSettings.Default.SmartResize)
        {
            case 0:
                colDisabledToolStripMenuItem.Checked = true;
                break;
            case 1:
                oneColumnToolStripMenuItem.Checked = true;
                break;
            case 2:
                twoColumnsToolStripMenuItem.Checked = true;
                break;
            case 3:
                threeColumnsToolStripMenuItem.Checked = true;
                break;
        }
    }

    private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var modalChild = new SettingsWindowControl(_socketConn, _aerodromes);
        var bm = new BaseModal(modalChild, "OzStrips Settings");
        bm.ReturnEvent += modalChild.ModalReturned;
        bm.Show(MainForm.MainFormInstance);
    }

    private void TinyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Util.SetEnvVar("StripSize", 0);
        _bayManager.BayRepository.ReloadStrips();
        SetStripSizeCheckBox();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        SetConnStatus();
    }

    private void ModifyButtonClicked(object sender, EventArgs e)
    {
        SettingsToolStripMenuItem_Click(this, new EventArgs());
    }

    private void AerodromeSelectorKeyDown(object sender, KeyPressEventArgs e)
    {
        if (_bayManager != null && e.KeyChar == Convert.ToChar(Keys.Enter, CultureInfo.InvariantCulture))
        {
            SetAerodrome(toolStripTextBox1.Text.ToUpper(CultureInfo.InvariantCulture));
            e.Handled = true;
        }
    }

    private void ReloadStripItem(object sender, EventArgs e)
    {
        StripElementList.Load();
    }

    private void BarCreatorClick(object sender, EventArgs e)
    {
        var modalChild = new BarCreator(_bayManager);
        var bm = new BaseModal(modalChild, "Add Bar");
        bm.ReturnEvent += modalChild.ModalReturned;
        bm.Show(this);
    }

    private void MainForm_Resize(object sender, EventArgs e)
    {
        if (WindowState != _lastState)
        {
            _lastState = WindowState;
            _postresizechecked = false;
            if (_bayManager is not null)
            {
                _bayManager.BayRepository.Resize();
            }
        }
    }

    private void ColDisabledToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SetSmartResizeColumnMode(0);
    }

    private void OneColumnToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SetSmartResizeColumnMode(1);

    }

    private void TwoColumnsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SetSmartResizeColumnMode(2);

    }

    private void ThreeColumnsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SetSmartResizeColumnMode(3);
    }

    private void SetSmartResizeColumnMode(int cols)
    {
        Util.SetEnvVar("SmartResize", cols);
        SetSmartResizeCheckBox();
        _bayManager.BayRepository.ReloadStrips();
    }
}
