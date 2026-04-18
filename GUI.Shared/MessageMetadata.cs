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
    public MessageMetadata(string icao, Servers server)
    {
        AerodromeICAO = icao;
        Server = server;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    public MessageMetadata()
    {
    }

    /// <summary>
    /// Gets or sets the aerodrome ICAO code.
    /// </summary>
    public string AerodromeICAO { get; set; } = null!;

    /// <summary>
    /// Gets or sets the server type.
    /// </summary>
    public Servers Server { get; set; }
}
