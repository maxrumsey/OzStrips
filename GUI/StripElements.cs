using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI;

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
        FIRST_WPT,
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
        PDC_INDICATOR,
        RWY,
        WTC,
        ROUTE,
        DEPFREQ,
    }

    public enum Actions
    {
        NONE,
        SHOW_ROUTE,
        OPEN_HDG_ALT,
        OPEN_FDR,
        PICK,
        ASSIGN_SSR,
        MOD_SID,
        OPEN_REROUTE,
        MOD_RWY,
        MOD_CFL,
        MOD_CLX,
        MOD_STD,
        MOD_GLOP,
        MOD_REMARK,
        COCK,
        SID_TRIGGER,
        SET_READY,
        SET_TOT,
        OPEN_PDC,
        OPEN_PM,
        OPEN_CDM,
    }

    public enum HoverActions
    {
        NONE,
        ROUTE_WARNING,
        RFL_WARNING,
        SSR_WARNING,
        SID_TRIGGER,
    }
}
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore SA1602 // Enumeration items should be documented
