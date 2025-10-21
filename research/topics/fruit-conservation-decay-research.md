# Fruit Conservation and Decay Research: Real-World Principles for Game Implementation

**Date**: 2025-01-18  
**Purpose**: Research-backed fruit conservation and spoilage mechanics for BlueMarble  
**Source**: Scientific research on fruit physiology, post-harvest biology, and food preservation  
**Related**: Extended auction system, deterioration mechanics, seasonal effects

---

## Executive Summary

This document provides scientifically-grounded research on fruit conservation, decay mechanisms, and preservation techniques applicable to BlueMarble's trading and deterioration systems. Based on real-world post-harvest biology, this research informs realistic spoilage rates, preservation effectiveness, and seasonal factors.

### Key Findings

- **Decay Rates**: Vary dramatically (1-60 days shelf life) based on fruit type, ripening pattern, and environmental factors
- **Primary Decay Mechanisms**: Respiration, ethylene production, moisture loss, microbial growth
- **Preservation Effectiveness**: Drying (95% decay reduction), cold storage (70-90%), salting (60-80%), fermentation (85-95%)
- **Environmental Factors**: Temperature (most critical), humidity, CO₂/O₂ levels, light exposure

---

## Part I: Fruit Decay Mechanisms

### 1. Respiration and Metabolic Processes

**Scientific Principle**: Fruits continue metabolic processes after harvest, consuming sugars and oxygen while producing CO₂, water, and heat.

```
Respiration Rate = f(Temperature, Fruit Type, Ripeness)

Q10 Factor: Respiration doubles for every 10°C temperature increase
```

**Climacteric vs Non-Climacteric Fruits**:

**Climacteric** (continue ripening post-harvest):
- Apples, pears, quinces: Significant ethylene production
- Shelf life: 30-180 days (cold storage), 7-14 days (ambient)
- Rapid quality deterioration once ripe
- Example: Apple respiration increases 5-10x at ripening peak

**Non-Climacteric** (minimal post-harvest ripening):
- Cherries, serviceberries, chokeberries: Low ethylene
- Shelf life: 3-21 days (cold storage), 1-5 days (ambient)
- More gradual deterioration
- Example: Cherry respiration remains relatively constant

### 2. Moisture Loss (Transpiration)

**Scientific Principle**: Water vapor loss through fruit skin leads to weight loss, shriveling, and quality degradation.

```
Moisture Loss Rate = (Vapor Pressure Deficit × Skin Permeability) / Resistance

Factors:
- Temperature: Higher temp = faster loss
- Humidity: Lower RH = faster loss  
- Air movement: Higher flow = faster loss
- Fruit coating: Waxy cuticle reduces loss
```

**Weight Loss Impact**:
- 3-5% loss: Noticeable quality reduction
- 5-10% loss: Significant shriveling, value drop
- >10% loss: Unmarketable condition

### 3. Enzymatic Browning

**Scientific Principle**: Polyphenol oxidase (PPO) enzymes react with oxygen when tissue is damaged, causing brown discoloration.

**Particularly Affects**:
- Apples, pears (high PPO content)
- Medlars, loquats (moderate)
- Accelerated by bruising, cutting, temperature extremes

**Prevention Methods**:
- Controlled atmosphere (low O₂)
- Acidification (lemon juice, ascorbic acid)
- Heat treatment (blanching)
- Sulfite treatment (traditional preservation)

### 4. Microbial Spoilage

**Scientific Principle**: Bacteria, molds, and yeasts colonize fruit surfaces and damaged tissue.

**Common Pathogens**:
- **Botrytis cinerea** (gray mold): Affects soft fruits (serviceberries, grapes)
- **Penicillium** spp.: Blue/green mold on pomes (apples, pears)
- **Rhizopus** spp.: Soft rot on damaged fruit
- **Acetobacter**: Fermentation of sugary fruits

**Growth Factors**:
- Temperature: Optimal 20-30°C, minimal <5°C
- pH: Most fungi prefer 4-7 (fruit range)
- Water activity: Requires >0.80 (all fresh fruits)
- Oxygen: Most require aerobic conditions

---

