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

        tb_pdc.Text = $"PDC for {controller.FDR.Callsign}." +
                      $"Climb via SID to @A090@";
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
        get => tb_pdc.Text;
    }

    private void OpenVatsysPDC(object sender, EventArgs e)
    {
        BaseModal?.ExitModal(false);
        _strip.Controller.OpenVatSysPDCWindow();
    }
}
