using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui;
using MaxRumsey.OzStripsPlugin.Gui.Properties;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using vatsys;

namespace MaxRumsey.OzStripsPlugin;

internal class BayRenderController(Bay bay) : IDisposable
{
    public const int StripHeight = 64;
    public const int BarHeight = 30;
    public const int StripWidth = 420;
    public const int CockOffset = 30;

    public static float Scale => OzStripsSettings.Default.StripScale;

    public StripElements.HoverActions? HoveredItem { get; set; }

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

        SkControl = new()
        {
            Size = new System.Drawing.Size(10, 1),
        };
        SkControl.PaintSurface += Paint;
        SkControl.Click += Click;
        SkControl.DoubleClick += Click;
        SkControl.MouseMove += Hover;
        SkControl.Name = "StripBoard";
        SkControl.BackColor = Color.Wheat;
        SkControl.Dock = DockStyle.Top;

        // SkControl.MouseMove += MouseMoved;
        Bay.ChildPanel.ChildPanel.Controls.Add(SkControl);
        SkControl.Show();

        ToolTip = new ToolTip();
    }

    private void MouseMoved(object sender, MouseEventArgs e)
    {
        SkControl?.Focus();
    }

    public void SetHeight()
    {
        if (SkControl is null)
        {
            return;
        }

        var y = GetHeightPreScale();

        SkControl.Size = new Size(SkControl.Size.Width, (int)(y * Scale));
    }

    public int GetHeightPreScale()
    {
        var y = 0;

        foreach (var item in Bay.Strips)
        {
            if (item.Type == Gui.StripItemType.STRIP)
            {
                y += StripHeight;
            }
            else
            {
                y += BarHeight;
            }
        }

        return y;
    }

    public int GetStripPosPreScale(Strip strip)
    {
        var y = 0;
        var list = Bay.Strips.ToList();
        list.Reverse();
        foreach (var item in list)
        {
            if (item.Type == Gui.StripItemType.STRIP)
            {
                if (item.Strip == strip)
                {
                    return y;
                }

                y += StripHeight;
            }
            else
            {
                y += BarHeight;
            }
        }

        return y;
    }

    public void Redraw()
    {
        SkControl?.Refresh();
    }

    private void Paint(object sender, SKPaintSurfaceEventArgs e)
    {
        try
        {
            if (SkControl is null)
            {
                return;
            }

            var canvas = e.Surface.Canvas;

            canvas.Scale(OzStripsSettings.Default.StripScale);

            // make sure the canvas is blank
            canvas.Clear(SKColor.Parse("404040"));
            var total = Bay.Strips.Count - 1;
            var y = 0;
            for (var i = total; i >= 0; i--)
            {
                if (Bay.Strips[i].Type == Gui.StripItemType.QUEUEBAR && Bay.Strips[i].BarText is not null)
                {
                    var count = 0;
                    for (var j = i; j >= 0; j--)
                    {
                        if (Bay.Strips[j].Type == Gui.StripItemType.STRIP)
                        {
                            count++;
                        }
                    }

                    Bay.Strips[i].BarText = $"Queue ({count})";
                }

                var stripView = Bay.Strips[i].RenderedStripItem;

                if (stripView is not null)
                {
                    var strip = Bay.Strips[i]?.Strip;
                    var cocked = false;
                    if (strip?.CockLevel == 1)
                    {
                        cocked = true;
                    }

                    stripView.Origin = new SKPoint(cocked ? CockOffset : 0, y);
                    try
                    {
                        stripView.Render(canvas);
                    }
                    catch (Exception ex)
                    {
                        Util.LogError(ex, $"Ozstrips Renderer - Strip {Bay.Strips[i]?.Strip?.FDR.Callsign}");
                    }
                }

                y += Bay.Strips[i].Type == Gui.StripItemType.STRIP ? StripHeight : BarHeight;
            }

            canvas.Flush();
        }
        catch (Exception ex)
        {
            Util.LogError(ex, "Ozstrips Renderer");
        }
    }

    private void Click(object sender, EventArgs e)
    {
        var args = (MouseEventArgs)e;
        var strip = DetermineStripAtPos((int)(args.Y / Scale));

        if (strip is not null)
        {
            args = new MouseEventArgs(args.Button, args.Clicks, (int)(args.X / Scale), (int)(args.Y / Scale), args.Delta);
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

        point = new Point((int)(point.Value.X / Scale), (int)(point.Value.Y / Scale));

        var strip = DetermineStripAtPos(point.Value.Y);

        strip?.RenderedStripItem?.HandleHover(point.Value);
    }

    private StripListItem? DetermineStripAtPos(int y)
    {
        var total = Bay.Strips.Count - 1;
        var jy = 0;
        for (var i = total; i >= 0; i--)
        {
            var y_offset = BarHeight;
            if (Bay.Strips[i].Type == Gui.StripItemType.STRIP)
            {
                y_offset = StripHeight;
            }

            if (jy <= y && y < (jy + y_offset))
            {
                return Bay.Strips[i];
            }

            jy += y_offset;
        }

        return null;
    }
}
