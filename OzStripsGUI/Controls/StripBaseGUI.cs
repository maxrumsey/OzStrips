using System;
using System.Drawing;
using System.Windows.Forms;

using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// The strip base.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="StripBaseGUI"/> class.
/// </remarks>
/// <param name="stripController">The strip controller.</param>
public abstract class StripBaseGUI(StripController stripController) : UserControl
{
    /// <summary>
    /// Gets the strip controller.
    /// </summary>
    protected StripController StripController { get; } = stripController;

    /// <summary>
    /// Gets the flight data record.
    /// </summary>
    protected FDP2.FDR FDR { get; } = stripController.FDR;

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
    /// Updates the strip.
    /// </summary>
    public abstract void UpdateStrip();

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
