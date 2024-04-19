# Flight Thread
## Departure
### ACD
<figure markdown="span">
  ![Arrival Strip](./images/acdsmc_strip.png)
  <figcaption>ACD and SMC Strip</figcaption>
</figure>
On filing a flight plan and logging in, the strip would appear in the Preactive Bay. The ACD would assign the SID, RWY, Initial Altitude and potentially Heading by left clicking on the Alt/Hdg Field.

Left clicking on the squawk would assign a squawk code, and left clicking on the destination would allow the full route and requested flight level to be checked.

Everything you would need to give a clearance is visible in the Strip, and once this clearance has been given, left click on the SID to mark the strip as "Cleared".

Alternatively, you could "Pick" the strip, (Left click on the Callsign), and press "PDC" in the Control Bar to send a vatSYS PDC. It is recommended once you send the PDC to cock the strip, (Left click on the ETD), as a reminder that the PDC has been sent but not read back.

### SMC
<figure markdown="span">
  ![Arrival Strip](./images/acdsmc_strip.png)
  <figcaption>ACD and SMC Strip</figcaption>
</figure>

When the aircraft requests pushback, SID trigger the aircraft into the pushback bay, making sure to enter the GATE number into the GATE field. (Left click on this field to enter it).
You can enter the pushback location / direction into the CLX field.

When the aircraft requests taxi, issue taxi instructions and place the hold short / holding point location into CLX field. If the aircraft is clear to cross a runway, you may enter "x###" into the remarks field, with ### being the runway number. eg: "x16L".

If the aircraft is holding short of a runway, waiting for ADC to approve the cross: pick the strip, press "XX CROSS XX" in the control bar, and place the strip into the "Holding Point" bay. This serves as a memory prompt to the ADC that an aircraft needs to cross the runway.

Finally, upon reaching the point where they'd be handed over to tower, trigger the strip into the holding point bay. If you explicitly issue the "MONITOR TOWER" instruction, move the strip into that bay as you issue it, otherwise you could forget. 

### ADC
<figure markdown="span">
  ![Arrival Strip](./images/acdsmc_strip.png)
  <figcaption>ACD and SMC Strip</figcaption>
</figure>

When you see the strip in the holding point bay, you can order the departure sequence by left clicking the aircraft callsign and using your arrow keys.

When you instruct the aircraft to line up, place the strip into the runway bay.

As you issue take off clearance, press the "00:00" in the strip to activate the timer. 

When you instruct the aircraft to contact departures, SID trigger the strip into the departures bay. When you no longer need the strip there to assess for wake turbulence separation, finally SID trigger the strip to hide it.