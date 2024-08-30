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
public interface IStripController : IDisposable
{
    /// <summary>
    /// Gets or sets the strip holder control.
    /// </summary>
    public Control? StripHolderControl { get; set; }

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
    public string? CondensedRoute { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not a list of valid routes has been selected.
    /// </summary>
    public DateTime RequestedRoutes { get; set; }

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
    /// Gets or sets the flight data record.
    /// </summary>
    public FDR FDR { get; set; }

    /// <summary>
    /// Gets or sets the current strip bay.
    /// </summary>
    public StripBay CurrentBay { get; set; }

    /// <summary>
    /// Gets or sets the strip control.
    /// </summary>
    public StripBaseGUI? StripControl { get; set; }

    /// <summary>
    /// Gets or sets the current cock level.
    /// </summary>
    public int CockLevel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the strip is in crossing mode.
    /// </summary>
    public bool Crossing { get; set; }

    /// <summary>
    /// Gets the arrival or departure type.
    /// </summary>
    public StripArrDepType ArrDepType { get; }

    /// <summary>
    /// Gets or sets the CFL.
    /// </summary>
    public string CFL { get; set; }

    /// <summary>
    /// Gets or sets the clearance.
    /// </summary>
    public string CLX { get; set; }

    /// <summary>
    /// Gets or sets the remarks.
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// Gets he requested flight level.
    /// </summary>
    public string RFL { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the strip is ready for departure.
    /// </summary>
    public bool Ready { get; set; }

    /// <summary>
    /// Gets or sets the gate.
    /// </summary>
    public string Gate { get; set; }

    /// <summary>
    /// Gets or sets the heading.
    /// </summary>
    public string HDG { get; set; }

    /// <summary>
    /// Gets the current eobt time.
    /// </summary>
    public string Time { get; }

    /// <summary>
    /// Gets or sets the runway.
    /// </summary>
    public string RWY { get; set; }

    /// <summary>
    /// Gets the first element in the route.
    /// </summary>
    public string FirstWpt { get; }

    /// <summary>
    /// Gets the full route text in the route.
    /// </summary>
    public string Route { get; }

    /// <summary>
    /// Gets or sets the SID.
    /// </summary>
    public string SID { get; set; }

    /// <summary>
    /// Gets a list of possible departure runways.
    /// </summary>
    public List<Airspace2.SystemRunway> PossibleDepRunways { get; }

    /// <summary>
    /// Gets a value indicating whether the squawk code is correct.
    /// </summary>
    public bool SquawkCorrect { get; }

    /// <summary>
    /// Performs initial setup checks and creations.
    /// </summary>
    public void Setup();

    /// <summary>
    /// Sets the HMI picked state.
    /// </summary>
    /// <param name="picked">True if picked, false otherwise.</param>
    public void SetHMIPicked(bool picked);

    /// <summary>
    /// Cocks the selected strip.
    /// </summary>
    public void CockStrip();

    /// <summary>
    /// That the strip has taken off.
    /// </summary>
    public void TakeOff();

    /// <summary>
    /// Coordinates the strip with the server.
    /// </summary>
    public void CoordinateStrip();

    /// <summary>
    /// Sends a strip deletion message to the server.
    /// </summary>
    public void SendDeleteMessage();

    /// <summary>
    /// Creates control for strip.
    /// </summary>
    public void CreateStripObj();

    /// <summary>
    /// Removes items from the strip holder control.
    /// </summary>
    public void ClearStripControl();

    /// <summary>
    /// Determines whether or not a SC is still valid (or should be kept alive).
    /// </summary>
    /// <returns>Valid SC.</returns>
    public bool DetermineSCValidity();

    /// <summary>
    /// Refreshes strip properties, determines if strip should be removed.
    /// </summary>
    public void UpdateFDR();

    /// <summary>
    /// Determines which bay to move strip to.
    /// </summary>
    public void SIDTrigger();

    /// <summary>
    /// Toggles the pick state.
    /// </summary>
    public void TogglePick();

    /// <summary>
    /// Determines if the strip is applicable to the passed aerodrome.
    /// </summary>
    /// <param name="name">The name of the aerodrome to check.</param>
    /// <returns>True if it is applicable false otherwise.</returns>
    public bool ApplicableToAerodrome(string name);

    /// <summary>
    /// Gets the distance to the specified aerodrome.
    /// </summary>
    /// <param name="aerodrome">The aerodrome to check.</param>
    /// <returns>The distance, or -1 if it is unable to be determined.</returns>
    public double GetDistToAerodrome(string aerodrome);

    /// <summary>
    /// Gets the radar track.
    /// </summary>
    /// <returns>The tradar track or null if none available.</returns>
    public RDP.RadarTrack? GetRadarTrack();

    /// <summary>
    /// Opens the VATSYS flight data record.
    /// </summary>
    public void OpenVatsysFDR();

    /// <summary>
    /// Sync the strip with the server.
    /// </summary>
    public void SyncStrip();
}
