using maxrumsey.ozstrips.gui;
using System;
using System.Windows.Forms;

namespace maxrumsey.ozstrips.controls
{
    public partial class Strip : StripBaseGUI
    {
        public Strip(StripController controller)
        {
            this.fdr = controller.fdr;
            InitializeComponent();

            pickToggleControl = pl_acid;

            base.lb_eobt = lb_eobt;
            base.lb_acid = lb_acid;
            base.lb_ssr = lb_ssr;
            base.lb_type = lb_type;
            base.lb_frul = lb_frul;
            base.lb_route = lb_route;
            base.lb_sid = lb_sid;
            base.lb_ades = lb_ades;
            base.lb_alt = lb_alt;
            base.lb_hdg = lb_hdg;
            base.lb_rwy = lb_rwy;
            base.lb_wtc = lb_wtc;
            base.lb_std = lb_std;
            base.lb_clx = lb_clx;
            base.lb_remark = lb_remark;
            base.lb_req = lb_req;
            base.lb_glop = lb_glop;
            base.lb_tot = lb_tot;

            base.crossColourControls = new Panel[]
            {
                pl_clx,
                pl_rwy,
                pl_route,
                pl_hdg,
                pl_alt,
                pl_req

            };
            base.cockColourControls = new Panel[] { };
            this.stripController = controller;
            UpdateStrip();

        }

        private void lb_sid_Click(object sender, EventArgs e)
        {
            stripController.SIDTrigger();
        }

        private void OpenHdgAlt(object sender, EventArgs e)
        {
            OpenHdgAltModal();
        }

        private void OpenFDR(object sender, EventArgs e)
        {
            OpenVatsysFDRModMenu();
        }

        private void lb_acid_Click(object sender, EventArgs e)
        {
            TogglePick();
        }

        private void lb_ssr_Click(object sender, EventArgs e)
        {
            AssignSSR();
        }

        private void OpenCLXBay(object sender, EventArgs e)
        {
            OpenCLXBayModal();

        }

        private void lb_tot_Click(object sender, EventArgs e)
        {
            stripController.TakeOff();
        }
    }
}