## Part II: Fruit-Specific Decay Research

### Pome Fruits (Apples, Pears, Quinces)

#### Malus domestica (Apple)

**Shelf Life**:
- Ambient (20°C): 7-21 days
- Cold storage (0-4°C): 3-12 months (variety dependent)
- Controlled atmosphere: Up to 12 months

**Decay Rate Factors**:
```javascript
appleDecayRate = {
    baseRate: 0.008,  // 0.8% per day at 20°C
    temperatureMultiplier: {
        30°C: 3.0,      // Summer heat
        20°C: 1.0,      // Room temperature
        10°C: 0.3,      // Cool storage
        0°C: 0.05       // Cold storage (near freezing)
    },
    varietyModifier: {
        'Granny Smith': 0.6,    // Excellent keeper
        'Fuji': 0.7,            // Very good
        'Red Delicious': 1.0,   // Standard
        'Golden Delicious': 1.2, // Faster ripening
        'McIntosh': 1.5         // Poor keeper
    },
    ethyleneSensitivity: 'HIGH',
    respiration: {
        preclimacteric: 10,     // ml CO₂/kg/hr
        climacteric: 40-100     // Peak respiration
    }
}
```

**Primary Decay Issues**:
1. **Superficial scald**: Skin browning in cold storage (>3 months)
2. **Core rot**: Internal breakdown from delayed cooling
3. **Bitter pit**: Calcium deficiency spots
4. **Blue mold**: Penicillium expansum post-injury

**Preservation Effectiveness**:
- Drying: 95% decay reduction, 6-12 month shelf life
- Cold storage: 90% reduction, 3-12 months
- Controlled atmosphere: 95% reduction, up to 12 months
- Wax coating: 30% reduction (commercial)

#### Pyrus communis (European Pear)

**Shelf Life**:
- Ambient: 5-14 days (post-ripening)
- Cold storage: 2-7 months
- Requires chilling for proper ripening (unlike apples)

**Decay Rate**:
```javascript
pearDecayRate = {
    baseRate: 0.012,  // 1.2% per day at 20°C (faster than apples)
    temperatureMultiplier: {
        30°C: 3.5,
        20°C: 1.0,
        10°C: 0.25,
        0°C: 0.04
    },
    ripeningSensitivity: 'VERY HIGH',
    textureChange: 'RAPID',    // Becomes mealy quickly
    shelfLifeVariability: 'HIGH' // Highly variety-dependent
}
```

**Unique Considerations**:
- Must be picked unripe and chilled for proper ripening
- Rapid texture deterioration once ripe (2-3 days)
- Very sensitive to ethylene exposure
- Harder to dry than apples (higher moisture content)

#### Cydonia oblonga (Quince)

**Shelf Life**:
- Ambient: 14-21 days (very firm fruit)
- Cold storage: 2-4 months
- More resistant to handling damage than apples/pears

**Unique Properties**:
- High tannin content (natural preservative)
- Very hard texture when fresh
- Usually cooked before consumption
- Less susceptible to enzymatic browning

### Serviceberries and Juneberries (Amelanchier spp.)

#### General Characteristics

**Shelf Life**:
- Ambient: 1-3 days only
- Cold storage: 5-10 days maximum
- Frozen: 12 months+

**Decay Rate**:
```javascript
serviceberryDecayRate = {
    baseRate: 0.15,   // 15% per day at 20°C (very perishable!)
    temperatureMultiplier: {
        30°C: 2.5,    // Spoils in hours
        20°C: 1.0,
        10°C: 0.4,
        0°C: 0.15     // Still relatively fast
    },
    moistureLoss: 'VERY HIGH',
    microbialSusceptibility: 'EXTREME',
    handlingSensitivity: 'EXTREME'
}
```

**Primary Decay Issues**:
1. **Thin skin**: Easy moisture loss and microbial entry
2. **High sugar content**: Rapid fermentation
3. **Soft texture**: Bruises easily
4. **Juice leakage**: Damaged berries contaminate others

**Preservation Strategy**:
- **Immediate cooling essential** (1-2 hours post-harvest)
- **Drying**: Most effective (80-90% reduction), makes "raisins"
- **Freezing**: Best for long-term (95% preservation)
- **Jam/preserves**: Traditional method, 6-12 months

