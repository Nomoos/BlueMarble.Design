# Crafting Interface Visual Mockups

**Document Type:** UI/UX Design Reference  
**Version:** 1.0  
**Author:** Game Design Research Team  
**Date:** 2025-01-08  
**Related Document:** [Assembly Skills System Research](../assembly-skills-system-research.md)

## Overview

This document provides detailed visual mockups for the crafting interface proposed in the Assembly Skills 
System Research. These mockups demonstrate how players interact with the crafting system, including material 
selection, success rate visualization, and quality feedback.

## Complete Crafting Workflow

### 1. Main Crafting Hub Interface

```
╔════════════════════════════════════════════════════════════════════════════╗
║                          CRAFTING WORKSHOP                                 ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║  [Blacksmithing]  [Tailoring]  [Alchemy]  [Woodworking]                  ║
║  ════════════════                                                          ║
║                                                                            ║
║  Your Level: 35                    Experience: 28,450 / 35,355            ║
║  Progress: [████████████████░░░░░░░░░░░░░░░░░░] 80% to Level 36          ║
║                                                                            ║
║  ┌──────────────────────────────────────────────────────────────────┐    ║
║  │ SPECIALIZATION: Weaponsmith                                      │    ║
║  │ Bonuses: +15% success for weapons, +10% quality, -20% materials  │    ║
║  └──────────────────────────────────────────────────────────────────┘    ║
║                                                                            ║
╠════════════════════════════════════════════════════════════════════════════╣
║ RECENT CRAFTS                     │  RECOMMENDED RECIPES                  ║
║ ─────────────────────────────     │  ────────────────────────             ║
║ ✓ Fine Steel Sword   (+125 XP)    │  [⚔] Damascus Blade                  ║
║ ✓ Iron Dagger        (+45 XP)     │       Level 38 (You: 35)             ║
║ ✗ Steel Plate Armor  (Failed)     │       Success: ~65% 🟡                ║
║                                    │       High XP Gain                   ║
║                                    │                                       ║
║                                    │  [⚔] Master Steel Sword              ║
║                                    │       Level 35 (You: 35)             ║
║                                    │       Success: ~88% 🟢                ║
║                                    │       Standard XP                    ║
╚════════════════════════════════════════════════════════════════════════════╝
```

### 2. Recipe Selection and Material View

