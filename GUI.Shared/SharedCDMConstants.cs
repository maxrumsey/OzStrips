using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Contains shared CDM constants.
/// </summary>
public static class SharedCDMConstants
{
    /// <summary>
    /// Represents a mapping between <see cref="StripBay"/> values and their corresponding <see cref="CDMState"/>
    /// values.
    /// </summary>
    public static Dictionary<StripBay, CDMState> BAY_STATE_MAP = new()
        {
            { StripBay.BAY_PUSHED, CDMState.PUSHED },
            { StripBay.BAY_TAXI, CDMState.PUSHED },
            { StripBay.BAY_HOLDSHORT, CDMState.PUSHED },
            { StripBay.BAY_RUNWAY, CDMState.PUSHED },

            { StripBay.BAY_CIRCUIT, CDMState.COMPLETE },
            { StripBay.BAY_OUT, CDMState.COMPLETE },
            { StripBay.BAY_DEAD, CDMState.COMPLETE },
        };
}
