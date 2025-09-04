using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Contains parameters for CDM operations at a specific aerodrome.
/// </summary>
public class CDMParameters
{
    /// <summary>
    /// Gets or sets the default DAR.
    /// </summary>
    public TimeSpan? DefaultRate { get; set; }
}
