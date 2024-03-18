using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maxrumsey.ozstrips.gui
{
    internal class Bay
    {
        public List<StripBay> BayTypes;
        public BayManager Manager;
        public String Name;
        public List<StripController> Strips = new List<StripController>();
        public FlowLayoutPanel ChildPanel;
        public Bay(List<StripBay> bays, BayManager bm, String name)
        {
            BayTypes = bays;
            Manager = bm;
            Name = name;
        }
        
        public bool ResponsibleFor(StripBay bay)
        {
            if (BayTypes.Contains(bay))
            {
                return true;
            }
            else return false;
        }

        //todo: check for dupes
        public void AddStrip(StripController stripController)
        {
            Strips.Add(stripController); // todo: add control action
            ChildPanel.Controls.Add(stripController.stripControl);
        }

    }
}
