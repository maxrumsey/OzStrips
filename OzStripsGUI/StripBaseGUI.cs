using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using vatsys;

namespace maxrumsey.ozstrips.gui
{
    public partial class StripBaseGUI : UserControl
    {
        public FDP2.FDR fdr;
        public Color defColor = Color.WhiteSmoke;
        public Panel[] cockColourControls;
        public StripController stripController;
        public Panel pickToggleControl;

        public Label lb_eobt;
        public Label lb_acid;
        public Label lb_ssr;
        public Label lb_type;
        public Label lb_frul;
        public Label lb_route;
        public Label lb_sid;
        public Label lb_ades;
        public Label lb_alt;
        public Label lb_hdg;
        public Label lb_rwy;
        public Label lb_wtc;
        public Label lb_std;
        public Label lb_clx;
        public Label lb_tot;

        public StripBaseGUI()
        {
            InitializeComponent();
        }



        public void Cock(int _cockLevel, bool sync = true)
        {
            if (_cockLevel == -1)
            {
                _cockLevel = stripController.cockLevel + 1;
                if (_cockLevel >= 3) _cockLevel = 0;
            }
            stripController.cockLevel = _cockLevel;
            Color color = defColor;
            if (stripController.cockLevel == 1)
            {
                color = Color.Aquamarine;
            }
            else if (stripController.cockLevel == 2)
            {
                color = Color.Pink;
            }

            foreach (Control control in cockColourControls)
            {
                control.BackColor = color;
            }

            if (sync) stripController.SyncStrip();
        }

        public void UpdateStrip()
        {
            SuspendLayout();
            if (fdr == null) return;
            lb_eobt.Text = stripController.Time;
            lb_acid.Text = fdr.Callsign;
            lb_ssr.Text = (fdr.AssignedSSRCode == -1) ? "XXXX" : Convert.ToString(fdr.AssignedSSRCode, 8).PadLeft(4, '0');
            lb_type.Text = fdr.AircraftType;
            lb_frul.Text = fdr.FlightRules;

            String rteItem = fdr.Route.Split(' ').ToList().Find(x => !x.Contains("/"));
            if (rteItem == null) rteItem = fdr.Route;

            if (lb_route != null) lb_route.Text = rteItem;

            if (lb_sid != null) lb_sid.Text = stripController.SID;
            if (lb_ades != null) lb_ades.Text = fdr.DesAirport;
            if (lb_alt != null) lb_alt.Text = stripController.CFL;
            if (lb_hdg != null) lb_hdg.Text = stripController.HDG;
            if (lb_clx != null) lb_clx.Text = stripController.CLX;
            if (lb_std != null) lb_std.Text = stripController.GATE;
            if (lb_tot != null && stripController.TakeOffTime != DateTime.MaxValue)
            {
                TimeSpan diff = DateTime.UtcNow - stripController.TakeOffTime;
                lb_tot.Text = diff.ToString(@"mm\:ss");
                lb_tot.ForeColor = Color.Green;
            }

            lb_rwy.Text = stripController.RWY;
            lb_wtc.Text = fdr.AircraftWake;
            ResumeLayout();
        }


        public void OpenHdgAltModal()
        {
            AltHdgControl modalChild = new AltHdgControl(stripController);
            BaseModal bm = new BaseModal(modalChild, "ACD Menu");
            modalChild.bm = bm;
            bm.ReturnEvent += new ReturnEventHandler(HeadingAltReturned);
            bm.ShowDialog();
            SetModalCoord(bm);

        }
        public void OpenCLXBayModal()
        {
            BayCLXControl modalChild = new BayCLXControl(stripController);
            BaseModal bm = new BaseModal(modalChild, "SMC Menu");
            modalChild.bm = bm;
            bm.ReturnEvent += new ReturnEventHandler(CLXBayReturned);
            bm.ShowDialog();
            SetModalCoord(bm);

        }
        public void OpenVatsysFDRModMenu()
        {
            //MMI.OpenFPWindow(stripController.fdr);
            stripController.OpenVatsysFDR();
        }

        /*
         *  Returned event args from handler
         */
        public void HeadingAltReturned(object source, ModalReturnArgs args)
        {
            AltHdgControl control = (AltHdgControl)args.child;
            if (control.Alt != "") stripController.CFL = control.Alt;
            if (control.Hdg != "") stripController.HDG = control.Hdg;
            if (control.Runway != "") stripController.RWY = control.Runway;
            if (control.SID != "")
            {
                stripController.SID = control.SID;
            }

            stripController.SyncStrip();
        }
        public void CLXBayReturned(object source, ModalReturnArgs args)
        {
            BayCLXControl control = (BayCLXControl)args.child;
            if (control.CLX != "") stripController.CLX = control.CLX;
            if (control.GATE != "") stripController.GATE = control.GATE;

            stripController.SyncStrip();
        }

        public void SetModalCoord(BaseModal bm)
        {
            int screenCount = Screen.AllScreens.Count();
            bm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
        }

        public void TogglePick()
        {
            if (stripController != null)
            {
                stripController.TogglePick();
            }
        }

        public void HMI_TogglePick(bool picked)
        {
            if (pickToggleControl != null)
            {
                Color color = Color.Gainsboro;
                if (picked) color = Color.Silver;

                pickToggleControl.BackColor = color;
            }
        }
        public void Initialise()
        {
            this.Dock = DockStyle.Fill;

        }

        public void AssignSSR()
        {
            FDP2.SetASSR(stripController.fdr);
        }
    }

}