### Stone Fruits and Drupes (Limited Coverage - Different Family)

While the provided list focuses on pome fruits (Rosaceae subfamily Maloideae), brief coverage:

**General Pattern**:
- More perishable than pomes (thin skin)
- Higher water content
- Rapid softening post-harvest
- Shelf life: 3-21 days cold storage

---

## Part III: Environmental Factors

### Temperature Effects

**Critical Temperature Ranges**:

```javascript
temperatureEffects = {
    veryHot: {
        range: '>35°C',
        effect: 'Rapid deterioration, cooking damage',
        decayMultiplier: 4.0,
        applicability: 'Desert summer, unshaded transport'
    },
    hot: {
        range: '25-35°C',
        effect: 'Fast ripening and decay',
        decayMultiplier: 2.0,
        applicability: 'Summer ambient, tropical regions'
    },
    warm: {
        range: '15-25°C',
        effect: 'Normal ambient decay',
        decayMultiplier: 1.0,
        applicability: 'Spring/autumn, moderate climates'
    },
    cool: {
        range: '5-15°C',
        effect: 'Slowed metabolism',
        decayMultiplier: 0.3,
        applicability: 'Cool cellars, early winter'
    },
    cold: {
        range: '0-5°C',
        effect: 'Near-optimal storage',
        decayMultiplier: 0.1,
        applicability: 'Refrigeration, winter storage'
    },
    freezing: {
        range: '<0°C',
        effect: 'Suspended decay OR tissue damage',
        decayMultiplier: 0.02,  // If properly frozen
        risk: 'Ice crystal damage if not quick-frozen',
        applicability: 'Deep winter, ice houses'
    }
}
```

**Chilling Injury** (Important Exception):
- Some tropical fruits (loquat, certain service berries) damaged by cold
- Threshold: <10°C for sensitive species
- Symptoms: Pitting, browning, increased decay
- Game implementation: Tropical fruits have cold storage penalty

### Humidity and Moisture

**Relative Humidity Impact**:

```javascript
humidityEffects = {
    veryLow: {
        range: '<60% RH',
        effect: 'Rapid moisture loss, shriveling',
        moistureLossMultiplier: 2.0,
        textureImpact: 'Becomes leathery, wrinkled'
    },
    low: {
        range: '60-75% RH',
        effect: 'Moderate moisture loss',
        moistureLossMultiplier: 1.3
    },
    optimal: {
        range: '85-95% RH',
        effect: 'Minimal moisture loss',
        moistureLossMultiplier: 1.0,
        note: 'Ideal for most pome fruits'
    },
    veryHigh: {
        range: '>95% RH',
        effect: 'Risk of surface moisture, mold growth',
        moldRiskMultiplier: 2.0,
        note: 'Condensation promotes pathogens'
    }
}
```

**Seasonal Humidity Patterns**:
- Summer: Lower RH (faster drying)
- Winter: Variable (dry indoors, humid outdoors)
- Monsoon/rainy seasons: Very high (mold risk)

### Atmospheric Composition

**Controlled Atmosphere (CA) Storage**:

```javascript
atmosphereEffects = {
    normal: {
        O2: 21,
        CO2: 0.04,
        decayMultiplier: 1.0
    },
    controlledAtmosphere: {
        O2: '1-5%',      // Reduced oxygen
        CO2: '1-5%',     // Elevated CO₂
        decayMultiplier: 0.05,
        shelfLifeExtension: '2-4x',
        costPremium: 'HIGH',
        applicability: 'High-value commercial storage only'
    },
    modifiedAtmosphere: {
        O2: '5-15%',
        CO2: '3-10%',
        decayMultiplier: 0.2,
        shelfLifeExtension: '50-100%',
        costPremium: 'MEDIUM',
        applicability: 'Package-level (fruit containers)'
    }
}
```

**Game Implementation Note**: CA storage could be high-tier preservation facility requiring significant investment.

---

## Part IV: Preservation Methods (Real-World Effectiveness)

### 1. Drying / Dehydration

