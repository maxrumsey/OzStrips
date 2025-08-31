using System.Text.Json.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Represents a controller strip with various operational details.
/// </summary>
public class SCDeletionDTO
{
#pragma warning disable SA1300 // Element should begin with upper-case letter

    /// <summary>
    /// Gets or sets the aircraft identifier.
    /// </summary>
    [JsonPropertyName("acid")]
    public string acid { get; set; } = string.Empty;
}
