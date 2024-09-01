using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace MaxRumsey.OzStripsPlugin.Gui;

internal class StripView(Strip strip, BayRenderController bayRC)
{
    private readonly Strip _strip = strip;
    private readonly BayRenderController _bayRenderController = bayRC;

    public SKPoint Origin { get; set; } = new SKPoint(0, 0);

    public void Render(SKCanvas canvas)
    {
        var bdrypaint = new SKPaint()
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
        };
        var x = _strip;
        var y = _bayRenderController;
        canvas.DrawRect(Origin.X, Origin.Y, Origin.X + BayRenderController.StripWidth, Origin.Y + BayRenderController.StripHeight, bdrypaint);
    }
}
