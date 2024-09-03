using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

internal class StripView(Strip strip, BayRenderController bayRC)
{
    private readonly Strip _strip = strip;
    private readonly BayRenderController _bayRenderController = bayRC;
    private readonly int _padding = 2;

    public SKPoint Origin { get; set; } = new SKPoint(0, 0);

    public SKPoint ElementOrigin { get; set; } = new SKPoint(0, 0);

    public void Render(SKCanvas canvas)
    {
        ElementOrigin = new SKPoint(Origin.X + _padding, Origin.Y + _padding);

        var x = _bayRenderController;
        var stripelementlist = StripElementList.Instance?.List;
        if (stripelementlist is null)
        {
            return;
        }

        var bdrypaint = new SKPaint()
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
        };

        DrawStripBackground(canvas);

        canvas.DrawRect(ElementOrigin.X, ElementOrigin.Y, BayRenderController.StripWidth, BayRenderController.StripHeight - (2 * _padding) - 1, bdrypaint);

        foreach (var element in stripelementlist)
        {
            var elpaint = new SKPaint()
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Stroke,
            };

            var textpaint = new SKPaint()
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.StrokeAndFill,
                IsAntialias = true,
            };

            var baseX = ElementOrigin.X + element.X;
            var baseY = ElementOrigin.Y + element.Y;

            var text = GetElementText(element);
            var fontsize = 10f;

            var typeface = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Bold);
            canvas.DrawRect(baseX, baseY, element.W, element.H, elpaint);

            canvas.DrawText(text, new SKPoint(baseX + (element.W / 2), baseY + ((fontsize + element.H) / 2)), SKTextAlign.Center, new SKFont(typeface, fontsize), textpaint);
        }
    }

    public void HandleClick(MouseEventArgs e)
    {
        
    }

    private string GetElementText(StripElement element)
    {
        switch (element.Value)
        {
            case StripElements.Values.EOBT:
                return _strip.Time;
            case StripElements.Values.ACID:
                return _strip.FDR.Callsign;
            case StripElements.Values.SSR:
                return (_strip.FDR.AssignedSSRCode == -1) ? "XXXX" : Convert.ToString(_strip.FDR.AssignedSSRCode, 8).PadLeft(4, '0');
            default:
                break;
        }

        return string.Empty;
    }

    private void DrawStripBackground(SKCanvas canvas)
    {
        var paint = new SKPaint()
        {
            Color = (_strip.ArrDepType == StripArrDepType.ARRIVAL) ? SKColors.Yellow : SKColors.CornflowerBlue,
            Style = SKPaintStyle.Fill,
        };

        canvas.DrawRect(Origin.X, Origin.Y, BayRenderController.StripWidth + (2 * _padding), BayRenderController.StripHeight, paint);
    }
}
