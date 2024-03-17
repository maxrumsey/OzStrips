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
    public partial class AltHdgControl : UserControl
    {
        public AltHdgControl()
        {
            InitializeComponent();
        }

        public string Hdg { get { return tb_hdg.Text; } }
        public string Alt { get { return tb_alt.Text; } }

        private void AltHdgControl_Load(object sender, EventArgs e)
        {

        }

        private void cb_alt_SelectedIndexChanged(object sender, EventArgs e)
        {
            tb_alt.Text = cb_alt.Text;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tb_alt.Text = "";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tb_hdg.Text = "";
        }

        private void AddHdgVal(int amt)
        {
            String amount = amt.ToString();
            if (tb_hdg.Text.Length < 3)
            {
                tb_hdg.Text += amount;
            }
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddHdgVal(7);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AddHdgVal(0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddHdgVal(1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddHdgVal(2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddHdgVal(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddHdgVal(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddHdgVal(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddHdgVal(6);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddHdgVal(8);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddHdgVal(9);
        }
    }
}
