using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vatsys;
using SocketIOClient;
using System.Text.Json.Serialization;

namespace maxrumsey.ozstrips
{
    internal class UpdatePacket
    {
        [JsonPropertyName("callsign")]
        public string Callsign { get; set; }

        [JsonPropertyName("acftType")]
        public string AircraftType { get; set; }

        [JsonPropertyName("frul")]
        public string FRUL {  get; set; }

        [JsonPropertyName("adep")]
        public string Adep { get; set; }

        [JsonPropertyName("ades")]
        public string Ades { get; set; }

        [JsonPropertyName("rawRoute")]
        public string RawRoute { get; set; }

        [JsonPropertyName("wtc")]
        public string WTC { get; set; }

        [JsonPropertyName("fdrState")]
        public string FDRState { get; set; }

        [JsonPropertyName("hasDeparted")]
        public string HasDeparted { get; set;}

        [JsonPropertyName("sid")]
        public string SID { get; set; }

        [JsonPropertyName("runway")]
        public string Runway { get; set; }

        [JsonPropertyName("ad_runways")]
        public string ADRunways { get; set; }

        [JsonPropertyName("ad_sids")]
        public string AD_SIDS { get; set; }

    }
}
