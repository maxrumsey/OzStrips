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
    public StripKey Key { get; set; } = new();

    public string RWY { get; set; } = string.Empty;

    public CDMSlotTypes SlotType { get; set; } = CDMSlotTypes.NORMAL;
}
