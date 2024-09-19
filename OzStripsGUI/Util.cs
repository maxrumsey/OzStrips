using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SocketIOClient.Transport.Http;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Contains various helper methods.
/// </summary>
public static class Util
{
    /// <summary>
    /// Creates an error box with a specified message.
    /// </summary>
    /// <param name="message">Message to display.</param>
    public static void ShowErrorBox(string message)
    {
        MessageBox.Show(Form.ActiveForm, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Creates an info box with a specified message.
    /// </summary>
    /// <param name="message">Message to display.</param>
    public static void ShowInfoBox(string message)
    {
        MessageBox.Show(Form.ActiveForm, message, "OzStrips", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    /// Creates an warning box with a specified message.
    /// </summary>
    /// <param name="message">Message to display.</param>
    public static void ShowWarnBox(string message)
    {
        MessageBox.Show(Form.ActiveForm, message, "OzStrips", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Creates an env var for ozstrips DLLs and returns the path.
    /// </summary>
    /// <returns>Path where dlls will be copied to.</returns>
    public static string SetAndReturnDLLVar()
    {
        var appdata_path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\ozstrips\";
        Environment.SetEnvironmentVariable("path", Environment.GetEnvironmentVariable("path") + ";" + appdata_path);
        return appdata_path;
    }

    /// <summary>
    /// Logs an error to vatsys, and posts it to the server.
    /// </summary>
    /// <param name="error">Exception.</param>
    /// <param name="source">Source string.</param>
    public static async void LogError(Exception error, string source = "OzStrips")
    {
        Errors.Add(error, source);

        try
        {
            using (var client = new HttpClient())
            {
                var data = new Dictionary<string, string>
                {
                    { "error", "ERROR: " + error.Message + "\n" + error.StackTrace },
                };
                var uri = (OzStripsConfig.socketioaddr + "/crash").Replace("//", "/").Replace(":/", "://");
                var task = client.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json"));
                _ = await task.ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            Errors.Add(ex, "OzStrips Error Reporter");
        }
    }
}
