# Flight Thread

!!! tip
    It is highly recommended that this guide is read in conjunction with VATPAC's [OzStrips Guide](https://sops.vatpac.org/client/towerstrips/), if you are a member of their division.

!!! note
    Bay names may vary based on type of aerodrome, division, etc. The information below is typical for an Australian radar tower.

## Departure
### ACD

On filing a flight plan and logging in, the strip will appear in the Preactive Bay. The ACD assigns the SID, RWY, and CFL by left clicking on the RWY or CFL box, and right clicking on the SID (green) box to change the SID if required.

Left clicking on the squawk will assign a squawk code, and left clicking on the route indicator field will display the full route on the vatSys ASD.

Everything you would need to give a clearance is visible in the Strip, and once this clearance has been given, left click on the SID to mark the strip as "Cleared". This will automatically move the strip into the Cleared bay. You may alternatively also "pick" the strip by clicking on the callsign, and click on the grey background of the Cleared bay to manually move it.

You can also press the PDC field (left of the SSR code) to send a vatSys PDC, or a [Hoppies PDC](./reference/pdc.md) if requested.

### SMC
When the aircraft requests and is granted permisison to pushback, move the strip into the pushback bay, making sure to enter the GATE number into the GATE field. (Top left box).
You can enter the pushback location / direction / disconnect point into the HOLDING POINT field. (Box right of the runway).

When the aircraft requests taxi, issue taxi instructions and place the hold short / holding point location into the HOLDING POINT field. If the aircraft is clear to cross a runway, you can toggle the crossing highlight on the strip by pressing the X key, with the aircraft picked.

Finally, upon reaching the point where they'd be handed over to tower, trigger the strip into the holding point bay. If you explicitly issue the "MONITOR TOWER" instruction, move the strip into that bay as you issue it.

!!! warning
    If the strip isn't moved into the Holding Point bay, tower may not be able to see it.

### ADC
When you see the strip in the holding point bay, you can order the departure sequence by left clicking the aircraft callsign and using your arrow keys. You can also pick the strip, and use Control+Click on a different strip to move the picked strip above/below that strip.

When you instruct the aircraft to line up, place the strip into the runway bay. You will need to do this manually, SID triggering the strip, (clicking the green box), will not work.

As you issue take off clearance, you can press the timer, ("00:00"), in the strip to activate the departure timer. 

When you instruct the aircraft to contact departures, SID trigger the strip into the departures bay. When you no longer need the strip there to assess for wake turbulence separation or situational awareness, finally SID trigger the strip to hide it. You can also pick the strip and press backspace to hide it.

If you accidentally hide the strip, left click on the aircraft's ground radar target, air track, or their vatSys strip and click into the Runway bay.