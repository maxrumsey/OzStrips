using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// CDM departure information.
/// </summary>
public class CDMDepartureDTO
{
    /// <summary>
    /// Gets or sets the callsign.
    /// </summary>
    [JsonRequired]
    public string Callsign { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ATOT.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset ATOT { get; set; } = DateTimeOffset.MaxValue;
}
