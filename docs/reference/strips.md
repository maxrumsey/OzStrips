# Strip
<figure markdown="span">
  ![Strip](../images/strip.png)
  <figcaption>A departure strip</figcaption>
</figure>

## Strip Colours
Departure strips are blue, while arrival strips are yellow.

## Strip Posting
You will only see strips for aircraft that have requested planned an ADES or ADEP of your aerodrome. Aircraft doing circuit work at your aerodrome, but not arriving or departing there will not receive a strip.

## Strip Layout
![Strip Reference](../images/strip_reference.png)

| Element | Description | Click Action | Possible Alert |
|---------|-------------|--------------|----------------|
| 1 | Bay Number | Change |
| 2 | Filed Off Blocks Time | Cock Strip |
| 3 | Aircraft Type | (L) Open flightplan (R) Open reroute menu |
| 4 | Destination | (L) Open flightplan (R) Open reroute menu |
| 5 | Route Button | Show route on vatSys ASD |
| 6 | Flight Rules | |
| 7 | Correct SSR Code + Mode C Received | |
| 8 | SSR Code | Generate Code |
| 9 | Wake Turbulence Category | |
| 10 | Callsign | Pick / Select Strip |
| 11| Runway | Change |
| 12 | Holding Point / Clearance Limit | Change |
| 13 | SID | (L) Move strip to next bay \* (R) Change SID |
| 14 | First Waypoint | (L) Open flightplan (R) Open reroute menu | Potentially non-compliant route filed |
| 15 | Departure Heading | Change |
| 16 | Requested Level | Open flightplan |
| 17 | Cleared Level | Change | Non-standard cruising level |
| 18 | Takeoff Timer | Start / Reset # |
| 19 | Global Ops Data ^ | Change |
| 20 | Local Remarks ^ | Change |

\* Won't automatically move bay into runway, this must be done manually.

\# Will also coordinate (activate, make blue) the strip if not already done via moving into Pushback or later bay.

^ Global Ops are visible to all controllers. Local Remarks are only visible to OzStrips users.

## Alerts
### Non-standard Cruising Altitude
Indicates a pilot had filed a level that is non-compliant with the table of standard cruising levels per the Australian AIP.
### Non-compliant Route Filed
Indicates a pilot has filed a route that is contrary to the list of routes in the Australian ERSA Flight Planning Requirements document.

It is at the controller's discretion whether or not the aircraft is recleared along a new route. Take into account possible workload and loss of strategic separation due to the non-compliant route, effect on TCU operations and ability of the pilot to reenter a new route.