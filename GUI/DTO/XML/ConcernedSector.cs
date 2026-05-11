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
[XmlRoot("ConcernedSector")]
public class ConcernedSector
{
    /// <summary>
    /// Gets or sets the list of aerodromes.
    /// </summary>
    [XmlArray("Aerodromes")]
    [XmlArrayItem("Aerodrome")]
    public string[] Aerodromes { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of positions.
    /// </summary>
    [XmlArray("Positions")]
    [XmlArrayItem("Position")]
    public string[] Positions { get; set; } = [];
}
