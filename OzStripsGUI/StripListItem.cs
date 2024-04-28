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
    public StripController? StripController { get; set; }

    /// <summary>
    /// Gets or sets the strip item type.
    /// </summary>
    public StripItemType Type { get; set; }

    /// <summary>
    /// Gets or sets the divider bar.
    /// </summary>
    public DividerBarControl? DividerBarControl { get; set; }
}
