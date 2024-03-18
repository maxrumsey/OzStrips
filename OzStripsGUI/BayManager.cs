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
        private FlowLayoutPanel flp_main;
        public BayManager(FlowLayoutPanel main) {
            Bays = new List<Bay>();
            this.flp_main = main;
        }

        public void AddStrip(StripController stripController)
        {
            foreach (Bay bay in Bays)
            {
                if (bay.ResponsibleFor(stripController.currentBay))
                {
                    bay.AddStrip(stripController);
                }
            }
        }

        public void AddBay(Bay bay)
        {
            Bays.Add(bay);
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

            foreach (Bay bay in Bays)
            {
                if (bay.ChildPanel == null) continue;
                bay.ChildPanel.Size = new System.Drawing.Size(x_each, y_main);
                bay.ChildPanel.Margin = new System.Windows.Forms.Padding();
                bay.ChildPanel.Padding = new System.Windows.Forms.Padding(3);

            }
        }
         
    }
}
