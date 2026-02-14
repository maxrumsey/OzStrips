using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using MaxRumsey.OzStripsPlugin.GUI.DTO;
using MaxRumsey.OzStripsPlugin.GUI.Properties;
using MaxRumsey.OzStripsPlugin.GUI.Shared;
using OpenTK.Graphics;
using SkiaSharp;
using vatsys;
using static System.Net.Mime.MediaTypeNames;

namespace MaxRumsey.OzStripsPlugin.GUI;

internal class StripView(Strip strip, BayRenderController bayRC) : IRenderedStripItem
{
    private readonly Strip _strip = strip;
    private readonly BayRenderController _bayRenderController = bayRC;
    private readonly int _padding = 2;
    private readonly byte _lastTransmitAlpha = 64;

    private static bool LastTransmitModifier => Keyboard.IsKeyDown(KeybindManager.ActiveKeybinds[KeybindManager.KEYBINDS.LAST_TRANSMIT_HIGHLIGHT]);

    /// <summary>
    /// Gets or sets the root of the strip (taking into account strip cocking). The strip background is drawn from this point.
    /// </summary>
    public SKPoint Origin { get; set; } = new SKPoint(0, 0);

    /// <summary>
    /// Gets or sets the root of the strip elements (black border).
    /// </summary>
    public SKPoint ElementOrigin { get; set; } = new SKPoint(0, 0);

    public void Render(SKCanvas canvas)
    {
        ElementOrigin = new SKPoint(Origin.X + _padding, Origin.Y + _padding);

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

        canvas.DrawRect(ElementOrigin.X, ElementOrigin.Y, BayRenderController.StripWidth, BayRenderController.StripHeight - (2 * _padding), bdrypaint);

        foreach (var element in stripelementlist)
        {
            if (element is null)
            {
                continue;
            }

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

            if (LastTransmitModifier && _bayRenderController.Bay.BayManager.LastTransmitManager.LastReceivedFrom != _strip.FDR.Callsign)
            {
                textpaint.Color = textpaint.Color.WithAlpha(_lastTransmitAlpha);
            }

            var basepaint = new SKPaint()
            {
                Color = GetElementBackColour(element),
                Style = SKPaintStyle.StrokeAndFill,
            };

            var highlightPaint = element.Value == StripElements.Values.SID ? new SKPaint()
            {
                Color = SKColors.Yellow,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2,
            }
            : null;

            var baseX = ElementOrigin.X + element.X;
            var baseY = ElementOrigin.Y + element.Y;

            var text = GetElementText(element);
            var fontsize = element.FontSize;

            var typeface = SKTypeface.FromFamilyName("Segoe UI", 700, 5, SKFontStyleSlant.Upright);

            canvas.DrawRect(baseX, baseY, element.W, element.H, basepaint);
            canvas.DrawRect(baseX, baseY, element.W, element.H, elpaint);

            if (_strip.SIDTransition is not null && highlightPaint is not null)
            {
                canvas.DrawRect(baseX + 1.5f, baseY + 1.5f, element.W - 2.5f, element.H - 2.5f, highlightPaint);
            }

            canvas.DrawText(text, new SKPoint(baseX + (element.W / 2), baseY + ((fontsize + element.H) / 2)), SKTextAlign.Center, new SKFont(typeface, fontsize), textpaint);
        }

        if (_bayRenderController.Bay.BayManager.AerodromeState.InvalidDestinationAircraft.Contains(_strip.FDR.Callsign))
        {
            var crossPaint = new SKPaint()
            {
                Color = SKColors.Red,
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
            };

            var textpaint = new SKPaint()
            {
                Color = SKColors.Red,
                Style = SKPaintStyle.StrokeAndFill,
                IsAntialias = true,
            };

            var typeface = SKTypeface.FromFamilyName("Segoe UI", 700, 5, SKFontStyleSlant.Upright);
            var fontsize = 30;
            textpaint.Color = textpaint.Color.WithAlpha(96);
            crossPaint.Color = crossPaint.Color.WithAlpha(128);

            canvas.DrawLine(ElementOrigin.X, ElementOrigin.Y, ElementOrigin.X + BayRenderController.StripWidth, ElementOrigin.Y + BayRenderController.StripHeight - (2 * _padding), crossPaint);
            canvas.DrawText("NO SLOT", new SKPoint(ElementOrigin.X + (BayRenderController.StripWidth / 2), ElementOrigin.Y + ((fontsize + BayRenderController.StripHeight) / 2)), SKTextAlign.Center, new SKFont(typeface, fontsize), textpaint);
        }
    }

