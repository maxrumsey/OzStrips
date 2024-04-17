using System.Windows.Forms;

namespace maxrumsey.ozstrips.gui
{
    internal static class Util
    {
        public static void ShowErrorBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
