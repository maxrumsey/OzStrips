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
[XmlRoot("AutoOpen")]
public class AutoOpen
{
    /// <summary>
    /// Gets or sets the aerodrome ICAO code.
    /// </summary>
    public string Aerodrome { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the position name.
    /// </summary>
    public string Position { get; set; } = string.Empty;
}
