using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MaxRumsey.OzStripsPlugin.GUI.Shared.ConnectionMetadataDTO;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

public class MessageMetadata
{
    public string AerodromeICAO { get; set; }

    public Servers Server { get; set; }

    public MessageMetadata(string icao, Servers server)
    {
        AerodromeICAO = icao;
        Server = server;
    }

    public MessageMetadata()
    {
    }
}
