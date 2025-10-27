A number of CDM options can be configured by suitably authorised users. These settings are visible to all site users, although modification is only available to authorised users. 
## Config
This page contains config changes for a specific ICAO code on the network. Config changes are synced across all server types. e.g: A change on YSSY Sweatbox 2, will also change options for YSSY Sweatbox 1.

**Disable plugin changes of Departure Rate:** Prevents plugin users from changing the Departure Rate. This may be utilised to ensure conformity with a flow plan.

**Destination aerodromes requiring slots:** This sets the list of aerodromes where a departure slot is required to prevent the `NO SLOT` warning.

**Aerodromes with limited departure rates:** This sets aerodromes with limited departure rates. e.g: During a milkrun, YMML departures may be limited to 24/hour, but departures to other aerodromes may be unrestricted.

The sum of individual departure rates will not exceed the overall plugin departure rate.

e.g: YSSY has an overall departure rate of 60/hour. Aircraft with an ADES of YMML are limited to 24/hour. The total departure rate of the aerodrome will exceed 60/hour.

## Whitelisted Departures
This contains the list of aircraft with departure slots. (e.g: for Worldflight). The list will only contain aircraft with an ADEP of the current selected aerodrome.