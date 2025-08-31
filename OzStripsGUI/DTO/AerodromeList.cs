using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

/// <summary>
/// Represents items in XML FIle.
/// </summary>
[Serializable]
[XmlRoot("AerodromeList")]

public class AerodromeList
{
    [XmlAttribute("Type")]
    public string Type;

    [XmlElement("Aerodrome")]
    public string[] Aerodromes;
}
