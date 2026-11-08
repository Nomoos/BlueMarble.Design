---
title: Discovery-Driven Gathering, Hunting and Adversity Skills in Games
date: 2026-11-08
owner: @Nomoos
status: complete
tags: [gathering, hunting, discovery, specialization, pvp, skills, tech-tree]
---

# Discovery-Driven Gathering, Hunting and Adversity Skills in Games

## Problem / Context

BlueMarble's first era limits map actions to **travel, gather and hunt**. The designer's ideas:

- Finding a stone lets the player choose "gather stones/minerals". What they keep finding steers
  their specialization (flint leads to knapping, ochre to pigments). Herbs, plants and fruits work
  the same way.
- Hunting starts generic. Later the player chooses what to hunt.
- Later still: hunting other hunters and gatherers, ambush and robbery. A player who is robbed
  and survives unlocks protection and guarding, so the skill tree grows from experience.

This note collects how existing games handle discovery, random finds, hunting, learning from
adversity, PvP robbery and era gates.

## Key Findings

- **Discovery as a ledger works** (Haven & Hearth): the first time you produce an item type
  yourself, you learn it and can use that item class in recipes.
- **Finds work as published loot tables** (Vintage Story panning). "Nothing" is the most common
  result, the common useful material comes at 10–20%, and rare finds at 0.1–1%, varying by terrain.
- **Announce what can't yet be identified** (Wurm Lore, UnReal World Herblore). The player sees
  "something you can't yet recognise" instead of the find being hidden.
- **Hunting resolves to a species**, with a quality chain (animal × weapon fit × clean kill) and
  soft caps by tool and skill.
- **Defence trains from being hit**, scaled by attacker strength (Kenshi). PvP robbery is kept
  in check by zone tiers, loss caps and reputation (Albion, EVE, Haven & Hearth scents).
- **The best era gates are evidence of doing**, not a shop (Ancestors, Dawn of Man milestones,
  Civilization VI Eurekas).

## Evidence

### Discovery

