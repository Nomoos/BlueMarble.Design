---
title: Crafting Quality and Failure in Games (WoW Addons, Novus Inceptio and Others)
date: 2026-11-08
owner: @Nomoos
status: complete
tags: [crafting, quality, failure, addons, wow, novus-inceptio, ui]
---

# Crafting Quality and Failure in Games

## Problem / Context

The BlueMarble MVP crafts with learn-by-doing proficiencies, six quality grades and recipes that
have a level and can fail. A failed attempt currently returns all inputs and teaches half. This
note compares how established games handle quality, failure and specialization, and which crafting
features players add through addons. The goal is to choose what fits a learn-by-doing,
geology-driven sandbox.

## Key Findings

- **WoW moved from random quality to a deterministic, budgeted guarantee.** The War Within removed
  the random "Inspiration" stat and added Concentration, a regenerating pool that guarantees the
  next tier. Blizzard named randomness and low crafter agency as the reasons.
- **Addons exist mainly to show money and odds.** Players install them to see cost, profit,
  outcome distribution and the cheapest input mix for a target grade.
- **Few games make failure cost nothing,** but none make it brutal unless failure is rare. OSRS
  iron smelting (50% failure, ore lost, no XP) is a well-known example of how punishing feels.
- **Material-agnostic recipes scale best** (Novus Inceptio, Star Wars Galaxies, Factorio): items
  are a form plus materials, and slots accept material classes.

## Evidence

### World of Warcraft (Dragonflight → The War Within → Midnight)

