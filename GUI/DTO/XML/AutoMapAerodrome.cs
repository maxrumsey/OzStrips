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
    /// <summary>
    /// Gets or sets the aerodrome full name per the vatsys dropdown.
    /// </summary>
    [XmlAttribute("FullName")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the aerodrome ICAO code.
    /// </summary>
    [XmlAttribute("ICAO")]
    public string ICAOCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of raw runway pairs.
    /// </summary>
    [XmlText]
    public string RawRunways { get; set; } = string.Empty;

    /// <summary>
    /// Gets the list of runwya pairs.
    /// </summary>
    public string[] RunwayPairs
    {
        get
        {
            return RawRunways.Split(',').Select(r => r.Trim().Replace("/", string.Empty)).ToArray();
        }
    }
}
