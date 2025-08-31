using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

/// <summary>
/// Represents items in XML FIle.
/// </summary>
[Serializable]
[XmlRoot("ConcernedSector")]

public class ConcernedSector
{
    [XmlArray("Aerodromes")]
    [XmlArrayItem("Aerodrome")]
    public string[] Aerodromes;

    [XmlArray("Positions")]
    [XmlArrayItem("Position")]
    public string[] Positions;
}
