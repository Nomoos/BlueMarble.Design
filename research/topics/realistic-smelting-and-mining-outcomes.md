---
title: Realistic Outcomes of Failed Smelting, Charcoal Burning, Forging and Mining
date: 2026-11-08
owner: @Nomoos
status: complete
tags: [metallurgy, bloomery, slag, mining, gangue, gemstones, crafting-failure]
---

# Realistic Outcomes of Failed Smelting, Charcoal Burning, Forging and Mining

## Problem / Context

In the BlueMarble MVP, a failed smelt returns its inputs, and mining never produces anything except
ore. Both are unrealistic. This note collects what pre-industrial and early-modern practice
actually produced when things went wrong, and what miners brought up instead of ore. It ends with
game-ready tables. The **probabilities in the tables are design suggestions**, not historical
figures. The facts above them are sourced.

## Key Findings

- **Bloomery yields are low.** A run gives a bloom of about 10% of the ore's weight in modern
  experiments, and 5–45% depending on ore. Hammering the bloom into a bar loses another 25–60%.
- **Failure produces material, not nothing.** Failed smelts leave slag cakes, spongy blooms or
  cast-iron lumps, and most of it can be re-smelted. Bloomery slag can hold up to 87% iron oxide.
- **Mining always brings up rock.** Without ore, the miner gets waste rock and gangue minerals, and
  sometimes a collectible tied to the host rock (quartz crystals, garnet, jasper, malachite, amber).
- **Several BlueMarble.Design documents conflict with these facts** (see the last section).

## Evidence

### Bloomery smelting

