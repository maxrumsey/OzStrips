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
    public const int StripHeight = 50;
    public const int StripWidth = 300;

    private readonly Bay _bay = bay;
    private readonly StripElementList? _striplist = StripElementList.Deserialize("C:/Users/exiflame/Desktop/List.xml");
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

        var y = _bay.Strips.Count * StripHeight;
        _skControl.Size = new Size(_skControl.Size.Width, y);
        var z = _striplist;
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

        for (var i = 0; i < _bay.Strips.Count; i++)
        {
            var stripView = _bay.Strips[i].StripView;

            if (stripView is not null)
            {
                stripView.Origin = new SKPoint(0, i * StripHeight);
                stripView.Render(canvas);
            }
        }
    }
}
