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
    private FormWindowState _lastState = FormWindowState.Minimized;

    private bool _postresizechecked = true;

    private MainFormController _mainFormController;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    /// <param name="readyForConnection">Whether the client can establish a server connection.</param>
    public MainForm(bool readyForConnection)
    {
        MainFormInstance = this;
        _mainFormController = new(this, readyForConnection);

        InitializeComponent();
        

        Util.SetAndReturnDLLVar();
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
    /// Opens the settings window.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Args.</param>
    public void ShowSettings(object sender, EventArgs e)
    {
        var modalChild = new SettingsWindowControl(_socketConn, _aerodromes);
        var bm = new BaseModal(modalChild, "OzStrips Settings");
        bm.ReturnEvent += modalChild.ModalReturned;
        bm.Show(MainForm.MainFormInstance);
    }

    /// <summary>
    /// Overrides keypress event to capture all keypresses.
    /// </summary>
    /// <param name="msg">Sender.</param>
    /// <param name="keyData">Key.</param>
    /// <returns>Handled.</returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        return _mainFormController.ProcessCmdKey(msg, keyData) ?? base.ProcessCmdKey(ref msg, keyData);
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
        _bayManager?.BayRepository.Resize();
        SetControlBarScrollBar();
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
        _bayManager.BayRepository.ReloadStrips(_socketConn);
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
        _bayManager.BayRepository.ReloadStrips(_socketConn);
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
        _bayManager.BayRepository.ReloadStrips(_socketConn);
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
        _bayManager.BayRepository.ReloadStrips(_socketConn);
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
        _bayManager.BayRepository.ReloadStrips(_socketConn);
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
        _bayManager.BayRepository.ReloadStrips(_socketConn);
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

    private void MainForm_Load(object sender, EventArgs e)
    {
        SetConnStatus();
        SetSmartResizeCheckBox();
    }

    private void ModifyButtonClicked(object sender, EventArgs e)
    {
        ShowSettings(this, EventArgs.Empty);
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
            _bayManager?.BayRepository.Resize();
            SetControlBarScrollBar();
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
        _bayManager.BayRepository.ReloadStrips(_socketConn);
    }

    private void SetControlBarScrollBar()
    {
        var margin = 0;

        if (pl_controlbar.HorizontalScroll.Visible)
        {
            margin = 17;
        }

        pl_controlbar.Padding = new Padding(0, 0, 0, margin);
    }

    private void FlipFlopStrip(object sender, EventArgs e)
    {
        _bayManager.FlipFlopStrip();
    }
}
