namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Defines the types of air traffic operations for controller strips, distinguishing between arrivals, departures, and unspecified types.
/// </summary>
public enum StripType
{
    /// <summary>
    /// Represents an arriving aircraft. Used for managing incoming flights to an airport.
    /// </summary>
    ARRIVAL,

    /// <summary>
    /// Represents a departing aircraft. Used for managing outgoing flights from an airport.
    /// </summary>
    DEPARTURE,

    /// <summary>
    /// Indicates a strip arriving and departing from the same aerodrme.
    /// </summary>
    LOCAL,

    /// <summary>
    /// Indicates a strip status of nil / not relevant.
    /// </summary>
    UNKNOWN = -1,
}