**Scientific Principle**: Reduces water activity (aw) below microbial growth threshold (<0.60).

**Process**:
```
Fresh Fruit (aw = 0.95-0.99)
↓ Sun/Air Drying (3-7 days)
↓ Dehydrator (24-48 hours at 60°C)
↓ Freeze Drying (24 hours, vacuum)
Dried Fruit (aw = 0.50-0.65)
```

**Effectiveness by Fruit Type**:

```javascript
dryingEffectiveness = {
    apples: {
        moistureReduction: '85%',  // 85% → 15% water
        weightLoss: '85%',         // 1kg → 150g
        shelfLife: '6-12 months',
        decayReduction: '95%',
        qualityRetention: 'EXCELLENT',
        rehydration: 'GOOD'
    },
    pears: {
        moistureReduction: '83%',
        shelfLife: '6-10 months',
        decayReduction: '93%',
        qualityRetention: 'GOOD',
        note: 'Slight texture degradation vs apples'
    },
    serviceberries: {
        moistureReduction: '80%',
        shelfLife: '4-8 months',
        decayReduction: '90%',
        qualityRetention: 'FAIR',
        note: 'Become very concentrated, raisin-like'
    },
    quinces: {
        moistureReduction: '80%',
        shelfLife: '6-9 months',
        decayReduction: '92%',
        requiresProcessing: 'Usually made into paste/membrillo'
    }
}
```

**Cost and Time**:
- Sun drying: Free, 3-7 days, weather-dependent
- Dehydrator: Fuel cost, 1-2 days, reliable
- Freeze drying: HIGH COST, 1 day, best quality

### 2. Cold Storage

**Scientific Principle**: Slows metabolic processes and microbial growth through temperature reduction.

**Real-World Storage Conditions**:

```javascript
coldStorageConditions = {
    rootCellar: {
        temperature: '4-10°C',
        humidity: '85-95% RH',
        shelfLifeMultiplier: 5,  // vs ambient
        cost: 'LOW',
        availability: 'Common in pre-industrial settings',
        capacity: 'Medium-large',
        limitations: 'Temperature not precise, seasonal variation'
    },
    iceHouse: {
        temperature: '0-2°C',
        humidity: '90-100% RH',
        shelfLifeMultiplier: 10,
        cost: 'MEDIUM',
        availability: 'Requires ice harvest/trade',
        seasonality: 'Winter collection for year-round use',
        limitations: 'Ice depletion over summer'
    },
    springHouse: {
        temperature: '8-12°C',
        humidity: '85-95% RH',
        shelfLifeMultiplier: 3,
        cost: 'LOW',
        availability: 'Requires natural spring',
        limitations: 'Warmer than root cellar, limited capacity'
    }
}
```

**Effectiveness by Fruit**:
- **Apples**: Excellent (3-12 months at 0°C)
- **Pears**: Very good (2-7 months at 0°C)
- **Serviceberries**: Poor (5-10 days even at 0°C)
- **Quinces**: Good (2-4 months at 0°C)

### 3. Salting / Brining

**Scientific Principle**: Osmotic dehydration and sodium's antimicrobial properties.

**Less Common for Fruits** (more for vegetables), but used historically:

```javascript
saltingForFruits = {
    effectiveness: 'MODERATE',
    decayReduction: '70%',
    shelfLife: '2-4 months',
    applications: [
        'Preserved lemons (citrus)',
        'Salted plums (umeboshi)',
        'Some berry preparations'
    ],
    tastChange: 'SIGNIFICANT',
    marketValue: 'REDUCED',
    culturalVariation: 'Asian cuisines more common',
    gameApplicability: 'LOW for pome fruits'
}
```

### 4. Sugar Preservation

**Scientific Principle**: High sugar concentration (>60%) inhibits microbial growth through osmosis.

**Methods**:

