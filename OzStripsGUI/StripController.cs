using maxrumsey.ozstrips.controls;
using maxrumsey.ozstrips.gui.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using vatsys;

namespace maxrumsey.ozstrips.gui
{
    /// <summary>
    /// Responsible for strip logic, represents a Vatsys FDR.
    /// </summary>
    public class StripController
    {
        public FDP2.FDR fdr;
        public StripBay currentBay;

        /// <summary>
        /// Value from 0-2, represents colour of cock elements
        /// </summary>
        public int cockLevel = 0;
        public StripBaseGUI stripControl;
        public Control stripHolderControl;
        public BayManager BayManager;
        public StripLayoutTypes StripType;
        public DateTime TakeOffTime = DateTime.MaxValue;
        private bool crossing = false;
        public static List<StripController> stripControllers = new List<StripController>();
        public StripController(FDP2.FDR fdr)
        {
            this.fdr = fdr;
            this.BayManager = BayManager.bayManager;
            this.currentBay = StripBay.BAY_PREA;
            if (ArrDepType == StripArrDepType.ARRIVAL) this.currentBay = StripBay.BAY_ARRIVAL;
            CreateStripObj();
        }

        /// <summary>
        /// Whether or not the strip is in crossing mode
        /// </summary>
        public bool Crossing
        {
            get
            {
                return crossing;
            }
            set
            {
                crossing = value;
                stripControl.SetCross();
            }
        }

        /// <summary>
        /// Loads in a cacheDTO object received from server, sets SCs accordingly
        /// </summary>
        /// <param name="cDTO"></param>
        public static void LoadCache(CacheDTO cDTO)
        {
            foreach (StripControllerDTO stripDTO in cDTO.strips)
            {
                UpdateFDR(stripDTO, BayManager.bayManager);
            }
        }

        public void HMI_SetPicked(bool picked)
        {
            stripControl.HMI_TogglePick(picked);
        }

        public void TakeOff()
        {
            if (TakeOffTime == DateTime.MaxValue) TakeOffTime = DateTime.UtcNow;
            else TakeOffTime = DateTime.MaxValue;

            CoordinateStrip();
            SyncStrip();
        }

        public void CoordinateStrip()
        {
            if (fdr.State == FDP2.FDR.FDRStates.STATE_PREACTIVE && (Network.Me.IsRealATC || MainForm.isDebug)) FDP2.EstFDR(fdr, true);
        }

        /// <summary>
        /// Creates control for strip
        /// </summary>
        public void CreateStripObj()
        {
            stripHolderControl = new Panel();
            stripHolderControl.BackColor = Color.FromArgb(193, 230, 242);
            if (ArrDepType == StripArrDepType.ARRIVAL) stripHolderControl.BackColor = Color.FromArgb(255, 255, 160);

            stripHolderControl.Padding = new Padding(3);
            stripHolderControl.Margin = new Padding(0);

            //stripHolderControl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            stripHolderControl.Size = new Size(100, 100);

            stripControl = new Strip(this);

            stripControl.Initialise();
            stripHolderControl.Size = new Size(stripControl.Size.Width, stripControl.Size.Height + 6);
            stripHolderControl.Controls.Add(stripControl);

            //stripHolderControl = new 
        }

        /// <summary>
        /// Removes items from the strip holder control
        /// </summary>
        public void ClearStripControl()
        {
            if (stripHolderControl != null && stripControl != null)
            {
                stripHolderControl.Controls.Clear();
            }
        }

        /// <summary>
        /// Refreshes strip properties, determines if strip should be removed.
        /// </summary>
        public void UpdateFDR()
        {
            stripControl.UpdateStrip();

            double distance = GetDistToAerodrome(BayManager.AerodromeName);

            if ((distance == -1 || distance > 50) && ArrDepType == StripArrDepType.DEPARTURE)
            {
                BayManager.DeleteStrip(this);
            }
        }

        /// <summary>
        /// Receives a fdr, updates according SC.
        /// </summary>
        /// <param name="fdr"></param>
        /// <param name="bayManager"></param>
        /// <returns></returns>
        public static StripController UpdateFDR(FDP2.FDR fdr, BayManager bayManager)
        {
            bool found = false;
            foreach (StripController controller in stripControllers)
            {
                if (controller.fdr.Callsign == fdr.Callsign)
                {
                    found = true;

                    if (FDP2.GetFDRIndex(fdr.Callsign) == -1)
                    {
                        bayManager.DeleteStrip(controller);
                    }
                    controller.UpdateFDR();
                    return controller;
                }
            }
            if (!found)
            {
                // todo: add this logic into separate static function
                StripController stripController = new StripController(fdr);
                bayManager.AddStrip(stripController);
                return stripController;
            }
            return null;
        }

