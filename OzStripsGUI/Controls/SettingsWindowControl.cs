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
public partial class SettingsWindowControl : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsWindowControl"/> class.
    /// </summary>
    public SettingsWindowControl()
    {
        InitializeComponent();

        if (Properties.OzStripsSettings.Default.UseVatSysPopup)
        {
            rb_vatsys.Checked = true;
            rb_ozstrips.Checked = false;
        }
        else
        {
            rb_vatsys.Checked = false;
            rb_ozstrips.Checked = true;
        }
    }

    /// <summary>
    /// Called when the modal is closed.
    /// </summary>
    /// <param name="source">Source.</param>
    /// <param name="args">Arguments.</param>
    public void ModalReturned(object source, ModalReturnArgs args)
    {
        var usevatsyspopup = false;
        if (rb_vatsys.Checked)
        {
            usevatsyspopup = true;
        }

        Properties.OzStripsSettings.Default.UseVatSysPopup = usevatsyspopup;
        Properties.OzStripsSettings.Default.Save();
    }

    private void SBButtonClick(object sender, EventArgs e)
    {
        return;
    }
}
