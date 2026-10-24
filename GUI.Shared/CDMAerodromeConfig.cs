using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Represents configurable CDM data about an aerodrome.
/// </summary>
public class CDMAerodromeConfig
{
    private DateTimeOffset _lastModified = DateTimeOffset.Now;

    /// <summary>
    /// Gets or sets the aerodrome code.
    /// </summary>
    public string? AerodromeCode { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether plugin initiated changes are currently locked.
    /// </summary>
    [JsonRequired]
    public bool PluginChangesLocked { get; set; }

    /// <summary>
    /// Gets or sets a dictionary of runway taxi times.
    /// </summary>
    [JsonRequired]
    public Dictionary<string, int> TaxiTimes { get; set; } = [];

    /// <summary>
    /// Gets or sets arrival aerodrome specific arrival rates.
    /// </summary>
    [JsonRequired]
    public Dictionary<string, int> SpecificAerodromeDepartureRate { get; set; } = [];

    /// <summary>
    /// Gets or sets a list of whitelisted ADESes.
    /// </summary>
    [JsonRequired]
    public List<string> WhitelistedAerodromes { get; set; } = [];

    /// <summary>
    /// Gets or sets when this record was last modified.
    /// </summary>
    public DateTimeOffset LastModified
    {
        get => _lastModified;
        set => _lastModified = DateTimeOffset.Now;
    }
}
