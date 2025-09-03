using MaxRumsey.OzStripsPlugin.Gui.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui;

public class MainFormController : IDisposable
{
    private FormWindowState _lastState = FormWindowState.Minimized;
    private bool _postresizechecked = true;
    private string _clientsOnline = string.Empty;

    private MainForm _mainForm;
    private readonly Timer _timer;
    private BayManager _bayManager;
    private SocketConn _socketConn;
    private string _metar = string.Empty;
    private bool _readyForConnection;
    private Action<object, EventArgs>? _defaultLayout;

    public static MainFormController? Instance { get; private set; }

    /// <summary>
    /// Gets whether or not a connection can be made to the server.
    /// </summary>
    public static bool ReadyForConnection => Instance?._readyForConnection ?? false;

    public static bool ControlHeld => Keyboard.Modifiers.HasFlag(ModifierKeys.Control);

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
        _bayManager = new(_mainForm.MainFLP);
        _socketConn = new(_bayManager, this);

        _socketConn.AerodromeStateChanged += AerodromeStateChanged;

        _mainForm.AerodromeManager.ViewListChanged += ViewListChanged;
        AerodromeListChanged(this, EventArgs.Empty);
        ViewListChanged(this, EventArgs.Empty);

        if (_defaultLayout is not null)
        {
            _bayManager.BayRepository.SetLayout(_defaultLayout);
        }
        else
        {
            throw new Exception("Default layout was not set. This means the config did not load correctly!");
        }

        if (_readyForConnection)
        {
            _socketConn.Connect();
        }


