using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using MaxRumsey.OzStripsPlugin.Gui.Controls;

using vatsys;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// The main application form.
/// </summary>
public partial class MainForm : Form
{
    private readonly Timer _timer;
    private readonly BayManager _bayManager;
    private readonly SocketConn _socketConn;
    private string _metar = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
        MainFormInstance = this;

        InitializeComponent();
        _timer = new()
        {
            Interval = 100,
        };

        _timer.Tick += UpdateTimer;
        _timer.Start();

        AddAerodrome("YBBN");
        AddAerodrome("YBCG");
        AddAerodrome("YBSU");
        AddAerodrome("YMML"); // todo: add more ads
        AddAerodrome("YPDN");
        AddAerodrome("YPPH");
        AddAerodrome("YSSY");

        _bayManager = new(flp_main);
        _socketConn = new(_bayManager, this);

        if (Network.IsConnected)
        {
            _socketConn.Connect();
        }

        AddVerticalStripBoard();
        AddVerticalStripBoard();
        AddVerticalStripBoard();

        _ = new Bay([StripBay.BAY_PREA], _bayManager, _socketConn, "Preactive", 0);
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 0);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 1);
        _ = new Bay([StripBay.BAY_TAXI], _bayManager, _socketConn, "Taxi", 1);
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 1);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 2);
        _ = new Bay([StripBay.BAY_OUT], _bayManager, _socketConn, "Departed", 2);
        _ = new Bay([StripBay.BAY_ARRIVAL], _bayManager, _socketConn, "Arrivals", 2);

        _bayManager.Resize();

        if (IsDebug)
        {
            var toolStripMenuItem = new ToolStripMenuItem
            {
                Text = "Manual Comm",
            };
            toolStripMenuItem.Click += (_, _) => OpenManDebug();
            debugToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
        }
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
    /// Sets the current aerodrome.
    /// </summary>
    /// <param name="name">The aerodrome name.</param>
    public void SetAerodrome(string name)
    {
        if (_bayManager != null)
        {
            _bayManager.SetAerodrome(name, _socketConn);
            _socketConn.SetAerodrome();
            lb_ad.Text = name;
        }
    }

    /// <summary>
    /// Sets the METAR information.
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
    /// Sets the ATIS code.
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
    /// <param name="conn">If connected or not.</param>
    public void SetConnStatus(bool conn)
    {
        pl_stat.BackColor = conn ? Color.Green : Color.OrangeRed;
    }

    /// <summary>
    /// Sets the selected track from vatSys.
    /// </summary>
    /// <param name="fdr">Selected FDR.</param>
    public void SetSelectedTrack(string? fdr)
    {
        _bayManager.Callsign = fdr;
    }

    /// <summary>
    /// Disconnects from VATSIM.
    /// </summary>
    public void DisconnectVATSIM()
    {
        _bayManager.WipeStrips();
        StripController.StripControllers.Clear();
        _socketConn.Disconnect();
    }

    /// <summary>
    /// Connects to VATSIM.
    /// </summary>
    public void ConnectVATSIM()
    {
        _socketConn.Connect();
    }

    /// <summary>
    /// Updates the flight data record.
    /// </summary>
    /// <param name="fdr">The flgiht data record.</param>
    /// <remarks>Triggered from Connector plugin.</remarks>
    public void UpdateFDR(FDP2.FDR fdr)
    {
        StripController.UpdateFDR(fdr, _bayManager, _socketConn);
    }

    /// <inheritdoc/>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Up:
                _bayManager.PositionKey(1);
                break;
            case Keys.Down:
                _bayManager.PositionKey(-1);
                break;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void UpdateTimer(object sender, EventArgs e)
    {
        if (!Visible)
        {
            return;
        }

        Invoke(() =>
        {
            tb_Time.Text = DateTime.UtcNow.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            _bayManager.ForceRerender();
        });
    }

    private void ForceRenderClicked(object sender, EventArgs e)
    {
        _bayManager.ForceRerender();
    }

    private void MainFormSizeChanged(object sender, EventArgs e)
    {
        _bayManager.Resize();
    }

    private void AddVerticalStripBoard()
    {
        var flp = new FlowLayoutPanel
        {
            AutoScroll = true,
            Margin = new(0),
            Padding = new(0),
            Size = new(100, 100),
            Location = new(0, 0),
        };

        flp_main.Controls.Add(flp);
        _bayManager.AddVertBoard(flp);
    }

    private void AddAerodrome(string name)
    {
        var toolStripMenuItem = new ToolStripMenuItem
        {
            Text = name,
        };
        toolStripMenuItem.Click += (sender, e) => SetAerodrome(name);
        ts_ad.DropDownItems.Add(toolStripMenuItem);
    }

    private void ToolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (_bayManager != null && e.KeyChar == Convert.ToChar(Keys.Enter, CultureInfo.InvariantCulture))
        {
            _bayManager.SetAerodrome(toolStripTextBox1.Text.ToUpper(CultureInfo.InvariantCulture), _socketConn);
            lb_ad.Text = toolStripTextBox1.Text.ToUpper(CultureInfo.InvariantCulture);
            e.Handled = true;
        }
    }

    private void Bt_inhibit_Click(object sender, EventArgs e)
    {
        _bayManager.Inhibit();
    }

    private void Bt_force_Click(object sender, EventArgs e)
    {
        _bayManager.ForceStrip(_socketConn);
    }

    private void ACDToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.WipeBays();
        _ = new Bay([StripBay.BAY_PREA], _bayManager, _socketConn, "Preactive", 0);
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 1);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 2);
        _bayManager.Resize();
        _bayManager.ReloadStrips();
    }

    private void SMCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.WipeBays();
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 0);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 0);
        _ = new Bay([StripBay.BAY_TAXI], _bayManager, _socketConn, "Taxi", 1);
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 2);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 2);
        _bayManager.Resize();
        _bayManager.ReloadStrips();
    }

    private void SMCACDToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.WipeBays();
        _ = new Bay([StripBay.BAY_PREA], _bayManager, _socketConn, "Preactive", 0);
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 0);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 0);
        _ = new Bay([StripBay.BAY_TAXI], _bayManager, _socketConn, "Taxi", 1);
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 2);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 2);
        _bayManager.Resize();
        _bayManager.ReloadStrips();
    }

    private void ADCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.WipeBays();
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 0);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 1);
        _ = new Bay([StripBay.BAY_OUT], _bayManager, _socketConn, "Departed", 2);
        _ = new Bay([StripBay.BAY_ARRIVAL], _bayManager, _socketConn, "Arrivals", 2);
        _bayManager.Resize();
        _bayManager.ReloadStrips();
    }

    private void AllToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.WipeBays();
        _ = new Bay([StripBay.BAY_PREA], _bayManager, _socketConn, "Preactive", 0);
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 0);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 1);
        _ = new Bay([StripBay.BAY_TAXI], _bayManager, _socketConn, "Taxi", 1);
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 1);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 2);
        _ = new Bay([StripBay.BAY_OUT], _bayManager, _socketConn, "Departed", 2);
        _ = new Bay([StripBay.BAY_ARRIVAL], _bayManager, _socketConn, "Arrivals", 2);
        _bayManager.Resize();
        _bayManager.ReloadStrips();
    }

    private void ADCSMCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _bayManager.WipeBays();
        _ = new Bay([StripBay.BAY_CLEARED], _bayManager, _socketConn, "Cleared", 0);
        _ = new Bay([StripBay.BAY_PUSHED], _bayManager, _socketConn, "Pushback", 0);
        _ = new Bay([StripBay.BAY_TAXI], _bayManager, _socketConn, "Taxi", 1);
        _ = new Bay([StripBay.BAY_HOLDSHORT], _bayManager, _socketConn, "Holding Point", 1);
        _ = new Bay([StripBay.BAY_RUNWAY], _bayManager, _socketConn, "Runway", 2);
        _ = new Bay([StripBay.BAY_OUT], _bayManager, _socketConn, "Departed", 2);
        _ = new Bay([StripBay.BAY_ARRIVAL], _bayManager, _socketConn, "Arrivals", 2);
        _bayManager.Resize();
        _bayManager.ReloadStrips();
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
    }

    private void Bt_pdc_Click(object sender, EventArgs e)
    {
        _bayManager.SendPDC();
    }

    private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
    }

    private void Ts_ad_Click(object sender, EventArgs e)
    {
    }

    private void GitHubToolStripMenuItem_Click(object sender, EventArgs e)
    {
        System.Diagnostics.Process.Start("https://github.com/maxrumsey/OzStrips/");
    }

    private void DocumentationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        System.Diagnostics.Process.Start("https://maxrumsey.xyz/OzStrips/");
    }
}
