using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maxrumsey.ozstrips.gui
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
