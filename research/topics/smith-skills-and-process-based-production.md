---
title: Smith Skills and Process-Based Production
date: 2026-11-08
owner: @Nomoos
status: complete
tags: [smithing, specialization, skills, production, crafting, learn-by-doing]
---

# Smith Skills and Process-Based Production

## Problem / Context

Game crafting (Minecraft, Terraria, Valheim, Factorio) usually maps inputs to outputs with a fixed
recipe. Real production is a chain of operations with skill, variance, waste, tools and division
of labour. BlueMarble learns by doing: activities train weighted sets of proficiencies. This note
asks two things. How should production be generalized so that a player's profile emerges from what
they actually make? And, as a test case, what skills would a smith have who works mainly on nails
and agricultural tools?

## Key Findings

- **Specialization has a sourced speed ratio.** Adam Smith (*Wealth of Nations* I.1):
  - a smith who never made nails makes 200–300 a day, "and those too very bad ones";
  - a smith used to nails, though not a nailer, makes 800–1,000;
  - a young man who did nothing else makes "upwards of 2,300".

  That is about **1 : 3.5 : 9** for novice, practised generalist and specialist
  ([Smith](https://www.econlib.org/library/Smith/smWN.html?chapter_num=4)).
- **Skill shows up as speed, quality and yield.** A 60 lb rod bundle gave 52 lb of large nails or
  as little as 42 lb of small ones; "the yield depended upon the skill of the nailer"
  ([Willets, Black Country nail trade](https://sedgleymanor.com/trades/nailmakers2.html)).
- **Real crafts are ordered operations.** A scythe takes more than 20 steps
  ([de: Sense](https://de.wikipedia.org/wiki/Sense_(Werkzeug))). A nail is: point, nick, set in the
  header, snap, head with two blows for a clasp head or four for a rose head
  ([Nova Scotia Museum](https://ojs.library.dal.ca/NSM/article/view/5845)).
- **DIN 8580 gives a ready taxonomy** of operations: primary shaping, forming, separating, joining,
  coating, and changing material properties
  ([de: Fertigungsverfahren](https://de.wikipedia.org/wiki/Fertigungsverfahren)).

## Evidence

### Game crafting vs real production

| Dimension | Minecraft, Terraria, Valheim | Factorio, Satisfactory | Real production |
|---|---|---|---|
| Time | instant | fixed, scaled by machine | many heats per piece |
| Skill | none (Valheim: station level) | none | dominant: about 9× rate, and quality |
| Variance and failure | none | none | bad pieces, failed welds, quench cracks |
| Waste | none | declared byproducts | 13–30% rod loss in nail making |
| Tools | being near a station | machine type | purpose-made jigs (nail header, swages, treadle "Oliver" hammer) |
| Batch economy | none | throughput ratios | up to six nailers shared one hearth to save fuel |
| Division of labour | none | machines | slitting mills made rod, nailers made nails, nailmasters put the work out |
| Maintenance | durability, then replace | none | peening scythes, re-laying plough shares |

### DIN 8580 mapped to smithing

| Group | Operations |
|---|---|
| Primary shaping | casting, bloom consolidation |
| Forming | drawing, upsetting, bending, fullering, swaging, peening |
| Separating | hot cutting, splitting, punching, grinding, filing |
| Joining | forge welding, steeling, riveting, hafting |
| Coating | oiling, tinning, blacking |
| Changing material properties | hardening, tempering, annealing, work-hardening |

### Historical trades

- **Nailer.** Worked from slit rod (England, slitting mills from 1590), was paid by the "tale"
  (1,200 made counted as 1,000), and used the nail header as the key tool
  ([Slitting mill](https://en.wikipedia.org/wiki/Slitting_mill)).
- **Scythe smith.** Water-powered hammers. Around 1900, about 215 Austrian works made 12 million
  scythes a year, with blades layered from steel and soft iron
  ([UNESCO](https://www.unesco.at/en/culture/intangible-cultural-heritage/inventory/element/article/austrian-scythe-forging)).
- **Edge-tool maker.** Finch Foundry made about 400 agricultural and mining tools a day at its peak
  ([Finch Foundry](https://en.wikipedia.org/wiki/Finch_Foundry)).
- **Farrier** (blacksmithing plus hoof anatomy), **whitesmith** (finishing work), and the **village
  smith**, who re-laid plough shares by welding a fluxed steel edge on in sections
  ([Small Farmer's Journal](https://smallfarmersjournal.com/forging-a-plow-share/)).

### Nail specialist vs agricultural-tool specialist

H = high, M = medium, L = low, – = none. The number is a suggested weight for the game (0–1).

| Technique or knowledge | Nails | Farm tools | Note |
|---|---|---|---|
| Fire management, short heats | H 0.8 | H 0.8 | the nailer keeps rods cycling through one fire |
| Reading heat colours | M 0.4 | H 0.9 | the welding heat must be equal on both surfaces ([Forge welding](https://en.wikipedia.org/wiki/Forge_welding)) |
| Drawing out, pointing | H 0.9 | H 0.8 | |
| Upsetting, heading | H 1.0 | L 0.3 | heading in the nail header is upsetting |
| Hot cutting | H 0.7 | M 0.4 | |
| Punching, drifting | L 0.1 | H 0.7 | tool eyes, share bolt holes, horseshoes |
| Bending, forming | L 0.1 | H 0.8 | sickle curve, hoe neck |
| Fullering, swaging | L 0.2 | M 0.5 | |
| Splitting | – | M 0.4 | fork tines |
| Forge welding | – 0.05 | H 0.9 | steel edges on iron tools |
| Steeling (laid steel) | – | H 1.0 | shares, axes, hoes |
| Hardening, quenching | – | H 0.8 | nails stay soft wrought iron |
| Tempering by colour | – | H 0.8 | purple 282 °C, light blue 337 °C ([Tempering](https://en.wikipedia.org/wiki/Tempering_(metallurgy))) |
| Grinding, sharpening | – | M 0.6 | |
| Cold peening | – | M 0.5 | removes nicks about 5 mm deep |
| Repair | L 0.1 | H 0.8 | |
| Making own tools and jigs | H 0.6 | M 0.5 | |
| Repetition and stamina | 1.0 | M 0.4 | |
| Materials (iron vs steel) | M 0.4 | H 0.9 | |
| Customer and agricultural knowledge | L 0.1 | H 0.7 | scythe forms were made for specific markets |
| Hafting, handles | – | M 0.4 | wood |

Some judgements are our own, not sourced: "nails stay soft" and every weight in the table.

## Implications for Design

1. **Process plans instead of fixed recipes.** An operation is: group (DIN), technique proficiency,
   inputs, tool and station, heats, and outputs (product, byproducts, scrap on failure). A plan is
   an ordered list of operations, and alternative plans are allowed.
2. **Three skill axes per operation:**
   - the product family (activity) drives speed through routine;
   - the technique drives success and quality;
   - the material drives yield and waste.

   Calibrate speed on 1 : 3.5 : 9.
3. **Model mass yield and scrap.** Waste runs from about 30% for a novice to about 13% for an
   expert, and scrap returns as an input.
4. **Heats cost fuel.** Batching and shared hearths lower the fuel per unit, which is why nails are
   made in bulk.
5. **A routine bonus** builds up while repeating one product family and decays when switching
   (Smith names the time lost between tasks).
6. **Repair is first-class.** Peening, re-steeling and re-laying create steady local demand.
7. **Intermediates and jigs are tradeable.** Rod iron, steel, a nail header whose quality affects
   output, and putting-out contracts give carriers and markets real goods.

### Suggested skill graph for the metal era

**New proficiencies**
- Techniques: drawing, upsetting, cutting, punching, bending, forgeWelding, steeling, hardening,
  tempering, grinding, peening.
- Material: steel.
- Product families under smithing: nailmaking, edgeTools, farriery, repair.

**Example activity weights** (the activity itself at 1.0)

| Activity | Weights |
|---|---|
| Make nails (batch) | nailmaking 1.0, upsetting 0.7, drawing 0.5, cutting 0.4, smithing 0.4, firecraft 0.4, iron 0.6 |
| Forge sickle | edgeTools 1.0, drawing 0.7, bending 0.5, hardening 0.5, tempering 0.5, grinding 0.3, steel 0.6 |
| Steel a hoe edge | steeling 1.0, forgeWelding 0.8, firecraft 0.7, hardening 0.4, tempering 0.4, steel 0.6, iron 0.5 |
| Repair a scythe | repair 1.0, peening 0.8, grinding 0.4, edgeTools 0.4, steel 0.4 |
| Forge horseshoe | farriery 1.0, bending 0.7, punching 0.6, drawing 0.4, iron 0.6 |

**Relations for spill**
- forgeWelding ↔ steeling 1.0
- hardening ↔ tempering 1.0
- forgeWelding ↔ firecraft 0.8
- upsetting ↔ nailmaking 0.8
- iron ↔ steel 0.5
- edgeTools ↔ nailmaking 0.2 (kept weak on purpose, so the two specialisations diverge)

## Next Steps

- This applies to the locked metal era. The stone-age era uses the same operation model for
  knapping: percussion, pressure flaking, retouch, hafting.
- Validate the weights by simulating a player who only makes nails against one who only makes
  farm tools.
