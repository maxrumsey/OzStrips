using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// Contains various helper methods.
/// </summary>
public static class Util
{
    private static ILogger? _logger;

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
        _logger ??= Telemetry.LoggerFactory.CreateLogger("Util");

        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }

        Errors.Add(error, source);

        _logger.LogError(error, "An error occurred in {Source}", source);
    }

    /// <summary>
    /// Logs an error to vatsys, and posts it to the server.
    /// </summary>
    /// <param name="text">Text to log.</param>
    public static async void LogText(string text)
    {
        _logger ??= Telemetry.LoggerFactory.CreateLogger("Util");
        _logger.LogWarning(text);
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
