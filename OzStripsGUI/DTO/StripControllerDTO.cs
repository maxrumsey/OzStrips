using System.Text.Json.Serialization;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents a controller strip with various operational details.
/// </summary>
public class StripControllerDTO
{
    /// <summary>
    /// Gets or sets the aircraft identifier.
    /// </summary>
    [JsonPropertyName("acid")]
    public string Acid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bay information for the strip.
    /// </summary>
    [JsonPropertyName("bay")]
    public StripBay Bay { get; set; }

    /// <summary>
    /// Gets or sets the cock level.
    /// </summary>
    [JsonPropertyName("cockLevel")]
    public int CockLevel { get; set; }

    /// <summary>
    /// Gets or sets the clearance.
    /// </summary>
    [JsonPropertyName("CLX")]
    public string CLX { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gate information.
    /// </summary>
    [JsonPropertyName("GATE")]
    public string Gate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time of takeoff.
    /// </summary>
    [JsonPropertyName("TOT")]
    public string TOT { get; set; } = "\0";

    /// <summary>
    /// Gets or sets a value indicating whether the aircraft is crossing.
    /// </summary>
    [JsonPropertyName("crossing")]
    public bool Crossing { get; set; }

    /// <summary>
    /// Gets or sets the subbay information.
    /// </summary>
    [JsonPropertyName("subbay")]
    public string SubBay { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any remarks associated with the strip.
    /// </summary>
    [JsonPropertyName("remark")]
    public string Remark { get; set; } = string.Empty;
}
