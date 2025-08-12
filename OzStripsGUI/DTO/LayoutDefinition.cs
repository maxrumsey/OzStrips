using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

[XmlRoot("Layout")]
public class LayoutDefinition
{
    [XmlAttribute("Type")]
    public string Type;

    [XmlElement("Element")]
    public LayoutElement[] Elements;

}