**Typical figures**
- **Ore and charcoal** go in at about 1:1 by weight. The Wealden Iron Research Group (WIRG) charges
  1 kg of each every 10–15 minutes, over about 5 hours, at 950–1125 °C
  ([WIRG](https://www.wealdeniron.org.uk/?p=930)).
- **Yield:** about 1.5 kg of bloom from 15 kg of ore at WIRG. Wareham Forge reports 5–45%
  ([WIRG bloom](https://www.wealdeniron.org.uk/experimental-bloomery-ironmaking/exp-bloom/),
  [Wareham](https://www.warehamforge.ca/ironsmelting/smelt/smelt06/index.html)).
- **Minimum charge:** about 8 kg of ore is used up building the slag bowl before any iron collects
  ([Wareham density](https://warehamforge.ca/ironsmelting/iron2022/density/density.html)).

**What goes wrong**
- **Air stops or runs low.** A bellows stop of 3–5 minutes freezes the slag; low air gives "lacy"
  iron ([EXARC](https://exarc.net/issue-2012-1/ea/if-you-dont-get-any-iron-towards-effective-method-small-iron-smelting-furnaces)).
- **Spongy blooms were common.** Of about 90 archaeological blooms, 34% were unconsolidated sponge
  and about 30% partly consolidated (Pleiner, quoted by Wareham).
- **Too much carbon** gives unforgeable cast iron. It was later decarburised in a finery forge
  ([Bloomery](https://en.wikipedia.org/wiki/Bloomery)).
- **A stuck bloom** is broken out of the frozen slag bowl cold: a spiky, low-density mass.
- **"Bear" and "salamander"** are blast-furnace terms: a fused mass of iron, unmelted charge and
  lining ([Beckley Furnace](https://beckleyfurnace.org/?p=350)).
- **Bad ores.** Bog ore gives phosphoric, cold-short iron; sulphur makes hot-short iron.

**Slag and reuse**
- Bloomery slag (fayalite and wüstite) holds up to 87% iron oxide, against about 2% for
  blast-furnace slag ([WIRG slags](https://www.wealdeniron.org.uk/?p=941)).
- Old slag was re-smelted, and slag heaps fed later blast furnaces. Slag was also used for roads.
- Hammer scale goes back into the charge.

**Bloom to bar.** Wareham recovers 60–75% of the bloom as bar, and 41% in the worst case
([Bloom2Bar](https://warehamforge.ca/ironsmelting/Bloom2Bar/bloom2bar.htm)).

### Charcoal burning

- **Yield:** about 25% by weight and 60% by volume. Good practice is 1 t of charcoal from 4 t of
  air-dry wood, 1:6 is common ([FAO](https://www.fao.org/4/x5328e/x5328e07.htm),
  [Charcoal](https://en.wikipedia.org/wiki/Charcoal)).
- **Failures:**
  - Uneven air burns part of the kiln to ash and leaves "brands" (under-charred pieces) for the
    next burn ([FAO ch. 4](https://www.fao.org/4/x5328e/x5328e05.htm)).
  - Wet wood lowers the yield.
  - Temperature trades yield against quality: 300 °C gives 42% yield with 68% carbon; 700 °C gives
    30% with 92%.

### Forging failures

- **Burnt steel** sparks and crumbles, and the burnt part is cut off. Wrought iron tolerates a
  sparking heat ([iforgeiron](https://www.iforgeiron.com/topic/8589-how-do-know-if-i-burnt-my-steel/)).
- **Other defects:** cold shuts, laps, cracks and quench cracks.
- **Scale loss** is about 2% on the first heat and about 1% on each later heat (forum estimate).
- **Salvage.** Scrap is bundled and forge-welded again ("faggoting"), and iron faggoted twice was
  sold as "Best" ([Faggoting](https://en.wikipedia.org/wiki/Faggoting_(metalworking))).

### Mining: waste rock, gangue and chance finds

**Terms**
- **Gangue:** worthless minerals grown together with the ore.
- **Waste rock:** barren country rock.
- **Tailings:** fines left after processing.

**Ore grade.** Pure magnetite is 72.4% iron, hematite 69.9%, goethite 62.9%, siderite 48.2%. Real
ores are lower ([Iron ore](https://en.wikipedia.org/wiki/Iron_ore)).

**Productivity.** Hand-driven galleries advanced 2–3 cm per day. Fire-setting broke about 1 t of
rock per 1 t of wood.

**Sorting.** Hand-sorting ("cobbing") was a separate job, done in Cornwall by the bal maidens
([Cornish Mining](https://cdn.cornishmining.org.uk/about/mining-in-cornwall-and-west-devon/mining-processes/dressing-the-ore)).

**What each setting yields besides ore**
- **Hematite in limestone** (Cumbria type).
  - Gangue: calcite, dolomite, baryte.
  - Finds: quartz crystals, rarely red "tomato quartz".
  - Source: [BGS](https://earthwise.bgs.ac.uk/index.php/Haematite_deposits_of_Cumbria).
- **Banded iron formation.** The ore is layered with jasper.
- **Skarn.** Gangue: garnet, pyroxene, epidote. Finds: pyrite, chalcopyrite.
- **Bog iron.** Comes with sand and quartz, and regrows within about a generation.
- **Gossan.** An "iron hat" of limonite and quartz above buried sulphides. Malachite and azurite
  form in the oxidised zone, and the gossan was a prospector's sign of ore below.
- **Collectibles tied to host rock:**
  - amethyst and agate in basalt cavities;
  - tourmaline and beryl in pegmatites;
  - jet in Jurassic shale;
  - amber in coal seams;
  - flint in chalk.

**Uses of waste**
- Fluorite as a flux.
- Limestone burnt to lime.
- Crushed rock for roads and fill; building stone; backfill for worked-out stopes.

## Game-Ready Tables

### Failed smelt outcomes

Skill decides how often a smelt fails. This table decides how it fails. Inputs are consumed.

| Cause | Share | Result | Reuse |
|---|---|---|---|
| Too little air or too cold | 35% | Slag cake with partly reduced ore | Crush and re-smelt (counts as about 0.5 ore) |
| Spongy bloom | 30% | Spongy bloom (low grade) plus slag; 50% chance it crumbles when consolidated | Pile the crumbs or re-smelt them |
| Re-oxidised (too much air) | 10% | Small bloom (about 40%) plus iron-rich slag | Re-smelt the slag |
| Too much carbon or too hot | 10% | Cast-iron lump, unforgeable | Re-smelt it, or later decarburise it in a finery forge |
| Bloom stuck, furnace frozen | 10% | Iron-and-slag mass; the furnace needs repair | About 50% recoverable as crumbs |
| Charge below the slag-bowl threshold | 5% | Tap slag only | Road fill |

Item tags that go with these outcomes:
- **Cold-short** (bog-ore iron): good for nails, worth less.
- **Brands:** half-burned wood from a charcoal burn, which goes into the next burn.
- **Fines and ash:** lost.

### Mining outcomes by setting

Rarity per mining action: U = 1–5%, R = 0.1–1%, VR = under 0.1%.

| Setting | Ore (grade) | Waste rock | Gangue | Chance finds |
|---|---|---|---|---|
| Hematite vein in limestone | hematite (50–65%) | limestone, dolomite | calcite, baryte, quartz | U: quartz crystals. R: fluorite. VR: tomato quartz |
| Magnetite skarn | magnetite (40–60%) | marble, hornfels | garnet, epidote | U: garnet, pyrite. R: chalcopyrite. VR: gem garnet |
| Banded iron formation | hematite/magnetite (25–35%) | chert, schist | chert | U: jasper |
| Bog iron | limonite (20–50%), regrows | peat, sand, clay | quartz sand | R: amber (coastal). VR: bog oak |
| Siderite ironstone | siderite (25–40%) | shale, sandstone | clay, pyrite | U: coal. R: amber, fossils. VR: jet |
| Gossan | limonite (30–50%) | granite, schist | quartz | U: malachite. R: azurite. VR: tourmaline, beryl |

On a "no ore" roll the miner gets waste rock and gangue. Sorting skill ("cobbing") turns more of the
mined rock into usable ore.

## Conflicts in BlueMarble.Design

- **Failed crafts lose nothing.** `docs/gameplay/mechanics/crafting-mechanics-overview.md:40` and
  `material-economy-system.md:357` say so, and the MVP does the same today. This contradicts reality.
- **Charcoal ratio and limestone.**
  `research/literature/survival-content-extraction-02-appropriate-technology.md:338-366` uses
  2:1 charcoal to ore and requires limestone. Real practice is about 1:1, and a bloomery is
  self-fluxing. Producing slag there is correct.
- **Temperature.** `research/game-design/step-1-foundation/mechanics-research.md:245-252` says
  ">1500 °C", which is cast-iron territory, not a bloomery.
- **Ore grades.** `mechanics-research.md:268-287` gives magnetite a lower grade than hematite;
  the reverse is true. Those figures are pure-mineral values, not real ore grades.
- **Bloomery yield.** `advanced-crafting-system-research.md:363-368` claims 60–70%; real yields
  are 10–45% of ore weight. Its salvage pattern (40–60% recovered as scrap) does fit reality.
- **Gems.** `mining-resource-extraction.md` lists generic ruby, sapphire, emerald and diamond not
  tied to host rocks, and its byproduct function is a stub. `material-economy-system.md` treats
  gems as separate nodes.

## Next Steps

- When the metal era unlocks, replace "failure returns inputs" with the table above for smelting,
  and use waste rock and chance finds for mining.
- Split smelting into smelt → bloom and consolidate → bar, so yield losses are visible.
- The stone-age era reuses the same pattern: knapping waste (flakes and debitage) and chance finds
  while gathering stones.
