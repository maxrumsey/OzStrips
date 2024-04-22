using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// A divider control bar.
/// </summary>
public partial class DividerBarControl : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DividerBarControl"/> class.
    /// </summary>
    public DividerBarControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Sets the queue number.
    /// </summary>
    /// <param name="num">The number.</param>
    public void SetVal(int num)
    {
        label1.Text = "(" + num + ") Queue";
    }
}
