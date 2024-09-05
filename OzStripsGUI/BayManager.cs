using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.DTO;

using vatsys;
using static vatsys.FDP2;

// todo: separate GUI components into separate class
namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Handles the bays.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BayManager"/> class.
/// </remarks>
public class BayManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BayManager"/> class.
    /// </summary>
    /// <param name="main">The flow layout for the bay.</param>
    /// <param name="layoutMethod">The current layout caller.</param>
    public BayManager(FlowLayoutPanel main, Action<object, EventArgs> layoutMethod)
    {
        BayRepository = new(main, layoutMethod, this);
    }

    /// <summary>
    /// Gets or sets the picked controller.
    /// </summary>
    public Strip? PickedController { get; set; }

    /// <summary>
    /// Gets the strip repository.
    /// </summary>
    public StripRepository StripRepository { get; } = new StripRepository();

    /// <summary>
    /// Gets the bay repository.
    /// </summary>
    public BayRepository BayRepository { get; internal set; }

    /// <summary>
    /// Gets or sets the aerodrome name.
    /// </summary>
    public string AerodromeName { get; set; } = "????";

    /// <summary>
    /// Sets the last selected track's FDR in vatSys.
    /// </summary>
    public string? PickedCallsign
    {
        set
        {
            if (value is not null)
            {
                var sc = StripRepository.GetController(value);
                if (sc is not null)
                {
                    SetPicked(sc.FDR);
                }
            }
            else
            {
                SetPicked();
            }
        }
    }

    /// <summary>
    /// Forces a track into the first bay.
    /// </summary>
    /// <param name="socketConn">The socket connection.</param>
    public void ForceStrip(SocketConn socketConn)
    {
        if (MMI.SelectedTrack != null)
        {
            var fdr = MMI.SelectedTrack.GetFDR();
            if (fdr is null)
            {
                return;
            }

            var controller = StripRepository.UpdateFDR(fdr, this, socketConn);

            if (controller != null && BayRepository.Bays[0] != null)
            {
                controller.CurrentBay = BayRepository.Bays[0].BayTypes[0];
                controller.SyncStrip();
                controller.FDR = fdr;
                UpdateBay(controller);
            }
        }
    }

    /// <summary>
    /// SidTriggers the selected strip.
    /// </summary>
    public void SidTrigger()
    {
        if (PickedController is not null)
        {
            PickedController.SIDTrigger();
        }
    }

    /// <summary>
    /// Cocks the selected strip.
    /// </summary>
    public void CockStrip()
    {
        if (PickedController is not null)
        {
            PickedController.CockStrip();
        }
    }

    /// <summary>
    /// Inhibit the strip.
    /// </summary>
    public void Inhibit()
    {
        if (PickedController != null)
        {
            PickedController.CurrentBay = StripBay.BAY_DEAD;
            PickedController.SyncStrip();
            UpdateBay(PickedController);
            SetPicked(true);
        }
    }

    /// <summary>
    /// Send the PDC.
    /// </summary>
    public void SendPDC()
    {
        if (PickedController != null)
        {
            MMI.OpenCPDLCWindow(PickedController.FDR, null, CPDLC.MessageCategories.FirstOrDefault(m => m.Name == "PDC"));
            SetPicked(true);
        }
    }

    /// <summary>
    /// Toggles crossing highlight on a strip.
    /// </summary>
    public void CrossStrip()
    {
        if (PickedController != null)
        {
            PickedController.Crossing = !PickedController.Crossing;
            PickedController.Control?.SetCross();
            SetPicked(true);
        }
    }

    /// <summary>
    /// Drop the strip to the specified bay.
    /// </summary>
    /// <param name="bay">The bay.</param>
    public void DropStrip(Bay bay)
    {
        if (PickedController != null)
        {
            var newBay = bay.BayTypes.FirstOrDefault();
            if (newBay == PickedController.CurrentBay)
            {
                return;
            }

            PickedController.CurrentBay = newBay;
            PickedController.SyncStrip();
            UpdateBay(PickedController);
            SetPicked(true);
        }
    }

    /// <summary>
    /// Sets the aerodrome. Reinitialises various classes.
    /// </summary>
    /// <param name="name">The name of the aerodrome.</param>
    /// <param name="socketConn">The socket connection.</param>
    public void SetAerodrome(string name, SocketConn socketConn)
    {
        AerodromeName = name;
        WipeStrips();
        StripRepository.Controllers.Clear();

        foreach (var fdr in FDP2.GetFDRs)
        {
            StripRepository.UpdateFDR(fdr, this, socketConn, true);
        }

        var instance = MainForm.MainFormInstance;
        if (instance is not null)
        {
            LockWindowUpdate(instance.Handle);

            foreach (var bay in BayRepository.Bays)
            {
                bay.Orderstrips();
            }

            LockWindowUpdate(IntPtr.Zero);
        }
    }

    /// <summary>
    /// Sets a controller to be picked.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <param name="sendToVatsys">Selects relevant track in vatSys.</param>
    public void SetPicked(Strip controller, bool sendToVatsys = false)
    {
        PickedController?.SetHMIPicked(false);
        PickedController = controller;
        controller.SetHMIPicked(true);

        if (sendToVatsys)
        {
            var rTrack = RDP.RadarTracks.FirstOrDefault(x => x.ActualAircraft.Callsign == controller.FDR.Callsign);
            var track = MMI.FindTrack(rTrack);
            if (track is not null)
            {
                if (MMI.SelectedTrack != track)
                {
                    MMI.SelectOrDeselectTrack(track);
                }

                if (MMI.SelectedGroundTrack != track)
                {
                    MMI.SelectOrDeselectGroundTrack(track);
                }
            }
        }
    }

    /// <summary>
    /// Sets a controller to be picked, from an FDR.
    /// </summary>
    /// <param name="fdr">The fdr.</param>
    public void SetPicked(FDR? fdr)
    {
        if (fdr is not null)
        {
            Strip? foundSC = null;
            foreach (var controller in StripRepository.Controllers)
            {
                if (controller.FDR.Callsign == fdr.Callsign)
                {
                    foundSC = controller;
                }
            }

            if (foundSC is not null)
            {
                SetPicked(foundSC);
            }
        }
        else
        {
            SetPicked();
        }
    }

    /// <summary>
    /// Sets the picked controller to be empty.
    /// </summary>
    /// <param name="sendToVatsys">Deselect ground track in vatSys.</param>
    public void SetPicked(bool sendToVatsys = false)
    {
        PickedController?.SetHMIPicked(false);
        PickedController = null;

        if (sendToVatsys)
        {
            MMI.SelectOrDeselectGroundTrack(MMI.SelectedGroundTrack);
            MMI.SelectOrDeselectTrack(MMI.SelectedTrack);
        }
    }

    /// <summary>
    /// Wipe the strips.
    /// </summary>
    public void WipeStrips()
    {
        PickedController = null;
        foreach (var bay in BayRepository.Bays)
        {
            bay.WipeStrips();
        }
    }

    /// <summary>
    /// Adds a strip.
    /// </summary>
    /// <param name="stripController">The strip controller to add.</param>
    /// <param name="save">If the strip controller should be saved.</param>
    /// <param name="inhibitreorders">Whether or not to inhibit strip reodering.</param>
    public void AddStrip(Strip stripController, bool save = true, bool inhibitreorders = false)
    {
        if (!stripController.DetermineSCValidity())
        {
            return;
        }

        foreach (var bay in BayRepository.Bays)
        {
            if (bay.ResponsibleFor(stripController.CurrentBay))
            {
                bay.AddStrip(stripController, inhibitreorders);
            }
        }

        if (save)
        {
            StripRepository.Controllers.Add(stripController);
        }
    }

    /// <summary>
    /// Updates the bay from the controller.
    /// </summary>
    /// <param name="stripController">The strip controller.</param>
    public void UpdateBay(Strip stripController)
    {
        foreach (var bay in BayRepository.Bays)
        {
            if (bay.OwnsStrip(stripController))
            {
                bay.RemoveStrip(stripController);
            }
        }

        AddStrip(stripController);

        if (stripController.CurrentBay >= StripBay.BAY_PUSHED)
        {
            stripController.CoordinateStrip();
        }
    }

    /// <summary>
    /// Forces a rerender.
    /// </summary>
    public void ForceRerender()
    {
        try
        {
            foreach (var bay in BayRepository.Bays)
            {
                bay.ForceRerender();
            }
        }
        catch (Exception ex)
        {
            Errors.Add(ex, "OzStrips");
        }
    }

    /// <summary>
    /// Positions the key.
    /// </summary>
    /// <param name="relPos">The relative position.</param>
    public void PositionKey(int relPos)
    {
        try
        {
            if (PickedController != null)
            {
                BayRepository.FindBay(PickedController)?.ChangeStripPosition(PickedController, relPos);
            }
        }
        catch (Exception ex)
        {
            Errors.Add(ex, "OzStrips");
        }
    }

    /// <summary>
    /// Queues up the selected strip.
    /// </summary>
    public void QueueUp()
    {
        try
        {
            if (PickedController != null)
            {
                BayRepository.FindBay(PickedController)?.QueueUp();
            }
        }
        catch (Exception ex)
        {
            Errors.Add(ex, "OzStrips");
        }
    }

    [DllImport("user32.dll")]
    private static extern long LockWindowUpdate(IntPtr handle);
}
