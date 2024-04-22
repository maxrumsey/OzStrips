using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents the bay configuration with a list of strips.
/// </summary>
public class BayDTO
{
    /// <summary>
    /// Gets or sets the bay information.
    /// </summary>
    [JsonPropertyName("bay")]
    public StripBay Bay { get; set; }

    /// <summary>
    /// Gets or sets the list of items or strips in the bay.
    /// </summary>
    [JsonPropertyName("list")]
    public List<string> List { get; set; } = [];
}
