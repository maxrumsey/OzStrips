using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Contains PDC request data.
/// </summary>
public class PDCRequest
{
    /// <summary>
    /// Contains flags representing status and progress of a PDC request.
    /// </summary>
    [Flags]
    public enum PDCFlags
    {
        /// <summary>
        /// No data.
        /// </summary>
        NULL = 0,

        /// <summary>
        /// Callsign is connected to Hoppies.
        /// </summary>
        ONLINE = 1,

        /// <summary>
        /// PDC request has been received.
        /// </summary>
        REQUESTED = 2,

        /// <summary>
        /// PDC request has been acknowledged.
        /// </summary>
        ACKNOWLEDGED = 4,

        /// <summary>
        /// PDC has been sent.
        /// </summary>
        SENT = 8,
    }

    /// <summary>
    /// Gets or sets the callsign associated with this PDC request.
    /// </summary>
    public string Callsign { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets flags representing status and progress of this PDC request.
    /// </summary>
    public PDCFlags Flags { get; set; }

    /// <summary>
    /// Gets or sets the last modified time of this PDC request.
    /// </summary>
    [JsonIgnore]
    public DateTime LastModified { get; set; } = DateTime.Now;
}
