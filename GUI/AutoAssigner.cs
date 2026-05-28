using System;
using System.Collections.Generic;
using System.Globalization;
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
    private readonly Regex _euroScopeApproachRunwayRegex = new(@"\bAPPROACH\s+RUNWAYS?\s+(?<runways>.*?)(?=\b(?:DRY|WET|FRICTION|DEPARTURE|TRANSITION|WEATHER|WIND|VISIBILITY|CLOUDS|TEMPERATURE|DEW|QNH|NO\s+SIGNIFICANT|ACKNOWLEDGE)\b|$)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private readonly Regex _runwayTokenRegex = new(@"\b(?<number>[0-3]?\d)\s*(?<side>L|R|C|LEFT|RIGHT|CENTRE|CENTER)?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private readonly Regex _tempRegex = new(@"\n?\s*\+?\s*\[TMP\] (-?\d{1,2})");

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
                    result.SID = GetSIDName(strip, rule.SID, result.Runway);
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
            var requestedJet = !bool.TryParse(rule.IsJet, out var parsedJet) || parsedJet;
            var isJet = IsJetAircraft(strip);
            var matched = isJet == requestedJet;

            if (matched != matchAsTrue)
            {
                return false;
            }
        }

        if (!string.IsNullOrEmpty(rule.VFR) && (strip.FDR.FlightRules == "V") != matchAsTrue)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(rule.ATISRegex) && !string.IsNullOrEmpty(_bayManager.AerodromeState.ATIS))
        {
            var regex = new Regex(rule.ATISRegex);

            var res = regex.Match(_bayManager.AerodromeState.ATIS);

            if (res.Success != matchAsTrue)
            {
                return false;
            }
        }

        if (!string.IsNullOrEmpty(rule.DestinationRegex))
        {
            var regex = new Regex(rule.DestinationRegex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            var matched = regex.IsMatch(strip.FDR.DesAirport ?? string.Empty);

            if (matched != matchAsTrue)
            {
                return false;
            }
        }

        if (!string.IsNullOrEmpty(rule.TempAbove) && !string.IsNullOrEmpty(_bayManager.AerodromeState.ATIS))
        {
            var reg = _tempRegex.Match(_bayManager.AerodromeState.ATIS);

            if (!reg.Success || reg.Groups.Count != 2 || !int.TryParse(reg.Groups[1].Value, out var temp))
            {
                Util.ShowErrorBox("The temperature field was not included in the ATIS, or was invalid.");
                return false;
            }

            var minTemp = int.Parse(rule.TempAbove, CultureInfo.InvariantCulture);

            if (minTemp <= temp != matchAsTrue)
            {
                return false;
            }
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

        if (rule.ZuluTimes.Count > 0)
        {
            var matched = false;
            var curTime = int.Parse(DateTime.UtcNow.ToString("HHmm", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

            foreach (var timePair in rule.ZuluTimes)
            {
                var timeSet = timePair.Split('-');
                if (timeSet.Length != 2 || timeSet.Any(x => x.Length != 4 || !x.All(char.IsDigit)))
                {
                    Util.LogError(new ArgumentException($"Time {timePair} was invalid."));
                    continue;
                }

                var time1 = int.Parse(timeSet[0], CultureInfo.InvariantCulture);
                var time2 = int.Parse(timeSet[1], CultureInfo.InvariantCulture);

                matched |= curTime > time1 && curTime < time2;

                if (matched)
                {
                    break;
                }
            }

            if (matched != matchAsTrue)
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

    internal static string GetSIDName(Strip strip, string shortSID, string preferredRunway = "")
    {
        if (!strip.FDR.DepAirport.StartsWith("NZ", StringComparison.OrdinalIgnoreCase))
        {
            return GetLegacySIDName(strip, shortSID);
        }

        var rwys = Airspace2.GetRunways(strip.FDR.DepAirport);
        var runwayHints = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (!string.IsNullOrWhiteSpace(preferredRunway))
        {
            runwayHints.Add(preferredRunway);
        }

        if (!string.IsNullOrWhiteSpace(strip.RWY))
        {
            runwayHints.Add(strip.RWY);
        }

        var assignedRunway = strip.FDR.DepartureRunway?.Name;
        if (!string.IsNullOrWhiteSpace(assignedRunway))
        {
            runwayHints.Add(assignedRunway!);
        }

        var orderedRunways = rwys?
            .OrderByDescending(x => runwayHints.Contains(x.Name))
            .ToList() ?? [];

        var routeWaypoints = strip.FDR.ParsedRoute
            .Select(x => x.Intersection.Name)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var foundSIDs = new List<string>();

        if (shortSID.StartsWith("#", StringComparison.InvariantCulture))
        {
            var regex = new Regex(shortSID.Remove(0, 1), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            foreach (var runway in orderedRunways)
            {
                foundSIDs.AddRange(runway.SIDs
                    .Select(x => x.sidStar?.Name ?? string.Empty)
                    .Where(x => !string.IsNullOrWhiteSpace(x) && regex.IsMatch(x)));
            }
        }
        else
        {
            foreach (var runway in orderedRunways)
            {
                foundSIDs.AddRange(runway.SIDs
                    .Select(x => x.sidStar?.Name ?? string.Empty)
                    .Where(x => !string.IsNullOrWhiteSpace(x) && x.IndexOf(shortSID, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        return foundSIDs.FirstOrDefault(x => SIDNameContainsRouteWaypoint(x, routeWaypoints)) ??
            foundSIDs.FirstOrDefault() ??
            shortSID;
    }

    private static string GetLegacySIDName(Strip strip, string shortSID)
    {
        var rwys = Airspace2.GetRunways(strip.FDR.DepAirport);
        var foundSIDs = rwys?.Select(x => x.SIDs.FirstOrDefault(x => x.sidStar.Name.Contains(shortSID)));

        // Match by regex instead.
        if (shortSID.StartsWith("#", StringComparison.InvariantCulture))
        {
            var regex = new Regex(shortSID.Remove(0, 1));
            foundSIDs = rwys?.Select(x => x.SIDs.FirstOrDefault(x => regex.IsMatch(x.sidStar.Name)));
        }

        return foundSIDs?.FirstOrDefault(x => x.sidStar is not null).sidStar?.Name ?? shortSID;
    }

    private static bool SIDNameContainsRouteWaypoint(string sidName, HashSet<string> routeWaypoints)
    {
        foreach (var waypoint in routeWaypoints)
        {
            if (waypoint.Length >= 3 && sidName.IndexOf(waypoint, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    public string[] GetDepartureRunways()
    {
        if (string.IsNullOrEmpty(_bayManager.AerodromeState.ATIS))
        {
            return [];
        }

        var atis = _bayManager.AerodromeState.ATIS.ToUpperInvariant();
        var runwayLine = atis.Split('\n').FirstOrDefault(x => x.Contains("[RWY]") || x.Contains("RWY:")) ?? string.Empty;

        if (!string.IsNullOrEmpty(runwayLine))
        {
            var taggedRunways = GetTaggedRunways(runwayLine);
            if (taggedRunways.Length > 0)
            {
                return taggedRunways;
            }
        }

        var euroScopeApproachRunways = GetEuroScopeRunways(atis, _euroScopeApproachRunwayRegex);
        return euroScopeApproachRunways.Length > 0 ? euroScopeApproachRunways : [];
    }

    private string[] GetTaggedRunways(string runwayLine)
    {
        var compactRunways = GetRunwaysFromText(runwayLine);
        return compactRunways.Length > 0 ? compactRunways : GetDepartureRunways(runwayLine);
    }

    private string[] GetEuroScopeRunways(string atis, Regex regex)
    {
        var match = regex.Match(atis);
        return match.Success ? GetRunwaysFromText(match.Groups["runways"].Value) : [];
    }

    private string[] GetRunwaysFromText(string text)
    {
        var runways = new List<string>();
        foreach (Match match in _runwayTokenRegex.Matches(text ?? string.Empty))
        {
            if (!int.TryParse(match.Groups["number"].Value, out var number) || number is < 1 or > 36)
            {
                continue;
            }

            var runway = number.ToString("00", CultureInfo.InvariantCulture) + RunwaySideToLetter(match.Groups["side"].Value);
            if (!runways.Contains(runway))
            {
                runways.Add(runway);
            }
        }

        return [.. runways];
    }

    private static string RunwaySideToLetter(string side)
    {
        return side.ToUpperInvariant() switch
        {
            "LEFT" => "L",
            "RIGHT" => "R",
            "CENTRE" => "C",
            "CENTER" => "C",
            "L" or "R" or "C" => side.ToUpperInvariant(),
            _ => string.Empty,
        };
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

        return [.. runways];
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

    private static bool IsJetAircraft(Strip strip)
    {
        var aircraftTypeAndWake = strip.FDR.AircraftTypeAndWake;
        var aircraftTypeText = aircraftTypeAndWake.ToString().Trim().ToUpperInvariant();

        return Performance.GetPerformanceData(aircraftTypeAndWake)?.IsJet == true ||
            LooksLikeJetType(aircraftTypeText);
    }

    private static bool LooksLikeJetType(string aircraftType)
    {
        aircraftType = (aircraftType ?? string.Empty).Split('/').First().Trim().ToUpperInvariant();
        return Regex.IsMatch(
            aircraftType,
            @"^(A(20|21|30|31|32|33|34|35|38|3ST)|B(3[789]|7\d{2}|CS)|CRJ|E(135|145|170|175|190|195|290|295)|F28|F70|F100|MD|DC9|GLF|CL(30|35|60)|C17|C5|C25|C56X|LJ|FA|H25|PRM|HDJT)",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
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