```javascript
sugarPreservation = {
    jam: {
        sugarContent: '60-65%',
        shelfLife: '12-24 months',
        decayReduction: '95%',
        processTime: '2-4 hours',
        fuelCost: 'MEDIUM',
        applicability: 'ALL pome fruits and berries',
        qualityChange: 'Transformed product (not fresh)',
        marketValue: 'Different market segment'
    },
    candying: {
        sugarContent: '70%+',
        shelfLife: '6-12 months',
        processTime: '24-72 hours (multiple soaks)',
        cost: 'HIGH (sugar expensive historically)',
        applicability: 'Luxury product'
    },
    dryingWithSugar: {
        sugarContent: '40-50%',
        shelfLife: '4-8 months',
        synergy: 'Sugar + reduced moisture',
        examples: 'Dried sweetened berries, fruit leather'
    }
}
```

### 5. Fermentation

**Scientific Principle**: Controlled microbial fermentation lowers pH and produces alcohol/acids, inhibiting spoilage organisms.

```javascript
fermentation = {
    cider: {
        sourceApples: 'Apples, pears',
        alcoholContent: '4-8%',
        shelfLife: '6-24 months',
        processTime: '2-4 weeks + aging',
        decayReduction: '98%',
        marketValue: 'HIGHER (alcoholic beverage)',
        storageRequirements: 'Cool, sealed containers'
    },
    vinegar: {
        source: 'Overripe/damaged fruits',
        acidContent: '4-8%',
        shelfLife: 'Indefinite',
        processTime: '3-6 months',
        marketValue: 'MEDIUM (condiment)',
        note: 'Salvages spoiling fruit'
    },
    fruitWine: {
        source: 'High-sugar berries, apples',
        alcoholContent: '8-12%',
        shelfLife: '1-10+ years',
        processTime: '3-12 months',
        marketValue: 'HIGH',
        complexity: 'HIGH (skill required)'
    }
}
```

### 6. Smoking (Limited Applicability)

**For Fruits**: Rarely used, but historical examples exist.

```javascript
smokingFruits = {
    effectiveness: 'LOW-MODERATE',
    decayReduction: '60%',
    shelfLife: '1-3 months',
    applications: [
        'Smoked plums (rare)',
        'Some dried berry preparations'
    ],
    flavorChange: 'EXTREME',
    gameApplicability: 'VERY LOW for pomes'
}
```

---

## Part V: Game Implementation Framework

### Fruit Categorization System

```javascript
fruitCategories = {
    excellentKeepers: {
        fruits: ['Apple (most varieties)', 'Quince', 'Some pears'],
        baseShelfLife: 30,  // days at ambient
        coldStorageMultiplier: 10,
        dryingEffectiveness: 0.95,
        gameUse: 'Trade commodities, reliable value stores'
    },
    goodKeepers: {
        fruits: ['Asian pear', 'European pear', 'Medlar'],
        baseShelfLife: 14,
        coldStorageMultiplier: 8,
        dryingEffectiveness: 0.92,
        gameUse: 'Short-distance trade, regional commerce'
    },
    moderateKeepers: {
        fruits: ['Loquat', 'Some crabapples'],
        baseShelfLife: 7,
        coldStorageMultiplier: 5,
        dryingEffectiveness: 0.88,
        gameUse: 'Local markets, immediate consumption'
    },
    poorKeepers: {
        fruits: ['Serviceberries', 'Juneberries', 'Some soft pomes'],
        baseShelfLife: 3,
        coldStorageMultiplier: 3,
        dryingEffectiveness: 0.90,  // But must dry immediately
        gameUse: 'Preservation urgent, local only when fresh'
    }
}
```

### Integrated Decay Formula

