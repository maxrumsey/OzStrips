using System.Collections.Generic;
using Newtonsoft.Json;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents a collection of strip controller data objects.
/// </summary>
public class CacheDTO
{
    /// <summary>
    /// Gets or sets the list of strip controllers.
    /// </summary>
    [JsonProperty("strips")]
    public List<StripControllerDTO> Strips { get; set; } = [];
}
