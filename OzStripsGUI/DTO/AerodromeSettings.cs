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
    public ConcernedSector[] ConcernedSectors;

    [XmlArray("AutoOpens")]
    public AutoOpen[] AutoOpens;

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
}
