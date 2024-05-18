OzStrips is a plugin for vatSys, that emulates a tower's electronic strips system.

At its core, it provides an easy to use and intuitive user interface; supporting controlling a single aerodrome position, to providing top-down control to all positions.
It's design and use case centres around a controller providing top-down services to a single aerodrome; while multi-aerodrome use is possible, it is not necessarily supported.

---
![Example Usage](images/fullwindow.png)

## Features
### Shared state
Changes made to your strips or layout will be replicated to other controllers using OzStrips. 

This allows greater situational awareness of aerodrome operations when operating with SMC, ADC and ACD split. Reliably project future workload and ease coordination.

In case of disconnection, your layout and aircraft data is automatically synced, allowing you to hit the ground running.
### Workload management
Easily "queue up" strips when aircraft request pushback, clearance or taxi. The position in queue can be readily gained, keeping pilots informed of expected delay, and ensuring no one is missed out.

### vatSys Integration
With one click, access an aircraft's Flight Plan Window, send a PDC or set the CFL.

Changes made to an aircraft's Runway, SID or CFL will automatically update the relevant vatSys Flight Data Record. HDG changes will be saved to the aircraft's Global Ops strip field, allowing easy access by Approach Controllers.

## Disclaimer
OzStrips is not associated with [VATPAC](https://vatpac.org/), [VATSIM](https://vatsim.net/) or [vatSys](https://virtualairtrafficsystem.com/).

If it is not obvious, use of OzStrips as a controller providing aerodrome services is completely optionable. Keep in mind that OzStrips is still in development, and may not work perfectly 100% of the time.
If you are not familiar with it's use, or run into problems, ensure your priority is to provide quality ATS services. 

Connection as an OBS to test out the plugin is recommended, fully supported, and your changes will not have any effect on live operations.