```
╔════════════════════════════════════════════════════════════════════════════╗
║                      CRAFTING: Steel Longsword                             ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║  Recipe Difficulty: Level 35 (Recommended)                                ║
║  Your Skill: Level 35 (Weaponsmith Specialization Active)                ║
║  Estimated Success Rate: 88% 🟢  [?] Show Breakdown                       ║
║  Expected Quality: Fine to Superior (65-88%)                              ║
║                                                                            ║
║ ┌────────────────────────────────────────────────────────────────────────┐║
║ │ REQUIRED MATERIALS                                                     │║
║ │ ─────────────────────────────────────────────────────────────────────  │║
║ │                                                                         │║
║ │ [🔲] Steel Ingot × 4                                                   │║
║ │      Available: 12 in inventory                                        │║
║ │      Quality Range: 65% - 85%                                          │║
║ │      ┌────────────────────────────────────────────┐                    │║
║ │      │ Select Steel Ingots:                       │                    │║
║ │      │ ☑ High Quality (85%) ██████████ +10%      │                    │║
║ │      │ ☐ Standard (70%)     ███████░░░            │                    │║
║ │      │ ☐ Standard (68%)     ██████░░░░            │                    │║
║ │      │ ☐ Poor (52%)         █████░░░░░ -5%       │                    │║
║ │      └────────────────────────────────────────────┘                    │║
║ │                                                                         │║
║ │ [🔲] Oak Handle × 1                                                    │║
║ │      Available: 3 in inventory                                         │║
║ │      Quality Range: 60% - 75%                                          │║
║ │      Selected: Aged Oak (75%) ████████░░ +5%                          │║
║ │                                                                         │║
║ │ OPTIONAL MATERIALS (Enhance Quality)                                   │║
║ │ ─────────────────────────────────────────────────────────────────────  │║
║ │                                                                         │║
║ │ [□] Quenching Oil                                                      │║
║ │     Effect: +10% quality, +5% durability                               │║
║ │     Available: 2 bottles                                               │║
║ │                                                                         │║
║ │ [□] Grindstone Polish                                                  │║
║ │     Effect: +8% sharpness, improved appearance                         │║
║ │     Available: 1 jar                                                   │║
║ │                                                                         │║
║ └────────────────────────────────────────────────────────────────────────┘║
║                                                                            ║
║ ┌────────────────────────────────────────────────────────────────────────┐║
║ │ CRAFTING SUMMARY                                                       │║
║ │ ─────────────────────────────────────────────────────────────────────  │║
║ │ Base Success Rate:        75%                                          │║
║ │ + Skill Match:          +13%  (Level 35 / 35)                         │║
║ │ + Specialization:       +15%  (Weaponsmith)                           │║
║ │ + Material Quality:      +8%  (High quality steel)                    │║
║ │ + Workshop Bonus:        +5%  (Master Forge)                          │║
║ │ - Item Complexity:      -28%  (Complex weapon)                        │║
║ │                          ────                                           │║
║ │ TOTAL SUCCESS RATE:      88% 🟢                                        │║
║ │                                                                         │║
║ │ Expected Quality: 72% (Fine) with chance for Superior                  │║
║ │ Crafting Time: 45 minutes                                              │║
║ │ Experience Gain: 180 XP (success) / 50 XP (failure)                   │║
║ └────────────────────────────────────────────────────────────────────────┘║
║                                                                            ║
║                    [Cancel]  [Begin Crafting]                             ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝
```

### 3. Interactive Crafting Progress

```
╔════════════════════════════════════════════════════════════════════════════╗
║                   CRAFTING IN PROGRESS: Steel Longsword                    ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║  Stage 1 of 4: Heating Metal to Forging Temperature                       ║
║  ─────────────────────────────────────────────────────────────────────     ║
║                                                                            ║
║                          🔥 FORGE FIRE 🔥                                  ║
║                    ┌──────────────────────┐                               ║
║                    │   🔲��🔲🔲🔲🔲🔲🔲   │                               ║
║                    │   🔲🔥🔥🔥🔥🔥🔥🔲   │  Temperature: 1,420°C         ║
║                    │   🔲🔥🔥🔥🔥🔥🔥🔲   │  Target: 1,200-1,500°C        ║
║                    │   🔲🔥🔲🔲🔲🔥🔲🔲   │  Status: OPTIMAL ✓            ║
║                    │   🔲🔲🔲🔲🔲🔲🔲🔲   │                               ║
║                    └──────────────────────┘                               ║
║                                                                            ║
║  Progress: [████████████████░░░░░░░░░░] 65%                              ║
║  Time Remaining: ~15 seconds                                              ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ QUALITY INDICATORS                                                 │  ║
║  │                                                                     │  ║
║  │ Temperature Control:  ████████░░ Excellent ✓                      │  ║
║  │ Heating Uniformity:   ███████░░░ Good ✓                           │  ║
║  │ Timing:               █████████░ Excellent ✓                      │  ║
║  │                                                                     │  ║
║  │ Current Quality Projection: 78% (Fine → Superior)                  │  ║
║  │ Bonus chance active: Master timing (+5% quality)                   │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  Next Stage: Hammering and Shaping                                        ║
║                                                                            ║
║                           [Cancel Crafting]                                ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝
```

### 4. Interactive Stage - Hammering

