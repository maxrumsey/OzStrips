using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

/// <summary>
/// Represents a collection of strip controller data objects.
/// </summary>
public class CacheDTO
{
#pragma warning disable SA1300 // Element should begin with upper-case letter

    /// <summary>
    /// Gets or sets the list of strip controllers.
    /// </summary>
    [JsonPropertyName("strips")]
    public List<StripDTO> strips { get; set; } = [];
}
