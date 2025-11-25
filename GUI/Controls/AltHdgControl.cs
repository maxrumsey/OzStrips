using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

/// <summary>
/// A altitude and heading control.
/// </summary>
public partial class AltHdgControl : UserControl
{
    private readonly List<Airspace2.SystemRunway> _runways;
    private readonly Strip _stripController;
    private readonly bool _fullyLoaded;
    private readonly string _callingLabel;

    /// <summary>
    /// Initializes a new instance of the <see cref="AltHdgControl"/> class.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <param name="callingLabel">The label item to be selected upon opening.</param>
    public AltHdgControl(Strip controller, string callingLabel = "")
    {
        _stripController = controller;
        _runways = controller.PossibleDepRunways;
        InitializeComponent();
        SuspendLayout();
        foreach (var runway in _runways)
        {
            cb_runway.Items.Add(runway.Name);
        }

        tb_alt.Text = controller.CFL;
        if (!string.IsNullOrEmpty(controller.CFL))
        {
            cb_alt.Text = controller.CFL;
        }

        cb_depfreq.Text = controller.DepartureFrequency;

        cb_depfreq.Items.AddRange(controller.PossibleDepFreqs);

        _fullyLoaded = true;
        cb_runway.Text = controller.RWY;
        cb_sid.Text = controller.SID;
        ResumeLayout();

        _callingLabel = callingLabel;
    }

    /// <summary>
    /// Gets the heading.
    /// </summary>
    public string DepFreq => cb_depfreq.Text;

    /// <summary>
    /// Gets the altitude.
    /// </summary>
    public string Alt => tb_alt.Text;

    /// <summary>
    /// Gets the runway.
    /// </summary>
    public string Runway => cb_runway.Text;

    /// <summary>
    /// Gets the SID.
    /// </summary>
    public string SID => cb_sid.Text;

    /// <summary>
    /// Gets or sets the base modal.
    /// </summary>
    public BaseModal? BaseModal { get; set; }

    private void AltitudeComboSelectedChanged(object sender, EventArgs e)
    {
        tb_alt.Text = cb_alt.Text;
    }

    private void ClearAltitudeButtonClicked(object sender, EventArgs e)
    {
        tb_alt.Text = string.Empty;
    }

    private async void ComboRwySelectedChanged(object sender, EventArgs e)
    {
        if (_fullyLoaded && _stripController.RWY != cb_runway.Text)
        {
            _stripController.RWY = cb_runway.Text;
        }

        // timer allows vatsys to determine which sid to give, then load this in accordingly.
        await Task.Delay(1000);

        if (BaseModal?.Visible != true)
        {
            return;
        }

        var aerodrome = _stripController.FDR.DepAirport;

        cb_sid.Items.Clear();

        var runways = Airspace2.GetRunways(aerodrome);
        foreach (var runway in runways)
        {
            if (runway.Name == Runway)
            {
                foreach (var sid in runway.SIDs)
                {
                    cb_sid.Items.Add(sid.sidStar.Name);
                }
            }
        }

        cb_sid.Text = _stripController.SID;
    }

    private void AltKeyDownChanged(object sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.Enter:
                BaseModal?.ExitModal(true);
                break;
            case Keys.Escape:
                BaseModal?.ExitModal();
                break;
        }
    }

    private void AltHdgControl_Load(object sender, EventArgs e)
    {
        if (BaseModal is null)
        {
            return;
        }

        switch (_callingLabel)
        {
            case "cfl":
                ActiveControl = tb_alt;
                break;
        }
    }
}
