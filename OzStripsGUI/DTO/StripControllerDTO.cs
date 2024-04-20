using System.Text.Json.Serialization;

namespace maxrumsey.ozstrips.gui.DTO
{
    public class StripControllerDTO
    {
        [JsonPropertyName("acid")]
        public string acid { get; set; } = "";

        [JsonPropertyName("bay")]
        public StripBay bay { get; set; } = 0;

        [JsonPropertyName("cockLevel")]
        public int cockLevel { get; set; } = 0;

        [JsonPropertyName("CLX")]
        public string CLX { get; set; } = "";

        [JsonPropertyName("GATE")]
        public string GATE { get; set; } = "";

        [JsonPropertyName("TOT")]
        public string TOT { get; set; } = "\0";

        [JsonPropertyName("crossing")]
        public bool crossing { get; set; } = false;

        [JsonPropertyName("subbay")]
        public string subbay { get; set; } = "";

        [JsonPropertyName("remark")]
        public string remark { get; set; } = "";

    }
}
