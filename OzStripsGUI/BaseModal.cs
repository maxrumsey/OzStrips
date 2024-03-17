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
    public partial class BaseModal : Form
    {
        Control child;
        public BaseModal(Control child, String text)
        {
            InitializeComponent();
            this.child = child;

            gb_cont.Controls.Add(child);
            child.Anchor = AnchorStyles.None;
            child.Location = new Point(6, 16);
            
            this.Text = text;
        }

        private void bt_canc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_acp_Click(object sender, EventArgs e)
        {
            // to add
            // child.confirm();

            this.Close();
        }
    }
}
