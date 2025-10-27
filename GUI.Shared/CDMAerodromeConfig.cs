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
    private DateTime _lastModified = DateTime.Now;

    /// <summary>
    /// Gets or sets the aerodrome code.
    /// </summary>
    public string? AerodromeCode { get; set; } = null!;

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

    /// <summary>
    /// Gets or sets when this record was last modified.
    /// </summary>
    public DateTime LastModified
    {
        get => _lastModified;
        set => _lastModified = DateTime.Now;
    }
}
