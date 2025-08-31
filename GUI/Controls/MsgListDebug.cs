using System;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

/// <summary>
/// A msg list debugger control.
/// </summary>
public partial class MsgListDebug : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MsgListDebug"/> class.
    /// </summary>
    /// <param name="socketConn">The socket connection.</param>
    public MsgListDebug(SocketConn socketConn)
    {
        InitializeComponent();
        foreach (var str in socketConn.Messages)
        {
            lb_menu.Items.Add(str);
        }
    }

    private void MenuClicked(object sender, EventArgs e)
    {
        rtb_text.Text = lb_menu.Text;
    }
}
