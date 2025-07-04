using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Accounts for all strip models.
/// </summary>
public class StripRepository
{
    /// <summary>
    /// Gets a list of strip controllers.
    /// </summary>
    public List<Strip> Controllers { get; } = [];

    /// <summary>
    /// Looks up controller by name.
    /// </summary>
    /// <param name="name">The aircraft callsign.</param>
    /// <returns>The aircraft's FDR.</returns>
    public Strip? GetController(string name)
    {
        foreach (var controller in Controllers)
        {
            if (controller.FDR.Callsign == name)
            {
                return controller;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a strip from a key.
    /// </summary>
    /// <param name="key">Strip key.</param>
    /// <returns>Strip or null.</returns>
    public Strip? GetController(StripKey key)
    {
        foreach (var controller in Controllers)
        {
            if (controller.StripKey.Matches(key))
            {
                return controller;
            }
        }

        return null;
    }

    /// <summary>
    /// Receives a fdr, updates existing strip controller, or makes a new one.
    /// </summary>
    /// <param name="fdr">The flight data record.</param>
    /// <param name="bayManager">The bay manager.</param>
    /// <param name="socketConn">The socket connection.</param>
    /// <param name="inhibitReorders">Whether or not to inhibit strip reordering.</param>
    /// <returns>The appropriate strip controller for the FDR.</returns>
    public Strip UpdateFDR(FDR fdr, BayManager bayManager, SocketConn socketConn, bool inhibitReorders = false)
    {
        foreach (var controller in Controllers)
        {
            if (controller.FDR.Callsign == fdr.Callsign)
            {
                if (GetFDRIndex(fdr.Callsign) == -1)
                {
                    bayManager.BayRepository.DeleteStrip(controller);
                }

                controller.CheckAndInvalidateSavedRoutes(fdr);

                controller.FDR = fdr;
                controller.UpdateFDR();
                return controller;
            }
        }

        // todo: add this logic into separate static function
        var stripController = new Strip(fdr, bayManager, socketConn);
        bayManager.AddStrip(stripController, true, inhibitReorders);
        stripController.FetchStripData();
        return stripController;
    }

    /// <summary>
    /// Receives a SC DTO object, updates relevant SC.
    /// </summary>
    /// <param name="stripControllerData">The strip controller data.</param>
    /// <param name="bayManager">The bay manager.</param>
    public void UpdateFDR(StripControllerDTO stripControllerData, BayManager bayManager)
    {
        try
        {
            foreach (var controller in Controllers)
            {
                if (controller.StripKey.Matches(stripControllerData.StripKey))
                {
                    var changeBay = false;
                    controller.CLX = !string.IsNullOrWhiteSpace(stripControllerData.CLX) ? stripControllerData.CLX : string.Empty;
                    controller.Gate = stripControllerData.GATE ?? string.Empty;
                    if (controller.CurrentBay != stripControllerData.bay)
                    {
                        changeBay = true;
                    }

                    controller.CurrentBay = stripControllerData.bay;
                    controller.Controller?.Cock(stripControllerData.cockLevel, false);
                    controller.TakeOffTime = stripControllerData.TOT == "\0" ?
                        null :
                        DateTime.Parse(stripControllerData.TOT, CultureInfo.InvariantCulture);

                    controller.Remark = !string.IsNullOrWhiteSpace(stripControllerData.remark) ? stripControllerData.remark : string.Empty;
                    controller.Crossing = stripControllerData.crossing;
                    controller.Controller?.SetCross(false);
                    controller.Ready = stripControllerData.ready;
                    if (changeBay)
                    {
                        bayManager.UpdateBay(controller); // prevent unessesscary reshufles
                    }

                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    /// <summary>
    /// Loads in a cacheDTO object received from server, sets SCs accordingly.
    /// </summary>
    /// <param name="cacheData">The cache data.</param>
    /// <param name="bayManager">The bay manager.</param>
    /// <param name="socketConn">The socket connection.</param>
    public void LoadCache(List<StripControllerDTO> cacheData, BayManager bayManager, SocketConn socketConn)
    {
        foreach (var stripDTO in cacheData)
        {
            UpdateFDR(stripDTO, bayManager);
        }

        socketConn.ReadyForBayData();
    }

    /// <summary>
    /// Marks all strips as awaiting routes to be fetched from the server. Called on connection establishment.
    /// </summary>
    public void MarkAllStripsAsAwaitingRoutes()
    {
        foreach (var strip in Controllers)
        {
            strip.RequestedRoutes = DateTime.MaxValue;
        }
    }

    /// <summary>
    /// Fetches strip status and returns it to server.
    /// </summary>
    /// <param name="acid">Strip callsign to check up on.</param>
    /// <param name="socketConn">Socket connection.</param>
    public void GetStripStatus(string acid, SocketConn socketConn)
    {
        foreach (var strip in Controllers)
        {
            if (strip.FDR.Callsign == acid)
            {
                socketConn.SendStripStatus(strip, acid);
                return;
            }
        }

        socketConn.SendStripStatus(null, acid);
    }
}
