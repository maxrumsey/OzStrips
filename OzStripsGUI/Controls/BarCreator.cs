using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// Represents the control used when creates bars.
/// </summary>
public partial class BarCreator : UserControl
{
    private readonly Dictionary<string, int> _bartypes = [];
    private readonly BayManager _bm;

    /// <summary>
    /// Initializes a new instance of the <see cref="BarCreator"/> class.
    /// </summary>
    /// <param name="bayManager">Current baymanager.</param>
    public BarCreator(BayManager bayManager)
    {
        InitializeComponent();

        _bartypes.Add("XXX CROSSING XXX", 3);
        _bartypes.Add("Runway ## Released to SMC", 1);
        _bartypes.Add("Autorelease Suspended", 2);
        _bartypes.Add("Standby For Ground", 1);
        _bartypes.Add("W - ###.# E - ###.#", 1);

        foreach (var bay in bayManager.BayRepository.Bays)
        {
            cb_bay.Items.Add(bay.Name);
        }

        foreach (var bar in _bartypes.Keys)
        {
            cb_item.Items.Add(bar);
        }

        cb_bay.SelectedIndex = 0;
        cb_item.SelectedIndex = 0;

        _bm = bayManager;
    }

    /// <summary>
    /// Called when parent modal is returned.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Args.</param>
    public void ModalReturned(object sender, ModalReturnArgs e)
    {
        _bm.AddBar(cb_bay.Text, _bartypes[cb_item.Text], tb_text.Text);
    }

    private void BarTypeChanged(object sender, EventArgs e)
    {
        tb_text.Text = cb_item.Text;
    }
}
