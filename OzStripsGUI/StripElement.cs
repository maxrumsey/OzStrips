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
    public StripElements.Actions LeftClick { get; set; } = StripElements.Actions.NONE;

    /// <summary>
    /// Gets or sets the Origin.X.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the Origin.Y.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Gets or sets strip width.
    /// </summary>
    public int W { get; set; }

    /// <summary>
    /// Gets or sets strip height.
    /// </summary>
    public int H { get; set; }

    /// <summary>
    /// Gets or sets the font size in pixels.
    /// </summary>
    public int FontSize { get; set; } = 12;
}
