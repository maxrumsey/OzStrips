using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public StripKey Key { get; set; } = new();

    /// <summary>
    /// Gets or sets the assigned runway.
    /// </summary>
    public string RWY { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the slot type.
    /// </summary>
    public CDMSlotTypes SlotType { get; set; } = CDMSlotTypes.NORMAL;

    /// <summary>
    /// Gets or sets the TOBT.
    /// </summary>
    public DateTime TOBT { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// Gets or sets the AOBT.
    /// </summary>
    public DateTime AOBT { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// Gets or sets the ATOT.
    /// </summary>
    public DateTime ATOT { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// Gets or sets the current state.
    /// </summary>
    public CDMState State { get; set; } = CDMState.PREACTIVE;
}
