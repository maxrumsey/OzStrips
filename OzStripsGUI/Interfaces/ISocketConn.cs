using System;
using System.Collections.Generic;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// ISocketConn.
/// </summary>
public interface ISocketConn : IDisposable
{
    /// <summary>
    /// Available server types.
    /// </summary>
    public enum Servers
    {
        /// <summary>
        /// Default connection.
        /// </summary>
        VATSIM,

        /// <summary>
        /// Sweatbox 1.
        /// </summary>
        SWEATBOX1,

        /// <summary>
        /// Sweatbox 2.
        /// </summary>
        SWEATBOX2,

        /// <summary>
        /// Sweatbox 3.
        /// </summary>
        SWEATBOX3,
    }

    /// <summary>
    /// Gets the messages, used for debugging.
    /// </summary>
    public List<string> Messages { get; }

    /// <summary>
    /// Gets or sets the current server type.
    /// </summary>
    public Servers Server { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the client is connected.
    /// </summary>
    public bool Connected { get; set; }

    /// <summary>
    /// Syncs the strip controller.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public void SyncSC(StripController sc);

    /// <summary>
    /// Requests routes for a given sc.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public void RequestRoutes(StripController sc);

    /// <summary>
    /// Requests bay order data from server.
    /// </summary>
    public void ReadyForBayData();

    /// <summary>
    /// Syncs the deletion of a controller.
    /// </summary>
    /// <param name="sc">The strip controller.</param>
    public void SyncDeletion(StripController sc);

    /// <summary>
    /// Sync the bay to the socket.
    /// </summary>
    /// <param name="bay">The bay to sync.</param>
    public void SyncBay(Bay bay);

    /// <summary>
    /// Sets the aerodrome based on the bay manager.
    /// </summary>
    public void SetAerodrome();

    /// <summary>
    /// Sets the server type.
    /// </summary>
    /// <param name="type">Server connection type.</param>
    public void SetServerType(Servers type);

    /// <summary>
    /// Sends the cache to the server.
    /// </summary>
    public void SendCache();

    /// <summary>
    /// Disconnects from the server.
    /// </summary>
    public void Close();

    /// <summary>
    /// Starts a fifteen second timer, ensures FDRs have loaded in before requesting SCs from server.
    /// </summary>
    public void Connect();

    /// <summary>
    /// Disconnects the io.
    /// </summary>
    public void Disconnect();
}
