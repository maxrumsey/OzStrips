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
using System.Xml.Linq;

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

            AddVerticalStripBoard();
            AddVerticalStripBoard();
            AddVerticalStripBoard();

            Bay bay1 = new Bay(new List<StripBay>() { StripBay.BAY_PREA },bayManager, "Bay1", 0);
            bayManager.AddBay(bay1, 0);
            Bay bay2 = new Bay(new List<StripBay>() { StripBay.BAY_CLEARED }, bayManager, "Bay2", 0);
            bayManager.AddBay(bay2, 0);
            Bay bay3 = new Bay(new List<StripBay>() { StripBay.BAY_PUSHED }, bayManager, "Bay3", 1);
            bayManager.AddBay(bay3, 1);
            Bay bay4 = new Bay(new List<StripBay>() { StripBay.BAY_TAXI }, bayManager, "Bay3", 1);
            bayManager.AddBay(bay4, 2);

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
            bayManager.ForceRerender();
        }

        private void forceRerenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bayManager.ForceRerender();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            bayManager.Resize();
        }

        private void AddVerticalStripBoard()
        {
            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.AutoScroll = true;
            flp.Margin = new Padding(0);
            flp.Padding = new Padding(0);
            flp.Size = new Size(100, 100);
            flp.Location = new Point(0, 0);
            flp_main.Controls.Add(flp);
            bayManager.AddVertBoard(flp);
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
