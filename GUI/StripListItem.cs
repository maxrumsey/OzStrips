using MaxRumsey.OzStripsPlugin.GUI.Controls;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// A strip list item.
/// </summary>
public class StripListItem
{
    /// <summary>
    /// Gets or sets the strip controller.
    /// </summary>
    public Strip? Strip { get; set; }

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
            return Strip?.ToString() ?? string.Empty;
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
        if (item.Strip == Strip &&
            item.Type == Type &&
            item.BarText == BarText &&
            item.Style == Style)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Converts the strip item into a bay change argument.
    /// </summary>
    /// <returns>Bay change argument.</returns>
    public BayChangeStripItem ToBayChangeArgument()
    {
        var barid = "\a";
        if (Type == StripItemType.BAR)
        {
            barid += $"{Style}{BarText}";
        }

        return new()
        {
            IsStrip = Type == StripItemType.STRIP,
            StripKey = Strip?.StripKey,
            BarIdentifier = Type != StripItemType.STRIP ? barid : null,
        };
    }
}
