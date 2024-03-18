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

            bayManager = new BayManager();
            Bay bay1 = new Bay(new List<StripBay>() { StripBay.BAY_PREA },bayManager, "Bay1");
            bayManager.AddBay(bay1);
            bay1.ChildPanel = flp_bay;

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
