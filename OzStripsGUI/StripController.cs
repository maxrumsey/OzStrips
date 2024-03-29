using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
        public BayManager BayManager;
        public StripLayoutTypes StripType;

        public static List<StripController> stripControllers = new List<StripController>();

        public StripController(FDP2.FDR fdr, String AD) {
            this.fdr = fdr;
            this.currentBay = StripBay.BAY_PREA;
            stripControllers.Add(this);
            CreateStripObj();
        }

        public void HMI_SetPicked(bool picked)
        {
            stripControl.HMI_TogglePick(picked);
        }

        public void CreateStripObj()
        {
            stripHolderControl = new Panel();
            stripHolderControl.BackColor = Color.Blue;
            stripHolderControl.Padding = new Padding(3);
            stripHolderControl.Margin = new Padding(0);

            //stripHolderControl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            stripHolderControl.Size = new Size(100, 100);

            stripControl = new Strip(this);
            stripControl.Initialise();
            stripHolderControl.Size = new Size(stripControl.Size.Width, stripControl.Size.Height+6);
            stripHolderControl.Controls.Add(stripControl);

            //stripHolderControl = new 
        }

        public void UpdateFDR()
        {
            stripControl.UpdateStrip();
        }
        public static void UpdateFDR(FDP2.FDR fdr, BayManager bayManager)
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
                // todo: add this logic into separate static function
                StripController stripController = new StripController(fdr, bayManager.AerodromeName);
                bayManager.AddStrip(stripController);
            }
        }

        public void SIDTrigger()
        {
            currentBay++; // todo: fix
            BayManager.UpdateBay(this);
        }

        public void TogglePick()
        {
            if (BayManager.Picked == this)
            {
                BayManager.SetPicked();
            } else
            {
                BayManager.SetPicked(this);
            }
        }

        public bool ApplicableToAerodrome(String name)
        {
            if (fdr.DepAirport == name || fdr.DesAirport == name)
            {
                return true;
            }
            return false;
        }

        public StripArrDepType ArrDepType
        {
            get {
                if (fdr.DesAirport == BayManager.AerodromeName) {
                    return StripArrDepType.ARRIVAL;
                } else if (fdr.DepAirport == BayManager.AerodromeName)
                {
                    return StripArrDepType.DEPARTURE;
                } else
                {
                    return StripArrDepType.UNKNOWN;
                }
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
            get
            {
                Regex r = new Regex(@"H(\d{3})");
                Match hdgMatch = r.Match(fdr.GlobalOpData);
                if (hdgMatch.Success)
                {
                    return hdgMatch.Value;
                }
                else return "";
            }
        }
        public String RWY
        {
            get
            {
                if (ArrDepType == StripArrDepType.DEPARTURE && fdr.DepartureRunway != null) return fdr.DepartureRunway.Name;
                else if (ArrDepType == StripArrDepType.ARRIVAL && fdr.ArrivalRunway != null) return fdr.ArrivalRunway.Name;
                else return "";
            }
            set
            {
                if (ArrDepType == StripArrDepType.DEPARTURE)
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
        }

        public String SID
        {
            get
            {
                return fdr.SIDSTARString;
            }
            set
            {
                bool found = false;
                foreach (Airspace2.SystemRunway.SIDSTARKey possibleSID in fdr.DepartureRunway.SIDs)
                {
                    if (possibleSID.sidStar.Name == value)
                    {
                        FDP2.SetSID(fdr, possibleSID.sidStar);
                        found = true;
                    }
                }
                if (!found)
                {
                    CreateError("Attempted to set invalid SID");
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

        public static void CreateError(String error)
        {
            CreateError(new Exception(error));
        }
        public static void CreateError(Exception error)
        {
            Errors.Add(error, "OzStrips");
        }
        public void OpenVatsysFDR()
        {
            MMI.OpenFPWindow(fdr);
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
        BAY_ARRIVAL
    }
    public enum StripLayoutTypes {
        STRIP_ACD,
        STRIP_DEP_SMC,
        STRIP_DEP_ADC,
        STRIP_ARR_ADC,
        STRIP_ARR_SMC
    }

    public enum StripArrDepType
    {
        ARRIVAL,
        DEPARTURE,
        UNKNOWN
    }
}
