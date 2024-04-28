using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents a collection of strip controller data objects.
/// </summary>
public class CacheDTO
{
    /// <summary>
    /// Gets or sets the list of strip controllers.
    /// </summary>
    [JsonPropertyName("strips")]
    public List<StripControllerDTO> Strips { get; set; } = [];
}
