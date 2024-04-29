using System.Collections.Generic;
using Newtonsoft.Json;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents the bay configuration with a list of strips.
/// </summary>
public class BayDTO
{
    /// <summary>
    /// Gets or sets the bay information.
    /// </summary>
    [JsonProperty("bay")]
    public StripBay Bay { get; set; }

    /// <summary>
    /// Gets or sets the list of items or strips in the bay.
    /// </summary>
    [JsonProperty("list")]
    public List<string> List { get; set; } = [];
}
