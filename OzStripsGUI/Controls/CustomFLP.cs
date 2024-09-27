using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// A custom flow layout panel.
/// </summary>
public class CustomFLP : FlowLayoutPanel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFLP"/> class.
    /// </summary>
    public CustomFLP()
    {
        // SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        UpdateStyles();
    }
}
