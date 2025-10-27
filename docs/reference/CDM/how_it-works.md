During events, or when traffic flow management procedures are warranted, CDM mode can be enabled for the aerodrome. A CDM Processor at each aerodrome keeps track of which stage of departure each aircraft is in. Active and Pushed aircraft are included within the **Departure Queue**, and will have a TSAT and CTOT time calculated for each aircraft.

The departure queue (as well as a range of statistics) is available on the [Ops Dashboard](https://cdm.maxrumsey.xyz/ops){target=new}.

## CDM Stages
| Name | OzStrips Bay | CDM Processor Action |
| ---- | ------------ | -------------------- |
| Preactive | Preactive or Cleared Bay above the bar | *Not included in CDM calculations* |
| Active | Cleared Bay, below the queue bar | Included within the departure queue, and will be issued a TSAT and CTOT |
| Pushed | Pushback, Taxi, Holding Point or Runway Bay | Included in the departure queue below any Active aircraft |
| Complete | Departed Bay, or GS > 50kts | Departure time is logged, *not included in CDM calculations* |

If an aircraft disconnects, or their strip is moved into the **Preactive Bay** or **Cleared Bay** above the bar, their CDM status will expire after 5 minutes. 

## CDM Abbreviations
| Abbreviation | Full Name | Explanation | Example |
| ------------ | --------- | ----------- | ------- |
| EOBT | Estimated Off Blocks Time | The off blocks time a pilot submits within their flight plan. This is not used for CDM calculations. | 10:00z |
| TOBT | Tactical Off Blocks Time | The time at which the pilot requests pushback. | 10:20z |
| TSAT | Tactical Start Approved Time | The time the CDM processor allots for pushback approval, taking into account CDM parameters and other aircraft. | 10:30z |
| AOBT | Actual Off Blocks Time | The time at which the aircraft actually pushes back. This should be as close to the final TSAT as possible | 10:31z |
| CTOT | Calculated Take Off TIme | The time the CDM system allots for aircraft departure. | 10:45z |
| ATOT | Actual Take Off Time | The time at which the aircraft actually departs. | 10:46z |

## Pushback Process
Pushback should only be issued when the aircraft is compliant with their TSAT. 

| Colour | Condition | Explanation |
| --- | --- | --- |
| Grey | In Queue | The aircraft has been placed in the queue but is not yet compliant with their TSAT. *Do not issue pushback*. |
| Green | Compliant with TSAT | The aircraft is compliant with their TSAT. Pushback may be issued **if aerodrome congestion allows**. |
| Yellow | Pushed Back | Aircraft has already pushed back, *no further action required*. |


## CDM Processing
CDM Processing refers to the process by which the departure order, TSAT and CTOT times are calculated for each aircraft. This occurs regularly at CDM enabled aerodromes, and after every CDM-relevant aircraft state change.

The CDM Processor takes the list of active and pushed aircraft, as well as the set **Departure Rate**, determines a priority sorted list of aircraft, and allocates a **CTOT** to each aircraft. **CTOT** times will be spaced by the set **Departure Rate**. A **TSAT** is calculated for each aircraft, based on the **CTOT**. **TSAT** times take into account the aircraft state, such that an aircraft will not be expected to pushback and depart within 5 minutes. **CTOT** and **TSAT** times are recalculated during each CDM Processing cycle, and can vary from the initial **TSAT** issued.

e.g: A planned **Departure Rate** of 30/hour is set, but the ADC controller actually maintains a rate of 40/hour. **AOBT** and **ATOT** will be earlier than the initial calculated **TSAT** and **CTOT**.

## Departure Monitoring
The **Departure Rate** is set based on conditions at the aerodrome, the amount of departures, the amount of arrivals, and amount of arrivals the destination aerodromes can accept. For the A-CDM system to work effectively, ADC must monitor the amount of depatures they release, and ensure this actual rate is as close to the set departure rate as possible.

To make this task easier, OzStrips provides a Departure Monitor screen above the **Runway Bay** when CDM is enabled at an aerodrome. The **Departure Rate** over the preceeding 5, 15 and 30 minutes are recorded, and presented to plugin users. Each interval is colour coded to quickly show the relative performance vs planned performance.
