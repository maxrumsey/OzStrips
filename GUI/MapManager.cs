using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// Manages map visibility in ASMGCS windows based on current bar states.
/// </summary>
internal static class MapManager
{
    internal static Form[] GetAllGroundWindows()
    {
        var groundWindows = new List<Form>();

        foreach (Form openForm in Application.OpenForms)
        {
            if (openForm.Text.StartsWith("Ground", StringComparison.InvariantCulture) && openForm.Name == "ASMGCSWindow")
            {
                groundWindows.Add(openForm);
            }
        }

        return [.. groundWindows];
    }

    internal static void SetApplicableMaps(string[] barNames, string aerodromeName)
    {
        foreach (var form in GetAllGroundWindows())
        {
            var mapControls = GetRunwayMapControls(form, aerodromeName);

            foreach (var control in mapControls)
            {
                var mapRunways = control.Text.Split('_')[1];
                var shouldChange = false;
                if (control.Text.StartsWith("Crossing", StringComparison.InvariantCulture))
                {
                    shouldChange = control.Checked != DoesBarExist(barNames, mapRunways, "Crossing");
                }
                else if (control.Text.StartsWith("Released", StringComparison.InvariantCulture))
                {
                    shouldChange = control.Checked != DoesBarExist(barNames, mapRunways, "Released");
                }

                if (shouldChange)
                {
                    control.PerformClick();
                }
            }
        }
    }

    internal static ToolStripMenuItem[] GetRunwayMapControls(Form groundWindow, string aerodromeName)
    {
        var mainMenu = groundWindow.Controls.Find("mainMenu", false).FirstOrDefault() as MenuStrip ?? throw new Exception("Can't locate map toolstrip.");

        var mapDropDown = mainMenu.Items.OfType<ToolStripMenuItem>().FirstOrDefault(item => item.Name == "mapsToolStripMenuItem") ?? throw new Exception("Can't locate map dropdown.");

        var specificAerodromeDropDown = mapDropDown.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(item => item.Text == aerodromeName) ?? throw new Exception($"Can't locate {aerodromeName} map folder.");

        return [.. specificAerodromeDropDown.DropDownItems.OfType<ToolStripMenuItem>().Where(x => x.Text.StartsWith("Crossing", StringComparison.InvariantCulture) || x.Text.StartsWith("Released", StringComparison.InvariantCulture))];
    }

    internal static bool DoesBarExist(string[] barNames, string runwayPair, string mapType)
    {
        var runways = new string[2];

        if (runwayPair.Length % 2 != 0)
        {
            throw new Exception("Malformed map runway name.");
        }

        runways[0] = runwayPair.Substring(0, runwayPair.Length / 2);
        runways[1] = runwayPair.Substring(runwayPair.Length / 2);

        if (mapType == "Crossing")
        {
            return barNames.Any(bar => bar.StartsWith($"XXX CROSSING RWY {runways[0]}/{runways[1]}", StringComparison.InvariantCulture));
        }
        else if (mapType == "Released")
        {
            return barNames.Any(bar => bar.StartsWith($"Runway {runways[0]}/{runways[1]} Released", StringComparison.InvariantCulture));
        }

        return false;
    }
}
