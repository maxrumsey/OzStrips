using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.Properties;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// Holds all individual bays.
/// </summary>
public class BayRepository(FlowLayoutPanel main, BayManager sender)
{
    private readonly List<FlowLayoutPanel> _flpVerticalBoards = [];

    private readonly BayManager _bayManager = sender;

    private Action<object, EventArgs>? _currentLayout;

    private int _currentLayoutIndex;

    private int _bayAmount;

    /// <summary>
    /// Gets the number of total bays possible.
    /// </summary>
    public int BayNum
    {
        get => Bays.Count;
    }

    /// <summary>
    /// Gets the list of bays.
    /// </summary>
    public List<Bay> Bays { get; } = [];

    /// <summary>
    /// Gets a value indicating whether the FLPs have been initalised.
    /// </summary>
    public bool Initialised => _flpVerticalBoards.Count > 0;

    /// <summary>
    /// Updates the bay based on the bay data.
    /// </summary>
    /// <param name="bayDTO">The bay data.</param>
    public void UpdateOrder(BayDTO bayDTO)
    {
        try
        {
            Bay? bay = null;
            foreach (var currentBay in Bays)
            {
                if (currentBay.BayTypes.Contains(bayDTO.bay))
                {
                    bay = currentBay;
                }
            }

            if (bay == null)
            {
                return;
            }

            var list = new List<StripListItem>();

            foreach (var dtoItem in bayDTO.list)
            {
                var listItem = bay.GetListItemByStr(dtoItem);
                if (listItem != null)
                {
                    list.Add(listItem);
                }
            }

            // incase of dodgy timing
            foreach (var oldListItem in bay.Strips)
            {
                if (!list.Contains(oldListItem) && oldListItem.Type == StripItemType.STRIP)
                {
                    list.Add(oldListItem);
                }
            }

            bay.Strips.Clear();
            bay.Strips.AddRange(list);
            bay.ResizeBay();
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Deletes the specified strip.
    /// </summary>
    /// <param name="strip">The strip to delete.</param>
    public void DeleteStrip(Strip strip)
    {
        // todo: add force delete server flag.

        /*
         * Don't send delete messages for deps incase they log back in.
         */
        if (strip.DefaultStripType == StripType.ARRIVAL)
        {
            strip.SendDeleteMessage();
        }

        FindBay(strip)?.RemoveStrip(strip, true);
        _bayManager.StripRepository.RemoveStrip(strip);
    }

    /// <summary>
    /// Finds the specified bay.
    /// </summary>
    /// <param name="stripController">The strip.</param>
    /// <returns>The bay if the name matches.</returns>
    public Bay? FindBay(Strip stripController)
    {
        foreach (var bay in Bays)
        {
            if (bay.OwnsStrip(stripController))
            {
                return bay;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds the specified bay.
    /// </summary>
    /// <param name="item">The strip.</param>
    /// <returns>The bay if the name matches.</returns>
    public Bay? FindBay(StripListItem item)
    {
        foreach (var bay in Bays)
        {
            if (bay.Strips.Contains(item))
            {
                return bay;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds the bay to the vertical board.
    /// </summary>
    /// <param name="bay">The bay.</param>
    /// <param name="verticalBoardNumber">The desired vertical board number.</param>
    public void AddBay(Bay bay, int verticalBoardNumber)
    {
        if (verticalBoardNumber >= _flpVerticalBoards.Count)
        {
            verticalBoardNumber = _flpVerticalBoards.Count - 1;
        }

        if (verticalBoardNumber < 0)
        {
            Util.LogError(new InvalidOperationException("No vertical board flow layout panels exist"));
            return;
        }

        /*
         * If less than 3 cols are present,
         * lay out left-right, top to bottom.
         */
        if (_currentLayoutIndex != 3)
        {
            var bayNums = _flpVerticalBoards.Select(x => x.Controls.Count).ToArray();

            if (bayNums is null)
            {
                throw new Exception("BayNums was null.");
            }

            var index = Array.IndexOf(bayNums, bayNums.Min());

            if (index < 0)
            {
                index = 0;
            }

            verticalBoardNumber = index;
        }

        bay.VerticalBoardNumber = verticalBoardNumber;

        Bays.Add(bay);
        _bayAmount++;
        _flpVerticalBoards[verticalBoardNumber].Controls.Add(bay.ChildPanel);
    }

    /// <summary>
    /// Reloads the strips. Called when stripboard layout is changed.
    /// </summary>
    /// <param name="socketConn">Socket connection.</param>
    public void ReloadStrips(SocketConn socketConn)
    {
        try
        {
            foreach (var strip in _bayManager.StripRepository.Strips)
            {
                foreach (var bay in Bays)
                {
                    if (bay.OwnsStrip(strip))
                    {
                        bay.RemoveStrip(strip);
                    }
                }

                _bayManager.AddStrip(strip, false, true);
            }

            foreach (var bay in Bays)
            {
                bay.ResizeBay();
            }

            socketConn.ReadyForBayData(true);

            ResizeStripBays();
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Sets the current resize mode layout function.
    /// </summary>
    /// <param name="func">Layout function.</param>
    public void SetLayout(Action<object, EventArgs> func)
    {
        _currentLayout = func;
    }

    /// <summary>
    /// Resizes the control.
    /// </summary>
    /// <param name="triggerRelayout">Whether or not to force a relayout of bays.</param>
    public void ConfigureAndSizeFLPs(bool triggerRelayout = false)
    {
        if (main == null)
        {
            return;
        }

        if (triggerRelayout)
        {
            _currentLayout?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (_currentLayoutIndex == 0)
        {
            AddVertBoard();
            AddVertBoard();
            AddVertBoard();
            _currentLayoutIndex = 3;
            _currentLayout?.Invoke(this, EventArgs.Empty);
        }

        // Places a vertical scroll bar if needed.
        ResizeStripBays();

        var y_main = main.Size.Height;

        if (main.Size.Width <= 840 && _currentLayoutIndex != 1)
        {
            ClearVertBoards();
            AddVertBoard();
            _currentLayoutIndex = 1;
            _currentLayout?.Invoke(this, EventArgs.Empty);
            return;
        }
        else if (main.Size.Width > 840 && main.Size.Width <= 1250 && _currentLayoutIndex != 2)
        {
            ClearVertBoards();
            AddVertBoard();
            AddVertBoard();
            _currentLayoutIndex = 2;
            _currentLayout?.Invoke(this, EventArgs.Empty);
            return;
        }
        else if (main.Size.Width > 1250 && _currentLayoutIndex != 3)
        {
            ClearVertBoards();
            AddVertBoard();
            AddVertBoard();
            AddVertBoard();
            _currentLayoutIndex = 3;
            _currentLayout?.Invoke(this, EventArgs.Empty);
            return;
        }

        // See if we need a horizontal scroll bar.
        main.PerformLayout();

        var vertScrollVisible = main.VerticalScroll.Visible;

        var x_each = (main.Size.Width - (vertScrollVisible ? 17 : 0)) / _currentLayoutIndex;

        foreach (var panel in _flpVerticalBoards)
        {
            panel.Size = new(x_each, y_main);
            panel.Margin = default;
            panel.Padding = new(2);
            panel.ResumeLayout();
        }

        ResizeStripBays(); // prevents issues from scroll bars appearing. TODO: fix this properly
    }

    /// <summary>
    /// Resizes only the strip bays.
    /// </summary>
    public void ResizeStripBays()
    {
        // If no FLPs exist
        if (_currentLayoutIndex == 0)
        {
            return;
        }

        var smartResize = OzStripsSettings.Default.SmartResize >= _currentLayoutIndex;

        var yMain = main.Size.Height;
        var xEach = (main.Size.Width - (main.VerticalScroll.Visible ? 17 : 0)) / _currentLayoutIndex;

        var allocatedSpace = new int[_currentLayoutIndex];

        foreach (var bay in Bays)
        {
            bay.ChildPanel.SuspendLayout();
            var childnum = _flpVerticalBoards[bay.VerticalBoardNumber].Controls.Count;

            if (childnum == 0)
            {
                return;
            }

            var height = (yMain - 4) / childnum;

            var minBayHeight = smartResize ? 70 : 200;

            // if height less than this, set to this.
            if (height < minBayHeight)
            {
                height = minBayHeight;
            }

            var reqheight = bay.GetRequestedHeight();

            if (smartResize && reqheight > 0)
            {
                // 36 = header height.
                height = 36 + reqheight;

                // taking up more than half the FLP height? cut it down.
                // rest of space will be allocated during additional space allocation.
                if (height > (yMain - 4) / 2)
                {
                    height = (yMain - 4) / 2;
                }
            }
            else if (smartResize)
            {
                height = 70;
            }

            allocatedSpace[bay.VerticalBoardNumber] += height;
            bay.ChildPanel.Size = new(xEach - 4, height);
        }

        var max = allocatedSpace.Max();

        /*
         * To avoid ugly situations where one FLP is much larger than the others, reallocate the remaining space.
         */
        for (var i = 0; i < _currentLayoutIndex; i++)
        {
            // Remaining space allowed to allocate to ensure uniformity.
            var remaining = (max > yMain ? max : yMain) - 4 - allocatedSpace[i];

            // If we have overallocated? or no bays exist.
            if (remaining <= 0 || _flpVerticalBoards[i].Controls.Count == 0)
            {
                continue;
            }

            var each = remaining / _flpVerticalBoards[i].Controls.Count;

            // Divvy up remaining space.
            foreach (var bay in Bays.Where(x => x.VerticalBoardNumber == i))
            {
                bay.ChildPanel.Size = new System.Drawing.Size(xEach - 4, bay.ChildPanel.Size.Height + each);
                remaining -= each;
            }

            var last_bay = Bays.Find(x => x.VerticalBoardNumber == i);

            if (last_bay is not null)
            {
                last_bay.ChildPanel.Size = new System.Drawing.Size(xEach - 4, last_bay.ChildPanel.Size.Height + remaining);
            }
        }

        foreach (var bay in Bays)
        {
            bay.ChildPanel.ResumeLayout();
        }

        foreach (var bay in Bays)
        {
            bay.ChildPanel.ConfigureScroll();
        }
    }

    /// <summary>
    /// Wipes the bays.
    /// </summary>
    public void WipeBays()
    {
        _bayAmount = 0;

        foreach (var bay in Bays)
        {
            bay.Dispose();
        }

        Bays.Clear();
        foreach (var flpVerticalBoard in _flpVerticalBoards)
        {
            flpVerticalBoard.SuspendLayout();
            flpVerticalBoard.Controls.Clear();
        }
    }

    /// <summary>
    /// Clears all vertical boards.
    /// </summary>
    private void ClearVertBoards()
    {
        main.Controls.Clear();
        _flpVerticalBoards.Clear();
    }

    /// <summary>
    /// Adds a vertical board.
    /// </summary>
    private void AddVertBoard()
    {
        var flp = new FlowLayoutPanel
        {
            AutoScroll = false,
            Margin = new(0),
            Padding = new(0),
            Size = new(100, 100),
            Location = new(0, 0),
            AutoSize = true,
            WrapContents = false,
            FlowDirection = FlowDirection.TopDown,
        };

        main.Controls.Add(flp);
        _flpVerticalBoards.Add(flp);
    }
}
