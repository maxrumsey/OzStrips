using System.Collections.Generic;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared
{
    /// <summary>
    /// Represents state data sent from the server about a specific aerodrome.
    /// </summary>
    public class AerodromeState
    {
        /// <summary>
        /// Gets or sets a list of connected callsigns.
        /// </summary>
        public List<string> Connections { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the circuit is active.
        /// </summary>
        public bool CircuitActive { get; set; }

        /// <summary>
        /// Gets or sets the aerodrome code.
        /// </summary>
        public string AerodromeCode { get; set; } = string.Empty;
    }
}
