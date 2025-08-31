using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

[XmlRoot("BayType")]
public class BayType
{
    [XmlAttribute("Type")]
    public string Type = string.Empty;

    [XmlElement("Bay")]
    public BayDefinition[] Bays;
}