```
╔════════════════════════════════════════════════════════════════════════════╗
║                   CRAFTING IN PROGRESS: Steel Longsword                    ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║  Stage 2 of 4: Hammering and Shaping                                      ║
║  ─────────────────────────────────────────────────────────────────────     ║
║                                                                            ║
║              🔨 HAMMER THE METAL IN THE HIGHLIGHTED ZONES 🔨               ║
║                                                                            ║
║                    Blade Shape Progress: 45%                               ║
║                                                                            ║
║           ╔══════════════════════════════════════════╗                    ║
║           ║          🔨 ←  Hit here!                 ║                    ║
║           ║    ┌─────────────────────────┐          ║                    ║
║           ║    │█████████████████████████▓│          ║  Temperature:     ║
║           ║    │█████████░░░░░░░░████████▓│          ║  1,380°C          ║
║           ║    │████████░░░░░░░░░░███████▓│  ← Hit  ║  GOOD ✓           ║
║           ║    │███████░░░░░░░░░░░███████▓│          ║                   ║
║           ║    │██████░░░░░[▓]░░░░███████▓│          ║  Symmetry:        ║
║           ║    │███████░░░░░░░░░░░███████▓│          ║  ████████░ 82%    ║
║           ║    │████████░░░░░░░░░████████▓│          ║                   ║
║           ║    │█████████████████████████▓│          ║  Shape:           ║
║           ║    │█████████████████████████▓│          ║  ███████░░ 75%    ║
║           ║    └─────────────────────────┘          ║                   ║
║           ║              Sword Blank                 ║                    ║
║           ╚══════════════════════════════════════════╝                    ║
║                                                                            ║
║  Hammer Strikes: 23 / ~35           Accuracy: ████████░░ 85%             ║
║                                                                            ║
║  💡 TIP: Keep temperature in optimal range for best quality               ║
║       Return to forge if metal cools below 1,100°C                        ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝
```

### 5. Success Result - Superior Quality

```
╔════════════════════════════════════════════════════════════════════════════╗
║                        ✨ CRAFTING SUCCESSFUL! ✨                          ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║                              ⚔️                                             ║
║                      SUPERIOR STEEL LONGSWORD                              ║
║                              ⚔️                                             ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │                                                                     │  ║
║  │  Quality: 87% ████████████████████ SUPERIOR                        │  ║
║  │  Rarity: 🟣 Purple (Superior Tier)                                 │  ║
║  │                                                                     │  ║
║  │  ─────────────────────────────────────────────────                 │  ║
║  │                                                                     │  ║
║  │  ⚔️  Damage: 48-52 (+25% from quality)                             │  ║
║  │  🛡️  Durability: 225/225 (+50% from quality)                       │  ║
║  │  ⚡  Attack Speed: 1.4s (base)                                      │  ║
║  │  📊  Weight: 3.2 kg                                                │  ║
║  │                                                                     │  ║
║  │  ─────────────────────────────────────────────────                 │  ║
║  │                                                                     │  ║
║  │  ✨ QUALITY BONUSES:                                               │  ║
║  │  • +25% Base Damage                                                │  ║
║  │  • +50% Durability                                                 │  ║
║  │  • +10% Critical Hit Chance                                        │  ║
║  │  • Reduced Repair Costs (-20%)                                     │  ║
║  │  • Enhanced Appearance (Visual polish effect)                      │  ║
║  │                                                                     │  ║
║  │  ─────────────────────────────────────────────────                 │  ║
║  │                                                                     │  ║
║  │  🏷️ Crafted by: [Your Name]                                        │  ║
║  │     Signature Quality Mark Applied                                 │  ║
║  │     Market Value: ~450 Gold                                        │  ║
║  │                                                                     │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ 🎓 EXPERIENCE GAINED: 270 XP                                        │  ║
║  │                                                                     │  ║
║  │    Blacksmithing: 28,450 → 28,720 / 35,355                         │  ║
║  │    [████████████████░░░░░░░░░░░░░░░░] 81%                         │  ║
║  │                                                                     │  ║
║  │    🎯 Quality Crafting Bonus: +50% XP                              │  ║
║  │    🔨 Weaponsmith Specialization: +20% XP                          │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║                                                                            ║
║          [Equip Item]  [Add to Inventory]  [Craft Another]                ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝
```

