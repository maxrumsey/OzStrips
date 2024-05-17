using Newtonsoft.Json;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents a controller strip with various operational details.
/// </summary>
public class StripControllerDTO
{
#pragma warning disable SA1300 // Element should begin with upper-case letter

    /// <summary>
    /// Gets or sets the aircraft identifier.
    /// </summary>
    [JsonPropertyName("acid")]
    public string acid { get; set; } = string.Empty;

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
    [JsonProperty(nameof(CLX))]
    public string CLX { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gate information.
    /// </summary>
    [JsonPropertyName("GATE")]
    public string GATE { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time of takeoff.
    /// </summary>
    [JsonProperty(nameof(TOT))]
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
}
