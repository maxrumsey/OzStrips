using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Metadata included with every request.
/// </summary>
public class RequestMetadata
{
    /// <summary>
    /// Gets or sets the aerodrome icao code.
    /// </summary>
    public string AerodromeICAO { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the server code.
    /// </summary>
    public int ServerCode { get; set; }
}
