using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// An aircraft in the CDM system.
/// </summary>
public class CDMAircraftDTO
{
    /// <summary>
    /// Gets or sets the strip key.
    /// </summary>
    [JsonRequired]
    public StripKey Key { get; set; } = new();

    /// <summary>
    /// Gets or sets the assigned runway.
    /// </summary>
    [JsonRequired]
    public string RWY { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the slot type.
    /// </summary>
    [JsonRequired]
    public CDMSlotTypes SlotType { get; set; } = CDMSlotTypes.NORMAL;

    /// <summary>
    /// Gets or sets the TOBT.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset TOBT { get; set; } = DateTimeOffset.MaxValue;

    /// <summary>
    /// Gets or sets the AOBT.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset AOBT { get; set; } = DateTimeOffset.MaxValue;

    /// <summary>
    /// Gets or sets the ATOT.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset ATOT { get; set; } = DateTimeOffset.MaxValue;

    /// <summary>
    /// Gets or sets the current state.
    /// </summary>
    [JsonRequired]
    public CDMState State { get; set; } = CDMState.PREACTIVE;
}
