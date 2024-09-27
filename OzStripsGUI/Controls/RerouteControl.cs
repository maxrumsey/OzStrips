using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// A altitude and heading control.
/// </summary>
public partial class RerouteControl : UserControl
{
    private readonly Strip _stripController;
    private readonly RouteDTO[]? _routes;

    /// <summary>
    /// Initializes a new instance of the <see cref="RerouteControl"/> class.
    /// </summary>
    /// <param name="controller">The controller.</param>
    public RerouteControl(Strip controller)
    {
        InitializeComponent();
        SuspendLayout();
        _stripController = controller;
        _routes = controller.ValidRoutes;

        if (_routes != null && _routes.Length > 0)
        {
            ls_routes.Items.Clear();
            foreach (var route in _routes)
            {
                ls_routes.Items.Add($"({route.acft}):{route.route}");
            }
        }

        tb_route.Text = controller.Route;
        ResumeLayout();
    }

    private void RouteSelected(object sender, EventArgs e)
    {
        tb_route.Text = ls_routes.Text.Split(':').Last();
    }

    private void Save_Click(object sender, EventArgs e)
    {
        if (!_stripController.FDR.HavePermission)
        {
            lb_error.Text = "Permission denied.";
            return;
        }

        try
        {
            lb_error.Text = string.Empty;
            FDP2.ModifyRoute(_stripController.FDR, tb_route.Text);
            if (_stripController.FDR.DepartureRunway is not null)
            {
                FDP2.SetDepartureRunway(_stripController.FDR, _stripController.FDR.DepartureRunway);
            }
            else
            {
                var runway = _stripController.PossibleDepRunways.FirstOrDefault();
                if (runway != null)
                {
                    FDP2.SetDepartureRunway(_stripController.FDR, runway);
                    FDP2.SetDepartureRunway(_stripController.FDR, null);
                }
                else
                {
                    lb_error.Text = "Synchronisation to network failed.";
                }
            }
        }
        catch (FDRParseException ex)
        {
            lb_error.Text = ex.Message;
        }
        catch (Exception ex)
        {
            lb_error.Text = ex.Message;
            Util.LogError(ex);
        }
    }
}
