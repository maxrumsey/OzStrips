using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using MaxRumsey.OzStripsPlugin.Gui.Properties;
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
    public BayManager(FlowLayoutPanel main)
    {
        BayRepository = new(main, this);
    }

    /// <summary>
    /// Gets the picked controller.
    /// </summary>
    public Strip? PickedStrip
    {
        get
        {
            if (PickedStripItem is not null && PickedStripItem.Type == StripItemType.STRIP)
            {
                return PickedStripItem.Strip;
            }

            return null;
        }
    }

    /// <summary>
    /// Gets or sets the picked list item.
    /// </summary>
    public StripListItem? PickedStripItem { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not WF mode is activated.
    /// </summary>
    public bool WorldFlightMode { get; set; }

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
    /// Gets the bay the current picked striplistitem is from.
    /// </summary>
    public Bay? PickedBay
    {
        get
        {
            if (PickedStripItem is not null)
            {
                return BayRepository.FindBay(PickedStripItem);
            }

            return null;
        }
    }

    /// <summary>
    /// Deselects all vatSys tracks.
    /// </summary>
    /// <param name="originatedFromGround">Whether this originated from deselection of a ground track.</param>
    public static void DeSelectAllVatSysTracks(bool originatedFromGround)
    {
        // todo: is passing originator type necessary?
        if (originatedFromGround && MMI.SelectedTrack != null)
        {
            MMI.SelectOrDeselectTrack(MMI.SelectedTrack);
        }
        else if (!originatedFromGround && MMI.SelectedGroundTrack != null)
        {
            MMI.SelectOrDeselectGroundTrack(MMI.SelectedGroundTrack);
        }
    }

    /// <summary>
    /// Sets relevant vatSys selected tracks, when we select an actual strip item.
    /// </summary>
    /// <param name="strip">Strip we are setting vatsys tracks to.</param>
    public static void SelectVatSysTracks(Strip? strip)
    {
        var rTrack = RDP.RadarTracks.FirstOrDefault(x => x.ActualAircraft.Callsign == strip?.FDR.Callsign);
        var groundTrack = MMI.FindTrack(rTrack);
        var fdrTrack = MMI.FindTrack(strip?.FDR);

        if (fdrTrack is not null && MMI.SelectedTrack != fdrTrack)
        {
            MMI.SelectOrDeselectTrack(fdrTrack);
        }

        if (groundTrack is not null && MMI.SelectedGroundTrack != groundTrack)
        {
            MMI.SelectOrDeselectGroundTrack(groundTrack);
        }
    }

    /// <summary>
    /// Sets the picked callsign, and if deselecting a track, deselects the corresponding air/ground track.
    /// </summary>
    /// <param name="callsign">Aircraft callsign.</param>
    /// <param name="originatedFromGround">Whether or not the track is a ground track.</param>
    public void SetPickedCallsignFromVatsys(string? callsign, bool originatedFromGround)
    {
        if (callsign is not null)
        {
            var sc = StripRepository.GetStrip(callsign);
            if (sc is not null)
            {
                SetPickedStripFromExternal(sc);
            }
        }
        else
        {
            if (PickedStrip == null)
            {
                return;
            }

            // This is a vatSys track being deselected.
            DeSelectAllVatSysTracks(originatedFromGround);
            RemovePicked(false, true);
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
        PickedStrip?.SIDTrigger();
    }

    /// <summary>
    /// Cocks the selected strip.
    /// </summary>
    public void CockStrip()
    {
        PickedStrip?.CockStrip();
    }

    /// <summary>
    /// Inhibit the strip.
    /// </summary>
    public void Inhibit()
    {
        if (PickedStrip != null)
        {
            PickedStrip.CurrentBay = StripBay.BAY_DEAD;
            PickedStrip.SyncStrip();
            UpdateBay(PickedStrip);
            RemovePicked(true, true);
        }
        else if (PickedBay is not null && PickedStripItem is not null && PickedStripItem.Type != StripItemType.STRIP)
        {
            PickedBay.DeleteBar(PickedStripItem);
        }
    }

    /// <summary>
    /// Send the PDC.
    /// </summary>
    /// <param name="strip">Strip to open PDC form for. Null if use picked controller.</param>
    public void SendPDC(Strip? strip = null)
    {
        if (PickedStrip != null && strip == null)
        {
            MMI.OpenCPDLCWindow(PickedStrip.FDR, null, CPDLC.MessageCategories.FirstOrDefault(m => m.Name == "PDC"));
            RemovePicked(true);
        }
        else if (strip != null)
        {
            MMI.OpenCPDLCWindow(strip.FDR, null, CPDLC.MessageCategories.FirstOrDefault(m => m.Name == "PDC"));
        }
    }

    /// <summary>
    /// Toggles crossing highlight on a strip.
    /// </summary>
    public void CrossStrip()
    {
        if (PickedStrip != null)
        {
            PickedStrip.Crossing = !PickedStrip.Crossing;
            PickedStrip.Controller?.SetCross();
            RemovePicked(true);
        }
    }

    /// <summary>
    /// Toggles crossing highlight on a strip.
    /// </summary>
    public void FlipFlopStrip()
    {
        if (PickedStrip != null)
        {
            PickedStrip.FlipFlop();
            RemovePicked(true);
        }
    }

    /// <summary>
    /// Drop the strip to the specified bay.
    /// </summary>
    /// <param name="bay">The bay.</param>
    public void DropStrip(Bay bay)
    {
        if (PickedStrip != null)
        {
            var newBay = bay.BayTypes.FirstOrDefault();
            if (newBay == PickedStrip.CurrentBay)
            {
                return;
            }

            PickedStrip.CurrentBay = newBay;
            PickedStrip.SyncStrip();
            UpdateBay(PickedStrip);

            PickedStripItem = bay.GetListItem(PickedStrip);

            RemovePicked(true);
        }
        else
        {
            MainFormController.Instance?.ForceStrip(null, null);
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
        StripRepository.Strips.Clear();

        foreach (var fdr in FDP2.GetFDRs)
        {
            StripRepository.UpdateFDR(fdr, this, socketConn, true);
        }

        if (MainFormController.Instance?.IsDisposed == false)
        {
            LockWindowUpdate(MainFormController.Instance.Handle);

            foreach (var bay in BayRepository.Bays)
            {
                bay.ResizeBay();
            }

            LockWindowUpdate(IntPtr.Zero);
        }

        BayRepository.ResizeStripBays();
    }

    /// <summary>
    /// Sets a strip item to be picked.
    /// </summary>
    /// <param name="item">The strip item.</param>
    /// <param name="bay">The bay the item is from, or null if this is an inhibited strip..</param>
    public void SetPickedStripItem(StripListItem item, Bay? bay = null)
    {
        RemovePicked(false, true);
        PickedStripItem = item;

        item.RenderedStripItem?.MarkPicked(true);

        if (item.Type == StripItemType.STRIP && item.Strip is not null)
        {
            bay?.ChildPanel.SetPicked(item.Strip);

            SelectVatSysTracks(item.Strip);
        }
    }

    /// <summary>
    /// Toggles a strip item as picked.
    /// </summary>
    /// <param name="item">The strip item.</param>
    /// <param name="bay">The specified bay.</param>
    public void TogglePickedStripItem(StripListItem item, Bay? bay = null)
    {
        if (PickedStripItem == item)
        {
            RemovePicked(true, true);
        }
        else
        {
            SetPickedStripItem(item, bay);
        }
    }

    /// <summary>
    /// Sets a controller to be picked, from an FDR.
    /// </summary>
    /// <param name="strip">The strip.</param>
    public void SetPickedStripFromExternal(Strip? strip)
    {
        if (strip is not null)
        {
            // Is this a duplicate or follow-up event from vatsys?
            // i.e. Did we just set the strip within vatsys.
            if (PickedStrip == strip)
            {
                return;
            }

            if (!SetPickedStripClass(strip))
                {
                    /*
                     * Strip is inhibited.
                     */

                    RemovePicked(false, true);
                    PickedStripItem = new()
                    {
                        Strip = strip,
                    };
                }
        }
        else
        {
            RemovePicked(false, true);
        }
    }

    /// <summary>
    /// Sets the picked controller to be empty.
    /// </summary>
    /// <param name="sendToVatsys">Whether or not to update vatSys tracks.</param>
    /// <param name="force">Override user preference on retaining picked strips.</param>
    public void RemovePicked(bool sendToVatsys = false, bool force = false)
    {
        if (force || !OzStripsSettings.Default.KeepStripPicked)
        {
            PickedBay?.ChildPanel.SetPicked(null);

            PickedStripItem?.RenderedStripItem?.MarkPicked(false);
            PickedStripItem = null;

            if (sendToVatsys)
            {
                MMI.SelectOrDeselectGroundTrack(MMI.SelectedGroundTrack);
                MMI.SelectOrDeselectTrack(MMI.SelectedTrack);
            }
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
    /// Adds a strip to the relevant bays. If required, saves the strip and resizes the bays.
    /// </summary>
    /// <param name="strip">The strip to add.</param>
    /// <param name="save">If the strip should be saved to the server.</param>
    /// <param name="inhibitreorders">Whether or not to inhibit strip reodering.</param>
    public void AddStrip(Strip strip, bool save = true, bool inhibitreorders = false)
    {
        if (!strip.DetermineSCValidity())
        {
            return;
        }

        foreach (var bay in BayRepository.Bays)
        {
            if (bay.ResponsibleFor(strip.CurrentBay))
            {
                bay.AddStrip(strip, inhibitreorders);

                // Lock bay scrollbars if this strip is picked.
                if (PickedStrip == strip)
                {
                    bay.ChildPanel.SetPicked(strip);
                }
            }
        }

        if (save && !StripRepository.Strips.Contains(strip))
        {
            StripRepository.Strips.Add(strip);
        }

        if (!inhibitreorders)
        {
            BayRepository.ResizeStripBays();
        }
    }

    /// <summary>
    /// Runs update function on relevant bays when a strip is moved.
    /// </summary>
    /// <param name="strip">The strip controller.</param>
    /// Called by inhibits, moving strips, sid triggers, server pos updates.
    public void UpdateBay(Strip strip)
    {
        foreach (var bay in BayRepository.Bays)
        {
            if (bay.OwnsStrip(strip))
            {
                bay.RemoveStrip(strip);

                // De-lock bay scrollbars.
                if (PickedStrip == strip)
                {
                    bay.ChildPanel.SetPicked(null);
                }
            }
        }

        AddStrip(strip);

        if (strip.CurrentBay >= StripBay.BAY_CLEARED)
        {
            strip.CoordinateStrip();
        }
        else if (strip.CurrentBay == StripBay.BAY_PREA)
        {
            strip.DeactivateStrip();
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
            Util.LogError(ex);
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
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Queues up the selected strip.
    /// </summary>
    public void QueueUp()
    {
        try
        {
            if (PickedStrip != null)
            {
                BayRepository.FindBay(PickedStrip)?.QueueUp();
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Creates the specified bar.
    /// </summary>
    /// <param name="bayString">Bay name.</param>
    /// <param name="type">Type of bay.</param>
    /// <param name="text">Text on bar.</param>
    public void AddBar(string bayString, int type, string text)
    {
        try
        {
            var bay = BayRepository.Bays.Find(x => x.Name == bayString);

            bay?.AddBar(type, text);
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Deletes a bar by params. Returns true if found.
    /// </summary>
    /// <param name="bayString">Bay name.</param>
    /// <param name="type">Type of bay.</param>
    /// <param name="text">Text on bar.</param>
    /// <returns>Whether or not the bar was found.</returns>
    public bool DeleteBarByParams(string bayString, int type, string text)
    {
        try
        {
            var bay = BayRepository.Bays.Find(x => x.Name == bayString);

            var bar = bay?.Strips.FirstOrDefault(x => x.BarText == text && x.Type == StripItemType.BAR && x.Style == type);

            if (bay is not null && bar is not null)
            {
                bay.DeleteBar(bar);
                return true;
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }

        return false;
    }

    /// <summary>
    /// Sets the picked strip item.
    /// </summary>
    /// <param name="strip">The strip item.</param>
    /// <returns>Whether or not the bay was found.</returns>
    public bool SetPickedStripClass(Strip strip)
    {
        foreach (var bay in BayRepository.Bays)
        {
            var item = bay.GetListItem(strip);
            if (item is not null)
            {
                SetPickedStripItem(item, bay);
                return true;
            }
        }

        return false;
    }

    [DllImport("user32.dll")]
    private static extern long LockWindowUpdate(IntPtr handle);
}
