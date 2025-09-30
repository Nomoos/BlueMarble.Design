# One-Page Design — Island Start (Open-world Medieval Simulation MMO)

Title: Island of Hands (working)

Elevator pitch:
A top-down, web & mobile-first medieval open-world simulation MMO where players farm, hunt, mine, trade and manage labor. Skills are mostly hidden and change gradually depending on concrete activities and materials (e.g., mining coal vs. rock vs. dirt). Players can assign earned experience from a limited XP pool to skills, hire other players or NPC workers via a work-auction, and shape an island economy through production, transport and trade.

Core pillars:
- Simulation: resources, production chains and tool durability matter.
- Emergent economy: player-driven markets, auctions for work, and transport logistics.
- Skill specialization via activity-specific hidden stats that change with each job/material.
- Social MMO: hire, trade, form small groups, transport goods across islands.

Platform & audience:
- Web (WebGL / WebRTC) and Mobile (iOS/Android) first, later native PC.
- Audience: players who like slow-burn simulation, economic systems, and social gameplay (target 16–40).

Camera & view:
- Top-down/isometric (60–90°) with zoom. Prioritize clarity of tiles/resources at small screen sizes.

Core gameplay loop (island start):
- Explore island → gather resources (hunt/collect/mine/dig) → process or transport goods → sell or post jobs on work-auction → assign XP from pool or hire workers → upgrade tools/structures → expand production/market reach.

Key mechanics
- Hidden stats (player profile):
  - Example stats: MiningCoal, MiningRock, DiggingDirt, Hunting, Transport, Farming, Crafting.
  - Stats change only when performing related actions and are mostly hidden (players infer by performance).
- XP Pool:
  - Players accumulate a single XP pool from activities.
  - The pool holds up to Pmax points (example Pmax=1000). At any time the player can allocate points to skills.
  - If the pool exceeds Pmax, new XP enters a bounded queue; when full, oldest queued knowledge (unallocated XP) is dropped or replaced by newest depending on chosen policy.
  - Option: automatic aging — if pool not used for N days, oldest XPs decay faster.
- Skill gain rules (example formula):
  - XP_gain = base_activity_xp * material_modifier * tool_efficiency * (1 - skill_level / skill_cap)
  - Material modifiers: Coal=1.2, Rock=1.0, Dirt=0.6 (example)
  - Tool efficiency scales speed and durability.
- Knowledge replacement policy:
  - FIFO queue of "unspent XP entries" with timestamps. When full, new entries replace oldest entry. Players can spend entries to bump skill levels. This enables players to commit to new knowledge choices intentionally.
- Hiring & work-auction:
  - Players or NPCs post job with requirements, duration and pay (coins or goods).
  - Workers have their own skill levels; higher skill = faster, higher quality, less tool wear.
  - Hiring someone consumes time and pay; employer avoids skill wear but gains outcomes.
- Tools & durability:
  - Tools wear with use; better tools cost more and may provide XP multipliers.
  - Repairs and material quality factor into long-term economic sinks.
- Transport & logistics:
  - Ships and carts require crew, have capacity and fuel/time costs.
  - Transport routes create income streams and logistics gameplay.
- Trading & economy:
  - Local market + island-wide auction board. Price discovery driven by supply/demand.
  - Currency and trade limits; initial economy closed to avoid runaway trade inflation.
- Persistence & progression:
  - Island-level structures (mills, quarries) that players upgrade through pooled resources.
  - Player progression is slow — specialization matters; allow respec costs and hard caps.

MVP scope (first 2-year focus; island start)
- Single island shard with a single authoritative server instance.
- Player entity with inventory, top-down movement, basic UI.
- Core activities:
  - Hunting (animals spawn & drop resources)
  - Gathering (forage)
  - Mining/Digging (rock, coal, dirt) with material differences
- XP pool mechanics + visible queue UI for allocation (simple)
- One basic market/auction board for hiring and commodity trading
- One hireable NPC worker prototype (AI) and hiring flow
- Tool system (durability, tiers)
- Basic ship/cargo prototype (local transport)
- Simple persistence (cloud save per account)
- Basic analytics: level attempts, job completions, resource flows

Technical & architecture suggestions
- Engine: Unity (WebGL + mobile) or Godot (lighter Web export). Unity recommended for tooling & mobile parity.
- Network: authoritative server for simulation. Start single-shard authoritative server that simulates island state.
- Server tech: Node.js + TypeScript or Go for server logic; use WebSockets (or WebRTC data) for real-time sync. Consider an MMO backend (Nakama, PlayFab, Photon, Colyseus) to reduce build time.
- DB & persistence: PostgreSQL for canonical data, Redis for ephemeral state, object storage for large assets.
- Scaling approach: shard-by-island or subzone servers; keep the first island single-shard to simplify.
- Hosting: cloud (GCP/AWS). Use dockerized services and CI/CD pipelines.
- Analytics: event based telemetry (job posted, job accepted, xp gained, item crafted, coin transfers).

Economy & balancing notes
- Introduce strong sinks (repair, taxes, construction) to avoid inflation.
- Limit stack sizes and transport throughput early.
- Cap skill and define diminishing returns to force specialization.

Safety & MMO considerations
- Authoritative simulation server to prevent client cheating.
- Commence soft PvP policy: for first 2-year island start, make PvP limited or consensual to avoid griefing.
- Moderation tools, player reporting and rate limits on auctions/trades.

Monetization (live service)
- Cosmetic shop, convenience subscriptions (extra storage), vanity ships/houses.
- No pay-to-win core progression. Paid DLC islands or expansions later.

Milestones (first 12 months focused on the 2-year island plan)
- Month 0–2: Prototyping (single-player local sim)
  - Deliverables: player movement, harvesting, mining, XP pool mechanics, small UI.
- Month 3–5: Server prototype + persistence
  - Deliverables: authoritative server for island, simple market & job posting, one hireable NPC.
- Month 6–8: Economy & tools
  - Deliverables: trading UI, tool durability, transport prototype (small boats), initial balancing.
- Month 9–12: Beta island shard & playtest
  - Deliverables: cloud deploy of single island shard, telemetry, player onboarding, repeated playtests.

Success metrics (early)
- New-player retention (day1 > 30%, day7 > 10% target for the genre)
- Average time-to-complete-first-job < 30 minutes
- Auction fulfill rate > 60% for posted jobs in early economy
- Resource sink usage (repairs/construction) > 10% of currency flux

Risks & mitigations
- Economy runaway: start closed economy test environment, tune sinks.
- Cheating / bots: authoritative server and bot detection; early anti-exploit telemetry.
- Scope creep: strict MVP cut and progressive feature gating.

Open questions (need answers to proceed)
1. PvP: fully enabled, consensual, or purely cooperative curation for the island start?
2. Expected concurrent users at launch (to plan shards & hosting)?
3. Monetization strategy preference now (F2P with cosmetics, paid app, subscription)?
4. Team size and tech skill focus (Unity/JS/Go)?
5. Do you want player characters to be permanent (permadeath) or soft-respawn?