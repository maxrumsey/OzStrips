using System;
using System.Globalization;
using System.Windows.Forms;

using MaxRumsey.OzStripsPlugin.Gui.DTO;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// Send the user a manual message.
/// </summary>
public partial class ManualMsgDebug : UserControl
{
    private readonly BayManager _bayManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManualMsgDebug"/> class.
    /// </summary>
    /// <param name="bayManager">The bay manager.</param>
    public ManualMsgDebug(BayManager bayManager)
    {
        InitializeComponent();
        _bayManager = bayManager;
    }

    private void SendButton(object sender, EventArgs e)
    {
        var scDTO = new StripControllerDTO
        {
            acid = tb_acid.Text,
            bay = (StripBay)int.Parse(tb_baynum.Text, CultureInfo.InvariantCulture),
            CLX = tb_clx.Text,
            GATE = tb_bay.Text,
            crossing = cb_crossing.Checked,
            cocklevel = int.Parse(tb_cocklevel.Text, CultureInfo.InvariantCulture),
            TOT = "\0",
        };

        StripController.UpdateFDR(scDTO, _bayManager);
    }
}
