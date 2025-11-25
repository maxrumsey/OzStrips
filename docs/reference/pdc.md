# PDCs
PDCs through Hoppies are available at approved PDC aerodromes, for users on version 0.8.0 or later. Within VATPAC, these are the approved PDC aerodromes:

`YMML, YSSY, YPAD, YPPH, YBBN, YSCB, YPDN, YBCG, YBCS, YBTL, YWLM`

The Hoppies connection is maintained automatically by the OzStrips server, no configuration or setup is required by the end user. Additionally, PDC requests and responses for aircraft are synced between controllers at the same aerodrome. The server polls Hoppies roughly every minute for aerdromes with controllers online, and every 2 minutes for aerodromes without controllers online.

When a PDC request is received at an aerodrome, and the callsign requesting the PDC is online at that aerodrome, a notification is distributed to online controllers. (Unless disabled), a notification sound is played, and the `Requested PDCs` elements in the menu bar is highlighted. The PDC element, within the aircraft's strip, will flash yellow and white until it is acknowledged.

Clicking on the PDC strip element opens the PDC window. For strips that are 'complete', (have a RWY, SID, CFL, Departure Frequency nominated), no modification of the premade PDC is required, except for defining the appropriate Departure Frequency, if required. Otherwise, a red error label will display indicating that not all required information has been entered into the Strip / FDR.

If all information has been entered, clicking `Send` will distribute the PDC to the Hoppies network.

## Controllers Offline
If an OzStrips equipped controller is not online and active at that aerodrome, a response will automatically be sent to the pilot indicating such.

## PDC Monitor
For enroute and approach controllers top-downing, a window can be opened using the `Tools` dropdown, which displays aerodromes with outstanding PDC requests. Only aerodromes within your selected sectors and subsectors will be highlighted.