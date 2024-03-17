using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;

namespace maxrumsey.ozstrips.gui
{
    
    public class StripController
    {
        public FDP2.FDR fdr;
        public StripBay currentBay;
        public int cockLevel = 0;
        public StripBaseGUI stripControl;
        public Control stripHolderControl;

        public static List<StripController> stripControllers = new List<StripController>();

        public StripController(FDP2.FDR fdr) {
            this.fdr = fdr;
            this.currentBay = StripBay.BAY_PREA;
            stripControllers.Add(this);
            CreateStripObj();
        }
        public void CreateStripObj()
        {
            stripControl = new Strip(this);
            //stripHolderControl = new 
        }
        public void FDRUpdate()
        {
            stripControl.UpdateStrip();
        }

    }

    public enum StripBay
    {
        BAY_DEAD,
        BAY_PREA,
        BAY_CLEARED,
        BAY_PUSHED,
        BAY_TAXI,
        BAY_HOLDSHORT,
        BAY_RUNWAY,
        BAY_OUT,
        BAY_ARRIVAL_PREA,
        BAY_ARRIVAL_CONT
    }
}
