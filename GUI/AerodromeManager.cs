using System;
using System.Collections.Generic;
using System.Linq;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.DTO.XML;
using MaxRumsey.OzStripsPlugin.GUI.Properties;
using vatsys;
using static vatsys.SectorsVolumes;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// Manages aerodromes immaediately available in the selection list.
/// </summary>
public class AerodromeManager
{
    private List<string> _defaultAerodromes = [];

    private List<string> _manuallySetAerodromes = [];

    private string _previousAerodromeType = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether OzStrips has been previously closed this session.
    /// </summary>
    public bool PreviouslyClosed { get; set; }

    /// <summary>
    /// Gets or sets applicable auto-open aerodrome for this position.
    /// </summary>
    public string? AutoOpenAerodrome { get; set; }

    /// <summary>
    /// List of concerned aerodromes.
    /// </summary>
    private List<string> _concernedAerodromes = new();

    /// <summary>
    /// Gets or sets a value indicating whether or not the version check is inhibited.
    /// </summary>
    public static bool InhibitVersionCheck { get; set; }

    /// <summary>
    /// Gets or sets the base autofill file path.
    /// </summary>
    public static string AerodromeAutoFillLocation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default PDC format.
    /// </summary>
    public static string PDCFormat { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets default strip colours.
    /// </summary>
    public static StripColour[] StripColours { get; set; } = [];

    /// <summary>
    /// Gets a value indicating whether the window should autoopen.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a list of manually set aerodromes.
    /// </summary>
    public List<string> ManuallySetAerodromes
    {
        get
        {
            return _manuallySetAerodromes;
        }

        set
        {
            _manuallySetAerodromes = value;
            AerodromeListChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets or sets the aerodrome settings.
    /// </summary>
    public AerodromeSettings? Settings { get; set; }

    /// <summary>
    /// Called when the list of quick-switch aerodromes changes.
    /// </summary>
    public event EventHandler? AerodromeListChanged;

    /// <summary>
    /// Called when the GUI is opened.
    /// </summary>
    public event EventHandler? OpenGUI;

    /// <summary>
    /// Called when the list of possible views changes.
    /// </summary>
    public event EventHandler? ViewListChanged;

    /// <summary>
    /// Gets the complete list of quick-switch aerodromes.
    /// </summary>
    public List<string> AerodromeList
    {
        get
        {
            var completeList = _concernedAerodromes.ToList();
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
            completeList = [.. completeList.Distinct()];

            return completeList;
        }
    }

    /// <summary>
    /// Gets the aerodrome type.
    /// </summary>
    /// <param name="aerodrome">ICAO code.</param>
    /// <returns>Aerodrome type.</returns>
    public string GetAerodromeType(string aerodrome)
    {
        return Settings?.AerodromeLists.FirstOrDefault(x => x.Aerodromes.Contains(aerodrome))?.Type ?? string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AerodromeManager"/> class.
    /// </summary>
    public AerodromeManager()
    {
        MMI.PrimePositonChanged += PrimePositionChanged;
        MMI.SectorsControlledChanged += SectorsChanged;

        LoadSettings();
        InhibitVersionCheck = Settings?.InhibitVersionCheck ?? false;
    }

    /// <summary>
    /// Initialises the aerodrome manager.
    /// </summary>
    public void Initialize()
    {
        PrimePositionChanged(this, EventArgs.Empty);
        SectorsChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when a new window is opened.
    /// </summary>
    public void InitialiseOnNewWindow()
    {
        _previousAerodromeType = string.Empty;
    }

    /// <summary>
    /// Configures the view list for a new aerodrome.
    /// </summary>
    /// <param name="aerodrome">New aerodrome.</param>
    public void ConfigureAerodromeListForNewAerodrome(string aerodrome)
    {
        var type = GetAerodromeType(aerodrome);

        if (type != _previousAerodromeType)
        {
            _previousAerodromeType = type;
            ViewListChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets the list of layouts.
    /// </summary>
    /// <param name="filter">Filter by aerodrome type.</param>
    /// <returns>List of layouts.</returns>
    /// <exception cref="Exception">Loading error.</exception>
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

    /// <summary>
    /// Loads settings.
    /// </summary>
    public void LoadSettings()
    {
        Settings = AerodromeSettings.Load();

        _defaultAerodromes = Settings?.DefaultAerodromes?.ToList() ?? new List<string>();
        AerodromeAutoFillLocation = Settings?.AerodromeAutoFillLocation ?? string.Empty;
        PDCFormat = Settings?.PDCFormat ?? string.Empty;
        StripColours = Settings?.StripColours ?? [];
    }

    private void SectorsChanged(object sender, EventArgs e)
    {
        MMI.InvokeOnGUI(() =>
        {
            try
            {
                var sectorList = new List<Sector>();
                _concernedAerodromes.Clear();

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
                        _concernedAerodromes.AddRange(concernedSector.Aerodromes);
                    }
                }

                _concernedAerodromes = [.. _concernedAerodromes.Distinct()];

                AerodromeListChanged?.Invoke(this, new());
            }
            catch (Exception ex)
            {
                Util.LogError(ex, "OzStrips");
            }
        });
    }

    private void PrimePositionChanged(object sender, EventArgs e)
    {
        MMI.InvokeOnGUI(() =>
        {
            try
            {
                var posName = MMI.PrimePosition?.Name;

                var res = Settings?.AutoOpens?.FirstOrDefault(x => x.Position == posName);

                AutoOpenAerodrome = res?.Aerodrome;

                AerodromeListChanged?.Invoke(this, new());

                if (AutoOpenAerodrome != null && AllowAutoOpen)
                {
                    OpenGUI?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Util.LogError(ex, "OzStrips");
            }
        });
    }

    private static void RecurseSectors(List<Sector> sectorList, Sector currentSector)
    {
        sectorList.Add(currentSector);

        /*
         * MMI.SectorsControlled automatically recurses subsectors.

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
        */
    }

    /// <summary>
    /// Possible auto-open modes.
    /// </summary>
    public enum AutoOpenModes
    {
        /// <summary>
        /// Only auto-open once.
        /// </summary>
        OncePerSession,

        /// <summary>
        /// Always auto-open.
        /// </summary>
        Always,

        /// <summary>
        /// Never auto-open.
        /// </summary>
        Never,
    }
}
