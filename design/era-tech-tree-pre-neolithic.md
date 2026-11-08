# Era Tech Tree: Before the Neolithic

**Version:** 0.1
**Date:** 2026-11-08
**Owner:** @Nomoos
**Status:** Draft
**Related:** [Pre-Neolithic eras and revolutions](../research/topics/pre-neolithic-eras-and-revolutions.md),
[Discovery-driven gathering and hunting](../research/topics/discovery-driven-gathering-and-hunting.md),
[Crafting quality and failure in games](../research/topics/crafting-quality-and-failure-in-games.md),
[Realistic smelting and mining outcomes](../research/topics/realistic-smelting-and-mining-outcomes.md),
[Smith skills and process-based production](../research/topics/smith-skills-and-process-based-production.md),
[Island start](island_start_game_design.md)

---

## Summary

The tech tree splits human history into eras. A **threshold** ("revolution") separates each era
from the next:

> Hunter-gatherers → (Neolithic revolution) → Agrarian society → (Industrial revolution) → Industrial age

The first playable eras come **before** the Neolithic revolution and start with the oldest
documented technology.

**Map actions** are limited to three: **travel, gather, hunt**.

**Specialization** grows out of what the player finds and does:
- A found stone opens stone gathering.
- Flint found again and again leads to knapping.
- Ochre leads to pigments.
- Herbs, plants and fruits work the same way.

**Hunting** starts generic and narrows to species.

**Conflict between people** comes later: hunting hunters and gatherers, ambush and robbery.
Surviving an attack trains vigilance, which opens guarding.

**Later eras are locked.** That includes the BlueMarble MVP's current content (iron smelting,
smithing, ships with fares, markets with copper coins). The game shows them as locked, so players
see where the tree leads.

## Design Principles

1. **Learn by doing, not buying.** There is no XP pool and no shop of nodes. Proficiencies grow
   with use and spill 15% to related ones, as in the MVP.
2. **Gates are evidence.** An era opens when the player has *shown* its threshold (made the first
   knapped flake, kept a hearth going, made fire). Wording follows the research: thresholds, not
   single events.
3. **The tree is a map of knowledge.**
   - Known nodes are shown in full.
   - Neighbouring nodes appear as silhouettes.
   - Later eras appear behind a lock.
4. **Geology drives finds.** What a place yields depends on its terrain and rock, as elsewhere in
   BlueMarble.
5. **Deterministic chance.** Every roll is a hash of world seed, place, time and actor, so the
   world evaluation on the 5-minute tick stays reproducible.

## Eras

| # | Era (Czech) | Dates | Threshold into the next era | Status |
|---|---|---|---|---|
| 0 | Forerunners (*Předchůdci*), prologue | 7–3.3 Ma | First knapped flake | **Inferred, not documented.** Shown as such |
| 1 | Earliest Paleolithic (*nejstarší paleolit*, Oldowan) | 3.3 Ma – 600 ka | Shaped hand axe | Playable first |
| 2 | Lower Paleolithic (*starý paleolit*, Acheulean) | 600–250 ka | Prepared core and hafted tool; making fire | Next |
| 3 | Middle Paleolithic (*střední paleolit*) | 250–40 ka | Blades, bone tools, needle, spear-thrower | Planned |
| 4 | Upper Paleolithic (*mladý paleolit*, Pavlovian) | 40–14 ka | Bow and microliths | Planned |
| 5 | Late Paleolithic and Mesolithic (*pozdní paleolit, mezolit*) | 14 ka – 5500 BC (CZ) | **Neolithic revolution** | Planned |
| later | Agrarian society, metals, iron age (current MVP content) | – | – | **Locked** |

### How locks work

The proposal behind this design compares three ways of locking eras. BlueMarble combines two of them:

- **B: resource barrier.** Later-era resources are not placed in the early-era world, so the game
  never asks the player for something they can't have. For example, ore deposits are not offered
  as a gather action.
- **C: UI lock.** Later eras are drawn with a lock and an "In a later era" label.

Option A, a hard code check against the game version, is not needed because gates are milestones.

## Actions

### Travel

- Walking and climbing between places in the world graph, as in the MVP.
- Swimming across straits once the player can swim.
- No rafts before era 4. The research marks early seafaring as speculative.
- Travel gives a passive chance to notice tracks, which feeds tracking.

### Gather

1. **By hand.** Every place has a **find table** built from its terrain. Rough shares:

   | Find | Share |
   |---|---|
   | Nothing | 50–60% |
   | Common items: fruits, nuts, roots, eggs, shellfish, cobbles | 15–20% |
   | Flint, where the rock has it | ~10% |
   | Ochre, pyrite, quartz crystals, jasper or agate, by host rock | 0.2–1% |

2. **Discovery.** The first own find of an item type is recorded in the **discovery ledger**. The
   journal notes it, and related proficiencies get a small bonus.
