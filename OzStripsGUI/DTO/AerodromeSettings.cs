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

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

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
    public ConcernedSector[]? ConcernedSectors;

    [XmlArray("AutoOpens")]
    public AutoOpen[]? AutoOpens;

    [XmlArray("DefaultAerodromes")]
    [XmlArrayItem("Aerodrome")]
    public string[]? DefaultAerodromes;

    public bool InhibitVersionCheck;

    [XmlArray("BayTypes")]
    public BayType[]? Bays;

    [XmlArray("LayoutDefinitions")]
    [XmlArrayItem("LayoutDefinition")]
    public LayoutDefinition[]? Layouts;

    [XmlArray("AerodromeLists")]
    [XmlArrayItem("AerodromeList")]
    public AerodromeList[]? AerodromeLists;

    internal static AerodromeSettings? Deserialize(string path)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(AerodromeSettings));
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
        // This is hacky but it's the easiest way to get the prof dir.
        // Unless I'm missing something obvious.
        if (vatsys.Profile.Loaded)
        {
            var shortNameObject = typeof(vatsys.Profile).GetField("shortName", BindingFlags.Static | BindingFlags.NonPublic);
            var shortName = (string)shortNameObject.GetValue((object) shortNameObject);

            return Path.Combine(Helpers.GetFilesFolder(), "Profiles", shortName, "Plugins\\AerodromeSettings.xml");
        }

        return string.Empty;
    }

    public static AerodromeSettings? Load()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\AerodromeSettings.xml");
        var baseSettings = AerodromeSettings.Deserialize(path);

        var overwrite = AerodromeSettings.Deserialize(GetADSettingsPath());

        if (baseSettings == null || overwrite == null)
        {
            return overwrite;
        }

        // Overwrite the base settings with the profile settings.
        baseSettings.ConcernedSectors = overwrite.ConcernedSectors ?? baseSettings.ConcernedSectors;
        baseSettings.AutoOpens = overwrite.AutoOpens ?? baseSettings.AutoOpens;
        baseSettings.DefaultAerodromes = overwrite.DefaultAerodromes ?? baseSettings.DefaultAerodromes;
        baseSettings.InhibitVersionCheck = overwrite.InhibitVersionCheck || baseSettings.InhibitVersionCheck;
        baseSettings.Layouts = overwrite.Layouts ?? baseSettings.Layouts;
        baseSettings.Bays = overwrite.Bays ?? baseSettings.Bays;
        baseSettings.AerodromeLists = overwrite.AerodromeLists ?? baseSettings.AerodromeLists;

        return baseSettings;
    }
}
