using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation;
using MaxRumsey.OzStripsPlugin.GUI.DTO.XML;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using vatsys;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MaxRumsey.OzStripsPlugin.GUI;

internal class AutoAssigner
{
    private readonly BayManager _bayManager;
    private readonly string _aerodrome;
    private readonly List<AssignmentRule> _assignmentRules = [];

    // Thank you ChatGPT.
    private readonly Regex _depRegex = new(@"(?i)\b\[?RWY\]?[:\s]*((?:(?=[^.,\n]*\b(?:DEPS?|DEPARTURES?|ARRS?\s+AND\s+DEPS?|ALL\s+OTHER\s+DEPS?))[^.,\n]+)|(?:(?![^.,\n]*\bFOR\s+ARRS?\b)[^.,\n]+))(?:(?:[^.,\n]*?(?<Runway>\d{2}[LCR]?))+)");
    private readonly Regex _rwyNameRegex = new(@"(\d{2}[LRC]?)");

    internal AutoAssigner(BayManager bayManager)
    {
        _bayManager = bayManager;
        _aerodrome = bayManager.AerodromeName;

        LoadData(AerodromeManager.AerodromeAutoFillLocation);
    }

    internal void LoadData(string filePrefix)
    {
        try
        {
            var deserializer = new DeserializerBuilder()
            .Build();

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filePrefix, $"{_aerodrome}.yml");

            if (!File.Exists(path))
            {
                return;
            }

            var yaml = File.ReadAllText(path);

            if (yaml.Length == 0)
            {
                return;
            }

            _assignmentRules.AddRange(deserializer.Deserialize<List<AssignmentRule>>(yaml) ?? []);
        }
        catch (YamlException ex)
        {
            Util.ShowErrorBox($"Failed to serialize. Error at line {ex.Start.Index}");
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }
    }

    public string[] DeterminePossibleDepartureFrequencies(Strip strip)
    {
        var deps = DetermineResult(strip).Departures ?? [];

        foreach (var freq in deps.ToArray())
        {
            if (!IsDepartureFreqAvailable(freq))
            {
                deps.Remove(freq);
            }
        }

        deps.Add("122.8");

        return [.. deps];
    }

    internal AssignmentResult DetermineResult(Strip strip)
    {
        var result = new AssignmentResult(strip.RWY);

        foreach (var rule in _assignmentRules)
        {
            if ((rule.If is not null && CompliesWithCondition(rule.If, result, strip)) || rule.If is null)
            {
                if (rule.IfNot is not null && !CompliesWithCondition(rule.IfNot, result, strip, false))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(rule.Runway))
                {
                    result.Runway = rule.Runway;
                }

                if (!string.IsNullOrEmpty(rule.CFL))
                {
                    result.CFL = rule.CFL;
                }

                if (!string.IsNullOrEmpty(rule.SID))
                {
                    result.SID = GetSIDName(strip, rule.SID);
                }

                if (rule.Departures.Count > 0)
                {
                    result.Departures = rule.Departures;
                }

                if (!string.IsNullOrEmpty(rule.AssignDepRunway))
                {
                    var atisRwy = GetATISDepRunway();
                    if (!string.IsNullOrEmpty(atisRwy))
                    {
                        result.Runway = atisRwy;
                    }
                }
            }
        }

        return result;
    }

    internal bool CompliesWithCondition(RuleCondition rule, AssignmentResult result, Strip strip, bool matchAsTrue = true)
    {
        if (!string.IsNullOrEmpty(rule.IsJet) && Performance.GetPerformanceData(strip.FDR.AircraftTypeAndWake)?.IsJet != matchAsTrue)
        {
            return false;
        }

        if (rule.Runway.Count > 0 && rule.Runway.Contains(result.Runway) != matchAsTrue)
        {
            return false;
        }

        if (rule.Waypoint.Count > 0)
        {
            var matched = false;
            foreach (var waypoint in rule.Waypoint)
            {
                if (strip.FDR.ParsedRoute.Any(x => x.Intersection.Name == waypoint))
                {
                    matched = true;
                    break;
                }
            }

            if (matched != matchAsTrue)
            {
                return false;
            }
        }

        if (rule.SID.Count > 0 && rule.SID.Contains(result.SID) != matchAsTrue)
        {
            return false;
        }

        if (rule.AtisDepRunway.Count > 0)
        {
            if (string.IsNullOrEmpty(_bayManager.AerodromeState.ATIS))
            {
                return false;
            }

            var unmatchedRunways = rule.AtisDepRunway.ToList();

            var matches = _depRegex.Matches(_bayManager.AerodromeState.ATIS);
            if (matches?.Count > 0)
            {
                foreach (var rwy in rule.AtisDepRunway)
                {
                    foreach (Match match in matches)
                    {
                        if (match.Value.Contains(rwy))
                        {
                            unmatchedRunways.Remove(rwy);
                        }
                    }
                }

                // if we matched all the runways, and the rule has been met.
                if (unmatchedRunways.Count == 0 != matchAsTrue)
                {
                    return false;
                }
            }
        }

        if (rule.Radials.Count > 0)
        {
            var matched = false;

            foreach (var radial in rule.Radials)
            {
                var stringRadials = radial.Split('-');

                if (stringRadials.Length != 2)
                {
                    continue;
                }

                int[] bearings = [0, 360];

                foreach (var stringRadial in stringRadials)
                {
                    if (int.TryParse(stringRadial, out var bearing))
                    {
                        bearings[Array.IndexOf(stringRadials, stringRadial)] = bearing;
                    }
                }

                var depRadial = TrueToMagnetic(strip.OutboundRadial, _aerodrome);

                if (depRadial == -1)
                {
                    continue;
                }

                var inRange = depRadial >= bearings[0] && depRadial < bearings[1];

                if (inRange)
                {
                    matched = true;
                }
            }

            if (matched != matchAsTrue)
            {
                return false;
            }
        }

        return true;
    }

    internal static string GetSIDName(Strip strip, string shortSID)
    {
        var rwys = Airspace2.GetRunways(strip.FDR.DepAirport);

        return rwys.Select(x => x.SIDs.FirstOrDefault(x => x.sidStar.Name.Contains(shortSID))).FirstOrDefault(x => x.sidStar is not null).sidStar.Name ?? shortSID;
    }

    public string GetATISDepRunway()
    {
        var matches = _depRegex.Matches(_bayManager.AerodromeState.ATIS);

        if (matches is null || matches.Count == 0)
        {
            return string.Empty;
        }

        var rawText = matches[0].Value;

        var nameMatches = _rwyNameRegex.Match(rawText);

        if (nameMatches.Success)
        {
            return nameMatches.Value;
        }

        return string.Empty;
    }

    public static int TrueToMagnetic(int trueBearing, string aerodrome)
    {
        var correction = LogicalPositions.Positions.FirstOrDefault(x => x.Name == aerodrome)?.MagneticVariation ?? 0;

        return (int)(trueBearing + correction);
    }

    public static bool IsDepartureFreqAvailable(string freq)
    {
        var atc = Network.GetOnlineATCs;
        var allFreqs = atc.SelectMany(x => x.Frequencies ?? []).Select(x => Conversions.FSDFrequencyToString(x)).ToArray() ?? [];

        return allFreqs.Contains(freq);
    }

    public static string DetermineDepFreq(List<string> freqs)
    {

        foreach (var freq in freqs)
        {
            if (IsDepartureFreqAvailable(freq))
            {
                return freq;
            }
        }

        return "122.8";
    }

    public bool IsFunctional => _assignmentRules.Count > 0 && !string.IsNullOrEmpty(_bayManager.AerodromeState.ATIS);
}
