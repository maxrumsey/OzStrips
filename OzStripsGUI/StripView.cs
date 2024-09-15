using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;
using vatsys;
using static vatsys.FDP2;

namespace MaxRumsey.OzStripsPlugin.Gui;

internal class StripView(Strip strip, BayRenderController bayRC) : IRenderedStripItem
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
                Color = GetElementForeColour(element),
                Style = SKPaintStyle.StrokeAndFill,
                IsAntialias = true,
            };

            var basepaint = new SKPaint()
            {
                Color = GetElementBackColour(element),
                Style = SKPaintStyle.StrokeAndFill,
            };

            var baseX = ElementOrigin.X + element.X;
            var baseY = ElementOrigin.Y + element.Y;

            var text = GetElementText(element);
            var fontsize = element.FontSize;

            var typeface = SKTypeface.FromFamilyName("Segoe UI", 700, 5, SKFontStyleSlant.Upright);

            canvas.DrawRect(baseX, baseY, element.W, element.H, basepaint);
            canvas.DrawRect(baseX, baseY, element.W, element.H, elpaint);

            canvas.DrawText(text, new SKPoint(baseX + (element.W / 2), baseY + ((fontsize + element.H) / 2)), SKTextAlign.Center, new SKFont(typeface, fontsize), textpaint);
        }
    }

    public void MarkPicked(bool picked)
    {
        _strip.SetHMIPicked(picked);
    }

    public void HandleClick(MouseEventArgs e)
    {
        var stripelementlist = StripElementList.Instance?.List;
        if (stripelementlist is null)
        {
            return;
        }

        StripElement? clicked = null;
        foreach (var element in stripelementlist)
        {
            if (element is not null &&
                (element.X + ElementOrigin.X) <= e.X && e.X < (element.W + element.X + ElementOrigin.X) &&
                (element.Y + ElementOrigin.Y) <= e.Y && e.Y < (element.Y + element.H + ElementOrigin.Y))
            {
                clicked = element;
            }
        }

        if (clicked is not null)
        {
            var action = clicked.LeftClick;
            if (e.Button == MouseButtons.Right)
            {
                action = clicked.RightClick;
            }

            HandleClickAction(action);
        }
        else
        {
            _bayRenderController.Bay.BayManager.DropStrip(_bayRenderController.Bay);
        }
    }

    public void HandleHover(Point e)
    {
        var stripelementlist = StripElementList.Instance?.List;
        if (stripelementlist is null)
        {
            return;
        }

        var flag = false;

        StripElement? hovered = null;
        foreach (var element in stripelementlist)
        {
            if (element is not null && element.Hover is not StripElements.HoverActions.NONE &&
                (element.X + ElementOrigin.X) <= e.X && e.X < (element.W + element.X + ElementOrigin.X) &&
                (element.Y + ElementOrigin.Y) <= e.Y && e.Y < (element.Y + element.H + ElementOrigin.Y))
            {
                hovered = element;
            }
        }

        if (hovered is not null && hovered.Hover != StripElements.HoverActions.NONE)
        {
            flag = true;
        }

        // todo: make less hacky
        if (hovered is not null && _bayRenderController.ToolTip is not null)
        {
            switch (hovered.Hover)
            {
                case StripElements.HoverActions.ROUTE_WARNING:
                    if (_bayRenderController.HoveredItem != StripElements.HoverActions.ROUTE_WARNING)
                    {
                        _bayRenderController.HoveredItem = StripElements.HoverActions.ROUTE_WARNING;

                        string str;

                        if (_strip.DodgyRoute)
                        {
                            var routes = new List<string>();
                            Array.ForEach(_strip.ValidRoutes, x => routes.Add("(" + x.acft + ") " + x.route));
                            str = _strip.Route +
                                "\n---\nPotentially non-compliant route detected! Accepted Routes:\n" + string.Join("\n", routes) + "\nParsed Route: " + _strip.CondensedRoute;
                        }
                        else
                        {
                            str = _strip.Route;
                        }

                        _bayRenderController.ToolTip.Show(str, _bayRenderController.SkControl, e);
                    }

                    break;
                case StripElements.HoverActions.RFL_WARNING:
                    if (_bayRenderController.HoveredItem != StripElements.HoverActions.RFL_WARNING && _strip.Controller.ShowCFLToolTip)
                    {
                        _bayRenderController.HoveredItem = StripElements.HoverActions.RFL_WARNING;
                        _bayRenderController.ToolTip.Show("Potentially non-compliant filed cruise level detected.", _bayRenderController.SkControl, e);
                    }

                    break;
            }
        }

        if (!flag && _bayRenderController.ToolTip is not null)
        {
            _bayRenderController.HoveredItem = null;
            _bayRenderController.ToolTip.RemoveAll();
            _bayRenderController.ToolTip.Hide(_bayRenderController.SkControl);
        }
    }

    private void HandleClickAction(StripElements.Actions action)
    {
        switch (action)
        {
            case StripElements.Actions.SHOW_ROUTE:
                var track = MMI.FindTrack(_strip.FDR);
                if (track is not null)
                {
                    if (track.GraphicRTE)
                    {
                        MMI.HideGraphicRoute(track);
                    }
                    else
                    {
                        MMI.ShowGraphicRoute(track);
                    }
                }

                break;
            case StripElements.Actions.OPEN_FDR:
                _strip.OpenVatsysFDR();
                break;
            case StripElements.Actions.PICK:
                _strip.TogglePick();
                break;
            case StripElements.Actions.ASSIGN_SSR:
                _strip.Controller.AssignSSR();
                break;
            case StripElements.Actions.MOD_CLX:
                _strip.Controller.OpenCLXBayModal("clx");
                break;
            case StripElements.Actions.MOD_STD:
                _strip.Controller.OpenCLXBayModal("std");
                break;
            case StripElements.Actions.MOD_GLOP:
                _strip.Controller.OpenCLXBayModal("glop");
                break;
            case StripElements.Actions.MOD_REMARK:
                _strip.Controller.OpenCLXBayModal("remark");
                break;
            case StripElements.Actions.MOD_CFL:
                _strip.Controller.OpenCFLWindow();
                break;
            case StripElements.Actions.MOD_RWY:
                _strip.Controller.OpenRWYWindow();
                break;
            case StripElements.Actions.MOD_SID:
                _strip.Controller.OpenSIDWindow();
                break;
            case StripElements.Actions.OPEN_HDG_ALT:
                _strip.Controller.OpenHDGWindow();
                break;
            case StripElements.Actions.OPEN_REROUTE:
                _strip.Controller.OpenRerouteMenu();
                break;
            case StripElements.Actions.SET_READY:
                _strip.Controller.ToggleReady();
                break;
            case StripElements.Actions.SET_TOT:
                _strip.TakeOff();
                break;
            case StripElements.Actions.COCK:
                _strip.CockStrip();
                break;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Necessary.")]
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
            case StripElements.Values.ADES:
                return _strip.FDR.DesAirport;
            case StripElements.Values.ROUTE:
                return "R";
            case StripElements.Values.FRUL:
                return _strip.FDR.FlightRules;
            case StripElements.Values.SSR_SYMBOL:
                return _strip.SquawkCorrect ? "*" : string.Empty;
            case StripElements.Values.TYPE:
                return _strip.FDR.AircraftType;
            case StripElements.Values.WTC:
                return _strip.FDR.AircraftWake;
            case StripElements.Values.RWY:
                return _strip.RWY;
            case StripElements.Values.READY:
                return _strip.Ready ? "RDY" : string.Empty;
            case StripElements.Values.CLX:
                return _strip.CLX;
            case StripElements.Values.SID:
                return _strip.SID;
            case StripElements.Values.FIRST_WPT:
                if (_strip.FirstWpt.Length > 5)
                {
                    return _strip.FirstWpt.Substring(0, 5) + "...";
                }
                else
                {
                    return _strip.FirstWpt;
                }

            case StripElements.Values.RFL:
                return _strip.RFL;
            case StripElements.Values.CFL:
                return _strip.CFL;
            case StripElements.Values.STAND:
                return _strip.Gate;
            case StripElements.Values.GLOP:
                return _strip.FDR.GlobalOpData;
            case StripElements.Values.REMARK:
                return _strip.Remark;
            case StripElements.Values.HDG:
                return _strip.HDG;
            case StripElements.Values.TOT:
                if (_strip.TakeOffTime != null)
                {
                    var diff = (TimeSpan)(DateTime.UtcNow - _strip.TakeOffTime);
                    return diff.ToString(@"mm\:ss", CultureInfo.InvariantCulture);
                }
                else
                {
                    return "00:00";
                }

            default:
                break;
        }

        return string.Empty;
    }

    private SKColor GetElementBackColour(StripElement element)
    {
        switch (element.Value)
        {
            case StripElements.Values.ACID:
                if (_bayRenderController.Bay.BayManager.PickedController == _strip)
                {
                    return SKColors.Silver;
                }

                break;
            case StripElements.Values.STAND:
            case StripElements.Values.ADES:
            case StripElements.Values.EOBT:
            case StripElements.Values.WTC:
            case StripElements.Values.ROUTE:
            case StripElements.Values.FRUL:
            case StripElements.Values.SSR_SYMBOL:
            case StripElements.Values.TYPE:
            case StripElements.Values.SSR:
                if (_strip.CockLevel == 1)
                {
                    return SKColors.Cyan;
                }

                break;
            case StripElements.Values.SID:
                return SKColors.Green;
            case StripElements.Values.CFL:
                return _strip.Controller.DetermineCFLBackColour();
            case StripElements.Values.FIRST_WPT:
                return _strip.Controller.DetermineRouteBackColour();
            case StripElements.Values.READY:
                var colour = SKColor.Empty;
                if (!_strip.Ready && (_strip.CurrentBay == StripBay.BAY_HOLDSHORT || _strip.CurrentBay == StripBay.BAY_RUNWAY) && _strip.ArrDepType == StripArrDepType.DEPARTURE)
                {
                    colour = SKColors.Orange;
                }

                return colour;
        }

        return SKColor.Empty;
    }

    private SKColor GetElementForeColour(StripElement element)
    {
        switch (element.Value)
        {
            case StripElements.Values.TOT:
                if (_strip.TakeOffTime is not null)
                {
                    return SKColors.Green;
                }

                break;
            case StripElements.Values.RFL:
                return SKColors.Gray;
        }

        return SKColors.Black;
    }

    private void DrawStripBackground(SKCanvas canvas)
    {
        var paint = new SKPaint()
        {
            Color = (_strip.ArrDepType == StripArrDepType.ARRIVAL) ? SKColor.Parse("ffffa0") : SKColor.Parse("c1e6f2"),
            Style = SKPaintStyle.Fill,
        };

        canvas.DrawRect(Origin.X, Origin.Y, BayRenderController.StripWidth + (2 * _padding), BayRenderController.StripHeight, paint);
    }
}
