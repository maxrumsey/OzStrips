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
    PRIORITY = 100,

    /// <summary>
    /// Aircraft waiting for a long time.
    /// </summary>
    LONGWAIT = 80,

    /// <summary>
    /// Aircraft with booked slots.
    /// </summary>
    BOOKED = 50,

    /// <summary>
    /// Aircraft with prebooked slots, but not "in the system" yet.
    /// </summary>
    PREBOOKED = 50,

    /// <summary>
    /// Normal aircraft.
    /// </summary>
    NORMAL = 30,

    /// <summary>
    /// Early with a slot.
    /// </summary>
    EARLY_NONCOMPLIANT = 30,
}
