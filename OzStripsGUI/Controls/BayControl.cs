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
        _stripHeight = Bay.GetStripHeight();

        var child = ChildPanel.Controls[0];
        var parent = ChildPanel.Parent;

        ChildPanel.VerticalScroll.SmallChange = _stripHeight;
        ChildPanel.VerticalScroll.Maximum = _ownerBay.GetRequestedHeight();
        ChildPanel.VerticalScroll.Minimum = 0;


        if (child.Height + 40 > parent.Height)
        {
            ChildPanel.VerticalScroll.Visible = true;
        }
        else
        {
            ChildPanel.VerticalScroll.Visible = false;
        }
    }

    private new void MouseWheel(object sender, MouseEventArgs e)
    {
        return;
    }

    private new void Scroll(object sender, ScrollEventArgs e)
    {
        var val = e.NewValue;

        if (val < 0)
        {
            val = 0;
        }

        ChildPanel.VerticalScroll.Value = val;
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
        ChildPanel.VerticalScroll.Value = val;
        ChildPanel.PerformLayout();
    }
}
