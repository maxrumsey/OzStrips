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
        public event ReturnEventHandler ReturnEvent;
        public BaseModal(Control child, String text)
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            this.child = child;

            gb_cont.Controls.Add(child);
            child.Anchor = AnchorStyles.Top;
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
            ReturnEvent(this, new ModalReturnArgs(this.child));
            this.Close();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Button btn = this.ActiveControl as Button;
            if (btn != null)
            {
                if (keyData == Keys.Enter)
                {
                    ReturnEvent(this, new ModalReturnArgs(this.child));
                    this.Close();
                    return true; // suppress default handling of space
                } else if (keyData == Keys.Escape)
                {
                    this.Close();
                    return true; // suppress default handling of space
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
    public class ModalReturnArgs : EventArgs
    {
        public Control child;
        public ModalReturnArgs(Object child)
        {
            this.child = (Control) child;
        }
    }
    public delegate void ReturnEventHandler(object source, ModalReturnArgs e);
}
