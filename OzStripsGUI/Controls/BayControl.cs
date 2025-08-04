using System;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// A control representing a bay.
/// </summary>
public partial class BayControl : UserControl
{
    private readonly Bay _ownerBay;
    private readonly BayManager _bayManager;
    private int _stripHeight;
    private int _stripBoardHeight;

    private int? _desiredScrollAmount;
    private Strip? _pickedStrip;

    /// <summary>
    /// Initializes a new instance of the <see cref="BayControl"/> class.
    /// </summary>
    /// <param name="bm">The bay manager.</param>
    /// <param name="name">The bay name.</param>
    /// <param name="bay">The bay.</param>
    public BayControl(BayManager bm, string name, Bay bay)
    {
        InitializeComponent();
        lb_bay_name.Text = name;
        ChildPanel = pl_main;
        ChildPanel.VerticalScroll.Enabled = true;
        _bayManager = bm;
        _ownerBay = bay;

        ChildPanel.Resize += ResizeStripBoard;
        ChildPanel.Scroll += Scroll;
        ChildPanel.MouseWheel += MouseWheel;
    }

    /// <summary>
    /// Gets the child panel.
    /// </summary>
    public Panel ChildPanel { get; }

    /// <summary>
    /// Configures the scroll bar after a resize.
    /// </summary>
    public void ConfigureScroll()
    {
        // todo: run set val during this if strip is picked.
        _stripHeight = Bay.GetStripHeight();

        if (ChildPanel.Controls.Count == 0)
        {
            return;
        }

        var child = ChildPanel.Controls[0];
        var parent = ChildPanel.Parent;

        _stripBoardHeight = parent.Height - panel2.Height;

        ChildPanel.VerticalScroll.SmallChange = _stripHeight;
        ChildPanel.VerticalScroll.Maximum = _ownerBay.GetRequestedHeight();
        ChildPanel.VerticalScroll.Minimum = 0;

        if (child.Height > _stripBoardHeight)
        {
            ChildPanel.VerticalScroll.Visible = true;
        }
        else
        {
            ChildPanel.VerticalScroll.Visible = false;
        }
    }

    /// <summary>
    /// Sets the picked strip.
    /// </summary>
    /// <param name="strip">Picked strip.</param>
    public void SetPicked(Strip? strip)
    {
        if (strip is null)
        {
            _desiredScrollAmount = null;
            _pickedStrip = null;
            ChildPanel.VerticalScroll.Enabled = true;
        }
        else
        {
            _pickedStrip = strip;
            _desiredScrollAmount = GetPickedStripPosition() - ChildPanel.VerticalScroll.Value;

            if (_desiredScrollAmount < 0)
            {
                _desiredScrollAmount = 0;
            }

            if (_desiredScrollAmount > _stripBoardHeight - _stripHeight)
            {
                _desiredScrollAmount = _stripBoardHeight - _stripHeight;
            }

            SetScrollValue(0);

            ChildPanel.VerticalScroll.Enabled = false;
        }
    }

    private new void MouseWheel(object sender, MouseEventArgs e)
    {
        SetScrollValue(ChildPanel.VerticalScroll.Value);
    }

    private new void Scroll(object sender, ScrollEventArgs e)
    {
        var val = e.NewValue;

        SetScrollValue(val);
    }

    private void ResizeStripBoard(object sender, EventArgs e)
    {
        ConfigureScroll();
    }

    private void LabelBayNameClicked(object sender, EventArgs e)
    {
        _bayManager.DropStrip(_ownerBay);
    }

    private void ButtonQueueClicked(object sender, EventArgs e)
    {
        _ownerBay.QueueUp();
    }

    private void ButtonDivClicked(object sender, EventArgs e)
    {
        _ownerBay.AddDivider(false);
    }

    private void SetScrollValue(int val)
    {
        if (_pickedStrip is not null && _desiredScrollAmount is not null)
        {
            val = GetPickedStripPosition() - (int)_desiredScrollAmount;
        }

        if (val < 0)
        {
            val = 0;
        }

        ChildPanel.VerticalScroll.Value = val;
    }

    private int GetPickedStripPosition()
    {
        if (_pickedStrip is not null)
        {
            return _ownerBay.GetStripPosition(_pickedStrip);
        }

        return 0;
    }

    private void PanelMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        // Focus();
    }
}
