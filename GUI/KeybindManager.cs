using MaxRumsey.OzStripsPlugin.GUI.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace MaxRumsey.OzStripsPlugin.GUI;

/// <summary>
/// Gets, and saves per-user keybinds.
/// </summary>
internal static class KeybindManager
{
    public static Dictionary<KEYBINDS, Key> ActiveKeybinds { get; private set; } = new();


    public static Keys GetKey(KEYBINDS bind)
    {
        return (Keys)KeyInterop.VirtualKeyFromKey(ActiveKeybinds[bind]);
    }

    private static Dictionary<KEYBINDS, Key> TryLoadKeybinds()
    {
        var keybinds = new Dictionary<KEYBINDS, Key>(DefaultKeybinds);

        try
        {
            if (string.IsNullOrEmpty(OzStripsSettings.Default.Keybinds))
            {
                return keybinds;
            }

            var deserialized = JsonSerializer.Deserialize<Dictionary<KEYBINDS, Key>>(OzStripsSettings.Default.Keybinds);

            foreach (var keypair in deserialized ?? [])
            {
                keybinds[keypair.Key] = keypair.Value;
            }
        }
        catch (Exception ex)
        {
            Util.LogError(ex);
        }

        return keybinds;
    }

    public static void SaveKeybinds(IDictionary<KEYBINDS, Key> keys)
    {
        var str = JsonSerializer.Serialize(keys);

        Util.SetEnvVar("Keybinds", str);
    }

    public static void Reload()
    {
        ActiveKeybinds = TryLoadKeybinds();
    }

    internal enum KEYBINDS
    {
        LAST_TRANSMIT_HIGHLIGHT,
        UP,
        DOWN,
        QUEUE,
        TRIGGER,
        COCK,
        AERODROME_LEFT,
        AERODROME_RIGHT,
        INHIBIT,
        CROSS,
        FLIP,
        AUTOFILL,
        LAST_TRANSMIT_PICK,
        MODIFIER1,
        MODIFIER2,
        FIND,
    }

    internal static readonly Dictionary<KEYBINDS, Key> DefaultKeybinds = new()
    {
        { KEYBINDS.LAST_TRANSMIT_HIGHLIGHT, Key.W },
        { KEYBINDS.UP, Key.Up },
        { KEYBINDS.DOWN, Key.Down },
        { KEYBINDS.QUEUE, Key.Space },
        { KEYBINDS.TRIGGER, Key.Enter },
        { KEYBINDS.COCK, Key.Tab },
        { KEYBINDS.AERODROME_LEFT, Key.OemOpenBrackets },
        { KEYBINDS.AERODROME_RIGHT, Key.OemCloseBrackets },
        { KEYBINDS.INHIBIT, Key.Back },
        { KEYBINDS.CROSS, Key.X },
        { KEYBINDS.FLIP, Key.F },
        { KEYBINDS.AUTOFILL, Key.A },
        { KEYBINDS.LAST_TRANSMIT_PICK, Key.T },
        { KEYBINDS.MODIFIER1, Key.LeftCtrl },
        { KEYBINDS.MODIFIER2, Key.LeftAlt },
        { KEYBINDS.FIND, Key.F },
    };

    internal static readonly Dictionary<KEYBINDS, string> Friendlyname = new()
    {
        { KEYBINDS.LAST_TRANSMIT_HIGHLIGHT, "Highlight Last Transmit" },
        { KEYBINDS.UP, "Move Strip Up" },
        { KEYBINDS.DOWN, "Move Strip Down" },
        { KEYBINDS.QUEUE, "Queue Strip" },
        { KEYBINDS.TRIGGER, "SID Trigger Strip" },
        { KEYBINDS.COCK, "Cock Strip" },
        { KEYBINDS.AERODROME_LEFT, "Move Prev Aerodrome" },
        { KEYBINDS.AERODROME_RIGHT, "Move Next Aerodrome" },
        { KEYBINDS.INHIBIT, "Inhibit Strip" },
        { KEYBINDS.CROSS, "Cross Strip" },
        { KEYBINDS.FLIP, "Flip Strip" },
        { KEYBINDS.AUTOFILL, "Autofill Strip" },
        { KEYBINDS.LAST_TRANSMIT_PICK, "Pick Last Transmit" },
        { KEYBINDS.MODIFIER1, "Strip Action Modifier" },
        { KEYBINDS.MODIFIER2, "Crossing Bar Modifier" },
        { KEYBINDS.FIND, "Find Strip (MODIFIER)" },
    };
}
