# Mortal Online 2 Material and Quality System Research

**Document Type:** Market Research & System Analysis  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2024  
**Status:** Research Report  
**Research Type:** Market Research  
**Priority:** Low

## Executive Summary

This research analyzes Mortal Online 2's material grading and crafting quality systems to identify applicable mechanics for BlueMarble's geological simulation game. Mortal Online 2's open-ended material system emphasizes player agency, experimentation, and emergent complexity through realistic material properties and player-driven quality outcomes.

**Key Findings:**
- Material quality directly impacts crafted item statistics and durability
- Open-ended crafting allows experimentation with material combinations
- Player skill and tool quality create meaningful progression
- No arbitrary class restrictions enable pure player-driven specialization
- Material discovery and knowledge sharing drive economic gameplay
- Quality variance creates natural market stratification

**Relevance to BlueMarble:**
BlueMarble's geological simulation provides an unprecedented foundation for implementing Mortal Online 2's material philosophy with scientifically accurate geological properties, creating a deeper and more authentic material system than currently exists in any game.

## Table of Contents

1. [Research Methodology](#research-methodology)
2. [Mortal Online 2 Material System Overview](#mortal-online-2-material-system-overview)
3. [Material Grading Mechanics](#material-grading-mechanics)
4. [Quality System Analysis](#quality-system-analysis)
5. [Player Choice and Agency](#player-choice-and-agency)
6. [Open-Ended Crafting Mechanics](#open-ended-crafting-mechanics)
7. [Player-Driven Economy](#player-driven-economy)
8. [Knowledge Discovery and Sharing](#knowledge-discovery-and-sharing)
9. [BlueMarble Integration Recommendations](#bluemarble-integration-recommendations)
10. [Implementation Considerations](#implementation-considerations)
11. [Conclusion](#conclusion)

## Research Methodology

### Research Approach

**Primary Sources:**
- Mortal Online 2 official wiki: https://mortalonline2.wiki/wiki/Main_Page
- Official website: https://www.mortalonline2.com/
- Community forums and player discussions
- Gameplay videos demonstrating crafting systems
- Developer interviews and patch notes

**Analysis Framework:**
1. **Material Properties**: How materials differ and impact outcomes
2. **Quality Mechanics**: How quality is determined and affects gameplay
3. **Player Agency**: Decision points and meaningful choices
4. **Experimentation**: Discovery and learning systems
5. **Economic Impact**: How materials drive player economy
6. **Specialization**: Paths for character differentiation

**Focus Areas:**
- Material grading systems
- Crafting quality calculations
- Player interaction with material systems
- Knowledge acquisition and sharing
- Economic implications of material variance

### Limitations and Scope

**In Scope:**
- Material quality and grading systems
- Crafting mechanics related to materials
- Player choice in material selection
- Economic systems driven by material quality

**Out of Scope:**
- Combat systems (except as they relate to gear quality)
- Territory control mechanics (covered in other research)
- Full loot PvP mechanics (not applicable to BlueMarble)
- Character attribute systems (covered in skill research)

## Mortal Online 2 Material System Overview

### Core Philosophy

Mortal Online 2's material system is built on principles of realism, experimentation, and player agency:

1. **Material Diversity**: Dozens of materials with unique properties
2. **No Hand-Holding**: Players discover material properties through use
3. **Realistic Properties**: Materials behave according to logical physical properties
4. **Combination Freedom**: Nearly any material can be combined in crafting
5. **Quality Variance**: Same material type can have different quality levels
6. **Knowledge Value**: Understanding materials becomes valuable player knowledge

### Material Categories

**Metals:**
- Iron (common, versatile, moderate properties)
- Steel (processed from iron, stronger, more expensive)
- Tungsteel (high-end, excellent properties, rare)
- Cronite (end-game, best properties, very rare)
- Cuprum (copper-based, lighter, different use cases)
- Pig Iron, Cast Iron variants
- Gold, Silver (decorative and special properties)

**Organic Materials:**
- Wood types (Spongewood, Ironwood, Bloodwood, etc.)
- Leather types (from different creatures)
- Bone and Carapace
- Silk and Cotton
- Animal parts (tendons, sinew, etc.)

**Stone and Minerals:**
- Granum (stone for construction)
- Calx (processed stone material)
- Saburra (sand-based material)
- Various gemstones

**Processed Materials:**
- Steel alloys (combining multiple materials)
- Treated leathers
- Refined textiles
- Composite materials

### Material Property System

Each material in Mortal Online 2 has distinct properties:

**Physical Properties:**
- **Weight**: Affects carry capacity and stamina drain
- **Durability**: How long items last before breaking
- **Strength**: Resistance to damage
- **Flexibility**: Affects weapon swing speed and armor mobility
- **Density**: Impacts mass and momentum

**Combat Properties (for equipment):**
- **Slashing Damage**: Effectiveness against unarmored targets
- **Piercing Damage**: Effectiveness against armored targets
- **Blunt Damage**: Crushing effectiveness
- **Defense Values**: Protection provided by armor

**Crafting Properties:**
- **Yield**: How much processed material obtained from raw
- **Processing Difficulty**: Skill requirements
- **Compatibility**: Which materials combine well
- **Aesthetic Properties**: Visual appearance of final product

## Material Grading Mechanics

### Quality Tiers

Mortal Online 2 uses a percentage-based quality system (0-100%):

**Quality Ranges:**
```
Poor Quality:     1-35%  - Significantly reduced performance
Standard Quality: 36-65% - Baseline performance
Good Quality:     66-85% - Enhanced performance
Excellent Quality: 86-95% - Superior performance
Masterwork:       96-100% - Maximum performance
```

### Quality Impact on Performance

Quality percentage directly scales item properties:

**Weapon Example:**
```
Base Damage: 50
Quality Multiplier: quality_percentage / 100

60% Quality Weapon:
- Actual Damage: 50 × 0.60 = 30 damage
- Durability: Base durability × 0.60

95% Quality Weapon:
- Actual Damage: 50 × 0.95 = 47.5 damage  
- Durability: Base durability × 0.95
```

**Armor Example:**
```
Base Protection: 30
Quality Multiplier: quality_percentage / 100

60% Quality Armor:
- Actual Protection: 30 × 0.60 = 18 protection
- Weight: May be higher due to poor construction
- Durability: Base durability × 0.60

95% Quality Armor:
- Actual Protection: 30 × 0.95 = 28.5 protection
- Weight: Optimized, potentially lighter
- Durability: Base durability × 0.95
```

### Material Quality Sources

Quality in Mortal Online 2 comes from multiple sources:

**1. Raw Material Quality:**
- Resource nodes have inherent quality ranges
- Higher-tier nodes yield better materials
- Extraction skill affects obtained quality
- Some locations have premium material sources

**2. Processing Quality:**
- Refining raw materials can improve or maintain quality
- Crafter skill level affects processing success
- Better tools improve processing outcomes
- Failed processing reduces quality

**3. Crafting Quality:**
- Final crafting step determines item quality
- Multiple material qualities are averaged/calculated
- Crafter skill provides quality bonuses
- Critical success can boost quality significantly

### Quality Calculation Example

**Crafting a Steel Sword:**

```
Material Inputs:
- Steel Blade Material: 85% quality
- Wood Handle Material: 70% quality
- Binding Material: 60% quality

Weighted Average (based on material importance):
- Blade (primary, 70% weight): 85% × 0.70 = 59.5
- Handle (secondary, 20% weight): 70% × 0.20 = 14.0
- Binding (tertiary, 10% weight): 60% × 0.10 = 6.0
Base Quality = 59.5 + 14.0 + 6.0 = 79.5%

Crafter Skill Bonus:
- Blade Crafting Skill: 80/100
- Skill bonus: +5% for high skill
Adjusted Quality = 79.5% + 5% = 84.5%

Random Variance:
- Natural variance: ±3%
- Roll result: +2%
Final Quality = 84.5% + 2% = 86.5% (Excellent tier)
```

## Quality System Analysis

### Multi-Stage Quality Pipeline

Mortal Online 2's quality system operates through multiple stages:

**Stage 1: Resource Extraction**
```
Raw Node Quality → Extraction Skill → Tool Quality → Obtained Material Quality
```
- Node location determines base quality range
- Extraction skill increases quality ceiling
- Tool quality affects extraction success rate
- Higher-quality resources are rarer

**Stage 2: Material Processing**
```
Raw Material → Processing Method → Crafter Skill → Processed Material
```
- Different processing methods available
- Some methods improve quality, others preserve it
- Processing can fail, reducing quality
- Batch processing vs. careful processing trade-offs

**Stage 3: Item Crafting**
```
Multiple Materials → Recipe → Crafting Skill → Final Item Quality
```
- Recipe determines material requirements and weights
- Crafting skill provides quality bonuses
- Critical successes can exceed material quality
- Failures result in lower quality or material loss

### Quality Progression Systems

**Skill-Quality Relationship:**

Players progress through quality tiers as they develop:

```
Novice Crafter (Skill 0-30):
- Typically produces: 30-50% quality items
- Wastes materials frequently
- Cannot reliably use high-quality inputs
- Limited recipe access

Journeyman Crafter (Skill 30-60):
- Typically produces: 50-75% quality items
- Reasonable material efficiency
- Can utilize good quality materials
- Access to standard recipes

Expert Crafter (Skill 60-85):
- Typically produces: 70-90% quality items
- Efficient material usage
- Benefits from excellent materials
- Access to advanced recipes

Master Crafter (Skill 85-100):
- Typically produces: 85-100% quality items
- Minimal material waste
- Maximizes material potential
- Can craft legendary items with right materials
```

### Quality Visibility and Information

**What Players Can See:**
- Material quality percentages (when identified)
- Final item quality percentages
- General quality tier (Poor, Standard, Good, etc.)
- Comparative quality (better/worse indicators)

**What Players Must Discover:**
- Optimal material combinations
- Processing methods that preserve quality
- Locations of high-quality resource nodes
- Recipes that maximize material synergy
- Skill thresholds for quality tiers

**Information Asymmetry:**
- Experienced crafters have knowledge advantage
- Material quality knowledge becomes valuable
- Players can specialize in material sourcing
- Teaching and learning systems emerge naturally

## Player Choice and Agency

### Material Selection Decisions

Players face meaningful choices at every stage:

**Resource Gathering Phase:**
```
Decision Points:
1. Location Selection
   - Near locations: Lower quality, convenient
   - Distant locations: Higher quality, risky travel
   - Secret locations: Best quality, knowledge-gated

2. Extraction Method
   - Quick extraction: Lower quality, faster
   - Careful extraction: Higher quality, slower
   - Tool selection: Better tools = better quality

3. Quality vs. Quantity
   - Accept lower quality for volume
   - Pursue high quality selectively
   - Balance based on market demand
```

**Processing Phase:**
```
Decision Points:
1. Processing Method Selection
   - Standard processing: Preserves quality
   - Advanced processing: Can improve quality, risky
   - Batch processing: Efficient but variable quality

2. Material Combination
   - Pure materials: Predictable properties
   - Alloys/mixtures: Experimental, potentially superior
   - Quality matching: Combine similar qualities

3. Risk Management
   - Process safe materials: Guaranteed outcome
   - Experiment with rare materials: High risk/reward
   - Learn through controlled experiments
```

**Crafting Phase:**
```
Decision Points:
1. Material Investment
   - Use cheap materials: Practice and learning
   - Use good materials: Reliable products
   - Use excellent materials: Masterwork attempts

2. Recipe Selection
   - Standard recipes: Known outcomes
   - Experimental recipes: Discovery potential
   - Optimized recipes: Learned through experience

3. Target Market
   - Mass market: Volume over quality
   - Premium market: Quality over volume
   - Custom orders: Client specifications
```

### Build Diversity Through Materials

Material choices enable specialization without class restrictions:

**Gatherer Specialist Paths:**
```
Mining Specialist:
- Knows premium ore locations
- Efficient extraction techniques
- Ore quality identification expert
- Trades raw materials to crafters

Woodcutter Specialist:
- Knows rare wood locations
- Understands wood property variations
- Supplies crafters with specific wood types
- Trades in exotic materials
```

**Crafter Specialist Paths:**
```
Weaponsmith:
- Specializes in metal weapons
- Knows optimal alloy combinations
- Masters heat treatment techniques
- Creates high-end combat gear

Armorsmith:
- Specializes in protective equipment
- Balances protection vs. weight
- Expert in material layering
- Supplies military equipment

Bowyer:
- Specializes in ranged weapons
- Masters wood and composite materials
- Understands tension and flexibility
- Supplies hunters and archers
```

**Trader Specialist Paths:**
```
Material Broker:
- Connects gatherers with crafters
- Maintains quality standard inventories
- Speculates on material markets
- Provides material sourcing services

Equipment Dealer:
- Buys from crafters, sells to users
- Maintains varied quality inventory
- Understands market pricing
- Provides upgrade services
```

### Experimentation and Discovery

Player agency extends to material experimentation:

**Discovery Systems:**

1. **Material Combination Discovery**
```
Example: Steel Alloys
- Pure iron: Known properties
- Iron + carbon: Creates steel (discovered through experimentation)
- Steel + tungsten: Creates tungsteel (advanced discovery)
- Optimal ratios: Learned through trial and error
```

2. **Processing Method Discovery**
```
Example: Leather Treatment
- Raw leather: Basic properties
- Oil treatment: Flexibility improvement (common knowledge)
- Alchemical treatment: Special properties (rare knowledge)
- Combined treatments: Experimental results
```

3. **Recipe Optimization**
```
Example: Sword Construction
- Standard recipe: Known outcome
- Alternative handle materials: Different performance
- Different binding methods: Durability variations
- Handle length variations: Balance changes
```

**Knowledge Spread:**
- Players share discoveries (or keep secrets)
- Experimentation drives content creation
- Community knowledge bases emerge
- Meta-gaming around optimal materials

## Open-Ended Crafting Mechanics

### Recipe Flexibility

Mortal Online 2 allows significant freedom in crafting:

**Flexible Material Substitution:**
```
Sword Recipe Base:
- Primary Material: Blade (metal)
  - Can use: Iron, Steel, Tungsteel, Cronite
  - Each gives different damage/weight/durability

- Secondary Material: Handle (wood/bone/leather)
  - Can use: Spongewood, Ironwood, Leather-wrapped
  - Each affects weight, grip, aesthetics

- Tertiary Material: Binding (leather/fiber/wire)
  - Can use: Leather strips, Plant fiber, Metal wire
  - Each affects durability and maintenance
```

**Material Property Inheritance:**
```
Final Weapon Properties = f(material_properties, crafting_quality)

Steel Sword with Ironwood Handle:
- Damage: Primarily from steel blade
- Weight: Steel + Ironwood (heavier handle)
- Swing Speed: Modified by total weight
- Durability: Weighted average of materials
- Appearance: Visual combination of materials
```

### Crafting as Skill Expression

Open-ended crafting enables crafter identity:

**Signature Builds:**
```
Crafter A: "The Practical Smith"
- Philosophy: Balance and reliability
- Material choices: Steel + Spongewood
- Result: Medium weight, good durability, affordable
- Market: Mid-tier fighters, everyday use

Crafter B: "The Perfectionist"
- Philosophy: Maximum performance regardless of cost
- Material choices: Tungsteel + Ironwood
- Result: High damage, excellent durability, expensive
- Market: Elite fighters, prestigious orders

Crafter C: "The Experimentalist"
- Philosophy: Unique material combinations
- Material choices: Unusual alloys and composites
- Result: Specialized properties, unpredictable
- Market: Players seeking specific advantages
```

### Quality-Driven Crafting Loops

The open system creates engaging feedback loops:

**Learning Loop:**
```
1. Craft with basic materials → Learn base mechanics
2. Experiment with better materials → Discover quality impact
3. Fail with expensive materials → Learn skill requirements
4. Practice with medium materials → Improve skill
5. Succeed with excellent materials → Create masterworks
6. Teach others or keep knowledge → Economic decisions
```

**Economic Loop:**
```
1. Identify market demand → Choose crafting focus
2. Source appropriate quality materials → Gatherer relationships
3. Craft items at target quality → Skill application
4. Price based on quality and cost → Economic strategy
5. Compete with other crafters → Market dynamics
6. Adjust strategy based on results → Adaptation
```

**Reputation Loop:**
```
1. Craft items of consistent quality → Build reputation
2. Gain known for specific items → Specialization recognition
3. Receive custom orders → Premium pricing
4. Source rare materials for orders → Supply chain
5. Deliver exceptional results → Enhanced reputation
6. Become sought-after crafter → Market influence
```

## Player-Driven Economy

### Material Markets

Material quality creates natural market stratification:

**Market Tiers:**

```
Budget Market:
- Materials: 30-50% quality
- Buyers: New players, practice crafters
- Volume: High
- Price: Low
- Competition: High

Standard Market:
- Materials: 50-70% quality
- Buyers: Average players, regular use
- Volume: Medium-High
- Price: Moderate
- Competition: Medium

Premium Market:
- Materials: 70-90% quality
- Buyers: Serious players, important gear
- Volume: Medium
- Price: High
- Competition: Medium-Low

Elite Market:
- Materials: 90-100% quality
- Buyers: Top-tier players, collectors
- Volume: Low
- Price: Very High
- Competition: Low (few can supply)
```

### Supply Chain Complexity

Material quality affects entire economy:

**Vertical Integration Example:**
```
Self-Sufficient Weaponsmith:
1. Mines own ore (75% quality achieved)
2. Processes ore to ingots (maintains 75%)
3. Crafts weapons (produces 80% quality weapons)
4. Sells directly to customers
5. Controls entire supply chain

Advantage: Full profit, quality control
Disadvantage: Time-intensive, skill requirements
```

**Specialized Supply Chain Example:**
```
Collaborative Production:
1. Elite Miner extracts 90% quality ore
2. Sells to Master Smelter who maintains quality
3. Sells ingots to Expert Weaponsmith
4. Weaponsmith creates 95% quality weapons
5. Trader sells to end customers

Advantage: Each specialist excels, highest quality possible
Disadvantage: Multiple profit-takers, coordination needed
```

### Price Discovery and Market Dynamics

Quality creates complex pricing:

**Pricing Factors:**
```
Base Item Price = material_cost + labor_cost + skill_premium + rarity_bonus

Material Cost Scaling:
- 50% quality material: 1.0x base cost
- 70% quality material: 2.5x base cost
- 90% quality material: 8.0x base cost
- 100% quality material: 20.0x base cost

Labor Cost Scaling:
- Novice crafter: 0.5x labor value
- Journeyman crafter: 1.0x labor value
- Expert crafter: 2.5x labor value
- Master crafter: 5.0x labor value

Final Item Pricing:
- 50% quality sword: 100 gold (materials) + 50 gold (labor) = 150 gold
- 70% quality sword: 250 gold (materials) + 100 gold (labor) = 350 gold
- 90% quality sword: 800 gold (materials) + 250 gold (labor) = 1,050 gold
- 100% quality sword: 2,000 gold (materials) + 500 gold (labor) = 2,500 gold
```

**Market Competition:**
- Multiple crafters compete on quality and price
- Reputation influences pricing power
- Material sourcing becomes competitive advantage
- Knowledge of optimal recipes creates value

### Player-to-Player Trading

Material quality drives trading interactions:

**Trading Scenarios:**

```
Scenario 1: Material Sourcing
Crafter: "Need 85%+ quality steel, willing to pay premium"
Miner: "I have 87% quality steel ingots, 300 gold each"
Crafter: "I'll take 20, plus 10% bonus if you can supply weekly"
→ Establishes ongoing business relationship

Scenario 2: Quality Guarantee
Buyer: "Looking for 90%+ quality armor set"
Crafter: "I can craft it, but need you to provide 90%+ materials"
Buyer: "I'll source materials, you craft, we split the quality risk"
→ Collaborative quality targeting

Scenario 3: Material Trade-Up
Trader A: "I have 50 units of 60% quality iron"
Trader B: "I'll trade 15 units of 80% quality iron"
→ Volume-quality arbitrage

Scenario 4: Knowledge Exchange
Experienced Crafter: "I'll teach you the tungsteel recipe"
New Crafter: "I'll supply you with premium materials for a month"
→ Knowledge as valuable commodity
```

## Knowledge Discovery and Sharing

### Material Knowledge Systems

Knowledge about materials becomes game content:

**Types of Material Knowledge:**

1. **Location Knowledge**
```
Common Knowledge:
- Basic iron ore found in mountains
- Wood available in forests

Valuable Knowledge:
- Specific high-quality ore veins
- Rare wood groves
- Seasonal availability patterns

Secret Knowledge:
- Hidden legendary material nodes
- Dangerous but rewarding locations
- Time-limited spawns
```

2. **Property Knowledge**
```
Basic Knowledge:
- Iron is heavier than wood
- Steel is stronger than iron

Intermediate Knowledge:
- Tungsteel has better damage/weight ratio
- Ironwood provides best handle properties

Advanced Knowledge:
- Optimal material combinations for specific builds
- Quality preservation techniques
- Material synergy effects
```

3. **Processing Knowledge**
```
Common Knowledge:
- How to smelt basic ores
- How to tan basic leather

Specialized Knowledge:
- Advanced alloy recipes
- Quality-preserving processing methods
- Efficient batch processing techniques

Master Knowledge:
- Legendary material combinations
- Secret processing methods
- Quality enhancement techniques
```

### Knowledge Acquisition Methods

Players learn through multiple channels:

**Experimentation:**
```
Trial and Error Process:
1. Observe: "What materials are available?"
2. Hypothesize: "This combination might work well"
3. Test: Craft an item with chosen materials
4. Analyze: "The result was better/worse than expected"
5. Refine: Adjust approach based on results
6. Repeat: Build comprehensive understanding

Cost: Materials and time
Benefit: First-hand knowledge, potential discoveries
```

**Social Learning:**
```
Community Knowledge Sharing:
- Guild teachings (often require membership)
- Mentor-apprentice relationships
- Public forums and wikis (player-created)
- Trade secrets (closely guarded)
- Observation (watching others craft)

Cost: Social capital, reciprocity
Benefit: Faster learning, proven methods
```

**Economic Learning:**
```
Market Analysis:
- Observe: Which materials command premium prices?
- Deduce: High prices indicate desirability
- Investigate: Why are these materials valuable?
- Apply: Focus gathering/crafting on valuable materials

Cost: Market research time
Benefit: Economically optimized knowledge
```

### Knowledge as Economic Asset

Material knowledge has economic value:

**Knowledge Trading:**
```
Valuable Knowledge Types:
1. Resource Location Information
   - "I'll show you the 85%+ iron vein for 500 gold"
   - One-time payment or profit-sharing arrangement

2. Recipe Information
   - "I'll teach you the tungsteel recipe for 1,000 gold"
   - May require secrecy agreement

3. Quality Technique Information
   - "I'll show you how to consistently hit 90% quality"
   - Often requires apprenticeship arrangement

4. Market Intelligence
   - "I'll tell you where to sell your materials for 20% more"
   - Ongoing relationship or percentage-based arrangement
```

**Knowledge Monopolies:**
```
Scenario: Rare Recipe Monopoly
- Only a few players know the recipe
- They can charge premium prices
- Knowledge eventually spreads
- Creates time-limited economic advantage

Scenario: Location Monopoly
- Player discovers exceptional resource node
- Keeps location secret
- Supplies market with premium materials
- Others search for alternative sources
```

### Community Knowledge Bases

Players create collective knowledge resources:

**Player-Created Resources:**
- Wiki pages with material properties
- Crafting calculators for quality prediction
- Resource location maps (sometimes incomplete)
- Recipe databases (constantly updated)
- Guide videos and tutorials

**Knowledge Gaps:**
- Some information deliberately withheld
- Experimental recipes not yet documented
- Secret locations known only to discoverers
- Advanced techniques learned through experience
- Constantly evolving meta-knowledge

## BlueMarble Integration Recommendations

### Leveraging Geological Simulation

BlueMarble can implement Mortal Online 2's material philosophy with scientific authenticity:

**Scientifically Accurate Material Properties:**

```
Instead of arbitrary material stats, use geological properties:

Hematite Iron Ore (70% Fe content):
- Density: 5.26 g/cm³ (real geological property)
- Hardness: 5-6 Mohs (affects extraction difficulty)
- Processing: Requires ~1,500°C smelting (realistic)
- Yield: 70% iron extraction (based on Fe content)
- Quality variance: Based on impurity levels in geological formation

Magnetite Iron Ore (60% Fe content):
- Density: 5.15 g/cm³
- Hardness: 5.5-6.5 Mohs
- Processing: Requires ~1,600°C smelting (magnetic properties)
- Yield: 60% iron extraction
- Quality variance: Based on titanium and other impurities
```

**Geological Formation Quality:**

```
BlueMarble's geological simulation determines material quality naturally:

High-Quality Ore Deposits:
- Formed under optimal geological conditions
- Lower impurity content
- Better crystal structure
- Higher extraction efficiency
- Example: Hydrothermal vein deposits

Standard-Quality Ore Deposits:
- Average geological formation conditions
- Moderate impurity levels
- Standard crystal structure
- Typical extraction efficiency
- Example: Sedimentary iron formations

Low-Quality Ore Deposits:
- Suboptimal formation conditions
- Higher impurity content
- Irregular structure
- Lower extraction efficiency
- Example: Bog iron deposits
```

### Enhanced Player Agency

BlueMarble can exceed Mortal Online 2's player agency:

**Geological Knowledge as Core Gameplay:**

```
Material Discovery Process:
1. Survey geological area → Identify potential resources
2. Analyze rock formations → Predict material quality
3. Test extraction methods → Discover optimal techniques
4. Share or guard knowledge → Economic decision
5. Adapt to geological changes → Dynamic gameplay

This is deeper than MO2 because:
- Real geological principles create consistent logic
- Knowledge is discoverable through scientific method
- Understanding transfers to real-world geology
- System is educational and entertaining
```

**Material Experimentation with Scientific Feedback:**

```
BlueMarble Advantage:
Instead of arbitrary material combinations, use real chemistry:

Steel Alloy Creation:
- Iron (Fe) + Carbon (C) = Steel
- Carbon content: 0.3-2.0% determines steel type
- Higher carbon = harder but more brittle
- Lower carbon = softer but tougher
- Players learn real metallurgy

Concrete Creation:
- Limestone + Clay + Water = Cement
- Aggregate (sand/gravel) + Cement = Concrete
- Mix ratios affect strength and workability
- Players learn real material science
```

### Quality System Implementation

Adapt MO2's quality system to BlueMarble:

**Multi-Factor Quality Calculation:**

```csharp
public class BlueMarbleQualitySystem
{
    public float CalculateResourceQuality(
        GeologicalFormation formation,
        ExtractionMethod method,
        PlayerSkill skill,
        ToolQuality tool)
    {
        // Base quality from geological formation
        float baseQuality = formation.MaterialPurity * 100f;
        
        // Geological factors
        float formationBonus = CalculateFormationBonus(formation);
        
        // Extraction skill impact
        float skillMultiplier = Mathf.Clamp(skill.Level / formation.DifficultyRating, 0.5f, 1.2f);
        
        // Tool quality impact
        float toolEfficiency = tool.Quality / 100f;
        
        // Extraction method efficiency
        float methodEfficiency = method.EfficiencyRating;
        
        // Random natural variance
        float variance = Random.Range(-5f, 5f);
        
        // Calculate final quality
        float quality = (baseQuality + formationBonus) * skillMultiplier * toolEfficiency * methodEfficiency;
        quality += variance;
        
        return Mathf.Clamp(quality, 1f, 100f);
    }
    
    private float CalculateFormationBonus(GeologicalFormation formation)
    {
        float bonus = 0f;
        
        // Optimal formation conditions
        if (formation.Temperature >= formation.OptimalTempMin && 
            formation.Temperature <= formation.OptimalTempMax)
        {
            bonus += 10f;
        }
        
        // Low impurity content
        if (formation.ImpurityLevel < 0.05f) // Less than 5% impurities
        {
            bonus += 15f;
        }
        
        // Ideal crystal structure
        if (formation.CrystalStructure == CrystalType.Ideal)
        {
            bonus += 10f;
        }
        
        return bonus;
    }
}
```

**Processing Quality Preservation:**

```csharp
public class MaterialProcessing
{
    public ProcessedMaterial ProcessMaterial(
        RawMaterial raw,
        ProcessingMethod method,
        PlayerSkill skill,
        Equipment equipment)
    {
        // Start with raw material quality
        float qualityInput = raw.Quality;
        
        // Processing method quality factor
        float methodQuality = method.QualityPreservation; // 0.7-1.1
        
        // Skill-based quality retention/improvement
        float skillBonus = CalculateSkillBonus(skill, method.Difficulty);
        
        // Equipment quality impact
        float equipmentBonus = (equipment.Quality / 100f) * 0.1f;
        
        // Process simulation (can fail)
        bool success = CheckProcessingSuccess(skill, method.Difficulty);
        
        if (success)
        {
            // Successful processing
            float outputQuality = qualityInput * methodQuality * (1f + skillBonus + equipmentBonus);
            
            // Critical success chance
            if (RollCriticalSuccess(skill))
            {
                outputQuality *= 1.15f; // 15% bonus on crit
            }
            
            return new ProcessedMaterial
            {
                Quality = Mathf.Clamp(outputQuality, 1f, 100f),
                Amount = CalculateYield(raw, method, success: true)
            };
        }
        else
        {
            // Processing failure
            float outputQuality = qualityInput * 0.6f; // 40% quality loss
            
            return new ProcessedMaterial
            {
                Quality = Mathf.Clamp(outputQuality, 1f, 100f),
                Amount = CalculateYield(raw, method, success: false) // Reduced yield
            };
        }
    }
    
    private float CalculateSkillBonus(PlayerSkill skill, float difficulty)
    {
        float relativeSkill = skill.Level / difficulty;
        
        if (relativeSkill >= 1.5f)
            return 0.15f; // 15% bonus for high skill
        else if (relativeSkill >= 1.0f)
            return 0.05f; // 5% bonus for adequate skill
        else
            return -0.10f; // 10% penalty for low skill
    }
    
    private bool CheckProcessingSuccess(PlayerSkill skill, float difficulty)
    {
        float successChance = Mathf.Clamp(skill.Level / difficulty, 0.3f, 0.95f);
        return Random.Range(0f, 1f) < successChance;
    }
    
    private bool RollCriticalSuccess(PlayerSkill skill)
    {
        float critChance = Mathf.Min(skill.Level * 0.001f, 0.1f); // Max 10% crit chance
        return Random.Range(0f, 1f) < critChance;
    }
}
```

### Open-Ended Crafting for BlueMarble

Enable MO2-style experimentation with geological materials:

**Flexible Recipe System:**

```csharp
public class FlexibleCraftingSystem
{
    public CraftedItem CraftItem(
        Recipe recipe,
        List<Material> providedMaterials,
        PlayerSkill skill,
        CraftingStation station)
    {
        // Validate materials meet recipe requirements
        if (!ValidateMaterialsForRecipe(recipe, providedMaterials))
        {
            return null; // Invalid combination
        }
        
        // Calculate combined material quality
        float materialQuality = CalculateCombinedQuality(recipe, providedMaterials);
        
        // Apply crafting skill bonus
        float skillBonus = CalculateCraftingSkillBonus(skill, recipe.Difficulty);
        
        // Station quality bonus
        float stationBonus = (station.Quality / 100f) * 0.15f;
        
        // Check for critical success
        bool critical = RollCriticalCraft(skill, recipe.Difficulty);
        
        // Calculate final item quality
        float finalQuality = materialQuality * (1f + skillBonus + stationBonus);
        
        if (critical)
        {
            finalQuality = 100f; // Masterwork on critical
        }
        
        // Generate item with calculated properties
        return new CraftedItem
        {
            Recipe = recipe,
            Quality = Mathf.Clamp(finalQuality, 1f, 100f),
            Materials = providedMaterials,
            CrafterName = skill.PlayerName,
            Properties = CalculateItemProperties(recipe, providedMaterials, finalQuality)
        };
    }
    
    private float CalculateCombinedQuality(Recipe recipe, List<Material> materials)
    {
        float totalQuality = 0f;
        
        foreach (var requirement in recipe.MaterialRequirements)
        {
            var providedMaterial = materials.Find(m => m.Type == requirement.Type);
            float materialContribution = providedMaterial.Quality * requirement.Weight;
            totalQuality += materialContribution;
        }
        
        return totalQuality;
    }
    
    private Dictionary<string, float> CalculateItemProperties(
        Recipe recipe,
        List<Material> materials,
        float quality)
    {
        var properties = new Dictionary<string, float>();
        
        // Inherit properties from materials
        foreach (var material in materials)
        {
            // Weight
            if (!properties.ContainsKey("Weight"))
                properties["Weight"] = 0f;
            properties["Weight"] += material.Weight * material.AmountUsed;
            
            // Durability (weighted average)
            if (!properties.ContainsKey("Durability"))
                properties["Durability"] = 0f;
            properties["Durability"] += material.Durability * material.Weight;
            
            // Material-specific properties
            foreach (var prop in material.Properties)
            {
                if (!properties.ContainsKey(prop.Key))
                    properties[prop.Key] = 0f;
                properties[prop.Key] += prop.Value * material.Weight;
            }
        }
        
        // Apply quality multiplier to relevant properties
        if (properties.ContainsKey("Strength"))
            properties["Strength"] *= (quality / 100f);
        
        if (properties.ContainsKey("Durability"))
            properties["Durability"] *= (quality / 100f);
        
        return properties;
    }
}
```

### Knowledge Discovery System

Implement MO2's knowledge economy with geological context:

**Geological Survey System:**

```
Survey Mechanics:
1. Player performs geological survey in area
2. Survey reveals:
   - Rock types present
   - Estimated material quality ranges
   - Geological formation conditions
   - Extraction difficulty estimates

3. Survey quality based on:
   - Player geology skill
   - Survey equipment quality
   - Time invested in survey
   - Previous knowledge of area

4. Knowledge value:
   - First discoverer has information advantage
   - Can share, sell, or guard information
   - Knowledge spreads through community
   - Creates exploration incentive
```

**Material Property Research:**

```
Research System:
1. Player gathers material samples
2. Tests materials in various applications
3. Records results (automated notebook system)
4. Builds personal material knowledge database

Example Research Flow:
- Gather 5 different iron ore samples
- Test each in tool crafting
- Compare durability, performance, cost
- Identify optimal iron source for tools
- Knowledge advantage in material sourcing

Can share research:
- Publish findings (free knowledge, reputation gain)
- Sell research data (monetize knowledge)
- Keep secret (competitive advantage)
- Teach apprentices (build network)
```

## Implementation Considerations

### Technical Requirements

**Database Schema for Quality System:**

```sql
-- Material quality tracking
CREATE TABLE materials (
    material_id INT PRIMARY KEY,
    material_type VARCHAR(50),
    quality_percentage FLOAT,
    source_location_id INT,
    extraction_date TIMESTAMP,
    extracted_by_player_id INT,
    geological_formation_id INT
);

-- Crafted items with material history
CREATE TABLE crafted_items (
    item_id INT PRIMARY KEY,
    recipe_id INT,
    final_quality FLOAT,
    crafted_by_player_id INT,
    crafting_date TIMESTAMP,
    crafting_station_id INT
);

-- Material composition of crafted items
CREATE TABLE item_materials (
    item_id INT,
    material_id INT,
    amount_used FLOAT,
    contribution_weight FLOAT,
    FOREIGN KEY (item_id) REFERENCES crafted_items(item_id),
    FOREIGN KEY (material_id) REFERENCES materials(material_id)
);

-- Geological formations affecting quality
CREATE TABLE geological_formations (
    formation_id INT PRIMARY KEY,
    location_x FLOAT,
    location_y FLOAT,
    location_z FLOAT,
    formation_type VARCHAR(50),
    purity_level FLOAT,
    impurity_composition JSON,
    quality_modifier FLOAT
);
```

**Performance Considerations:**

```
Material Quality Calculations:
- Cache common material property lookups
- Pre-calculate geological formation bonuses
- Use spatial indexing for location-based quality
- Batch process quality calculations where possible
- Optimize quality query patterns

Estimated Performance:
- Material quality query: <1ms
- Crafting quality calculation: <5ms
- Geological formation lookup: <2ms
- Total crafting operation: <10ms (acceptable)
```

### Balancing Considerations

**Quality Distribution Targets:**

```
Target Distribution for Material Quality:
- Poor (1-35%): 10% of available materials
- Standard (36-65%): 40% of available materials
- Good (66-85%): 30% of available materials
- Excellent (86-95%): 15% of available materials
- Masterwork (96-100%): 5% of available materials

Ensures:
- Low-quality materials accessible for practice
- Standard materials readily available
- High-quality materials require effort
- Masterwork materials are prestigious
```

**Economic Balance:**

```
Pricing Progression (relative to standard quality):
- Poor quality: 0.3x price
- Standard quality: 1.0x price (baseline)
- Good quality: 2.5x price
- Excellent quality: 6.0x price
- Masterwork quality: 15.0x price

Ensures:
- Budget options available
- Standard tier is economical baseline
- Quality improvements are meaningfully expensive
- Top tier is luxury/prestige pricing
```

**Progression Balance:**

```
Skill-Quality Relationship:
- Skill 0-30: Reliably produces 30-60% quality
- Skill 30-60: Reliably produces 50-80% quality
- Skill 60-85: Reliably produces 70-95% quality
- Skill 85-100: Reliably produces 85-100% quality

Critical Success Rates:
- Skill 0-30: 2% chance
- Skill 30-60: 5% chance
- Skill 60-85: 8% chance
- Skill 85-100: 10% chance

Ensures:
- Clear progression incentive
- High skill meaningfully better
- Critical successes feel special
- Skill ceiling is attainable but prestigious
```

### Player Experience Design

**Tutorial and Onboarding:**

```
Introduction to Material Quality:
1. Simple gathering quest with quality feedback
   "Gather 10 iron ore. Notice the quality percentages!"

2. Basic crafting with quality impact demonstration
   "Craft a tool with 40% iron vs 80% iron. Feel the difference!"

3. Material selection tutorial
   "Choose materials wisely - quality affects everything!"

4. Experimentation encouragement
   "Try different material combinations and discover what works!"

5. Knowledge sharing introduction
   "Share discoveries with others or keep secrets for profit!"
```

**Quality Feedback Systems:**

```
Visual Quality Indicators:
- Color coding (gray/white/green/blue/purple for tiers)
- Numerical percentage display
- Visual material appearance changes with quality
- Comparative indicators (better/worse than average)
- Historical quality tracking for locations

In-Game Information:
- Tooltip showing material quality and properties
- Crafting preview showing expected quality range
- Post-craft report showing what affected quality
- Material comparison tools
- Quality trends for gathering locations
```

**Progression Satisfaction:**

```
Reward Milestones:
- First 70%+ quality craft: Achievement unlock
- First 90%+ quality craft: Special recognition
- First 100% quality craft: Legendary achievement
- 100 high-quality crafts: Master crafter title
- Teach 10 other players: Mentor recognition

Ensures:
- Clear progression feedback
- Satisfying achievement moments
- Social recognition for excellence
- Incentive to continue improving
```

## Conclusion

### Key Takeaways for BlueMarble

**1. Material Quality as Core Mechanic:**
Mortal Online 2 demonstrates that material quality systems create depth and replayability. BlueMarble can implement this with scientific accuracy, making quality a natural consequence of geological properties rather than arbitrary stats.

**2. Player Agency Through Choices:**
Every stage of material acquisition, processing, and crafting should present meaningful choices. BlueMarble's geological simulation enables even richer decision-making based on real scientific principles.

**3. Open-Ended Experimentation:**
Allowing players to experiment with material combinations drives engagement and creates emergent gameplay. BlueMarble can leverage real chemistry and material science to make this experimentation educational and realistic.

**4. Knowledge as Content:**
Material knowledge becomes valuable game content in MO2. BlueMarble can enhance this by making geological knowledge discoverable through scientific method, creating educational value alongside entertainment.

**5. Player-Driven Economy:**
Quality variance creates natural market stratification and specialization opportunities. BlueMarble's realistic material system will enable even more sophisticated economic gameplay.

**6. No Artificial Class Restrictions:**
MO2's success with open-ended crafting shows that players naturally specialize when given freedom. BlueMarble should embrace this philosophy with geological specializations emerging from player choice.

### Recommendations Summary

**High Priority Implementations:**
1. ✅ Multi-tier quality system (0-100% with meaningful impact)
2. ✅ Geological formation quality determination (scientifically accurate)
3. ✅ Material property inheritance in crafting
4. ✅ Flexible recipe system allowing experimentation
5. ✅ Knowledge discovery and sharing systems

**Medium Priority Implementations:**
1. ⚠️ Material market stratification systems
2. ⚠️ Crafter reputation and specialization recognition
3. ⚠️ Material combination experimentation tools
4. ⚠️ Geological survey and research mechanics
5. ⚠️ Quality feedback and tutorial systems

**Future Considerations:**
1. 🔄 Advanced material science simulation
2. 🔄 Complex supply chain and vertical integration
3. 🔄 Knowledge trading and information economy
4. 🔄 Material discovery as endgame content
5. 🔄 Community knowledge base integration

### Unique Advantages for BlueMarble

BlueMarble can exceed Mortal Online 2's material system by:

1. **Scientific Authenticity**: Real geological properties instead of arbitrary stats
2. **Educational Value**: Players learn actual material science and geology
3. **Consistent Logic**: All material interactions follow physical laws
4. **Deeper Experimentation**: Scientific method applies to material discovery
5. **Planetary Scale**: Geological diversity across entire Earth surface
6. **Dynamic Geology**: Material properties can change based on geological processes

### Final Thoughts

Mortal Online 2's material and quality system demonstrates the power of player agency, open-ended experimentation, and quality-driven gameplay. By implementing these principles within BlueMarble's scientific geological simulation, we can create a material system that is simultaneously more realistic, more engaging, and more educational than any existing game.

The key is maintaining the player freedom and experimentation that makes MO2's system successful while leveraging BlueMarble's unique geological foundation to provide scientific authenticity and educational value. This creates a genuinely novel game system that cannot be replicated without BlueMarble's geological simulation technology.

## References

### Primary Sources

- Mortal Online 2 Official Website: https://www.mortalonline2.com/
- Mortal Online 2 Wiki: https://mortalonline2.wiki/wiki/Main_Page
- Mortal Online 2 Steam Community Discussions
- Developer interviews and patch notes

### Related BlueMarble Research

- [Skill and Knowledge System Research](skill-knowledge-system-research.md) - Skill progression systems analysis
- [Mechanics Research](mechanics-research.md) - Economic simulation mechanics
- [Assembly Skills System Research](assembly-skills-system-research.md) - Crafting system design
- [Crafting Quality Model](../../docs/gameplay/mechanics/crafting-quality-model.md) - Mathematical specifications
- [Crafting Interface Mockups](assets/crafting-interface-mockups.md) - Visual design examples

### Further Reading

- Real-world materials science and metallurgy references
- Geological formation and mineralization processes
- Economic theory for virtual economies
- Game design patterns for player agency and emergent gameplay

---

**Document Status**: Complete  
**Last Updated**: 2024  
**Next Review**: After initial implementation prototype
