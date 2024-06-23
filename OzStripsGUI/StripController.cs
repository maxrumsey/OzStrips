using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using MaxRumsey.OzStripsPlugin.Gui.Controls;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using MaxRumsey.OzStripsPlugin.Gui.Properties;
using vatsys;

using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Responsible for strip logic, represents a Vatsys FDR.
/// </summary>
public sealed class StripController : IDisposable
{
    private static readonly Regex _headingRegex = new(@"H(\d{3})");
    private static readonly Regex _routeRegex = new(@"^[^\d/]+$");

    private readonly BayManager _bayManager;
    private readonly SocketConn _socketConn;
    ////private readonly StripLayoutTypes StripType;

    private StripBaseGUI? _stripControl;

    private bool _crossing;

    /// <summary>
    /// Initializes a new instance of the <see cref="StripController"/> class.
    /// </summary>
    /// <param name="fdr">The flight data record.</param>
    /// <param name="bayManager">Gets or sets the bay manager.</param>
    /// <param name="socketConn">The socket connection.</param>
    public StripController(FDR fdr, BayManager bayManager, SocketConn socketConn)
    {
        FDR = fdr;
        _bayManager = bayManager;
        ParentAerodrome = bayManager.AerodromeName;
        _socketConn = socketConn;
        CurrentBay = StripBay.BAY_PREA;
        if (ArrDepType == StripArrDepType.ARRIVAL)
        {
            CurrentBay = StripBay.BAY_ARRIVAL;
        }

        CreateStripObj();
    }

    /// <summary>
    /// Gets a list of strip controllers.
    /// </summary>
    public static List<StripController> StripControllers { get; } = [];

    /// <summary>
    /// Gets or sets the strip holder control.
    /// </summary>
    public Control? StripHolderControl { get; set; }

    /// <summary>
    /// Gets or sets the take off time.
    /// </summary>
    public DateTime? TakeOffTime { get; set; }

    /// <summary>
    /// Gets the ICAO name of the aerodrome being controlled.
    /// </summary>
    public string ParentAerodrome { get; }

    /// <summary>
    /// Gets the flight data record.
    /// </summary>
    public FDR FDR { get; internal set; }

    /// <summary>
    /// Gets or sets the current strip bay.
    /// </summary>
    public StripBay CurrentBay { get; set; }

    /// <summary>
    /// Gets or sets the current cock level.
    /// </summary>
    public int CockLevel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the strip is in crossing mode.
    /// </summary>
    public bool Crossing
    {
        get => _crossing;
        set
        {
            _crossing = value;
            _stripControl?.SetCross();
        }
    }

    /// <summary>
    /// Gets the arrival or departure type.
    /// </summary>
    public StripArrDepType ArrDepType
    {
        get
        {
            if (_bayManager == null)
            {
                return StripArrDepType.UNKNOWN;
            }

            if (FDR.DesAirport.Equals(_bayManager.AerodromeName, StringComparison.InvariantCultureIgnoreCase))
            {
                return StripArrDepType.ARRIVAL;
            }

            return FDR.DepAirport.Equals(_bayManager.AerodromeName, StringComparison.CurrentCultureIgnoreCase) ?
                StripArrDepType.DEPARTURE :
                StripArrDepType.UNKNOWN;
        }
    }

    /// <summary>
    /// Gets or sets the CFL.
    /// </summary>
    public string CFL
    {
        get => FDR.CFLString;

        set
        {
            if (Network.Me.IsRealATC || MainForm.IsDebug)
            {
                SetCFL(FDR, value);
            }
        }
    }

