using MaxRumsey.OzStripsPlugin.GUI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO.XML;

[XmlRoot("Bay")]
public class BayDefinition
{
    [XmlAttribute("Name")]
    public string Name;

    [XmlAttribute("Circuit")]
    public bool Circuit;

    [XmlAttribute("CDMDisplay")]
    public bool CDMDisplay;

    [XmlElement("Type")]
    public StripBay[] Types;
}
