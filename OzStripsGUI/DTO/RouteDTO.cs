using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents the route returned from ERSA FPR for a specific city pair.
/// </summary>
public class RouteDTO
{
#pragma warning disable SA1300 // Element should begin with upper-case letter

    /// <summary>
    /// Gets or sets the aircraft type.
    /// </summary>
    [JsonPropertyName("acft")]
    public string acft { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the route information.
    /// </summary>
    [JsonPropertyName("route")]
    public string route { get; set; } = string.Empty;
}
