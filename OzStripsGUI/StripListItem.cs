using MaxRumsey.OzStripsPlugin.Gui.Controls;
using static vatsys.FDP2;

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
    /// Gets or sets the bar text.
    /// </summary>
    public string? BarText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the view class.
    /// </summary>
    internal IRenderedStripItem? RenderedStripItem { get; set; }

    internal int? Style { get; set; }

    /// <summary>
    /// To string method.
    /// </summary>
    /// <returns>Description.</returns>
    public override string ToString()
    {
        if (Type == StripItemType.STRIP)
        {
            return StripController?.ToString() ?? string.Empty;
        }
        else
        {
            return "BAR";
        }
    }

    /// <summary>
    /// Determines whether or not an object equals the class.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>Equal.</returns>
    public bool Matches(object obj)
    {
        if (obj.GetType() != typeof(StripListItem))
        {
            return false;
        }

        var item = (StripListItem)obj;
        if (item.StripController == StripController &&
            item.Type == Type &&
            item.BarText == BarText &&
            item.Style == Style)
        {
            return true;
        }

        return false;
    }
}
