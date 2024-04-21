using System.Windows.Forms;

namespace maxrumsey.ozstrips.controls
{
    public class CustomFLP : FlowLayoutPanel
    {
        public CustomFLP() : base()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
    }
}
