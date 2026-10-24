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
    /// Gets or sets the aircraft associated with this result.
    /// </summary>
    [JsonRequired]
    public CDMAircraftDTO Aircraft { get; set; } = null!;

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
}
