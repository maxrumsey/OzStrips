# OzStrips User Guide - New User Edition

Welcome to OzStrips! This guide is designed to get you up and running quickly. For detailed operational usage refer to refer to your local vatsim division

---

## 📋 Table of Contents

### Getting Started
- [What is OzStrips?](#what-is-ozstrips)
- [Quick Start (5 Minutes)](#quick-start-5-minutes)
- [Essential Concepts (Learn These First)](#essential-concepts-learn-these-first)
- [Your First 10 Minutes](#your-first-10-minutes)

### Core Skills
- [Common Tasks (How to Do What You Need)](#common-tasks-how-to-do-what-you-need)
- [Essential Strip Fields (Start with These)](#essential-strip-fields-start-with-these)
- [Essential Keyboard Shortcuts](#essential-keyboard-shortcuts)
- [Common Issues (And How to Fix Them)](#common-issues-and-how-to-fix-them)

### Customization & Advanced Features
- [Customizing Your View](#customizing-your-view)
- [Understanding Alerts and Warnings](#understanding-alerts-and-warnings)

### Reference & Troubleshooting
- [Next Steps](#next-steps)
- [Complete Strip Field Reference](#complete-strip-field-reference)
- [Complete Keyboard Shortcuts](#complete-keyboard-shortcuts)
- [Detailed Workflow Examples](#detailed-workflow-examples)
- [Comprehensive Troubleshooting](#comprehensive-troubleshooting)
- [Getting Help](#getting-help)

---

---

## 🎯 What is OzStrips?

OzStrips is a plugin for vatSys that replaces paper flight strips with digital ones. Think of it as a digital version of the paper strips that air traffic controllers use to track aircraft.

**Why use OzStrips?**
- No more paper strips cluttering your desk
- Automatic updates when aircraft move between positions
- Visual alerts for potential issues
- Integration with vatSys for seamless operation

---

## 🚀 Quick Start (5 Minutes)

### Step 1: Install and Connect
1. **Install**: OzStrips comes with VATPAC's vatSys profiles, or download using the [vatSys Plugin Manager](https://github.com/badvectors/PluginManager)
2. **Open**: Go to **Window** → **OzStrips** in vatSys
3. **Connect**: Select your airport from the dropdown
4. **Verify**: Connection status should turn green

![OzStrips Dropdown](images/ozstripsdropdown.png)

*Finding OzStrips in the vatSys menu*

### Step 2: Your First Strip
When an aircraft files a flight plan to/from your airport, a strip will appear automatically.

**What you'll see:**
- **Blue strips** = Departing aircraft
- **Yellow strips** = Arriving aircraft  
- **Pink strips** = Local aircraft (circuits)

![Strip Example](images/ozstripsexample.png)

*Your first strip - this aircraft is departing (blue)*

### Step 3: Basic Strip Movement
**The easiest way to move strips:**
1. **Click the SID box** (Standard Instrument Departure) on the strip
2. The strip moves to the next bay automatically
3. This is called "SID triggering" and is the recommended method

**Alternative method (pick and drop):**
1. Click the **callsign** to select the strip
2. Click the **destination bay** to drop it there

### Step 4: Understanding Strip Bays
Strips move through different bays representing stages of flight:

| Bay | Meaning | What Happens Here |
|-----|---------|-------------------|
| **Preactive** | Aircraft filed plan, not cleared yet | Initial planning |
| **Cleared** | Aircraft has clearance | Ready for pushback |
| **Pushback** | Aircraft requesting/approved pushback | Ground operations |
| **Holding Point** | Aircraft at holding point | Ready for taxi |
| **Runway** | Aircraft on runway | Ready for takeoff |
| **Departures** | Aircraft airborne | Handed off to departures |

![OzStrips Window](images/ozstripswindow.png)

*The main OzStrips interface showing different bays*

---

## 📚 Essential Concepts (Learn These First)

### What is a "Strip"?
A strip is a digital card containing all the information about an aircraft's flight. Each strip has multiple fields you can interact with.

### What is "SID Triggering"?
Clicking the SID (Standard Instrument Departure) box automatically moves the strip to the next bay. This is the safest and most efficient way to move strips.

### What are "Bays"?
Bays are sections of your stripboard that represent different stages of an aircraft's flight. Strips move from left to right as the aircraft progresses.

### What do the colors mean?
- **Blue**: Departure aircraft (leaving your airport)
- **Yellow**: Arrival aircraft (coming to your airport)
- **Pink**: Local aircraft (staying in your area, like training flights)

---

## 🎮 Your First 10 Minutes

### Try This: Move a Strip
1. Find a strip in the "Preactive" bay
2. Click the **SID box** (the field that shows the departure procedure)
3. Watch the strip move to the "Cleared" bay
4. Click the SID box again to move it to "Pushback"

### Try This: Check for Issues
Look for these visual indicators on strips:
- **Orange background**: Potential problems (hover for details)
- **Yellow border**: Warnings that need attention
- **Red highlights**: Critical issues

### Try This: Use Keyboard Shortcuts
- **Enter**: SID trigger (move to next bay) - *Most important!*
- **Tab**: Mark aircraft as ready
- **F**: Switch between Arrival/Departure/Local
- **X**: Mark as crossing runway

---

## 🔧 Common Tasks (How to Do What You Need)

### Moving Strips Between Positions
**Recommended method (SID triggering):**
1. Click the **SID box** on the strip
2. Strip moves to next bay automatically
3. Repeat as needed

**Manual method (pick and drop):**
1. Click the **callsign** to select the strip
2. Click the **destination bay** to drop it
3. Use this only when SID triggering isn't appropriate

### Marking Aircraft as Ready
- **Click the "Ready" field** on the strip, OR
- **Press Tab** while the strip is selected

### Changing Aircraft Type
- **Click the aircraft type field** to open flight plan
- **Right-click** for additional options

### Assigning Runways
- **Click the runway field** to change runway assignment
- Available runways depend on your airport

### Handling Arrivals vs Departures
- **Press F** to switch strip type if needed
- **Blue strips** = Departures (leaving)
- **Yellow strips** = Arrivals (coming in)

---

## ⚠️ Common Issues (And How to Fix Them)

### "Aircraft not appearing in OzStrips"
**Solution:**
1. Click the aircraft in vatSys
2. Press **FOR STP** 
3. Check that the aircraft is going to/from your airport

### "Strip moved to wrong bay"
**Solution:**
1. Use **SID triggering** instead of drag-and-drop
2. Use **Backspace** to remove the strip
3. Press **FOR STP** to restore it

### "Connection shows red"
**Solution:**
1. Wait 10 seconds for connection to establish
2. Check your internet connection
3. Restart OzStrips if needed

### "Strip type is wrong (arrival showing as departure)"
**Solution:**
1. Select the strip
2. Press **F** to flip-flop the type
3. Check the aircraft's filed route

---

## 🎯 Essential Strip Fields (Start with These)

You don't need to know all 21 fields right away. Start with these essential ones:

| Field | What It Does | How to Use |
|-------|-------------|------------|
| **Callsign** | Aircraft identification | Click to select strip |
| **SID** | Departure procedure | Click to move to next bay |
| **Runway** | Assigned runway | Click to change runway |
| **Ready** | Aircraft ready status | Click to mark as ready |
| **CFL** | Cleared flight level | Click to change altitude |

**Pro tip**: You can hover over any field to see what it does!

---

## ⌨️ Essential Keyboard Shortcuts

Learn these 5 shortcuts first:

| Key | What It Does | When to Use |
|-----|-------------|-------------|
| **Enter** | Move strip to next bay | Most common action |
| **Tab** | Mark aircraft as ready | When aircraft reports ready |
| **F** | Switch strip type | If arrival/departure is wrong |
| **X** | Mark as crossing | Aircraft crossing runway |
| **Backspace** | Remove strip | Hide strip when no longer needed |

---

## 🎨 Customizing Your View

### Adjust Strip Size
- Go to **Help** → **Settings**
- Use the **Strip Scale** slider
- Smaller strips = more strips visible

### Smart Resize
- Enable **Smart Resize** for automatic bay height adjustment
- Useful when you have limited screen space

![Smart Resize](images/ozstripssmartresize.png)

*Smart resize setting for automatic layout adjustment*

### View Modes
- **Single Column**: For small screens
- **Multi-Column**: Standard view
- **Narrow Layout**: Automatically adjusts to screen size

![Narrow Layout](images/ozstripsnarrow.png)

*Single column layout for compact displays*

![View Mode](images/ozstripsviewmode.png)

*Selecting different view modes*

---

## 🔍 Understanding Alerts and Warnings

OzStrips will show you visual warnings when there are potential issues:

### Orange Background
- **SSR Code**: Aircraft not squawking correct code
- **SID Field**: VFR aircraft assigned SID (shouldn't happen)
- **Route Field**: Non-compliant route filed

![Bad Level](images/ozstripsbadlevel.png)

*Example of incorrect level assignment (orange background)*

![Bad Route](images/ozstripsbadroute.png)

*Example of incorrect route assignment (orange background)*

![VFR SID](images/ozstripsvfrsid.png)

*Example of VFR aircraft assigned SID (orange background)*

### Yellow Border
- **SID Field**: SID has a transition (normal, just informational)

![SID Transition](images/ozstripssidtransition.png)

*SID with transition (yellow border)*

### Red Highlights
- **Crossing**: Aircraft marked as crossing runway
- **Critical Issues**: Serious problems requiring attention

**How to check what's wrong:**
1. **Hover over the field** to see details
2. **Click the field** to fix the issue
3. **Check the tooltip** for specific guidance

---

## 📖 Next Steps

### Once You're Comfortable with the Basics:
1. **Learn more strip fields** - See the complete reference table
2. **Explore keyboard shortcuts** - Full list in the reference section
3. **Try different view modes** - Find what works for your setup
4. **Practice with workflow examples** - See how real controllers use it

### Reference Materials:
- **Complete Strip Field Reference**: All 21 fields and their functions
- **Keyboard Shortcuts**: Full list of all shortcuts
- **Workflow Examples**: Real-world usage scenarios
- **Troubleshooting**: Detailed problem-solving guide

---

## 📋 Complete Strip Field Reference

Each strip contains 21 fields that can be interacted with:

![Strip Reference](images/ozstripstrip.png)

*Reference strip showing all 21 fields*

| Field # | Content | Left Click | Right Click | Possible Alert / Alarm |
|---------|---------|------------|-------------|----------------------|
| **1** | **Bay Number** | Edit Bay Number | | |
| **2** | **Filed Off Blocks Time** | Cock Strip | | |
| **3** | **Aircraft Type** | Open Flightplan | | |
| **4** | **Wake Turbulence Category** | | | |
| **5** | **Destination** | Open Flightplan | | |
| **6** | **Voice Receive Capability Indicator** | Show Route | | |
| **7** | **Flight Rules** | Show Route | | |
| **8** | **PDC Indicator** | Open PDC Window | Open PM Window | |
| **9** | **SSR Code** | Autogenerate Code | | Incorrect SSR Code or Mode |
| **10** | **Callsign** | Select Strip | | Worldflight Team |
| **11** | **Runway** | Change Runway | | |
| **12** | **Ready Flag** | Toggle Ready Flag | | Aircraft in Holding Point / Runway bay but not ready |
| **13** | **Holding Point** | Edit Holding Point | | |
| **14** | **SID** | Move strip to next bay | Change SID | SID Transition Exists or VFR Aircraft issued a SID |
| **15** | **First Waypoint** | Open flightplan | Open Reroute Window | Potentially Incorrect Routing |
| **16** | **Requested Level** | Open flightplan | | |
| **17** | **Cleared Level** | Change CFL | | Incorrect Cruising Level |
| **18** | **vatSys Global Ops Field** | Edit | | |
| **19** | **OzStrips Remarks** | Edit | | |
| **20** | **Departure Heading** | Edit Departure Heading | | No HDG input to Radar SID Departures |
| **21** | **Takeoff Timer** | Start / Reset | | |

> **Note**: SID triggering won't automatically move strips into the runway bay - this must be done manually to prevent accidental runway placement.

---

## ⌨️ Complete Keyboard Shortcuts

| Key | Action | When to Use |
|-----|--------|-------------|
| **Up** | Move strip vertically up | Reordering strips in same bay |
| **Down** | Move strip vertically down | Reordering strips in same bay |
| **Ctrl + Up** | Move to next bar | Quick bay transitions |
| **Ctrl + Down** | Move to previous bar | Quick bay transitions |
| **Space** | Add to queue | Aircraft waiting for clearance |
| **Tab** | Cock strip | Mark aircraft as ready |
| **X** | Mark as crossing | Aircraft crossing runway |
| **Alt + X** | Create crossing bar | Visual reminder for runway crossing |
| **F** | Flip-flop strip type | Switch between Arrival/Departure/Local |
| **Backspace** | Remove strip/bar | Hide strip or delete custom bar |
| **Enter** | SID trigger | Move strip to next bay |
| **[** | Previous aerodrome | Switch between airports |
| **]** | Next aerodrome | Switch between airports |

---

## 📊 Detailed Workflow Examples

### ACD (Aerodrome Control) Workflow
![ACD Workflow](images/ozstripsworkflowacd.png)

*Typical workflow for aerodrome control operations*

**ACD Workflow Steps:**
1. **Preactive Bay**: Aircraft files flight plan
2. **Cleared Bay**: Aircraft receives clearance
3. **Pushback Bay**: Aircraft requests/approved for pushback
4. **Holding Point Bay**: Aircraft at holding point, ready for taxi
5. **Runway Bay**: Aircraft on runway, ready for takeoff
6. **Departures Bay**: Aircraft airborne, handed off to departures

### ADC (Approach Departure Control) Workflow  
![ADC Workflow](images/ozstripsworkflowadc.png)

*Workflow for approach and departure control*

**ADC Workflow Steps:**
1. **Monitor arrivals** from approach control
2. **Coordinate departures** with tower
3. **Manage traffic flow** between arrivals and departures
4. **Hand off departures** to en-route control

### SMC (Surface Movement Control) Workflow
![SMC Workflow](images/ozstripsworkflowsmc.png)

*Surface movement control workflow*

**SMC Workflow Steps:**
1. **Monitor ground traffic** movement
2. **Coordinate taxi routes** and holding points
3. **Manage runway crossings** and safety
4. **Coordinate with tower** for runway operations

---

## 🔧 Comprehensive Troubleshooting

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

### Alert and Warning Issues
**Problem**: Orange backgrounds or yellow borders on strips

**Solutions**:
- **Orange SSR Code**: Check aircraft is squawking correct code
- **Orange SID Field**: Remove SID from VFR aircraft
- **Orange Route Field**: Check route compliance
- **Yellow SID Border**: Normal - SID has transition
- **Red Highlights**: Check for crossing or critical issues

![Workflow Examples](images/ozstripsworkflowacd.png)

*Example workflow for aerodrome control operations*

---

## 🆘 Getting Help

### Quick Help
- **Hover over any field** to see what it does
- **Press F1** in OzStrips for context-sensitive help
- **Check the status bar** for current information

### Documentation
- **VATPAC OzStrips Guide**: Operational procedures
- **Reference Documentation**: Detailed technical information
- **This Guide**: User-friendly learning material

### Support
- **Contact the development team** for technical issues
- **VATPAC Forums** for community support
- **VATSIM Forums** for general questions

---

## 🎉 Congratulations!

You now know enough to start using OzStrips effectively. Remember:

1. **Start simple** - Use SID triggering for most strip movements
2. **Learn gradually** - You don't need to know everything at once
3. **Practice regularly** - The more you use it, the more natural it becomes
4. **Ask for help** - The community is there to support you

**Happy controlling!** 🛩️

---

*For detailed reference information, see the [Complete Reference Guide](../reference/) or visit the [VATPAC OzStrips Guide](https://sops.vatpac.org/client/towerstrips/).* 