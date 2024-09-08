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

    public Bay Bay { get; } = bay;

    public ToolTip? ToolTip { get; private set; }

    internal SKControl? SkControl { get; private set; }

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

        SkControl = new SKControl();
        SkControl.Size = new System.Drawing.Size(10, 1);
        SkControl.PaintSurface += Paint;
        SkControl.Click += Click;
        SkControl.DoubleClick += Click;
        SkControl.MouseMove += Hover;
        SkControl.Name = "StripBoard";
        SkControl.BackColor = Color.Wheat;
        SkControl.Dock = DockStyle.Bottom;
        Bay.ChildPanel.ChildPanel.Controls.Add(SkControl);
        SkControl.Show();

        ToolTip = new ToolTip();
    }

    public void SetHeight()
    {
        if (SkControl is null)
        {
            return;
        }

        var y = Bay.Strips.Count * StripHeight;
        SkControl.Size = new Size(SkControl.Size.Width, y);
    }

    public void Redraw()
    {
        SkControl?.Refresh();
    }

    private void Paint(object sender, SKPaintSurfaceEventArgs e)
    {
        if (SkControl is null)
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
        var strip = DetermineStripAtPos(args.Y);

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

    private void Hover(object sender, EventArgs e)
    {
        var point = SkControl?.PointToClient(Cursor.Position);
        if (point is null)
        {
            return;
        }

        var strip = DetermineStripAtPos(point.Value.Y);

        if (strip is not null)
        {
            strip.RenderedStripItem?.HandleHover(point.Value);
        }
    }

    private StripListItem? DetermineStripAtPos(int y)
    {
        var total = Bay.Strips.Count - 1;
        var i = y / StripHeight;
        return Bay.Strips.ElementAtOrDefault(total - i);
    }
}
