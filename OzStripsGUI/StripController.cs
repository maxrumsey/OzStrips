using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.Controls;
using SkiaSharp;
using vatsys;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// The strip base.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="StripController"/> class.
/// </remarks>
public class StripController
{
    // private string _rtetooltiptext = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="StripController"/> class.
    /// </summary>
    /// <param name="stripController">The Strip Controller.</param>
    public StripController(Strip stripController)
    {
        Strip = stripController;
        FDR = stripController.FDR;
    }

    /// <summary>
    /// Gets a value indicating whether or not the CFL tooltip should be shown.
    /// </summary>
    public bool ShowCFLToolTip { get; private set; }

    /// <summary>
    /// Gets the strip controller.
    /// </summary>
    protected Strip Strip { get; }

    /// <summary>
    /// Gets the flight data record.
    /// </summary>
    protected FDP2.FDR FDR { get; }

    /// <summary>
    /// Gets or sets the pick toggle control.
    /// </summary>
    protected Panel? PickToggleControl { get; set; }

    /// <summary>
    /// Changes the cock level.
    /// </summary>
    /// <param name="cockLevel">The new cock level.</param>
    /// <param name="sync">If the cock level should be synced.</param>
    /// <param name="update">If the cock level should update.</param>
    public void Cock(int cockLevel, bool sync = true, bool update = true)
    {
        if (cockLevel == -1)
        {
            cockLevel = Strip.CockLevel + 1;
            if (cockLevel >= 2)
            {
                cockLevel = 0;
            }
        }

        if (update)
        {
            Strip.CockLevel = cockLevel;
        }

        if (sync)
        {
            Strip.SyncStrip();
        }
    }

    /// <summary>
    /// Sets the strip to cross.
    /// </summary>
    /// <param name="sync">If the cross should be synced.</param>
    public void SetCross(bool sync = true)
    {
        if (sync)
        {
            Strip.SyncStrip();
        }
    }

    /// <summary>
    /// toggles if the HMI is picked.
    /// </summary>
    /// <param name="picked">If the value is picked or not.</param>
    public void HMI_TogglePick(bool picked)
    {
        if (PickToggleControl != null)
        {
            var color = Color.Empty;
            if (picked)
            {
                color = Color.Silver;
            }

            PickToggleControl.BackColor = color;
        }
    }

    /// <summary>
    /// Opens the CFL window.
    /// </summary>
    public void OpenCFLWindow()
    {
        if (Properties.OzStripsSettings.Default.UseVatSysPopup)
        {
            var track = MMI.FindTrack(FDR);
            if (track is not null)
            {
                MMI.OpenCFLMenu(track, Cursor.Position);
            }
        }
        else
        {
            OpenHdgAltModal("cfl");
        }
    }

    /// <summary>
    /// Determines the colour of the CFL highlight.
    /// </summary>
    /// <returns>Colour.</returns>
    public SKColor DetermineCFLBackColour()
    {
        var first = FDR.ParsedRoute.First().Intersection.LatLong;
        var last = FDR.ParsedRoute.Last().Intersection.LatLong;

        int[] eastRVSM = [41000, 45000, 49000];
        int[] westRVSM = [43000, 47000, 51000];

        if (first == last)
        {
            return SKColor.Empty;
        }

        var track = Conversions.CalculateTrack(first, last);
        var positions = LogicalPositions.Positions.FirstOrDefault(e => e.Name == Strip.ParentAerodrome);
        if (positions is null)
        {
            return SKColor.Empty;
        }

        var variation = positions.MagneticVariation;
        track += variation;

        var even = true;

        if (track is >= 0 and < 180)
        {
            even = false;
        }

        var digit = int.Parse(Strip.RFL[1].ToString(), CultureInfo.InvariantCulture);
        var shouldbeeven = digit % 2 == 0;

        var colour = SKColor.Empty;
        if (even != shouldbeeven && FDR.RFL >= 3000 && Strip.ArrDepType == StripArrDepType.DEPARTURE)
        {
            colour = SKColors.OrangeRed;
            ShowCFLToolTip = true;
        }
        else
        {
            ShowCFLToolTip = false;
        }

        if (FDR.RFL >= 41000 && ((even && westRVSM.Contains(FDR.RFL)) || (!even && eastRVSM.Contains(FDR.RFL))))
        {
            colour = SKColor.Empty;
            ShowCFLToolTip = false;
        }
        else if (FDR.RFL >= 41000 && Strip.ArrDepType == StripArrDepType.DEPARTURE)
        {
            colour = SKColors.OrangeRed;
            ShowCFLToolTip = true;
        }

        return colour;
    }

