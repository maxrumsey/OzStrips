using MaxRumsey.OzStripsPlugin.GUI.Shared;

namespace GUI.Connector;

/// <summary>
/// Implemented by the OzStrips MainForm.
/// </summary>
public interface IStripsWindow
{
    /// <summary>
    /// Gets a strip DTO by callsign.
    /// </summary>
    /// <param name="callsign">Callsign.</param>
    /// <returns>DTO, if found.</returns>
    public StripDTO? GetDTO(string callsign);

    /// <summary>
    /// Determines whether or not the selected strip is in a queue.
    /// </summary>
    /// <param name="key">Strip key.</param>
    /// <returns>In queue.</returns>
    public bool InQueue(StripKey key);

    /// <summary>
    /// Gets a CDMResult by callsign.
    /// </summary>
    /// <param name="key">Strip key.</param>
    /// <returns>CDM Result, if found.</returns>
    public CDMResultDTO? GetCDMResult(StripKey key);
}
