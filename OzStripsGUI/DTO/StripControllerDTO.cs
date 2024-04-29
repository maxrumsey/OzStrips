using Newtonsoft.Json;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents a controller strip with various operational details.
/// </summary>
public class StripControllerDTO
{
    /// <summary>
    /// Gets or sets the aircraft identifier.
    /// </summary>
    [JsonProperty("acid")]
    public string Acid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bay information for the strip.
    /// </summary>
    [JsonProperty("bay")]
    public StripBay Bay { get; set; }

    /// <summary>
    /// Gets or sets the cock level.
    /// </summary>
    [JsonProperty("cockLevel")]
    public int CockLevel { get; set; }

    /// <summary>
    /// Gets or sets the clearance.
    /// </summary>
    [JsonProperty(nameof(CLX))]
    public string CLX { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gate information.
    /// </summary>
    [JsonProperty("GATE")]
    public string Gate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time of takeoff.
    /// </summary>
    [JsonProperty(nameof(TOT))]
    public string TOT { get; set; } = "\0";

    /// <summary>
    /// Gets or sets a value indicating whether the aircraft is crossing.
    /// </summary>
    [JsonProperty("crossing")]
    public bool Crossing { get; set; }

    /// <summary>
    /// Gets or sets the subbay information.
    /// </summary>
    [JsonProperty("subbay")]
    public string SubBay { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any remarks associated with the strip.
    /// </summary>
    [JsonProperty("remark")]
    public string Remark { get; set; } = string.Empty;
}
