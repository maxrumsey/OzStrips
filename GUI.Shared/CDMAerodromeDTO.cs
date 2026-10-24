using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared
{
    /// <summary>
    /// CDMAerodrome DTO.
    /// </summary>
    public class CDMAerodromeDTO
    {
        /// <summary>
        /// Gets or sets the list of CDM active aircraft.
        /// </summary>
        [JsonRequired]
        public List<CDMAircraftDTO> Aircraft { get; set; } = [];

        /// <summary>
        /// Gets or sets the server code.
        /// </summary>
        [JsonRequired]
        public string AerodromeServerCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of slots.
        /// </summary>
        [JsonRequired]
        public List<CDMSlotDTO> Slots { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of departures.
        /// </summary>
        [JsonRequired]
        public List<CDMDepartureDTO> Departures { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of CDM results last generated.
        /// </summary>
        /// Gets a thread-safe copy of CDM results.
        [JsonRequired]
        public List<CDMResultDTO> CurrentResults { get; set; } = [];

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        [JsonRequired]
        public CDMParameters Parameters { get; set; } = new();
    }
}