3. **Focus.** A discovery opens a focused action: "Gather stones", "Gather herbs" or "Gather fruit
   and nuts". A focus about doubles the chance for its category and halves the others. Unfocused
   gathering stays available.
4. **Deeper focus.** Repeated finds open narrower focuses. "Look for flint" appears after flint has
   been found three times, and "dig for ochre" after ochre has been found.
5. **Unknown finds.** Low proficiency shows a find as unknown ("unknown reddish stone"). It becomes
   identified when the material's proficiency passes a threshold or after handling it several
   times. Generic uses work at once: any stone can serve as a hammerstone.
6. **"What can be found here".** The place panel lists known finds by likelihood, plus "something
   you can't recognise yet" while unknowns remain (Wurm Lore).

### Hunt

1. **Era 0:** scavenging carcasses and catching small animals by hand.
2. **Generic "Hunt"** draws an encounter from the place's fauna table.
3. **After a few encounters** with a species, "Hunt: hare" or "Hunt: deer" appears. Encounters with
   that species get more likely, and the risk depends on the species (a boar injures, a hare
   escapes).
4. **Yields:** meat, hide, bone and sinew. Quality = animal × tool fit × kill quality,
   soft-capped by butchering proficiency.
5. **Injuries** slow actions for a while. No permanent death in the first eras.

### First processing: knapping (gate 0 → 1)

- Needs a **hammerstone** (any cobble) and a **core** of a knappable rock (flint, chert, quartzite).
- Gives flakes and choppers, which are stone tools that wear.
- Knapping waste (debitage) stays at the place, and other players can find it later.
- The first flake the player makes opens era 1.

## Conflict: Ambush, Robbery, Guarding

This is a prototype. Bots attack in the MVP; players attacking players comes later.

- **Risk tiers on the place graph:**

  | Place | Robbery |
  |---|---|
  | Camp | Safe |
  | Surroundings | Capped at a share of carried goods; tools in hand are kept |
  | Frontier | Full robbery |

- **Robbers.** Raider bands, which are bots in the MVP, roam the frontier. A robbery takes a share
  of carried goods.
- **Vigilance.** Surviving an attack trains *vigilance*, scaled by the attacker's strength
  (0.1× up to about 6×, after Kenshi). Daily caps and diminishing returns per attacker prevent
  farming.
- **Guarding.** Vigilance opens **Guard**: watching a place or a companion makes robbery there less
  likely and trains vigilance.
- **Tracks.** A robbery leaves tracks on the place for a while. Reading them is a roll of the
  reader's tracking against the robber's stealth. The same tracking serves hunters, guards and
  robbers.
- **Robbing players.** When players can rob each other, robbery lowers reputation more the safer
  the place. Repeat robbers become outlaws whom anyone may attack.

## Proficiencies Added for the Early Eras

| Group | Proficiencies |
|---|---|
| Activities | gathering, foraging (plants), stone gathering, hunting, scavenging, tracking, knapping, butchering, vigilance |
| Materials | fruit and nuts, roots, herbs, stone (cobbles), flint, quartz, ochre, meat, hide, bone |
| Movement | walking, climbing, swimming (existing) |

**Relations for spill:**
- stone gathering ↔ knapping
- flint ↔ knapping
- hunting ↔ tracking ↔ vigilance
- butchering ↔ hunting
- foraging ↔ herbs ↔ roots

## What Happens to the Current MVP Content

The current content stays in code as the **iron** scenario: smelting, smithing, carriers, markets,
copper money, quality grades and the crafting card. New games start in the **pre-Neolithic**
scenario. Quality grades, the crafting card and learn-by-doing carry over to stone tools.

Research for when the metal era unlocks:
- **Failed smelting** gives slag, spongy blooms or cast-iron lumps instead of returning the inputs.
- **Mining without ore** brings up waste rock, gangue and chance finds tied to the host rock.
- **Smithing becomes process plans.** Nailmaking and edge-tool work diverge into different skill
  profiles.

## MVP Roadmap

| Step | Content |
|---|---|
| 12 | Eras and the locked later era: game state carries the era, two scenarios, an era timeline in the UI |
| 13 | Gathering and finds: find tables per terrain, discovery ledger, new pack categories |
| 14 | Specialization by finds: focused gathering, unknown finds, "what can be found here" |
| 15 | Hunting: scavenging, fauna tables, species-specific hunting, injuries, yields with quality |
| 16 | First gate: knapping stone; stone tools with wear; era 1 opens |
| 17 | Ambush and guarding prototype: risk tiers, raider bots, vigilance, guarding, tracks |

## Open Questions

- Should the prologue's hominin be named (Australopithecus) or kept abstract?
- Does food and hunger matter in the first eras, or are gathered goods only material for
  discoveries and tools?
- Death in the first eras: injury only, or respawn with losses?
- Can players teach each other discoveries at the same place (Haven & Hearth "teach")?
- How do the island-start design's medieval assumptions (XP pool, medieval lore) relate to a
  stone-age start? Is it a separate scenario, or a prologue to the same world?
