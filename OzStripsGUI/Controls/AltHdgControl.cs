using maxrumsey.ozstrips.gui;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms;
using vatsys;

namespace maxrumsey.ozstrips.controls
{
    public partial class AltHdgControl : UserControl
    {
        private List<Airspace2.SystemRunway> runways;
        private StripController stripController;
        public BaseModal bm;
        private System.Timers.Timer sidTimer;
        public AltHdgControl(StripController controller)
        {
            stripController = controller;
            runways = controller.PossibleDepRunways;
            InitializeComponent();
            foreach (Airspace2.SystemRunway runway in runways)
            {
                cb_runway.Items.Add(runway.Name);
            }
            tb_alt.Text = controller.CFL;
            if (controller.CFL != "") cb_alt.Text = controller.CFL;
            tb_hdg.Text = controller.HDG; // todo: add some sort of parsing for this
            cb_runway.Text = controller.RWY;
            cb_sid.Text = controller.SID;
        }

        public string Hdg { get { return tb_hdg.Text; } }
        public string Alt { get { return tb_alt.Text; } }
        public string Runway { get { return cb_runway.Text; } }
        public string SID { get { return cb_sid.Text; } }

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

        private void cb_runway_SelectedIndexChanged(object sender, EventArgs e)
        {
            stripController.RWY = cb_runway.Text;

            // timer allows vatsys to determine which sid to give, then load this in accordingly.
            sidTimer = new System.Timers.Timer();
            sidTimer.Interval = 1000;
            sidTimer.AutoReset = false;
            sidTimer.Elapsed += updateSIDS;
            sidTimer.Start();

        }
        private void updateSIDS(object e, EventArgs args)
        {
            if (bm != null && bm.Visible) MainForm.mainForm.Invoke((MethodInvoker) delegate () { 
                String aerodrome = stripController.fdr.DepAirport;

                cb_sid.Items.Clear();

                List<Airspace2.SystemRunway> runways = Airspace2.GetRunways(aerodrome);
                Airspace2.SystemRunway sysRunway;
                foreach (Airspace2.SystemRunway runway in runways)
                {
                    if (runway.Name == Runway)
                    {
                        sysRunway = runway;
                        foreach (Airspace2.SystemRunway.SIDSTARKey sid in sysRunway.SIDs)
                        {
                            cb_sid.Items.Add(sid.sidStar.Name);
                        }

                    }
                }
                cb_sid.Text = stripController.SID;
            });
        }

        private void tb_alt_KeyDown(object sender, KeyEventArgs e)
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

