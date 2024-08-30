using System.Collections.Generic;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// IBayManager.
/// </summary>
public interface IBayManager
{
    /// <summary>
    /// Gets or sets the picked controller.
    /// </summary>
    public IStripController? PickedController { get; set; }

    /// <summary>
    /// Gets or sets the number of present bays.
    /// </summary>
    public int BayNum { get; set; }

    /// <summary>
    /// Gets or sets the aerodrome name.
    /// </summary>
    public string AerodromeName { get; set; }

    /// <summary>
    /// Gets the list of bays.
    /// </summary>
    public List<IBay> Bays { get; }

    /// <summary>
    /// Sets the last selected track's FDR in vatSys.
    /// </summary>
    public string? PickedCallsign
    {
        set;
    }

    /// <summary>
    /// Updates the bay based on the bay data.
    /// </summary>
    /// <param name="bayDTO">The bay data.</param>
    public void UpdateOrder(BayDTO bayDTO);

    /// <summary>
    /// Forces a track into the first bay.
    /// </summary>
    /// <param name="socketConn">The socket connection.</param>
    public void ForceStrip(SocketConn socketConn);

    /// <summary>
    /// SidTriggers the selected strip.
    /// </summary>
    public void SidTrigger();

    /// <summary>
    /// Cocks the selected strip.
    /// </summary>
    public void CockStrip();

    /// <summary>
    /// Inhibit the strip.
    /// </summary>
    public void Inhibit();

    /// <summary>
    /// Send the PDC.
    /// </summary>
    public void SendPDC();

    /// <summary>
    /// Toggles crossing highlight on a strip.
    /// </summary>
    public void CrossStrip();

    /// <summary>
    /// Drop the strip to the specified bay.
    /// </summary>
    /// <param name="bay">The bay.</param>
    public void DropStrip(Bay bay);

    /// <summary>
    /// Deletes the specified strip.
    /// </summary>
    /// <param name="strip">The strip to delete.</param>
    public void DeleteStrip(IStripController strip);

    /// <summary>
    /// Sets the aerodrome. Reinitialises various classes.
    /// </summary>
    /// <param name="name">The name of the aerodrome.</param>
    /// <param name="socketConn">The socket connection.</param>
    public void SetAerodrome(string name, SocketConn socketConn);

    /// <summary>
    /// Sets a controller to be picked.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <param name="sendToVatsys">Selects relevant track in vatSys.</param>
    public void SetPicked(IStripController controller, bool sendToVatsys = false);

    /// <summary>
    /// Sets a controller to be picked, from an FDR.
    /// </summary>
    /// <param name="fdr">The fdr.</param>
    public void SetPicked(FDR? fdr);

    /// <summary>
    /// Sets the picked controller to be empty.
    /// </summary>
    /// <param name="sendToVatsys">Deselect ground track in vatSys.</param>
    public void SetPicked(bool sendToVatsys = false);

    /// <summary>
    /// Wipe the strips.
    /// </summary>
    public void WipeStrips();

    /// <summary>
    /// Adds a strip.
    /// </summary>
    /// <param name="stripController">The strip controller to add.</param>
    /// <param name="save">If the strip controller should be saved.</param>
    /// <param name="inhibitreorders">Whether or not to inhibit strip reodering.</param>
    public void AddStrip(IStripController stripController, bool save = true, bool inhibitreorders = false);

    /// <summary>
    /// Finds the specified bay.
    /// </summary>
    /// <param name="stripController">The strip.</param>
    /// <returns>The bay if the name matches.</returns>
    public IBay? FindBay(IStripController stripController);

    /// <summary>
    /// Updates the bay from the controller.
    /// </summary>
    /// <param name="stripController">The strip controller.</param>
    public void UpdateBay(IStripController stripController);

    /// <summary>
    /// Adds the bay to the vertical board.
    /// </summary>
    /// <param name="bay">The bay.</param>
    /// <param name="verticalBoardNumber">The vertical board number.</param>
    public void AddBay(IBay bay, int verticalBoardNumber);

    /// <summary>
    /// Wipes the bays.
    /// </summary>
    public void WipeBays();

    /// <summary>
    /// Reloads the strips. Called when stripboard layout is changed.
    /// </summary>
    public void ReloadStrips();

    /// <summary>
    /// Forces a rerender.
    /// </summary>
    public void ForceRerender();

    /// <summary>
    /// Resizes the control.
    /// </summary>
    public void Resize();

    /// <summary>
    /// Positions the key.
    /// </summary>
    /// <param name="relPos">The relative position.</param>
    public void PositionKey(int relPos);

    /// <summary>
    /// Queues up the selected strip.
    /// </summary>
    public void QueueUp();
}
