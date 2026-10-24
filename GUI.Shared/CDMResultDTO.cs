using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// DTO for CDM Results.
/// </summary>
public class CDMResultDTO
{
    /// <summary>
    /// Specifies the type of result, such as aircraft or ground stop.
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// Aircraft.
        /// </summary>
        AIRCRAFT,

        /// <summary>
        /// Ground stop.
        /// </summary>
        GROUND_STOP,
    }

    /// <summary>
    /// Gets or sets the aircraft associated with this result.
    /// </summary>
    public CDMAircraftDTO? Aircraft { get; set; }

    /// <summary>
    /// Gets or sets the TSAT.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset TSAT { get; set; } = DateTimeOffset.MaxValue;

    /// <summary>
    /// Gets or sets the CTOT.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset CTOT { get; set; } = DateTimeOffset.MaxValue;

    /// <summary>
    /// Gets or sets the final computed slot type.
    /// </summary>
    [JsonRequired]
    public CDMSlotTypes FinalSlotType { get; set; } = CDMSlotTypes.NORMAL;

    /// <summary>
    /// Gets or sets a valid and correlated slot.
    /// </summary>
    public CDMSlotDTO? Slot { get; set; }

    /// <summary>
    /// Gets or sets the duration of the ground stop.
    /// </summary>
    [JsonRequired]
    public TimeSpan GroundStopLength { get; set; } = TimeSpan.MaxValue;

    /// <summary>
    /// Gets or sets a value indicating whether the entity is affected by a ground stop.
    /// </summary>
    [JsonRequired]
    public bool AffectedByGroundStop { get; set; }

    /// <summary>
    /// Gets or sets the result type.
    /// </summary>
    [JsonRequired]
    public ResultType Type { get; set; } = ResultType.AIRCRAFT;
}
