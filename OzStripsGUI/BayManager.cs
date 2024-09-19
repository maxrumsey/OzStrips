using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using OpenTK.Graphics.ES11;
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
    /// Gets the picked controller.
    /// </summary>
    public Strip? PickedController
    {
        get
        {
            if (PickedStripItem is not null && PickedStripItem.Type == StripItemType.STRIP)
            {
                return PickedStripItem.StripController;
            }

            return null;
        }
    }

    /// <summary>
    /// Gets or sets the picked list item.
    /// </summary>
    public StripListItem? PickedStripItem { get; set; }

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
    /// Gets the bay the current picked striplistitem is from.
    /// </summary>
    public Bay? PickedBay
    {
        get;
        internal set;
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
        else if (PickedBay is not null && PickedStripItem is not null && PickedStripItem.Type != StripItemType.STRIP)
        {
            PickedBay.DeleteBar(PickedStripItem);
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
            PickedController.Controller?.SetCross();
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
    /// Sets a strip item to be picked.
    /// </summary>
    /// <param name="item">The strip item.</param>
    /// <param name="sendToVatsys">Selects relevant track in vatSys.</param>
    /// <param name="bay">The bay the item is from.</param>
    public void SetPicked(StripListItem item, bool sendToVatsys = false, Bay? bay = null)
    {
        SetPicked(false);
        PickedStripItem = item;
        PickedBay = bay;
        item.RenderedStripItem?.MarkPicked(true);

        if (sendToVatsys && item.Type == StripItemType.STRIP)
        {
            var rTrack = RDP.RadarTracks.FirstOrDefault(x => x.ActualAircraft.Callsign == item.StripController?.FDR.Callsign);
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
    /// Toggles a strip item as picked.
    /// </summary>
    /// <param name="item">The strip item.</param>
    /// <param name="sendToVatsys">Selects relevant track in vatSys.</param>
    /// <param name="bay">The specified bay.</param>
    public void TogglePicked(StripListItem item, bool sendToVatsys = false, Bay? bay = null)
    {
        if (PickedStripItem == item)
        {
            SetPicked(sendToVatsys);
        }
        else
        {
            SetPicked(item, sendToVatsys, bay);
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
                SetPickedStripItem(foundSC);
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
        PickedStripItem?.RenderedStripItem?.MarkPicked(false);
        PickedStripItem = null;

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
        PickedStripItem = null;
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
    /// Move strips to the next bar.
    /// </summary>
    /// <param name="direction">Position (up/down).</param>
    public void PositionToNextBar(int direction)
    {
        if (PickedStripItem is null)
        {
            return;
        }

        var bay = BayRepository.FindBay(PickedStripItem);
        if (bay is null)
        {
            return;
        }

        var index = bay.Strips.IndexOf(PickedStripItem);
        if (bay.Strips.ElementAtOrDefault(index + direction)?.Type != StripItemType.STRIP)
        {
            bay.ChangeStripPosition(PickedStripItem, direction);
            return;
        }

        if (direction == 1)
        {
            for (var i = index + 2; i < bay.Strips.Count; i++)
            {
                var presentElement = bay.Strips.ElementAtOrDefault(i);
                if (presentElement is not null && presentElement.Type != StripItemType.STRIP)
                {
                    bay.ChangeStripPositionAbs(PickedStripItem, i - 1);
                    break;
                }
            }
        }
        else
        {
            for (var i = index - 2; i >= 0; i--)
            {
                var presentElement = bay.Strips.ElementAtOrDefault(i);
                if (presentElement is not null && presentElement.Type != StripItemType.STRIP)
                {
                    bay.ChangeStripPositionAbs(PickedStripItem, i + 1);
                    break;
                }
            }
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
            if (PickedStripItem != null)
            {
                BayRepository.FindBay(PickedStripItem)?.ChangeStripPosition(PickedStripItem, relPos);
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

    /// <summary>
    /// Creates the specified bar.
    /// </summary>
    /// <param name="baystring">Bay name.</param>
    /// <param name="type">Type of bay.</param>
    /// <param name="text">Text on bar.</param>
    public void AddBar(string baystring, int type, string text)
    {
        try
        {
            var bay = BayRepository.Bays.Find(x => x.Name == baystring);

            bay.AddBar(type, text);
        }
        catch (Exception ex)
        {
            Errors.Add(ex, "OzStrips");
        }
    }

    /// <summary>
    /// Sets the picked strip item.
    /// </summary>
    /// <param name="strip">The strip item.</param>
    /// <param name="sendToVatsys">Whether or not the change is propogated to vatsys.</param>
    public void SetPickedStripItem(Strip strip, bool sendToVatsys = false)
    {
        foreach (var bay in BayRepository.Bays)
        {
            var item = bay.GetListItem(strip);
            if (item is not null)
            {
                SetPicked(item, sendToVatsys, bay);
            }
        }
    }

    [DllImport("user32.dll")]
    private static extern long LockWindowUpdate(IntPtr handle);
}
