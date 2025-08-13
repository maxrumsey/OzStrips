# Changelog
## v0.6.2
### Features
- OzStrips automatically manages the list of aerodromes in the selection window, considering Position / Sectors selected.
- Added auto-open feature when moving to a tower position.

### Misc changes
- Refactored GUI supporting code out of the MainForm class. General cleanup.
- Rewrote global aerodrome state management.
- Added ability to customise bay layouts.
## v0.6.1
### Features
- When strip is picked, bay scrollbar is locked to always show the strip.
- Added QuickSearch window (Ctr+F) to search for strips.
- Pressing Alt-X again will remove a "XXX CROSS XXX" bar.
- Reenabled auto-sort of PREA strips option.

### Bugfixes
- Rewrote pick/depick code to prevent needless interaction with vatSys, and reduce complexity.
## v0.6.0
### Features
- Added Local / Circuit strip type (pink).
- Added ability to flip/flop ADEP-ADEP strips between arr, loc and dep modes.
- Added "CIRCUIT AREA RELEASED" bar.
- Inhibited strip coordination for local strips.
- Made Sweatbox Error message less annoying.
- Putting a strip into the PREA bay deactivates it, allowing changes.

### Bugfixes
- On a strip refile, data is properly refreshed.
- Disabled ALPHA sort of PREA strips pending bugfix.

### Misc Changes
- Changed how server correlates server strips with client strips.

## v0.5.7
### Features
- Added text pilot strip field.
- Added PDC indicator in place of SSR asterix.
- Pilots who disconnect and reconnect will not lose their strip.
- Right click PDC field to open PM window.
### Bugfixes
- Pilots who refile on the ground will not have old valid routing data shown.
- Fixed issue where OzStrips would scroll when clicking on form when OzStrips is not focused.

## v0.5.6
### Features
- Added warning to SSR field when an aircraft isn't squawking correct code / mode C in TAXI or superior bay.

- Changed how BLK levels are displayed.

- Pick or de-picking a strip also picks the corresponding PREA track.

- Picking a ground track will also pick the air/ normal strip (and vice-versa).

- The first waypoint will (should) always display the first waypoint where the flight planned route is commenced. (No MLs / TESATs)

- At SY and AS, SIDs with transitions will have the SID TRIGGER box be highlighted, and on hover will display the name of the transition.

### Bugfixes
- Fixed issue where when an aircraft refiles without disconnecting, UI would display the new FDR elements, but clicking on the RWY / SID field would display details from the original FDR.

## v0.5.5
### Bugfixes
- Refactored how bays display / are layed out in 1/2 col modes.

- Changed how first waypoint is displayed.
## v0.5.4
### Bugfixes
- Added fix for HDG not being removed from GLOP when removed from popup window.

### Features
- Added alert when an aircraft with a radar SID without a heading is placed in the holding point / runway / departure bay.

## v0.5.3
### Bugfixes
- Fixes issues with the route-parser.
### Features
- Preactive strips are now automatically sorted alphabetically, toggleable in settings.

## v0.5.2
### Features
- Added several WF features including highlights for aircraft with STS/STATE
- Strips become coordinated when moved into the cleared bay.
- ~~PREA strips are ordered alphabetically.~~ *Pending end of WF*

### Bugfixes
- Fixed error caused by empty routes in FDR.
- Fix issues with non-All View Modes.
- Fixed some issues with strip caching.
- Fixed issue with client not reconnecting after an extended server downtime.
- Fixed error when MainForm is accessed while disposed.

## v0.5.1
### Bugfixes
- Fixed bug caused when OzStrips closes as a connection is trying to establish.

## v0.5.0
### Features
- Completely rewrote the server & client network communication system.
- Alert comes up for a VFR aircraft with a SID.
- Improve some error handling.
- Fixed some issues with loading of bays.

## v0.4.5
### Bugfixes
- Fixed issue with SID field actions in new strip
- Temporarily disabled "View" modes as they are broken
- Fixed error caused by iterating through the list of radar tracks.

## v0.4.4
### Features
- Added ability to create strip bars
- Completely rewrote strip rendering logic
    - Reduced load time when switching between aerodromes
- Added move to next bar (ctr up/down), cross (X), Inhibit (backspace) keyboard commands.
- Improved error reporting.
- Resized some controls.
### Bugfixes
- Fixed bug where view mode is not preserved on form resize.

## v0.4.3
### Bugfixes
- Fixed leaking of winforms controls on disposal.

## v0.4.2
### Bugfixes
- Fixed race condition causing a NRE
- Fixed bug with error message reported.
- Fixed bug with error message related to network connections.
- Optimised some control creation.

## v0.4.1
### Features
- Made OzStrips responsive to changes in window size.
- Added the tiny strip.
- Added the ability to customise aerodrome list.
- Added a few keyboard commands.
- Added ability to use OzStrips on the sweatbox.
- Added the reroute menu.
- Added aircraft-types to routes.
- Added error reporting to server.

### Bugfixes
- Fixed bug with cruising altitude being above F410 and causing a NSCA alert.
- Optimised strips and moving between aerodromes.
- Fixed issue where server connection light would not update on super fast connections.

## v0.3.7
### Bugfixes
- Fix crashing issues.
- Fix strips of deleted aircraft from hanging around.

### Features
- Adds small strips.
- Add ability to use vatSys CFL/Strip/Runway dropdowns.

## v0.3.6
### Bugfixes
- Fixed bug where selecting the strip of a disconnected aircraft would cause a CTD.

## v0.3.4
### Features
- Left click on vatSys air/ground track will select relevant strip (and vice versa).
- Added error popup when a connection fails to establish.
- Added link to Changelog.
### Bugfixes
- Fixed issue where the SID primary waypoint was being inadvertently readded.
### Misc
- A large amount of backend code was rewritten.
- Majority of code was documented. (Thanks Glenn!)
- Server-side changes to caching of strips, as well as to how METAR and ATIS codes were fetched.
