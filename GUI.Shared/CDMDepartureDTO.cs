using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

public class CDMDepartureDTO
{
    public string Callsign { get; set; } = string.Empty;

    public DateTime ATOT { get; set; } = DateTime.MaxValue;
}
