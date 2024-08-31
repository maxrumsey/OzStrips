using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// A altitude and heading control.
/// </summary>
public partial class AltHdgControl : UserControl
{
    private readonly List<Airspace2.SystemRunway> _runways;
    private readonly Strip _stripController;
    private readonly bool _fullyLoaded;

    /// <summary>
    /// Initializes a new instance of the <see cref="AltHdgControl"/> class.
    /// </summary>
    /// <param name="controller">The controller.</param>
    public AltHdgControl(Strip controller)
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

        tb_hdg.Text = controller.HDG; // todo: add some sort of parsing for this
        _fullyLoaded = true;
        cb_runway.Text = controller.RWY;
        cb_sid.Text = controller.SID;
        ResumeLayout();
    }

    /// <summary>
    /// Gets the heading.
    /// </summary>
    public string Hdg => tb_hdg.Text;

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

    private void ClearHeadingButtonClicked(object sender, EventArgs e)
    {
        tb_hdg.Text = string.Empty;
    }

    private void AddHdgVal(int amt)
    {
        var amount = amt.ToString(CultureInfo.InvariantCulture);
        if (tb_hdg.Text.Length < 3)
        {
            tb_hdg.Text += amount;
        }
    }

    private void Button7Clicked(object sender, EventArgs e)
    {
        AddHdgVal(7);
    }

    private void ButtonZeroClicked(object sender, EventArgs e)
    {
        AddHdgVal(0);
    }

    private void Button1Clicked(object sender, EventArgs e)
    {
        AddHdgVal(1);
    }

    private void Button2Clicked(object sender, EventArgs e)
    {
        AddHdgVal(2);
    }

    private void Button3Clicked(object sender, EventArgs e)
    {
        AddHdgVal(3);
    }

    private void Button4Clicked(object sender, EventArgs e)
    {
        AddHdgVal(4);
    }

    private void Button5Clicked(object sender, EventArgs e)
    {
        AddHdgVal(5);
    }

    private void Button6Clicked(object sender, EventArgs e)
    {
        AddHdgVal(6);
    }

    private void Button8Clicked(object sender, EventArgs e)
    {
        AddHdgVal(8);
    }

    private void Button9Clicked(object sender, EventArgs e)
    {
        AddHdgVal(9);
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
}
