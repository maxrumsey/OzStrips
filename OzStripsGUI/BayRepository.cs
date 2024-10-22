using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using MaxRumsey.OzStripsPlugin.Gui.Properties;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Holds all individual bays.
/// </summary>
public class BayRepository(FlowLayoutPanel main, Action<object, EventArgs> layoutMethod, BayManager sender)
{
    private readonly List<FlowLayoutPanel> _flpVerticalBoards = [];

    private readonly BayManager _bayManager = sender;

    private Action<object, EventArgs> _currentLayout = layoutMethod;

    private int _currentLayoutIndex;

    /// <summary>
    /// Gets or sets the number of present bays.
    /// </summary>
    public int BayNum { get; set; }

    /// <summary>
    /// Gets the list of bays.
    /// </summary>
    public List<Bay> Bays { get; } = [];

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
            bay.Orderstrips();
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
        strip.SendDeleteMessage();
        FindBay(strip)?.RemoveStrip(strip, true);
        _bayManager.StripRepository.Controllers.Remove(strip);
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
    /// <param name="verticalBoardNumber">The vertical board number.</param>
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

        if (_currentLayoutIndex != 3)
        {
            var maxflpnum = BayNum / _currentLayoutIndex;
            if (_flpVerticalBoards[verticalBoardNumber].Controls.Count >= maxflpnum)
            {
                verticalBoardNumber = _currentLayoutIndex - 1;
                for (var i = 0; i < _flpVerticalBoards.Count; i++)
                {
                    if (_flpVerticalBoards[i].Controls.Count < maxflpnum)
                    {
                        verticalBoardNumber = i;
                    }
                }
            }
        }

        bay.VerticalBoardNumber = verticalBoardNumber;

        Bays.Add(bay);
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
            foreach (var strip in _bayManager.StripRepository.Controllers)
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
                bay.Orderstrips();
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
    /// <param name="triggerRelayout">Whether or not to trigger a relayout of bays.</param>
    public void Resize(bool triggerRelayout = false)
    {
        if (main == null)
        {
            return;
        }

        if (triggerRelayout)
        {
            _currentLayout(this, EventArgs.Empty);
            return;
        }

        if (_currentLayoutIndex == 0)
        {
            AddVertBoard();
            AddVertBoard();
            AddVertBoard();
            _currentLayoutIndex = 3;
            _currentLayout(this, EventArgs.Empty);
        }

        ResizeStripBays();

        var y_main = main.Size.Height;

        if (main.Size.Width <= 840 && _currentLayoutIndex != 1)
        {
            ClearVertBoards();
            AddVertBoard();
            _currentLayoutIndex = 1;
            _currentLayout(this, EventArgs.Empty);
            return;
        }
        else if (main.Size.Width > 840 && main.Size.Width <= 1250 && _currentLayoutIndex != 2)
        {
            ClearVertBoards();
            AddVertBoard();
            AddVertBoard();
            _currentLayoutIndex = 2;
            _currentLayout(this, EventArgs.Empty);
            return;
        }
        else if (main.Size.Width > 1250 && _currentLayoutIndex != 3)
        {
            ClearVertBoards();
            AddVertBoard();
            AddVertBoard();
            AddVertBoard();
            _currentLayoutIndex = 3;
            _currentLayout(this, EventArgs.Empty);
            return;
        }

        var x_each = (main.Size.Width - (main.VerticalScroll.Visible ? 17 : 0)) / _currentLayoutIndex;

        foreach (var panel in _flpVerticalBoards)
        {
            panel.Size = new(x_each, y_main);
            panel.Margin = default;
            panel.Padding = new(2);
            panel.ResumeLayout();
        }

        ResizeStripBays(); // prevents issues from scroll bars appearing.
    }

    /// <summary>
    /// Resizes only the strip bays.
    /// </summary>
    public void ResizeStripBays()
    {
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
            var height = (yMain - 4) / childnum;

            var smartResizeMaxHeight = smartResize ? 70 : 300;
            if (height < smartResizeMaxHeight)
            {
                height = smartResizeMaxHeight;
            }

            var reqheight = bay.GetRequestedHeight();

            if (smartResize && reqheight > 0)
            {
                height = 36 + reqheight;

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

        if (smartResize)
        {
            for (var i = 0; i < _currentLayoutIndex; i++)
            {
                var remaining = (max > yMain ? max : yMain) - 4 - allocatedSpace[i];

                if (remaining <= 0 || _flpVerticalBoards[i].Controls.Count == 0)
                {
                    continue;
                }

                var each = remaining / _flpVerticalBoards[i].Controls.Count;

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
        }

        foreach (var bay in Bays)
        {
            bay.ChildPanel.ResumeLayout();
        }
    }

    /// <summary>
    /// Wipes the bays.
    /// </summary>
    public void WipeBays()
    {
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
