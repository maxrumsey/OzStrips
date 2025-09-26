using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Contains Slot information for an aircraft.
/// </summary>
public class CDMSlotDTO
{
    /// <summary>
    /// Gets or sets the planned take off time.
    /// </summary>
    public DateTime PTOT { get; set; } = DateTime.MaxValue;
}
