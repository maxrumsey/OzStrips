using MaxRumsey.OzStripsPlugin.GUI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI;

internal class CDMAircraftList : List<CDMAircraftDTO>
{
    public CDMAircraftList(List<CDMAircraftDTO> aircraft)
    {
        OverwriteAndAddAiraftList(aircraft);
    }

    public CDMAircraftList()
    {
    }

    public void OverwriteAndAddAiraftList(List<CDMAircraftDTO> newList)
    {
        foreach (var aircraft in newList)
        {
            // Remove "inferior" aircraft.
            RemoveAll(x => x.Key.Matches(aircraft.Key) && x.State <= aircraft.State);

            // If there's no "superior" aircraft in the list, add us.
            if (!Exists(x => x.Key.Matches(aircraft.Key) && x.State > aircraft.State))
            {
                CheckAndAddAircraft(aircraft);
            }
        }
    }

    public void CheckAndAddAircraft(CDMAircraftDTO aircraft)
    {
        var onlinePilot = Network.GetOnlinePilots.Find(x => x.Callsign == aircraft.Key.Callsign);
        if (onlinePilot == null)
        {
            return;
        }

        if (onlinePilot.GroundSpeed > 50)
        {
            aircraft.State = CDMState.COMPLETE;
        }

        Add(aircraft);
    }
}
