using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Represents the result of an assignment attempt.
/// </summary>
public class AssignmentResult(string baseRunway)
{
    private readonly string _baseRunway = baseRunway;

    private string _overrideRunway = string.Empty;

    /// <summary>
    /// Gets or sets the runway assigned.
    /// </summary>
    public string Runway
    {
        get
        {
            return !string.IsNullOrEmpty(_baseRunway) ? _baseRunway : _overrideRunway;
        }

        set
        {
            _overrideRunway = value;
        }
    }

    /// <summary>
    /// Gets or sets the SID assigned.
    /// </summary>
    public string SID { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the departure frequencies assigned.
    /// </summary>
    public List<string> Departures { get; set; } = [];

    /// <summary>
    /// Gets or sets the CFL assigned.
    /// </summary>
    public string CFL { get; set; } = string.Empty;
}
