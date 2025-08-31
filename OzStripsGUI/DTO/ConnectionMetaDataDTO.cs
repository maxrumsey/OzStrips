using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.DTO;

/// <summary>
/// Represents metadata about the connection to the OzStrips server.
/// </summary>
public struct ConnectionMetadataDTO
{
    /// <summary>
    /// Gets or sets the version of the OzStrips plugin.
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the API version of the client.
    /// </summary>
    public string APIVersion { get; set; }

    /// <summary>
    /// Gets or sets the current server type.
    /// </summary>
    public SocketConn.Servers Server { get; set; }

    /// <summary>
    /// Gets or sets the current aerodrome nmae.
    /// </summary>
    public string AerodromeName { get; set; }

    /// <summary>
    /// Gets or sets the user callsign.
    /// </summary>
    public string Callsign { get; set; }

    /// <summary>
    /// Gets or sets the VATSIM ID of the user.
    /// </summary>
    public string VatsimID { get; set; }

}