```javascript
calculateFruitDecay(fruit, conditions, time) {
    // Base decay rate (% per day)
    let baseRate = fruit.category.baseDecayRate;
    
    // Temperature effect (most critical)
    const tempMultiplier = getTemperatureMultiplier(conditions.temperature, fruit);
    
    // Humidity effect
    const humidityMultiplier = getHumidityMultiplier(conditions.humidity);
    
    // Seasonal modifiers
    const seasonMultiplier = getSeasonalMultiplier(conditions.season);
    
    // Handling damage
    const handlingDamage = conditions.handlingQuality || 1.0;
    
    // Storage facility bonus
    const facilityBonus = conditions.storageFacility 
        ? facilitiesBonuses[conditions.storageFacility]
        : 1.0;
    
    // Preservation method
    const preservationReduction = conditions.preservation
        ? preservationMethods[conditions.preservation].decayReduction
        : 0;
    
    // Combined decay rate
    const effectiveDecayRate = baseRate 
        × tempMultiplier 
        × humidityMultiplier 
        × seasonMultiplier 
        × handlingDamage 
        × facilityBonus 
        × (1 - preservationReduction);
    
    // Calculate decay over time
    const totalDecay = 1 - Math.exp(-effectiveDecayRate × time);
    
    // Value degradation (accelerates as decay progresses)
    const valueMultiplier = calculateValueDegradation(totalDecay);
    
    return {
        percentDecayed: totalDecay × 100,
        remainingFreshness: (1 - totalDecay) × 100,
        currentValue: fruit.baseValue × valueMultiplier,
        daysUntilSpoiled: Math.log(0.1) / -effectiveDecayRate  // 90% decay
    };
}
```

### Preservation Decision Matrix

```javascript
preservationDecisionMatrix = {
    serviceberries: {
        scenario: 'Harvested 100kg, 3 days to nearest market',
        withoutPreservation: {
            arrivalCondition: '40-50% spoiled',
            marketValue: '50-60 cr (50% of fresh)',
            totalLoss: '40-50 cr + 50kg wasted'
        },
        withDrying: {
            dryingTime: '2-3 days',
            yield: '20kg dried',
            marketValue: '80 cr (premium dried fruit)',
            processingCost: '15 cr (fuel, labor)',
            netValue: '65 cr',
            advantage: '+15 cr vs fresh attempt'
        },
        withImmediateSale: {
            travelTime: '1 day (rushed)',
            arrivalCondition: '90% good',
            marketValue: '95 cr',
            transportCost: '25 cr (rush fee)',
            netValue: '70 cr',
            advantage: '+20 cr (best if market is close)'
        },
        optimalStrategy: 'Dry if market >2 days away, rush fresh if <1 day'
    },
    
    apples: {
        scenario: 'Harvested 500kg, 14 days to distant market',
        withoutPreservation: {
            ambientStorage: '30% spoiled',
            marketValue: '350 cr (70% salvaged)',
            loss: '150 cr'
        },
        withColdStorage: {
            arrivalCondition: '95% excellent',
            marketValue: '480 cr',
            storageCost: '30 cr',
            netValue: '450 cr',
            advantage: '+100 cr vs ambient'
        },
        withDrying: {
            yield: '85kg dried',
            marketValue: '255 cr (dried premium)',
            processingCost: '40 cr',
            netValue: '215 cr',
            advantage: 'Long shelf life but lower value'
        },
        optimalStrategy: 'Cold storage for fresh market, dry for long-term'
    }
}
```

---

## Part VI: Seasonal Effects Integration

### Seasonal Decay Modifiers (Real-World Based)

```javascript
seasonalDecayPatterns = {
    spring: {
        temperature: 'MODERATE',
        humidity: 'HIGH',
        decayMultiplier: 1.1,
        notes: 'Increased humidity, mold risk',
        preservationBonus: {
            'drying': 0.9  // Harder to dry (humid)
        },
        marketDynamics: {
            'fresh fruit': 0.8,  // Winter storage depleted
            'dried fruit': 1.2   // High demand
        }
    },
    
    summer: {
        temperature: 'HOT',
        humidity: 'VARIABLE',
        decayMultiplier: 1.8,
        notes: 'Rapid spoilage, difficult fresh storage',
        preservationBonus: {
            'drying': 1.3  // Excellent drying conditions
        },
        marketDynamics: {
            'fresh fruit': 1.5,  // Harvest season, abundant
            'preserved': 0.6     // Low demand (fresh available)
        }
    },
    
    autumn: {
        temperature: 'COOL',
        humidity: 'MODERATE',
        decayMultiplier: 0.7,
        notes: 'Optimal storage season',
        preservationBonus: {
            'cold storage': 1.2,  // Ideal conditions
            'root cellar': 1.3
        },
        marketDynamics: {
            'fresh fruit': 1.8,   // Peak harvest
            'storage preparation': 1.5
        }
    },
    
    winter: {
        temperature: 'COLD',
        humidity: 'LOW',
        decayMultiplier: 0.3,
        notes: 'Natural refrigeration, moisture loss risk',
        preservationBonus: {
            'cold storage': 1.5,  // Free natural cooling
            'ice harvest': 'AVAILABLE'
        },
        marketDynamics: {
            'fresh fruit': 0.4,   // Scarce, premium
            'dried fruit': 1.3,
            'preserved': 1.4
        }
    }
}
```

