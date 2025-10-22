# Quick Start Guide - Player Protection Demo

## Opening the Demo

1. Navigate to `/assets/demos/game-demo.html`
2. Open in any modern web browser (Chrome, Firefox, Safari, Edge)
3. No installation or dependencies required

## Basic Usage

### Step 1: Create a Player
1. Enter a player name (default: "Guardian")
2. Click "Create Player"
3. You'll start with 1000 currency

### Step 2: Create Assets
1. Select an asset type (Mine, Factory, Storage, Tower)
2. Adjust location coordinates if desired
3. Click "Create Asset"
4. Assets appear in the "Your Assets" list

### Step 3: Start Personal Patrol
1. Choose patrol type (Zone or Path)
2. Select zone shape (Circular or Rectangular)
3. Choose patrol pattern:
   - **Perimeter**: Walk edges of zone
   - **Random**: Visit random points
   - **Spiral**: Spiral pattern from center
4. Set patrol radius (10-500 meters)
5. Click "Start Patrol"
6. Watch the real-time visualization!

### Step 4: Hire NPC Guards
1. Set payment rate (currency/hour)
2. Set contract duration (hours)
3. Click "Find Available NPCs"
4. Review NPC list sorted by effectiveness
5. Click any NPC to hire them
6. Contract appears in "Active Contracts"

## Features to Try

### Personal Patrol Features
- **Try different patterns**: Compare perimeter, random, and spiral
- **Adjust radius**: See how it affects coverage
- **Watch threat detection**: Red markers show detected threats
- **Monitor statistics**: Distance, duration, threats detected

### NPC Hiring Features
- **Compare NPCs**: Different types have different base rates
- **Distance matters**: Closer NPCs are more cost-effective
- **Payment threshold**: Try low payments to see rejections
- **Multiple guards**: Hire several for increased protection

### Protection Effectiveness
- **No protection**: 0% effectiveness
- **Personal patrol**: 90% effectiveness (highest!)
- **Single NPC guard**: 75% effectiveness
- **Combined protection**: Up to 100% when stacked

## Understanding the Metrics

### Patrol Statistics
- **Status**: Active/Stopped
- **Duration**: How long you've been patrolling
- **Distance**: Total meters covered
- **Threats Detected**: Number of threats found
- **Effectiveness**: Always 90% for personal patrol

### Protection Status
- **Active Protections**: Total number of active protection methods
- **Total Effectiveness**: Combined effectiveness (capped at 100%)
- **Hired Guards**: Number of guards under contract
- **Total Cost/Hour**: Current protection expenses

### NPC Information
- **Distance**: How far away the NPC currently is
- **Travel Time**: Time needed to reach your location
- **Base Rate**: Minimum payment required
- **Effective Rate**: Actual cost per hour including travel
- **Acceptable Payment**: Green = yes, Red = too low

## Tips & Tricks

1. **Start with personal patrol** - It's free and most effective
2. **Hire guards when you're offline** - Keep protection running
3. **Compare NPC types** - Guards are cheapest, Traders most expensive
4. **Watch the visualization** - See your patrol path and threats
5. **Experiment with patterns** - Each has different coverage strengths

## Troubleshooting

**"No NPCs available"**
- Try increasing payment rate
- Reduce required duration
- All NPCs might be hired (refresh page to reset)

**Patrol not showing**
- Make sure you created a player first
- Check that patrol is active (green status)
- Refresh page if needed

**Visualization not updating**
- Patrol should update every 0.5 seconds
- Check browser console for errors (F12)

## Technical Notes

- Demo runs entirely in browser (offline capable)
- No server connection required
- All data resets on page refresh
- Works on desktop and tablet (not optimized for mobile)

## Next Steps

After exploring the demo:
1. Review the [specification documents](../../docs/gameplay/)
2. Consider protection strategies for your gameplay
3. Provide feedback on mechanics and balance
4. Test different scenarios and edge cases

## Demo Limitations

This demo is for concept demonstration only:
- Simplified threat AI (random placement)
- No actual combat or damage calculation
- No resource consumption simulation
- No persistence between sessions
- Limited to 20 pre-generated NPCs

For full implementation details, see the specification documents.
