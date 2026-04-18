using System.Xml.Serialization;
using MaxRumsey.OzStripsPlugin.GUI.Shared;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO.XML;

/// <summary>
/// Represents the strip colour settings.
/// </summary>
[XmlRoot("StripColour")]
public class StripColour
{
    /// <summary>
    /// Gets or sets strip type.
    /// </summary>
    [XmlAttribute("Type")]
    public StripType Type { get; set; } = StripType.UNKNOWN;

    /// <summary>
    /// Gets or sets strip colour.
    /// </summary>
    [XmlText]
    public string Colour { get; set; } = "#FFFFFF";
}