    public void MarkPicked(bool picked)
    {
        _strip.SetHMIPicked(picked);
    }

    public void HandleClick(System.Windows.Forms.MouseEventArgs e)
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
            else if (e.Button == MouseButtons.Middle)
            {
                action = clicked.MiddleClick;
            }

            HandleClickAction(action);
        }
        else
        {
            // If no strip element was clicked, treat as a strip being dropped onto the bay.
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

        /*
         * Get strip element the mouse cursor is on right now
         */
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

        /*
         * Set flag if the strip element will not trigger a tooltip
         */
        if (hovered is not null && hovered.Hover != StripElements.HoverActions.NONE)
        {
            flag = true;
        }

        // todo: make less hacky
        /*
         * Tooltip triggered below
         */
        if (hovered is not null && _bayRenderController.ToolTip is not null)
        {
            switch (hovered.Hover)
            {
                case StripElements.HoverActions.ROUTE_WARNING:
                    if (_bayRenderController.HoveredItem != StripElements.HoverActions.ROUTE_WARNING)
                    {
                        _bayRenderController.HoveredItem = StripElements.HoverActions.ROUTE_WARNING;

                        string str;

                        if (_strip.IsAlertActive(Shared.AlertTypes.ROUTE))
                        {
                            var routes = new List<string>();
                            Array.ForEach(_strip.ValidRoutes, x => routes.Add("(" + x.AircraftType + ") " + x.RouteText));
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
                    if (_bayRenderController.HoveredItem != StripElements.HoverActions.RFL_WARNING && (_strip.IsAlertActive(Shared.AlertTypes.RFL) || _strip.CFL.Contains("B")))
                    {
                        _bayRenderController.HoveredItem = StripElements.HoverActions.RFL_WARNING;
                        _bayRenderController.ToolTip.Show(
                       (
                        (_strip.CFL.Contains("B") ? (_strip.CFL + "\n") : string.Empty) +
                        (_strip.IsAlertActive(Shared.AlertTypes.RFL) ? "Potentially non-compliant filed cruise level detected." : string.Empty)).Trim(),
                       _bayRenderController.SkControl,
                       e);
                    }

                    break;
                case StripElements.HoverActions.SSR_WARNING:
                    if (_bayRenderController.HoveredItem != StripElements.HoverActions.SSR_WARNING &&
                    _strip.IsAlertActive(Shared.AlertTypes.SSR))
                    {
                        _bayRenderController.HoveredItem = StripElements.HoverActions.SSR_WARNING;
                        _bayRenderController.ToolTip.Show("Incorrect SSR Code or Mode.", _bayRenderController.SkControl, e);
                    }

                    break;
                case StripElements.HoverActions.SID_TRIGGER:
                    var transExists = _strip.SIDTransition?.Length > 0;
                    if (_bayRenderController.HoveredItem != StripElements.HoverActions.SID_TRIGGER &&
                        (transExists || _strip.IsAlertActive(Shared.AlertTypes.VFR_SID)))
                    {
                        _bayRenderController.HoveredItem = StripElements.HoverActions.SID_TRIGGER;
                        _bayRenderController.ToolTip.Show((transExists ? _strip.SIDTransition + " Transition\n" : string.Empty) + (_strip.IsAlertActive(Shared.AlertTypes.VFR_SID) ? "VFR Aircraft issued a SID." : string.Empty), _bayRenderController.SkControl, e);
                    }

                    break;
            }
        }

        /*
         * Remove tooltip if no tooltip is required at the moment.
         */
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
            case StripElements.Actions.SID_TRIGGER:
                _strip.SIDTrigger();
                break;
            case StripElements.Actions.PICK:
                if (MainFormController.ControlHeld)
                {
                    _bayRenderController.Bay.BayManager.DropStripBelow(_bayRenderController.Bay.Strips.First(x => x?.Strip == _strip));
                    break;
                }

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
                _strip.Controller.OpenCLXBayModal("freq");
                break;
            case StripElements.Actions.OPEN_REROUTE:
                _strip.Controller.OpenRerouteMenu();
                break;
            case StripElements.Actions.OPEN_CDM:
                _strip.Controller.OpenCDM();
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
            case StripElements.Actions.OPEN_PDC:
                if (_strip.PDCRequest?.Flags.HasFlag(PDCRequest.PDCFlags.REQUESTED) == true && _strip.StripType != StripType.ARRIVAL)
                {
                    if (!_strip.PDCRequest.Flags.HasFlag(PDCRequest.PDCFlags.ACKNOWLEDGED))
                    {
                        _strip.PDCFlags |= PDCRequest.PDCFlags.ACKNOWLEDGED;
                        _strip.SyncStrip();
                    }

                    _strip.Controller.OpenPDCWindow();
                    break;
                }

                _strip.Controller.OpenVatSysPDCWindow();
                break;
            case StripElements.Actions.OPEN_PM:
                MMI.OpenPMWindow(_strip.FDR.Callsign);
                break;
            case StripElements.Actions.INHIBIT_ROUTE:
                if (_strip.IsAlertActive(Shared.AlertTypes.ROUTE))
                {
                    _strip.InhibitAlert(Shared.AlertTypes.ROUTE);
                }

                break;
            case StripElements.Actions.INHIBIT_RFL:
                if (_strip.IsAlertActive(Shared.AlertTypes.RFL))
                {
                    _strip.InhibitAlert(Shared.AlertTypes.RFL);
                }

                break;
            case StripElements.Actions.INHIBIT_SSR:
                if (_strip.IsAlertActive(Shared.AlertTypes.SSR))
                {
                    _strip.InhibitAlert(Shared.AlertTypes.SSR);
                }

                break;
            case StripElements.Actions.INHIBIT_SID:
                if (_strip.IsAlertActive(Shared.AlertTypes.VFR_SID))
                {
                    _strip.InhibitAlert(Shared.AlertTypes.VFR_SID);
                }

                break;
            case StripElements.Actions.INHIBIT_READY:
                if (_strip.IsAlertActive(Shared.AlertTypes.READY))
                {
                    _strip.InhibitAlert(Shared.AlertTypes.READY);
                }

                break;
            case StripElements.Actions.INHIBIT_HDG:
                if (_strip.IsAlertActive(Shared.AlertTypes.NO_HDG))
                {
                    _strip.InhibitAlert(Shared.AlertTypes.NO_HDG);
                }

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
                if (_strip.FDR.TextOnly)
                {
                    return "T";
                }
                else if (_strip.FDR.ReceiveOnly)
                {
                    return "R";
                }

                return string.Empty;

            case StripElements.Values.FRUL:
                return _strip.FDR.FlightRules;
            case StripElements.Values.PDC_INDICATOR:
                return _strip.FDR.PDCSent || _strip.PDCFlags.HasFlag(PDCRequest.PDCFlags.SENT) ? "P" : string.Empty;
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
            case StripElements.Values.DEPFREQ:
                return _strip.DepartureFrequency;
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
                if (_strip.CFL.Contains("B"))
                {
                    return "BLK";
                }

                return _strip.CFL;
            case StripElements.Values.STAND:
                if (!string.IsNullOrEmpty(_strip.AllocatedBay) && string.IsNullOrEmpty(_strip.Gate))
                {
                    return _strip.AllocatedBay;
                }

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
                if (_bayRenderController.Bay.BayManager.PickedStrip == _strip)
                {
                    return SKColors.Silver;
                }
                else if (_bayRenderController.Bay.BayManager.LastTransmitManager.LastReceivedFrom == _strip.FDR.Callsign &&
                    OzStripsSettings.Default.ShowLastTransmit)
                {
                    return SKColors.Lime;
                }
                else if (_bayRenderController.Bay.BayManager.AerodromeState.WorldFlightTeams.Contains(_strip.FDR.Callsign))
                {
                    return SKColors.Yellow;
                }

                break;
            case StripElements.Values.STAND:
            case StripElements.Values.ADES:
            case StripElements.Values.EOBT:
            case StripElements.Values.WTC:
            case StripElements.Values.ROUTE:
            case StripElements.Values.FRUL:
            case StripElements.Values.PDC_INDICATOR:
            case StripElements.Values.TYPE:
            case StripElements.Values.SSR:
                if (element.Value == StripElements.Values.STAND && !string.IsNullOrEmpty(_strip.AllocatedBay) && string.IsNullOrEmpty(_strip.Gate))
                {
                    return SKColors.LightGray;
                }

                if (element.Value == StripElements.Values.PDC_INDICATOR && _strip.StripType != StripType.ARRIVAL)
                {
                    var requestedPDC = _strip.PDCRequest;

                    if (requestedPDC is not null && requestedPDC.Flags.HasFlag(PDCRequest.PDCFlags.REQUESTED) && !_strip.PDCFlags.HasFlag(PDCRequest.PDCFlags.SENT))
                    {
                        if (DateTime.Now.Second % 2 == 0 && !_strip.PDCFlags.HasFlag(PDCRequest.PDCFlags.ACKNOWLEDGED))
                        {
                            return SKColors.White;
                        }

                        return SKColors.Yellow;
                    }
                }

                /*
                * Incorrect SSR Code & Mode C alert
                */
                if ((element.Value == StripElements.Values.SSR) && _strip.IsAlertActive(Shared.AlertTypes.SSR))
                {
                    return SKColors.Orange;
                }

                if ((element.Value == StripElements.Values.EOBT) &&
                    _strip.CDMResult is not null &&
                    _strip.CDMResult.Aircraft.State != CDMState.PUSHED)
                {
                    if (_strip.ReadyToPush)
                    {
                        return SKColors.LimeGreen;
                    }

                    if (_strip.CDMResult.Slot is not null)
                    {
                        return SKColors.Pink;
                    }

                    return SKColors.LightGray;
                }

                if (_strip.CockLevel == 1)
                {
                    return SKColors.Cyan;
                }

                break;
            case StripElements.Values.GLOP:
            case StripElements.Values.REMARK:
            case StripElements.Values.DEPFREQ:
            case StripElements.Values.HDG:
                /*
                 * HDG unassigned to radar sid in rwy bay.
                 */
                if (element.Value == StripElements.Values.GLOP &&
                    _strip.IsAlertActive(Shared.AlertTypes.NO_HDG))
                {
                    return SKColors.Orange;
                }

                if (_strip.Crossing)
                {
                    return SKColors.Red;
                }

                break;
            case StripElements.Values.SID:
                var sidcolour = SKColors.LimeGreen;

                if (_strip.IsAlertActive(Shared.AlertTypes.VFR_SID))
                {
                    sidcolour = SKColors.Orange;
                }

                return sidcolour;
            case StripElements.Values.RFL:
                return _strip.IsAlertActive(Shared.AlertTypes.RFL) ? SKColors.Orange : SKColors.Empty;
            case StripElements.Values.FIRST_WPT:
                return _strip.IsAlertActive(Shared.AlertTypes.ROUTE) ? SKColors.Orange : SKColors.Empty;
            case StripElements.Values.READY:
                var colour = SKColor.Empty;
                if (_strip.IsAlertActive(Shared.AlertTypes.READY))
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
        var color = SKColor.Empty;

        color = _strip.StripType switch
        {
            StripType.ARRIVAL => SKColor.Parse("ffffa0"), // Light yellow for arrivals
            StripType.DEPARTURE => SKColor.Parse("c1e6f2"), // Light blue for departures
            StripType.LOCAL => SKColor.Parse("e6aedd"), // Light purple for local
            _ => SKColor.Parse("404040"), // Default gray for unknown
        };

        if (LastTransmitModifier && _bayRenderController.Bay.BayManager.LastTransmitManager.LastReceivedFrom != _strip.FDR.Callsign)
        {
            color = color.WithAlpha(_lastTransmitAlpha);
        }

        var paint = new SKPaint()
        {
            Color = color,
            Style = SKPaintStyle.Fill,
        };

        canvas.DrawRect(Origin.X, Origin.Y, BayRenderController.StripWidth + (2 * _padding), BayRenderController.StripHeight, paint);
    }
}
