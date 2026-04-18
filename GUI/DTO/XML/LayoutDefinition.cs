using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

/// <summary>
/// Contains layout definition information.
/// </summary>
[XmlRoot("Layout")]
public record LayoutDefinition
{
    /// <summary>
    /// Gets or sets the layout name.
    /// </summary>
    [XmlAttribute("Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a list of elements.
    /// </summary>
    [XmlElement("Element")]
    public LayoutElement[] Elements { get; set; } = [];

    /// <summary>
    /// Gets or sets the layout type.
    /// </summary>
    [XmlAttribute("Type")]
    public string Type { get; set; } = string.Empty;
}
