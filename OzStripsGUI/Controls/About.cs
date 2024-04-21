using maxrumsey.ozstrips.gui;
using System.Windows.Forms;

namespace maxrumsey.ozstrips.controls
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            lb_version.Text = "Version: " + Config.version;

        }
    }
}