        _bayManager.BayRepository.ConfigureAndSizeFLPs();
        _mainForm.AerodromeManager.AerodromeListChanged += AerodromeListChanged;
    }

    public void FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Dispose();
    }

    private void AerodromeStateChanged(object sender, EventArgs e)
    {
        SetClientList(_bayManager.AerodromeState.Connections);
        _mainForm.StatusPanel.Invalidate();
        if (_bayManager.CircuitActive != _bayManager.AerodromeState.CircuitActive)
        {
            _bayManager.CircuitActive = _bayManager.AerodromeState.CircuitActive;
            ViewListChanged(this, EventArgs.Empty);
        }
    }

    private void ViewListChanged(object sender, EventArgs e)
    {
        _mainForm.ViewListToolStrip.DropDownItems.Clear();

        var layouts = _mainForm.AerodromeManager.ReturnLayouts(_mainForm.AerodromeManager.GetAerodromeType(_bayManager.AerodromeName));
        var bays = layouts.First(x => x.Name == "All").Elements.Select(x => x.Bay);

        var circuitBayDefined = bays.Any(x => x.Circuit);

        foreach (var layout in layouts)
        {
            var toolStripMenuItem = new ToolStripMenuItem
            {
                Text = layout.Name,
            };

            var action = (object sender, EventArgs e) =>
            {
                _bayManager.BayRepository.WipeBays();

                foreach (var element in layout.Elements)
                {
                    // If this is the circuit bay and we don't have it enabled.
                    if (element.Bay is null ||
                        (element.Bay.Circuit && !_bayManager.CircuitActive))
                    {
                        continue;
                    }

                    var types = element.Bay.Types.ToList();

                    // If circuit mode is enabled, don't have duplicate circuit bays
                    if (_bayManager.CircuitActive && !element.Bay.Circuit && circuitBayDefined)
                    {
                        types.Remove(StripBay.BAY_CIRCUIT);
                    }

                    _ = new Bay(types, _bayManager, _socketConn, element.Name, element.Column);
                }

                _bayManager.BayRepository.ConfigureAndSizeFLPs();
                _bayManager.BayRepository.ReloadStrips(_socketConn);
            };

            if (layout.Name == "All")
            {
                _defaultLayout = action;
            }

            toolStripMenuItem.Click += (sender, e) =>
            {
                _bayManager.BayRepository.SetLayout(action);

                // there are better ways of doing this!
                action(sender, e);
            };

            _mainForm.ViewListToolStrip.DropDownItems.Add(toolStripMenuItem);
        }

        // If bayrepo initialised, than directly call the layout func
        if (!_bayManager.BayRepository.Initialised)
        {
            _bayManager.BayRepository.ConfigureAndSizeFLPs();
        }

        _defaultLayout?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Gets or sets the custom list of aerodromes.
    /// </summary>
    /// <param name="value">The list of aerodromes.</param>
    public void SetCustomAerodromeList(List<string> value)
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
            SetCircuitToolStripStatus();
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
        _bayManager.BayRepository.ConfigureAndSizeFLPs(true);
        _bayManager.BayRepository.ConfigureAndSizeFLPs(); // double resize to take into account addition / subtraction of scroll bars.
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
                _bayManager.PurgeDataAndSetNewAerodrome(name, _socketConn);
                SetCircuitToolStripStatus();
                _socketConn.SubscribeToAerodrome();
                _mainForm.AerodromeLabel.Text = name;
                SetATISCode("Z");
                _mainForm.AerodromeManager.ConfigureAerodromeListForNewAerodrome(name);
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
        for (var i = 0; i < _mainForm.AerodromeManager.AerodromeList.Count; i++)
        {
            if (_mainForm.AerodromeManager.AerodromeList[i] == _bayManager.AerodromeName)
            {
                index = i;
            }
        }

        index += direction;
        if (index >= _mainForm.AerodromeManager.AerodromeList.Count)
        {
            index = 0;
        }

        if (index < 0)
        {
            index = _mainForm.AerodromeManager.AerodromeList.Count - 1;
        }

        SetAerodrome(_mainForm.AerodromeManager.AerodromeList[index]);
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
    /// Sets the list of online clients. Called from SocketConn.
    /// </summary>
    /// <param name="clients">The list of clients.</param>
    public void SetClientList(List<string> clients)
    {
        clients = clients.ToList();
        clients.Sort();
        clients.Remove(Network.Callsign);
        var str = String.Join("\n", clients);

        if (str != _clientsOnline)
        {
            _clientsOnline = str;
            _mainForm.ClientsToolTip.RemoveAll();
            _mainForm.ClientsToolTip.SetToolTip(_mainForm.StatusPanel, str);
        }
    }

    /// <summary>
    /// Sets the connection status, green is connected, orange/red if not.
    /// </summary>
    public void SetConnStatus()
    {
        _mainForm.StatusPanel.Invalidate();
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
        else if (keyData == (Keys.F | Keys.Control))
        {
            ShowQuickSearch();

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

    public static List<BaseModal> GetOpenModals()
    {
        var list = new List<BaseModal>();

        foreach (var form in Application.OpenForms)
        {
            if (form is BaseModal modal)
            {
                list.Add(modal);
            }
        }

        return list;
    }

    public static bool IsSettingsOpen()
    {
        return GetOpenModals().Any(x => x.Child is SettingsWindowControl);
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
            _bayManager.BayRepository.ConfigureAndSizeFLPs();
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
        _bayManager?.BayRepository.ConfigureAndSizeFLPs();
        _mainForm.SetControlBarScrollBar();
    }

    internal void AddAerodrome(string name)
    {
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

    internal void ToggleCircuitBay(object sender, EventArgs e)
    {
        _socketConn.RequestCircuit(!_bayManager.CircuitActive);
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

    /// <summary>
    /// Opens the settings window.
    /// </summary>
    public void ShowQuickSearch()
    {
        var modalChild = new QuickSearch(_bayManager);
        var bm = new BaseModal(modalChild, "Quick Search");
        modalChild.BaseModal = bm;
        bm.ReturnEvent += modalChild.ModalReturned;
        bm.Show(MainForm.MainFormInstance);
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
            _bayManager?.BayRepository.ConfigureAndSizeFLPs();
            _mainForm.SetControlBarScrollBar();
        }
    }

    public void SetSmartResizeColumnMode(int cols)
    {
        Util.SetEnvVar("SmartResize", cols);
        _mainForm.SetSmartResizeCheckBox();
        _bayManager.BayRepository.ReloadStrips(_socketConn);
    }

    public void SetCircuitToolStripStatus()
    {
        var isRadarTower = String.IsNullOrEmpty(_mainForm.AerodromeManager.GetAerodromeType(_bayManager.AerodromeName));
        var canSend = _socketConn.HaveSendPerms;

        _mainForm.ToggleCircuitToolStrip.Enabled = isRadarTower && canSend;
    }

    public void ConnStatusPaint(object sender, PaintEventArgs e)
    {
        var borderWidth = 5;
        var g = e.Graphics;
        g.Clear(Color.Purple);
        var coreBrush = new SolidBrush(_socketConn.Connected ? Color.Green : Color.OrangeRed);

        var outerBrush = new SolidBrush(_bayManager.AerodromeState.Connections?.Count > 1 ? Color.Blue : coreBrush.Color);

        var textBrush = new SolidBrush(Color.Black);
        var font = new Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold);

        g.FillRectangle(outerBrush, e.ClipRectangle);

        g.FillRectangle(coreBrush, e.ClipRectangle.X + borderWidth, e.ClipRectangle.Y + borderWidth, e.ClipRectangle.Width - (borderWidth * 2), e.ClipRectangle.Height - (borderWidth * 2));

        g.DrawString("CONN STAT", font, textBrush, e.ClipRectangle, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center});
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
        if (IsSettingsOpen())
        {
            return;
        }

        var modalChild = new SettingsWindowControl(_socketConn, _mainForm.AerodromeManager.ManuallySetAerodromes);
        var bm = new BaseModal(modalChild, "OzStrips Settings");
        bm.ReturnEvent += modalChild.ModalReturned;
        bm.Show(_mainForm);
    }

    public void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        _mainForm.AerodromeManager.PreviouslyClosed = true;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _mainForm.AerodromeManager.AerodromeListChanged -= AerodromeListChanged;
        _socketConn.Dispose();
    }

    private void AerodromeListChanged(object sender, EventArgs e)
    {
        foreach (var item in _mainForm.AerodromeListToolStrip.DropDownItems.OfType<ToolStripItem>().ToArray())
        {
            if (item.Tag is null)
            {
                _mainForm.AerodromeListToolStrip.DropDownItems.Remove(item);
            }
        }

        foreach (var item in _mainForm.AerodromeManager.AerodromeList)
        {
            AddAerodrome(item);
        }

        if (_mainForm.AerodromeManager.AutoOpenAerodrome != null &&
            _bayManager?.AerodromeName != _mainForm.AerodromeManager.AutoOpenAerodrome)
        {
            try
            {
                SetAerodrome(_mainForm.AerodromeManager.AutoOpenAerodrome);
            }
            catch (Exception ex)
            {
                Util.LogError(ex);
            }
        }
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
