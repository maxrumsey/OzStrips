using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;

namespace maxrumsey.ozstrips.gui
{
    public class Bay
    {
        public List<StripBay> BayTypes;
        public BayManager Manager;
        public String Name;
        public List<StripListItem> Strips = new List<StripListItem>();
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
            bool found = false;
            foreach (StripListItem item in Strips)
            {
                if (item.StripController == controller) found = true;
            }

            return found;
        }

        public void RemoveStrip(StripController controller, bool remove)
        {
            if (remove) Strips.RemoveAll(item => item.StripController == controller);
            orderStrips();

        }
        public void RemoveStrip(StripController controller)
        {
            RemoveStrip(controller, true);

        }
        public void WipeStrips()
        {
            foreach (StripListItem strip in Strips)
            {
                RemoveStrip(strip.StripController, false);
            }

            Strips = new List<StripListItem>();
            orderStrips();

        }

        //todo: check for dupes
        public void AddStrip(StripController stripController)
        {
            StripListItem strip = new StripListItem();
            strip.StripController = stripController;
            strip.Type = StripItemType.STRIP;

            Strips.Add(strip); // todo: add control action
            orderStrips();

        }
        public void ForceRerender()
        {
            StripListItem[] stripList = new StripListItem[Strips.Count];
            Strips.CopyTo(stripList);

            foreach (StripListItem s in stripList)
            {
                if (s.Type == StripItemType.STRIP) s.StripController.UpdateFDR();

            }
        }
        public void orderStrips()
        {
            ChildPanel.ChildPanel.SuspendLayout();
            ChildPanel.ChildPanel.Controls.Clear();
            for (int i = 0; i < Strips.Count; i++)
            {
                if (Strips[i].Type == StripItemType.STRIP) ChildPanel.ChildPanel.Controls.Add(Strips[i].StripController.stripHolderControl);
                if (Strips[i].Type == StripItemType.QUEUEBAR)
                {
                    ChildPanel.ChildPanel.Controls.Add(Strips[i].DividerBarControl);
                }
            }
            ChildPanel.ChildPanel.ResumeLayout();

        }

        public void ChangeStripPosition(StripController sc, int relpos)
        {
            int originalPosition = Strips.FindIndex(a => a.StripController == sc);
            StripListItem stripItem = Strips[originalPosition];
            int newPosition = originalPosition + relpos;

            if (newPosition < 0 || newPosition > (Strips.Count - 1)) return;

            Strips.RemoveAt(originalPosition);
            Strips.Insert(newPosition, stripItem);
            orderStrips();
            if (Manager.socketConn != null) Manager.socketConn.SyncBay(this);

        }
        public void ChangeStripPositionAbs(StripListItem item, int abspos)
        {

            if (abspos < 0 || abspos > (Strips.Count - 1)) return;

            Strips.Remove(item);
            Strips.Insert(abspos, item);
            orderStrips();
            if (Manager.socketConn != null) Manager.socketConn.SyncBay(this);

        }
        public void AddDivider(bool? force, bool sync = true)
        {
            bool containsDiv = false;
            StripListItem currentItem = null;
            foreach (StripListItem item in Strips)
            {
                if (item.Type == StripItemType.QUEUEBAR) { 
                    containsDiv = true;
                    currentItem = item;
                }
            }

            if (!containsDiv)
            {
                StripListItem newItem = new StripListItem();
                newItem.Type = StripItemType.QUEUEBAR;
                newItem.DividerBarControl = new DividerBarControl();
                Strips.Insert(0, newItem);
            } else if (force == null || force == false)
            {
                Strips.Remove(currentItem);
            }
            orderStrips();
            if (Manager.socketConn != null && sync) Manager.socketConn.SyncBay(this);
        }

        public void QueueUp()
        {

            if (Manager.Picked != null && OwnsStrip(Manager.Picked))
            {
                AddDivider(true, false);
                StripListItem item = Strips.Find(a => a?.StripController == Manager.Picked);
                ChangeStripPositionAbs(item, DivPosition);
                Manager.SetPicked();
                if (Manager.socketConn != null) Manager.socketConn.SyncBay(this);

            }
        }

        public int DivPosition
        {
            get
            {
                int val = 0;
                for (int i = 0; i < Strips.Count; i++)
                {
                    if (Strips[i].Type == StripItemType.QUEUEBAR) val = i;
                }
                return val;
            }
        }

        public StripListItem GetListItemByStr(string code)
        {
            StripListItem returnedItem = null;
            foreach (StripListItem stripListItem in Strips)
            {
                if (code == "\a" && stripListItem.Type == StripItemType.QUEUEBAR) returnedItem = stripListItem;
                else if (stripListItem.Type == StripItemType.STRIP && stripListItem.StripController.fdr.Callsign == code) returnedItem = stripListItem;
            }

            return returnedItem;
        }
    }

    public class StripListItem
    {
        public StripController StripController;
        public StripItemType Type;
        public DividerBarControl DividerBarControl;



    }

    public enum StripItemType
    {
        STRIP,
        QUEUEBAR,
        BLOCKED
    }
}


