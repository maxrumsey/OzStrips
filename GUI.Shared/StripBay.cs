namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Defines the various statuses of a strip bay in OzStrips.
/// </summary>
public enum StripBay
{
    /// <summary>
    /// Indicates the strip is not currently active (inhibited).
    /// </summary>
    BAY_DEAD,

    /// <summary>
    /// Indicates preactive strip state.
    /// </summary>
    BAY_PREA,

    /// <summary>
    /// Indicates the strip has received a clearance.
    /// </summary>
    BAY_CLEARED,

    /// <summary>
    /// Indicates the strip is cleared for pushback.
    /// </summary>
    BAY_PUSHED,

    /// <summary>
    /// Indicates the aircraft is cleared for taxi.
    /// </summary>
    BAY_TAXI,

    /// <summary>
    /// Indicates the aircraft is holding short of the runway.
    /// </summary>
    BAY_HOLDSHORT,

    /// <summary>
    /// Indicates the aircraft is cleared to enter/cross/use the runway.
    /// </summary>
    BAY_RUNWAY,

    /// <summary>
    /// Indicates the aircraft has departed.
    /// </summary>
    BAY_OUT,

    /// <summary>
    /// Indicates the strip for an arriving aircraft, pending a landing clearance.
    /// </summary>
    BAY_ARRIVAL,

    /// <summary>
    /// Indicates the strip for an aircraft that is in the circuit.
    /// </summary>
    BAY_CIRCUIT,
}
