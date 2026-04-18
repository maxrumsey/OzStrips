using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO.XML;

/// <summary>
/// Represents an object carrying the aerodrome settings.
/// </summary>
[Serializable]
[XmlRoot("AerodromeSettings")]
public class AerodromeSettings
{
    /// <summary>
    /// Gets or sets a list of concerned positions.
    /// </summary>
    [XmlArray("ConcernedSectors")]
    public ConcernedSector[]? ConcernedSectors { get; set; }

    /// <summary>
    /// Gets or sets a list of auto-open positions.
    /// </summary>
    [XmlArray("AutoOpens")]
    public AutoOpen[]? AutoOpens { get; set; }

    /// <summary>
    /// Gets or sets a list of default aerodromes.
    /// </summary>
    [XmlArray("DefaultAerodromes")]
    [XmlArrayItem("Aerodrome")]
    public string[]? DefaultAerodromes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the version check is inhibited.
    /// </summary>
    public bool InhibitVersionCheck { get; set; }

    /// <summary>
    /// Gets or sets a list of bay types.
    /// </summary>
    [XmlArray("BayTypes")]
    public BayType[]? Bays { get; set; }

    /// <summary>
    /// Gets or sets a list of layout definitions.
    /// </summary>
    [XmlArray("LayoutDefinitions")]
    [XmlArrayItem("LayoutDefinition")]
    public LayoutDefinition[]? Layouts { get; set; }

    /// <summary>
    /// Gets or sets a list of aerodromes.
    /// </summary>
    [XmlArray("AerodromeLists")]
    [XmlArrayItem("AerodromeList")]
    public AerodromeList[]? AerodromeLists { get; set; }

    /// <summary>
    /// Gets or sets a list of auto-map aerodromes.
    /// </summary>
    [XmlArray("AutoMapAerodromes")]
    [XmlArrayItem("AutoMapAerodrome")]
    public AutoMapAerodrome[]? AutoMapAerodromes { get; set; }

    /// <summary>
    /// Gets or sets the default PDC format.
    /// </summary>
    public string? PDCFormat { get; set; }

    /// <summary>
    /// Gets or sets the default autofill file path.
    /// </summary>
    public string? AerodromeAutoFillLocation { get; set; }

    /// <summary>
    /// Gets or sets a list of default strip colours.
    /// </summary>
    [XmlElement("StripColour")]
    public StripColour[]? StripColours { get; set; }

    /// <summary>
    /// Gets or sets the default layout.
    /// </summary>
    public string? DefaultLayout { get; set; }

    internal static AerodromeSettings? Deserialize(string path)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(AerodromeSettings));

            if (!File.Exists(path))
            {
                return null;
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            AerodromeSettings element;

            element = (AerodromeSettings)serializer.Deserialize(fs);

            return element;
        }
        catch (Exception ex)
        {
           Util.LogError(ex);
        }

        return null;
    }

    internal static string GetADSettingsPath()
    {
        return Path.Combine(GetPluginsDirectory(), "Configs", "AerodromeSettings.xml");
    }

    internal static string GetPluginsDirectory()
    {
        // This is hacky but it's the easiest way to get the prof dir.
        // Unless I'm missing something obvious.
        if (Profile.Loaded)
        {
            var shortNameObject = typeof(Profile).GetField("shortName", BindingFlags.Static | BindingFlags.NonPublic);
            var shortName = (string)shortNameObject.GetValue(shortNameObject);

            return Path.Combine(Helpers.GetFilesFolder(), "Profiles", shortName, "Plugins");
        }

        return string.Empty;
    }

    /// <summary>
    /// Loads the aerodrome settings.
    /// </summary>
    /// <returns>Aerodrome settings.</returns>
    /// <exception cref="Exception">Loading error.</exception>
    public static AerodromeSettings? Load()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\AerodromeSettings.xml");
        var baseSettings = Deserialize(path);

        var overwrite = Deserialize(GetADSettingsPath());

        if (baseSettings == null)
        {
            if (overwrite == null)
            {
                throw new Exception("AerodromeSettings.xml was not found or corrupted.");
            }

            return overwrite;
        }
        else if (overwrite == null)
        {
            return baseSettings;
        }

        // Overwrite the base settings with the profile settings.
        baseSettings.ConcernedSectors = overwrite.ConcernedSectors ?? baseSettings.ConcernedSectors;
        baseSettings.AutoOpens = overwrite.AutoOpens ?? baseSettings.AutoOpens;
        baseSettings.DefaultAerodromes = overwrite.DefaultAerodromes ?? baseSettings.DefaultAerodromes;
        baseSettings.InhibitVersionCheck = overwrite.InhibitVersionCheck || baseSettings.InhibitVersionCheck;
        baseSettings.Layouts = overwrite.Layouts ?? baseSettings.Layouts;
        baseSettings.Bays = overwrite.Bays ?? baseSettings.Bays;
        baseSettings.AerodromeLists = overwrite.AerodromeLists ?? baseSettings.AerodromeLists;
        baseSettings.AerodromeAutoFillLocation = overwrite.AerodromeAutoFillLocation ?? baseSettings.AerodromeAutoFillLocation;
        baseSettings.PDCFormat = overwrite.PDCFormat ?? baseSettings.PDCFormat;
        baseSettings.AutoMapAerodromes = overwrite.AutoMapAerodromes ?? baseSettings.AutoMapAerodromes;
        baseSettings.StripColours = overwrite.StripColours ?? baseSettings.StripColours;
        baseSettings.DefaultLayout = overwrite.DefaultLayout ?? baseSettings.DefaultLayout;

        return baseSettings;
    }
}