- **Haven & Hearth.** Each item type (every stone, berry and fish) can be discovered, for about
  50–100 Learning Points. A discovery also unlocks that item class for recipes: a fireplace needs
  stones, so you must have discovered a stone first. You only discover items you produced
  yourself, but another player can "teach" you one. Curiosities are studied passively and turn
  time into LP ([Discovery](https://ringofbrodgar.com/wiki/Discovery),
  [Curiosities](https://ringofbrodgar.com/wiki/Curiosities)).
- **Vintage Story.** There is no tree; what you find decides what you can make. Only some rocks
  can be knapped (andesite, basalt, chert, flint, granite, obsidian, peridotite), ranked by
  strength: obsidian > flint > other stone ([Knapping](https://wiki.vintagestory.at/Knapping)).
  Panning one block of gravel gives: 62% nothing, 20% loose stone, 15% copper nugget, 11% flint,
  3% quartz, 0.4% gold, 0.2% lapis. Bony soil near ruins has its own table (1% emerald, peridot or
  gold) ([Panning](https://wiki.vintagestory.at/Panning)).
- **Wurm Online.** Botanizing and foraging work on resource nodes. The "Lore" action lists possible
  finds by likelihood and says when something is present that you can't yet identify. Skill
  raises speed, quality, quantity and rarity ([Botanizing](https://www.wurmpedia.com/index.php/Botanizing)).
- **UnReal World.** Pure learn-by-doing:
  - failed attempts teach, cancelled ones do not;
  - harder tasks teach more;
  - daily gains are capped;
  - effective skill is clamped to 5–95%;
  - Herblore is needed to identify herbs ([Skills](https://www.unrealworld.fi/wiki/index.php/Skills)).
- **Ancestors: The Humankind Odyssey.** About 300 "neurons" stay dark until you do the related
  action. Evolution leaps make them permanent ([Neurons](https://ancestors.fandom.com/wiki/Neurons)).
- **Dawn of Man.** Knowledge Points come from milestones (your first and fifth skin dryer). The
  Stone Age is split into Paleolithic, Mesolithic and Neolithic ([TheGamer](https://www.thegamer.com/dawn-of-man-beginners-guide/)).
- **Eco.** Specialties cost escalating points (0, 0, 5, 15, 50, 100…) and come from research
  books ([Specialty](https://wiki.play.eco/en/Specialty)).

### Hunting

- **UnReal World.**
  - Tracking gives a passive chance to find trails while travelling. Prints depend on ground and
    animal weight ([Tracking](https://www.unrealworld.fi/wiki/index.php/Tracking)).
  - Trapping skill raises catch chance and spawn rate near traps. Terrain and bait matter
    ([Trapping](https://www.unrealworld.fi/wiki/index.php/Trapping)).
- **Red Dead Redemption 2.** Pelt grade = animal grade (1–3 stars) × fit of weapon and ammo ×
  clean kill ([guide](https://www.powerpyx.com/red-dead-redemption-2-how-to-get-perfect-pelts/)).
- **Haven & Hearth.** Prey behaves differently by species: it flees, enrages or poisons. Hide
  quality is soft-capped by tool quality, then by Survival skill (each cap averages toward the
  lower value) ([Hunting](https://ringofbrodgar.com/wiki/Hunting)).

### Learning from adversity, and PvP robbery

- **Kenshi Toughness** trains from damage taken, scaled by attacker strength: 0.1× for hits at or
  below your level up to about 6× for hits 50 points above. Theft that is seen creates a bounty
  that fades unless the thief is notorious ([Toughness](https://kenshi.fandom.com/wiki/Toughness)).
  These figures come from search summaries.
- **Haven & Hearth scents.** Crimes leave scents on the spot: theft lasts 8 game days. Finding a
  scent is the observer's Perception + Exploration against the culprit's Intelligence + Stealth
  ([Scents](https://ringofbrodgar.com/wiki/Scents)).
- **Albion Online zones.** Blue zones have no PvP. Yellow zones allow knockdown without loot. Red
  zones allow full loot at a reputation cost. Black zones are lawless
  ([zones](https://www.albionmarket.gg/guides/zones-and-flagging)).
- **EVE Online.** An unprovoked attack costs security status scaled by the system's security.
  Below −5 a player becomes an outlaw ([Security status](https://wiki.eveuniversity.org/Security_Status)).

### Era gates

- **Civilization VI.** "Eurekas" pre-pay about 40% of a tech when the player does a related action.
- **Empire Earth.** Epoch advances cost resources and buildings.
- **Ancestors and Haven & Hearth.** The tree is a map of what the player has done.

## Implications for Design

1. **Discovery ledger instead of buying unlocks.**
   - The first own find of a material type records a discovery: a small proficiency bonus and
     permission to use that class in recipes.
   - Discoveries can be taught by another player at the same place.
2. **Each place has a terrain-based find table**: about 60% nothing, 15–20% common stone, about
   10% flint where flint exists, and 0.2–1% ochre or semi-precious stones.
   - Rolls are seeded from world seed, place, time and actor, so world ticks stay deterministic.
3. **A first find opens a focus, not a lock.** "Gather: stones" makes mineral finds about twice
   as likely and others less likely. Unfocused gathering stays available.
4. **Announce finds before they are identified.** "Unknown reddish earth" stays unknown until the
   parent proficiency passes a threshold or the item has been handled N times.
   - Generic uses (any stone works for a hearth) are available at once.
5. **Generic hunting resolves to a species.** After a few encounters, "Hunt: hare" appears, with
   species-specific risk.
   - Hunt quality = animal × tool fit × kill quality, soft-capped by butchering skill.
6. **Guard skill grows from surviving attacks, scaled by attacker strength.** Daily caps and
   diminishing returns for repeat attackers prevent farming.
7. **Places carry risk tiers.**
   - Camp: safe.
   - Surroundings: robbery capped at a share of carried goods.
   - Frontier: full robbery.
   - Robbery lowers reputation more the safer the place was, and repeat robbers become outlaws.
8. **Robberies leave tracks** that tracking can read: the victim's or guard's tracking against the
   robber's stealth. One tracking skill then serves hunters, guards and robbers.
9. **The tree is drawn as a map of knowledge.** Known nodes are shown in full, neighbouring ones as
   silhouettes, later eras locked. Era gates unlock from milestones, never from a currency.

## Next Steps

- Tune find-table percentages per BlueMarble terrain type with the geology team.
- Decide how robbery works before real multiplayer: bot raiders in the MVP, players later.
- Prototype the "unknown find" identification thresholds.
