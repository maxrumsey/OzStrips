using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// Accounts for all strip models.
/// </summary>
public class StripRepository
{
    /// <summary>
    /// Gets a list of strip controllers.
    /// </summary>
    public List<Strip> Strips { get; } = [];

    /// <summary>
    /// Looks up strip by callsign.
    /// </summary>
    /// <param name="callsign">The aircraft callsign.</param>
    /// <returns>The aircraft's FDR.</returns>
    public Strip? GetStrip(string callsign)
    {
        foreach (var strip in Strips)
        {
            if (strip.FDR.Callsign == callsign)
            {
                return strip;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a strip from a key.
    /// </summary>
    /// <param name="key">Strip key.</param>
    /// <returns>Strip or null.</returns>
    public Strip? GetStrip(StripKey key)
    {
        foreach (var strip in Strips)
        {
            if (strip.StripKey.Matches(key))
            {
                return strip;
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
        foreach (var controller in Strips)
        {
            if (controller.FDR.Callsign == fdr.Callsign)
            {
                if (GetFDRIndex(fdr.Callsign) == -1)
                {
                    bayManager.BayRepository.DeleteStrip(controller);
                }

                // If ades / adep changed, delete and recreate strip.
                if (controller.ADESADEPPairChanged(fdr))
                {
                    bayManager.BayRepository.DeleteStrip(controller);
                    return CreateStrip(fdr, bayManager, socketConn, inhibitReorders);
                }
                else
                {
                    controller.FDR = fdr;
                    controller.UpdateFDR();
                    return controller;
                }
            }
        }

        return CreateStrip(fdr, bayManager, socketConn, inhibitReorders);
    }

    /// <summary>
    /// Receives a Strip DTO object, updates relevant Strip.
    /// </summary>
    /// <param name="stripDTO">The strip controller data.</param>
    /// <param name="bayManager">The bay manager.</param>
    public void UpdateStripData(StripDTO stripDTO, BayManager bayManager)
    {
        // todo: move this to strip class?
        try
        {
            foreach (var strip in Strips)
            {
                if (strip.StripKey.Matches(stripDTO.StripKey))
                {
                    var changeBay = false;
                    strip.CLX = !string.IsNullOrWhiteSpace(stripDTO.CLX) ? stripDTO.CLX : string.Empty;
                    strip.Gate = stripDTO.GATE ?? string.Empty;

                    if (strip.CurrentBay != stripDTO.bay)
                    {
                        changeBay = true;
                    }

                    strip.CurrentBay = stripDTO.bay;
                    strip.Controller?.Cock(stripDTO.cockLevel, false);
                    strip.TakeOffTime = stripDTO.TOT == "\0" ?
                        null :
                        DateTime.Parse(stripDTO.TOT, CultureInfo.InvariantCulture);

                    strip.Remark = !string.IsNullOrWhiteSpace(stripDTO.remark) ? stripDTO.remark : string.Empty;
                    strip.Crossing = stripDTO.crossing;
                    strip.Controller?.SetCross(false);
                    strip.Ready = stripDTO.ready;
                    strip.OverrideStripType = stripDTO.OverrideStripType;
                    strip.PDCFlags = stripDTO.PDCFlags;

                    if (changeBay)
                    {
                        bayManager.UpdateBay(strip, true); /* prevent unessesscary reshufles
                        * now that we render using skiasharp, is this necessary?
                        * yes, will prevent needless skiacabvas height changes.
                        */
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
    public void LoadCache(List<StripDTO> cacheData, BayManager bayManager, SocketConn socketConn)
    {
        foreach (var stripDTO in cacheData)
        {
            UpdateStripData(stripDTO, bayManager);
        }

        socketConn.ReadyForBayData();
    }

    /// <summary>
    /// Marks all strips as awaiting routes to be fetched from the server. Called on connection establishment.
    /// </summary>
    public void MarkAllStripsAsAwaitingRoutes()
    {
        foreach (var strip in Strips)
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
        foreach (var strip in Strips)
        {
            if (strip.FDR.Callsign == acid)
            {
                socketConn.SendStripStatus(strip, acid);
                return;
            }
        }

        socketConn.SendStripStatus(null, acid);
    }

    private static Strip CreateStrip(FDR fdr, BayManager bayManager, SocketConn socketConn, bool inhibitReorders = false)
    {
        var stripController = new Strip(fdr, bayManager, socketConn);
        bayManager.AddStrip(stripController, true, inhibitReorders);
        stripController.FetchStripData();
        return stripController;
    }
}
