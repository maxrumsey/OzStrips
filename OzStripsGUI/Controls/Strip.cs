using System;
using System.Drawing;
using System.Globalization;
using System.Linq;

using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// A UI for the strips.
/// </summary>
public partial class Strip : StripBaseGUI
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Strip"/> class.
    /// </summary>
    /// <param name="controller">The strip controller.</param>
    public Strip(StripController controller)
        : base(controller)
    {
        InitializeComponent();

        PickToggleControl = pl_acid;

        CrossColourControls =
        [
            pl_clx,
            pl_rwy,
            pl_route,
            pl_hdg,
            pl_alt,
            pl_req,
        ];

        CockColourControls =
        [
            pl_std,
            pl_ades,
            pl_eobt,
            pl_wtc,
            panel3,
            pl_frul,
            pl_ssricon,
            pl_type,
            pl_ssr,
        ];

        UpdateStrip();
    }

    /// <inheritdoc/>
    public override void UpdateStrip()
    {
        SuspendLayout();
        if (FDR == null)
        {
            return;
        }

        if (lb_eobt != null)
        {
            lb_eobt.Text = StripController.Time;
        }

        lb_acid.Text = FDR.Callsign;
        lb_ssr.Text = (FDR.AssignedSSRCode == -1) ? "XXXX" : Convert.ToString(FDR.AssignedSSRCode, 8).PadLeft(4, '0');
        lb_type.Text = FDR.AircraftType;
        lb_frul.Text = FDR.FlightRules;

        var rteItem = FDR.Route.Split(' ').ToList().Find(x => !x.Contains("/"));
        if (rteItem == null)
        {
            rteItem = FDR.Route;
        }

        if (lb_route != null)
        {
            lb_route.Text = rteItem;
        }

        if (lb_sid != null)
        {
            lb_sid.Text = StripController.SID;
        }

        if (lb_ades != null)
        {
            lb_ades.Text = FDR.DesAirport;
        }

        if (lb_alt != null)
        {
            lb_alt.Text = StripController.CFL;
        }

        if (lb_hdg != null)
        {
            lb_hdg.Text = string.IsNullOrEmpty(StripController.HDG) ? string.Empty : "H" + StripController.HDG;
        }

        if (lb_clx != null)
        {
            lb_clx.Text = StripController.CLX;
        }

        if (lb_std != null)
        {
            lb_std.Text = StripController.Gate;
        }

        if (lb_remark != null)
        {
            lb_remark.Text = StripController.Remark;
        }

        if (lb_tot != null && StripController.TakeOffTime != null)
        {
            var diff = DateTime.UtcNow - StripController.TakeOffTime;
            lb_tot.Text = diff.ToString();
            lb_tot.ForeColor = Color.Green;
        }
        else if (lb_tot != null)
        {
            lb_tot.Text = "00:00";
            lb_tot.ForeColor = Color.Black;
        }

        if (lb_req != null)
        {
            lb_req.Text = (FDR.RFL / 100).ToString(CultureInfo.InvariantCulture);
        }

        if (lb_glop != null)
        {
            lb_glop.Text = FDR.GlobalOpData;
        }

        if (lb_ssricon != null && StripController.SquawkCorrect)
        {
            lb_ssricon.Text = "*";
        }
        else if (lb_ssricon != null)
        {
            lb_ssricon.Text = string.Empty;
        }

        SetCross(false);
        Cock(0, false, false);
        lb_rwy.Text = StripController.RWY;
        lb_wtc.Text = FDR.AircraftWake;
        ResumeLayout();
    }

    private void SidClicked(object sender, EventArgs e)
    {
        StripController.SIDTrigger();
    }

    private void OpenHdgAlt(object sender, EventArgs e)
    {
        OpenHdgAltModal();
    }

    private void OpenFDR(object sender, EventArgs e)
    {
        OpenVatsysFDRModMenu();
    }

    private void AcidClicked(object sender, EventArgs e)
    {
        TogglePick();
    }

    private void SSRClicked(object sender, EventArgs e)
    {
        AssignSSR();
    }

    private void OpenCLXBay(object sender, EventArgs e)
    {
        OpenCLXBayModal();
    }

    private void TOTClicked(object sender, EventArgs e)
    {
        StripController.TakeOff();
    }

    private void EOBTClicked(object sender, EventArgs e)
    {
        Cock(-1);
    }
}
