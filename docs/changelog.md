# Changelog
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