using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// The list of aircraft contained in the quick search.
/// </summary>
public class AircraftList(Strip strip)
{
    /// <summary>
    /// Aircraft strip object.
    /// </summary>
    private readonly Strip _strip = strip;

    /// <summary>
    /// Gets the aircraft callsign.
    /// </summary>
    public string Callsign
    {
        get => _strip.FDR.Callsign;
    }

    /// <summary>
    /// Gets the aircraft bay name.
    /// </summary>
    public string Bay
    {
        get => _strip.CurrentBay.ToString() ?? "Unknown";
    }
}
