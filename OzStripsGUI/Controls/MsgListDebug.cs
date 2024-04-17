using maxrumsey.ozstrips.gui;
using System;
using System.Windows.Forms;

namespace maxrumsey.ozstrips.controls
{
    public partial class MsgListDebug : UserControl
    {
        public MsgListDebug(SocketConn socketConn)
        {
            InitializeComponent();
            foreach (var str in socketConn.Messages)
            {
                lb_menu.Items.Add(str);
            }
        }

        private void lb_menu_Click(object sender, EventArgs e)
        {
            rtb_text.Text = lb_menu.Text;
        }
    }
}
