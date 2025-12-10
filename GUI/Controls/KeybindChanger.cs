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

    public KeybindChanger()
    {
        InitializeComponent();

        foreach (var keybind in KeybindManager.ActiveKeybinds)
        {
            var label = new Label();
            label.Text = $"{KeybindManager.Friendlyname[keybind.Key]} : {keybind.Value.ToString()}";
            label.Tag = keybind.Key;
            label.Click += Clicked;
            flowLayoutPanel1.Controls.Add(label);
        }

        KeyDown += KeyDownEv; ;
    }

    private void KeyDownEv(object sender, System.Windows.Forms.KeyEventArgs args)
    {
        if (_activeKey is null)
        {
            return;
        }

        var key = (KEYBINDS)_activeKey.Tag;
        var keybind = KeyInterop.KeyFromVirtualKey((int)args.KeyCode);


    }

    private void Clicked(object sender, EventArgs e)
    {
        var label = (Label)sender;

        SetActiveKey(label);
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
