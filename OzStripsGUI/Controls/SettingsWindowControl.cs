using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// A altitude and heading control.
/// </summary>
public partial class SettingsWindowControl : UserControl
{
    private readonly SocketConn _socket;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsWindowControl"/> class.
    /// </summary>
    /// <param name="socketConn">Socket Controller.</param>
    public SettingsWindowControl(SocketConn socketConn)
    {
        InitializeComponent();

        _socket = socketConn;

        if (Properties.OzStripsSettings.Default.UseVatSysPopup)
        {
            rb_vatsys.Checked = true;
            rb_ozstrips.Checked = false;
        }
        else
        {
            rb_vatsys.Checked = false;
            rb_ozstrips.Checked = true;
        }

        var servercontrol = rb_vatsim;

        switch (_socket.Server)
        {
            case SocketConn.Servers.SWEATBOX1:
                servercontrol = rb_sb1;
                break;
            case SocketConn.Servers.SWEATBOX2:
                servercontrol = rb_sb2;
                break;
            case SocketConn.Servers.SWEATBOX3:
                servercontrol = rb_sb3;
                break;
            default:
                break;
        }

        servercontrol.Checked = true;
    }

    /// <summary>
    /// Called when the modal is closed.
    /// </summary>
    /// <param name="source">Source.</param>
    /// <param name="args">Arguments.</param>
    public void ModalReturned(object source, ModalReturnArgs args)
    {
        var usevatsyspopup = false;
        if (rb_vatsys.Checked)
        {
            usevatsyspopup = true;
        }

        Properties.OzStripsSettings.Default.UseVatSysPopup = usevatsyspopup;
        Properties.OzStripsSettings.Default.Save();
    }

    private void SBButtonClick(object sender, EventArgs e)
    {
        var type = SocketConn.Servers.VATSIM;

        if (rb_sb1.Checked)
        {
            type = SocketConn.Servers.SWEATBOX1;
        }
        else if (rb_sb2.Checked)
        {
            type = SocketConn.Servers.SWEATBOX2;
        }
        else if (rb_sb3.Checked)
        {
            type = SocketConn.Servers.SWEATBOX3;
        }

        _socket.SetServerType(type);
    }
}
