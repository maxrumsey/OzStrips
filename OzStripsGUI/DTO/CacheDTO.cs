using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace maxrumsey.ozstrips.gui.DTO
{
    public class CacheDTO
    {
        [JsonPropertyName("strips")]
        public List<StripControllerDTO> strips { get; set; }
    }
}
