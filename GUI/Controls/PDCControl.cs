using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

/// <summary>
/// A altitude and heading control.
/// </summary>
public partial class PDCControl : UserControl
{
    private readonly Strip _strip;

    /// <summary>
    /// Initializes a new instance of the <see cref="PDCControl"/> class.
    /// </summary>
    /// <param name="controller">The strip.</param>
    public PDCControl(Strip controller)
    {
        _strip = controller;
        InitializeComponent();
        lb_title.Text += $" {controller.FDR.Callsign}";

        foreach (var freq in Network.Me?.Frequencies?.Where(x => x != 99998).ToArray() ?? [])
        {
            var freqString = Conversions.FSDFrequencyToString(freq);
            cb_delivery.Items.Add(freqString);
        }

        if (cb_delivery.Items.Count > 0)
        {
            cb_delivery.SelectedIndex = 0;
        }

        LayoutPDC();
    }

    /// <summary>
    /// Gets or sets the base modal.
    /// </summary>
    public BaseModal? BaseModal { get; set; }

    /// <summary>
    /// Gets the PDC text.
    /// </summary>
    public string PDCText
    {
        get => tb_pdc.Text.Replace("\r", string.Empty).Replace("\n", ". ").Replace("{", string.Empty).Replace("}", string.Empty);
    }

    private void OpenVatsysPDC(object sender, EventArgs e)
    {
        BaseModal?.ExitModal(false);
        _strip.Controller.OpenVatSysPDCWindow();
    }

    private void LayoutPDC()
    {
        var trans = string.Empty;
        if (!string.IsNullOrEmpty(_strip.SIDTransition))
        {
            trans = $"{CPDLCify(_strip.SIDTransition!)}:TRAN";
        }

        if (!int.TryParse(_strip.CFL, out var cfl))
        {
            cfl = 0;
        }

        var cfl_fl = cfl;

        cfl *= 100;
        var ssr = (_strip.FDR.AssignedSSRCode == -1) ? "XXXX" : Convert.ToString(_strip.FDR.AssignedSSRCode, 8).PadLeft(4, '0');

        var format = AerodromeManager.PDCFormat
        .Replace("\n", "\r\n")
        .Replace("{CALLSIGN}", _strip.FDR.Callsign)
        .Replace("{TYPE}", _strip.FDR.AircraftType)
        .Replace("{ADEP}", _strip.FDR.DepAirport)
        .Replace("{ETD}", _strip.FDR.ETD.ToString("HHmm", CultureInfo.InvariantCulture))
        .Replace("{ADES}", CPDLCify(_strip.FDR.DesAirport))
        .Replace("{RWY}", CPDLCify(_strip.RWY))
        .Replace("{SID}", CPDLCify(_strip.SID))
        .Replace("{TRANS}", trans)
        .Replace("{ROUTE}", _strip.FDR.Route)
        .Replace("{CFL}", CPDLCify(cfl.ToString(CultureInfo.InvariantCulture)))
        .Replace("{CFL_FL}", CPDLCify(cfl_fl.ToString(CultureInfo.InvariantCulture)))
        .Replace("{RFL_FL}", CPDLCify(_strip.RFL.ToString(CultureInfo.InvariantCulture)))
        .Replace("{RFL}", CPDLCify(_strip.FDR.RFL.ToString(CultureInfo.InvariantCulture)))
        .Replace("{FREQ}", CPDLCify(_strip.DepartureFrequency))
        .Replace("{SQUAWK}", CPDLCify(ssr))
        .Replace("{READBACK}", cb_delivery.Text);

        var error = string.Empty;

        if (ssr == "XXXX" ||
            cfl == 0 ||
            string.IsNullOrEmpty(_strip.SID) ||
            string.IsNullOrEmpty(_strip.RWY) ||
            string.IsNullOrEmpty(_strip.DepartureFrequency))
        {
            error = "Error: All PDC elements could not be filled.";
        }

        label1.Text = error;
        tb_pdc.Text = format;
    }

    private void ResetButtonClicked(object sender, EventArgs e)
    {
        LayoutPDC();
    }

    private void cb_delivery_SelectedIndexChanged(object sender, EventArgs e)
    {
        LayoutPDC();
    }

    private static string CPDLCify(string input)
    {
        return $"@{input}@";
    }
}
