using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

/// <summary>
/// A altitude and heading control.
/// </summary>
public partial class DebugSetATIS : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DebugSetATIS"/> class.
    /// </summary>
    /// <param name="controller">The strip.</param>
    public DebugSetATIS()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets the PDC text.
    /// </summary>
    public string ATISText
    {
        get => tb_atis.Text;
    }
}
