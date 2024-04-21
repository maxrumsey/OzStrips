using System.Windows.Forms;

namespace maxrumsey.ozstrips.gui
{
    internal static class Util
    {
        public static void ShowErrorBox(string message)
        {
            MessageBox.Show(Form.ActiveForm, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowInfoBox(string message)
        {
            MessageBox.Show(Form.ActiveForm, message, "OzStrips", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void ShowWarnBox(string message)
        {
            MessageBox.Show(Form.ActiveForm, message, "OzStrips", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
