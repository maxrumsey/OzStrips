namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Defines the types of items that can be associated with a controller strip in air traffic control simulations.
/// </summary>
public enum StripItemType
{
    /// <summary>
    /// Represents a standard controller strip, used for managing individual flights.
    /// </summary>
    STRIP,

    /// <summary>
    /// Represents a queue bar, used to visually separate or organize strips into manageable sections.
    /// </summary>
    QUEUEBAR,

    /// <summary>
    /// Indicates that the strip or section is blocked and cannot be used currently.
    /// </summary>
    BAR,
}
