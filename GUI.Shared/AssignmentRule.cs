using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Represents a single assignment rule.
/// </summary>
public record AssignmentRule
{
    /// <summary>
    /// Gets or sets a value indicating whether to try and assign a default departure runway.
    /// </summary>
    [YamlMember(Alias = "assign_dep_rwy")]
    public string AssignDepRunway { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets conditions that must be matched to assign this rule.
    /// </summary>
    [YamlMember(Alias = "if")]
    public RuleCondition? If { get; set; }

    /// <summary>
    /// Gets or sets conditions that must not be matched to assign this rule.
    /// </summary>
    [YamlMember(Alias = "if_not")]
    public RuleCondition? IfNot { get; set; }

    /// <summary>
    /// Gets or sets the runway to assign.
    /// </summary>
    [YamlMember(Alias = "runway")]
    public string Runway { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SID to assign.
    /// </summary>
    [YamlMember(Alias = "SID")]
    public string SID { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the departure frequencies to try and assign.
    /// </summary>
    [YamlMember(Alias = "departures")]
    public List<string> Departures { get; set; } = [];

    /// <summary>
    /// Gets or sets the CFL to be assigned.
    /// </summary>
    [YamlMember(Alias = "CFL")]
    public string CFL { get; set; } = string.Empty;
}
