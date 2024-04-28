namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Defines the various statuses of a strip bay in air traffic control simulation.
/// </summary>
public enum StripBay
{
    /// <summary>
    /// Indicates the strip is not currently active.
    /// </summary>
    BAY_DEAD,

    /// <summary>
    /// Indicates preparation status before activation.
    /// </summary>
    BAY_PREA,

    /// <summary>
    /// Indicates the strip has been cleared for the next stage.
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
    /// Indicates the aircraft is on the runway.
    /// </summary>
    BAY_RUNWAY,

    /// <summary>
    /// Indicates the aircraft has exited the runway.
    /// </summary>
    BAY_OUT,

    /// <summary>
    /// Indicates the strip for an arriving aircraft.
    /// </summary>
    BAY_ARRIVAL,
}