using System.Collections.Generic;

namespace MaxRumsey.OzStripsPlugin.Gui.DTO
{
    internal class AerodromeState
    {
        public List<string> Connections { get; set; } = new List<string>();

        public bool CircuitActive;

        public string AerodromeCode { get; set; } = string.Empty;
    }
}