- **Quality.** Final skill is compared with recipe difficulty. At or above it gives the top tier;
  below it the tier depends on skill. Gear has 5 tiers, reagents 3; Midnight reduces reagents to 2
  ([overview](https://www.wow-professions.com/guides/dragonflight-crafting-system-overview),
  [Wowhead](https://www.wowhead.com/news=378932/simplifying-crafting-professions-and-reagents-in-midnight)).
- **Reagent quality** adds bonus skill: about 25% of base difficulty for top-rank reagents in
  Dragonflight, 40% after a The War Within tuning pass. Each reagent's weight differs per recipe,
  which makes the math opaque.
- **Concentration** (The War Within):
  - a pool of 1000 per profession, regenerating about 240 per day;
  - spending it guarantees the next tier, at a cost that grows with the gap.
  ([wiki](https://warcraft.wiki.gg/wiki/Concentration)).
- **Other crafting stats.**
  - Ingenuity refunds Concentration.
  - Multicraft gives extra output.
  - Resourcefulness saves reagents.
- **Specializations.** Knowledge points come from first crafts and first gathers, then from weekly
  sources.
- **Crafting orders.** The customer supplies the reagents and the crafter still gains skill
  ([wiki](https://warcraft.wiki.gg/wiki/Crafting_Orders)).

### Popular addons and what they add

| Addon | What it adds |
|---|---|
| [TradeSkillMaster](https://support.tradeskillmaster.com/tsm-addon-documentation/1236068-tsm-addon-crafting-operations) | Crafting cost and profit. Restock "operations" (keep 10–20 in stock, craft only above a minimum profit). Automatic queue |
| [CraftSim](https://www.curseforge.com/wow/addons/craftsim) | Cheapest reagent-quality mix for a target tier. Expected profit, price per outcome quality. What-if simulation. Best tool or gear. Concentration value per point |
| [Skillet](https://www.curseforge.com/wow/addons/skillet) | Persistent multi-recipe queue. Shopping list across characters and bank. Filters "can level from" and "have materials". Reagent → recipe drill-down |
| [Auctionator](https://www.curseforge.com/wow/addons/auctionator) | Reagent cost and profit in the crafting window. Shopping lists |
| [CraftScan](https://www.curseforge.com/wow/addons/craftscan) | Scans chat for crafting requests and keeps a customer queue |

**Features players install addons for, and that should be native:**
1. Input cost, profit per craft, profit per hour.
2. Cheapest input-grade mix that reaches a target grade.
3. The full distribution of outcome grades, not one "expected" grade.
4. A what-if simulator (skill, tool, inputs).
5. A persistent queue with an aggregated shopping list.
6. Restock targets.
7. Filters "teaches me something" and "I have materials", plus reagent → recipe drill-down.
8. Best-tool recommendation.
9. An order inbox with history.

### Novus Inceptio (information is thin)

- **Status.** A solo-developer sandbox MMO, in Early Access since October 2015, with Mixed reviews.
  Update 0.60.008 (7 October 2026) rebuilt harvest chance, resource quality and transmutation
  ([Steam](https://store.steampowered.com/app/386590), [Wikipedia](https://en.wikipedia.org/wiki/Novus_Inceptio)).
- **Crafting is part-based and material-agnostic.** "If you use stone, you make stone axe, but if
  you use iron, you create iron axe": one recipe per item form. Wood type and density matter
  ([dev post](https://discussions.unity.com/t/novus-inceptio-indie-mmo-for-players-of-the-old-school-in-development/559470)).
- **Skills and durability.**
  - Skills improve with use, and unused attributes slowly decline.
  - Every item is unique in quality and durability, and repair lowers quality until the item
    "disintegrates into dust".
  - Trees yield bark, leaves, branches and resin, not just wood.
- **Unknown:** failure rules, quality formulas and wear rates. There is no maintained wiki.

### Other games

| Game | Quality model | On failure | XP on failure |
|---|---|---|---|
| WoW (Dragonflight and later) | skill vs difficulty, tiers | crafts don't fail | – |
| Star Wars Galaxies | weighted resource attributes (1–1000); experimentation rolls from +8% to −14% per line | not documented | practice mode gives XP instead of an item |
| Wurm Online | QL 1–100 from skill, tool and material; material QL caps the product | a failed improve damages the item, and repair costs QL | yes |
| Haven & Hearth | deterministic: weighted inputs, then tool (3q + q_tool)/4, then skill soft cap | no roll | – |
| Life is Feudal | influence weights (about 50% material, 20% skill, 10% each tool) | not documented | – |
| Ultima Online | normal or exceptional (+35% damage) | some materials lost, tool loses a use | small |
| EVE Online | no quality; material and time efficiency research | manufacturing never fails | – |
| Vintage Story | the player's own execution, no RNG | mistakes leave recyclable bits | no skills |
| Albion Online | quality roll; return rate 15–54% | never fails | fame always |
| OSRS (iron) | none | 50%: ore lost | none |

Sources: [SWGR](https://swgr.org/wiki/crafting), [Wurmpedia](https://wurmpedia.com/index.php/Quality_Level),
[Ring of Brodgar](https://ringofbrodgar.com/wiki/Quality), [uo.com](https://uo.com/wiki/ultima-online-wiki/skills/blacksmithing/),
[EVE Uni](https://wiki.eveuniversity.org/Manufacturing), [Vintage Story](https://wiki.vintagestory.at/Smithing),
[albionmarket](https://albionmarket.gg/guides/crafting-and-refining-profit),
[OSRS wiki](https://oldschool.runescape.wiki/w/Iron_bar).

### Extensible recipe patterns

- **Form + material** (Novus Inceptio, Star Wars Galaxies, Mortal Online 2). Slots accept material
  classes, and per-slot weights turn material properties into item stats.
- **Reactions with material filters and probabilistic products** (Dwarf Fortress).
- **Recipe categories bound to machines, with byproducts and quality inheritance** (Factorio). The
  recycler returns 25% of the inputs.
- **Alternate recipes as trade-offs, not upgrades** (Satisfactory).
- **A stateful workpiece shaped by operations** (Wurm, Vintage Story).

## Implications for Design

1. **Show the distribution, keep the spread small.** Before crafting, show odds such as "70% Fine,
   25% Good". Hidden randomness hurts agency, which was Blizzard's lesson.
2. **Make failure cost something, scaled by the gap.** Consume 25–50% of the inputs and turn the
   lost part into a geological byproduct (slag, rock dust) that can be recycled at about 25%.
   Keep near-full learning: punishing failure fits learn-by-doing badly. See
   [realistic smelting and mining outcomes](realistic-smelting-and-mining-outcomes.md).
3. **Tools.**
   - The tier gates what material hardness can be worked.
   - Tool quality nudges success and a little of the output quality.
   - Wear consumes the tool, and repair lowers its maximum quality (a sink).
4. **An optional Concentration-like budget** that guarantees +1 grade.
5. **Material-agnostic recipes:** slots take material classes, and geology-generated properties
   drive the stats.
6. **Build the addon features in:** cost, profit, odds, cheapest mix, queue and shopping list.
7. **A first-craft bonus per recipe × material** rewards breadth.

## Next Steps

- Decide the failure cost for the MVP's recipes.
- Prototype the grade-distribution preview, which the MVP crafting card already approximates with
  a range.
