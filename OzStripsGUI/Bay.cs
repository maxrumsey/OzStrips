using System.Collections.Generic;
using System.Linq;

using MaxRumsey.OzStripsPlugin.Gui.Controls;
using MaxRumsey.OzStripsPlugin.Gui.DTO;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// A bay.
/// </summary>
public class Bay
{
    private readonly BayManager _bayManager;
    private readonly SocketConn _socketConnection;

    /// <summary>
    /// Initializes a new instance of the <see cref="Bay"/> class.
    /// </summary>
    /// <param name="bays">The list of bays.</param>
    /// <param name="bayManager">The bay manager.</param>
    /// <param name="socketConn">The socket connection.</param>
    /// <param name="name">The name of the bay.</param>
    /// <param name="vertBoardNum">The vertical board number.</param>
    public Bay(List<StripBay> bays, BayManager bayManager, SocketConn socketConn, string name, int vertBoardNum)
    {
        BayTypes = bays;
        _bayManager = bayManager;
        _socketConnection = socketConn;
        Name = name;
        VerticalBoardNumber = vertBoardNum;
        ChildPanel = new(bayManager, name, this);

        bayManager.AddBay(this, vertBoardNum);
    }

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
    /// Gets a list of strips.
    /// </summary>
    public List<StripListItem> Strips { get; } = [];

    /// <summary>
    /// Gets a list of bay types.
    /// </summary>
    public List<StripBay> BayTypes { get; }

    /// <summary>
    /// Gets the div position.
    /// </summary>
    public int DivPosition
    {
        get
        {
            var val = 0;
            for (var i = 0; i < Strips.Count; i++)
            {
                if (Strips[i].Type == StripItemType.QUEUEBAR)
                {
                    val = i;
                }
            }

            return val;
        }
    }

    /// <summary>
    /// Converts the Bay into a Bay Transfer object.
    /// </summary>
    /// <param name="bay">The bay to convert.</param>
    public static implicit operator BayDTO(Bay bay)
    {
        var bayDTO = new BayDTO { bay = bay.BayTypes[0] };
        var childList = new List<string>();
        foreach (var item in bay.Strips)
        {
            switch (item.Type)
            {
                case StripItemType.STRIP when item.StripController is not null:
                    childList.Add(item.StripController.FDR.Callsign);
                    break;
                case StripItemType.QUEUEBAR:
                    childList.Add("\a"); // indicates q-bar
                    break;
            }
        }

        bayDTO.list = childList;
        return bayDTO;
    }

