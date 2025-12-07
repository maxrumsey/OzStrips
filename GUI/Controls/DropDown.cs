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
    }

    /// <summary>
    /// Called when the drop down is completed, and will return a value.
    /// </summary>
    public event EventHandler<string>? Complete;

    private void Setup()
    {
        MiddleClickClose = true;
        HasMinimizeButton = false;
        HasMaximizeButton = false;
        HasSendBackButton = false;
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
                element.KeyPress += (s, e) =>
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        Complete?.Invoke(element, tb.Text);
                        Close();
                    }
                };
                break;
            default:
                throw new InvalidOperationException("Unknown drop down item type");
        }

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
}
