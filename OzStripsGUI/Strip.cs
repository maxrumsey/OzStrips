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

namespace maxrumsey.ozstrips.gui
{
    public partial class Strip : StripBaseGUI
    {
        public Strip(StripController controller)
        {
            this.fdr = controller.fdr;
            InitializeComponent();

            pickToggleControl = pl_acid;

            this.cockColourControls = new Panel[] {
                this.pl_eobt,
                this.pl_multi,
                this.pl_multi2
                };

            this.stripController = controller;
            UpdateStrip();

        }

        public override void UpdateStrip()
        {
            if (fdr == null) return;
            lb_eobt.Text = fdr.ETD.ToString("HHmm");
            lb_acid.Text = fdr.Callsign;
            lb_ssr.Text = (fdr.AssignedSSRCode == -1) ? "XXXX" : Convert.ToString(fdr.AssignedSSRCode, 8).PadLeft(4, '0');
            lb_type.Text = fdr.AircraftType;
            lb_frul.Text = fdr.FlightRules;
            lb_route.Text = fdr.Route;
            lb_sid.Text = stripController.SID;
            lb_ades.Text = fdr.DesAirport;
            lb_alt.Text = stripController.CFL;
            lb_hdg.Text = stripController.HDG;
            lb_rwy.Text = stripController.RWY;
        }

        private void lb_sid_Click(object sender, EventArgs e)
        {
            stripController.SIDTrigger();
        }

        private void lb_eobt_Click(object sender, EventArgs e)
        {
            Cock(-1);
        }

        private void pl_route_MouseHover(object sender, EventArgs e)
        {
            tp_rte.SetToolTip(pl_route, fdr.Route);
        }

        private void lb_eobt_DoubleClick(object sender, EventArgs e)
        {
            Cock(-1);
        }

        private void lb_std_Click(object sender, EventArgs e)
        {

        }


        // open hdg-alt box
        private void pl_multi3_Click(object sender, EventArgs e)
        {
            
        }

        private void lb_hdg_Click(object sender, EventArgs e)
        {
            OpenHdgAltModal();
        }

        private void lb_alt_Click(object sender, EventArgs e)
        {
            OpenHdgAltModal();

        }

        private void lb_route_Click(object sender, EventArgs e)
        {
            OpenVatsysFDRModMenu();
        }

        private void lb_ades_Click(object sender, EventArgs e)
        {
            OpenVatsysFDRModMenu();
        }

        private void Strip_Load(object sender, EventArgs e)
        {

        }

        private void Strip_Load_1(object sender, EventArgs e)
        {

        }

        private void lb_acid_Click(object sender, EventArgs e)
        {
            TogglePick();
        }
    }
}
