using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;
using vatsys;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

internal class BarView(BayRenderController bayRC) : IRenderedStripItem
{
    private readonly BayRenderController _bayRenderController = bayRC;

    public SKPoint Origin { get; set; } = new SKPoint(0, 0);

    public SKPoint ElementOrigin { get; set; } = new SKPoint(0, 0);

    public void Render(SKCanvas canvas)
    {
        DrawStripBackground(canvas);
    }

    public void HandleClick(MouseEventArgs e)
    {
        var x = _bayRenderController;
        return;
    }

    private void DrawStripBackground(SKCanvas canvas)
    {
        var paint = new SKPaint()
        {
            Color = SKColors.LightGray,
            Style = SKPaintStyle.Fill,
        };

        canvas.DrawRect(Origin.X, Origin.Y, BayRenderController.StripWidth + 4, BayRenderController.StripHeight, paint);
    }
}
