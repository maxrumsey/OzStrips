using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Response when a client subscribes to a specific aerodrome.
/// </summary>
public class AerodromeSubscriptionResponse
{
    /// <summary>
    /// Gets or sets the aerodrome ICAO.
    /// </summary>
    public string AerodromeICAO { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the server code.
    /// </summary>
    public ConnectionMetadataDTO.Servers Server { get; set; }

    /// <summary>
    /// Gets or sets the list of strips.
    /// </summary>
    public StripDTO[] StripCache { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of bays.
    /// </summary>
    public BayDTO[] Bays { get; set; } = [];

    /// <summary>
    /// Gets or sets the connection error.
    /// </summary>
    public Exception? Error { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AerodromeSubscriptionResponse"/> class.
    /// </summary>
    public AerodromeSubscriptionResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AerodromeSubscriptionResponse"/> class.
    /// </summary>
    /// <param name="ex">Exception.</param>
    public AerodromeSubscriptionResponse(Exception ex)
    {
        Error = ex;
    }
}
