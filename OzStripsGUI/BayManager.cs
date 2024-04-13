using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static vatsys.FDP2;
using vatsys;

// todo: separate GUI components into separate class
namespace maxrumsey.ozstrips.gui
{
    public class BayManager
    {
        public List<Bay> Bays;
        public FlowLayoutPanel flp_main;
        public List<FlowLayoutPanel> flp_vert_boards = new List<FlowLayoutPanel>();
        public StripController Picked;
        public String AerodromeName = "YMML";
        public SocketConn socketConn;
        public BayManager(FlowLayoutPanel main) {
            Bays = new List<Bay>();
            this.flp_main = main;
        }

        public void DropStrip(Bay bay)
        {
            if (Picked != null)
            {
                Picked.currentBay = bay.BayTypes.FirstOrDefault();
                Picked.SyncStrip();
                UpdateBay(Picked);
                SetPicked();
            }
        }

        public void DeleteStrip(StripController strip)
        {
            FindBay(strip).RemoveStrip(strip, true);
            StripController.stripControllers.Remove(strip);
        }

        public void SetAerodrome(String name)
        {
            AerodromeName = name;
            socketConn.SetAerodrome();
            WipeStrips();
            StripController.stripControllers = new List<StripController>();

            foreach (FDP2.FDR fdr in FDP2.GetFDRs)
            {
                StripController.UpdateFDR(fdr, this);
            }
        }

        public void AddVertBoard(FlowLayoutPanel flp_vert)
        {
            flp_vert_boards.Add(flp_vert);
        }

        public void SetPicked(StripController controller)
        {
            if (Picked != null) Picked.HMI_SetPicked(false);
            Picked = controller;
            controller.HMI_SetPicked(true);
        }
        public void SetPicked()
        {
            Picked.HMI_SetPicked(false);
            Picked = null;
        }

        public void WipeStrips()
        {
            foreach (Bay bay in Bays)
            {
                bay.WipeStrips();
            }
        }

        public void AddStrip(StripController stripController)
        {
            stripController.BayManager = this;
            double distance = stripController.GetDistToAerodrome(AerodromeName);

            if (stripController.ApplicableToAerodrome(AerodromeName) == false) return;
            if (distance > 50 || distance == -1) return;

            foreach (Bay bay in Bays)
            {
                if (bay.ResponsibleFor(stripController.currentBay))
                {
                    bay.AddStrip(stripController);
                }
            }

            StripController.stripControllers.Add(stripController);
        }

        public Bay FindBay(StripController stripController) {
            foreach (Bay bay in Bays)
            {
                if (bay.OwnsStrip(stripController))
                {
                    return bay;
                }
            }
            return null;

        }

        public void UpdateBay(StripController stripController)
        {
            foreach (Bay bay in Bays)
            {
                if (bay.OwnsStrip(stripController))
                {
                    bay.RemoveStrip(stripController);
                }
            }
            stripController.ClearStripControl();
            stripController.CreateStripObj();
            AddStrip(stripController);

        }

        public void AddBay(Bay bay, int vertboardnum)
        {
            Bays.Add(bay);
            flp_vert_boards[vertboardnum].Controls.Add(bay.ChildPanel);
            //flp_main.PerformLayout();
        }

        public void ForceRerender()
        {
            foreach (Bay bay in Bays)
            {
                bay.ForceRerender();
            }
        }
        public void Resize()
        {
            if (flp_main == null) return;
            int x_main = flp_main.Size.Width;
            int y_main = flp_main.Size.Height;

            int x_each = flp_main.Size.Width / 3;

            foreach (FlowLayoutPanel panel in flp_vert_boards)
            {
                panel.Size = new System.Drawing.Size(x_each, y_main);
                panel.Margin = new System.Windows.Forms.Padding();
                panel.Padding = new System.Windows.Forms.Padding(2);

            }
            foreach (Bay bay in Bays)
            {
                int childnum = flp_vert_boards[bay.VerticalBoardNumber].Controls.Count;
                bay.ChildPanel.Size = new System.Drawing.Size(x_each-4, (y_main-4) / childnum);
            }
        }

        public void PositionKey(int relPos)
        {
            if (Picked != null)
            {
                FindBay(Picked)?.ChangeStripPosition(Picked, relPos);

            }
        }

    }
}
