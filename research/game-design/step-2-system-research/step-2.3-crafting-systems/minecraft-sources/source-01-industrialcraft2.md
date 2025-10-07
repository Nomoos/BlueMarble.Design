# Source Analysis: IndustrialCraft 2 (IC2)

**Source Type:** Official Mod Documentation  
**Source:** IndustrialCraft 2 Wiki (https://wiki.industrial-craft.net/)  
**Date Accessed:** 2025-01-15  
**Relevance:** High - Foundational technology mod with tiered progression

## Source Overview

IndustrialCraft 2 is one of the oldest and most established technology mods in Minecraft. It introduces electricity-based machinery and a tiered power system, serving as a blueprint for many subsequent technology mods.

## Key Findings

### 1. Tiered Power System (EU - Energy Units)

IC2 implements a voltage-based power system with distinct tiers:

```
Low Voltage (LV): 32 EU/t
├── Basic machines
├── Copper cables
└── Entry-level power generation

Medium Voltage (MV): 128 EU/t
├── Advanced machines
├── Gold cables
└── Improved efficiency

High Voltage (HV): 512 EU/t
├── Industrial-scale operations
├── Iron cables
└── Mass production

Extreme Voltage (EV): 2048 EU/t
├── End-game technology
├── Glass fiber cables
└── Maximum efficiency
```

**Relevance to BlueMarble:** The tiered voltage system creates natural progression gates. Players must build infrastructure at each tier before advancing, similar to how BlueMarble's geological depth creates natural progression tiers.

### 2. Ore Processing Chain

IC2 introduces ore doubling through the Macerator, establishing a pattern used by most tech mods:

```
Raw Ore → Macerator → 2x Dust → Furnace → 2x Ingots
```

Advanced processing adds stages:
```
Raw Ore → Ore Washing Plant → Purified Crushed Ore
↓
Thermal Centrifuge → Purified Dust + Byproducts
↓
Furnace → 2.5x Ingots + Secondary Materials
```

**Relevance to BlueMarble:** Multi-stage processing with increasing yields mirrors real-world beneficiation processes. BlueMarble can implement similar ore processing chains based on geological properties.

### 3. Infrastructure Dependencies

IC2 machines require supporting infrastructure:

- **Power Generation:** Coal/biomass generators → geothermal → solar → nuclear
- **Power Distribution:** Cable networks with voltage transformers
- **Energy Storage:** BatBoxes (LV) → MFE (MV) → MFSU (HV)
- **Safety Systems:** Machines explode if supplied wrong voltage

**Relevance to BlueMarble:** Infrastructure requirements create spatial planning challenges. BlueMarble's mining operations naturally require similar infrastructure (ventilation, drainage, power).

### 4. Material Dependencies

IC2 introduces key crafting components that gate progression:

```
Copper/Tin → Basic Circuits → Basic Machines
Gold/Redstone → Advanced Circuits → Advanced Machines
Diamonds/Iridium → Elite Circuits → End-game Technology
```

**Relevance to BlueMarble:** Material-gated progression prevents rushing to end-game content. BlueMarble's geological distribution creates natural material gates.

### 5. Nuclear Power System

IC2's nuclear reactors demonstrate complex multi-block systems:

- Requires uranium ore (rare resource)
- Reactor chambers (multi-block structure)
- Cooling systems (prevent meltdown)
- Active management (player must maintain reactor)
- Consequences (meltdown creates permanent terrain damage)

**Relevance to BlueMarble:** Complex systems with real consequences increase engagement. BlueMarble's geological simulation can create similar high-risk, high-reward scenarios (e.g., deep mining near unstable formations).

## Design Patterns Identified

### Pattern: Voltage Tiers as Progression Gates

Each voltage tier requires:
1. Materials from previous tier
2. Infrastructure upgrades (cables, transformers)
3. Higher power generation capacity
4. New machines unlock new capabilities

### Pattern: Machine Specialization

IC2 machines are specialized rather than multi-purpose:
- Macerator (grinding)
- Extractor (liquids/rubber)
- Compressor (plates/dense materials)
- Induction furnace (efficient smelting)

This creates a diverse machine ecosystem rather than one "do-everything" machine.

### Pattern: Hazard Systems

IC2 includes realistic hazards:
- Machines explode from overvoltage
- Nuclear reactors can melt down
- Cables have loss over distance
- Energy storage can overload

Hazards create tension and reward careful planning.

## Newly Discovered Sources

While analyzing IC2, related resources discovered:

1. **IC2 Experimental Branch** - Alternative IC2 version with different mechanics
2. **IC2 Classic** - Faithful recreation of original IC2 for newer Minecraft versions
3. **Tech Reborn** - Spiritual successor with similar systems but refined
4. **Advanced Machines Addon** - IC2 addon showing mod extensibility

These sources saved for potential future analysis.

## Recommendations for BlueMarble

1. **Implement Tiered Infrastructure:** Use geological depth/complexity as natural tier gates
2. **Multi-Stage Processing:** Design ore processing chains that reward investment in advanced facilities
3. **Specialized Buildings:** Create specialized processing buildings rather than one universal processor
4. **Consequence Systems:** Include realistic hazards (cave-ins, flooding, gas accumulation) that reward proper infrastructure
5. **Power Requirements:** Implement power/energy systems for deep mining and advanced processing

## Connection to BlueMarble's Goals

IC2's design philosophy aligns perfectly with BlueMarble's geological realism:

- **Tiered Progression:** IC2's voltage tiers ↔ BlueMarble's depth tiers
- **Infrastructure:** IC2's power networks ↔ BlueMarble's mining infrastructure (ventilation, drainage)
- **Processing Chains:** IC2's ore processing ↔ BlueMarble's beneficiation processes
- **Consequences:** IC2's explosions/meltdowns ↔ BlueMarble's geological hazards (collapses, flooding)

## Summary

IndustrialCraft 2 demonstrates that complex, tiered technology systems can be both engaging and accessible. Its voltage-based progression, infrastructure requirements, and hazard systems provide proven patterns for BlueMarble's crafting and processing systems. The mod's 10+ year success shows players appreciate depth and realistic constraints when properly implemented.

**Status:** ✅ Analysis Complete  
**Next Source:** BuildCraft
