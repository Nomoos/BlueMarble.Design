# Social Interaction Gameplay Mechanics

**Version:** 1.0  
**Date:** 2025-10-06  
**Author:** BlueMarble Design Team  
**Status:** Implemented

## Overview

This document describes the player-facing gameplay mechanics for the Social Interaction and Settlement Management System. It focuses on how players interact with settlements, diplomacy, and federations during normal gameplay.

## Settlement Gameplay

### Establishing Your First Settlement

#### Prerequisites
- Player level 10 or higher
- Required influence: Political 10, Economic 5, Military 5
- Resources: 500 gold, 100 wood, 50 stone
- Suitable location (flat terrain, near water preferred)

#### Step-by-Step Process
1. Open world map and switch to "Settlement View"
2. Use influence scanner to find suitable locations (highlighted green)
3. Click desired location to preview settlement placement
4. Review influence requirements and costs
5. Name your settlement
6. Confirm establishment

#### Tips for New Settlement Founders
- Start with a Village (lowest requirements)
- Choose locations near resource nodes for economic bonuses
- Water access provides +10% population growth
- Trade route proximity increases economic output
- Consider defensive position for military stability

### Managing Your Settlement

#### Daily Operations
Players can interact with their settlements through the Settlement Management UI:

**Population Tab**
- View current population and happiness
- Monitor growth rate and capacity
- Manage tax rates (affects happiness vs. income)
- Review population needs (food, housing, employment)

**Economy Tab**
- Track daily economic output
- View tax income
- Monitor resource production
- Manage trade agreements
- Set market policies

**Infrastructure Tab**
- Construct buildings (farms, markets, workshops)
- Build defenses (walls, towers, barracks)
- Upgrade existing structures
- Queue construction projects

**Influence Tab**
- Monitor your influence levels
- View influence requirements for current settlement type
- Track influence gains/losses
- Review active modifiers

#### Settlement Upgrades
As your influence grows, you can upgrade settlements:

**Village → Town**
- Requirements: Political 25, Economic 20, Military 15
- Cost: 5,000 gold, 500 wood, 300 stone
- Upgrade time: 7 days
- Benefits: Higher population cap (2,000), increased tax income, unlock town-specific buildings

**Town → City**
- Requirements: Political 50, Economic 40, Military 30
- Cost: 25,000 gold, 2,000 wood, 1,500 stone
- Upgrade time: 14 days
- Benefits: Maximum population cap (10,000), major trade hub bonuses, city-exclusive buildings

### Settlement Types and Strategies

#### Village Strategy
- Focus: Rapid establishment and basic resource production
- Best for: New players, resource gathering, population growth
- Key buildings: Farms, logging camps, basic housing
- Defensive approach: Minimal defense, rely on allies

#### Town Strategy
- Focus: Balanced growth and trade development
- Best for: Mid-game expansion, trade networks, regional influence
- Key buildings: Markets, workshops, upgraded housing, walls
- Defensive approach: Moderate defense, capable of repelling small attacks

#### City Strategy
- Focus: Maximum economic and political power
- Best for: Late game dominance, federation headquarters, trade empires
- Key buildings: Grand marketplace, guildhalls, cathedral, fortifications
- Defensive approach: Strong defenses, can withstand sieges

#### Outpost Strategy
- Focus: Strategic military positioning
- Best for: Border defense, forward operations, resource claim protection
- Key buildings: Barracks, armory, watchtowers, minimal housing
- Defensive approach: Maximum defense-to-population ratio

#### Trading Post Strategy
- Focus: Pure economic specialization
- Best for: Trade route control, merchant guilds, economic warfare
- Key buildings: Merchant quarters, auction house, bank, warehouses
- Defensive approach: Hire mercenary protection, rely on economic power

## Influence Mechanics

### Understanding Influence

Influence represents your ability to control and maintain settlements. There are three types:

#### Political Influence
**How to Gain:**
- Complete diplomatic quests (+2-5 per quest)
- Form alliances (+10 per alliance)
- Maintain settlements (+1 per settlement per day)
- Win elections in democratic federations (+15)
- Negotiate successful peace treaties (+5-10)
- Host diplomatic events (+3)

