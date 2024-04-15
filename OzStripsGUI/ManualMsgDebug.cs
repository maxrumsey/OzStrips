using maxrumsey.ozstrips.gui.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;

namespace maxrumsey.ozstrips.gui
{
    public partial class ManualMsgDebug : UserControl
    {
        public ManualMsgDebug()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StripControllerDTO scDTO = new StripControllerDTO() { ACID = tb_acid.Text, bay=(StripBay) Int32.Parse(tb_baynum.Text), CLX = tb_clx.Text, GATE=tb_bay.Text, Crossing=cb_crossing.Checked, StripCockLevel=Int32.Parse(tb_cocklevel.Text), TOT= "\0" };

            StripController.UpdateFDR(scDTO, BayManager.bayManager);
        }
    }
}
