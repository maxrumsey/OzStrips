using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
