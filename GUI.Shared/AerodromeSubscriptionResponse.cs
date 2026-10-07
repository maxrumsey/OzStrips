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

    /// <summary>
    /// Gets or sets the aerodrome ICAO.
    /// </summary>
    [Obsolete("Included for backwards compatibility.")]
    public string AerodromeICAO
    {
        get => AIT.ICAOCode;
        set => AIT.ICAOCode = value;
    }

    /// <summary>
    /// Gets or sets the server code.
    /// </summary>
    [Obsolete("Included for backwards compatibility.")]

    public ConnectionMetadataDTO.Servers Server
    {
        get => (ConnectionMetadataDTO.Servers)AIT.OldServerNumber;
        set => AIT.OldServerNumber = (int)value;
    }

    /// <summary>
    /// Gets or sets the aerodrome identification token (AIT).
    /// </summary>
    public AerodromeIdentificationToken AIT { get; set; } = new AerodromeIdentificationToken();

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
}
