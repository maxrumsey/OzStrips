using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

[XmlRoot("Bay")]
public class BayDefinition
{
    [XmlAttribute("Name")]
    public string Name;

    [XmlAttribute("Circuit")]
    public bool Circuit;

    [XmlElement("Type")]
    public StripBay[] Types;

}
