using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;
using maxrumsey.ozstrips;
using System.Net.Sockets;
using System.Net.Http.Headers;

namespace maxrumsey.ozstrips.gui
{
    public partial class MainForm : Form
    {
        Timer timer;
        Socket socket;
        BayManager bayManager;
        public MainForm(Socket _socket, List<FDP2.FDR> fdrs)
        {
            socket = _socket;
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += updateTimer;
            timer.Start();

            bayManager = new BayManager(flp_main) ;
            Bay bay1 = new Bay(new List<StripBay>() { StripBay.BAY_PREA },bayManager, "Bay1");
            bayManager.AddBay(bay1);
            Bay bay2 = new Bay(new List<StripBay>() { StripBay.BAY_PREA }, bayManager, "Bay2");
            bayManager.AddBay(bay2);
            Bay bay3 = new Bay(new List<StripBay>() { StripBay.BAY_PREA }, bayManager, "Bay3");
            bayManager.AddBay(bay3);

            bay1.ChildPanel = flp_bay1;
            bay2.ChildPanel = flp_bay2;
            bay3.ChildPanel = flp_bay3;
            bayManager.Resize();
            foreach (FDP2.FDR fdr in fdrs)
            {
                StripController stripController = new StripController(fdr);
                bayManager.AddStrip(stripController);
            }
            /*flp_bay.Controls.Clear();
            foreach (StripController controller in StripController.stripControllers)
            {
                flp_bay.Controls.Add(controller.stripControl);

            }*/
        }


        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void UpdateFDR(FDP2.FDR fdr)
        {
            StripController.UpdateFDR(fdr, bayManager);
        }

        private void updateTimer(object sender, EventArgs e)
        {
            tb_Time.Text = DateTime.UtcNow.ToString("HH:mm:ss");
        }

        private void forceRerenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bayManager.ForceRerender();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            bayManager.Resize();
        }
        /*public void FDRDownlink(List<FDP2.FDR> fdrs)
{
flp_bay.Controls.Clear();
foreach (FDP2.FDR fdr in fdrs)
{
flp_bay.Controls.Add(new Strip(fdr));
}
}*/

    }
}
