﻿using System;
using System.Windows.Forms;

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
            pl_rte,
            pl_frul,
            pl_ssricon,
            pl_type,
            pl_ssr,
        ];

        // Ugly, but moves strip display logic into the base gui.
        StripElements.Add("eobt", lb_eobt);
        StripElements.Add("acid", lb_acid);
        StripElements.Add("ssr", lb_ssr);
        StripElements.Add("type", lb_type);
        StripElements.Add("frul", lb_frul);
        StripElements.Add("route", lb_route);
        StripElements.Add("sid", lb_sid);
        StripElements.Add("ades", lb_ades);
        StripElements.Add("CFL", lb_alt);
        StripElements.Add("HDG", lb_hdg);
        StripElements.Add("CLX", lb_clx);
        StripElements.Add("stand", lb_std);
        StripElements.Add("remark", lb_remark);
        StripElements.Add("tot", lb_tot);
        StripElements.Add("rfl", lb_req);
        StripElements.Add("glop", lb_glop);
        StripElements.Add("ssrsymbol", lb_ssricon);
        StripElements.Add("rwy", lb_rwy);
        StripElements.Add("wtc", lb_wtc);

        UpdateStrip();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Strip"/> class.
    /// This function is used exclusively in design-time.
    /// </summary>
    public Strip()
    {
        InitializeComponent();
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

    private void RouteClicked(object sender, EventArgs e)
    {
        ToggleRoute();
    }

    private void LBAltClicked(object sender, EventArgs e)
    {
        OpenCFLWindow();
    }

    private void LBHdgClicked(object sender, EventArgs e)
    {
        OpenHDGWindow();
    }

    private void LBRwyClicked(object sender, EventArgs e)
    {
        OpenRWYWindow();
    }

    private void SidClicked(object sender, EventArgs e)
    {
        var me = (MouseEventArgs)e;
        if (me.Button == MouseButtons.Right)
        {
            OpenSIDWindow();
        }
        else
        {
            StripController.SIDTrigger();
        }
    }
}
