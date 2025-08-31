using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

[XmlRoot("Layout")]
public record LayoutDefinition
{
    [XmlAttribute("Name")]
    public string Name;

    [XmlElement("Element")]
    public LayoutElement[] Elements;

    [XmlAttribute("Type")]
    public string Type = string.Empty;
}
