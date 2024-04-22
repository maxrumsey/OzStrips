using System;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Return value from the modal return value.
/// </summary>
public class ModalReturnArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModalReturnArgs"/> class.
    /// </summary>
    /// <param name="child">The child.</param>
    public ModalReturnArgs(Control child)
    {
        Child = child;
    }

    /// <summary>
    /// Gets the child.
    /// </summary>
    public Control Child { get; }
}
