using MaxRumsey.OzStripsPlugin.GUI.Shared;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared
{
    /// <summary>
    /// Child item of a bay.
    /// </summary>
    public class BayItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether this is an aircraft strip.
        /// </summary>
        public bool IsStrip { get; set; }

        /// <summary>
        /// Gets or sets the stripkey.
        /// </summary>
        public StripKey? StripKey { get; set; }

        /// <summary>
        /// Gets or sets the bar identifier.
        /// </summary>
        public string? BarIdentifier { get; set; }

        /// <summary>
        /// Converts to a string DTO.
        /// </summary>
        /// <returns>DTO.</returns>
        public string ToDTO()
        {
            if (IsStrip)
            {
                return StripKey?.Callsign ?? string.Empty;
            }
            else
            {
                return BarIdentifier ?? string.Empty;
            }
        }
    }
}
