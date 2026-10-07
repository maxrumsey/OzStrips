using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MaxRumsey.OzStripsPlugin.GUI.Shared.ConnectionMetadataDTO;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Contains message metadata sent with each SignalR message.
/// </summary>
public class MessageMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    /// <param name="icao">ICAO code.</param>
    /// <param name="server">Server.</param>
    [Obsolete("Included for backwards compatibility.")]
    public MessageMetadata(string icao, Servers server)
    {
        AerodromeICAO = icao;
        Server = server;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    /// <param name="ait">AerodromeIdentificationToken.</param>
    public MessageMetadata(AerodromeIdentificationToken ait)
    {
        AIT = ait;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    public MessageMetadata()
    {
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
    /// Gets or sets the trace id.
    /// </summary>
    public string TraceID { get; set; } = null!;
}
