using maxrumsey.ozstrips.gui.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Timers;
using vatsys;

namespace maxrumsey.ozstrips.gui
{
    public class SocketConn
    {
        SocketIOClient.SocketIO io;
        private BayManager bayManager;
        private bool isDebug =  !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VisualStudioEdition"));
        public List<string> Messages = new List<string>();
        private bool versionShown = false;
        private bool freshClient = false;
        private System.Timers.Timer fifteensecTimer;
        private MainForm mainForm;
        public SocketConn(BayManager bayManager, MainForm mf)
        {
            mainForm = mf;
            this.bayManager = bayManager;
            io = new SocketIOClient.SocketIO(Config.socketioaddr);
            io.OnAny((sender, e) =>
            {
                MetadataDTO metaDTO = e.GetValue<MetadataDTO>(1);
                if (metaDTO.version != Config.version && !versionShown)
                {
                    Util.ShowErrorBox("New Update Available: " + metaDTO.version);
                    versionShown = true;
                }
                if (metaDTO.apiversion != Config.apiversion) {
                    Util.ShowErrorBox("OzStrips incompatible with current API version!");
                    mf.Invoke((System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        mf.Close();
                        mf.Dispose();
                    });
                }
            });

            io.OnConnected += async (sender, e) =>
            {
                await io.EmitAsync("client:aerodrome_subscribe", bayManager.AerodromeName);
                mf.SetConnStatus(true);
            };
            io.OnDisconnected += (sender, e) =>
            {
                mf.SetConnStatus(false);
            };

            io.OnError += (sender, e) =>
            {
                mf.SetConnStatus(false);
                Errors.Add(new Exception(e), "OzStrips");
            };
            io.OnReconnected += async (sender, e) =>
            {
                await io.EmitAsync("client:aerodrome_subscribe", bayManager.AerodromeName);
                mf.SetConnStatus(true);
            };

            io.On("server:sc_change", sc =>
            {
                StripControllerDTO scDTO = sc.GetValue<StripControllerDTO>();
                Messages.Add("s:sc_change: " + JsonSerializer.Serialize(scDTO));

                mf.Invoke((System.Windows.Forms.MethodInvoker)delegate () { StripController.UpdateFDR(scDTO, bayManager); });

            });
            io.On("server:sc_cache", sc =>
            {
                CacheDTO scDTO = sc.GetValue<CacheDTO>();
                Messages.Add("s:sc_cache: " + JsonSerializer.Serialize(scDTO));

                mf.Invoke((System.Windows.Forms.MethodInvoker)delegate () { StripController.LoadCache(scDTO); });

            });
            io.On("server:order_change", bdto =>
            {
                BayDTO bayDTO = bdto.GetValue<BayDTO>();
                Messages.Add("s:order_change: " + JsonSerializer.Serialize(bayDTO));

                mf.Invoke((System.Windows.Forms.MethodInvoker)delegate () { bayManager.UpdateOrder(bayDTO); });
            });
            io.On("server:update_cache", (args) =>
            {
                Messages.Add("s:update_cache: ");
                SendCache();
            });
            if (Network.IsConnected) Connect();
            bayManager.socketConn = this;
        }

        public void SyncSC(StripController sc)
        {
            StripControllerDTO scDTO = CreateStripDTO(sc);
            Messages.Add("c:sc_change: " + JsonSerializer.Serialize(scDTO));

            if (io.Connected && (Network.Me.IsRealATC || isDebug)) io.EmitAsync("client:sc_change", scDTO);

        }
        public void SyncBay(Bay bay)
        {
            BayDTO bayDTO = CreateBayDTO(bay);
            Messages.Add("c:order_change: " + JsonSerializer.Serialize(bayDTO));

            if (io.Connected && (Network.Me.IsRealATC || isDebug)) io.EmitAsync("client:order_change", bayDTO);
        }
        public void SetAerodrome()
        {
            if (io.Connected) io.EmitAsync("client:aerodrome_subscribe", bayManager.AerodromeName);
        }

        public BayDTO CreateBayDTO(Bay bay)
        {
            BayDTO bayDTO = new BayDTO { bay = bay.BayTypes.First() };
            List<string> childList = new List<string>();
            foreach (StripListItem item in bay.Strips)
            {
                if (item.Type == StripItemType.STRIP) childList.Add(item.StripController.fdr.Callsign);
                else if (item.Type == StripItemType.QUEUEBAR) childList.Add("\a"); // indicates q-bar
            }
            bayDTO.list = childList;
            return bayDTO;
        }
        public StripControllerDTO CreateStripDTO(StripController sc)
        {
            StripControllerDTO scDTO = new StripControllerDTO { ACID = sc.fdr.Callsign, bay = sc.currentBay, CLX = sc.CLX, GATE = sc.GATE, StripCockLevel = sc.cockLevel, Crossing = sc.Crossing, remark = sc.Remark };
            if (sc.TakeOffTime != DateTime.MaxValue)
            {
                scDTO.TOT = sc.TakeOffTime.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                scDTO.TOT = "\0";
            }
            return scDTO;
        }
        public CacheDTO CreateCachePacket()
        {
            List<StripControllerDTO> strips = new List<StripControllerDTO>();

            foreach (StripController strip in StripController.stripControllers)
            {
                strips.Add(CreateStripDTO(strip));
            }

            return new CacheDTO() {strips = strips };
        }

        public async void SendCache()
        {
            CacheDTO cacheDTO = CreateCachePacket();
            if (io.Connected && (Network.Me.IsRealATC || isDebug)) await io.EmitAsync("client:sc_cache", cacheDTO);
        }

        public void Close()
        {
            io.DisconnectAsync();
            io.Dispose();
        }

        public void Connect()
        {
            fifteensecTimer = new System.Timers.Timer();
            fifteensecTimer.AutoReset = false;
            fifteensecTimer.Interval = 15000;
            fifteensecTimer.Elapsed += ConnectIO;
            fifteensecTimer.Start();
            mainForm.SetAerodrome(bayManager.AerodromeName);
        }

        private void ConnectIO(object sender, ElapsedEventArgs e)
        {
            io.ConnectAsync();
        }
    }
}
