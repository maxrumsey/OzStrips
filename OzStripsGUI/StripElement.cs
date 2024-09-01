using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Represents items in XML FIle.
/// </summary>
[Serializable]
[XmlRoot("StripElement")]
public class StripElement
{
    /// <summary>
    /// Gets or sets the content of the strip item.
    /// </summary>
    public StripElements.Values Value { get; set; }

    /// <summary>
    /// Gets or sets the left click action.
    /// </summary>
    public StripElements.Actions LeftClick { get; set; }
}
