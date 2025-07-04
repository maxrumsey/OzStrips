using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO;

public class StripKey
{
    public string Callsign { get; set; } = string.Empty;

    public string VatsimID { get; set; } = string.Empty;

    public string ADEP { get; set; } = string.Empty;

    public string ADES { get; set; } = string.Empty;

    public bool Matches(StripKey other)
    {
        return Callsign == other.Callsign &&
            VatsimID == other.VatsimID &&
            ADEP == other.ADEP &&
            ADES == other.ADES;
    }
}
