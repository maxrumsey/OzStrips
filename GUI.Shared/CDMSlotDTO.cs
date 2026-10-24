using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
    [JsonRequired]
    public DateTimeOffset PTOT { get; set; } = DateTimeOffset.MaxValue;
}
