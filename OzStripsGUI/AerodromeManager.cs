using MaxRumsey.OzStripsPlugin.Gui.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Manages aerodromes immaediately available in the selection list.
/// </summary>
public class AerodromeManager
{
    public string? AutoOpenAerodrome;

    public List<string> ConcernedAerodromes = new();

    public List<string> ManuallySetAerodromes = new();

    public AerodromeSettings? Settings;

    public List<string> AerodromeList
    {
        get
        {
            var completeList = ConcernedAerodromes.ToList();
            completeList.AddRange(ManuallySetAerodromes);

            if (AutoOpenAerodrome is not null)
            {
                completeList.Add(AutoOpenAerodrome);
            }

            return completeList;
        }
    }

    public void Initialise()
    {
        MMI.PrimePositonChanged += PrimePositionChanged;
        MMI.SectorsControlledChanged += SectorsChanged;
    }

    private void SectorsChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void PrimePositionChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
