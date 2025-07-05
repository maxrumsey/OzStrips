using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
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
    /// Creates a question box with a specified message and returns the result.
    /// </summary>
    /// <param name="message">Message to show.</param>
    /// <returns>Question Result.</returns>
    public static DialogResult ShowQuestionBox(string message)
    {
        return MessageBox.Show(Form.ActiveForm, message, "OzStrips", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            using var client = new HttpClient();
            var data = new Dictionary<string, string>
                {
                    { "error", "ERROR: " + error.Message + "\n" + error.StackTrace },
                    { "version", OzStripsConfig.version },
                };
            var uri = (OzStripsConfig.socketioaddr + "Crash").Replace("//", "/").Replace(":/", "://");
            var task = client.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json"));
            _ = await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Errors.Add(ex, "OzStrips Error Reporter");
        }
    }

    /// <summary>
    /// Logs an error to vatsys, and posts it to the server.
    /// </summary>
    /// <param name="text">Text to log.</param>
    public static async void LogText(string text)
    {
        try
        {
            using var client = new HttpClient();
            var data = new Dictionary<string, string>
                {
                    { "error", "DIAG: " + text },
                };
            var uri = (OzStripsConfig.socketioaddr + "/crash").Replace("//", "/").Replace(":/", "://");
            var task = client.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json"));
            _ = await task.ConfigureAwait(false);
        }
        catch
        {
            return;
        }
    }

    /// <summary>
    /// Sets and saves an environment variable.
    /// </summary>
    /// <param name="name">Variable name.</param>
    /// <param name="value">Variable value.</param>
    public static void SetEnvVar(string name, object value)
    {
        Properties.OzStripsSettings.Default[name] = value;
        Properties.OzStripsSettings.Default.Save();
    }

    /// <summary>
    /// Gets a dictionary containing aerodrome waypoints that differ from the normal format.
    /// </summary>
    public static readonly Dictionary<string, string> DifferingAerodromeWaypoints = new()
    {
        { "YSSY", "TESAT" },
        { "YBCG", "GOMOL" },
        { "YBHM", "OVRON" },
        { "YMEN", "ESDAN" },
        { "YMHB", "TASUM" },
        { "YSNW", "NWA" },
        { "YPED", "DOLVU" },
    };
}
