using System.Windows.Forms;

namespace maxrumsey.ozstrips.controls
{
    public class CustomFLP : FlowLayoutPanel
    {
        public CustomFLP() : base()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }
    }
}
