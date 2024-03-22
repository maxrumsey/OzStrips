using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maxrumsey.ozstrips.gui
{
    public partial class BayControl : UserControl
    
    {
        public FlowLayoutPanel ChildPanel;
        public BayControl(String name)
        {
            InitializeComponent();
            lb_bay_name.Text = name;
            ChildPanel = flp_stripbay;
            flp_stripbay.VerticalScroll.Visible = true;
        }
    }
}