    /// <summary>
    /// Gets or sets the clearance.
    /// </summary>
    public string CLX { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the remarks.
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// Gets he requested flight level.
    /// </summary>
    public string RFL
    {
        get
        {
            return (FDR.RFL / 100).ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
        }
    }

    /// <summary>
    /// Gets or sets the gate.
    /// </summary>
    public string Gate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the heading.
    /// </summary>
    public string HDG
    {
        get
        {
            var hdgMatch = _headingRegex.Match(FDR.GlobalOpData);
            return hdgMatch.Success ? hdgMatch.Value.Replace("H", string.Empty) : string.Empty;
        }

        set
        {
            if ((Network.Me.IsRealATC || MainForm.IsDebug) && !string.IsNullOrWhiteSpace(value))
            {
                SetGlobalOps(FDR, "H" + value);
            }
        }
    }

    /// <summary>
    /// Gets the current eobt time.
    /// </summary>
    public string Time
    {
        get
        {
            if (ArrDepType == StripArrDepType.DEPARTURE && FDR.ATD == DateTime.MaxValue)
            {
                return FDR.ETD.ToString("HHmm", CultureInfo.InvariantCulture);
            }

            return ArrDepType == StripArrDepType.DEPARTURE ?
                FDR.ATD.ToString("HHmm", CultureInfo.InvariantCulture) :
                string.Empty;
        }
    }

    /// <summary>
    /// Gets or sets the runway.
    /// </summary>
    public string RWY
    {
        get
        {
            if (ArrDepType == StripArrDepType.DEPARTURE && FDR.DepartureRunway != null)
            {
                return FDR.DepartureRunway.Name;
            }

            return ArrDepType == StripArrDepType.ARRIVAL && FDR.ArrivalRunway != null ? FDR.ArrivalRunway.Name : string.Empty;
        }

        set
        {
            if (ArrDepType == StripArrDepType.DEPARTURE)
            {
                var aerodrome = FDR.DepAirport;
                var runways = Airspace2.GetRunways(aerodrome);
                foreach (var runway in runways)
                {
                    if (runway.Name == value)
                    {
                        if (Network.Me.IsRealATC || MainForm.IsDebug)
                        {
                            SetDepartureRunway(FDR, runway);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the first element in the route.
    /// </summary>
    public string FirstWpt
    {
        get
        {
            return FDR.Route.Split(' ').ToList().Find(x => _routeRegex.Match(x).Success) ?? FDR.Route;
        }
    }

    /// <summary>
    /// Gets the full route text in the route.
    /// </summary>
    public string Route
    {
        get
        {
            return FDR.Route;
        }
    }

    /// <summary>
    /// Gets or sets the SID.
    /// </summary>
    public string SID
    {
        get
        {
            if (ArrDepType == StripArrDepType.DEPARTURE && FDR.SID is not null)
            {
                return FDR.SID.Name;
            }
            else if (ArrDepType == StripArrDepType.DEPARTURE)
            {
                return string.Empty;
            }
            else
            {
                return ">";
            }
        }

        set
        {
            var found = false;
            foreach (var possibleSID in FDR.DepartureRunway.SIDs)
            {
                if (possibleSID.sidStar.Name == value)
                {
                    if (value == FDR.SIDSTARString)
                    {
                        return; // dont needlessly set sid
                    }

                    if (Network.Me.IsRealATC || MainForm.IsDebug)
                    {
                        SetSID(FDR, possibleSID.sidStar);
                    }

                    found = true;
                }
            }

            if (!found)
            {
                CreateError("Attempted to set invalid SID");
            }
        }
    }

    /// <summary>
    /// Gets a list of possible departure runways.
    /// </summary>
    public List<Airspace2.SystemRunway> PossibleDepRunways
    {
        get
        {
            var aerodrome = FDR.DepAirport;
            return Airspace2.GetRunways(aerodrome);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the squawk code is correct.
    /// </summary>
    public bool SquawkCorrect
    {
        get
        {
            var rtrack = GetRadarTrack();
            return rtrack?.ActualAircraft.TransponderModeC == true && rtrack.ActualAircraft.TransponderCode == FDR.AssignedSSRCode;
        }
    }

    /// <summary>
    /// Gets a dictionary which contains the departure next state for a given state.
    /// </summary>
    private static Dictionary<StripBay, StripBay> NextBayDep { get; } = new()
    {
        { StripBay.BAY_PREA, StripBay.BAY_CLEARED },
        { StripBay.BAY_CLEARED, StripBay.BAY_PUSHED },
        { StripBay.BAY_PUSHED, StripBay.BAY_TAXI },
        { StripBay.BAY_TAXI, StripBay.BAY_HOLDSHORT },
        { StripBay.BAY_RUNWAY, StripBay.BAY_OUT },
        { StripBay.BAY_OUT, StripBay.BAY_DEAD },
    };

    /// <summary>
    /// Gets a dictionary which contains the arrival next state for a given state.
    /// </summary>
    private static Dictionary<StripBay, StripBay> NextBayArr { get; } = new()
    {
        { StripBay.BAY_TAXI, StripBay.BAY_DEAD },
        { StripBay.BAY_RUNWAY, StripBay.BAY_TAXI },
    };

    /// <summary>
    /// Converts a strip controller to the data object.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public static implicit operator StripControllerDTO(StripController sc)
    {
        var scDTO = new StripControllerDTO
        {
            acid = sc.FDR.Callsign,
            bay = sc.CurrentBay,
            CLX = sc.CLX,
            GATE = sc.Gate,
            cockLevel = sc.CockLevel,
            crossing = sc.Crossing,
            remark = sc.Remark,
            TOT = sc.TakeOffTime is not null ? sc.TakeOffTime!.ToString() : "\0",
        };

        return scDTO;
    }

    /// <summary>
    /// Converts a strip controller to the data object.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public static implicit operator SCDeletionDTO(StripController sc)
    {
        var scDTO = new SCDeletionDTO
        {
            acid = sc.FDR.Callsign,
        };

        return scDTO;
    }

    /// <summary>
    /// Looks up fdr by name.
    /// </summary>
    /// <param name="name">The aircraft callsign.</param>
    /// <returns>The aircraft's FDR.</returns>
    public static FDR? GetFDR(string name)
    {
        foreach (var controller in StripControllers)
        {
            if (controller.FDR.Callsign == name)
            {
                return controller.FDR;
            }
        }

        return null;
    }

    /// <summary>
    /// Looks up controller by name.
    /// </summary>
    /// <param name="name">The aircraft callsign.</param>
    /// <returns>The aircraft's FDR.</returns>
    public static StripController? GetController(string name)
    {
        foreach (var controller in StripControllers)
        {
            if (controller.FDR.Callsign == name)
            {
                return controller;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds a error string to the VATSYS error system.
    /// </summary>
    /// <param name="error">The error.</param>
    public static void CreateError(string error)
    {
        CreateError(new Exception(error));
    }

    /// <summary>
    /// Adds a exception to the VATSYS error system.
    /// </summary>
    /// <param name="error">The error exception.</param>
    public static void CreateError(Exception error)
    {
        Errors.Add(error, "OzStrips");
    }

    /// <summary>
    /// Receives a fdr, updates according SC.
    /// </summary>
    /// <param name="fdr">The flight data record.</param>
    /// <param name="bayManager">The bay manager.</param>
    /// <param name="socketConn">The socket connection.</param>
    /// <returns>The appropriate strip controller for the FDR.</returns>
    public static StripController UpdateFDR(FDR fdr, BayManager bayManager, SocketConn socketConn)
    {
        foreach (var controller in StripControllers)
        {
            if (controller.FDR.Callsign == fdr.Callsign)
            {
                if (GetFDRIndex(fdr.Callsign) == -1)
                {
                    bayManager.DeleteStrip(controller);
                }

                controller.FDR = fdr;
                controller.UpdateFDR();
                return controller;
            }
        }

        // todo: add this logic into separate static function
        var stripController = new StripController(fdr, bayManager, socketConn);
        bayManager.AddStrip(stripController);
        return stripController;
    }

    /// <summary>
    /// Loads in a cacheDTO object received from server, sets SCs accordingly.
    /// </summary>
    /// <param name="cacheData">The cache data.</param>
    /// <param name="bayManager">The bay manager.</param>
    public static void LoadCache(CacheDTO cacheData, BayManager bayManager)
    {
        foreach (var stripDTO in cacheData.strips)
        {
            UpdateFDR(stripDTO, bayManager);
        }
    }

    /// <summary>
    /// Receives a SC DTO object, updates relevant SC.
    /// </summary>
    /// <param name="stripControllerData">The strip controller data.</param>
    /// <param name="bayManager">The bay manager.</param>
    public static void UpdateFDR(StripControllerDTO stripControllerData, BayManager bayManager)
    {
        foreach (var controller in StripControllers)
        {
            if (controller.FDR.Callsign == stripControllerData.acid)
            {
                var changeBay = false;
                controller.CLX = !string.IsNullOrWhiteSpace(stripControllerData.CLX) ? stripControllerData.CLX : string.Empty;
                controller.Gate = stripControllerData.GATE ?? string.Empty;
                if (controller.CurrentBay != stripControllerData.bay)
                {
                    changeBay = true;
                }

                controller.CurrentBay = stripControllerData.bay;
                controller._stripControl?.Cock(stripControllerData.cockLevel, false);
                controller.TakeOffTime = stripControllerData.TOT == "\0" ?
                    null :
                    DateTime.Parse(stripControllerData.TOT, CultureInfo.InvariantCulture);

                controller.Remark = !string.IsNullOrWhiteSpace(stripControllerData.remark) ? stripControllerData.remark : string.Empty;
                controller._crossing = stripControllerData.crossing;
                controller._stripControl?.SetCross(false);

                if (changeBay)
                {
                    bayManager.UpdateBay(controller); // prevent unessesscary reshufles
                }

                return;
            }
        }
    }

    /// <summary>
    /// Clears the strip controllers.
    /// </summary>
    public static void ClearControllers()
    {
        StripControllers.Clear();
    }

    /// <summary>
    /// Sets the HMI picked state.
    /// </summary>
    /// <param name="picked">True if picked, false otherwise.</param>
    public void SetHMIPicked(bool picked)
    {
        _stripControl?.HMI_TogglePick(picked);
    }

    /// <summary>
    /// That the strip has taken off.
    /// </summary>
    public void TakeOff()
    {
        if (TakeOffTime is null)
        {
            TakeOffTime = DateTime.UtcNow;
            CoordinateStrip();
        }
        else
        {
            TakeOffTime = null;
        }

        SyncStrip();
    }

    /// <summary>
    /// Coordinates the strip with the server.
    /// </summary>
    public void CoordinateStrip()
    {
        if (FDR.State == FDR.FDRStates.STATE_PREACTIVE && (Network.Me.IsRealATC || MainForm.IsDebug))
        {
            MMI.EstFDR(FDR);
        }

        if (CurrentBay == StripBay.BAY_PREA)
        {
            Util.ShowWarnBox("You have coordinated this strip while it is in your Preactive Bay.\nYou will no longer be able make changes to the flight plan!\nOpen the vatSys Flight Plan window and deactivate the strip if you still need to make changes to SID, RWY or Altitude.");
        }
    }

    /// <summary>
    /// Sends a strip deletion message to the server.
    /// </summary>
    public void SendDeleteMessage()
    {
        _socketConn.SyncDeletion(this);
    }

    /// <summary>
    /// Creates control for strip.
    /// </summary>
    public void CreateStripObj()
    {
        StripHolderControl = new Panel
        {
            BackColor = Color.FromArgb(193, 230, 242),
        };
        if (ArrDepType == StripArrDepType.ARRIVAL)
        {
            StripHolderControl.BackColor = Color.FromArgb(255, 255, 160);
        }

        StripHolderControl.Padding = new(3);
        StripHolderControl.Margin = new(0);

        ////stripHolderControl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        StripHolderControl.Size = new(100, 100);
        if (OzStripsSettings.Default.UseBigStrips)
        {
            _stripControl = new Strip(this);
        }
        else
        {
            _stripControl = new LittleStrip(this);
        }

        _stripControl.Initialise();
        _stripControl.UpdateStrip();
        _stripControl.HMI_TogglePick(_bayManager.PickedController == this);

        StripHolderControl.Size = _stripControl.Size with { Height = _stripControl.Size.Height + 6 };
        StripHolderControl.Controls.Add(_stripControl);
    }

    /// <summary>
    /// Removes items from the strip holder control.
    /// </summary>
    public void ClearStripControl()
    {
        StripHolderControl?.Controls.Clear();
    }

    /// <summary>
    /// Refreshes strip properties, determines if strip should be removed.
    /// </summary>
    public void UpdateFDR()
    {
        _stripControl?.UpdateStrip();

        var distance = GetDistToAerodrome(_bayManager.AerodromeName);

        if (distance is -1 or > 50 && ArrDepType == StripArrDepType.DEPARTURE)
        {
            _bayManager.DeleteStrip(this);
        }
    }

    /// <summary>
    /// Determines which bay to move strip to.
    /// </summary>
    public void SIDTrigger()
    {
        Dictionary<StripBay, StripBay> stripBayResultDict;
        switch (ArrDepType)
        {
            case StripArrDepType.ARRIVAL:
                stripBayResultDict = NextBayArr;
                break;
            case StripArrDepType.DEPARTURE:
                stripBayResultDict = NextBayDep;
                break;
            default:
                return;
        }

        if (stripBayResultDict.TryGetValue(CurrentBay, out var nextBay))
        {
            CurrentBay = nextBay;
            _bayManager.UpdateBay(this);
            SyncStrip();
        }
    }

    /// <summary>
    /// Toggles the pick state.
    /// </summary>
    public void TogglePick()
    {
        if (_bayManager.PickedController == this)
        {
            _bayManager.SetPicked(true);
        }
        else
        {
            _bayManager.SetPicked(this, true);
        }
    }

    /// <summary>
    /// Determines if the strip is applicable to the passed aerodrome.
    /// </summary>
    /// <param name="name">The name of the aerodrome to check.</param>
    /// <returns>True if it is applicable false otherwise.</returns>
    public bool ApplicableToAerodrome(string name)
    {
        return FDR.DepAirport == name || FDR.DesAirport == name;
    }

    /// <summary>
    /// Gets the distance to the specified aerodrome.
    /// </summary>
    /// <param name="aerodrome">The aerodrome to check.</param>
    /// <returns>The distance, or -1 if it is unable to be determined.</returns>
    public double GetDistToAerodrome(string aerodrome)
    {
        try
        {
            var adCoord = Airspace2.GetAirport(aerodrome)?.LatLong;
            var planeCoord = FDR.PredictedPosition.Location;
            var radarTracks = (from radarTrack in RDP.RadarTracks
                               where radarTrack.ActualAircraft.Callsign == FDR.Callsign
                               select radarTrack).ToList();

            if (radarTracks.Count > 0)
            {
                foreach (var rTrack in radarTracks)
                {
                    planeCoord = rTrack.ActualAircraft.Position;
                }
            }

            if (adCoord != null)
            {
                return Conversions.CalculateDistance(adCoord, planeCoord);
            }
        }
        catch
        {
        }

        return -1;
    }

    /// <summary>
    /// Gets the radar track.
    /// </summary>
    /// <returns>The tradar track or null if none available.</returns>
    public RDP.RadarTrack? GetRadarTrack()
    {
        try
        {
            var radarTracks = RDP.RadarTracks
                .Where(radarTrack => radarTrack.ActualAircraft.Callsign.Equals(FDR.Callsign))
                .ToList();
            return radarTracks.Count > 0 ? radarTracks[0] : null;
        }
        catch (Exception e)
        {
            Errors.Add(e, "OzStrips");
            return null;
        }
    }

    /// <summary>
    /// Opens the VATSYS flight data record.
    /// </summary>
    public void OpenVatsysFDR()
    {
        MMI.OpenFPWindow(FDR);
    }

    /// <summary>
    /// Sync the strip with the server.
    /// </summary>
    public void SyncStrip()
    {
        _socketConn.SyncSC(this);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _stripControl?.Dispose();
        StripHolderControl?.Dispose();
    }
}
