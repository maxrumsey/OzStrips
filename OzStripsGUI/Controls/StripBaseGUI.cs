using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using vatsys;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// The strip base.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="StripBaseGUI"/> class.
/// </remarks>
public class StripBaseGUI : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StripBaseGUI"/> class.
    /// </summary>
    /// <param name="stripController">The Strip Controller.</param>
    public StripBaseGUI(StripController stripController)
    {
        StripController = stripController;
        FDR = stripController.FDR;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StripBaseGUI"/> class.
    /// Used exclusively in Design-Time.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public StripBaseGUI()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    /// <summary>
    /// Gets the strip controller.
    /// </summary>
    protected StripController StripController { get; }

    /// <summary>
    /// Gets the flight data record.
    /// </summary>
    protected FDP2.FDR FDR { get; }

    /// <summary>
    /// Gets or sets the pick toggle control.
    /// </summary>
    protected Panel? PickToggleControl { get; set; }

    /// <summary>
    /// Gets or sets the cross colour controls.
    /// </summary>
    protected Panel[] CrossColourControls { get; set; } = [];

    /// <summary>
    /// Gets or sets the cock colour controls.
    /// </summary>
    protected Panel[] CockColourControls { get; set; } = [];

    /// <summary>
    /// Gets or sets the default color.
    /// </summary>
    protected Color DefColor { get; set; } = Color.Empty;

    /// <summary>
    /// Gets or sets a dictionary containg strip controls.
    /// </summary>
    protected Dictionary<string, Control> StripElements { get; set; } = new Dictionary<string, Control>();

    /// <summary>
    /// Gets or sets a dictionary containg strip tooltips.
    /// </summary>
    protected Dictionary<string, ToolTip> StripToolTips { get; set; } = new Dictionary<string, ToolTip>();

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
            cockLevel = StripController.CockLevel + 1;
            if (cockLevel >= 2)
            {
                cockLevel = 0;
            }
        }

        if (update)
        {
            StripController.CockLevel = cockLevel;
        }

        var marginLeft = 0;
        var color = Color.Empty;
        if (StripController.CockLevel == 1)
        {
            marginLeft = 30;
            color = Color.Cyan;
        }

        foreach (var pl in CockColourControls)
        {
            pl.BackColor = color;
        }

        if (StripController.StripHolderControl is not null)
        {
            StripController.StripHolderControl.Margin = new(marginLeft, 0, 0, 0);
        }

        if (sync)
        {
            StripController.SyncStrip();
        }
    }

    /// <summary>
    /// Sets the strip to cross.
    /// </summary>
    /// <param name="sync">If the cross should be synced.</param>
    public void SetCross(bool sync = true)
    {
        var color = DefColor;
        if (StripController.Crossing)
        {
            color = Color.Salmon;
        }

        foreach (Control control in CrossColourControls)
        {
            control.BackColor = color;
        }

        if (sync)
        {
            StripController.SyncStrip();
        }
    }

    /// <summary>
    /// Initialises the form.
    /// </summary>
    public void Initialise()
    {
        Dock = DockStyle.Fill;
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
            MMI.OpenCFLMenu(MMI.FindTrack(FDR), Cursor.Position);
        }
        else
        {
            OpenHdgAltModal();
        }
    }

    /// <summary>
    /// Opens the HDG window.
    /// </summary>
    public void OpenHDGWindow()
    {
        OpenHdgAltModal();
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
    /// Updates the strip.
    /// </summary>
    public void UpdateStrip()
    {
        SuspendLayout();
        if (FDR == null)
        {
            return;
        }

        if (StripElements.ContainsKey("eobt"))
        {
            StripElements["eobt"].Text = StripController.Time;
        }

        StripElements["acid"].Text = FDR.Callsign;
        StripElements["ssr"].Text = (FDR.AssignedSSRCode == -1) ? "XXXX" : Convert.ToString(FDR.AssignedSSRCode, 8).PadLeft(4, '0');
        StripElements["type"].Text = FDR.AircraftType;
        StripElements["frul"].Text = FDR.FlightRules;

        if (StripElements.ContainsKey("route"))
        {
            StripElements["route"].Text = StripController.FirstWpt;
        }

        if (StripToolTips.ContainsKey("routetooltip") && StripElements.ContainsKey("route"))
        {
            StripToolTips["routetooltip"].SetToolTip(StripElements["route"], StripController.Route);
        }

        if (StripElements.ContainsKey("sid"))
        {
            StripElements["sid"].Text = StripController.SID;
        }

        if (StripElements.ContainsKey("ades"))
        {
            StripElements["ades"].Text = FDR.DesAirport;
        }

        if (StripElements.ContainsKey("CFL"))
        {
            StripElements["CFL"].Text = StripController.CFL;

            if (StripElements.ContainsKey("rfl") && StripController.ArrDepType == StripArrDepType.DEPARTURE)
            {
                try
                {
                    StripElements["CFL"].BackColor = DetermineCFLBackColour();
                }
                catch
                {
                }
            }
        }

        if (StripElements.ContainsKey("HDG"))
        {
            StripElements["HDG"].Text = string.IsNullOrEmpty(StripController.HDG) ? string.Empty : "H" + StripController.HDG;
        }

        if (StripElements.ContainsKey("CLX"))
        {
            StripElements["CLX"].Text = StripController.CLX;
        }

        if (StripElements.ContainsKey("stand"))
        {
            StripElements["stand"].Text = StripController.Gate;
        }

        if (StripElements.ContainsKey("remark"))
        {
            StripElements["remark"].Text = StripController.Remark;
        }

        if (StripElements.ContainsKey("tot") && StripController.TakeOffTime != null)
        {
            var diff = (TimeSpan)(DateTime.UtcNow - StripController.TakeOffTime);
            StripElements["tot"].Text = diff.ToString(@"mm\:ss", CultureInfo.InvariantCulture);
            StripElements["tot"].ForeColor = Color.Green;
        }
        else if (StripElements.ContainsKey("tot"))
        {
            StripElements["tot"].Text = "00:00";
            StripElements["tot"].ForeColor = Color.Black;
        }

        if (StripElements.ContainsKey("rfl"))
        {
            StripElements["rfl"].Text = StripController.RFL;
        }

        if (StripElements.ContainsKey("glop"))
        {
            StripElements["glop"].Text = FDR.GlobalOpData;
        }

        if (StripElements.ContainsKey("ssrsymbol") && StripController.SquawkCorrect)
        {
            StripElements["ssrsymbol"].Text = "*";
        }
        else if (StripElements.ContainsKey("ssrsymbol"))
        {
            StripElements["ssrsymbol"].Text = string.Empty;
        }

        SetCross(false);
        Cock(0, false, false);
        StripElements["rwy"].Text = StripController.RWY;
        StripElements["wtc"].Text = FDR.AircraftWake;
        ResumeLayout();
    }

    /// <summary>
    /// Toggles display of the route for the strip.
    /// </summary>
    protected void ToggleRoute()
    {
        var track = MMI.FindTrack(FDR);
        if (track is not null)
        {
            if (track.GraphicRTE)
            {
                MMI.HideGraphicRoute(track);
            }
            else
            {
                MMI.ShowGraphicRoute(track);
            }
        }
    }

    /// <summary>
    /// Determines the colour of the CFL highlight.
    /// </summary>
    /// <returns>Colour.</returns>
    protected Color DetermineCFLBackColour()
    {
        var first = FDR.ParsedRoute.First().Intersection.LatLong;
        var last = FDR.ParsedRoute.Last().Intersection.LatLong;

        if (first == last)
        {
            return Color.Transparent;
        }

        var track = Conversions.CalculateTrack(first, last);
        var variation = LogicalPositions.Positions.Where(e => e.Name == StripController.ParentAerodrome).FirstOrDefault().MagneticVariation;

        track += variation;

        var even = true;

        if (track >= 0 && track < 180)
        {
            even = false;
        }

        var digit = int.Parse(StripController.RFL[1].ToString(), CultureInfo.InvariantCulture);
        var shouldbeeven = digit % 2 == 0;

        var colour = Color.Transparent;
        if (even != shouldbeeven)
        {
            colour = Color.OrangeRed;
            StripToolTips["cfltooltip"].Active = true;
        }
        else
        {
            StripToolTips["cfltooltip"].Active = false;
        }

        return colour;
    }

    /// <summary>
    /// Opens the heading/altitude modal dialog.
    /// </summary>
    protected void OpenHdgAltModal()
    {
        var modalChild = new AltHdgControl(StripController);
        var bm = new BaseModal(modalChild, "ACD Menu :: " + StripController.FDR.Callsign);
        modalChild.BaseModal = bm;
        bm.ReturnEvent += HeadingAltReturned;
        bm.Show(MainForm.MainFormInstance);
    }

    /// <summary>
    /// Opens the Clearance bya modal.
    /// </summary>
    protected void OpenCLXBayModal()
    {
        var modalChild = new BayCLXControl(StripController);
        var bm = new BaseModal(modalChild, "SMC Menu :: " + StripController.FDR.Callsign);
        modalChild.BaseModal = bm;
        bm.ReturnEvent += CLXBayReturned;
        bm.Show(MainForm.MainFormInstance);
    }

    /// <summary>
    /// Opens that VATSYS flight data record mod menu.
    /// </summary>
    protected void OpenVatsysFDRModMenu()
    {
        ////MMI.OpenFPWindow(stripController.fdr);
        StripController.OpenVatsysFDR();
    }

    /// <summary>
    /// Toggles the pick.
    /// </summary>
    protected void TogglePick()
    {
        StripController.TogglePick();
    }

    /// <summary>
    /// Assigns a squawk.
    /// </summary>
    protected void AssignSSR()
    {
        if (FDR.AssignedSSRCode == -1)
        {
            FDP2.SetASSR(StripController.FDR);
        }
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
                StripController.CFL = control.Alt;
            }

            StripController.HDG = control.Hdg;
            if (!string.IsNullOrEmpty(control.Runway) && StripController.RWY != control.Runway)
            {
                StripController.RWY = control.Runway;
            }

            if (!string.IsNullOrEmpty(control.SID) && StripController.SID != control.SID)
            {
                StripController.SID = control.SID;
            }

            StripController.SyncStrip();
        }
        catch (Exception)
        {
        }
    }

    private void CLXBayReturned(object source, ModalReturnArgs args)
    {
        var control = (BayCLXControl)args.Child;
        StripController.CLX = control.CLX;
        StripController.Gate = control.Gate;
        StripController.Remark = control.Remark;
        FDP2.SetGlobalOps(StripController.FDR, control.Glop);
        StripController.SyncStrip();
    }
}