### 6. Failure Result - Learning Experience

```
╔════════════════════════════════════════════════════════════════════════════╗
║                          ⚠️  CRAFTING FAILED  ⚠️                           ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║                         🔨 Damaged Sword Blank 🔨                          ║
║                              (Unusable)                                    ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │  WHAT WENT WRONG:                                                  │  ║
║  │                                                                     │  ║
║  │  ❌ Temperature dropped too low during shaping                     │  ║
║  │     → Metal became brittle and cracked                             │  ║
║  │                                                                     │  ║
║  │  ⚠️  Uneven hammer strikes                                          │  ║
║  │     → Blade warped and lost structural integrity                   │  ║
║  │                                                                     │  ║
║  │  ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━    │  ║
║  │                                                                     │  ║
║  │  💡 LESSONS LEARNED:                                               │  ║
║  │                                                                     │  ║
║  │  • Maintain temperature above 1,100°C during shaping               │  ║
║  │  • Use more consistent hammer strikes for symmetry                 │  ║
║  │  • Consider using quenching oil for better temperature control     │  ║
║  │                                                                     │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ 📚 MATERIALS OUTCOME:                                              │  ║
║  │                                                                     │  ║
║  │  Steel Ingots (4): 2 recovered (50% salvage rate)                  │  ║
║  │  Oak Handle (1): Lost (consumed in attempt)                        │  ║
║  │                                                                     │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ 🎓 EXPERIENCE GAINED: 60 XP                                         │  ║
║  │                                                                     │  ║
║  │    Blacksmithing: 28,450 → 28,510 / 35,355                         │  ║
║  │    [████████████████░░░░░░░░░░░░░░░░] 81%                         │  ║
║  │                                                                     │  ║
║  │    💡 Learning from failure is part of mastery!                    │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║                                                                            ║
║           [Review Lessons]  [Try Again]  [Return to Workshop]             ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝
```

## Tailoring Interface Example

### Crafting Fiber Clothing (Referenced in Issue)

```
╔════════════════════════════════════════════════════════════════════════════╗
║                      CRAFTING: Linen Tunic from Fiber                      ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║  Recipe Difficulty: Level 8 (Recommended)                                 ║
║  Your Tailoring Skill: Level 12 (Above recommended)                       ║
║  Estimated Success Rate: 92% 🟢  [?] Show Breakdown                       ║
║  Expected Quality: Fine (68-82%)                                          ║
║                                                                            ║
║ ┌────────────────────────────────────────────────────────────────────────┐║
║ │ FIBER MATERIALS (Select Quality)                                      │ ║
║ │ ─────────────────────────────────────────────────────────────────────  │║
║ │                                                                         │║
║ │ [🌿] Flax Fiber × 6                                                    │║
║ │                                                                         │║
║ │      YOUR INVENTORY:                                                   │║
║ │      ┌──────────────────────────────────────────────┐                  │║
║ │      │ ⭐ Premium Flax (88%) ██████████░ +15%      │ ← Currently      │║
║ │      │   Source: Aged plants, perfect climate      │   Selected       │║
║ │      │   Effect: Higher quality, better durability │                  │║
║ │      │   Price: 8 gold/unit                        │                  │║
║ │      │                                              │                  │║
║ │      │ ○ Standard Flax (65%) ███████░░░            │                  │║
║ │      │   Source: Common harvest                    │                  │║
║ │      │   Effect: Normal quality                    │                  │║
║ │      │   Price: 3 gold/unit                        │                  │║
║ │      │                                              │                  │║
║ │      │ ○ Poor Flax (42%) ████░░░░░░ -8%           │                  │║
║ │      │   Source: Young plants                      │                  │║
║ │      │   Effect: Lower quality, reduced durability │                  │║
║ │      │   Price: 1 gold/unit                        │                  │║
║ │      └──────────────────────────────────────────────┘                  │║
║ │                                                                         │║
║ │ [🧵] Thread × 2                                                        │║
║ │      Selected: Fine Linen Thread (72%) ████████░░                     │║
║ │                                                                         │║
║ │ [🎨] Dye (Optional)                                                    │║
║ │      [□] Natural Blue Dye (Rare)                                       │║
║ │          Effect: Attractive appearance, +20% market value              │║
║ │                                                                         │║
║ └────────────────────────────────────────────────────────────────────────┘║
║                                                                            ║
║ ┌────────────────────────────────────────────────────────────────────────┐║
║ │ MATERIAL CHOICE IMPACT                                                 │║
║ │ ─────────────────────────────────────────────────────────────────────  │║
║ │                                                                         │║
║ │ Using Premium Flax:                   Using Standard Flax:             │║
║ │ • Success Rate: 92% 🟢                • Success Rate: 87% 🟢           │║
║ │ • Quality: Fine-Superior (68-82%)     • Quality: Standard-Fine (55-70%)│║
║ │ • Durability: High (+40%)             • Durability: Normal             │║
║ │ • Market Value: ~45 gold              • Market Value: ~28 gold         │║
║ │ • Material Cost: 48 gold              • Material Cost: 18 gold         │║
║ │ • Profit Potential: -3 gold loss      • Profit Potential: +10 gold    │║
║ │   (But: Better XP, builds reputation) │   (Lower quality, less XP)     │║
║ │                                                                         │║
║ └────────────────────────────────────────────────────────────────────────┘║
║                                                                            ║
║ 💡 TAILORING TIP: Higher skill levels unlock better use of premium        ║
║    materials. Your level 12 skill makes the most of this premium flax!   ║
║                                                                            ║
║                    [Cancel]  [Begin Crafting]                             ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝
```

