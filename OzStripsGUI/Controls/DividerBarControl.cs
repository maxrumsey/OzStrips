using System.Windows.Forms;

namespace maxrumsey.ozstrips.controls
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
