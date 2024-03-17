using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketIOClient;
using SocketIO;
using vatsys;

namespace maxrumsey.ozstrips
{
    public class Socket
    {
        private SocketIOClient.SocketIO client;
        private NetworkATC vatsysConn;
        public bool connected = false;
        private string authCode;
        public Socket(NetworkATC vatsysConna) {
            vatsysConn = vatsysConna;
            client = new SocketIOClient.SocketIO("http://localhost:8069/");
            client.OnConnected += connectionEstablished;
            client.OnError += connectionError;
            client.OnDisconnected += connectionError;
            client.On("auth", AuthReceived);

            client.ConnectAsync();

        }

        private async void connectionEstablished(object sender, EventArgs e)
        {
            connected = true;
            await client.EmitAsync("connector_logon", vatsysConn.Callsign, vatsysConn.ValidATC);

        }

        private void connectionError(object sender, String e)
        {
            connected = false;
            Errors.Add(new Exception(e), "OzStrips");

        }

        private void AuthReceived(SocketIOResponse e)
        {
            authCode = e.GetValue<String>();
            FetchFDRs();
        }

        public void SendFDR(FDP2.FDR fdr)
        {
            sendFDRs(new List<FDP2.FDR>() { fdr });
        }
        public async void sendFDRs(List<FDP2.FDR> fdrs)
        {
            try
            {
                if (connected)
                {
                    List<UpdatePacket> packets = new List<UpdatePacket>();
                    foreach (FDP2.FDR fdr in fdrs)
                    {
                        packets.Add(UpdatePacketFactory.CreateUpdatePacket(fdr));
                        await client.EmitAsync("connector_FDRs", authCode, packets);
                    }
                }
                else
                {
                    return;
                }
            } catch (Exception e)
            {
                Errors.Add(e, "OzStrips");
            }
        }
        private void FetchFDRs()
        {
            List<FDP2.FDR> fdrs = FDP2.GetFDRs;

            sendFDRs(fdrs);

        }
    }
}
