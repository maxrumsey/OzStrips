using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.Gui;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1602 // Enumeration items should be documented
public static class StripElements
{
    /// <summary>
    /// List of possible values.
    /// </summary>
    public enum Values
    {
        EOBT,
        ACID,
        SSR,
        TYPE,
        FRUL,
        FIRSTWPT,
        SID,
        ADES,
        CFL,
        HDG,
        CLX,
        STAND,
        REMARK,
        TOT,
        RFL,
        READY,
        GLOP,
        SSRSYMBOL,
        RWY,
        WTC,
        ROUTE,
    }

    public enum Actions
    {
        NONE,
        SHOW_ROUTE,
        OPEN_HDG_ALT,
        OPEN_FDR,
        PICK,
        ASSIGN_SSR,
        OPEN_SID,
        OPEN_REROUTE,
        OPEN_RWY,
        OPEN_CFL,
    }
}
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore SA1602 // Enumeration items should be documented
