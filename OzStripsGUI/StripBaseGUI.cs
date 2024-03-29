using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;

namespace maxrumsey.ozstrips.gui
{
    public partial class StripBaseGUI : UserControl
    {
        public int cockLevel = 0;
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

        public StripBaseGUI()
        {
            InitializeComponent();
        }



        public void Cock(int _cockLevel)
        {
            if (_cockLevel == -1)
            {
                _cockLevel = this.cockLevel + 1;
                if (_cockLevel >= 3) _cockLevel = 0;
            }
            this.cockLevel = _cockLevel;
            Color color = defColor;
            if (this.cockLevel == 1)
            {
                color = Color.Aquamarine;
            }
            else if (this.cockLevel == 2)
            {
                color = Color.Pink;
            }

            foreach (Control control in cockColourControls)
            {
                control.BackColor = color;
            }
            

        }

        public void UpdateStrip()
        {
            if (fdr == null) return;
            lb_eobt.Text = fdr.ETD.ToString("HHmm");
            lb_acid.Text = fdr.Callsign;
            lb_ssr.Text = (fdr.AssignedSSRCode == -1) ? "XXXX" : Convert.ToString(fdr.AssignedSSRCode, 8).PadLeft(4, '0');
            lb_type.Text = fdr.AircraftType;
            lb_frul.Text = fdr.FlightRules;

            String rteItem =  fdr.Route.Split(' ').ToList().Find(x => !x.Contains("/"));
            if (rteItem == null) rteItem = fdr.Route;

            lb_route.Text = rteItem;

            lb_sid.Text = stripController.SID;
            lb_ades.Text = fdr.DesAirport;
            lb_alt.Text = stripController.CFL;
            lb_hdg.Text = stripController.HDG;
            lb_rwy.Text = stripController.RWY;
        }


        public void OpenHdgAltModal()
        {
            AltHdgControl modalChild = new AltHdgControl(stripController);
            BaseModal bm = new BaseModal(modalChild, "Edit");
            bm.ReturnEvent += new ReturnEventHandler(HeadingAltReturned);
            //bm.SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);
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
            AltHdgControl control = (AltHdgControl) args.child;
            if (control.Alt != "") stripController.CFL = control.Alt;
            if (control.Hdg != "") stripController.HDG = control.Hdg;
            if (control.Runway != "") stripController.RWY = control.Runway;
            if (control.SID != "") {
                stripController.SID = control.SID;
            }
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

    }
    
}
