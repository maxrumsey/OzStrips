using maxrumsey.ozstrips.gui;
using Newtonsoft.Json;
using System;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;
using vatsys.Plugin;

namespace maxrumsey.ozstrips
{

    [Export(typeof(IPlugin))]
    public class OzStrips : IPlugin
    {
        private CustomToolStripMenuItem ozStripsOpener;
        private MainForm GUI;

        public static HttpClient _httpClient = new HttpClient();
        private static readonly Version _version = new Version(1, 0);
        private static readonly string _versionUrl = "https://raw.githubusercontent.com/maxrumsey/OzStrips/master/Version.json";

        public OzStrips()
        {
            Network.Connected += Connected;
            Network.Disconnected += Disconnected;
            ozStripsOpener = new CustomToolStripMenuItem(CustomToolStripMenuItemWindowType.Main, CustomToolStripMenuItemCategory.Windows, new ToolStripMenuItem("OzStrips"));
            ozStripsOpener.Item.Click += OpenGUI;
            MMI.AddCustomMenuItem(ozStripsOpener);

            _ = CheckVersion();
        }

        private void Connected(object sender, EventArgs e)
        {
            if (GUI != null && GUI.IsHandleCreated) GUI.Invoke((System.Windows.Forms.MethodInvoker)delegate () { GUI.ConnectVATSIM(); });
        }
        private void Disconnected(object sender, EventArgs e)
        {
            if (GUI != null && GUI.IsHandleCreated) GUI.Invoke((System.Windows.Forms.MethodInvoker)delegate () { GUI.DisconnectVATSIM(); });
        }

        private static async Task CheckVersion()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_versionUrl);

                var version = JsonConvert.DeserializeObject<Version>(response);

                if (version.Major == _version.Major && version.Minor == _version.Minor) return;

                Errors.Add(new Exception("A new version of the plugin is available."), "OzStrips Connector");
            }
            catch { }
        }

        /// Plugin Name
        public string Name { get => "OzStrips Connector"; }

        public void OnFDRUpdate(FDP2.FDR updated)
        {
            //Errors.Add(new Exception("mew"));
            //System.Diagnostics.Process.Start("http://google.com");
            if (GUI != null && GUI.IsHandleCreated) GUI.Invoke((System.Windows.Forms.MethodInvoker)delegate () { GUI.UpdateFDR(updated); });

        }

        /// Not needed for this plugin. But you could for instance, use the new position of the radar track or its change in state (cancelled, etc.) to do some processing. 
        public void OnRadarTrackUpdate(RDP.RadarTrack updated)
        {

        }
        private void OpenGUI(object sender, EventArgs e)
        {
            MMI.InvokeOnGUI(delegate () { OpenGUI(); });
        }

        private void OpenGUI()
        {
            if (GUI == null || GUI.IsDisposed)
            {
                GUI = new MainForm();

            }
            else if (GUI.Visible) return;

            GUI.Show();


        }
    }
}
