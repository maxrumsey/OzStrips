using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.DTO.XML;
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

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

/// <summary>
/// Drop down form.
/// </summary>
public partial class DropDown : BaseForm
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DropDown"/> class.
    /// </summary>
    /// <param name="items">List of drop down items.</param>
    /// <param name="title">Element title.</param>
    public DropDown(DropDownItem[] items, string title)
    {
        InitializeComponent();
        Setup();
        Text = title;

        foreach (var item in items)
        {
            AddElement(item);
        }

        PerformLayout();
        flowLayoutPanel1.Size = new(100, 28 * items.Length);
        ClientSize = new(100, (28 * items.Length) + 25);
        MinimumSize = new(100, 28);
        MaximumSize = ClientSize;
        StartPosition = FormStartPosition.Manual;
        Location = Cursor.Position;


        foreach (var control in flowLayoutPanel1.Controls)
        {
            var textbox = control as TextBox;
            textbox?.Select();
        }
    }

    /// <summary>
    /// Called when the drop down is completed, and will return a value.
    /// </summary>
    public event EventHandler<string>? Complete;

    private void Setup()
    {
        MiddleClickClose = true;
    }

    private void AddElement(DropDownItem item)
    {
        Control element;
        switch (item.Type)
        {
            case DropDownItem.DropDownItemType.BUTTON:
                element = new GenericButton();
                element.Text = item.Text;
                element.Size = new(100, 28);
                element.MouseUp += Element_MouseUp;
                break;
            case DropDownItem.DropDownItemType.FREETEXT:
                var tb = new TextBox();
                element = tb;
                tb.Text = item.Text;
                tb.Size = new(100, 28);
                tb.MaxLength = item.MaxLen;
                tb.CharacterCasing = CharacterCasing.Upper;
                element.KeyPress += (s, e) =>
                {
                    if (e.KeyChar == (char)Keys.Escape)
                    {
                        Close();
                    }
                    else if (e.KeyChar == (char)Keys.Enter)
                    {
                        Complete?.Invoke(element, tb.Text);
                        Close();
                    }
                };
                break;
            default:
                throw new InvalidOperationException("Unknown drop down item type");
        }

        element.Margin = new(0);

        flowLayoutPanel1.Controls.Add(element);
    }

    private void Element_MouseUp(object sender, MouseEventArgs e)
    {
        var control = sender as Control ?? throw new Exception("Sender was null.");
        if (e.Button == MouseButtons.Left)
        {
            Complete?.Invoke(this, control.Text);
            Close();
        }
        else if (e.Button == MouseButtons.Middle)
        {
            Close();
        }
    }

    private static bool DropDownAlreadyOpen()
    {
        foreach (Form form in Application.OpenForms)
        {
            if (form is DropDown)
            {
                return true;
            }
        }

        return false;
    }

    private static void CreateDropDown(DropDownItem[] items, string title, Action<string> result)
    {
        if (DropDownAlreadyOpen())
        {
            return;
        }
        var dropdown = new DropDown(items, title);
        dropdown.Complete += (s, e) =>
        {
            try
            {
                result(e);
            }
            catch (Exception ex)
            {
                Util.LogError(ex);
            }
        };
        dropdown.Show(MainForm.MainFormInstance);
    }

    /// <summary>
    /// Shows the gate drop down for the specified strip.
    /// </summary>
    /// <param name="strip">Strip.</param>
    public static void ShowGateDropDown(Strip strip)
    {
        CreateDropDown([new(DropDownItem.DropDownItemType.FREETEXT, strip.Gate, 4)], strip.FDR.Callsign, s =>
        {
            strip.Gate = s;
            strip.SyncStrip();
        });
    }

    /// <summary>
    /// Shows the clx drop down for the specified strip.
    /// </summary>
    /// <param name="strip">Strip.</param>
    public static void ShowCLXDropDown(Strip strip)
    {
        CreateDropDown([new(DropDownItem.DropDownItemType.FREETEXT, strip.CLX)], strip.FDR.Callsign, s =>
        {
            strip.CLX = s;
            strip.SyncStrip();
        });
    }

    /// <summary>
    /// Shows the glop drop down for the specified strip.
    /// </summary>
    /// <param name="strip">Strip.</param>
    public static void ShowGlopDropDown(Strip strip)
    {
        CreateDropDown([new(DropDownItem.DropDownItemType.FREETEXT, strip.FDR.GlobalOpData, 10)], strip.FDR.Callsign, s =>
        {
            if (!Network.Me.IsRealATC)
            {
                return;
            }

            strip.FDR.GlobalOpData = s;
        });
    }

    /// <summary>
    /// Shows the remark drop down for the specified strip.
    /// </summary>
    /// <param name="strip">Strip.</param>
    public static void ShowRmkDropDown(Strip strip)
    {
        CreateDropDown([new(DropDownItem.DropDownItemType.FREETEXT, strip.Remark, 10)], strip.FDR.Callsign, s =>
        {
            strip.Remark = s;
            strip.SyncStrip();
        });
    }

    /// <summary>
    /// Shows the dep freq drop down for the specified strip.
    /// </summary>
    /// <param name="strip">Strip.</param>
    public static void ShowFreqDropDown(Strip strip)
    {
        List<DropDownItem> items = new();

        foreach (var freq in strip.PossibleDepFreqs)
        {
            items.Add(new(DropDownItem.DropDownItemType.BUTTON, freq));
        }

        items.Add(new(DropDownItem.DropDownItemType.FREETEXT, string.Empty, 7));
        CreateDropDown(items.ToArray(), strip.FDR.Callsign, s =>
        {
            strip.DepartureFrequency = s;
            strip.SyncStrip();
        });
    }

    /// <summary>
    /// Shows a crossing / release bar dropdown.
    /// </summary>
    /// <param name="autoMapAerodrome">Automap aerodrome file.</param>
    /// <param name="type">Crossing or Released.</param>
    /// <param name="bayManager">Bay Manager.</param>
    public static void ShowCrossingOrReleaseDropDown(AutoMapAerodrome autoMapAerodrome, string type, BayManager bayManager)
    {
        List<DropDownItem> items = new();

        var runways = autoMapAerodrome.RunwayPairs;

        foreach (var runway in runways)
        {
            if (runway.Length % 2 != 0)
            {
                continue;
            }

            items.Add(new(DropDownItem.DropDownItemType.BUTTON, runway.Insert(runway.Length / 2, "/")));
        }

        CreateDropDown([.. items], "Runway", s =>
        {
            try
            {
                bayManager.ToggleCrossReleaseBar(s.Replace("/", string.Empty), type);
            }
            catch (Exception ex)
            {
                Util.LogError(ex);
            }
        });
    }
}
