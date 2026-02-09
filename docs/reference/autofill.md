# Strip Autofill
For aerodromes where an ATIS is available, and an autofill config file is available, strip autofilling is available. When strip autofilling is available, the autofill element within the menu bar will display with a green outline.

To autofill a strip, pick the strip, (click on the callsign) and press "A". The RWY, SID, CFL and appropriate departure frequency will be inserted automatically. If a RWY has already been entered for that aircraft, the plugin will autofill the SID, CFL and departure frequency only. This is appropriate for situations where an aircraft requires a non-duty, or non-default runway.

A heirarchy of departure frequencies are defined, and the first "primed" frequency is selected.

e.g: At Melbourne, RWY 27 is nominated for departures. MAE and ELW are online. By default, a jet outbound via DOSEL will receive
1. Runway 27.
2. A procedural SID terminating at DOSEL.
3. A CFL of 5000 feet.
4. The first appropriate departure frequency online. (MAE)

If that aircraft were to then require Runway 16, the controller would enter that runway, and re-attempt an autofill. In this case, the `ISPEG X` SID would be selected.

## Data Format
To enable autofill at an aerodrome, create a `.yml` file titled by the aerodrome's ICAO code.

### Departure Runway
The departure runway for a strip can be automatically nominated based on the runway mode in the ATIS with:

``` yaml
    - assign_dep_rwy: "true"
```

Otherwise, the departure runway can be manually assigned with:

``` yaml
    - runway: "16R"
```

### CFL
The CFL can be set with:

``` yaml
    - CFL: "030"
```

### SID
The SID can be set with:

``` yaml
    - SID: "MARUB"
```

This will match any procedure titled `MARUBx` where the `x` represents the current chart number (e.g. `MARUB7`).

To target a procedure title suffixed with a particular character (e.g. `FLIKI2B`), use:

``` yaml
    - SID: "#FLIKI\\dB"
```

!!! note
    The `#` denotes a regex string, while the `\\d` denotes a single letter. The final `B` indicates the actual letter to match.

### Departure Frequency
A list of departure frequencies can be provided, which the plugin will try to match based on the order in which they are provided. The first frequency (from left to right) which is currently primed by a controller within range will be selected.

``` yaml
    - departures: ["129.7", "123.0", "124.4", "128.3", "126.1", "125.3", "124.55", "129.8", "133.15"]
```

### Conditionals
Complex logic can be created to assign any of the above-mentioned items based on a set of conditions. 

Available conditional elements include:

| Element | Meaning |
| --- | --- |
| `jet: "true"` | Aircraft is a jet aircraft |
| `runway: ["16R"]` | Aircraft's assigned departure runway is 16R |
| `VFR: "true"` | Aircraft's FDR is filed VFR |
| `WTC: ["H", "J"]` | Wake turbulence category matches either `H` or `J` (options include `L`, `M`, `H`, `J`) |
| `atis_dep_rwy: ["34L", "16L"]` | Current runway mode on the ATIS references both `34L` and `16L` as departure runways |
| `radials: ["241-360", "0-066"]` | Track from aerodrome to first non-SID, non-lat/long waypoint falls between the nominated radials (*note the passage of North*) |
| `waypoint: ["OLSEM", "BANDA"]` | Aircraft's FDR route includes either `OLSEM` or `BANDA` |

!!! tip
    To test the inverse of a `true` statement, use `if_not:`.

Indentation is important. Each conditional statement should be indented by 4 spaces from the `- if:` statement. The 'true' block should be indented 2 spaces.

!!! example
    ``` yaml
        - if:
            runway: ["14R"]
            waypoint: ["NUNPA"]
          SID: "#NUNP\\dB"
    ```

### Example File
In the example below, VFR aircraft are assigned 4,500ft while IFR aircraft are assigned 8,000ft. The departure runway on the ATIS is assigned to all aircraft and the default SID is used once the runway has been nominated, except for departures from Runway 14R to FLIKI, VIMAP, NUNPA or MOTRA. The departure frequency first tries to match the overlying TCU controller, then tries the overlying Enroute controller.

``` yaml
    - assign_dep_rwy: "true"
    - CFL: "080"
    - if:
        VFR: "true"
      CFL: "045"

    - if:
        runway: ["14R"]
        waypoint: ["FLIKI"]
      SID: "#FLIK\\dB"
    - if:
        runway: ["14R"]
        waypoint: ["VIMAP"]
      SID: "#VIMA\\dB"
    - if:
        runway: ["14R"]
        waypoint: ["NUNPA"]
      SID: "#NUNP\\dB"
    - if:
        runway: ["14R"]
        waypoint: ["MOTRA"]
      SID: "#MOTR\\dB"
    
    - departures: ["123.8", "122.6"]
```