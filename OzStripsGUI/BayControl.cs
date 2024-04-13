using System;
using System.Windows.Forms;

namespace maxrumsey.ozstrips.gui
{
    public partial class BayControl : UserControl

    {
        public FlowLayoutPanel ChildPanel;
        private Bay OwnerBay;
        private BayManager BayManager;
        public BayControl(BayManager bm, String name, Bay bay)
        {
            InitializeComponent();
            lb_bay_name.Text = name;
            ChildPanel = (FlowLayoutPanel)flp_stripbay;
            flp_stripbay.VerticalScroll.Visible = true;

            this.BayManager = bm;
            OwnerBay = bay;
        }

        private void lb_bay_name_Click(object sender, EventArgs e)
        {
            BayManager.DropStrip(OwnerBay);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OwnerBay.AddDivider(false);
        }

        private void bt_queue_Click(object sender, EventArgs e)
        {
            OwnerBay.QueueUp();
        }
    }
}
