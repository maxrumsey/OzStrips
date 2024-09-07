using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using SkiaSharp;
using vatsys;
using static System.Net.Mime.MediaTypeNames;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

internal class BarView(BayRenderController bayRC) : IRenderedStripItem
{
    private readonly BayRenderController _bayRenderController = bayRC;
    private bool _picked;

    public SKPoint Origin { get; set; } = new SKPoint(0, 0);

    public SKPoint ElementOrigin { get; set; } = new SKPoint(0, 0);

    public StripListItem? Item { get; set; }

    public string? Text { get; set; } = string.Empty;

    public void Render(SKCanvas canvas)
    {
        DrawStripBackground(canvas);
        DrawStripFrame(canvas);
        DrawStripText(canvas);
    }

    public void HandleClick(MouseEventArgs e)
    {
        if (Item is null)
        {
            return;
        }

        _bayRenderController.Bay.BayManager.TogglePicked(Item, true);
    }

    public void MarkPicked(bool picked)
    {
        _picked = picked;
    }

    private void DrawStripFrame(SKCanvas canvas)
    {
        var paint = new SKPaint()
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
        };

        canvas.DrawRect(Origin.X, Origin.Y, BayRenderController.StripWidth + 4, BayRenderController.StripHeight, paint);
    }

    private void DrawStripText(SKCanvas canvas)
    {
        var paint = new SKPaint()
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
        };
        var fontsize = 18;

        var typeface = SKTypeface.FromFamilyName("Segoe UI", 700, 5, SKFontStyleSlant.Upright);

        canvas.DrawText(Text, new SKPoint(Origin.X + ((BayRenderController.StripWidth + 4) / 2), Origin.Y + ((fontsize + BayRenderController.StripHeight) / 2)), SKTextAlign.Center, new SKFont(typeface, fontsize), paint);
    }

    private void DrawStripBackground(SKCanvas canvas)
    {
        var paint = new SKPaint()
        {
            Color = _picked ? SKColors.DarkGray : SKColors.Gray,
            Style = SKPaintStyle.Fill,
        };

        canvas.DrawRect(Origin.X, Origin.Y, BayRenderController.StripWidth + 4, BayRenderController.StripHeight, paint);
    }
}
