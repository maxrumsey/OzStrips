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
        public void UpdateFDR()
        {
            stripControl.UpdateStrip();
        }
        public static void UpdateFDR(FDP2.FDR fdr)
        {
            bool found = false;
            foreach (StripController controller in stripControllers)
            {
                if (controller.fdr == fdr)
                {
                    found = true;
                    controller.UpdateFDR();
                }
            }
            if (!found)
            {

            }
        }
        public String CFL
        {
            get => this.fdr.CFLString; set
            {
                FDP2.SetCFL(fdr, value);
            }
        }
        public String HDG
        {
            set
            {
                FDP2.SetGlobalOps(fdr, "H" + value);
            }
        }
        public String DepRWY
        {
            get
            {
                return fdr.DepartureRunway.Name;
            }
            set
            {
                String aerodrome = fdr.DepAirport;
                List<Airspace2.SystemRunway> runways = Airspace2.GetRunways(aerodrome);
                foreach (Airspace2.SystemRunway runway in runways)
                {
                    if (runway.Name == value)
                    {
                        FDP2.SetDepartureRunway(fdr, runway);
                    }
                }


            }
        }
        public List<Airspace2.SystemRunway> PossibleDepRunways
        {
            get
            {
                String aerodrome = fdr.DepAirport;
                return Airspace2.GetRunways(aerodrome);
            }
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
