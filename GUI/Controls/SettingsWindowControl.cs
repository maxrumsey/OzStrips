using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.GUI.Properties;
using vatsys;
using static MaxRumsey.OzStripsPlugin.GUI.Shared.ConnectionMetadataDTO;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

/// <summary>
/// A altitude and heading control.
/// </summary>
public partial class SettingsWindowControl : UserControl
{
    private readonly List<string> _autoOpenOptions = new()
    {
        "Once per session",
        "Always",
        "Never",
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsWindowControl"/> class.
    /// </summary>
    /// <param name="socketConn">Socket Controller.</param>
    /// <param name="aerodromes">List of aerodromes.</param>
    public SettingsWindowControl(List<string> aerodromes)
    {
        InitializeComponent();

        foreach (var s in aerodromes)
        {
            lb_ads.Items.Add(s);
        }

        cb_keeppicked.Checked = OzStripsSettings.Default.KeepStripPicked;
        cb_preasort.Checked = OzStripsSettings.Default.AlphaSortPrea;
        cb_lasttransmit.Checked = OzStripsSettings.Default.ShowLastTransmit;

        tb_scale.Value = (int)(100f * OzStripsSettings.Default.StripScale);

        foreach (var option in _autoOpenOptions)
        {
            cb_open.Items.Add(option);
        }

        cb_open.SelectedIndex = OzStripsSettings.Default.AutoOpenBehaviour > -1 ? OzStripsSettings.Default.AutoOpenBehaviour : 0;
    }

    /// <summary>
    /// Called when the modal is closed.
    /// </summary>
    /// <param name="source">Source.</param>
    /// <param name="args">Arguments.</param>
    public void ModalReturned(object source, ModalReturnArgs args)
    {
        Util.SetEnvVar("StripScale", tb_scale.Value / 100f);

        Util.SetEnvVar("KeepStripPicked", cb_keeppicked.Checked);

        Util.SetEnvVar("AlphaSortPrea", cb_preasort.Checked);

        Util.SetEnvVar("AutoOpenBehaviour", cb_open.SelectedIndex);

        Util.SetEnvVar("ShowLastTransmit", cb_lasttransmit.Checked);

        MainFormController.Instance?.ForceResize();
    }

    private void ADAddClick(object sender, EventArgs e)
    {
        if (tb_ad.Text.Length == 4 && tb_ad.Text.All(char.IsLetter))
        {
            lb_ads.Items.Add(tb_ad.Text.ToUpper(CultureInfo.InvariantCulture));
            tb_ad.Text = string.Empty;

            MainFormController.Instance?.SetCustomAerodromeList(lb_ads.Items.OfType<string>().ToList());
        }
    }

    private void ADRemoveClick(object sender, EventArgs e)
    {
        if (lb_ads.SelectedIndex != -1)
        {
            lb_ads.Items.RemoveAt(lb_ads.SelectedIndex);
            MainFormController.Instance?.SetCustomAerodromeList(lb_ads.Items.OfType<string>().ToList());
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
