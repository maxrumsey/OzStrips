using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Types of CDM slots, with associated priority values.
/// </summary>
public enum CDMSlotTypes
{
    /// <summary>
    /// Max priority.
    /// </summary>
    PRIORITY,

    /// <summary>
    /// Aircraft waiting for a long time.
    /// </summary>
    LONGWAIT,

    /// <summary>
    /// Aircraft with booked slots.
    /// </summary>
    BOOKED,

    /// <summary>
    /// Aircraft with prebooked slots, but not "in the system" yet.
    /// </summary>
    PREBOOKED,

    /// <summary>
    /// Normal aircraft.
    /// </summary>
    NORMAL,

    /// <summary>
    /// Early with a slot.
    /// </summary>
    EARLY_NONCOMPLIANT,
}
