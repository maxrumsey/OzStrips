using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Represents the bay configuration with a list of strips.
/// </summary>
public class BayDTO
{
#pragma warning disable SA1300 // Element should begin with upper-case letter

    /// <summary>
    /// Gets or sets the bay information.
    /// </summary>
    [JsonPropertyName("bay")]
    public StripBay bay { get; set; }

    /// <summary>
    /// Gets or sets the list of items or strips in the bay.
    /// </summary>
    [JsonPropertyName("list")]
    public List<string> list { get; set; } = [];
}
