using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        }

        public void OpenHdgAltModal()
        {
            AltHdgControl modalChild = new AltHdgControl();
            BaseModal bm = new BaseModal(modalChild, "Edit");
            bm.ReturnEvent += new ReturnEventHandler(HeadingAltReturned);
            bm.SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);
            bm.ShowDialog();
        }

        /*
         *  Returned event args from handler
         */
        public void HeadingAltReturned(object source, ModalReturnArgs args)
        {
            AltHdgControl control = (AltHdgControl) args.child;
            if (control.Alt != "") stripController.CFL = control.Alt;
            if (control.Hdg != "") stripController.HDG = control.Hdg;


        }
    }
    
}
