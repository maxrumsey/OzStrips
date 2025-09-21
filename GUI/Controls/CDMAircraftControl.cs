using MaxRumsey.OzStripsPlugin.GUI.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

/// <summary>
/// Presents information about the current CDM result to the user.
/// </summary>
public partial class CDMAircraftControl : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CDMAircraftControl"/> class.
    /// </summary>
    /// <param name="cdmDTO">CDM DTO object.</param>
    public CDMAircraftControl(CDMResultDTO cdmDTO)
    {
        InitializeComponent();

        lb_acid.Text += cdmDTO.Aircraft.Key.Callsign;

        lb_items.Items.Add($"CTOT: {GetUserFriendlyDateTime(cdmDTO.CTOT)}");
        lb_items.Items.Add($"TSAT: {GetUserFriendlyDateTime(cdmDTO.TSAT)}");
        lb_items.Items.Add("------------");

        lb_items.Items.Add($"TOBT: {GetUserFriendlyDateTime(cdmDTO.Aircraft.TOBT)}");
        lb_items.Items.Add($"AOBT: {GetUserFriendlyDateTime(cdmDTO.Aircraft.AOBT)}");
        lb_items.Items.Add($"ATOT: {GetUserFriendlyDateTime(cdmDTO.Aircraft.ATOT)}");
        lb_items.Items.Add($"SLOT EOBT: {"##:##Z"}");
        lb_items.Items.Add("------------");
        lb_items.Items.Add($"State: {cdmDTO.Aircraft.State.ToString()}");
        lb_items.Items.Add($"Current Priority: {cdmDTO.Aircraft.SlotType.ToString()}");
        lb_items.Items.Add($"Projected Final Priority: {cdmDTO.FinalSlotType.ToString()}");
    }

    private static string GetUserFriendlyDateTime(DateTime time)
    {
        if (time != DateTime.MaxValue)
        {
            return time.ToUniversalTime().ToString("HH:mmZ", System.Globalization.CultureInfo.InvariantCulture);
        }
        else
        {
            return "##:##Z";
        }
    }
}
