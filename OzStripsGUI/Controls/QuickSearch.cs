using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// Represents the control used when creates bars.
/// </summary>
public partial class QuickSearch : UserControl
{
    private readonly BayManager _bm;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuickSearch"/> class.
    /// </summary>
    /// <param name="bayManager">Current baymanager.</param>
    public QuickSearch(BayManager bayManager)
    {
        InitializeComponent();

        _bm = bayManager;

        DisplayData();
    }

    /// <summary>
    /// Gets the list of aircraft in the table.
    /// </summary>
    public List<AircraftList> Aircrafts { get; } = new();

    /// <summary>
    /// Gets the selected strip.
    /// </summary>
    public Strip? Selected { get; private set; }

    /// <summary>
    /// Gets or sets the base modal that this control is contained in.
    /// </summary>
    public BaseModal? BaseModal { get; set; }

    /// <summary>
    /// Called when parent modal is returned.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Args.</param>
    public void ModalReturned(object sender, ModalReturnArgs e)
    {
        if (Selected is not null)
        {
            _bm.SetPickedStripFromExternal(Selected);
        }
    }

    private void DisplayData()
    {
        var dt = new DataTable();
        dt = ConvertToDatatable();
        dataGridView1.DataSource = dt;

        dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    private DataTable ConvertToDatatable()
    {
        var dt = new DataTable();
        dt.Columns.Add("Callsign");
        dt.Columns.Add("Bay");

        foreach (var item in Aircrafts)
        {
            var row = dt.NewRow();
            row["Callsign"] = item.Callsign;
            row["Bay"] = item.Bay;
            dt.Rows.Add(row);
        }

        return dt;
    }

    private new void TextChanged(object sender, EventArgs e)
    {
        Aircrafts.Clear();

        foreach (var strip in _bm.StripRepository.Strips)
        {
            if (strip.FDR.Callsign.Contains(tb_callsign.Text.ToUpper(CultureInfo.InvariantCulture)))
            {
                Aircrafts.Add(new AircraftList { _strip = strip });
            }
        }

        DisplayData();

        if (dataGridView1.SelectedRows.Count == 1)
        {
            Selected = _bm.StripRepository.Strips.FirstOrDefault(s => s.FDR.Callsign == dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
        }
        else
        {
            Selected = null;
        }
    }

    private void SelectionChange(object sender, EventArgs e)
    {
        Selected = dataGridView1.SelectedRows.Count > 0
            ? _bm.StripRepository.Strips.FirstOrDefault(s => s.FDR.Callsign == dataGridView1.SelectedRows[0].Cells[0].Value.ToString())
            : null;
    }

    private new void KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.Enter:

                BaseModal?.ExitModal(true);

                break;
            case Keys.Escape:
                BaseModal?.ExitModal();
                break;
        }
    }

    private new void DoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        BaseModal?.ExitModal(true);
    }
}
