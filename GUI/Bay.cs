using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using MaxRumsey.OzStripsPlugin.GUI.Controls;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.DTO.XML;
using MaxRumsey.OzStripsPlugin.GUI.Properties;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using Microsoft.SqlServer.Server;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// A bay.
/// </summary>
public class Bay : System.IDisposable
{
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
    /// <param name="containsCDMDisplay">Whether or not CDM stats are displayed here.</param>
    /// <param name="totalNumBays">The total number of bays that will exist post-layout.</param>
    public Bay(List<StripBay> bays, BayManager bayManager, SocketConn socketConn, string name, int vertBoardNum, bool containsCDMDisplay, int totalNumBays)
    {
        BayTypes = bays;
        _bayManager = bayManager;
        BayManager = bayManager;
        _socketConnection = socketConn;
        Name = name;
        VerticalBoardNumber = vertBoardNum;
        ChildPanel = new(bayManager, name, this, containsCDMDisplay, socketConn);

        _bayRenderController = new BayRenderController(this);

        _bayRenderController.Setup();

        bayManager.BayRepository.AddBay(this, vertBoardNum, totalNumBays);
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
    /// Gets the scaled height of a strip.
    /// </summary>
    /// <returns>Height (px).</returns>
    public static int GetStripHeight() => (int)(BayRenderController.StripHeight * BayRenderController.Scale);

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
            if (item.Strip == controller)
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
            Strips.RemoveAll(item => item.Strip == controller);
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
        foreach (var strip in Strips.Where(x => x.Strip is not null))
        {
            RemoveStrip(strip.Strip!, false);
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
            Strip = stripController,
            Type = StripItemType.STRIP,
            RenderedStripItem = new StripView(stripController, _bayRenderController),
        };

        try
        {
            if (BayTypes.Contains(StripBay.BAY_PREA) && OzStripsSettings.Default.AlphaSortPrea)
            {
                var abovetheBar = new List<StripListItem>() { strip };
                for (var i = Strips.Count - 1; i >= 0; i--)
                {
                    if (Strips[i].Type == StripItemType.STRIP && Strips[i].Strip is not null)
                    {
                        abovetheBar.Add(Strips[i]);
                        Strips.Remove(Strips[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                abovetheBar = [.. abovetheBar.OrderByDescending(x => x.Strip!.FDR.Callsign)];

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
                s.Strip?.UpdateFDR();
            }
        }

        _bayRenderController.Redraw();
    }

    /// <summary>
    /// Resizes the bay.
    /// </summary>
    public void ResizeBay()
    {
        _bayRenderController.SetHeight();
        ChildPanel.ConfigureScroll();
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

        ClientInitiatedStripMove(item, newPosition);
        ResizeBay();

        BayItem? argument = null;
        if (newPosition != 0)
        {
            argument = Strips[newPosition - 1].ToBayChangeArgument();
        }

        _socketConnection.SyncBay(new()
        {
            Type = BayChange.BayChangeTypes.MOVE_ELEMENT_BELOW,
            BayType = BayTypes[0],
            TargetItem = item.ToBayChangeArgument(),
            ArgumentItem = argument,
        });
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

        ClientInitiatedStripMove(item, abspos);
        ResizeBay();

        BayItem? argument = null;
        if (abspos != 0)
        {
            argument = Strips[abspos - 1].ToBayChangeArgument();
        }

        _socketConnection.SyncBay(new()
        {
            Type = BayChange.BayChangeTypes.MOVE_ELEMENT_BELOW,
            BayType = BayTypes[0],
            TargetItem = item.ToBayChangeArgument(),
            ArgumentItem = argument,
        });
    }

    /// <summary>
    /// Moves strip to new position, when iniated by the user, and sends a CDM update if relevant.
    /// </summary>
    /// <param name="item">Strip item.</param>
    /// <param name="index">Index to move it to.</param>
    public void ClientInitiatedStripMove(StripListItem item, int index)
    {
        Strips.Remove(item);
        Strips.Insert(index, item);

        if (BayTypes.Contains(StripBay.BAY_CLEARED) && item.Strip is not null)
        {
            var queueBarIndex = Strips.FindIndex(a => a.Type == StripItemType.QUEUEBAR);
            if (queueBarIndex != -1 && index < queueBarIndex)
            {
                // send CDM update
                _socketConnection.SendCDMUpdate(item.Strip, CDMState.ACTIVE);
            }
        }
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
            OnBarsChanged?.Invoke(this, EventArgs.Empty);

            if (sync)
            {
                _socketConnection.SyncBay(new()
                {
                    Type = BayChange.BayChangeTypes.ADD_BAR,
                    BayType = BayTypes[0],
                    TargetItem = bar.ToBayChangeArgument(),
                });
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
            _socketConnection.SyncBay(new()
            {
                Type = BayChange.BayChangeTypes.DELETE_BAR,
                BayType = BayTypes[0],
                TargetItem = bar.ToBayChangeArgument(),
            });
        }

        OnBarsChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Gets the requested bay height.
    /// </summary>
    /// <returns>Height (px).</returns>
    public int GetRequestedHeight() => (int)(_bayRenderController.GetHeightPreScale() * BayRenderController.Scale);

    /// <summary>
    /// Gets the strip position for a specific strip.
    /// </summary>
    /// <param name="strip">Selected strip.</param>
    /// <returns>Position (px).</returns>
    public int GetStripPosition(Strip strip) => (int)(_bayRenderController.GetStripPosPreScale(strip) * BayRenderController.Scale);

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
            if (currentItem is not null)
            {
                _socketConnection.SyncBay(new()
                {
                    Type = BayChange.BayChangeTypes.DELETE_BAR,
                    BayType = BayTypes[0],
                    TargetItem = currentItem.ToBayChangeArgument(),
                });
            }
            else
            {
                _socketConnection.SyncBay(new()
                {
                    Type = BayChange.BayChangeTypes.ADD_BAR,
                    BayType = BayTypes[0],
                    TargetItem = Strips[0].ToBayChangeArgument(),
                });
            }
        }

        OnBarsChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Queue the current item up.
    /// </summary>
    public void QueueUp()
    {
        if (_bayManager.PickedStrip != null && OwnsStrip(_bayManager.PickedStrip))
        {
            AddDivider(true, false);
            var item = Strips.Find(a => a?.Strip == _bayManager.PickedStrip);
            ChangeStripPositionAbs(item, DivPosition);
            _bayManager.RemovePicked(true);
        }
    }

    /// <summary>
    /// Gets if available a list item by strip name. Will create or delete a bar if needed.
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
        }

        if (code == "\a" && returnedItem == null)
        {
            AddDivider(true, false);
            OnBarsChanged?.Invoke(this, EventArgs.Empty);

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
            OnBarsChanged?.Invoke(this, EventArgs.Empty);

            return GetListItemByStr(code);
        }

        return returnedItem;
    }

    /// <summary>
    /// Gets if available a list item by bay item. Will create or delete a bar if needed.
    /// </summary>
    /// <param name="bayItem">The bayItem..</param>
    /// <returns>The list item if there is a match, otherwise null.</returns>
    public StripListItem? GetListItemByBayItem(BayItem bayItem)
    {
        StripListItem? returnedItem = null;

        if (bayItem.IsStrip && bayItem.StripKey is not null)
        {
            returnedItem = Strips.FirstOrDefault(x => x.Type == StripItemType.STRIP && x.Strip!.StripKey.Matches(bayItem.StripKey));
        }
        else
        {
            returnedItem = GetListItemByStr(bayItem.BarIdentifier ?? string.Empty);
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
            if (stripListItem.Type == StripItemType.STRIP && stripListItem.Strip == strip)
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
        ChildPanel.Dispose();
        _bayRenderController.Dispose();
    }

    /// <summary>
    /// Called when bars may be added or removed.
    /// </summary>
    public event EventHandler? OnBarsChanged;

    /// <summary>
    /// Called when this bay's bars may have been changed externally, and the bay needs to update accordingly.
    /// </summary>
    public void BarsChangedExternally()
    {
        OnBarsChanged?.Invoke(this, EventArgs.Empty);
    }
}
