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
    /// <summary>
    /// Initializes a new instance of the <see cref="PDCControl"/> class.
    /// </summary>
    /// <param name="controller">The strip.</param>
    public PDCControl(Strip controller)
    {
        InitializeComponent();
        lb_title.Text += $" {controller.FDR.Callsign}";

        tb_pdc.Text = $"PDC for {controller.FDR.Callsign}." +
                      $"Climb via SID to @A090@";
    }

    /// <summary>
    /// Gets the PDC text.
    /// </summary>
    public string PDCText
    {
        get => tb_pdc.Text;
    }
}
