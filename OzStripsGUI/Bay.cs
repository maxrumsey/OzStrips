using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maxrumsey.ozstrips.gui
{
    public class Bay
    {
        public List<StripBay> BayTypes;
        public BayManager Manager;
        public String Name;
        public List<StripController> Strips = new List<StripController>();
        public BayControl ChildPanel;
        public int VerticalBoardNumber;
        public Bay(List<StripBay> bays, BayManager bm, String name, int vertboardnum)
        {
            BayTypes = bays;
            Manager = bm;
            Name = name;
            VerticalBoardNumber = vertboardnum;
            this.ChildPanel = new BayControl(bm, name, this);

            bm.AddBay(this, vertboardnum);

        }

        public bool ResponsibleFor(StripBay bay)
        {
            if (BayTypes.Contains(bay))
            {
                return true;
            }
            else return false;
        }

        public bool OwnsStrip(StripController controller)
        {
            return Strips.Contains(controller);
        }

        public void RemoveStrip(StripController controller, bool remove)
        {
            if (remove) Strips.Remove(controller);
            orderStrips();

        }
        public void RemoveStrip(StripController controller)
        {
            RemoveStrip(controller, true);

        }
        public void WipeStrips()
        {
            foreach (StripController strip in Strips)
            {
                RemoveStrip(strip, false);
            }
            Strips = new List<StripController>();
        }

        //todo: check for dupes
        public void AddStrip(StripController stripController)
        {
            Strips.Add(stripController); // todo: add control action
            orderStrips();
        }
        public void ForceRerender()
        {
            foreach (StripController s in Strips)
            {
                s.UpdateFDR();
            }
        }
        private void orderStrips()
        {
            ChildPanel.ChildPanel.SuspendLayout();
            ChildPanel.ChildPanel.Controls.Clear();
            for (int i = 0; i < Strips.Count; i++)
            {
                ChildPanel.ChildPanel.Controls.Add(Strips[i].stripHolderControl);
            }
            ChildPanel.ChildPanel.ResumeLayout();

        }

        public void ChangeStripPosition(StripController sc, int relpos)
        {
            int originalPosition = Strips.FindIndex(a => a == sc);
            int newPosition = originalPosition + relpos;

            if (newPosition < 0 || newPosition > (Strips.Count - 1)) return;

            Strips.RemoveAt(originalPosition);
            Strips.Insert(newPosition, sc);
            orderStrips();
        }
    }
}
