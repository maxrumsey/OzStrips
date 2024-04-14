using System.Text.Json.Serialization;

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

        [JsonPropertyName("TOT")]
        public string TOT { get; set; }

        [JsonPropertyName("crossing")]
        public bool Crossing { get; set; }

    }
}
