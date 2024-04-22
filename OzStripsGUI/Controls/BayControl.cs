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
        ChildPanel = flp_stripbay;
        flp_stripbay.VerticalScroll.Visible = true;

        _bayManager = bm;
        _ownerBay = bay;
    }

    /// <summary>
    /// Gets the child panel.
    /// </summary>
    public FlowLayoutPanel ChildPanel { get; }

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
}