**How to Lose:**
- Break diplomatic agreements (-15)
- Lose settlements (-5 per settlement)
- War declarations (-10 if you start the war)
- Failed diplomatic missions (-3)
- Natural decay (1% per day if inactive)

#### Economic Influence
**How to Gain:**
- Control trading posts (+5 per post)
- High trade volume (+0.01 per 1000 gold traded)
- Successful market manipulation (+5)
- Resource monopolies (+10 per resource)
- Tax collection (+0.1 per 100 gold collected)
- Complete economic quests (+3-8)

**How to Lose:**
- Trade embargoes (-5 per embargo)
- Economic sanctions (-10)
- Market crashes affecting your goods (-5)
- Failed investments (-3)
- Natural decay (1% per day if inactive)

#### Military Influence
**How to Gain:**
- Win PvP battles (+1 per victory)
- Successfully defend settlements (+5 per defense)
- Capture settlements (+10 per capture)
- Defeat world bosses with military forces (+3)
- Train large armies (+0.1 per soldier trained)
- Complete military quests (+3-8)

**How to Lose:**
- Lose PvP battles (-1 per loss)
- Failed settlement defenses (-10)
- Army desertion due to low morale (-5)
- Surrender in wars (-15)
- Natural decay (1% per day if inactive)

### Influence Strategies

#### Diplomatic Path
- Focus on political influence
- Form many alliances
- Avoid conflicts
- Mediate disputes for influence
- Best for: Players who prefer negotiation over combat

#### Economic Path
- Focus on economic influence
- Control trade routes
- Establish trading posts
- Dominate markets
- Best for: Players who enjoy trading and market gameplay

#### Military Path
- Focus on military influence
- Capture settlements
- Win battles
- Build strong defenses
- Best for: Players who prefer combat and conquest

#### Balanced Path
- Maintain all three influence types
- Most flexible approach
- Can adapt to situations
- Best for: Versatile players and leaders

## Diplomacy Gameplay

### Diplomatic Relationships

#### Establishing Relationships
When you first interact with another player or guild diplomatically, a relationship is automatically created at Neutral (0 value). You can then work to improve or worsen this relationship.

#### Relationship Progression

**Neutral → Friendly (+25)**
- Trade regularly with the entity (natural progression)
- Complete joint quests together
- Exchange gifts or resources
- Support each other in territorial disputes
- Time: ~2-4 weeks of regular interaction

**Friendly → Allied (+50)**
- Propose formal alliance (costs 1,000 gold)
- Sign mutual defense pact
- Establish trade bonuses
- Commit to supporting each other's settlements
- Time: ~1-2 weeks after reaching Friendly

**Neutral → Rival (-25)**
- Compete for same resources
- Challenge each other's settlements
- Undercut each other in markets
- Time: Natural progression through competition

**Rival → Hostile (-50)**
- Aggressive territorial challenges
- Trade embargoes
- Support each other's enemies
- Minor skirmishes
- Time: ~1-2 weeks of continued rivalry

**Hostile → War (-75)**
- Formal war declaration (costs 5,000 gold)
- Must have valid casus belli (war justification)
- Full PvP enabled between factions
- Settlement capture allowed
- Time: Instant upon declaration

### Diplomatic Actions

#### Alliance Proposals
**Requirements:**
- Relationship must be Friendly or better
- Proposer must have 30+ Political influence
- Cannot be at war with ally's allies

**Process:**
1. Open diplomacy UI
2. Select target entity
3. Click "Propose Alliance"
4. Set alliance terms (mutual defense, trade bonuses, etc.)
5. Wait for response (7-day expiration)

**Benefits:**
- -25% trade costs with ally
- Shared intelligence on threats
- Mutual defense (automatic assistance if attacked)
- Combined influence for joint operations
- Path to federation formation

#### War Declarations
**Requirements:**
- Relationship must be Hostile or Rival
- Must have valid casus belli
- Costs 5,000 gold
- Political influence penalty if unjustified

**Valid Casus Belli:**
- Territory dispute (competing settlement claims)
- Ally defense (ally was attacked)
- Broken agreement (target violated treaty)
- Resource protection (target threatening your resources)
- Religious/Cultural conflict (RP justification)

**Process:**
1. Open diplomacy UI
2. Select target entity
3. Click "Declare War"
4. Select casus belli
5. Write declaration message
6. Confirm (pays cost)

