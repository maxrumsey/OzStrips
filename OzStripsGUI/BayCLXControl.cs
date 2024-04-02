using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;
using static vatsys.FDP2;
using static vatsys.SectorsVolumes;

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
            tb_bay.Text = controller.BAY;
        }

        public string CLX { get { return tb_clx.Text; } }
        public string BAY { get { return tb_bay.Text; } }



        private void AltHdgControl_Load(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            tb_clx.Text = "";
        }
    }
}

