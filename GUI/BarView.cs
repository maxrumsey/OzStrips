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

namespace MaxRumsey.OzStripsPlugin.GUI;

internal class BarView(BayRenderController bayRC) : IRenderedStripItem
{
    private readonly BayRenderController _bayRenderController = bayRC;
    private bool _picked;

    public SKPoint Origin { get; set; } = new SKPoint(0, 0);

    public SKPoint ElementOrigin { get; set; } = new SKPoint(0, 0);

    public StripListItem? Item { get; set; }

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

        if (MainFormController.ControlHeld)
        {
            _bayRenderController.Bay.BayManager.DropStripBelow(Item);
            return;
        }

        _bayRenderController.Bay.BayManager.TogglePickedStripItem(Item, _bayRenderController.Bay);
    }

    public void MarkPicked(bool picked)
    {
        _picked = picked;
    }

    public void HandleHover(Point e)
    {
        return;
    }

    private void DrawStripFrame(SKCanvas canvas)
    {
        var paint = new SKPaint()
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
        };

        canvas.DrawRect(Origin.X, Origin.Y, BayRenderController.StripWidth + 4, BayRenderController.BarHeight - 1, paint);
    }

    private void DrawStripText(SKCanvas canvas)
    {
        if (Item is null)
        {
            return;
        }

        var paint = new SKPaint()
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true,
        };

        if (Item.Style == 3)
        {
            paint.Color = SKColors.White;
        }

        var fontsize = 18;

        var typeface = SKTypeface.FromFamilyName("Segoe UI", 700, 5, SKFontStyleSlant.Upright);

        canvas.DrawText(Item.BarText, new SKPoint(Origin.X + ((BayRenderController.StripWidth + 4) / 2), Origin.Y + ((fontsize + BayRenderController.BarHeight) / 2)), SKTextAlign.Center, new SKFont(typeface, fontsize), paint);
    }

    private void DrawStripBackground(SKCanvas canvas)
    {
        var paint = new SKPaint()
        {
            Color = SKColors.Gray,
            Style = SKPaintStyle.Fill,
        };

        if (Item?.Style == 1)
        {
            paint.Color = SKColors.LightGray;
        }
        else if (Item?.Style == 2)
        {
            paint.Color = SKColors.Orange;
        }
        else if (Item?.Style == 3)
        {
            paint.Color = SKColors.Red;
        }

        if (_picked)
        {
            paint.Color = SKColors.DarkGray;
        }

        canvas.DrawRect(Origin.X, Origin.Y, BayRenderController.StripWidth + 4, BayRenderController.BarHeight - 1, paint);
    }
}
