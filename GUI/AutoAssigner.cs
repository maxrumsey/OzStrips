using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms.VisualStyles;
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

    private readonly Regex _rwyNameRegex = new(@"^(\d{2}[LRC]?|[LRC])$");

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
            Util.ShowErrorBox($"Failed to serialize. Error at line {ex.Start.Line}, character {ex.Start.Column} in file {_aerodrome}.yml");
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
        if (!string.IsNullOrEmpty(rule.IsJet))
        {
            var type = strip.FDR.AircraftTypeAndWake;

            // if we can't find the type, assume its a prop.
            var isJet = type is not null && Performance.GetPerformanceData(type)?.IsJet == true;

            if (isJet != matchAsTrue)
            {
                return false;
            }
        }

        if (!string.IsNullOrEmpty(rule.VFR) && (strip.FDR.FlightRules == "V") != matchAsTrue)
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

        if (rule.Airway.Count > 0)
        {
            var matched = false;
            foreach (var airway in rule.Airway)
            {
                if (strip.FDR.ParsedRoute.Any(x => x.AirwayName == airway))
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

        if (rule.WTC.Count > 0)
        {
            var matched = false;
            foreach (var wtc in rule.WTC)
            {
                if (strip.FDR.AircraftWake == wtc)
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

            var depRunways = GetDepartureRunways();
            if (depRunways.Length > 0)
            {
                foreach (var rwy in rule.AtisDepRunway)
                {
                    if (depRunways.Contains(rwy))
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

        return rwys?.Select(x => x.SIDs.FirstOrDefault(x => x.sidStar.Name.Contains(shortSID)))?.FirstOrDefault(x => x.sidStar is not null).sidStar?.Name ?? shortSID;
    }

    public string[] GetDepartureRunways()
    {
        if (string.IsNullOrEmpty(_bayManager.AerodromeState.ATIS))
        {
            return [];
        }

        var runwayLine = _bayManager.AerodromeState.ATIS.ToUpperInvariant().Split('\n').FirstOrDefault(x => x.Contains("[RWY]") || x.Contains("RWY:")) ?? string.Empty;

        if (!string.IsNullOrEmpty(runwayLine))
        {
            return GetDepartureRunways(runwayLine);
        }

        return [];
    }

    private string[] GetDepartureRunways(string rwyLine)
    {
        var runways = new List<string>();

        var currentSentence = new List<string>();
        var currentWord = string.Empty;

        for (var i = 0; i < rwyLine.Length; i++)
        {
            var currentChar = rwyLine[i];
            var lastChar = i == rwyLine.Length - 1;

            if (char.IsLetterOrDigit(currentChar))
            {
                currentWord += currentChar;
            }

            if ("., ".Contains(currentChar) || lastChar)
            {
                if (currentWord.Length > 0)
                {
                    currentSentence.Add(currentWord);
                    currentWord = string.Empty;
                }

                if (currentChar == ',' || currentChar == '.' || lastChar)
                {
                    if (currentSentence.Count > 0)
                    {
                        var potentialRunways = new List<string>();
                        var matchDeps = currentSentence.Any(x => x.Contains("DEP"));
                        var matchArrs = currentSentence.Any(x => x.Contains("ARR"));

                        if (matchArrs && !matchDeps)
                        {
                            // This sentence is arrivals only.

                            currentSentence.Clear();
                            continue;
                        }

                        foreach (var word in currentSentence)
                        {
                            var match = _rwyNameRegex.Match(word);
                            if (match.Success)
                            {
                                potentialRunways.Add(match.Value);
                            }
                        }

                        var lastFullRunwayMatched = string.Empty;

                        foreach (var runway in potentialRunways)
                        {
                            // If this is a 'L' or 'R' style of runway, and we have a full runway matched before, use that number.
                            if (runway.Length == 1 && !string.IsNullOrEmpty(lastFullRunwayMatched))
                            {
                                var number = string.Join(string.Empty, lastFullRunwayMatched.Where(char.IsDigit).ToArray());

                                runways.Add(number + runway);
                            }
                            else if (runway.Length >= 2)
                            {
                                runways.Add(runway);
                                lastFullRunwayMatched = runway;
                            }
                        }
                    }

                    currentSentence.Clear();
                }
            }
        }

        return runways.ToArray();
    }

    public string GetATISDepRunway()
    {
        return GetDepartureRunways().FirstOrDefault() ?? string.Empty;
    }

    public static int TrueToMagnetic(int trueBearing, string aerodrome)
    {
        var correction = LogicalPositions.Positions.FirstOrDefault(x => x.Name == aerodrome)?.MagneticVariation ?? 0;

        return (int)(trueBearing + correction);
    }

    public static bool IsDepartureFreqAvailable(string freq)
    {
        var atc = Network.GetOnlineATCs;
        var allFreqs = atc.SelectMany(x => x.Frequencies ?? []).Select(x => Conversions.FSDFrequencyToString(x)).ToList() ?? [];
        allFreqs.AddRange(Network.Me.Frequencies?.Select(x => Conversions.FSDFrequencyToString(x)) ?? []);
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
