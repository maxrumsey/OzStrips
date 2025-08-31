using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

/// <summary>
/// A FlowLayoutPanel that does not scroll when it loses and regains focus.
/// </summary>
public class NoScrollFLP : FlowLayoutPanel
{
    /// <summary>
    /// Prevents scrolling to control.
    /// </summary>
    /// <param name="activeControl">The active control</param>
    /// <returns>Point.</returns>
    protected override System.Drawing.Point ScrollToControl(System.Windows.Forms.Control activeControl)
    {
        // Returning the current location prevents the panel from
        // scrolling to the active control when the panel loses and regains focus
        return DisplayRectangle.Location;
    }
}
