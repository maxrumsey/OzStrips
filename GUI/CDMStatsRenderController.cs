using MaxRumsey.OzStripsPlugin.GUI;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.Properties;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.Windows;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.GUI;

internal class CDMStatsRenderController : IDisposable
{
    private readonly BayManager _bayManager;
    private readonly SocketConn _socketConn;
    private const int WIDTH = 400;
    private const int HEIGHT = 34;
    private const int FONTSIZE = 15;

    private readonly Timer _renderTimer;

    public CDMStatsRenderController(BayManager bayManager, Control parent, SocketConn socketConn)
    {
        socketConn.AerodromeStateChanged += StateChanged;
        _socketConn = socketConn;

        var parentsize = parent.Size;
        _bayManager = bayManager;
        _renderTimer = new();
        _renderTimer.Tick += RerenderTriggered;
        _renderTimer.Interval = 1000;
        _renderTimer.Start();

        SkControl = new()
        {
            Size = new System.Drawing.Size(WIDTH, HEIGHT),
            Location = new System.Drawing.Point(parentsize.Width - WIDTH - 1, 1),
            Anchor = AnchorStyles.Right | AnchorStyles.Top,
        };
        SkControl.PaintSurface += Paint;
        SkControl.Name = "CDMStats";
        SkControl.BackColor = Color.Black;

        SkControl.Show();
        parent.Controls.Add(SkControl);
        SkControl.BringToFront();
    }

    public SKControl SkControl { get; private set; }

    private int WidthEach => RatePeriods.Length > 0 ? WIDTH / RatePeriods.Length : WIDTH;

    private int[] RatePeriods => _bayManager.AerodromeState.CDMStatistics.CDMRates.Keys.ToArray();

    public void Dispose()
    {
        _renderTimer.Dispose();
        _socketConn.AerodromeStateChanged -= StateChanged;
    }

    private void StateChanged(object sender, EventArgs e)
    {
        MMI.InvokeOnGUI(SkControl.Invalidate);
    }

    private void RerenderTriggered(object sender, EventArgs e)
    {
        SkControl?.Invalidate();
    }

    private void Paint(object sender, SKPaintSurfaceEventArgs e)
    {
        try
        {
            var textPaint = new SKPaint()
            {
                Color = SKColors.White,
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
            };

            var canvas = e.Surface.Canvas;

            // make sure the canvas is blank
            canvas.Clear(SKColors.Black);

            if (_bayManager.AerodromeState.CDMParameters?.Enabled != true)
            {
                canvas.Flush();
                return;
            }

            var font = new SKFont(SKTypeface.FromFamilyName("Segoe UI", 700, 5, SKFontStyleSlant.Upright), FONTSIZE);

            var offset = 0;
            foreach (var period in RatePeriods)
            {
                var backPaint = new SKPaint()
                {
                    Style = SKPaintStyle.StrokeAndFill,
                    Color = SKColors.Red,
                };

                var textLoc = new SKPoint(offset + (WidthEach / 2), (FONTSIZE + HEIGHT) / 2);

                var actual = _bayManager.AerodromeState.CDMStatistics.CDMRates[period];
                var depRate = 60 / ((float)(_bayManager.AerodromeState.CDMParameters.DefaultRate ?? TimeSpan.FromMinutes(2)).TotalMinutes);
                var goalAmount = (int)(depRate * (period / 60f));

                var discrepancy = actual - goalAmount;

                var colour = GetColour(discrepancy);

                backPaint.Color = colour;
                canvas.DrawRect(offset, 0, WidthEach, HEIGHT, backPaint);

                canvas.DrawText($"{period}m: {actual}/{goalAmount}", textLoc, SKTextAlign.Center, font, textPaint);

                offset += WidthEach;
            }

            canvas.Flush();
        }
        catch (Exception ex)
        {
            Util.LogError(ex, "Ozstrips Renderer");
        }
    }

    /// <summary>
    /// Gets the background colour.
    /// </summary>
    /// <param name="discrepancy">Number of aircraft over/under goal.</param>
    /// <returns>Colour.</returns>
    private static SKColor GetColour(int discrepancy)
    {
        if (discrepancy == 0)
        {
            return SKColors.Green;
        }
        else if (discrepancy is > 0 and <= 2)
        {
            return SKColors.OrangeRed;
        }
        else if (discrepancy >= 3)
        {
            return SKColors.Red;
        }
        else if (discrepancy is < 0 and >= -2)
        {
            return SKColors.Blue;
        }
        else
        {
            return SKColors.Purple;
        }
    }
}
