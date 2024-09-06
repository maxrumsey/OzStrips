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
    public const int StripHeight = 64;
    public const int StripWidth = 420;
    public const int CockOffset = 30;
    private SKControl? _skControl;

    public Bay Bay { get; } = bay;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Setup()
    {
        if (StripElementList.Instance is null)
        {
            StripElementList.Load();
        }

        _skControl = new SKControl();
        _skControl.Size = new System.Drawing.Size(10, 1);
        _skControl.PaintSurface += Paint;
        _skControl.Click += Click;
        _skControl.DoubleClick += Click;
        _skControl.Name = "StripBoard";
        _skControl.BackColor = Color.Wheat;
        _skControl.Dock = DockStyle.Top;
        Bay.ChildPanel.ChildPanel.Controls.Add(_skControl);
        _skControl.Show();
    }

    public void SetHeight()
    {
        if (_skControl is null)
        {
            return;
        }

        var y = Bay.Strips.Count * StripHeight;
        _skControl.Size = new Size(_skControl.Size.Width, y);
    }

    public void Redraw()
    {
        _skControl?.Refresh();
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
        var total = Bay.Strips.Count - 1;
        for (var i = total; i >= 0; i--)
        {
            var stripView = Bay.Strips[i].RenderedStripItem;

            if (stripView is not null)
            {
                var strip = Bay.Strips[i]?.StripController;
                var cocked = false;
                if (strip is not null && strip.CockLevel == 1)
                {
                    cocked = true;
                }

                stripView.Origin = new SKPoint(cocked ? CockOffset : 0, (total - i) * StripHeight);
                stripView.Render(canvas);
            }
        }

        canvas.Flush();
    }

    private void Click(object sender, EventArgs e)
    {
        var args = (MouseEventArgs)e;
        var total = Bay.Strips.Count - 1;
        var i = args.Y / StripHeight;
        var strip = Bay.Strips.ElementAtOrDefault(total - i);

        if (strip is not null)
        {
            strip.RenderedStripItem?.HandleClick(args);
        }
        else
        {
            Bay.BayManager.DropStrip(Bay);
        }

        Redraw();
    }
}
