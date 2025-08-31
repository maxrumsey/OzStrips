using System.Text.Json.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1623 // Property summary documentation should match accessors

/// <summary>
/// Represents a controller strip with various operational details.
/// </summary>
public class StripDTO
{
    /// <summary>
    /// Gets or sets the bay information for the strip.
    /// </summary>
    [JsonPropertyName("bay")]
    public StripBay bay { get; set; }

    /// <summary>
    /// Gets or sets the cock level.
    /// </summary>
    [JsonPropertyName("cockLevel")]
    public int cockLevel { get; set; }

    /// <summary>
    /// Gets or sets the clearance.
    /// </summary>
    [JsonPropertyName(nameof(CLX))]
    public string CLX { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gate information.
    /// </summary>
    [JsonPropertyName("GATE")]
    public string GATE { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time of takeoff.
    /// </summary>
    [JsonPropertyName(nameof(TOT))]
    public string TOT { get; set; } = "\0";

    /// <summary>
    /// Gets or sets a value indicating whether the aircraft is crossing.
    /// </summary>
    [JsonPropertyName("crossing")]
    public bool crossing { get; set; }

    /// <summary>
    /// Gets or sets the subbay information.
    /// </summary>
    [JsonPropertyName("subbay")]
    public string subbay { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any remarks associated with the strip.
    /// </summary>
    [JsonPropertyName("remark")]
    public string remark { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating the ready status associated with the strip.
    /// </summary>
    [JsonPropertyName("ready")]
    public bool ready { get; set; }

    /// <summary>
    /// Gets or sets the strip key which uniquely identifies the strip.
    /// </summary>
    public StripKey StripKey { get; set; }

    /// <summary>
    /// Gets or sets the strip overriden type.
    /// </summary>
    public StripType OverrideStripType { get; set; } = StripType.UNKNOWN;
}
