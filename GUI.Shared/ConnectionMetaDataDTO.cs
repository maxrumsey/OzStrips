using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Represents metadata about the connection to the OzStrips server.
/// </summary>
public record ConnectionMetadataDTO
{
    /// <summary>
    /// Available server types.
    /// </summary>
    public enum Servers
    {
        /// <summary>
        /// Default connection.
        /// </summary>
        VATSIM,

        /// <summary>
        /// Sweatbox 1.
        /// </summary>
        SWEATBOX1,

        /// <summary>
        /// Sweatbox 2.
        /// </summary>
        SWEATBOX2,

        /// <summary>
        /// Sweatbox 3.
        /// </summary>
        SWEATBOX3,
    }

    /// <summary>
    /// Gets or sets the version of the OzStrips plugin.
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API version of the client.
    /// </summary>
    public string APIVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current server type.
    /// </summary>
    public Servers Server { get; set; }

    /// <summary>
    /// Gets or sets the current aerodrome nmae.
    /// </summary>
    public string AerodromeName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user callsign.
    /// </summary>
    public string Callsign { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the VATSIM ID of the user.
    /// </summary>
    public string VatsimID { get; set; } = string.Empty;
}