## Skill Progression Visualization

### Experience and Level Progress

```
╔════════════════════════════════════════════════════════════════════════════╗
║                       BLACKSMITHING SKILL DETAILS                          ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║  Current Level: 35                    Specialization: Weaponsmith          ║
║                                                                            ║
║  Experience: 28,720 / 35,355 (81%)                                        ║
║  [████████████████████████████████████████████░░░░░░░░]                   ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ LEVEL 35 UNLOCKS:                                                  │  ║
║  │ ✓ Steel weapon mastery                                             │  ║
║  │ ✓ Advanced tempering techniques                                    │  ║
║  │ ✓ Composite armor crafting                                         │  ║
║  │ ✓ Weapon customization options                                     │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ LEVEL 36 PREVIEW (6,635 XP away):                                  │  ║
║  │ 🔒 Advanced alloy creation                                         │  ║
║  │ 🔒 Decorative inlay techniques                                     │  ║
║  │ 🔒 Improved success rates (+2%)                                    │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  ─────────────────────────────────────────────────────────────────────    ║
║                                                                            ║
║  MILESTONE PROGRESS TO MASTER (Level 50):                                 ║
║                                                                            ║
║  Basic Training     [██████████] 20 ✓ Complete                            ║
║  Journeyman Work    [█████████░] 35   Current Level                       ║
║  Expert Techniques  [░░░░░░░░░░] 50   15 levels remaining                ║
║  Master Smith       [░░░░░░░░░░] 75   Master specialization unlocked     ║
║  Legendary Crafter  [░░░░░░░░░░] 100  Ultimate achievement               ║
║                                                                            ║
║  ─────────────────────────────────────────────────────────────────────    ║
║                                                                            ║
║  STATISTICS:                                                               ║
║  • Items Crafted: 487                                                     ║
║  • Success Rate: 82%                                                      ║
║  • Quality Average: 68% (Fine tier)                                       ║
║  • Masterwork Items: 12                                                   ║
║  • Critical Successes: 38                                                 ║
║  • Skill Points Earned: 35                                                ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝
```

### Specialization Tree

