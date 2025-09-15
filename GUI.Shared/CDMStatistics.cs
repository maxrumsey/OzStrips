using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Represents server sent CDM stats.
/// </summary>
public class CDMStatistics
{
    /// <summary>
    /// Gets or sets a dictionary containg actual CDM dep rates.
    /// </summary>
    public Dictionary<int, float> CDMRates { get; set; } = new()
    {
        { 5, 5 },
        { 15, 15 },
        { 30, 30 },
    };
}
