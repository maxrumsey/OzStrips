using MaxRumsey.OzStripsPlugin.Gui.DTO;
using MaxRumsey.OzStripsPlugin.Gui.Properties;
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
    private List<string> _defaultAerodromes = new();

    private List<string> _manuallySetAerodromes = new();

    private string _previousAerodromeType = string.Empty;

    public bool PreviouslyClosed;

    public string? AutoOpenAerodrome;

    public List<string> ConcernedAerodromes = new();

    public static bool InhibitVersionCheck;

    public bool AllowAutoOpen
    {
        get
        {
            var mode = (AutoOpenModes)OzStripsSettings.Default.AutoOpenBehaviour;

            return mode switch
            {
                AutoOpenModes.Always => true,
                AutoOpenModes.OncePerSession => !PreviouslyClosed,
                AutoOpenModes.Never => false,
                _ => false,
            };
        }
    }

    public List<string> ManuallySetAerodromes
    {
        get
        {
            return _manuallySetAerodromes;
        }

        set
        {
            _manuallySetAerodromes = value;
            AerodromeListChanged(this, EventArgs.Empty);
        }
    }

    public AerodromeSettings? Settings;

    public event EventHandler AerodromeListChanged;

    public event EventHandler OpenGUI;

    public event EventHandler ViewListChanged;

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

            if (completeList.Count == 0)
            {
                completeList.AddRange(_defaultAerodromes);
            }

            completeList.Sort();
            completeList = completeList.Distinct().ToList();

            return completeList;
        }
    }

    public string GetAerodromeType(string aerodrome)
    {
        return Settings?.AerodromeLists.FirstOrDefault(x => x.Aerodromes.Contains(aerodrome))?.Type ?? string.Empty;
    }

    public AerodromeManager()
    {
        MMI.PrimePositonChanged += PrimePositionChanged;
        MMI.SectorsControlledChanged += SectorsChanged;

        LoadSettings();
        InhibitVersionCheck = Settings?.InhibitVersionCheck ?? false;
    }

    public void Initialize()
    {
        PrimePositionChanged(this, EventArgs.Empty);
        SectorsChanged(this, EventArgs.Empty);
    }

    public void InitialiseOnNewWindow()
    {
        _previousAerodromeType = string.Empty;
    }

    public void ConfigureAerodromeListForNewAerodrome(string aerodrome)
    {
        var type = GetAerodromeType(aerodrome);

        if (type != _previousAerodromeType)
        {
            _previousAerodromeType = type;
            ViewListChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<LayoutDefinition> ReturnLayouts(string filter)
    {
        if (Settings is null || Settings.Layouts is null)
        {
            throw new Exception("Unable to access layouts.");
        }

        var layouts = Settings.Layouts.Where(x => x.Type == filter).ToList();
        var bays = Settings.Bays.FirstOrDefault(x => x.Type == filter);

        if (layouts.Count == 0 || bays is null)
        {
            throw new Exception($"No layouts or bays of type {filter} found.");
        }

        foreach (var layout in layouts)
        {
            foreach (var element in layout.Elements)
            {
                element.Bay = bays.Bays.FirstOrDefault(x => x.Name == element.Name);
            }
        }

        return layouts;
    }

    public void LoadSettings()
    {
        Settings = AerodromeSettings.Load();

        _defaultAerodromes = Settings?.DefaultAerodromes?.ToList() ?? new List<string>();
    }

    private void SectorsChanged(object sender, EventArgs e)
    {
        MMI.InvokeOnGUI(() =>
        {
            var sectorList = new List<Sector>();
            ConcernedAerodromes.Clear();

            if (MMI.SectorsControlled == null)
            {
                return;
            }

            // Generate list of all sectors.
            foreach (var topLevelSector in MMI.SectorsControlled.ToList())
            {
                RecurseSectors(sectorList, topLevelSector);
            }

            foreach (var sector in sectorList)
            {
                // Match to concernedsectors
                var concernedSectors = Settings?.ConcernedSectors?
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

            AerodromeListChanged?.Invoke(this, new());
        });
    }

    private void PrimePositionChanged(object sender, EventArgs e)
    {
        MMI.InvokeOnGUI(() =>
        {
            var posName = MMI.PrimePosition?.Name;

            var res = Settings?.AutoOpens?.FirstOrDefault(x => x.Position == posName);

            AutoOpenAerodrome = res?.Aerodrome;

            AerodromeListChanged?.Invoke(this, new());

            if (AutoOpenAerodrome != null && AllowAutoOpen)
            {
                OpenGUI?.Invoke(this, EventArgs.Empty);
            }
        });
    }

    private static void RecurseSectors(List<Sector> sectorList, Sector currentSector)
    {
        sectorList.Add(currentSector);

        foreach (var child in currentSector.SubSectors)
        {
            // TODO: look for all dupes
            if (child.Name == currentSector.Name)
            {
                // AIS error
                continue;
            }

            if (sectorList.Any(x => x.Name == child.Name))
            {
                // Weird multi level circular reference.
                continue;
            }

            RecurseSectors(sectorList, child);
        }
    }

    public enum AutoOpenModes
    {
        OncePerSession,
        Always,
        Never
    }
}
