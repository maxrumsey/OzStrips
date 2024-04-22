namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Defines the layout types for controller strips, which are used to specify different operational roles or functionalities within the air traffic control simulation.
/// </summary>
public enum StripLayoutTypes
{
    /// <summary>
    /// Represents a general aviation strip, potentially used for both arrivals and departures without specific control assignments.
    /// </summary>
    STRIP_ACD,

    /// <summary>
    /// Represents a departure strip managed by Surface Movement Control (SMC), focusing on ground operations and initial taxi phases.
    /// </summary>
    STRIP_DEP_SMC,

    /// <summary>
    /// Represents a departure strip managed by Aerodrome Control (ADC), focusing on the control of aircraft in the vicinity of the aerodrome.
    /// </summary>
    STRIP_DEP_ADC,

    /// <summary>
    /// Represents an arrival strip managed by Aerodrome Control (ADC), focusing on the control of arriving aircraft as they approach and enter the aerodrome area.
    /// </summary>
    STRIP_ARR_ADC,

    /// <summary>
    /// Represents an arrival strip managed by Surface Movement Control (SMC), focusing on ground operations post-landing.
    /// </summary>
    STRIP_ARR_SMC,
}