```
╔════════════════════════════════════════════════════════════════════════════╗
║                    BLACKSMITHING SPECIALIZATION PATHS                      ║
║                          (Unlocked at Level 25)                            ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║   ┌──────────────────────┐  ┌──────────────────────┐  ┌─────────────────┐║
║   │   WEAPONSMITH   ⚔️   │  │   ARMORSMITH   🛡️   │  │  TOOLMAKER  🔧  │║
║   │ ─────────────────────│  │ ─────────────────────│  │ ────────────────│║
║   │  ✓ SELECTED          │  │                      │  │                 │║
║   │                      │  │                      │  │                 │║
║   │ Specialization:      │  │ Specialization:      │  │ Specialization: │║
║   │ Weapons & Blades     │  │ Armor & Protection   │  │ Tools & Utility │║
║   │                      │  │                      │  │                 │║
║   │ BONUSES:             │  │ BONUSES:             │  │ BONUSES:        │║
║   │ +15% weapon success  │  │ +15% armor success   │  │ +15% tool       │║
║   │ +10% weapon quality  │  │ +10% armor quality   │  │     success     │║
║   │ -20% materials       │  │ +20% durability      │  │ +10% quality    │║
║   │ -25% craft time      │  │ -20% materials       │  │ -30% materials  │║
║   │                      │  │                      │  │ +5 tool uses    │║
║   │ UNLOCKS:             │  │ UNLOCKS:             │  │                 │║
║   │ • Damascus blades    │  │ • Plate armor        │  │ UNLOCKS:        │║
║   │ • Enchanted weapons  │  │ • Shield mastery     │  │ • Precision     │║
║   │ • Legendary swords   │  │ • Composite armor    │  │   tools         │║
║   │ • Custom hilts       │  │ • Decorative armor   │  │ • Mining gear   │║
║   │                      │  │                      │  │ • Craft tools   │║
║   │ PROGRESSION:         │  │ PROGRESSION:         │  │ • Engineering   │║
║   │ Level 35 / 100       │  │ Not chosen           │  │   components    │║
║   │ [███████░░░] 35%    │  │                      │  │                 │║
║   └──────────────────────┘  └──────────────────────┘  └─────────────────┘║
║                                                                            ║
║  ⚠️  NOTE: Specialization choice is permanent but you can still craft     ║
║           items from other paths (just without bonuses)                   ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝
```

## Quality Tier Visualization

```
╔════════════════════════════════════════════════════════════════════════════╗
║                         ITEM QUALITY COMPARISON                            ║
╠════════════════════════════════════════════════════════════════════════════╣
║                                                                            ║
║  Example: Iron Longsword at Different Quality Tiers                       ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ CRUDE (0-20%)                    ⚔️                                 │  ║
║  │ ━━━━░░░░░░░░░░░░░░░░ 15%                                           │  ║
║  │                                                                     │  ║
║  │ Damage: 28-32 (-20%)             Visual: Rough, unpolished         │  ║
║  │ Durability: 60/60 (-40%)         Appearance: Crude workmanship     │  ║
║  │ Repair Cost: High                Market Value: 8 gold              │  ║
║  │ Special: Faster degradation      Seller Rating: -10%               │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ STANDARD (21-60%)                ⚔️                                 │  ║
║  │ ████████░░░░░░░░░░░░ 45%                                           │  ║
║  │                                                                     │  ║
║  │ Damage: 35-40 (base)             Visual: Standard polish            │  ║
║  │ Durability: 100/100              Appearance: Functional             │  ║
║  │ Repair Cost: Normal              Market Value: 20 gold             │  ║
║  │ Special: None                    Seller Rating: Standard            │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ FINE (61-85%)                    ⚔️✨                               │  ║
║  │ █████████████░░░░░░░ 72%                                           │  ║
║  │                                                                     │  ║
║  │ Damage: 38-44 (+10%)             Visual: Well-polished             │  ║
║  │ Durability: 125/125 (+25%)       Appearance: Quality craftsmanship │  ║
║  │ Repair Cost: Reduced             Market Value: 35 gold             │  ║
║  │ Special: +5% crit chance         Seller Rating: +15%               │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ SUPERIOR (86-95%)                ⚔️✨✨                             │  ║
║  │ ████████████████░░░░ 88%                                           │  ║
║  │                                                                     │  ║
║  │ Damage: 44-50 (+25%)             Visual: Excellent polish & details│  ║
║  │ Durability: 150/150 (+50%)       Appearance: Superior work         │  ║
║  │ Repair Cost: Very low            Market Value: 75 gold             │  ║
║  │ Special: +10% crit, +5% speed    Seller Rating: +30%               │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
║  ┌────────────────────────────────────────────────────────────────────┐  ║
║  │ MASTERWORK (96-100%)             ⚔️✨✨⭐                           │  ║
║  │ ████████████████████ 98%                                           │  ║
║  │                                                                     │  ║
║  │ Damage: 53-60 (+50%)             Visual: Perfect, artistic         │  ║
║  │ Durability: 200/200 (+100%)      Appearance: Masterwork            │  ║
║  │ Repair Cost: Minimal             Market Value: 200+ gold           │  ║
║  │ Special: +15% crit, +10% speed   Seller Rating: +50%               │  ║
║  │          +1 Enchantment slot     Crafter Signature visible         │  ║
║  │          Legendary status        Highly sought after               │  ║
║  └────────────────────────────────────────────────────────────────────┘  ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝
```