    /// <summary>
    /// The number of queues items.
    /// </summary>
    /// <returns>The number.</returns>
    public int CountQueued()
    {
        var count = 0;

        foreach (var item in Strips)
        {
            switch (item.Type)
            {
                case StripItemType.QUEUEBAR:
                    return count;
                case StripItemType.STRIP:
                    count++;
                    break;
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets if this bay is responsible for the bay strip.
    /// </summary>
    /// <param name="bay">The bay.</param>
    /// <returns>True of it is responsible, false otherwise.</returns>
    public bool ResponsibleFor(StripBay bay)
    {
        return BayTypes.Contains(bay);
    }

    /// <summary>
    /// Gets if this bay owns the strip.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <returns>True if it owns the strip, false otherwise.</returns>
    public bool OwnsStrip(StripController controller)
    {
        var found = false;
        foreach (var item in Strips)
        {
            if (item.StripController == controller)
            {
                found = true;
            }
        }

        return found;
    }

    /// <summary>
    /// Remove the strip if indicated, otherwise orders the strips.
    /// </summary>
    /// <param name="controller">The controller to remove.</param>
    /// <param name="remove">If the controller should be removed or not.</param>
    public void RemoveStrip(StripController controller, bool remove)
    {
        if (remove)
        {
            Strips.RemoveAll(item => item.StripController == controller);
        }

        Orderstrips();
    }

    /// <summary>
    /// Removes the specified strip.
    /// </summary>
    /// <param name="controller">The controller.</param>
    public void RemoveStrip(StripController controller)
    {
        RemoveStrip(controller, true);
    }

    /// <summary>
    /// Wipes the strips.
    /// </summary>
    public void WipeStrips()
    {
        foreach (var strip in Strips.Where(x => x.StripController is not null))
        {
            RemoveStrip(strip.StripController!, false);
        }

        Strips.Clear();
        Orderstrips();
    }

    /// <summary>
    /// Adds the strip to the bay.
    /// </summary>
    /// <param name="stripController">The strip.</param>
    /// <remarks>todo: check for dupes.</remarks>
    public void AddStrip(StripController stripController)
    {
        var strip = new StripListItem
        {
            StripController = stripController,
            Type = StripItemType.STRIP,
        };

        Strips.Add(strip); // todo: add control action
        Orderstrips();
    }

    /// <summary>
    /// Forces a rerender of all the strips.
    /// </summary>
    public void ForceRerender()
    {
        var stripList = new StripListItem[Strips.Count];
        Strips.CopyTo(stripList);

        foreach (var s in stripList)
        {
            if (s.Type == StripItemType.STRIP)
            {
                s.StripController?.UpdateFDR();
            }
        }
    }

    /// <summary>
    /// Orders the strips.
    /// </summary>
    public void Orderstrips()
    {
        ChildPanel.ChildPanel.SuspendLayout();
        ChildPanel.ChildPanel.Controls.Clear();
        var queueLen = CountQueued();
        foreach (var strip in Strips)
        {
            switch (strip.Type)
            {
                case StripItemType.STRIP when strip.StripController?.StripHolderControl != null:
                    ChildPanel.ChildPanel.Controls.Add(strip.StripController!.StripHolderControl);
                    break;
                case StripItemType.QUEUEBAR when strip.DividerBarControl is not null:
                    ChildPanel.ChildPanel.Controls.Add(strip.DividerBarControl);
                    strip.DividerBarControl?.SetVal(queueLen);
                    strip.DividerBarControl?.ReloadSize();
                    break;
            }
        }

        ChildPanel.ChildPanel.ResumeLayout();
    }

    /// <summary>
    /// Changes a strip position.
    /// </summary>
    /// <param name="stripController">The strip controller.</param>
    /// <param name="relativePosition">The relative position.</param>
    public void ChangeStripPosition(StripController stripController, int relativePosition)
    {
        var originalPosition = Strips.FindIndex(a => a.StripController == stripController);
        var stripItem = Strips[originalPosition];
        var newPosition = originalPosition + relativePosition;

        if (newPosition < 0 || newPosition > Strips.Count - 1)
        {
            return;
        }

        Strips.RemoveAt(originalPosition);
        Strips.Insert(newPosition, stripItem);
        Orderstrips();
        _socketConnection.SyncBay(this);
    }

    /// <summary>
    /// Changes the strip position.
    /// </summary>
    /// <param name="item">The strip item to change.</param>
    /// <param name="abspos">The new position.</param>
    public void ChangeStripPositionAbs(StripListItem item, int abspos)
    {
        if (abspos < 0 || abspos > Strips.Count - 1)
        {
            return;
        }

        Strips.Remove(item);
        Strips.Insert(abspos, item);
        Orderstrips();
        _socketConnection.SyncBay(this);
    }

    /// <summary>
    /// Adds a new divider.
    /// </summary>
    /// <param name="force">If the division should be forced.</param>
    /// <param name="sync">If to sync the changes to the server.</param>
    public void AddDivider(bool? force, bool sync = true)
    {
        var containsDiv = false;
        StripListItem? currentItem = null;
        foreach (var item in Strips)
        {
            if (item.Type == StripItemType.QUEUEBAR)
            {
                containsDiv = true;
                currentItem = item;
            }
        }

        if (!containsDiv)
        {
            var newItem = new StripListItem
            {
                Type = StripItemType.QUEUEBAR,
                DividerBarControl = new(),
            };
            Strips.Insert(0, newItem);
        }
        else if (currentItem is not null && force is null or false)
        {
            Strips.Remove(currentItem);
        }

        Orderstrips();
        if (sync)
        {
            _socketConnection.SyncBay(this);
        }
    }

    /// <summary>
    /// Queue the current item up.
    /// </summary>
    public void QueueUp()
    {
        if (_bayManager.PickedController != null && OwnsStrip(_bayManager.PickedController))
        {
            AddDivider(true, false);
            var item = Strips.Find(a => a?.StripController == _bayManager.PickedController);
            ChangeStripPositionAbs(item, DivPosition);
            _bayManager.SetPicked(true);
            _socketConnection.SyncBay(this);
        }
    }

    /// <summary>
    /// Gets if available a list item by strip name.
    /// </summary>
    /// <param name="code">The code of the item.</param>
    /// <returns>The list item if there is a match, otherwise null.</returns>
    public StripListItem? GetListItemByStr(string code)
    {
        StripListItem? returnedItem = null;
        foreach (var stripListItem in Strips)
        {
            if (code == "\a" && stripListItem.Type == StripItemType.QUEUEBAR)
            {
                returnedItem = stripListItem;
            }
            else if (stripListItem.Type == StripItemType.STRIP && stripListItem.StripController?.FDR.Callsign == code)
            {
                returnedItem = stripListItem;
            }
        }

        if (code == "\a" && returnedItem == null)
        {
            AddDivider(true, false);
            return GetListItemByStr(code);
        }

        return returnedItem;
    }
}
