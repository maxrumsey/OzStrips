using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Represents configurable CDM data about an aerodrome.
/// </summary>
public class CDMAerodromeConfig
{
    /// <summary>
    /// Gets or sets the aerodrome code.
    /// </summary>
    public string? AerodromeCode { get; set; } = null!;

    /// <summary>
    /// Gets or sets the base airport departure rate.
    /// </summary>
    public int BaseADR { get; set; } = 30;

    /// <summary>
    /// Gets or sets a value indicating whether plugin initiated changes are currently locked.
    /// </summary>
    public bool PluginChangesLocked { get; set; }

    /// <summary>
    /// Gets or sets a dictionary of runway taxi times.
    /// </summary>
    public Dictionary<string, int> TaxiTimes { get; set; } = [];

    /// <summary>
    /// Gets or sets arrival aerodrome specific arrival rates.
    /// </summary>
    public Dictionary<string, int> SpecificAerodromeDepartureRate { get; set; } = [];

    /// <summary>
    /// Gets or sets a list of whitelisted ADESes.
    /// </summary>
    public List<string> WhitelistedAerodromes { get; set; } = [];
}
