using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO.XML;

/// <summary>
/// Contains child bays for a specific aerodrome type.
/// </summary>
[XmlRoot("BayType")]
public class BayType
{
    /// <summary>
    /// Gets or sets the aerodrome type.
    /// </summary>
    [XmlAttribute("Type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of bays.
    /// </summary>
    [XmlElement("Bay")]
    public BayDefinition[] Bays { get; set; } = [];
}
