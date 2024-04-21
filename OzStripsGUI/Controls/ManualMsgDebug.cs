using maxrumsey.ozstrips.gui;
using maxrumsey.ozstrips.gui.DTO;
using System;
using System.Windows.Forms;

namespace maxrumsey.ozstrips.controls
{
    public partial class ManualMsgDebug : UserControl
    {
        public ManualMsgDebug()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StripControllerDTO scDTO = new StripControllerDTO() { acid = tb_acid.Text, bay = (StripBay)Int32.Parse(tb_baynum.Text), CLX = tb_clx.Text, GATE = tb_bay.Text, crossing = cb_crossing.Checked, cockLevel = Int32.Parse(tb_cocklevel.Text), TOT = "\0" };

            StripController.UpdateFDR(scDTO, BayManager.bayManager);
        }
    }
}