**Consequences:**
- Full PvP enabled in all zones
- Settlement capture allowed
- Trade automatically embargoed
- Allied entities may join the war
- Neutral parties may take sides

#### Peace Treaties
**Requirements:**
- Currently at war
- One party proposes, other accepts
- May require reparations

**Terms:**
1. Ceasefire date
2. Territory changes (if any)
3. Reparations (gold, resources)
4. Future relationship (Neutral, Rival, or path to Friendly)
5. Restrictions (non-aggression period, demilitarized zones)

**Process:**
1. Open war diplomacy screen
2. Propose peace terms
3. Negotiate with other party
4. Both parties accept
5. Treaty becomes active

### Federation Formation

#### Creating a Federation
**Requirements:**
- Minimum 2 entities, all must be Allied
- Founder must have 40+ Political influence
- Foundation cost: 10,000 gold
- All members must approve charter

**Steps:**
1. Ensure all potential members are Allied
2. Open Federation UI
3. Click "Create Federation"
4. Name the federation
5. Set governance model (Democratic, Oligarchic, Autocratic)
6. Define charter terms:
   - Member contribution percentage
   - Voting rules
   - Admission requirements
   - Expulsion rules
7. Invite founding members
8. All members vote to approve
9. Federation established

#### Federation Governance

**Democratic Model:**
- All members have equal vote
- Leadership rotates or elected annually
- Major decisions require majority vote
- Best for: Equal partnerships

**Oligarchic Model:**
- Council of top contributors makes decisions
- Influence determines council membership
- Decisions require council majority
- Best for: Merit-based organizations

**Autocratic Model:**
- Single leader makes all decisions
- Leader is highest influence member
- Fast decision making
- Best for: Strong leadership, wartime, efficiency

#### Federation Benefits

**Collective Influence:**
- Pool influence for federation operations
- Access higher-tier settlements together
- Combined military strength for defense

**Economic Integration:**
- Free internal trade (0% transaction costs)
- Shared treasury for federation projects
- Resource sharing and distribution
- Market stabilization

**Strategic Advantages:**
- Coordinated military operations
- Shared intelligence networks
- Joint infrastructure projects
- Diplomatic weight in negotiations

#### Federation Projects

Federations can undertake large-scale projects:

**Infrastructure Projects:**
- Grand Trade Hub: +30% trade income for all members
- Federation Highway: Faster travel between settlements
- Communication Network: Instant messaging across territory

**Military Projects:**
- Federation Fortress: Massive defensive structure
- Combined Fleet: Shared naval power
- Military Academy: Training bonus for all members

**Economic Projects:**
- Central Bank: Federation-wide economic stability
- Resource Stockpile: Emergency reserves
- Merchant Guild: Trade route dominance

## Territorial Disputes

### When Disputes Occur

Territorial disputes trigger automatically when:
- Two entities have overlapping influence areas
- Combined influence exceeds 150% of control requirements
- Entities are not Allied or have no non-aggression pact

### Dispute Resolution Options

#### Diplomatic Resolution (Recommended)
**Best for:** Neutral or Friendly relationships

**Process:**
1. Receive dispute notification
2. Enter negotiation period (48-168 hours)
3. Propose resolution terms
4. Other party counter-proposes or accepts
5. Mutual agreement ends dispute

**Possible Outcomes:**
- Territory division (split influence zones)
- Exclusive control (one party withdraws)
- Joint control (Allied entities only)
- Neutral zone (both parties share limited control)

**Benefits:**
- No resource loss
- Relationship improvement possible
- Population happiness maintained
- Quick resolution

#### Military Resolution
**Best for:** Hostile or War status

**Process:**
1. Challenge settlement control
2. Begin siege or battle period
3. Military operations determine winner
4. Victor gains control

**Costs:**
- Military resources and casualties
- Population happiness decrease
- Relationship damage (-10 to -25)
- Economic disruption

**Benefits:**
- Clear winner determined
- Complete control established
- Demonstrates military strength

#### Economic Resolution
**Best for:** Trading posts and economic settlements

**Process:**
1. Begin economic competition period (1-4 weeks)
2. Highest economic influence at end wins
3. Loser may maintain minor presence if Allied

