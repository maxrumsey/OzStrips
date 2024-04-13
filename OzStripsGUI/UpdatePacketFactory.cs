using vatsys;

namespace maxrumsey.ozstrips
{
    internal class UpdatePacketFactory
    {

        public static UpdatePacket CreateUpdatePacket(FDP2.FDR fdp)
        {
            UpdatePacket updatePacket = new UpdatePacket();
            updatePacket.Callsign = fdp.Callsign;
            updatePacket.Adep = fdp.DepAirport;
            updatePacket.Ades = fdp.DesAirport;
            updatePacket.AircraftType = fdp.AircraftType;
            updatePacket.WTC = fdp.AircraftWake;
            updatePacket.FRUL = fdp.FlightRules;
            updatePacket.RawRoute = fdp.Route;

            return updatePacket;
        }
    }
}
