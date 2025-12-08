using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static MaxRumsey.OzStripsPlugin.GUI.Controls.DropDownItem;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

/// <summary>
/// Drop down item used by the DropDown Form.
/// </summary>
public class DropDownItem(DropDownItemType type, string text, int maxlen = 3)
{
    /// <summary>
    /// Gets or sets the type of drop down item.
    /// </summary>
    public DropDownItemType Type { get; set; } = type;

    /// <summary>
    /// Gets or sets the text of the drop down item.
    /// </summary>
    public string Text { get; set; } = text;

    /// <summary>
    /// Possible drop down item types.
    /// </summary>
    public enum DropDownItemType
    {
        /// <summary>
        /// Button item.
        /// </summary>
        BUTTON,

        /// <summary>
        /// Freetext element.
        /// </summary>
        FREETEXT,
    }

    /// <summary>
    /// Max length for freetext items.
    /// </summary>
    public int MaxLen = maxlen;
}
