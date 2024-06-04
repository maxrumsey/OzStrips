# Changelog

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