**Costs:**
- Economic investment required
- Market focus needed
- Opportunity cost

**Benefits:**
- Non-violent resolution
- Economic relationship established
- Trade agreements often follow

### Dispute Strategy Tips

1. **Prevent disputes:** Coordinate with neighbors before expanding influence
2. **Quick resolution:** Don't let disputes drag on, instability hurts both parties
3. **Consider relationships:** Diplomatic resolution strengthens future cooperation
4. **Economic disputes:** Often best resolved through trade agreements
5. **Military disputes:** Only when diplomatic options exhausted or at war

## Player Progression Path

### Early Game (Level 1-20)
**Focus:** Building initial influence
- Complete quests for influence rewards
- Trade regularly to build economic influence
- Join a guild for political influence bonuses
- Participate in PvE content for military influence

**Milestone:** Establish first Village (Level 10)

### Mid Game (Level 20-40)
**Focus:** Settlement expansion and diplomacy
- Upgrade Village to Town
- Form alliances with other players
- Participate in faction politics
- Challenge for additional settlements

**Milestone:** Form or join a Federation (Level 30)

### Late Game (Level 40+)
**Focus:** Empire building and strategic dominance
- Establish multiple settlements
- Lead or significantly contribute to federation
- Control high-tier settlements (Cities)
- Engage in large-scale diplomacy and warfare

**Milestone:** Control a City or lead a major Federation

## Tips for Success

### Settlement Management
1. **Population happiness is key:** Unhappy populations grow slowly and produce less
2. **Balance tax rates:** High taxes = more income but lower happiness
3. **Invest in infrastructure early:** Buildings pay for themselves over time
4. **Location matters:** Strategic positioning provides long-term advantages
5. **Maintain influence:** Active gameplay prevents influence decay

### Diplomatic Success
1. **Build relationships gradually:** Don't rush alliance proposals
2. **Communicate:** Use in-game messaging for diplomacy
3. **Keep commitments:** Breaking agreements severely damages reputation
4. **Mediate disputes:** Helping others builds political influence
5. **Choose allies wisely:** Allied wars affect you too

### Federation Leadership
1. **Clear governance:** Establish rules early and stick to them
2. **Fair contribution:** Don't let some members carry all the weight
3. **Active communication:** Regular federation meetings build cohesion
4. **Strategic vision:** Have clear goals for the federation
5. **Conflict resolution:** Address internal disputes quickly

### Combat Considerations
1. **Defensive buildings:** Worth the investment for valuable settlements
2. **Military influence:** Deters challenges even if you don't use it
3. **Choose battles wisely:** Every war has costs
4. **Timing matters:** Attack when enemy influence is low
5. **Exit strategy:** Know when to negotiate peace

## Frequently Asked Questions

**Q: How long does it take to establish a settlement?**
A: Instant upon meeting requirements and paying costs. Population and infrastructure develop over time.

**Q: Can I lose my settlement?**
A: Yes, if your influence falls too low or another entity successfully challenges your control.

**Q: What happens to my settlement if I'm offline?**
A: It continues operating, but influence decay is faster. Federation membership helps protect offline settlements.

**Q: How many settlements can I control?**
A: No hard limit, but influence requirements make controlling many settlements challenging. Focus on quality over quantity.

**Q: Can I be in multiple federations?**
A: No, entities can only be in one federation at a time.

**Q: What if I want to leave a federation?**
A: You can leave voluntarily, but there may be a cooldown period before joining another federation.

**Q: Do I need to be in a guild to engage with social systems?**
A: No, solo players can establish settlements and engage in diplomacy, though guild membership provides bonuses.

**Q: How do I increase my influence?**
A: Through gameplay activities in each influence category (diplomatic actions, trading, combat) and maintaining settlements.

## Conclusion

The Social Interaction and Settlement Management System provides deep strategic gameplay that rewards both cooperation and competition. Whether you prefer building a peaceful trading empire, forging powerful alliances, or conquering through military might, the system supports diverse playstyles while maintaining meaningful player interaction and world impact.

Success in this system comes from understanding influence mechanics, building strong relationships, and making strategic decisions about settlement management, diplomacy, and federation participation. The most powerful players and guilds are those who master all aspects of the system and adapt their strategies to changing political landscapes.
