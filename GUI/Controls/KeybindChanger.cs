using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static MaxRumsey.OzStripsPlugin.GUI.KeybindManager;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls;
public partial class KeybindChanger : UserControl
{
    private Label? _activeKey;
    private Button _resetButton;

    public KeybindChanger()
    {
        InitializeComponent();

        foreach (var keybind in KeybindManager.ActiveKeybinds)
        {
            var label = new Label();
            label.Text = $"{KeybindManager.Friendlyname[keybind.Key]} : {keybind.Value.ToString()}";
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Tag = keybind.Key;
            label.Click += Clicked;
            label.AutoSize = true;
            label.Dock = DockStyle.Top;
            label.Padding = new(4);
            flowLayoutPanel1.Controls.Add(label);
        }

        _resetButton = new Button();
        _resetButton.Text = "Reset";
        _resetButton.Click += ResetAll;
        flowLayoutPanel1.Controls.Add(_resetButton);

        _resetButton.TabStop = false;
    }

    private void ResetAll(object sender, EventArgs e)
    {
        KeybindManager.SaveKeybinds(DefaultKeybinds);
        KeybindManager.Reload();
        ResetAllLabels();

        ActiveControl = null;
    }

    /// <summary>
    /// Handles key presses to set keybinds.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="newKey">New key.</param>
    /// <returns>Successfully handled.</returns>
    protected override bool ProcessCmdKey(ref Message message, Keys _)
    {
        if (_activeKey is null)
        {
            return false;
        }

        var key = (KEYBINDS)_activeKey.Tag;
        var keybind = KeybindManager.GetPressedKeys().FirstOrDefault();

        if (keybind == Key.None)
        {
            return false;
        }

        KeybindManager.ActiveKeybinds[key] = keybind;
        KeybindManager.SaveKeybinds(KeybindManager.ActiveKeybinds);
        KeybindManager.Reload();

        ResetAllLabels();

        return true;
    }

    private void Clicked(object sender, EventArgs e)
    {
        var label = (Label)sender;
        SetActiveKey(label);
    }

    private void ResetAllLabels()
    {
        _activeKey = null;
        foreach (Control control in flowLayoutPanel1.Controls)
        {
            if (control is Label label)
            {
                var key = (KEYBINDS)label.Tag;
                label.Text = $"{KeybindManager.Friendlyname[key]} : {KeybindManager.ActiveKeybinds[key].ToString()}";
            }
        }
    }

    private void SetActiveKey(Label label)
    {
        if (_activeKey is not null)
        {
            var key = (KEYBINDS)_activeKey.Tag;
            _activeKey.Text = $"{KeybindManager.Friendlyname[key]} : {KeybindManager.ActiveKeybinds[key].ToString()}";
        }

        _activeKey = label;
        _activeKey.Text = $"{KeybindManager.Friendlyname[(KEYBINDS)label.Tag]} : <>";
    }
}