    /// <summary>
    /// Determines the colour of the Route highlight.
    /// </summary>
    /// <returns>Colour.</returns>
    public SKColor DetermineRouteBackColour()
    {
        var colour = SKColor.Empty;
        if (Strip.DodgyRoute)
        {
            colour = SKColors.Orange;
        }

        return colour;
    }

    /// <summary>
    /// Opens the HDG window.
    /// </summary>
    public void OpenHDGWindow()
    {
        OpenHdgAltModal("hdg");
    }

    /// <summary>
    /// Opens the RWY window.
    /// </summary>
    public void OpenRWYWindow()
    {
        if (Properties.OzStripsSettings.Default.UseVatSysPopup)
        {
            MMI.OpenRWYMenu(FDR, Cursor.Position);
        }
        else
        {
            OpenHdgAltModal();
        }
    }

    /// <summary>
    /// Opens the Reroute window.
    /// </summary>
    public void OpenRerouteMenu()
    {
        var modalChild = new RerouteControl(Strip);
        var bm = new BaseModal(modalChild, "Reroute :: " + Strip.FDR.Callsign);

        // modalChild.BaseModal = bm;
        bm.Show(MainForm.MainFormInstance);
    }

    /// <summary>
    /// Opens the SID window.
    /// </summary>
    public void OpenSIDWindow()
    {
        if (Properties.OzStripsSettings.Default.UseVatSysPopup)
        {
            MMI.OpenSIDSTARMenu(FDR, Cursor.Position);
        }
        else
        {
            OpenHdgAltModal();
        }
    }

    /// <summary>
    /// Opens the Clearance bya modal.
    /// </summary>
    /// <param name="labelName">Label Name.</param>
    public void OpenCLXBayModal(string labelName)
    {
        var modalChild = new BayCLXControl(Strip, labelName);
        var bm = new BaseModal(modalChild, "SMC Menu :: " + Strip.FDR.Callsign);
        modalChild.BaseModal = bm;
        bm.ReturnEvent += CLXBayReturned;
        bm.Show(MainForm.MainFormInstance);
    }

    /// <summary>
    /// Assigns a squawk.
    /// </summary>
    public void AssignSSR()
    {
        if (FDR.AssignedSSRCode == -1 && Network.Me.IsRealATC)
        {
            FDP2.SetASSR(Strip.FDR);
        }
    }

    /// <summary>
    /// Toggles strip ready status.
    /// </summary>
    public void ToggleReady()
    {
        Strip.Ready = !Strip.Ready;
        Strip.SyncStrip();
    }

