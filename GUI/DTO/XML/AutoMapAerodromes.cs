using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO.XML;

/// <summary>
/// Represents items in XML FIle.
/// </summary>
[Serializable]
[XmlRoot("AutoMapAerodrome")]

public class AutoMapAerodrome
{
    [XmlAttribute("FullName")]
    public string FullName;

    [XmlAttribute("ICAO")]
    public string ICAOCode;

    [XmlText]
    public string RawRunways;

    public string[] RunwayPairs
    {
        get
        {
            return RawRunways.Split(',').Select(r => r.Trim().Replace("/", string.Empty)).ToArray();
        }
    }
}
