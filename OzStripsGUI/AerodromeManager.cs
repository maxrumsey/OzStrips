using MaxRumsey.OzStripsPlugin.Gui.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vatsys;
using static vatsys.SectorsVolumes;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Manages aerodromes immaediately available in the selection list.
/// </summary>
public class AerodromeManager
{
    private List<string> _manualAerodromes = new();

    public string? AutoOpenAerodrome;

    public List<string> ConcernedAerodromes = new();

    public List<string> ManuallySetAerodromes
    {
        get
        {
            return _manualAerodromes;
        }

        set
        {
            AerodromeListChanged(this, EventArgs.Empty);
            _manualAerodromes = value;
        }
    }

    public AerodromeSettings? Settings;

    public event EventHandler AerodromeListChanged;

    public List<string> AerodromeList
    {
        get
        {
            var completeList = ConcernedAerodromes.ToList();
            completeList.AddRange(ManuallySetAerodromes);

            if (AutoOpenAerodrome is not null)
            {
                completeList.Add(AutoOpenAerodrome);
            }

            return completeList;
        }
    }

    public AerodromeManager()
    {
        MMI.PrimePositonChanged += PrimePositionChanged;
        MMI.SectorsControlledChanged += SectorsChanged;

        Settings = AerodromeSettings.Load();
    }

    private void SectorsChanged(object sender, EventArgs e)
    {
        var sectorList = new List<Sector>();
        ConcernedAerodromes.Clear();

        // Generate list of all sectors.
        foreach (var topLevelSector in MMI.SectorsControlled.ToList())
        {
            RecurseSectors(sectorList, topLevelSector);
        }

        foreach (var sector in sectorList)
        {
            // Match to concernedsectors
            var concernedSectors = Settings?.ConcernedSectors
                .Where(x => x.Positions.Contains(sector.Name))
                .ToList();

            if (concernedSectors is null)
            {
                continue;
            }

            // Add concerned aerodromes.
            foreach (var concernedSector in concernedSectors)
            {
                ConcernedAerodromes.AddRange(concernedSector.Aerodromes);
            }
        }

        ConcernedAerodromes = ConcernedAerodromes.Distinct().ToList();

        AerodromeListChanged(this, new());
    }

    private void PrimePositionChanged(object sender, EventArgs e)
    {
        var posName = MMI.PrimePosition.Name;

        var res = Settings?.AutoOpens.FirstOrDefault(x => x.Position == posName);

        AutoOpenAerodrome = res?.Aerodrome;

        AerodromeListChanged(this, new());
    }

    private static void RecurseSectors(List<Sector> sectorList, Sector currentSector)
    {
        sectorList.Add(currentSector);

        foreach (var child in currentSector.SubSectors)
        {
            RecurseSectors(sectorList, child);
        }
    }
}
