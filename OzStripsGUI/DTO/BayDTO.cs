using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace maxrumsey.ozstrips.gui.DTO
{
    public class BayDTO
    {
        [JsonPropertyName("bay")]
        public StripBay bay { get; set; }

        [JsonPropertyName("list")]
        public List<string> list { get; set; }

    }
}
