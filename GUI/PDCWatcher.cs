using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// Form to watch PDC requests.
/// </summary>
public partial class PDCWatcher : BaseForm
{
    private readonly Dictionary<string, int> _openRequests = new();
    private readonly HubConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="PDCWatcher"/> class.
    /// </summary>
    public PDCWatcher()
    {
        InitializeComponent();
        _connection = new HubConnectionBuilder()
            .WithUrl(OzStripsConfig.socketioaddr + "PDCHub")
            .WithAutomaticReconnect()
            .Build();

        _connection.On<Dictionary<string, int>?>("NewPDCs", (Dictionary<string, int>? pdcs) =>
        {
            if (pdcs != null)
            {
                _openRequests.Clear();

                foreach (var aerodrome in pdcs.ToList())
                {
                    if (OwnsAerodrome(aerodrome.Key))
                    {
                        _openRequests[aerodrome.Key] = aerodrome.Value;
                    }
                }

                MMI.InvokeOnGUI(() =>
                {
                    try
                    {
                        DisplayData();
                    }
                    catch (Exception ex)
                    {
                        Util.LogError(ex, "OzStrips PDC");
                    }
                });
            }
        });

        Start();
    }

    private DataTable CreateDataTable()
    {
        var table = new DataTable();

        table.Columns.Add("ICAO", typeof(string));
        table.Columns.Add("Open Requests", typeof(int));

        foreach (var entry in _openRequests)
        {
            var row = table.NewRow();
            row["ICAO"] = entry.Key;
            row["Open Requests"] = entry.Value;
            table.Rows.Add(row);
        }

        return table;
    }

    private void DisplayData()
    {
        dataGridView1.DataSource = CreateDataTable();

        foreach (DataGridViewColumn column in dataGridView1.Columns)
        {
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }

    private static bool OwnsAerodrome(string icao)
    {
        var sectors = MMI.SectorsControlled.ToList();
        foreach (var sector in sectors.ToArray())
        {
            sectors.AddRange(sector.SubSectors ?? []);
        }

        var aerodromeLoc = Airspace2.GetAirport(icao)?.LatLong ?? new();

        foreach (var sector in sectors)
        {
            if (sector.IsInSector(aerodromeLoc, 100))
            {
                return true;
            }
        }

        return false;
    }

    private async void Start()
    {
        try
        {
            await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            Util.LogError(ex, "OzStrips PDC");
        }
    }

    private void SelectionChanged(object sender, EventArgs e)
    {
        dataGridView1.ClearSelection();
    }
}
