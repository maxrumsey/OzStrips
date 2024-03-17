using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using vatsys.Plugin;
using System.Collections.Concurrent;
using vatsys;
using maxrumsey.ozstrips.gui;
using System.Windows.Forms;

namespace maxrumsey.ozstrips
{

    [Export(typeof(IPlugin))]
    public class OzStrips : IPlugin
    {
        private CustomToolStripMenuItem ozStripsOpener;
        private NetworkATC vatsysConn;
        private bool isObs = false;
        private Socket socketClient;
        private MainForm GUI;

        public OzStrips()
        {
            Network.Connected += Connected;
            ozStripsOpener = new CustomToolStripMenuItem(CustomToolStripMenuItemWindowType.Main, CustomToolStripMenuItemCategory.Windows, new ToolStripMenuItem("OzStrips"));
            ozStripsOpener.Item.Click += OpenGUI;
            MMI.AddCustomMenuItem(ozStripsOpener);
        }

        private void Connected(object sender, EventArgs e)
        {
            Errors.Add(new Exception("Connection Established"));
            vatsysConn = Network.Me;
            isObs = !vatsysConn.IsRealATC;

            MMI.InvokeOnGUI((System.Windows.Forms.MethodInvoker)delegate () { OpenGUI(); });
            
           try
            {
                socketClient = new Socket(vatsysConn);
            }
            catch (Exception er)
            {
                Errors.Add(er, "OzStrips");
            }

        }


        /// Plugin Name
        public string Name { get => "OzStrips Connector"; }

        public void OnFDRUpdate(FDP2.FDR updated)
        {
            //Errors.Add(new Exception("mew"));
            //System.Diagnostics.Process.Start("http://google.com");
            socketClient.SendFDR(updated);
            if (GUI != null) GUI.Invoke((System.Windows.Forms.MethodInvoker)delegate () { GUI.UpdateFDR(updated); });
        }
        
        /// Not needed for this plugin. But you could for instance, use the new position of the radar track or its change in state (cancelled, etc.) to do some processing. 
        public void OnRadarTrackUpdate(RDP.RadarTrack updated)
        {

        }


        public void Test()
        {
            throw new NotImplementedException();
        }

        private void OpenGUI(object sender, EventArgs e)
        {
            OpenGUI();
        }
        private void OpenGUI()
        {
            if (GUI == null || GUI.IsDisposed)
            {
                List<FDP2.FDR> fdrs = FDP2.GetFDRs;

                GUI = new MainForm(socketClient, fdrs);

            }
            else if (GUI.Visible) return;

            GUI.Show();
        }
    }
}
