using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// A about dialog showing copyright and version info.
/// </summary>
public partial class About : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="About"/> class.
    /// </summary>
    public About()
    {
        InitializeComponent();
        lb_version.Text = "Version: " + OzStripsConfig.version;
    }
}
