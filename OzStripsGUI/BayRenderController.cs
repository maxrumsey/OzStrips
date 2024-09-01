using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace MaxRumsey.OzStripsPlugin;

internal class BayRenderController(Bay bay) : IDisposable
{
    private readonly Bay _bay = bay;
    private readonly int _stripHeight = 50;
    private SKControl? _skControl;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Setup()
    {
        _skControl = new SKControl();
        _skControl.Size = new System.Drawing.Size(10, 1);
        _skControl.PaintSurface += Paint;
        _skControl.Name = "StripBoard";
        _skControl.BackColor = Color.Wheat;
        _skControl.Dock = DockStyle.Top;
        _bay.ChildPanel.ChildPanel.Controls.Add(_skControl);
        _skControl.Show();
    }

    public void SetHeight()
    {
        if (_skControl is null)
        {
            return;
        }

        var y = _bay.Strips.Count * _stripHeight;
        _skControl.Size = new Size(_skControl.Size.Width, y);
    }

    private void Paint(object sender, SKPaintSurfaceEventArgs e)
    {
        if (_skControl is null)
        {
            return;
        }

        var canvas = e.Surface.Canvas;

        // make sure the canvas is blank
        canvas.Clear(SKColors.White);

        // draw some text
        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
        };
        using var font = new SKFont
        {
            Size = 24,
        };
        var coord = new SKPoint(e.Info.Width / 2, (e.Info.Height + font.Size) / 2);
        canvas.DrawText("SkiaSharp", coord, SKTextAlign.Center, font, paint);
    }
}
