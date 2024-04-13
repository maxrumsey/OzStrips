using System;
using System.Collections.Generic;
using System.Windows.Forms;
using vatsys;

namespace maxrumsey.ozstrips.gui
{
    public partial class BayCLXControl : UserControl
    {
        private List<Airspace2.SystemRunway> runways;
        private StripController stripController;
        public BayCLXControl(StripController controller)
        {
            stripController = controller;
            InitializeComponent();

            tb_clx.Text = controller.CLX;
            tb_bay.Text = controller.GATE;
        }

        public string CLX { get { return tb_clx.Text; } }
        public string GATE { get { return tb_bay.Text; } }



        private void AltHdgControl_Load(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            tb_clx.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tb_bay.Text = "";
        }
    }
}

