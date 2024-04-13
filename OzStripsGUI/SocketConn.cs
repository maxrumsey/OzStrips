using maxrumsey.ozstrips.gui.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace maxrumsey.ozstrips.gui
{
    public class SocketConn
    {
        SocketIOClient.SocketIO io;
        private BayManager bayManager;
        public SocketConn(BayManager bayManager, MainForm mf) {
            this.bayManager = bayManager;
            io = new SocketIOClient.SocketIO("http://localhost:8069");
            io.OnConnected += async (sender, e) =>
            {
                await io.EmitAsync("client:aerodrome_subscribe", bayManager.AerodromeName);
                mf.SetConnStatus(true);
            };
            io.On("server:sc_change", sc =>
            {
                StripControllerDTO scDTO = sc.GetValue<StripControllerDTO>();
                mf.Invoke((System.Windows.Forms.MethodInvoker)delegate () { StripController.UpdateFDR(scDTO, bayManager); }); 

            });

            io.ConnectAsync();

            bayManager.socketConn = this;
        }

        public void SyncSC(StripController sc)
        {
            StripControllerDTO scDTO = new StripControllerDTO { ACID = sc.fdr.Callsign, bay = sc.currentBay, CLX = sc.CLX, GATE = sc.GATE, StripCockLevel = sc.cockLevel };
            io.EmitAsync("client:sc_change", scDTO);
        }

        public void SetAerodrome()
        {
            io.EmitAsync("client:aerodrome_subscribe", bayManager.AerodromeName);
        }
    }
}
