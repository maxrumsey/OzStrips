using System.Xml.Serialization;
using MaxRumsey.OzStripsPlugin.GUI.DTO.XML;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

/// <summary>
/// Layout elements.
/// </summary>
[XmlRoot("Element")]
public class LayoutElement
{
    /// <summary>
    /// Gets or sets the layout element name.
    /// </summary>
    [XmlAttribute("Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default column.
    /// </summary>
    [XmlAttribute("Column")]
    public int Column { get; set; }

    /// <summary>
    /// Gets or sets the correlated bay.
    /// </summary>
    internal BayDefinition? Bay { get; set; }
}
