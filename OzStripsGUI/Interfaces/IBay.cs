using System.Collections.Generic;
using System.Linq;

using MaxRumsey.OzStripsPlugin.Gui.Controls;
using MaxRumsey.OzStripsPlugin.Gui.DTO;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// A bay.
/// </summary>
public interface IBay
{
    /// <summary>
    /// Gets or sets the vertical board number.
    /// </summary>
    public int VerticalBoardNumber { get; set; }

    /// <summary>
    /// Gets the bay child panel.
    /// </summary>
    public BayControl ChildPanel { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets a list of strips.
    /// </summary>
    public List<StripListItem> Strips { get; set; }

    /// <summary>
    /// Gets a list of bay types.
    /// </summary>
    public List<StripBay> BayTypes { get; }

    /// <summary>
    /// Gets the div position.
    /// </summary>
    public int DivPosition { get; }

    /// <summary>
    /// The number of queues items.
    /// </summary>
    /// <returns>The number.</returns>
    public int CountQueued();

    /// <summary>
    /// Gets if this bay is responsible for the bay strip.
    /// </summary>
    /// <param name="bay">The bay.</param>
    /// <returns>True of it is responsible, false otherwise.</returns>
    public bool ResponsibleFor(StripBay bay);

    /// <summary>
    /// Gets if this bay owns the strip.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <returns>True if it owns the strip, false otherwise.</returns>
    public bool OwnsStrip(IStripController controller);

    /// <summary>
    /// Remove the strip if indicated, otherwise orders the strips.
    /// </summary>
    /// <param name="controller">The controller to remove.</param>
    /// <param name="remove">If the controller should be removed or not.</param>
    public void RemoveStrip(IStripController controller, bool remove);

    /// <summary>
    /// Removes the specified strip.
    /// </summary>
    /// <param name="controller">The controller.</param>
    public void RemoveStrip(IStripController controller);

    /// <summary>
    /// Wipes the strips.
    /// </summary>
    public void WipeStrips();

    /// <summary>
    /// Adds the strip to the bay.
    /// </summary>
    /// <param name="stripController">The strip.</param>
    /// <param name="inhibitreorders">Whether or not to inhibit reorders.</param>
    /// <remarks>todo: check for dupes.</remarks>
    public void AddStrip(IStripController stripController, bool inhibitreorders);

    /// <summary>
    /// Forces a rerender of all the strips.
    /// </summary>
    public void ForceRerender();

    /// <summary>
    /// Orders the strips.
    /// </summary>
    public void Orderstrips();

    /// <summary>
    /// Changes a strip position.
    /// </summary>
    /// <param name="stripController">The strip controller.</param>
    /// <param name="relativePosition">The relative position.</param>
    public void ChangeStripPosition(IStripController stripController, int relativePosition);

    /// <summary>
    /// Changes the strip position.
    /// </summary>
    /// <param name="item">The strip item to change.</param>
    /// <param name="abspos">The new position.</param>
    public void ChangeStripPositionAbs(StripListItem item, int abspos);

    /// <summary>
    /// Adds a new divider.
    /// </summary>
    /// <param name="force">If the division should be forced.</param>
    /// <param name="sync">If to sync the changes to the server.</param>
    public void AddDivider(bool? force, bool sync = true);

    /// <summary>
    /// Queue the current item up.
    /// </summary>
    public void QueueUp();

    /// <summary>
    /// Gets if available a list item by strip name.
    /// </summary>
    /// <param name="code">The code of the item.</param>
    /// <returns>The list item if there is a match, otherwise null.</returns>
    public StripListItem? GetListItemByStr(string code);
}
