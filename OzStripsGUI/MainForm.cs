using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.Controls;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using MaxRumsey.OzStripsPlugin.Gui.Properties;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// The main application form.
/// </summary>
/// All but the most basic of logic is abstracted to MainFormController
public partial class MainForm : Form
{
    private MainFormController _mainFormController;
    private NoScrollFLP _flpMain;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    /// <param name="readyForConnection">Whether the client can establish a server connection.</param>
    public MainForm(bool readyForConnection, AerodromeManager aerodromeManager)
    {
        MainFormInstance = this;
        AerodromeManager = aerodromeManager;
        _mainFormController = new(this, readyForConnection);

        InitializeComponent();
        InitialiseEvents();

        CreateMainFLP();
        _mainFormController.Initialize();

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

    public Label AerodromeLabel => lb_ad;

    public ToolTip MetarToolTip => tt_metar;

    public ToolTip ClientsToolTip => tt_clients;

    public ToolStripMenuItem ToggleCircuitToolStrip => ts_toggleCircuit;

    public Panel StatusPanel => pl_stat;

    public Label ATISLabel => lb_atis;

    public TextBox TimerTextBox => tb_Time;

    public ToolStripMenuItem AerodromeListToolStrip => ts_ad;

    public ToolStripMenuItem ViewListToolStrip => ts_mode;

    public string EnteredAerodrome => toolStripTextBox1.Text;

    public FlowLayoutPanel MainFLP => _flpMain;

    public AerodromeManager AerodromeManager { get; private set; }

    public MainFormController Controller
    {
        get
        {
            return _mainFormController;
        }
    }

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
    /// Overrides keypress event to capture all keypresses.
    /// </summary>
    /// <param name="msg">Sender.</param>
    /// <param name="keyData">Key.</param>
    /// <returns>Handled.</returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        return _mainFormController.ProcessCmdKey(ref msg, keyData) ?? base.ProcessCmdKey(ref msg, keyData);
    }

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var modalChild = new About();
        var bm = new BaseModal(modalChild, "About OzStrips");
        bm.Show(this);
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

    public void SetSmartResizeCheckBox()
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

    private void ModifyButtonClicked(object sender, EventArgs e)
    {
        _mainFormController.ShowSettings(this, EventArgs.Empty);
    }

    private void ReloadStripItem(object sender, EventArgs e)
    {
        StripElementList.Load();
    }

    private void ReloadAerodromes(object sender, EventArgs e)
    {
        AerodromeManager.LoadSettings();
    }

    private void ColDisabledToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _mainFormController.SetSmartResizeColumnMode(0);
    }

    private void OneColumnToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _mainFormController.SetSmartResizeColumnMode(1);
    }

    private void TwoColumnsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _mainFormController.SetSmartResizeColumnMode(2);
    }

    private void ThreeColumnsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _mainFormController.SetSmartResizeColumnMode(3);
    }

    private void CreateMainFLP()
    {
        // Try to prevent designer auto-deletion.
        _flpMain = new();
        _flpMain.AutoScroll = true;
        _flpMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
        _flpMain.Dock = System.Windows.Forms.DockStyle.Fill;
        _flpMain.Location = new System.Drawing.Point(0, 25);
        _flpMain.Margin = new System.Windows.Forms.Padding(0);
        _flpMain.Name = "flp_main";
        _flpMain.Size = new System.Drawing.Size(1784, 891);
        _flpMain.TabIndex = 2;
        _flpMain.WrapContents = false;

        Controls.Add(_flpMain);
        Controls.SetChildIndex(_flpMain, 0);
    }

    public void SetControlBarScrollBar()
    {
        var margin = 0;

        if (pl_controlbar.HorizontalScroll.Visible)
        {
            margin = 17;
        }

        pl_controlbar.Padding = new Padding(0, 0, 0, margin);
    }

    private void InitialiseEvents()
    {
        bt_flip.Click += _mainFormController.FlipFlopStrip;
        bt_bar.Click += _mainFormController.BarCreatorClick;
        bt_cross.Click += _mainFormController.Bt_cross_Click;
        bt_inhibit.Click += _mainFormController.Bt_inhibit_Click;
        toolStripTextBox1.KeyPress += _mainFormController.AerodromeSelectorKeyDown;
        toolStripMenuItem1.Click += _mainFormController.ShowMessageList_Click;
        settingsToolStripMenuItem.Click += _mainFormController.ShowSettings;
        ts_toggleCircuit.Click += _mainFormController.ToggleCircuitBay;
        pl_stat.Paint += _mainFormController.ConnStatusPaint;
        FormClosed += _mainFormController.MainForm_FormClosed;
        Load += _mainFormController.MainForm_Load;
        ResizeEnd += _mainFormController.MainFormSizeChanged;
        Resize += _mainFormController.MainForm_Resize;
        Closing += _mainFormController.FormClosing;
    }
}
