using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Used to describe a stripitem within a bay change event.
/// </summary>
public class BayChangeStripItem
{
    /// <summary>
    /// Gets or sets a value indicating whether this is a real aircraft strip.
    /// </summary>
    public bool IsStrip { get; set; }

    /// <summary>
    /// Gets or sets the strip key if relevant.
    /// </summary>
    public StripKey? StripKey { get; set; }

    /// <summary>
    /// Gets or sets the bar identifier if relevant.
    /// </summary>
    public string? BarIdentifier { get; set; }
}
