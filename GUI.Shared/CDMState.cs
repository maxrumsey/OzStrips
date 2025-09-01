using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// States for each CDM aircraft.
/// </summary>
public enum CDMState
{
    /// <summary>
    /// An aircraft with a slot.
    /// </summary>
    PREACTIVE = 10,

    /// <summary>
    /// An aircraft that is in a queue.
    /// </summary>
    ACTIVE = 20,

    /// <summary>
    /// An aircraft that has pushed.
    /// </summary>
    PUSHED = 30,

    /// <summary>
    /// An aircraft that has departed.
    /// </summary>
    DEPARTED = 40,
}
