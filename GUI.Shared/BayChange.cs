using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Contains information about a change to bay items or a reorder.
/// </summary>
public class BayChange
{
    /// <summary>
    /// Bay change types.
    /// </summary>
    public enum BayChangeTypes
    {
        /// <summary>
        /// used when a bar is added.
        /// </summary>
        ADD_BAR,

        /// <summary>
        /// Used when a bar is deleted.
        /// </summary>
        DELETE_BAR,

        /// <summary>
        /// Used when a strip item is moved.
        /// </summary>
        MOVE_ELEMENT_BELOW,
    }

    /// <summary>
    /// Gets or sets the type of change.
    /// </summary>
    public BayChangeTypes Type { get; set; }

    /// <summary>
    /// Gets or sets the element being moved/deleted/added.
    /// </summary>
    public BayChangeStripItem? TargetItem { get; set; }

    /// <summary>
    /// Gets or sets the element **above** the item being moved/deleted/added where relevant.
    /// </summary>
    public BayChangeStripItem? ArgumentItem { get; set; }

    /// <summary>
    /// Gets or sets the described bay type.
    /// </summary>
    public StripBay? BayType { get; set; }
}
