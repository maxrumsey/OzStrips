using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;

namespace MaxRumsey.OzStripsPlugin;

internal class Updater : IDisposable
{
    private readonly string _docPluginsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "vatSys Files\\Profiles\\Australia\\Plugins");
    private readonly string _programFilesPluginFolder = Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), "vatSys\\bin\\Plugins");
    private readonly string _ozStripsDir;
    private readonly string _installerURL = "http://localhost:8000/installer.exe"; // example
    private readonly string _ozstripsDownloadURL = "<plugin.zip>"; // example
    private readonly WebClient _webClient = new WebClient();

    internal Updater()
    {
        _ozStripsDir = GetPluginDirectory();

        if (string.IsNullOrEmpty(_ozStripsDir))
        {
            Errors.Add(new("Unable to find plugin directory!"), "OzStrips Updater");
            return;
        }
    }

    public async void Update()
    {
        await CreateUpdaterExe();
        if (File.Exists(Path.Combine(GetTempDir(), "installer.exe")))
        {
            var startInfo = new ProcessStartInfo(Path.Combine(GetTempDir(), "installer.exe"), _ozstripsDownloadURL + " \"" + _docPluginsFolder + "\"");
            startInfo.UseShellExecute = true;
            Process.Start(startInfo);
            Process.GetCurrentProcess().Kill();
        }
        else
        {
            Errors.Add(new("Failed to install the updater."), "OzStrips Updater");
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    private static string GetTempDir()
    {
        var basetempdir = System.IO.Path.GetTempPath();

        if (!Directory.Exists(Path.Combine(basetempdir, "ozstrips")))
        {
            Directory.CreateDirectory(Path.Combine(basetempdir, "ozstrips"));
        }

        return Path.Combine(basetempdir, "ozstrips");
    }

    private static bool DoesUpdaterExeExist()
    {
        var ozstripsTempDir = GetTempDir();
        var installerEXE = Path.Combine(ozstripsTempDir, "installer.exe");
        return File.Exists(installerEXE);
    }

    private string GetPluginDirectory()
    {
        if (Directory.Exists(Path.Combine(_docPluginsFolder, "OzStrips")))
        {
            return Path.Combine(_docPluginsFolder, "OzStrips");
        }
        else if (Directory.Exists(Path.Combine(_programFilesPluginFolder, "OzStrips")))
        {
            Errors.Add(new("Unable to use OzStrips Updater while OzStrips is installed in ProgramFilesx86"), "OzStrips");
            return string.Empty;
        }
        else
        {
            return string.Empty;
        }
    }

    private async Task CreateUpdaterExe()
    {
        var ozstripsTempDir = GetTempDir();
        var installerEXE = Path.Combine(ozstripsTempDir, "installer.exe");
        if (DoesUpdaterExeExist())
        {
            File.Delete(installerEXE);
        }

        await _webClient.DownloadFileTaskAsync(_installerURL, installerEXE);
    }
}
