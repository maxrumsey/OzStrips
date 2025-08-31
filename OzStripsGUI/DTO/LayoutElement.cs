using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

[XmlRoot("Element")]
public class LayoutElement
{
    [XmlAttribute("Name")]
    public string Name;

    [XmlAttribute("Column")]
    public int Column;

    internal BayDefinition? Bay;
}
