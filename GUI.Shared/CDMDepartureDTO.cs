using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// CDM departure information.
/// </summary>
public class CDMDepartureDTO
{
    /// <summary>
    /// Gets or sets the callsign.
    /// </summary>
    public string Callsign { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ATOT.
    /// </summary>
    public DateTime ATOT { get; set; } = DateTime.MaxValue;
}
