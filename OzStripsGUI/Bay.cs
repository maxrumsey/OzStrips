using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using MaxRumsey.OzStripsPlugin.Gui.Controls;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using Microsoft.SqlServer.Server;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// A bay.
/// </summary>
public class Bay : System.IDisposable
{
    private const bool ALPHASORT = false;
    private readonly BayManager _bayManager;
    private readonly SocketConn _socketConnection;
    private readonly BayRenderController _bayRenderController;

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
        BayManager = bayManager;
        _socketConnection = socketConn;
        Name = name;
        VerticalBoardNumber = vertBoardNum;
        ChildPanel = new(bayManager, name, this);

        _bayRenderController = new BayRenderController(this);

        _bayRenderController.Setup();

        bayManager.BayRepository.AddBay(this, vertBoardNum);
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
    /// Gets the current bay manager.
    /// </summary>
    public BayManager BayManager { get; }

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
                case StripItemType.BAR:
                    childList.Add($"\a{item.Style}{item.BarText}");
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
    public bool OwnsStrip(Strip controller)
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
    public void RemoveStrip(Strip controller, bool remove)
    {
        if (remove)
        {
            Strips.RemoveAll(item => item.StripController == controller);
        }

        ResizeBay();
    }

    /// <summary>
    /// Removes the specified strip.
    /// </summary>
    /// <param name="controller">The controller.</param>
    public void RemoveStrip(Strip controller)
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
        ResizeBay();
    }

    /// <summary>
    /// Adds the strip to the bay.
    /// </summary>
    /// <param name="stripController">The strip.</param>
    /// <param name="inhibitreorders">Whether or not to inhibit reorders.</param>
    /// <remarks>todo: check for dupes.</remarks>
    public void AddStrip(Strip stripController, bool inhibitreorders)
    {
        var strip = new StripListItem
        {
            StripController = stripController,
            Type = StripItemType.STRIP,
            RenderedStripItem = new StripView(stripController, _bayRenderController),
        };

        try
        {
            if (BayTypes.Contains(StripBay.BAY_PREA) && ALPHASORT)
            {
                var abovetheBar = new List<StripListItem>() { strip };
                for (var i = Strips.Count - 1; i >= 0; i--)
                {
                    if (Strips[i].Type == StripItemType.STRIP && Strips[i].StripController is not null)
                    {
                        abovetheBar.Add(Strips[i]);
                        Strips.Remove(Strips[i]);
                    }
                    else
                    {
                        break;
                    }
                }

    #pragma warning disable CS8602 // Dereference of a possibly null reference.
                abovetheBar = abovetheBar.OrderByDescending(x => x.StripController.FDR.Callsign).ToList();
    #pragma warning restore CS8602 // Dereference of a possibly null reference.

                Strips.AddRange(abovetheBar);
            }
            else
            {
                Strips.Add(strip);
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex);

            Strips.Add(strip);
        }

        if (!inhibitreorders)
        {
            ResizeBay();
        }
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

        _bayRenderController.Redraw();
    }

    /// <summary>
    /// Orders the strips.
    /// </summary>
    public void ResizeBay()
    {
        _bayRenderController.SetHeight();
    }

    /// <summary>
    /// Changes a strip position.
    /// </summary>
    /// <param name="item">The strip.</param>
    /// <param name="relativePosition">The relative position.</param>
    public void ChangeStripPosition(StripListItem item, int relativePosition)
    {
        var originalPosition = Strips.FindIndex(a => a == item);
        var stripItem = Strips[originalPosition];
        var newPosition = originalPosition + relativePosition;

        if (newPosition < 0 || newPosition > Strips.Count - 1)
        {
            return;
        }

        Strips.RemoveAt(originalPosition);
        Strips.Insert(newPosition, stripItem);
        ResizeBay();
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
        ResizeBay();
        _socketConnection.SyncBay(this);
    }

    /// <summary>
    /// Creates and adds the specified bar.
    /// </summary>
    /// <param name="type">Bartype.</param>
    /// <param name="text">Bar text.</param>
    /// <param name="sync">Whether or not to sync bar to server.</param>
    public void AddBar(int type, string text, bool sync = true)
    {
        var bar = new StripListItem()
        {
            Type = StripItemType.BAR,
            BarText = text,
            RenderedStripItem = new BarView(_bayRenderController),
            Style = type,
        };

        ((BarView)bar.RenderedStripItem).Item = bar;
        var found = false;

        foreach (var item in Strips)
        {
            if (item.Matches(bar))
            {
                found = true;
            }
        }

        if (!found)
        {
            Strips.Add(bar);
            ResizeBay();

            if (sync)
            {
                _socketConnection.SyncBay(this);
            }
        }

        BayManager.BayRepository.ResizeStripBays();
    }

    /// <summary>
    /// Creates and adds the specified bar.
    /// </summary>
    /// <param name="bar">The bar.</param>
    /// <param name="sync">Whether or not to sync the bar to server.</param>
    public void DeleteBar(StripListItem bar, bool sync = true)
    {
        Strips.Remove(bar);

        ResizeBay();

        if (sync)
        {
            _socketConnection.SyncBay(this);
        }
    }

    /// <summary>
    /// Gets the requested bay height.
    /// </summary>
    /// <returns>Height (px).</returns>
    public int GetRequestedHeight() => (int)(_bayRenderController.GetHeightPreScale() * BayRenderController.Scale);

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
                RenderedStripItem = new BarView(_bayRenderController),
                BarText = "Queue (0)",
            };
            ((BarView)newItem.RenderedStripItem).Item = newItem;
            Strips.Insert(0, newItem);
        }
        else if (currentItem is not null && force is null or false)
        {
            Strips.Remove(currentItem);
        }

        ResizeBay();
        BayManager.BayRepository.ResizeStripBays();
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
            _bayManager.RemovePicked(true);
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
            else if (code[0] == '\a' && stripListItem.Type == StripItemType.BAR && stripListItem.BarText == code.Substring(Math.Min(2, code.Length)))
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
        else if (code[0] == '\a' && returnedItem is null)
        {
            var res = int.TryParse(code[1].ToString(), out var x);
            if (!res)
            {
                x = 1;
            }

            AddBar(x, code.Substring(2), false);
            return GetListItemByStr(code);
        }

        return returnedItem;
    }

    /// <summary>
    /// To string method.
    /// </summary>
    /// <returns>Description.</returns>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Gets if available a list item by strip.
    /// </summary>
    /// <param name="strip">The strip.</param>
    /// <returns>The list item if there is a match, otherwise null.</returns>
    public StripListItem? GetListItem(Strip strip)
    {
        StripListItem? returnedItem = null;
        foreach (var stripListItem in Strips)
        {
            if (stripListItem.Type == StripItemType.STRIP && stripListItem.StripController == strip)
            {
                returnedItem = stripListItem;
            }
        }

        return returnedItem;
    }

    /// <summary>
    /// Dispose.
    /// </summary>
    public void Dispose()
    {
        throw new System.NotImplementedException();
    }
}
