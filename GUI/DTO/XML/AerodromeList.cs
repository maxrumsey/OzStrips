using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO.XML;

/// <summary>
/// Represents items in XML FIle.
/// </summary>
[Serializable]
[XmlRoot("AerodromeList")]

public class AerodromeList
{
    /// <summary>
    /// Gets or sets type of aerodrome.
    /// </summary>
    [XmlAttribute("Type")]
    public string Type { get; set; } = null!;

    /// <summary>
    /// Gets or sets aerodrome ICAO codes.
    /// </summary>
    [XmlElement("Aerodrome")]
    public string[] Aerodromes { get; set; } = null!;
}
