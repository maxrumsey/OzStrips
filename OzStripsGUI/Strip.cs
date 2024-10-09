using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
public sealed class Strip
{
    private static readonly Regex _headingRegex = new(@"H(\d{3})");
    private static readonly Regex _routeRegex = new(@"^[^\d/]+$");
    private static readonly Regex _sidRouteRegex = new(@"^[\w\d]+\/\d\d");
    private static readonly Regex _gpscoordRegex = new(@"^[\d]+\w[\d]+\w");

    private readonly BayManager _bayManager;
    private readonly SocketConn _socketConn;
    ////private readonly StripLayoutTypes StripType;

    private bool _crossing;

    /// <summary>
    /// Initializes a new instance of the <see cref="Strip"/> class.
    /// </summary>
    /// <param name="fdr">The flight data record.</param>
    /// <param name="bayManager">Gets or sets the bay manager.</param>
    /// <param name="socketConn">The socket connection.</param>
    public Strip(FDR fdr, BayManager bayManager, SocketConn socketConn)
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

        Controller = new StripController(this);
    }

    /// <summary>
    /// Gets or sets the take off time.
    /// </summary>
    public DateTime? TakeOffTime { get; set; }

    /// <summary>
    /// Gets or sets the valid routes.
    /// </summary>
    public RouteDTO[]? ValidRoutes { get; set; }

    /// <summary>
    /// Gets or sets the condensed route.
    /// </summary>
    public string? CondensedRoute { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether or not a list of valid routes has been selected.
    /// </summary>
    public DateTime RequestedRoutes { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// Gets or sets a value indicating whether or not a list of valid routes has been compared to the current route.
    /// </summary>
    public bool ParsedRoutes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not an aircraft has filed a dodgy route.
    /// </summary>
    public bool DodgyRoute { get; set; }

    /// <summary>
    /// Gets the ICAO name of the aerodrome being controlled.
    /// </summary>
    public string ParentAerodrome { get; }

    /// <summary>
    /// Gets the strip view controller.
    /// </summary>
    public StripController Controller { get; }

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
    /// Gets or sets a value indicating whether the strip is ready for departure.
    /// </summary>
    public bool Ready { get; set; }

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
            return FDR.Route.Split(' ').ToList().Find(x => _routeRegex.Match(x).Success && (x != "DCT")) ?? FDR.Route;
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
                Util.LogError(new("Attempted to set invalid SID"));
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
    public static implicit operator StripControllerDTO(Strip sc)
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
            ready = sc.Ready,
        };

        return scDTO;
    }

    /// <summary>
    /// Converts a strip controller to the data object.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public static implicit operator SCDeletionDTO(Strip sc)
    {
        var scDTO = new SCDeletionDTO
        {
            acid = sc.FDR.Callsign,
        };

        return scDTO;
    }

    /// <summary>
    /// Sets the HMI picked state.
    /// </summary>
    /// <param name="picked">True if picked, false otherwise.</param>
    public void SetHMIPicked(bool picked)
    {
        Controller?.HMI_TogglePick(picked);
    }

    /// <summary>
    /// Cocks the selected strip.
    /// </summary>
    public void CockStrip()
    {
        Controller?.Cock(-1);
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
    /// Determines whether or not a SC is still valid (or should be kept alive).
    /// </summary>
    /// <returns>Valid SC.</returns>
    public bool DetermineSCValidity()
    {
        if (FDR is null)
        {
            Util.LogError(new("Strip deleted due to non-existence of vatsys FDR."));
            return false;
        }

        if (Network.GetOnlinePilots.Find(x => x.Callsign == FDR.Callsign) is null)
        {
            return false;
        }

        if (FDR.State < FDR.FDRStates.STATE_PREACTIVE && ArrDepType == StripArrDepType.DEPARTURE)
        {
            return false;
        }

        var distance = GetDistToAerodrome(_bayManager.AerodromeName);

        if (distance is -1 or > 50 && ArrDepType == StripArrDepType.DEPARTURE)
        {
            return false;
        }

        if (!ApplicableToAerodrome(_bayManager.AerodromeName))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Refreshes strip properties, determines if strip should be removed.
    /// </summary>
    public void UpdateFDR()
    {
        if (!DetermineSCValidity())
        {
            _bayManager.BayRepository.DeleteStrip(this);
            return;
        }

        // Route fetch will retry every minute.
        if (ValidRoutes is null && (RequestedRoutes == DateTime.MaxValue || (DateTime.Now - RequestedRoutes) > TimeSpan.FromMinutes(1)))
        {
            _socketConn.RequestRoutes(this);
            RequestedRoutes = DateTime.Now;
        }

        if (ValidRoutes is not null)
        {
            CondensedRoute = CleanVatsysRoute(FDR.Route);
            DodgyRoute = true;
            foreach (var validroute in ValidRoutes)
            {
                if (validroute.route.Contains(CondensedRoute))
                {
                    DodgyRoute = false;
                }
            }

            if (ArrDepType != StripArrDepType.DEPARTURE || (int)FDR.State > 5)
            {
                DodgyRoute = false;
            }
            else
            {
                // account for situations where aircraft joins route from interim point via sid.
                if (DodgyRoute)
                {
                    var rte = string.Join(" ", CleanVatsysRoute(FDR.Route).Split(' ').Skip(1).ToArray());
                    foreach (var validroute in ValidRoutes)
                    {
                        if (validroute.route.Contains(rte))
                        {
                            DodgyRoute = false;
                        }
                    }
                }

                if (!DodgyRoute && CondensedRoute == "FAIL")
                {
                    DodgyRoute = true;
                }
            }
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
            _bayManager.RemovePicked(true);
        }
        else
        {
            _bayManager.SetPickedStripItem(this, true);
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
            var planeCoord = FDR.PredictedPosition?.Location;
            var radarTracks = (from radarTrack in RDP.RadarTracks
                               where radarTrack.ActualAircraft.Callsign == FDR.Callsign
                               select radarTrack).ToList();

            if (radarTracks.Count > 0)
            {
                foreach (var rTrack in radarTracks)
                {
                    planeCoord = rTrack.ActualAircraft?.Position;
                }
            }

            if (adCoord is not null && planeCoord is not null)
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
                .Where(radarTrack => radarTrack?.ActualAircraft?.Callsign?.Equals(FDR.Callsign) == true)
                .ToList();
            return radarTracks.Count > 0 ? radarTracks[0] : null;
        }
        catch (Exception e)
        {
            Util.LogError(e);
            return null;
        }
    }

    /// <summary>
    /// To string method.
    /// </summary>
    /// <returns>Description.</returns>
    public override string ToString()
    {
        return FDR.Callsign;
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

    private static string CleanVatsysRoute(string rawRoute)
    {
        try
        {
            var rawRouteArr = rawRoute.Split(' ');
            var routeArr = new List<string>();

            foreach (var routeElement in rawRouteArr)
            {
                if (routeElement.Contains("/"))
                {
                    // Don't include SIDs or gps coords in route
                    if (!_sidRouteRegex.Match(routeElement).Success && !_gpscoordRegex.Match(routeElement).Success)
                    {
                        routeArr.Add(routeElement.Split('/').First());
                    }
                }
                else if (routeElement != "DCT")
                {
                    routeArr.Add(routeElement);
                }
            }

            if (routeArr.Count < 3)
            {
                return "FAIL";
            }

            /*
                * Remove SIDs and STARS
                */
            if (char.IsNumber(routeArr.Last().Last()))
            {
                routeArr.RemoveAt(routeArr.Count - 1);
            }

            if (char.IsNumber(routeArr.First().Last()))
            {
                routeArr.RemoveAt(0);
            }

            /*
                * Remove first and last waypoint incase they have filed / are cleared via a SID.
                * (Will validate based on route only)
                */
            if (routeArr.Count < 3)
            {
                return "FAIL";
            }

            if (!routeArr.First().Any(char.IsDigit))
            {
                routeArr.RemoveAt(0);
            }

            if (!routeArr.Last().Any(char.IsDigit))
            {
                routeArr.RemoveAt(routeArr.Count - 1);
            }

            return string.Join(" ", routeArr);
        }
        catch (Exception ex)
        {
            Util.LogText($"PARSER, RTE: {rawRoute}, ERR: {ex.Message}\n{ex.StackTrace}");
            return "FAIL";
        }
    }
}
