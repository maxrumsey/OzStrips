# User Guide

OzStrips is a plugin for vatSys that emulates a tower's electronic strips system. This comprehensive guide covers all essential operations and workflows for using OzStrips effectively.

---

## 📋 Table of Contents

- [Getting Started](#getting-started)
- [Interface Overview](#interface-overview)
- [Basic Operations](#basic-operations)
- [Keyboard Shortcuts](#keyboard-shortcuts)
- [Common Workflows](#common-workflows)
- [Advanced Features](#advanced-features)
- [Troubleshooting](#troubleshooting)
- [Tips and Tricks](#tips-and-tricks)
- [Quick Reference](#quick-reference)

---

## 🚀 Getting Started

### Installation
Use the [vatSys Plugin Manager](https://github.com/badvectors/PluginManager) to install OzStrips. 

> **Note**: If you have previously manually installed OzStrips, delete the plugin files before installing via the Plugin Manager.

### First Launch
1. Open vatSys and connect to VATSIM
2. Go to **Tools** → **OzStrips** in the vatSys menu
3. Select your aerodrome from the dropdown menu
4. Wait for the connection status to turn green

### Your First Strip
- **Departure strips** appear in the **Preactive** bay (blue strips)
- **Arrival strips** appear in the **Arrival** bay (yellow strips)
- Click on the aircraft's **callsign** to select the strip
- Use the **SID** button (green box) to move the strip to the next bay

---

## 🖥️ Interface Overview

### Main Window Layout

The OzStrips interface is organized into a grid layout with multiple panels:

![OzStrips Interface](images/sb.png)

### Key Interface Elements

#### Top Menu Bar
- **Aerodrome**: Select your airport (YBBN, YBCG, YBSU, YMEN, YMML, YPPH, YSCB, YSSY)
- **View Mode**: Choose controller position (All, ACD, SMC, ADC, or combinations)
- **Debug**: View connection messages
- **About**: Version information
- **View**: Display options

#### Main Panels (8 Bays)
| Bay | Location | Purpose |
|-----|----------|---------|
| **Preactive** | Top Left | Aircraft with filed flight plans, not yet cleared |
| **Pushback** | Top Middle | Aircraft requesting/approved for pushback |
| **Runway** | Top Right | Aircraft on runway, ready for takeoff |
| **Cleared** | Middle Left | Aircraft with clearance, ready for pushback |
| **Taxi** | Middle Middle | Aircraft taxiing to holding point |
| **Departed** | Middle Right | Aircraft airborne, handed off to departures |
| **Holding Point** | Bottom Middle | Aircraft at holding point, ready for taxi |
| **Arrivals** | Bottom Right | Aircraft approaching for landing |

#### Bottom Status Bar
- **CONN STAT**: Green button showing connection status
- **Timestamp**: Current UTC time
- **Aerodrome**: Current airport code
- **ATIS Code**: Current ATIS information
- **Control Buttons**: INHIBIT, XX CROSS XX, ADD BAR, FLIP FLOP

---

## 📹 OzStrips Tutorial Video

<iframe width="560" height="315" src="https://www.youtube.com/embed/ZEU4bshgQ1s" title="OzStrips Tutorial" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

---

## ⚡ Basic Operations

### Selecting and Moving Strips

#### Selecting a Strip
- **Left-click** on the aircraft's **callsign** to select a strip
- The strip will be highlighted to show it's selected

#### Moving Strips Between Bays
- **Method 1 (Recommended)**: Select a strip and click the **SID button** (green box) to move to next bay
- **Method 2**: Select a strip and drag it to another bay (use with caution)

#### Moving Strips Within a Bay
- Select a strip and use **Up/Down arrow keys** to move vertically
- Use **Ctrl + Up/Down** to move to next/previous bar

### Strip Colors and States

#### Strip Colors
| Color | Aircraft Type |
|-------|---------------|
| **Blue** | Departure aircraft |
| **Yellow** | Arrival aircraft |
| **Pink** | Local aircraft (circuits) |

#### Strip States
| State | Description |
|-------|-------------|
| **Preactive** | Aircraft filed flight plan, not yet cleared |
| **Cleared** | Aircraft has clearance, ready for pushback |
| **Pushback** | Aircraft requesting/approved for pushback |
| **Holding Point** | Aircraft at holding point, ready for taxi |
| **Runway** | Aircraft on runway, ready for takeoff |
| **Departures** | Aircraft airborne, handed off to departures |

---

## ⌨️ Keyboard Shortcuts

| Key | Action | When to Use |
|-----|--------|-------------|
| **Up/Down** | Move strip vertically | Reordering strips in same bay |
| **Ctrl + Up/Down** | Move to next/previous bar | Quick bay transitions |
| **Space** | Add to queue | Aircraft waiting for clearance |
| **Tab** | Cock strip | Mark aircraft as ready |
| **X** | Mark as crossing | Aircraft crossing runway |
| **Alt + X** | Create crossing bar | Visual reminder for runway crossing |
| **F** | Flip-flop strip type | Switch between Arrival/Departure/Local |
| **Backspace** | Remove strip/bar | Hide strip or delete custom bar |
| **Enter** | SID trigger | Move strip to next bay |
| **[ / ]** | Change aerodrome | Switch between airports |

---

## 🔄 Common Workflows

### Departure Workflow (ACD → SMC → ADC)

#### ACD (Aerodrome Clearance Delivery)
1. Strip appears in **Preactive** bay
2. Assign **SID** (green box), **Runway**, and **CFL** (cleared flight level)
3. Issue clearance to pilot
4. Click **SID** button to move to **Cleared** bay

#### SMC (Surface Movement Control)
1. Aircraft requests pushback - move to **Pushback** bay
2. Enter **Gate** number in top-left field
3. Aircraft requests taxi - move to **Holding Point** bay
4. Enter holding point in **Holding Point** field
5. If crossing runway, enter "x###" in remarks (e.g., "x16L")

#### ADC (Aerodrome Director)
1. Sequence departures using arrow keys
2. Issue line-up instructions - move to **Runway** bay
3. Record take-off time by clicking timer
4. Hand off to departures - click **SID** to move to **Departures** bay

### Arrival Workflow
1. Strip appears in **Arrival** bay (yellow)
2. Assign runway and approach
3. Monitor aircraft approach
4. Issue landing clearance
5. Record landing time
6. Clear runway

### Working with Multiple Controllers
- **Top-down**: Select "All" view mode
- **Split positions**: Select appropriate view (ACD+SMC, ADC+SMC, etc.)
- **Non-OzStrips controllers**: Use view mode that includes their position

---

## 🛠️ Advanced Features

### Custom Bars
- Click **Add Bar** button to create custom dividers
- Useful for organizing strips by runway, direction, or priority
- Right-click bars to delete them

### Strip Remarks
- Click on **Remarks** field to add notes
- Only visible to OzStrips users
- Use for coordination notes, special instructions

### Global Ops Field
- Click on **Global Ops** field to edit
- Visible to all vatSys controllers
- Use for departure headings, coordination notes

### Route Display
- Right-click on **Route** field to show full route on vatSys ASD
- Useful for route validation and coordination

### PDC Integration
- Select a strip and click **PDC** button
- Opens vatSys PDC window for that aircraft
- Automatically fills aircraft information

---

## 🔧 Troubleshooting

### Connection Issues
**Problem**: CONN STAT shows red

**Solutions**:
- Wait 10 seconds for connection to establish
- Check internet connection
- Restart OzStrips and vatSys

### Missing Strips
**Problem**: Aircraft not appearing in OzStrips

**Solutions**:
- Click aircraft in vatSys and press **FOR STP**
- Check that ADEP or ADES matches your aerodrome
- Ensure aircraft has filed a flight plan

### Wrong Strip Type
**Problem**: Arrival aircraft showing as departure (or vice versa)

**Solutions**:
- Select strip and press **F** to flip-flop type
- Check aircraft's filed route

### Strip in Wrong Bay
**Problem**: Strip moved to incorrect bay

**Solutions**:
- Use **SID** button instead of drag-and-drop
- Select strip and use arrow keys to move
- Use **Backspace** to remove, then **FOR STP** to restore

### Integration Issues
**Problem**: Changes not appearing in vatSys

**Solutions**:
- Ensure strip is coordinated (blue in vatSys)
- Check that you're using latest version
- Restart both applications

### Performance Issues
**Problem**: OzStrips running slowly

**Solutions**:
- Close unnecessary vatSys windows
- Restart OzStrips
- Check system resources

---

## 💡 Tips and Tricks

### Efficiency Tips
- **Use SID trigger** instead of drag-and-drop to avoid mistakes
- **Learn keyboard shortcuts** for faster operation
- **Use custom bars** to organize strips by runway or priority
- **Set up proper view mode** for your controller position

### Coordination Tips
- **Use remarks field** for coordination notes
- **Set departure headings** in Global Ops field
- **Mark crossing aircraft** with X for situational awareness
- **Use queue bars** to manage multiple aircraft

### Power User Tips
- **Alt-tab** between vatSys and OzStrips quickly
- **Use multiple aerodromes** by switching with [ and ] keys
- **Customize layout** with custom bars and organization
- **Use debug mode** to monitor connection status

---

## 📖 Quick Reference

### Most Common Operations
1. **Select strip**: Click callsign
2. **Move to next bay**: Click SID button (green box)
3. **Reorder strips**: Use Up/Down arrows
4. **Add to queue**: Press Space
5. **Mark ready**: Press Tab
6. **Remove strip**: Press Backspace

### Essential Keyboard Shortcuts
- **Up/Down**: Move strip vertically
- **Enter**: SID trigger (next bay)
- **Space**: Add to queue
- **Tab**: Cock strip
- **F**: Flip-flop strip type
- **Backspace**: Remove strip

### Bay Workflow
```
Preactive → Cleared → Pushback → Holding Point → Runway → Departures
```

### Connection Status
| Status | Meaning |
|--------|---------|
| **Green** | Connected and ready |
| **Red** | Disconnected or connecting |
| **Yellow** | Reconnecting |

### Strip Colors
| Color | Type |
|-------|------|
| **Blue** | Departure |
| **Yellow** | Arrival |
| **Pink** | Local |

### View Modes
| Mode | Purpose |
|------|---------|
| **All** | Top-down control |
| **ACD** | Clearance delivery only |
| **SMC** | Surface movement only |
| **ADC** | Aerodrome director only |
| **Combinations** | Split positions |

### Supported Aerodromes
| Code | Airport |
|------|---------|
| **YBBN** | Brisbane |
| **YBCG** | Gold Coast |
| **YBSU** | Sunshine Coast |
| **YMEN** | Melbourne |
| **YMML** | Melbourne |
| **YPPH** | Perth |
| **YSCB** | Canberra |
| **YSSY** | Sydney |

---

*For additional help, visit the [OzStrips Documentation](https://maxrumsey.xyz/OzStrips/) or the [VATPAC OzStrips Guide](https://sops.vatpac.org/client/towerstrips/).* 