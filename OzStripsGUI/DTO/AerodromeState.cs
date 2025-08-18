using System.Collections.Generic;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO
{
    public class AerodromeState
    {
        public List<string>? Connections { get; set; } = new List<string>();

        public bool CircuitActive { get; set; }

        public string AerodromeCode { get; set; } = string.Empty;
    }
}
