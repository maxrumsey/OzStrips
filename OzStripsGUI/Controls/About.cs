using maxrumsey.ozstrips.gui.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;
using maxrumsey.ozstrips.gui;

namespace maxrumsey.ozstrips.controls
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            lb_version.Text = "Version: " + Config.version;

        }

    }
}
