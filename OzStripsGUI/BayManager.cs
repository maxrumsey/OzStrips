using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maxrumsey.ozstrips.gui
{
    internal class BayManager
    {
        public List<Bay> Bays;
        public BayManager() {
            Bays = new List<Bay>();
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

    }
}
