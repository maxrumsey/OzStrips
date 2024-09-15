using System;
using System.Windows.Forms;

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
}
