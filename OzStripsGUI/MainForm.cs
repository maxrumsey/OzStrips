using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using vatsys;

namespace maxrumsey.ozstrips.gui
{
    public partial class MainForm : Form
    {
        Timer timer;
        Socket socket;
        BayManager bayManager;
        SocketConn socketConn;
        public MainForm(List<FDP2.FDR> fdrs)
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += updateTimer;
            timer.Start();

            AddAerodrome("YMML"); //todo: add more ads
            AddAerodrome("YSSY");
            AddAerodrome("YBBN");



            bayManager = new BayManager(flp_main);

            AddVerticalStripBoard();
            AddVerticalStripBoard();
            AddVerticalStripBoard();

            socketConn = new SocketConn(bayManager, this);

            Bay bay_pr = new Bay(new List<StripBay>() { StripBay.BAY_PREA }, bayManager, "Preactive", 0);
            Bay bay_cl = new Bay(new List<StripBay>() { StripBay.BAY_CLEARED }, bayManager, "Cleared", 0);
            Bay bay_pb = new Bay(new List<StripBay>() { StripBay.BAY_PUSHED }, bayManager, "Pushback", 1);
            Bay bay_tx = new Bay(new List<StripBay>() { StripBay.BAY_TAXI }, bayManager, "Taxi", 1);
            Bay bay_hp = new Bay(new List<StripBay>() { StripBay.BAY_HOLDSHORT }, bayManager, "Holding Point", 1);
            Bay bay_rw = new Bay(new List<StripBay>() { StripBay.BAY_RUNWAY }, bayManager, "Runway", 2);
            Bay bay_out = new Bay(new List<StripBay>() { StripBay.BAY_OUT }, bayManager, "Departed", 2);
            Bay bay_arr = new Bay(new List<StripBay>() { StripBay.BAY_ARRIVAL }, bayManager, "Arrivals", 2);

            bayManager.Resize();

            /*flp_bay.Controls.Clear();
            foreach (StripController controller in StripController.stripControllers)
            {
                flp_bay.Controls.Add(controller.stripControl);

            }*/
        }

        public void SetConnStatus(bool conn)
        {
            if (conn)
            {
                pl_stat.BackColor = Color.Green;
            }
            else
            {
                pl_stat.BackColor = Color.OrangeRed;
            }
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
        private void AddAerodrome(String name)
        {
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem
            {
                Text = name
            };
            toolStripMenuItem.Click += (Object sender, EventArgs e) =>
            {
                SetAerodrome(name);
            };
            ts_ad.DropDownItems.Add(toolStripMenuItem);
        }
        private void SetAerodrome(String name)
        {
            if (bayManager != null)
            {
                bayManager.SetAerodrome(name);
                lb_ad.Text = name;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                bayManager.PositionKey(1);
            }
            else if (keyData == Keys.Down)
            {
                bayManager.PositionKey(-1);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (bayManager != null && e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                bayManager.SetAerodrome(toolStripTextBox1.Text.ToUpper()); ;
                lb_ad.Text = toolStripTextBox1.Text.ToUpper();
                e.Handled = true;

            }
        }

        private void bt_inhibit_Click(object sender, EventArgs e)
        {
            bayManager.Inhibit();
        }
    }
}
