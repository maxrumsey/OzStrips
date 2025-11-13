using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Contains and-style rules for auto-assignment.
/// </summary>
public class RuleCondition
{
    /// <summary>
    /// Gets or sets a list of runways, where at least one must be on the ATIS as a departure runway.
    /// </summary>
    [YamlMember(Alias = "atis_dep_rwy")]
    public List<string> AtisDepRunway { get; set; } = [];

    /// <summary>
    /// Gets or sets a list of runways, where at least one must be the planned departure runway.
    /// </summary>
    [YamlMember(Alias = "runway")]
    public List<string> Runway { get; set; } = [];

    /// <summary>
    /// Gets or sets a list of waypoints, where at least one must be in the planned route.
    /// </summary>
    [YamlMember(Alias = "waypoint")]
    public List<string> Waypoint { get; set; } = [];

    /// <summary>
    /// Gets or sets a list of SIDs, where at least one must be the planned SID.
    /// </summary>
    [YamlMember(Alias = "SID")]
    public List<string> SID { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether the aircraft should be a jet.
    /// </summary>
    [YamlMember(Alias = "jet")]
    public string IsJet { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a list of radials, where at least one must match the planned departure radial.
    /// </summary>
    [YamlMember(Alias = "radials")]
    public List<string> Radials { get; set; } = [];
}