### Preservation Timing Strategy

**Critical Decision Points**:

1. **At Harvest** (Day 0):
   - Fresh sale vs preservation?
   - Market distance and transport time?
   - Current season's preservation conditions?

2. **During Transport** (Days 1-N):
   - Monitor deterioration
   - Emergency preservation if quality drops?
   - Cut losses or continue?

3. **At Market** (Arrival):
   - Accept reduced price for slightly deteriorated?
   - Process into preserves on-site?
   - Rush to next market?

---

## Part VII: Summary Tables for Game Implementation

### Fruit Decay Rates (Simplified)

| Fruit Type | Base Decay (day) | Cold Storage | Dried | Shelf Life (fresh) |
|------------|------------------|--------------|-------|-------------------|
| Apple | 1.0% | 0.1% | 0.03% | 30 days |
| Pear | 1.5% | 0.15% | 0.04% | 20 days |
| Quince | 0.8% | 0.1% | 0.03% | 40 days |
| Serviceberry | 15% | 2% | 0.5% | 3 days |
| Crabapple | 1.2% | 0.12% | 0.04% | 25 days |
| Loquat | 3% | 0.5% | 0.08% | 10 days |
| Medlar | 2% | 0.3% | 0.06% | 14 days |

### Preservation Effectiveness Summary

| Method | Decay Reduction | Shelf Life | Cost | Processing Time |
|--------|----------------|------------|------|----------------|
| Drying | 93-95% | 6-12 mo | Low-Med | 1-7 days |
| Cold Storage | 85-90% | 3-12 mo | Low-High | Immediate |
| Sugar/Jam | 95% | 12-24 mo | Medium | 2-4 hours |
| Fermentation | 98% | 6-24 mo | Low | 2-12 weeks |
| Salting | 70% | 2-4 mo | Low | 1-2 days |
| Smoking | 60% | 1-3 mo | Medium | 1-2 days |

### Environmental Multipliers

| Factor | Range | Decay Multiplier | Game Season |
|--------|-------|------------------|-------------|
| Very Hot | >30°C | 3.0x | Summer peak |
| Hot | 25-30°C | 1.8x | Summer |
| Warm | 15-25°C | 1.0x | Spring/Autumn |
| Cool | 5-15°C | 0.3x | Autumn/Spring |
| Cold | 0-5°C | 0.1x | Winter |
| Freezing | <0°C | 0.05x | Deep winter |

---

## Part VIII: Integration with Existing Systems

### Connection to Extended Auction System

**Current Implementation**:
```javascript
// From swap-router.js
getDeteriorationRate(commodityId, season, preservation) {
    const commodity = this.commodities.get(commodityId);
    if (!commodity || !commodity.perishable) return 0;
    
    // Current simplified model
    const seasonMultipliers = {
        'spring': 1.0,
        'summer': 1.3,
        'winter': 0.5
    };
    
    // This research provides detailed backing for these numbers
}
```

**Enhanced with Research**:
```javascript
getDeteriorationRate(commodityId, season, preservation, storageConditions) {
    const fruit = this.fruitDatabase.get(commodityId);
    if (!fruit) return 0;
    
    // Research-backed base rates
    let baseRate = fruit.decayRates[fruit.category];
    
    // Temperature effect (from Part V)
    const tempMultiplier = this.calculateTemperatureEffect(
        season,
        storageConditions.facility
    );
    
    // Humidity effect (from Part III)
    const humidityMultiplier = this.calculateHumidityEffect(season);
    
    // Preservation (from Part IV)
    const preservationReduction = preservation
        ? this.preservationMethods[preservation].decayReduction
        : 0;
    
    return baseRate × tempMultiplier × humidityMultiplier × (1 - preservationReduction);
}
```

