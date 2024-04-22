namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Defines the types of air traffic operations for controller strips, distinguishing between arrivals, departures, and unspecified types.
/// </summary>
public enum StripArrDepType
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
    /// Indicates an unknown or unspecified operation type, used when the operation type cannot be determined.
    /// </summary>
    UNKNOWN,
}
