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