### New Data Structures

```javascript
// Fruit classification database
fruitDatabase = new Map([
    ['apple', {
        category: 'excellentKeeper',
        baseDecayRate: 0.01,
        coldStorageMultiplier: 0.1,
        dryingEffectiveness: 0.95,
        climaticPeaks: { autumn: 1.5 },
        marketValue: { fresh: 20, dried: 35 }
    }],
    ['serviceberry', {
        category: 'poorKeeper',
        baseDecayRate: 0.15,
        coldStorageMultiplier: 0.13,
        dryingEffectiveness: 0.90,
        climaticPeaks: { summer: 2.0 },
        marketValue: { fresh: 40, dried: 55 },
        urgencyWarning: true
    }]
    // ... 100+ fruits from list
]);
```

---

## Part IX: Future Research Directions

### High Priority

1. **Microbial Decay Models**: More detailed pathogen simulation
2. **Quality Grading System**: A/B/C grade based on freshness percentage
3. **Packaging Effects**: Ventilated boxes, wax paper, etc.
4. **Transport Damage**: Bruising from rough handling, stacking weight

### Medium Priority

5. **Ripening Simulation**: Climacteric vs non-climacteric detailed models
6. **Pest Damage**: Insect infestation during storage
7. **Disease Spread**: One spoiled fruit contaminating others
8. **Player Skill**: Experienced traders reduce losses

### Low Priority

9. **Genetic Varieties**: Different apple cultivars with distinct properties
10. **Terroir Effects**: Geographic origin affecting quality and shelf life
11. **Historical Techniques**: Period-specific preservation methods
12. **Chemical Preservation**: Sulfites, ascorbic acid treatments

---

## References

### Scientific Literature

1. Kader, A.A. (2002). *Postharvest Technology of Horticultural Crops*. University of California Division of Agriculture and Natural Resources.

2. Yahia, E.M. (2011). *Postharvest Biology and Technology of Tropical and Subtropical Fruits*. Woodhead Publishing.

3. Saltveit, M.E. (2016). "Respiratory Metabolism" in *The Commercial Storage of Fruits, Vegetables, and Florist and Nursery Stocks*. USDA Agricultural Handbook 66.

4. Watkins, C.B. (2003). "Principles and practices of postharvest handling and stress." In *Apples: Botany, Production and Uses*.

### Traditional Knowledge

5. Root Cellaring: Natural Cold Storage of Fruits & Vegetables - Mike and Nancy Bubel

6. Preserving Food without Freezing or Canning - The Gardeners and Farmers of Terre Vivante

### Online Resources

7. USDA Post-Harvest Compendium: https://www.ba.ars.usda.gov/hb66/

8. FAO Post-Harvest Guidelines: http://www.fao.org/food-loss-and-food-waste/

---

## Appendix: Comprehensive Fruit List Processing

**Note**: The provided list contains 100+ pome fruits (Rosaceae family). Key categories:

**Malus** (Apples and Crabapples): 40+ species
- Domestic apple (M. domestica) - primary commercial
- Various crabapples - smaller, more tart
- Wild species - regional varieties

**Pyrus** (Pears): 20+ species
- European pear (P. communis) - primary
- Asian pears (P. pyrifolia, P. × bretschneideri)
- Wild pears - diverse characteristics

**Amelanchier** (Serviceberries/Juneberries): 15+ species
- Very perishable, excellent dried
- North American natives
- Sweet, berry-like pomes

**Others**: Quince, Medlar, Loquat, Rowan, Hawthorn, etc.

**Game Implementation Strategy**:
- Use 5-10 representative types with varying decay rates
- Regional specialization based on geographic origins
- Rare varieties as premium commodities

---

## License

Research document for BlueMarble.Design project.  
Scientific principles are public domain; game implementation is proprietary.

---

**Document Status**: Complete  
**Next Actions**: 
1. Integrate detailed decay rates into swap router
2. Create fruit classification database
3. Implement preservation decision AI for NPCs
4. Design cold storage facility mechanics