## Mobile/Tablet Responsive Design

```
╔════════════════════════════════╗
║  CRAFTING (Mobile View)        ║
╠════════════════════════════════╣
║                                ║
║  Steel Longsword               ║
║  Level 35 (Recommended)        ║
║                                ║
║  Success: 88% 🟢               ║
║  Quality: Fine-Superior        ║
║                                ║
║  ┌──────────────────────────┐ ║
║  │ Materials:               │ ║
║  │                          │ ║
║  │ [🔲] Steel Ingot × 4    │ ║
║  │      ████████░░ 85%     │ ║
║  │                          │ ║
║  │ [🔲] Oak Handle × 1     │ ║
║  │      ████████░░ 75%     │ ║
║  │                          │ ║
║  │ Optional:                │ ║
║  │ [□] Quenching Oil       │ ║
║  │     +10% quality         │ ║
║  └──────────────────────────┘ ║
║                                ║
║  Materials: 32g                ║
║  Time: 45 min                  ║
║  XP: 180                       ║
║                                ║
║  [Cancel]    [Craft]           ║
║                                ║
╚════════════════════════════════╝
```

---

## Design Notes and Rationale

### Color Coding System
- **Green (🟢):** Success rates 75%+, safe crafting
- **Yellow (🟡):** Success rates 50-74%, moderate risk
- **Orange (🟠):** Success rates 25-49%, risky
- **Red (🔴):** Success rates <25%, very risky

### Quality Indicators
- **Gray:** Crude quality (poor craftsmanship)
- **White:** Standard quality (normal items)
- **Blue:** Fine quality (above average)
- **Purple (🟣):** Superior quality (excellent work)
- **Gold (🟠):** Masterwork quality (legendary items)

### User Experience Principles
1. **Transparency:** Players always see success rates and expected quality
2. **Informed Choice:** Material selection shows impact on outcomes
3. **Learning from Failure:** Failed attempts provide useful feedback
4. **Progress Feedback:** Clear indication of skill progression and unlocks
5. **Strategic Depth:** Multiple paths to success (materials, tools, environment)

### Accessibility Considerations
- Clear visual hierarchy with borders and spacing
- Color coding supplemented with text indicators
- Consistent layout across all crafting professions
- Screen reader friendly text labels
- Keyboard navigation support

---

**Document Version:** 1.0  
**Last Updated:** 2025-01-08  
**Related Documents:** [Assembly Skills System Research](../assembly-skills-system-research.md)
