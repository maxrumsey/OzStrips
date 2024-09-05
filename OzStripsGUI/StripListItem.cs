using MaxRumsey.OzStripsPlugin.Gui.Controls;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// A strip list item.
/// </summary>
public class StripListItem
{
    /// <summary>
    /// Gets or sets the strip controller.
    /// </summary>
    public Strip? StripController { get; set; }

    /// <summary>
    /// Gets or sets the strip item type.
    /// </summary>
    public StripItemType Type { get; set; }

    /// <summary>
    /// Gets or sets the view class.
    /// </summary>
    internal IRenderedStripItem? RenderedStripItem { get; set; }
}
