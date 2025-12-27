using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Enum of alert types.
/// </summary>
[Flags]
public enum AlertTypes
{
    /// <summary>
    /// Incorrect RFL.
    /// </summary>
    RFL = 1,

    /// <summary>
    /// Invalid route.
    /// </summary>
    ROUTE = 2,

    /// <summary>
    /// Invalid SSR code.
    /// </summary>
    SSR = 4,

    /// <summary>
    /// No HDG set.
    /// </summary>
    NO_HDG = 8,

    /// <summary>
    /// VFR with a SID.
    /// </summary>
    VFR_SID = 16,

    /// <summary>
    /// Aircraft not ready.
    /// </summary>
    READY = 32,
}
