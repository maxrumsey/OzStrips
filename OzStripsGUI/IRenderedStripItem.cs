using System.Drawing;
using System.Windows.Forms;
using SkiaSharp;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// Represents a rendered item that appears in a strip bay.
/// </summary>
internal interface IRenderedStripItem
{
    /// <summary>
    /// Gets or sets the origin strip elements are drawn at.
    /// </summary>
    SKPoint ElementOrigin { get; set; }

    /// <summary>
    /// Gets or sets the origin of the strip item.
    /// </summary>
    SKPoint Origin { get; set; }

    /// <summary>
    /// Triggered when the item is clicked.
    /// </summary>
    /// <param name="e">The mouse event args.</param>
    void HandleClick(MouseEventArgs e);

    /// <summary>
    /// Triggered when the item is hovered on.
    /// </summary>
    /// <param name="point">The mouse position.</param>
    void HandleHover(Point point);

    /// <summary>
    /// Called when the item is rendered.
    /// </summary>
    /// <param name="canvas">Canvas.</param>
    void Render(SKCanvas canvas);

    /// <summary>
    /// Marks the strip as picked or not.
    /// </summary>
    /// <param name="picked">Whether to mark them as picked or not.</param>
    void MarkPicked(bool picked);
}
