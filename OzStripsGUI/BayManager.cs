using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// todo: separate GUI components into separate class
namespace maxrumsey.ozstrips.gui
{
    public class BayManager
    {
        public List<Bay> Bays;
        public FlowLayoutPanel flp_main;
        public List<FlowLayoutPanel> flp_vert_boards = new List<FlowLayoutPanel>();
        public BayManager(FlowLayoutPanel main) {
            Bays = new List<Bay>();
            this.flp_main = main;
        }

        public void AddVertBoard(FlowLayoutPanel flp_vert)
        {
            flp_vert_boards.Add(flp_vert);
        }

        public void AddStrip(StripController stripController)
        {
            stripController.BayManager = this;
            foreach (Bay bay in Bays)
            {
                if (bay.ResponsibleFor(stripController.currentBay))
                {
                    bay.AddStrip(stripController);
                }
            }
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
                panel.Padding = new System.Windows.Forms.Padding(3);

            }
            foreach (Bay bay in Bays)
            {
                int childnum = flp_vert_boards[bay.VerticalBoardNumber].Controls.Count;
                bay.ChildPanel.Size = new System.Drawing.Size(x_each, y_main / childnum);
            }
        }
         
    }
}
