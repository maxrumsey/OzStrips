using System;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Return value from the modal return value.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ModalReturnArgs"/> class.
/// </remarks>
/// <param name="child">The child.</param>
public class ModalReturnArgs(Control child) : EventArgs
{
    /// <summary>
    /// Gets the child.
    /// </summary>
    public Control Child { get; } = child;
}
