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

        ReloadSize();
    }

    /// <summary>
    /// Sets the size of the divider bar in accordance with the size of strips.
    /// </summary>
    public void ReloadSize()
    {
        if (Properties.OzStripsSettings.Default.StripScale != 2)
        {
            Size = new System.Drawing.Size(431, 30);
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        }
        else
        {
            Size = new System.Drawing.Size(431, 50);
            label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        }
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