        /// <summary>
        /// Receives a SC DTO object, updates relevant SC.
        /// </summary>
        /// <param name="scDTO"></param>
        /// <param name="bayManager"></param>
        public static void UpdateFDR(StripControllerDTO scDTO, BayManager bayManager)
        {
            foreach (StripController controller in stripControllers)
            {
                if (controller.fdr.Callsign == scDTO.acid)
                {
                    bool changeBay = false;
                    controller.CLX = scDTO.CLX != null ? scDTO.CLX : "";
                    controller.GATE = scDTO.GATE != null ? scDTO.GATE : "";
                    if (controller.currentBay != scDTO.bay) changeBay = true;
                    controller.currentBay = scDTO.bay;
                    controller.stripControl.Cock(scDTO.cockLevel, false);
                    if (scDTO.TOT == "\0") controller.TakeOffTime = DateTime.MaxValue;
                    else controller.TakeOffTime = DateTime.Parse(scDTO.TOT);
                    controller.Remark = scDTO.remark != null ? scDTO.remark : "";
                    controller.crossing = scDTO.crossing;
                    controller.stripControl.SetCross(false);

                    if (changeBay) bayManager.UpdateBay(controller); // prevent unessesscary reshufles

                    return;
                }
            }
        }

        /// <summary>
        /// Determines which bay to move strip to.
        /// </summary>
        public void SIDTrigger()
        {
            Dictionary<StripBay, StripBay> stripBayResultDict;
            if (ArrDepType == StripArrDepType.ARRIVAL)
            {
                stripBayResultDict = NextBayArr;
            }
            else if (ArrDepType == StripArrDepType.DEPARTURE)
            {
                stripBayResultDict = NextBayDep;

            }
            else
            {
                return;
            }
            StripBay nextBay;
            if (stripBayResultDict.TryGetValue(currentBay, out nextBay))
            {
                currentBay = nextBay;
                BayManager.UpdateBay(this);
                SyncStrip();
            }
        }

        public void TogglePick()
        {
            if (BayManager.Picked == this)
            {
                BayManager.SetPicked();
            }
            else
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
            get
            {
                if (BayManager == null) return StripArrDepType.UNKNOWN;

                if (fdr.DesAirport == BayManager.AerodromeName)
                {
                    return StripArrDepType.ARRIVAL;
                }
                else if (fdr.DepAirport == BayManager.AerodromeName)
                {
                    return StripArrDepType.DEPARTURE;
                }
                else
                {
                    return StripArrDepType.UNKNOWN;
                }
            }
        }

        public double GetDistToAerodrome(String aerodrome)
        {
            try
            {
                Coordinate adCoord = Airspace2.GetAirport(aerodrome)?.LatLong;
                Coordinate planeCoord = fdr.PredictedPosition.Location;
                List<RDP.RadarTrack> RadarTracks = (from radarTrack in RDP.RadarTracks
                                                    where radarTrack.ActualAircraft.Callsign == fdr.Callsign
                                                    select radarTrack).ToList();

                if (RadarTracks.Count > 0)
                {
                    foreach (RDP.RadarTrack rTrack in RadarTracks)
                    {
                        planeCoord = rTrack.ActualAircraft.Position;
                    }
                }
                if (adCoord != null)
                {
                    double distance = Conversions.CalculateDistance(adCoord, planeCoord);
                    return distance;
                }
            }
            catch
            {

            }
            return -1;

        }

        public String CFL
        {
            get => this.fdr.CFLString; set
            {
                if (Network.Me.IsRealATC || MainForm.isDebug) FDP2.SetCFL(fdr, value);
            }
        }

        public String CLX = "";
        public String Remark = "";
        public String GATE = "";

        public String HDG
        {
            set
            {
                if ((Network.Me.IsRealATC || MainForm.isDebug) && value != "") FDP2.SetGlobalOps(fdr, "H" + value);
            }
            get
            {
                Regex r = new Regex(@"H(\d{3})");
                Match hdgMatch = r.Match(fdr.GlobalOpData);
                if (hdgMatch.Success)
                {
                    return hdgMatch.Value.Replace("H", "");
                }
                else return "";
            }
        }

        public String Time
        {
            get
            {
                if (ArrDepType == StripArrDepType.DEPARTURE && fdr.ATD == DateTime.MaxValue) return fdr.ETD.ToString("HHmm");
                else if (ArrDepType == StripArrDepType.DEPARTURE) return fdr.ATD.ToString("HHmm");
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
                            if (Network.Me.IsRealATC || MainForm.isDebug) FDP2.SetDepartureRunway(fdr, runway);
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
                        if (value == fdr.SIDSTARString) return; // dont needlessly set sid
                        if (Network.Me.IsRealATC || MainForm.isDebug) FDP2.SetSID(fdr, possibleSID.sidStar);
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

        public static Dictionary<StripBay, StripBay> NextBayDep = new Dictionary<StripBay, StripBay>
        {
            { StripBay.BAY_PREA, StripBay.BAY_CLEARED },
            { StripBay.BAY_CLEARED, StripBay.BAY_PUSHED },
            { StripBay.BAY_PUSHED, StripBay.BAY_TAXI },
            { StripBay.BAY_TAXI, StripBay.BAY_HOLDSHORT },
            { StripBay.BAY_RUNWAY, StripBay.BAY_OUT },
            { StripBay.BAY_OUT, StripBay.BAY_DEAD }
        };

        public static Dictionary<StripBay, StripBay> NextBayArr = new Dictionary<StripBay, StripBay>
        {

            { StripBay.BAY_TAXI, StripBay.BAY_DEAD },
            { StripBay.BAY_RUNWAY, StripBay.BAY_TAXI }
        };

        public void SyncStrip()
        {
            if (BayManager.socketConn != null)
            {
                BayManager.socketConn.SyncSC(this);
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
        BAY_ARRIVAL
    }
    public enum StripLayoutTypes
    {
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
