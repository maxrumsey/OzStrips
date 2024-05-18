using System.Text.Json.Serialization;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents the metadata for an API, including versions.
/// </summary>
public class MetadataDTO
{
#pragma warning disable SA1300 // Element should begin with upper-case letter

    /// <summary>
    /// Gets or sets the version of the data.
    /// </summary>
    [JsonPropertyName("version")]
    public string version { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API version.
    /// </summary>
    [JsonPropertyName("apiversion")]
    public string apiversion { get; set; } = string.Empty;
}
