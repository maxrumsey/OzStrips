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
    private static readonly Regex _firstWptLatLongRegex = new(@"^[^\d/]+$");
    private static readonly Regex _sidRouteRegex = new(@"^[\w\d]+\/[\d]{2}\w?");
    private static readonly Regex _gpscoordRegex = new(@"^[\d]+\w[\d]+\w");
    private static readonly Regex _airwayRegex = new(@"^\w\d+");

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
        if (StripType == StripType.ARRIVAL)
        {
            CurrentBay = StripBay.BAY_ARRIVAL;
        }

        OriginalAerodromePair = FDR.DepAirport + FDR.DesAirport;

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
    /// Gets or sets a string containing the aerodrome pair that valid routes were fetched for.
    /// </summary>
    public string OriginalAerodromePair { get; set; } = string.Empty;

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
    /// Gets the strip key.
    /// </summary>
    public StripKey StripKey
    {
        get
        {
            return new StripKey()
            {
                Callsign = FDR.Callsign,
                VatsimID = Network.GetOnlinePilots.Find(x => x?.Callsign == FDR.Callsign)?.ID ?? string.Empty,
                ADEP = FDR.DepAirport,
                ADES = FDR.DesAirport,
            };
        }
    }

    /// <summary>
    /// Gets the default or original strip type, regardless of any overrides.
    /// </summary>
    public StripType DefaultStripType
    {
        get
        {
            if (FDR.DesAirport == _bayManager.AerodromeName && FDR.DepAirport == _bayManager.AerodromeName)
            {
                return StripType.LOCAL;
            }

            if (FDR.DesAirport == _bayManager.AerodromeName)
            {
                return StripType.ARRIVAL;
            }

            if (FDR.DepAirport == _bayManager.AerodromeName)
            {
                return StripType.DEPARTURE;
            }

            return StripType.UNKNOWN;
        }
    }

    /// <summary>
    /// Gets the arrival or departure type.
    /// </summary>
    public StripType StripType
    {
        get
        {
            var originalType = DefaultStripType;

            if (originalType != StripType.LOCAL)
            {
                return originalType;
            }
            else
            {
                if (OverrideStripType == StripType.UNKNOWN)
                {
                    OverrideStripType = StripType.DEPARTURE;
                }

                return OverrideStripType;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value which when set will override the default arr/dep type.
    /// </summary>
    public StripType OverrideStripType { get; set; } = StripType.UNKNOWN;

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

            /*
             * Only blank GLOPs when a heading is in the GLOP.
             */
            else if ((Network.Me.IsRealATC || MainForm.IsDebug) && _headingRegex.Match(FDR.GlobalOpData).Success)
            {
                SetGlobalOps(FDR, string.Empty);
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
            if (StripType == StripType.DEPARTURE && FDR.ATD == DateTime.MaxValue)
            {
                return FDR.ETD.ToString("HHmm", CultureInfo.InvariantCulture);
            }

            return StripType == StripType.DEPARTURE ?
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
            if ((StripType == StripType.DEPARTURE || StripType == StripType.LOCAL) && FDR.DepartureRunway != null)
            {
                return FDR.DepartureRunway.Name;
            }

            return StripType == StripType.ARRIVAL && FDR.ArrivalRunway != null ? FDR.ArrivalRunway.Name : string.Empty;
        }

        set
        {
            if (DefaultStripType == StripType.DEPARTURE || DefaultStripType == StripType.LOCAL)
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
    /// Gets the SID transition for the strip, if applicable.
    /// </summary>
    public string? SIDTransition
    {
        get
        {
            if (FDR.SID is null || FDR.SID.Transitions.Count == 0 || StripType != StripType.DEPARTURE)
            {
                return null;
            }

            var wpt = FirstWpt;

            if (FDR.SID.Transitions.ContainsKey(wpt))
            {
                return wpt;
            }

            return "RADAR";
        }
    }

    /// <summary>
    /// Gets the first element in the route.
    /// Don't include first waypoint, and if going DCT, mark first element as DCT.
    /// </summary>
    public string FirstWpt
    {
        get
        {
            var aerodrome = FDR.DepAirport;
            var firstWptUnsuitableMatches = new List<string> { aerodrome };

            firstWptUnsuitableMatches.AddRange(Util.DifferingAerodromeWaypoints.Where(x => x.Key == aerodrome).Select(x => x.Value));
            var wpt = string.Empty;

            // Procedural SIDs
            if (FDR.SID?.Name.Length > 3)
            {
                /*
                 * Don't Match:
                 * SID Names
                 * DCTs
                 * YMML/ ML/ TESATs etc
                 */
                wpt = FDR.RouteNoParse.Split(' ').ToList().Find(x => _firstWptLatLongRegex.IsMatch(x) && (x != "DCT") && !firstWptUnsuitableMatches.Where(unsuitMatch => unsuitMatch.Contains(x)).Any()) ?? "DCT";
            }
            else if (FDR.SID != null)
            {
                // Radar SIDs
                /*
                 * Don't Match:
                 * YMML/ TESAT/ ML etc
                 * GPS wpts (due to SY3 issue)
                 */
                wpt = FDR.ParsedRoute.Where(x => x.Type == FDR.ExtractedRoute.Segment.SegmentTypes.WAYPOINT && x.SIDSTARName.Length == 0 && _firstWptLatLongRegex.IsMatch(x.Intersection.Name)).ToList().Find(x => !firstWptUnsuitableMatches.Where(unsuitMatch => unsuitMatch.Contains(x.Intersection.Name)).Any())?.Intersection.Name ?? "DCT";
            }
            else
            {
                // Other routing (vfr).
                /*
                 * Don't Match:
                 * YMML/ TESAT/ ML etc
                 */
                var intersection = FDR.ParsedRoute.Where(x => x.Type == FDR.ExtractedRoute.Segment.SegmentTypes.WAYPOINT && x.SIDSTARName.Length == 0).ToList().Find(x => !firstWptUnsuitableMatches.Where(unsuitMatch => unsuitMatch.Contains(x.Intersection.Name)).Any());

                if (intersection is not null)
                {
                    // If latlong return fullname (will match regex);
                    // ie don't return Albany for YABA.
                    wpt = intersection.Intersection.FullName.Length > 0 && char.IsDigit(intersection.Intersection.FullName[0]) ? intersection.Intersection.FullName : intersection.Intersection.Name;
                }
                else
                {
                    // No intersection found, return DCT.
                    wpt = "DCT";
                }
            }

            if (_gpscoordRegex.IsMatch(wpt))
            {
                return "#GPS#";
            }

            return wpt;
        }
    }

    /// <summary>
    /// Gets a value indicating whether a VFR aircraft has been given a SID.
    /// </summary>
    public bool VFRSIDAssigned
    {
        get
        {
            return FDR.FlightRules == "V" && !string.IsNullOrEmpty(FDR.SIDSTARString) && StripType != StripType.ARRIVAL;
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
            if (StripType != StripType.ARRIVAL && FDR.SID is not null)
            {
                return FDR.SID.Name;
            }
            else if (StripType != StripType.ARRIVAL)
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
            var ssrCodeCorrect = rtrack?.ActualAircraft.TransponderCode == FDR.AssignedSSRCode;
            var modec = rtrack?.ActualAircraft.TransponderModeC;

            // Probably a cleaner way to do this, but I am lazy and it is 2AM.
            if (FDR.AircraftType == "A388")
            {
                return ssrCodeCorrect;
            }

            return modec == true && ssrCodeCorrect;
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
    /// Gets a dictionary which contains the arrival next state for a given state.
    /// </summary>
    private static Dictionary<StripBay, StripBay> NextBayLocal { get; } = new()
    {
        { StripBay.BAY_PREA, StripBay.BAY_CLEARED },
        { StripBay.BAY_CLEARED, StripBay.BAY_PUSHED },
        { StripBay.BAY_PUSHED, StripBay.BAY_TAXI },
        { StripBay.BAY_TAXI, StripBay.BAY_HOLDSHORT },
        { StripBay.BAY_ARRIVAL, StripBay.BAY_RUNWAY },
        { StripBay.BAY_RUNWAY, StripBay.BAY_ARRIVAL },
    };

    /// <summary>
    /// Converts a strip controller to the data object.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public static implicit operator StripDTO(Strip sc)
    {
        var scDTO = new StripDTO
        {
            bay = sc.CurrentBay,
            CLX = sc.CLX,
            GATE = sc.Gate,
            cockLevel = sc.CockLevel,
            crossing = sc.Crossing,
            remark = sc.Remark,
            TOT = sc.TakeOffTime is not null ? sc.TakeOffTime!.ToString() : "\0",
            ready = sc.Ready,
            StripKey = sc.StripKey,
            OverrideStripType = sc.OverrideStripType,
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
    /// For local strips, cycle between APP, DEP and LOC strip modes.
    /// </summary>
    public void FlipFlop()
    {
        if (DefaultStripType != StripType.LOCAL)
        {
            return;
        }

        OverrideStripType++;

        if (OverrideStripType > StripType.LOCAL)
        {
            OverrideStripType = StripType.ARRIVAL;
        }

        SyncStrip();
    }

    /// <summary>
    /// Coordinates the strip.
    /// </summary>
    public void CoordinateStrip()
    {
        if (FDR.State == FDR.FDRStates.STATE_PREACTIVE && (Network.Me.IsRealATC || MainForm.IsDebug) && StripType != StripType.LOCAL)
        {
            MMI.EstFDR(FDR);
        }

        if (CurrentBay == StripBay.BAY_PREA)
        {
            Util.ShowWarnBox("You have coordinated this strip while it is in your Preactive Bay.\nYou will no longer be able make changes to the flight plan!\nOpen the vatSys Flight Plan window and deactivate the strip if you still need to make changes to SID, RWY or Altitude.");
        }
    }

    /// <summary>
    /// Deactivates the strip.
    /// </summary>
    public void DeactivateStrip()
    {
        if (FDR.State == FDR.FDRStates.STATE_COORDINATED && (Network.Me.IsRealATC || MainForm.IsDebug))
        {
            FDP2.BackFDR(FDR);
        }
    }

    /// <summary>
    /// Check if adep or ades changed.
    /// </summary>
    /// <param name="fdr">New FDR.</param>
    /// <returns>Whether or not a change is detected.</returns>
    public bool ADESADEPPairChanged(FDR fdr)
    {
        var adPair = fdr.DepAirport + fdr.DesAirport;
        return adPair != OriginalAerodromePair;
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

        // FIN, SUS or INCT FDRs
        if (FDR.State < FDR.FDRStates.STATE_PREACTIVE && StripType == StripType.DEPARTURE)
        {
            return false;
        }

        var distance = GetDistToAerodrome(_bayManager.AerodromeName);

        if (distance is -1 or > 50 && DefaultStripType == StripType.DEPARTURE)
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
    /// Refreshes strip properties, and determines if strip should be removed.
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
                if (validroute.RouteText.Contains(CondensedRoute))
                {
                    DodgyRoute = false;
                }
            }

            // If not departure or FDR active.
            if (DefaultStripType != StripType.DEPARTURE || (int)FDR.State > 5)
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
                        if (validroute.RouteText.Contains(rte))
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
    /// Moves the strip into the "next" bay.
    /// </summary>
    public void SIDTrigger()
    {
        // TODO: do something with this.
        Dictionary<StripBay, StripBay> stripBayResultDict;

        switch (StripType)
        {
            case StripType.ARRIVAL:
                stripBayResultDict = NextBayArr;
                break;
            case StripType.DEPARTURE:
                stripBayResultDict = NextBayDep;
                break;

            case StripType.LOCAL:
                stripBayResultDict = NextBayLocal;
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
        if (_bayManager.PickedStrip == this)
        {
            _bayManager.RemovePicked(true, true);
        }
        else
        {
            _bayManager.SetPickedStripClass(this);
        }
    }

    /// <summary>
    /// Requests new strip data from the server, for new strips.
    /// </summary>
    public void FetchStripData()
    {
        _socketConn.RequestStrip(this);
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
                               where radarTrack?.ActualAircraft?.Callsign == FDR.Callsign
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
                .Where(radarTrack => radarTrack?.ActualAircraft?.Callsign == FDR.Callsign)
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

    /// <summary>
    /// Cleans up the vatsys route into a format parseable by the route checker.
    /// </summary>
    /// <param name="rawRoute">The raw route.</param>
    /// <returns>The parsed route.</returns>
    private static string CleanVatsysRoute(string rawRoute)
    {
        try
        {
            var rawRouteArr = rawRoute.Split(' ');
            var routeArr = new List<string>();

            foreach (var routeElement in rawRouteArr)
            {
                // Don't include GPS waypoints.
                if (_gpscoordRegex.Match(routeElement).Success)
                {
                    continue;
                }

                if (routeElement.Contains("/"))
                {
                    // Don't include vatsys inserted SIDs
                    if (!_sidRouteRegex.Match(routeElement).Success)
                    {
                        routeArr.Add(routeElement.Split('/').First());
                    }
                }
                else if (routeElement != "DCT")
                {
                    // Don't include funky simbrief inserted waypoints.
                    if (routeElement.Any(char.IsDigit) && !_airwayRegex.IsMatch(routeElement))
                    {
                        continue;
                    }

                    routeArr.Add(routeElement);
                }
            }

            if (routeArr.Count < 3)
            {
                return "\0";
            }

            /*
             * Remove first and last waypoint
             * (Will validate based on airways only)
             */
            if (routeArr.Count < 3)
            {
                return "\0";
            }

            if (!routeArr.First().Any(char.IsDigit))
            {
                routeArr.RemoveAt(0);
            }

            if (!routeArr.Last().Any(char.IsDigit))
            {
                routeArr.RemoveAt(routeArr.Count - 1);
            }

            // Remove intermediary wpts.
            var finalArr = new List<string>();

            for (var i = 0; i < routeArr.Count; i++)
            {
                var element = routeArr[i];

                if (i + 2 < routeArr.Count && element == routeArr[i + 2])
                {
                    i++;
                    continue;
                }

                finalArr.Add(element);
            }

            return string.Join(" ", finalArr);
        }
        catch (Exception ex)
        {
            Util.LogText($"PARSER, RTE: {rawRoute}, ERR: {ex.Message}\n{ex.StackTrace}");
            return "\0";
        }
    }
}
