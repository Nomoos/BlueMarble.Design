# BlueMarble Interactive Demos

This directory contains interactive demonstrations of game mechanics and systems for BlueMarble MMORPG.

## Available Demos

### [Player Protection Systems Demo](./game-demo.html)

An interactive web-based demonstration showcasing the comprehensive player protection and asset management systems.

**Features Demonstrated:**

#### 🛡️ Player Protection Systems
- Personal Zone Patrol (circular/rectangular zones)
- Personal Path Patrol (custom waypoint routes)
- Hired Guard System (NPC and player guards)
- Real-time threat detection and statistics

#### 🚶 Personal Patrol System
- **Zone Patrol Types:**
  - Circular zones (10m - 500m radius)
  - Rectangular zones (up to 1km²)
  
- **Patrol Patterns:**
  - Perimeter: Walk edges of zone
  - Random: Random points within zone
  - Spiral: Spiral from center outward

- **Real-time Features:**
  - Live position tracking
  - Visual patrol representation
  - Threat detection visualization
  - Distance and duration statistics

#### 🤖 Smart NPC Behavior
- NPCs evaluate guard jobs based on payment/distance ratio
- 24-hour walking distance consideration (≈50km range)
- Multiple NPC behavioral types:
  - Guard (combat specialists)
  - Trader (commerce focused)
  - Builder (construction experts)
  - Explorer (discovery oriented)
  - Miner (resource extraction)
  - Transporter (logistics specialists)

#### 💰 Economic Systems
- Cost-effectiveness analysis
- Guard hiring with negotiable rates
- Contract management
- Protection effectiveness tracking
- Real-time cost calculations

#### 🎮 Player Freedom Mechanics
- Asset creation and management
- Mine, factory, and storage facility placement
- Defense tower construction
- Resource trading simulation

## How to Use

1. Open `game-demo.html` in a web browser
2. Create a player to get started
3. Add assets (mines, factories, etc.) to protect
4. Set up personal patrols or hire NPC guards
5. Monitor real-time statistics and protection effectiveness

## Technical Details

**Technologies Used:**
- Pure HTML5, CSS3, and JavaScript
- No external dependencies
- Responsive design
- Real-time updates and animations

**Performance:**
- Lightweight implementation
- 60 FPS animations
- Minimal memory usage
- Browser-compatible (modern browsers)

## Demo Statistics

- **Protection Methods:** 5 different protection types
- **NPC Types:** 6 behavioral variants
- **Patrol Patterns:** 3 distinct algorithms
- **Real-time Metrics:** 10+ tracked statistics

## Game Design Principles Demonstrated

### 1. Meaningful Player Choices
- Multiple protection strategies with different costs/benefits
- No "correct" answer - situational optimization required
- Trade-offs between time, currency, and effectiveness

### 2. Economic Balance
- Higher effectiveness = higher cost or time investment
- Personal patrol: 90% effective, no cost, high time
- Hired guards: 75-85% effective, medium-high cost, no time
- Automated defense: 60-65% effective, high initial cost, ongoing maintenance

### 3. Smart NPC Behavior
- NPCs make logical decisions based on payment and distance
- Distance-based job evaluation prevents exploitation
- Behavioral types create diverse NPC personalities

### 4. Emergent Gameplay
- Players create unique protection strategies
- Combination of methods for optimal coverage
- Dynamic response to changing threats

### 5. Risk vs Reward
- Assets require protection investment
- Greater assets = greater protection needs
- Economic calculation drives decision-making

## Integration with Documentation

This demo illustrates concepts from:
- [Player Protection Systems Specification](../../docs/gameplay/spec-player-protection-systems.md)
- [Player Freedom Mechanics](../../docs/gameplay/spec-player-freedom-mechanics.md)
- [Economy Systems](../../docs/systems/economy-systems.md)
- [Gameplay Systems](../../docs/systems/gameplay-systems.md)

## Future Enhancements

Planned additions to this demo:
- [ ] Alliance-based protection systems
- [ ] Advanced threat AI
- [ ] Resource management visualization
- [ ] Multi-player simulation
- [ ] Map-based asset placement
- [ ] Time progression simulation
- [ ] Weather and environmental effects
- [ ] Player reputation system

## Feedback

This demo is designed to showcase game mechanics for design validation and stakeholder review. For feedback or suggestions:
- Review the related specification documents
- Open issues for specific feature requests
- Test different protection strategies and share findings

## License

This demo is part of the BlueMarble.Design repository and follows the same proprietary license. All rights reserved.
