# Mortal Online 2 Material and Quality System Research

**Document Type:** Market Research  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2025-01-15  
**Status:** Final  
**Research Type:** Market Research  
**Priority:** Low

## Executive Summary

This research document provides comprehensive analysis of Mortal Online 2's material grading and crafting systems to inform BlueMarble's material quality mechanics. Mortal Online 2 offers a sophisticated open-ended material system where player knowledge, skill, and choices directly influence material quality and crafting outcomes through deep interconnected mechanics.

**Key Findings:**
- **Multi-dimensional Material System**: Materials are defined by multiple properties (durability, weight, density, yield) rather than simple quality grades
- **Player-Driven Quality**: Quality emerges from player decisions about material selection, processing methods, and skill application
- **Realistic Material Behavior**: Materials behave according to their physical properties, creating authentic crafting experiences
- **Knowledge-Based Discovery**: Players learn optimal material combinations through experimentation and experience
- **Economic Integration**: Material properties directly impact item value, creating natural market differentiation

**Applicability to BlueMarble:**
Mortal Online 2's approach aligns exceptionally well with BlueMarble's geological simulation foundation. The emphasis on realistic material properties, player experimentation, and emergent quality systems provides an excellent framework for integrating geological accuracy with engaging gameplay mechanics.

## Table of Contents