    /*
    public void UpdateStrip()
    {
        if (FDR == null)
        {
            return;
        }

        SetLabel("eobt", Strip.Time);

        SetLabel("acid", FDR.Callsign);
        SetLabel("ssr", (FDR.AssignedSSRCode == -1) ? "XXXX" : Convert.ToString(FDR.AssignedSSRCode, 8).PadLeft(4, '0'));
        SetLabel("type", FDR.AircraftType);
        SetLabel("frul", FDR.FlightRules);

        SetLabel("route", Strip.FirstWpt);
        SetBackColour("route", DetermineRouteBackColour());

        if (StripToolTips.ContainsKey("routetooltip"))
        {
            if (Strip.DodgyRoute)
            {
                var routes = new List<string>();
                Array.ForEach(Strip.ValidRoutes, x => routes.Add("(" + x.acft + ") " + x.route));
                var str = Strip.Route +
                                "\n---\nPotentially non-compliant route detected! Accepted Routes:\n" + string.Join("\n", routes) + "\nParsed Route: " + Strip.CondensedRoute;
                if (str != _rtetooltiptext)
                {
                    StripToolTips["routetooltip"].SetToolTip(StripElements["route"], str);
                    _rtetooltiptext = str;
                }
            }
            else
            {
                var str = Strip.Route;
                if (str != _rtetooltiptext)
                {
                    StripToolTips["routetooltip"].SetToolTip(StripElements["route"], str);
                    _rtetooltiptext = str;
                }
            }
        }

        SetLabel("sid", Strip.SID);

        SetLabel("ades", FDR.DesAirport);
        SetLabel("CFL", Strip.CFL);

        try
        {
            if (Strip.ArrDepType == StripArrDepType.DEPARTURE)
            {
                var colour = DetermineCFLBackColour();
                SetBackColour("CFL", colour);
            }
        }
        catch
        {
        }

        SetLabel("HDG", string.IsNullOrEmpty(Strip.HDG) ? string.Empty : "H" + Strip.HDG);

        SetLabel("CLX", Strip.CLX);

        SetLabel("stand", Strip.Gate);

        SetLabel("remark", Strip.Remark);

        if (Strip.TakeOffTime != null)
        {
            var diff = (TimeSpan)(DateTime.UtcNow - Strip.TakeOffTime);
            SetLabel("tot", diff.ToString(@"mm\:ss", CultureInfo.InvariantCulture));
            SetForeColour("tot", Color.Green);
        }
        else
        {
            SetLabel("tot", "00:00");
            SetForeColour("tot", Color.Black);
        }

        SetLabel("rfl", Strip.RFL);

        SetLabel("ready", Strip.Ready ? "RDY" : string.Empty);

        if (!Strip.Ready && (Strip.CurrentBay == StripBay.BAY_HOLDSHORT || Strip.CurrentBay == StripBay.BAY_RUNWAY) && Strip.ArrDepType == StripArrDepType.DEPARTURE)
        {
            SetBackColour("ready", Color.Orange);
        }
        else
        {
            SetBackColour("ready", Color.Empty);
        }

        SetLabel("glop", FDR.GlobalOpData);

        if (Strip.SquawkCorrect)
        {
            SetLabel("ssrsymbol", "*");
        }
        else
        {
            SetLabel("ssrsymbol", string.Empty);
        }

        SetCross(false);
        Cock(0, false, false);

        SetLabel("rwy", Strip.RWY);
        SetLabel("wtc", FDR.AircraftWake);

        ResumeLayout();
    }
    */

    /// <summary>
    /// Opens the heading/altitude modal dialog.
    /// </summary>
    /// <param name="activeLabel">The label item to be designated as the active control.</param>
    protected void OpenHdgAltModal(string activeLabel = "")
    {
        var modalChild = new AltHdgControl(Strip, activeLabel);
        var bm = new BaseModal(modalChild, "ACD Menu :: " + Strip.FDR.Callsign);
        modalChild.BaseModal = bm;
        bm.ReturnEvent += HeadingAltReturned;
        bm.Show(MainForm.MainFormInstance);
    }

    /// <summary>
    /// Callback for the heading alt return.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="args">The arguments.</param>
    private void HeadingAltReturned(object source, ModalReturnArgs args)
    {
        try
        {
            var control = (AltHdgControl)args.Child;
            if (!string.IsNullOrEmpty(control.Alt))
            {
                Strip.CFL = control.Alt;
            }

            Strip.HDG = control.Hdg;
            if (!string.IsNullOrEmpty(control.Runway) && Strip.RWY != control.Runway)
            {
                Strip.RWY = control.Runway;
            }

            if (!string.IsNullOrEmpty(control.SID) && Strip.SID != control.SID)
            {
                Strip.SID = control.SID;
            }

            Strip.SyncStrip();
        }
        catch
        {
        }
    }

    private void CLXBayReturned(object source, ModalReturnArgs args)
    {
        var control = (BayCLXControl)args.Child;
        Strip.CLX = control.CLX;
        Strip.Gate = control.Gate;
        Strip.Remark = control.Remark;
        FDP2.SetGlobalOps(Strip.FDR, control.Glop);
        Strip.SyncStrip();
    }
}
