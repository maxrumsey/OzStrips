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

        public void RemoveStrip(StripController controller)
        {
            Strips.Remove(controller);

        }

        //todo: check for dupes
        public void AddStrip(StripController stripController)
        {
            Strips.Add(stripController); // todo: add control action
            ChildPanel.ChildPanel.Controls.Add(stripController.stripHolderControl);
        }
        public void ForceRerender()
        {
            foreach (StripController s in Strips)
            {
                s.UpdateFDR();
            }
        }

    }
}
