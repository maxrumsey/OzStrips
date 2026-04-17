using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MaxRumsey.OzStripsPlugin.GUI.Shared.ConnectionMetadataDTO;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

public class MessageMetadata(string icao, Servers server)
{
    public string AerodromeICAO { get; set; } = icao;

    public Servers Server { get; set; } = server;
}
