using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

/// <summary>
/// key for a strip, used to identify it uniquely.
/// </summary>
public class StripKey
{
    /// <summary>
    /// Gets or sets the callsign of the aircraft.
    /// </summary>
    public string Callsign { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the VATSIM ID of the aircraft.
    /// </summary>
    public string VatsimID { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the departure airport of the aircraft.
    /// </summary>
    public string ADEP { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the destination airport of the aircraft.
    /// </summary>
    public string ADES { get; set; } = string.Empty;

    /// <summary>
    /// Whether this key matches another stripkey.
    /// </summary>
    /// <param name="other">Key to compare.</param>
    /// <returns>Matches.</returns>
    public bool Matches(StripKey other)
    {
        return Callsign == other.Callsign &&
            VatsimID == other.VatsimID &&
            ADEP == other.ADEP &&
            ADES == other.ADES;
    }
}
