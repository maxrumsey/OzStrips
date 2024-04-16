using System.Text.Json.Serialization;

namespace maxrumsey.ozstrips.gui.DTO
{
    public class MetadataDTO
    {
        [JsonPropertyName("version")]
        public string version { get; set; }

        [JsonPropertyName("apiversion")]
        public string apiversion { get; set; }

    }
}
