using System;
using System.Collections.Generic;
using System.Windows.Forms;
using vatsys;
using maxrumsey.ozstrips.gui;

namespace maxrumsey.ozstrips.controls
{
    public partial class BayCLXControl : UserControl
    {
        private List<Airspace2.SystemRunway> runways;
        private StripController stripController;
        public BaseModal bm;
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
            if (tb_bay.Text.Length == 0) bm.ActiveControl = tb_bay;
            else bm.ActiveControl = tb_clx;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tb_clx.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tb_bay.Text = "";
        }

        private void tb_clx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                bm.ExitModal(true);
            }
            else if (e.KeyData == Keys.Escape)
            {
                bm.ExitModal();
            }
        }
    }
}

