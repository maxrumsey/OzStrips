using Newtonsoft.Json;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents the metadata for an API, including versions.
/// </summary>
public class MetadataDTO
{
    /// <summary>
    /// Gets or sets the version of the data.
    /// </summary>
    [JsonProperty("version")]
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API version.
    /// </summary>
    [JsonProperty("apiversion")]
    public string ApiVersion { get; set; } = string.Empty;
}