1. [Research Objectives](#research-objectives)
2. [Methodology](#methodology)
3. [Core Material System Mechanics](#core-material-system-mechanics)
4. [Material Grading and Properties](#material-grading-and-properties)
5. [Crafting and Quality Systems](#crafting-and-quality-systems)
6. [Player Agency and Choice](#player-agency-and-choice)
7. [Economic and Social Integration](#economic-and-social-integration)
8. [Comparative Analysis](#comparative-analysis)
9. [BlueMarble Applicability Assessment](#bluemarble-applicability-assessment)
10. [Recommendations](#recommendations)
11. [Implementation Considerations](#implementation-considerations)
12. [Next Steps](#next-steps)

## Research Objectives

### Primary Research Questions
1. **What are the core mechanics of material grading and crafting in Mortal Online 2?**
   - How are materials categorized and differentiated?
   - What properties define material quality?
   - How do materials interact in crafting recipes?

2. **How does player choice and interaction influence material quality?**
   - What decisions do players make regarding materials?
   - How does skill affect material processing and outcomes?
   - What role does experimentation play?

3. **What ideas are applicable to BlueMarble?**
   - Which mechanics align with geological simulation?
   - How can realistic material properties enhance gameplay?
   - What systems promote player engagement and discovery?

### Secondary Research Questions
1. How does the material system integrate with the broader economy?
2. What role does player knowledge and community sharing play?
3. How balanced is the progression from basic to advanced materials?
4. What quality of life features support the material system?

### Success Criteria
This research is successful if it:
- Provides comprehensive understanding of MO2's material mechanics
- Identifies concrete mechanics applicable to BlueMarble
- Offers actionable recommendations for material system design
- Highlights potential pitfalls and challenges to avoid


## Methodology

### Research Approach
**Qualitative Analysis** focusing on system design, player experience, and emergent gameplay patterns.

### Data Collection Methods
- **Wiki Analysis:** Comprehensive review of Mortal Online 2 wiki documentation (mortalonline2.wiki)
- **Video Analysis:** Examination of gameplay videos and crafting demonstrations
- **Developer Content:** Review of developer interviews and design blog posts
- **Community Research:** Analysis of player guides, forum discussions, and strategy resources
- **Comparative Study:** Cross-reference with existing BlueMarble documentation and other analyzed games

### Data Sources
- **Official Wiki:** https://mortalonline2.wiki/wiki/Main_Page
- **Official Website:** https://www.mortalonline2.com/
- **Developer Blog Posts:** Design philosophy and system explanations
- **YouTube Channels:** Crafting guides and material analysis videos
- **Reddit Communities:** r/MortalOnline discussion threads
- **Player-Created Guides:** Crafting calculators and material databases

### Limitations
- Research based on publicly available information; no direct developer interviews
- System may evolve post-research through game updates
- Some advanced mechanics may not be fully documented
- Player-discovered optimizations may not be officially documented
- Research conducted from external perspective (not in-game testing)

## Core Material System Mechanics

### Material Philosophy

Mortal Online 2's material system is built on **physical realism** and **emergent complexity**:

```
Core Design Principle:
Materials are not simply "good" or "bad" - they have trade-offs that make 
different materials optimal for different purposes.
```

### Material Definition Structure

Every craftable material in MO2 is defined by multiple properties:

**Primary Physical Properties:**
```
Material: Steel (example)
├── Durability: 8100      # Hit points before breaking
├── Weight: 5800          # Affects character weight/stamina
├── Density: 7.8          # Weight per volume
├── Yield Strength: 2200  # Resistance to permanent deformation
├── Hardness: 180         # Scratch resistance
├── Flexibility: 35       # Bend before breaking
└── Thermal Properties    # Heat conductivity, melting point
```

### Material Categories

**Metal Ores:**
- Iron Ores (Granum, Calx, Cuprum variations)
- Precious Metals (Gold, Silver)
- Specialized Alloys (Steel, Tungsteel, Cronite)

**Stone Materials:**
- Building Stone (Granite, Marble)
- Tool Stone (Flakestone, Obsidian)
- Decorative Stone

**Organic Materials:**
- Wood Types (Dense Wood, Spongewood, Ironwood)
- Leather (various animal hides)
- Bone and Chitin

**Refined Materials:**
- Alloys (player-created metal combinations)
- Processed Leather
- Composite Materials

### Material Acquisition

**Extraction Process:**
```
1. Resource Node Discovery
   └── Different geographic locations yield different ore types
   
2. Extraction
   └── Mining/Gathering skill affects yield amount
   └── Does NOT affect material quality (quality is node-specific)
   
3. Raw Material Processing
   └── Crushing ore → crushed ore
   └── Smelting → ingots (with potential alloy creation)
   
4. Material Application
   └── Use in crafting recipes
```

**Key Insight:** Quality is inherent to the source node, not extraction skill. This creates geographic specialization and trade opportunities.


## Material Grading and Properties

### Property-Based System (Not Grade-Based)

Unlike many games, MO2 does **not** use simple quality grades (Common/Rare/Epic). Instead, materials exist on multiple continuous scales:

**Durability Spectrum:**
```
Low Durability Materials:
- Lightweight armor (less stamina penalty)
- Fragile but accessible
- Example: Basic Iron ~6000 durability

High Durability Materials:
- Heavy protection
- Expensive and rare
- Example: Tungsteel ~15000 durability
```

**Weight vs Protection Trade-off:**
```
Light Materials (Silk, Bone):
+ Low stamina drain
+ Fast movement
- Less physical protection
- Harder to repair

Heavy Materials (Steel, Tungsteel):
+ Maximum protection
+ High durability
- Significant stamina penalty
- Restricted movement speed
```

### Material Property Analysis

**Example: Weapon Material Selection**

```
Scenario: Crafting a Sword

Option A: Steel
- Durability: 8100
- Weight: 5800 (Heavy)
- Damage: High
- Cost: Moderate
→ Best for: Frontline melee combat

Option B: Bone Tissue
- Durability: 4500
- Weight: 1200 (Very Light)
- Damage: Moderate
- Cost: Low
→ Best for: Mobile/skirmish combat

Option C: Tungsteel (Steel + Tungsten alloy)
- Durability: 15000+
- Weight: 7200 (Very Heavy)
- Damage: Very High
- Cost: Extremely High
→ Best for: Elite warriors with high strength
```

### Geographic Material Distribution

**Regional Specialization:**

```
Northern Regions:
└── Calx (Iron variant) - Abundant
└── Dense Wood - Common
└── Crepite (Copper variant) - Rare

Southern Regions:
└── Granum (Pure iron) - Abundant
└── Spongewood - Common
└── Obsidian - Rare

Mountainous Areas:
└── Tungsten Ore - Very Rare
└── Precious Metals - Rare
└── Quality Stone - Common
```

This creates natural trade networks and territorial value.

### Material Information Presentation

**In-Game Display:**
```
Material: Steel Ingot
────────────────────
Durability:  8100
Weight:      5800
Density:     7.8
Yield:       2200
Hardness:    180
Flexibility: 35
────────────────────
Uses: Weapons, Armor, Tools
Value: ~50 gold/unit
```

**Transparency:** Players have access to all material properties before crafting, enabling informed decisions.


## Crafting and Quality Systems

### Crafting Formula System

Mortal Online 2 uses a **flexible recipe system** where players select:

**1. Base Recipe:**
```
Example: Two-Handed Sword Recipe
├── Primary Material: [Player Choice] × 40 units
├── Handle Material: [Player Choice] × 8 units  
└── Reinforcement: [Player Choice] × 12 units
```

**2. Material Selection:**
Players choose specific materials for each component:
```
Player Decision Process:
├── What properties do I prioritize? (Durability vs Weight)
├── What materials can I afford?
├── What materials are available in my region?
└── What is my skill level? (Affects success rate)
```

### Quality Determination

**Final Item Quality Calculation:**

```
Item_Stats = f(
    Material_Properties,      // Physical properties of chosen materials
    Crafter_Skill,           // Reduces waste, improves success chance
    Recipe_Complexity,        // More complex = higher failure risk
    Tool_Quality,            // Better tools = better outcomes
    Random_Variation          // ±5-10% variation
)
```

**Skill Impact:**
- **Low Skill:** High material waste (30-50%), frequent failures, lower durability output (-20%)
- **High Skill:** Minimal waste (5-10%), rare failures, maximum durability output, potential bonus (+5-10%)

### Experimentation and Discovery

**Alloy Creation System:**

Players can experiment with metal combinations:

```
Example: Steel Creation
Base: Iron (80%) + Coal (20%)
Result: Basic Steel (Standard recipe)

Experiment: Iron (75%) + Coal (20%) + Small Tungsten (5%)
Result: Tungsteel Variant (Discovery!)
Properties: +30% durability, +15% weight
```

**Knowledge Acquisition:**
- Successful experiments recorded in crafting book
- Can be taught to other players
- Creates specialized knowledge economy
- Encourages community experimentation

### Quality Variation Range

**Typical Quality Ranges by Skill:**

```
Novice Crafter (Skill 0-30):
├── Success Rate: 40-60%
├── Material Efficiency: 50-70%
└── Property Output: 70-85% of maximum

Journeyman Crafter (Skill 30-70):
├── Success Rate: 70-90%
├── Material Efficiency: 75-90%
└── Property Output: 85-95% of maximum

Master Crafter (Skill 70-100):
├── Success Rate: 95-99%
├── Material Efficiency: 90-95%
└── Property Output: 95-105% of maximum
```

**Masterwork Chance:** Master crafters have ~2-5% chance to create masterwork items with +10-15% bonus to all properties.

## Player Agency and Choice

### Decision Points in Material System

**1. Material Source Selection**

```
Player Decision: Where to gather materials?

Option A: Local Resources
+ Immediately available
+ Low transportation cost
- May not be optimal materials
- Subject to local depletion

Option B: Trade for Distant Materials
+ Access to specialized materials
+ Optimal properties possible
- High gold cost
- Transportation risk (PvP)

Option C: Territorial Control
+ Secure resource access
+ Long-term supply
- Requires guild support
- Contested territory risk
```

**2. Material Quality vs Cost Trade-off**

```
Scenario: Crafting Full Armor Set

Budget Build:
- Materials: Basic Iron, Leather
- Cost: ~500 gold
- Properties: Adequate for PvE
- Replacement: Every 20-30 hours

Balanced Build:
- Materials: Steel, Processed Leather
- Cost: ~2000 gold
- Properties: Good all-around
- Replacement: Every 50-80 hours

Premium Build:
- Materials: Tungsteel, Molarium
- Cost: ~10000+ gold
- Properties: Elite performance
- Replacement: 100+ hours
```

### Player Knowledge Development

**Knowledge Progression Curve:**

```
Stage 1: Novice (0-10 hours)
├── Learn basic recipes
├── Understand material categories
├── Experience failures
└── Develop material preferences

Stage 2: Apprentice (10-50 hours)
├── Recognize material trade-offs
├── Calculate cost efficiency
├── Begin experimentation
└── Build recipe knowledge

Stage 3: Journeyman (50-200 hours)
├── Optimize material combinations
├── Teach others
├── Discover niche recipes
└── Establish reputation

Stage 4: Master (200+ hours)
├── Innovate new approaches
├── Predict market trends
├── Create specialized builds
└── Community expert status
```

### Choice Consequences

**Full Loot Impact:**
Every crafted item can be lost in PvP, making material choices critical:
- Use cheaper materials for risky situations
- Save premium gear for important battles
- Calculate risk/reward for each excursion

## Economic and Social Integration

### Material-Driven Economy

**Supply Chain Example: Steel Production**

```
1. Raw Material Extraction
   ├── Miners extract iron ore (Various regions)
   ├── Loggers harvest coal (Forest regions)
   └── Geographic specialization creates trade

2. Processing
   ├── Smelters convert ore → ingots
   ├── Skill level affects yield
   └── Location matters (near resources vs cities)

3. Crafting
   ├── Smiths create weapons/armor
   ├── Quality variation creates market tiers
   └── Reputation affects prices

4. Distribution
   ├── Traders transport goods
   ├── PvP risk = premium prices
   └── Regional scarcity = opportunity
```

**Price Differentiation:**

```
Material Market Tiers:

Basic Materials (Common):
- Iron, Leather, Wood
- Price: Stable, low
- Availability: High
- Margin: 10-20%

Premium Materials (Rare):
- Tungsten, Cronite, Oghmium
- Price: High volatility
- Availability: Low
- Margin: 100-300%
```

### Guild Integration

**Material System Guild Dynamics:**

```
Shared Resources:
├── Guild-controlled resource nodes
├── Communal material storage
├── Coordinated extraction schedules
└── Defense of gathering operations

Knowledge Sharing:
├── Recipe libraries
├── Training programs
├── Material source mapping
└── Market intelligence
```

### Reputation and Trust

**Player-Driven Quality Assurance:**

```
Master Smith "Ironheart":
├── Known for: Tungsteel weapons
├── Reputation: Always uses premium materials
├── Pricing: 20% above market (worth it)
└── Wait List: 2-3 day queue

Bargain Crafter "QuickSteel":
├── Known for: Fast production
├── Reputation: Variable quality
├── Pricing: 30% below market
└── Wait List: Immediate
```


## Comparative Analysis

### Mortal Online 2 vs Other Material Systems

**Comparison Table: Material System Design**

| Game | Material Grades | Quality Source | Player Choice | Realism Level |
|------|----------------|----------------|---------------|---------------|
| **Mortal Online 2** | Property-based (continuous) | Material selection + Skill | High - Full control | Very High - Physical properties |
| World of Warcraft | Tiered (Common→Legendary) | Drop/Crafting RNG | Low - Limited choice | Low - Arbitrary stats |
| Wurm Online | Quality 1-100 | Node quality + Skill | High - Tool choice | High - Realistic properties |
| Life is Feudal | Quality 0-100 | Extraction skill + Node | Medium - Some control | High - Physical simulation |
| Vintage Story | Grade system (Poor→Legendary) | Tool tier + Biome | Medium - Limited by tech | Medium - Abstracted |

**Key Differentiators for MO2:**
1. **Multi-Property System:** Materials defined by 6+ properties instead of single "quality" value
2. **No Hard Tiers:** Continuous spectrum rather than discrete quality levels
3. **Full Transparency:** All material properties visible before crafting
4. **Geographic Variation:** Real differences between material sources
5. **Trade-off Complexity:** No objectively "best" material - context dependent

### Strengths vs Weaknesses Analysis

**Mortal Online 2 Material System Strengths:**

✅ **Depth and Complexity:**
- Multiple material properties create interesting trade-offs
- No simple "best in slot" - context matters
- Encourages experimentation and knowledge building

✅ **Player Agency:**
- Full control over material selection
- Meaningful choices at every stage
- Direct impact of decisions on outcomes

✅ **Economic Richness:**
- Geographic specialization creates trade
- Multiple quality tiers support different budgets
- Knowledge itself becomes valuable commodity

✅ **Realism:**
- Material properties reflect real physics
- Crafting process feels authentic
- Learning curve mirrors real-world skill acquisition

✅ **Social Integration:**
- Specialization encourages interdependence
- Knowledge sharing builds community
- Reputation systems emerge naturally

**Mortal Online 2 Material System Weaknesses:**

❌ **High Complexity:**
- Steep learning curve for new players
- Overwhelming number of materials and properties
- Requires significant wiki/guide consultation

❌ **Information Overload:**
- Too many numbers and properties to track mentally
- Difficult to compare materials at a glance
- Spreadsheet-dependent for optimization

❌ **Full Loot Tension:**
- Discourages using premium materials (loss fear)
- Creates conservative behavior
- May limit material system engagement

❌ **Market Volatility:**
- Difficult to price craft labor accurately
- Material costs can spike unexpectedly
- Hard for casual players to participate economically

❌ **Barrier to Entry:**
- New players struggle with material choices
- Expensive to experiment (material loss)
- Long time investment to gain knowledge

### Best Practices Identified

**What MO2 Does Exceptionally Well:**

1. **Transparent Information:** All material stats visible, predicted craft outcomes shown
2. **Trade-off Design:** Weight vs Durability, Cost vs Performance, Risk vs Reward
3. **Geographic Gameplay:** Different regions = different materials, creates territorial value
4. **Skill Progression:** Gradual mastery curve, always room for improvement
5. **Player-Driven Discovery:** Experimentation rewarded, community knowledge building

## BlueMarble Applicability Assessment

### Alignment with BlueMarble Core Concepts

**Strong Alignment Areas:**

✅ **Geological Realism (Perfect Fit)**
```
MO2 Approach → BlueMarble Translation:

Material Physical Properties → Geological Material Properties
├── MO2: Density, Hardness, Flexibility
└── BlueMarble: Mineral composition, Hardness (Mohs), Crystalline structure

Geographic Distribution → Geological Distribution
├── MO2: Regional ore types
└── BlueMarble: Real-world geological formations

Extraction Context → Geological Context
├── MO2: Node location matters
└── BlueMarble: Deposit depth, formation type, purity
```

**Why It Works:**
- BlueMarble already simulates geological reality
- Material properties are scientifically grounded
- Natural geographic variation exists in the simulation
- Player discovery aligns with geological education goals

✅ **Player Knowledge Progression (Excellent Fit)**
```
MO2 Knowledge System → BlueMarble Application:

Material Property Learning → Geological Understanding
├── Players learn through experimentation
├── Success requires material knowledge
└── Community knowledge sharing

Crafting Mastery → Processing Expertise
├── Skill improves outcomes
├── Experience reveals optimal approaches
└── Specialization emerges naturally
```

✅ **Economic Complexity (Strong Fit)**
- Geographic specialization supports player-driven economy
- Processing value addition creates economic depth
- Quality differentiation provides market tiers
- Natural emergence of market dynamics

### Areas Requiring Adaptation

⚠️ **Full Loot PvP Context**

```
MO2 Challenge:
- Full loot PvP discourages premium material use
- Players hoard best materials
- Risk aversion limits system engagement

BlueMarble Adaptation:
Option A: No Full Loot
+ Players willing to use premium materials
+ More engagement with quality system
- Less risk/reward tension

Option B: Different Context
+ Focus on construction/development
+ Items not carried into combat
- Different risk model needed
```

**Recommendation:** BlueMarble should avoid full loot mechanics to encourage material system engagement.

⚠️ **Complexity Management**

```
MO2 Challenge:
- 100+ materials with 6+ properties each
- Requires extensive documentation
- Spreadsheet-dependent optimization

BlueMarble Solutions:

1. Better UI/UX
   ├── Visual material comparison tools
   ├── In-game crafting calculator
   ├── Material property tooltips
   └── Simplified displays for casual players

2. Graduated Complexity
   ├── Simple materials for beginners
   ├── Advanced materials for experienced players
   ├── Optional depth for enthusiasts
   └── Viable gameplay at all levels

3. Smart Defaults
   ├── Suggest appropriate materials
   ├── "Good/Better/Best" quick selection
   ├── Detailed stats available on demand
   └── Learn complexity gradually
```

**Recommendation:** Implement progressive complexity with better tooling.

### Recommended Implementations for BlueMarble

**1. Material Property System**

```csharp
public class GeologicalMaterial : IMaterial
{
    // Core identification
    public MaterialId Id { get; set; }
    public string Name { get; set; }
    public MineralComposition Composition { get; set; }
    
    // Physical properties (MO2-inspired)
    public float Hardness { get; set; }          // Mohs scale (1-10)
    public float Density { get; set; }           // g/cm³
    public float Durability { get; set; }        // Wear resistance
    public float Workability { get; set; }       // Processing difficulty
    
    // Geological context
    public FormationType Formation { get; set; }
    public float Purity { get; set; }            // 0-100%
    public List<Impurity> Impurities { get; set; }
    
    // Economic properties
    public Rarity RarityClass { get; set; }
    public float BaseValue { get; set; }
    
    // Processing requirements
    public ProcessingRequirements Processing { get; set; }
    public List<Material> Byproducts { get; set; }
}
```

**2. Geographic Material Distribution**

```csharp
public class GeologicalMaterialMapper
{
    public MaterialQuality GetMaterialAtLocation(
        GeoCoordinate location,
        float depth,
        MaterialType targetMaterial)
    {
        // Use BlueMarble's geological simulation
        var geologicalData = GetGeologicalFormation(location, depth);
        
        // Material quality based on formation
        var quality = CalculateFormationQuality(geologicalData);
        
        // Purity based on geological processes
        var purity = CalculateOrePurity(geologicalData, targetMaterial);
        
        return new MaterialQuality
        {
            BaseQuality = quality,
            Purity = purity,
            ExtractionDifficulty = CalculateDifficulty(geologicalData, depth),
            Yield = CalculateYield(purity, geologicalData)
        };
    }
}
```

### Integration with Existing BlueMarble Systems

**Connections to Current Documentation:**

```
Material Quality System Integration:

├── crafting-quality-model.md
│   └── Extend with MO2-style material properties
│   └── Add material selection phase
│   └── Implement property-based quality calculation
│
├── mechanics-research.md
│   └── Material quality grades → Multi-property system
│   └── Enhanced trade-off complexity
│   └── Geographic specialization mechanics
│
├── skill-knowledge-system-research.md
│   └── MO2 skill progression model
│   └── Knowledge discovery integration
│   └── Crafting skill impact on quality
│
└── implementation-plan.md
    └── Phase 1: Enhanced Material Properties (Month 2)
    └── Already planned, now informed by MO2 research
```


## Recommendations

### Immediate Actions

**1. Adopt Multi-Property Material System**
- **Description:** Implement material properties (hardness, density, durability, workability) instead of simple quality grades
- **Rationale:** Aligns with BlueMarble's geological realism, creates meaningful trade-offs, supports depth
- **Priority:** High
- **Timeline:** Phase 1 (Month 2) - Already planned in implementation-plan.md
- **Implementation:**
  - Define core material properties based on real geology
  - Map geological data to crafting-relevant properties
  - Create material comparison UI/UX
  - Implement property-based quality calculation

**2. Implement Geographic Material Specialization**
- **Description:** Different geological regions yield different material types and qualities
- **Rationale:** Creates territorial value, encourages exploration, supports player-driven economy
- **Priority:** High
- **Timeline:** Phase 1-2 (Months 2-4)
- **Implementation:**
  - Use geological formation data for material distribution
  - Implement location-based material quality
  - Create resource mapping tools for players
  - Design trade route mechanics

**3. Design Progressive Material Complexity**
- **Description:** Layer material system complexity for different player experience levels
- **Rationale:** Avoid overwhelming new players while providing depth for experienced players
- **Priority:** High
- **Timeline:** Phase 2 (Months 3-5)
- **Implementation:**
  - Basic materials: Simple properties, easy to understand
  - Advanced materials: Complex trade-offs
  - Expert materials: Rare combinations, experimental
  - UI modes: Simple view vs Detailed view

### Long-term Considerations

**Material Discovery Mechanics**
- Implement MO2-style experimentation for alloy/composite creation
- Allow player-driven recipe discovery
- Create knowledge sharing mechanics (teaching, guild libraries)
- Balance discovery rate for long-term engagement

**Economic Integration**
- Design material-driven market dynamics
- Support specialization roles (extractors, processors, crafters)
- Implement regional trading mechanics
- Create scarcity and abundance cycles

**Skill Progression Balance**
- Adopt MO2's use-based skill improvement
- Faster initial progression than MO2 (first 50 hours)
- Extended mastery ceiling (100-200 hours for master level)
- Meaningful skill differentiation at all levels

**Social Systems**
- Guild material storage and sharing
- Mentor/apprentice mechanics
- Reputation systems for crafters
- Community knowledge building tools

### Areas for Further Research

**User Experience for Complex Systems**
- How to present 6+ material properties without overwhelming?
- What UI/UX patterns work best for material comparison?
- How to teach material system naturally through gameplay?

**Balance Testing Required**
- Optimal number of distinct materials (MO2 has 100+)
- Property value ranges for interesting trade-offs
- Skill progression curve testing with players
- Economic balance for material pricing

**Alternative Approaches to Explore**
- Tiered material revelation (unlock complexity gradually)
- Material "signature" system (simplified comparison)
- AI-assisted material selection for casual players
- Procedural material generation for infinite variety

### Implementation Priority Matrix

```
High Priority + High Impact:
├── Multi-property material system
├── Geographic distribution
└── Basic crafting quality calculation

High Priority + Medium Impact:
├── Progressive complexity
├── Material extraction mechanics
└── Skill progression system

Medium Priority + High Impact:
├── Experimentation/discovery
├── Knowledge progression
└── Economic integration

Medium Priority + Medium Impact:
├── Social systems
├── UI/UX optimization
└── Tutorial/onboarding

Low Priority:
├── Advanced alloy systems
├── Procedural generation
└── Complex market simulation
```

## Implementation Considerations

### Technical Architecture

**Material Data Structure:**
```csharp
// Extend existing BlueMarble material system
public class EnhancedGameMaterial : IMaterial
{
    // BlueMarble existing properties
    public MaterialId Id { get; set; }
    public string Name { get; set; }
    public MineralComposition Composition { get; set; }
    
    // MO2-inspired game properties
    public MaterialProperties PhysicalProperties { get; set; }
    public ProcessingRequirements Processing { get; set; }
    public GeographicDistribution Distribution { get; set; }
    public EconomicProperties Economics { get; set; }
}

public class MaterialProperties
{
    public float Hardness { get; set; }        // 1-10 (Mohs scale)
    public float Density { get; set; }         // g/cm³
    public float Durability { get; set; }      // 1-100
    public float Workability { get; set; }     // 1-100 (ease of processing)
    public float Flexibility { get; set; }     // 1-100
    
    // Calculated composite scores
    public float ToolEffectiveness => CalculateToolScore();
    public float ArmorEffectiveness => CalculateArmorScore();
    public float WeightClass => CalculateWeight();
}
```

### Database Schema Extensions

```sql
-- Material Properties Table
CREATE TABLE MaterialProperties (
    MaterialId INT PRIMARY KEY,
    Hardness FLOAT,
    Density FLOAT,
    Durability FLOAT,
    Workability FLOAT,
    Flexibility FLOAT,
    BaseValue DECIMAL(10,2)
);

-- Geographic Material Quality
CREATE TABLE MaterialNodeQuality (
    NodeId BIGINT PRIMARY KEY,
    Latitude FLOAT,
    Longitude FLOAT,
    Depth FLOAT,
    MaterialId INT,
    QualityModifier FLOAT,  -- 0.5 to 1.5
    Purity FLOAT,           -- 0-100%
    Yield FLOAT,            -- Units per extraction
    DepletionLevel FLOAT    -- 0-100%
);

-- Player Material Knowledge
CREATE TABLE PlayerMaterialKnowledge (
    PlayerId BIGINT,
    MaterialId INT,
    UnderstandingLevel FLOAT,  -- 0-100%
    TimesUsed INT,
    FirstDiscovered TIMESTAMP,
    PRIMARY KEY (PlayerId, MaterialId)
);
```

### UI/UX Recommendations

**Material Comparison Tool:**
```
┌─────────────────────────────────────────────────┐
│  Compare Materials: Iron Ores                   │
├─────────────────┬──────────┬──────────┬─────────┤
│ Property        │ Granum   │ Calx     │ Cuprum  │
├─────────────────┼──────────┼──────────┼─────────┤
│ Durability      │ ████ 80  │ ███ 65   │ ██ 45   │
│ Weight          │ ███ 70   │ ████ 85  │ ██ 50   │
│ Workability     │ ███ 60   │ ████ 75  │ ████ 80 │
│ Cost (per unit) │ 15g      │ 10g      │ 8g      │
│ Availability    │ Common   │ Common   │ Uncommon│
├─────────────────┴──────────┴──────────┴─────────┤
│ Best For:                                        │
│ Granum: Heavy armor, high durability needs      │
│ Calx: Balanced approach, cost-effective         │
│ Cuprum: Lightweight items, easy processing      │
└──────────────────────────────────────────────────┘
```

## Next Steps

### Immediate Actions Required

- [x] **Complete Research Documentation** - This document
  - Due: 2025-01-15
  - Owner: Research Team
  
- [ ] **Review with Design Team** - Discuss recommendations and priorities
  - Due: 2025-01-20
  - Owner: Lead Designer
  
- [ ] **Update Implementation Plan** - Integrate MO2 findings into existing roadmap
  - Due: 2025-01-25
  - Owner: Technical Lead
  - Reference: `research/game-design/implementation-plan.md`

- [ ] **Prototype Material Properties** - Create proof-of-concept for multi-property system
  - Due: 2025-02-01
  - Owner: Development Team
  
- [ ] **Design UI Mockups** - Material selection and comparison interfaces
  - Due: 2025-02-01
  - Owner: UI/UX Designer

### Follow-up Research

**Suggested Additional Research Topics:**

1. **User Testing: Material System Complexity**
   - Topic: How much complexity can players handle?
   - Method: Prototype testing with varying complexity levels
   - Timeline: 2-3 weeks
   - Priority: High

2. **Economic Simulation: Material Markets**
   - Topic: Market dynamics with multi-property materials
   - Method: Economic modeling and simulation
   - Timeline: 3-4 weeks
   - Priority: Medium

3. **Comparative UI/UX Study**
   - Topic: Best practices for presenting complex material data
   - Method: Analyze successful implementations in other games
   - Timeline: 1-2 weeks
   - Priority: High

### Stakeholder Communication

**Design Team Presentation:**
- Schedule: Week of 2025-01-20
- Format: Slide deck with key findings and recommendations
- Focus: Actionable takeaways and implementation roadmap

**Development Team Sync:**
- Schedule: Week of 2025-01-27
- Format: Technical deep-dive on implementation
- Focus: Architecture, data structures, integration points

### Documentation Updates

**Files to Update:**

1. **research/RESEARCH_ISSUES_SUMMARY.md**
   - Add MO2 material research to completed research section
   - Mark research issue as complete
   - Link to this document

2. **research/game-design/implementation-plan.md**
   - Integrate MO2 findings into Phase 1 material system work
   - Add specific MO2-inspired features to roadmap

3. **docs/gameplay/mechanics/crafting-quality-model.md**
   - Extend with multi-property material system
   - Add MO2-style material selection phase

4. **research/game-design/skill-knowledge-system-research.md**
   - Add detailed material system analysis section
   - Update recommendations based on research

## Appendices

### Appendix A: Research Sources

**Primary Sources:**
- Mortal Online 2 Official Wiki: https://mortalonline2.wiki/wiki/Main_Page
- Mortal Online 2 Official Website: https://www.mortalonline2.com/
- Star Vault Developer Blog Posts (2020-2024)

**Secondary Sources:**
- YouTube: "Mortal Online 2 Crafting Guide" series
- Reddit: r/MortalOnline discussions on material meta
- Community Spreadsheets: Material property calculators
- Player-created guides on Steam Community

### Appendix B: Material Property Examples

**Example Material Comparison: Weapons**

```
Sword Materials Comparison:

Steel:
├── Durability: 8100
├── Weight: 5800 (Heavy)
├── Hardness: 180
├── Damage: 45 base
├── Cost: ~50g per unit
└── Best for: Balanced melee combat

Bone Tissue:
├── Durability: 4500
├── Weight: 1200 (Very Light)
├── Hardness: 90
├── Damage: 35 base
├── Cost: ~10g per unit
└── Best for: Mobile/skirmish builds

Tungsteel (Steel + Tungsten):
├── Durability: 15000
├── Weight: 7200 (Very Heavy)
├── Hardness: 250
├── Damage: 52 base
├── Cost: ~200g per unit
└── Best for: Elite heavy fighters
```

### Appendix C: Skill Progression Data

**Crafting Skill Progression Timeline:**

```
Weapon Smithing Progression (Use-Based):

Hours 0-10: Novice (Skill 0-20)
├── Success Rate: 40-60%
├── Material Waste: 40-50%
├── Quality Range: 30-60%
└── Activities: Practice swords, daggers

Hours 10-30: Apprentice (Skill 20-40)
├── Success Rate: 60-75%
├── Material Waste: 25-40%
├── Quality Range: 50-75%
└── Activities: Standard weapons, first sales

Hours 30-80: Journeyman (Skill 40-60)
├── Success Rate: 75-90%
├── Material Waste: 15-25%
├── Quality Range: 65-85%
└── Activities: Quality weapons, build reputation

Hours 80-150: Expert (Skill 60-80)
├── Success Rate: 90-95%
├── Material Waste: 10-15%
├── Quality Range: 75-95%
└── Activities: Premium weapons, experimentation

Hours 150-300+: Master (Skill 80-100)
├── Success Rate: 95-99%
├── Material Waste: 5-10%
├── Quality Range: 85-105% (masterwork possible)
└── Activities: Legendary weapons, discoveries
```

### Appendix D: Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-01-15 | BlueMarble Research Team | Initial comprehensive research document |

---

**Document Status:** ✅ COMPLETE - Ready for team review and implementation planning

**Related Documentation:**
- [Skill and Knowledge System Research](./skill-knowledge-system-research.md)
- [Mechanics Research](./mechanics-research.md)
- [Implementation Plan](./implementation-plan.md)
- [Crafting Quality Model](../../docs/gameplay/mechanics/crafting-quality-model.md)
- [Player Freedom Analysis](./player-freedom-analysis.md)
