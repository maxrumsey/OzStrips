using System.Windows.Forms;

namespace maxrumsey.ozstrips.gui
{
    public partial class DividerBarControl : UserControl
    {
        public DividerBarControl()
        {
            InitializeComponent();
        }

        public void SetVal(int num)
        {
            label1.Text = "(" + num + ") Queue";
        }
    }
}
