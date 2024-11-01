using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.Properties;
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
    /// <param name="aerodromes">List of aerodromes.</param>
    public SettingsWindowControl(SocketConn socketConn, List<string> aerodromes)
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

        foreach (var s in aerodromes)
        {
            lb_ads.Items.Add(s);
        }

        if (OzStripsSettings.Default.KeepStripPicked)
        {
            cb_keeppicked.Checked = true;
        }

        tb_scale.Value = (int)(100f * OzStripsSettings.Default.StripScale);
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

        Util.SetEnvVar("UseVatSysPopup", usevatsyspopup);

        Util.SetEnvVar("StripScale", tb_scale.Value / 100f);

        Util.SetEnvVar("KeepStripPicked", cb_keeppicked.Checked);

        MainForm.MainFormInstance?.ForceResize();
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

    private void ADAddClick(object sender, EventArgs e)
    {
        if (tb_ad.Text.Length == 4 && tb_ad.Text.All(char.IsLetter))
        {
            lb_ads.Items.Add(tb_ad.Text.ToUpper(CultureInfo.InvariantCulture));
            tb_ad.Text = string.Empty;

            MainForm.MainFormInstance?.SetAerodromeList(lb_ads.Items.OfType<string>().ToList());
        }
    }

    private void ADRemoveClick(object sender, EventArgs e)
    {
        if (lb_ads.SelectedIndex != -1)
        {
            lb_ads.Items.RemoveAt(lb_ads.SelectedIndex);
            MainForm.MainFormInstance?.SetAerodromeList(lb_ads.Items.OfType<string>().ToList());
        }
    }

    private void ADKeyPress(object sender, KeyPressEventArgs e)
    {
       if (e.KeyChar == (char)Keys.Return)
        {
            ADAddClick(sender, e);
        }
    }

    private void KeyboardCommandsOpened(object sender, LinkLabelLinkClickedEventArgs e)
    {
        System.Diagnostics.Process.Start("https://maxrumsey.xyz/OzStrips/reference/keyboardcommands/");
    }
}
