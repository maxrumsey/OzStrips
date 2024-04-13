using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace maxrumsey.ozstrips.gui.DTO
{
    public class StripControllerDTO
    {
        [JsonPropertyName("acid")]
        public string ACID { get; set; }

        [JsonPropertyName("bay")]
        public StripBay bay { get; set; }

        [JsonPropertyName("cockLevel")]
        public int StripCockLevel { get; set; }

        [JsonPropertyName("CLX")]
        public string CLX { get; set; }

        [JsonPropertyName("GATE")]
        public string GATE { get; set; }

    }
}
