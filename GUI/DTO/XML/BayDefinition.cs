using System.Xml.Serialization;
using MaxRumsey.OzStripsPlugin.GUI.Shared;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO.XML;

/// <summary>
/// Represents a single bay definition.
/// </summary>
[XmlRoot("Bay")]
public class BayDefinition
{
    /// <summary>
    /// Gets or sets the bay name.
    /// </summary>
    [XmlAttribute("Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this is a circuit bay.
    /// </summary>
    [XmlAttribute("Circuit")]
    public bool Circuit { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is a coordinator bay.
    /// </summary>
    [XmlAttribute("Coordinator")]
    public bool Coordinator { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this bay should display CDM stats.
    /// </summary>
    [XmlAttribute("CDMDisplay")]
    public bool CDMDisplay { get; set; }

    /// <summary>
    /// Gets or sets a list of StripBay types.
    /// </summary>
    [XmlElement("Type")]
    public StripBay[] Types { get; set; } = [];
}
