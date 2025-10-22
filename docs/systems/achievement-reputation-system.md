# BlueMarble - Achievement and Reputation System Design

**Version:** 1.0  
**Date:** 2025-01-19  
**Author:** BlueMarble Systems Team

## Overview

This document details the achievement tracking, reputation management, and reward distribution systems for BlueMarble. These interconnected systems drive player progression, social dynamics, and economic integration while maintaining balance and preventing exploitation.

## System Architecture

### Core Components

```
┌─────────────────────────────────────────────────────────────┐
│                     Event System                             │
│  (Player Actions, Quest Completion, Combat, Trading, etc.)   │
└─────────────────┬───────────────────────────────────────────┘
                  │
                  ↓
┌─────────────────────────────────────────────────────────────┐
│              Achievement Tracking Engine                     │
│  - Progress monitoring  - Condition evaluation               │
│  - Multi-category tracking  - Tiered achievements            │
└─────────────┬──────────────────────────────┬────────────────┘
              │                               │
              ↓                               ↓
┌──────────────────────────┐    ┌──────────────────────────┐
│   Reputation System      │    │    Reward Distribution   │
│  - Faction standing      │    │  - Currency rewards      │
│  - Player trust score    │    │  - Item rewards          │
│  - Guild reputation      │    │  - Access unlocks        │
│  - Reputation spread     │    │  - Cosmetic rewards      │
└──────────┬───────────────┘    └────────┬─────────────────┘
           │                              │
           └──────────────┬───────────────┘
                          ↓
         ┌────────────────────────────────────┐
         │        Economy Integration         │
         │  - Market access tiers             │
         │  - Price bonuses/penalties         │
         │  - Special vendor access           │
         └────────────────────────────────────┘
```

## Achievement System

### Achievement Categories

#### 1. Combat Achievements

**Purpose:** Recognize combat prowess and tactical mastery

**Examples:**
- **First Blood:** Defeat your first enemy (Reward: 10 XP)
- **Monster Hunter:** Defeat 100 enemies (Reward: Title "Hunter", 500 XP)
- **Boss Slayer:** Defeat 10 unique boss enemies (Reward: Unique cosmetic, 1000 XP)
- **Perfect Victory:** Win a combat encounter without taking damage (Reward: Combat skill point)
- **Elite Killer:** Defeat 50 elite-tier enemies (Reward: Rare weapon blueprint)

**Progression Tiers:**
```
Novice → Experienced → Veteran → Master → Legendary
  100      500          1000      5000      10000 kills
```

#### 2. Exploration Achievements

**Purpose:** Encourage world discovery and geographic knowledge

**Examples:**
- **Cartographer:** Discover 50 unique locations (Reward: Map enhancement, 300 XP)
- **World Traveler:** Visit all major cities (Reward: Fast travel discount, Title "Traveler")
- **Deep Explorer:** Discover 10 hidden locations (Reward: Exploration skill boost)
- **Peak Climber:** Reach the highest point on the planet (Reward: Unique view cosmetic)
- **Cave Dweller:** Explore 25 underground locations (Reward: Dark vision ability)

**Discovery Types:**
- Cities and settlements
- Natural landmarks
- Dungeons and caves
- Secret areas
- Resource-rich zones

#### 3. Social Achievements

**Purpose:** Foster community engagement and cooperation

**Examples:**
- **Social Butterfly:** Add 50 players to friends list (Reward: Increased friend list capacity)
- **Guild Founder:** Create and maintain a guild for 30 days (Reward: Guild creation discount)
- **Party Leader:** Successfully complete 100 group activities (Reward: Leadership bonus)
- **Mentor:** Help 10 new players complete their first quest (Reward: Mentorship title, XP bonus)
- **Diplomat:** Reach positive reputation with all factions (Reward: Diplomatic immunity benefit)

**Guild Achievements:**
- Guild-wide progress tracking
- Shared rewards for all members
- Special guild-only achievements
- Competitive guild rankings

#### 4. Crafting Achievements

**Purpose:** Reward dedication to crafting professions

**Examples:**
- **Apprentice Smith:** Craft 100 items (Reward: Crafting speed +5%)
- **Master Craftsman:** Reach level 50 in any crafting profession (Reward: Title, recipe unlock)
- **Quality Artisan:** Craft 50 high-quality items (Reward: Quality success rate +3%)
- **Innovation:** Discover 10 new recipe variations (Reward: Experimental crafting unlock)
- **Mass Producer:** Craft 1000 total items (Reward: Bulk crafting efficiency)

**Profession-Specific:**
- Each crafting profession has unique achievement tracks
- Cross-profession achievements for versatility
- Quality-based achievements (common → legendary)

#### 5. Collection Achievements

**Purpose:** Encourage systematic gathering and completion

**Examples:**
- **Resource Hoarder:** Collect 10,000 total resources (Reward: Inventory expansion)
- **Rare Collector:** Obtain 25 rare items (Reward: Increased rare find chance)
- **Set Completionist:** Complete 5 equipment sets (Reward: Set bonus enhancement)
- **Treasure Hunter:** Find 50 treasure chests (Reward: Treasure detection ability)
- **Specimen Collector:** Catalog 100 unique creatures (Reward: Bestiary unlock)

#### 6. Economic Achievements

**Purpose:** Recognize trading success and economic contribution

**Examples:**
- **First Sale:** Complete your first player trade (Reward: Trade chat access)
- **Merchant:** Complete 100 successful trades (Reward: Reduced market fees)
- **Tycoon:** Accumulate 100,000 gold (Reward: Title "Tycoon", investment opportunities)
- **Market Maker:** List 1000 items in auction house (Reward: Additional listing slots)
- **Fair Trader:** Maintain 4.5+ trading reputation for 90 days (Reward: "Trusted Trader" badge)

### Achievement Structure

#### Data Model

```typescript
interface Achievement {
  id: string;
  name: string;
  description: string;
  category: AchievementCategory;
  tier: AchievementTier;
  
  // Progress tracking
  requirements: AchievementRequirement[];
  currentProgress: number;
  targetProgress: number;
  
  // Rewards
  rewards: AchievementReward[];
  
  // Metadata
  isHidden: boolean;  // Secret achievements
  isRepeatable: boolean;
  cooldownDays?: number;
  prerequisites: string[];  // Required achievement IDs
  
  // Stats
  completionRate: number;  // Percentage of players who completed
  rarity: AchievementRarity;
}

enum AchievementCategory {
  COMBAT = "combat",
  EXPLORATION = "exploration",
  SOCIAL = "social",
  CRAFTING = "crafting",
  COLLECTION = "collection",
  ECONOMIC = "economic",
  EVENT = "event"
}

enum AchievementTier {
  BRONZE = 1,
  SILVER = 2,
  GOLD = 3,
  PLATINUM = 4,
  LEGENDARY = 5
}

enum AchievementRarity {
  COMMON = "common",        // >50% completion rate
  UNCOMMON = "uncommon",    // 25-50%
  RARE = "rare",            // 10-25%
  EPIC = "epic",            // 1-10%
  LEGENDARY = "legendary"   // <1%
}
```

#### Reward Types

```typescript
interface AchievementReward {
  type: RewardType;
  amount?: number;
  itemId?: string;
  unlockId?: string;
}

enum RewardType {
  EXPERIENCE = "xp",
  CURRENCY = "currency",
  ITEM = "item",
  TITLE = "title",
  COSMETIC = "cosmetic",
  STAT_BONUS = "stat_bonus",
  ACCESS = "access",
  REPUTATION = "reputation",
  SKILL_POINT = "skill_point"
}
```

### Achievement Tracking Engine

#### Event-Driven Architecture

```typescript
class AchievementTracker {
  private eventBus: EventBus;
  private playerAchievements: Map<PlayerId, AchievementProgress[]>;
  
  constructor(eventBus: EventBus) {
    this.eventBus = eventBus;
    this.subscribeToEvents();
  }
  
  private subscribeToEvents(): void {
    // Subscribe to all relevant game events
    this.eventBus.on(GameEvent.ENEMY_KILLED, this.onEnemyKilled.bind(this));
    this.eventBus.on(GameEvent.LOCATION_DISCOVERED, this.onLocationDiscovered.bind(this));
    this.eventBus.on(GameEvent.ITEM_CRAFTED, this.onItemCrafted.bind(this));
    this.eventBus.on(GameEvent.QUEST_COMPLETED, this.onQuestCompleted.bind(this));
    this.eventBus.on(GameEvent.TRADE_COMPLETED, this.onTradeCompleted.bind(this));
    this.eventBus.on(GameEvent.GUILD_JOINED, this.onGuildJoined.bind(this));
  }
  
  private onEnemyKilled(event: EnemyKilledEvent): void {
    const achievements = this.getRelevantAchievements(
      event.playerId, 
      AchievementCategory.COMBAT
    );
    
    for (const achievement of achievements) {
      this.updateProgress(event.playerId, achievement.id, {
        enemyType: event.enemyType,
        enemyLevel: event.enemyLevel,
        isBoss: event.isBoss,
        damageTaken: event.playerDamageTaken
      });
    }
  }
  
  private updateProgress(
    playerId: PlayerId, 
    achievementId: string,
    context: any
  ): void {
    const progress = this.playerAchievements.get(playerId)
      ?.find(p => p.achievementId === achievementId);
    
    if (!progress || progress.completed) return;
    
    // Update progress based on achievement requirements
    progress.currentProgress += this.calculateProgressIncrement(
      achievementId, 
      context
    );
    
    // Check for completion
    if (progress.currentProgress >= progress.targetProgress) {
      this.completeAchievement(playerId, achievementId);
    }
  }
  
  private completeAchievement(
    playerId: PlayerId, 
    achievementId: string
  ): void {
    const achievement = this.getAchievement(achievementId);
    
    // Mark as completed
    this.markCompleted(playerId, achievementId);
    
    // Distribute rewards
    this.distributeRewards(playerId, achievement.rewards);
    
    // Update reputation if applicable
    if (achievement.reputationImpact) {
      this.updateReputation(playerId, achievement.reputationImpact);
    }
    
    // Broadcast achievement
    this.eventBus.emit(GameEvent.ACHIEVEMENT_COMPLETED, {
      playerId,
      achievementId,
      achievement
    });
    
    // Check for follow-up achievements
    this.checkDependentAchievements(playerId, achievementId);
  }
}
```

## Reputation System

### Reputation Types

#### 1. Faction Reputation

**Purpose:** Track standing with game factions and organizations

**Reputation Levels:**
```
Hated      -10000 to -6000   (Hostile, attacked on sight)
Hostile     -5999 to -3000   (Aggressive, limited services)
Unfriendly  -2999 to -1000   (Cold, basic services only)
Neutral     -999  to  999    (Standard interactions)
Friendly    1000  to  2999   (Discounts, additional options)
Honored     3000  to  5999   (Special items, significant benefits)
Revered     6000  to  9999   (Rare items, faction abilities)
Exalted     10000+           (Unique rewards, faction champion)
```

**Reputation Spread Mechanics:**

```typescript
class FactionReputationSystem {
  private factions: Map<FactionId, Faction>;
  private reputationSpreadRate: number = 0.3;
  
  modifyReputation(
    playerId: PlayerId,
    factionId: FactionId,
    amount: number,
    reason: string
  ): void {
    const faction = this.factions.get(factionId);
    const currentRep = faction.getPlayerReputation(playerId);
    const newRep = currentRep + amount;
    
    // Update primary faction
    faction.setPlayerReputation(playerId, newRep);
    this.logReputationChange(playerId, factionId, amount, reason);
    
    // Spread to allied/enemy factions
    this.spreadReputation(playerId, faction, amount);
    
    // Update access levels
    this.updatePlayerAccess(playerId, faction, newRep);
    
    // Check for reputation-based achievements
    this.checkReputationAchievements(playerId, factionId, newRep);
  }
  
  private spreadReputation(
    playerId: PlayerId,
    faction: Faction,
    amount: number
  ): void {
    // Positive rep spreads positively to allies, negatively to enemies
    const spreadAmount = amount * this.reputationSpreadRate;
    
    for (const [allyId, relationship] of faction.relationships) {
      if (relationship === FactionRelationship.ALLIED) {
        this.modifyReputation(
          playerId, 
          allyId, 
          spreadAmount * 0.5,
          `Alliance with ${faction.name}`
        );
      } else if (relationship === FactionRelationship.HOSTILE) {
        this.modifyReputation(
          playerId,
          allyId,
          -spreadAmount * 0.5,
          `Opposition to ${faction.name}`
        );
      }
    }
  }
  
  private updatePlayerAccess(
    playerId: PlayerId,
    faction: Faction,
    reputation: number
  ): void {
    const player = this.getPlayer(playerId);
    
    // Unlock faction-specific content based on reputation
    if (reputation >= 1000 && reputation < 3000) {
      player.unlockFactionVendor(faction.id, VendorTier.BASIC);
    } else if (reputation >= 3000 && reputation < 6000) {
      player.unlockFactionVendor(faction.id, VendorTier.ADVANCED);
      player.unlockFactionQuests(faction.id, QuestTier.ADVANCED);
    } else if (reputation >= 6000 && reputation < 10000) {
      player.unlockFactionVendor(faction.id, VendorTier.ELITE);
      player.unlockFactionQuests(faction.id, QuestTier.ELITE);
      player.unlockFactionAbility(faction.id);
    } else if (reputation >= 10000) {
      player.unlockFactionVendor(faction.id, VendorTier.LEGENDARY);
      player.unlockFactionMountOrPet(faction.id);
      player.grantFactionChampionStatus(faction.id);
    }
  }
}
```

#### 2. Player Trust Score

**Purpose:** Track reliability in player-to-player interactions

**Components:**
```typescript
interface PlayerTrustScore {
  playerId: PlayerId;
  
  // Overall trust score (0-1000)
  overallScore: number;
  
  // Component scores
  tradingReputation: number;      // 0-200
  questCompletionRate: number;    // 0-200
  guildContribution: number;      // 0-200
  communityReports: number;       // 0-200 (negative from reports)
  helpfulness: number;            // 0-200
  
  // Metadata
  totalTrades: number;
  successfulTrades: number;
  totalReports: number;
  verifiedReports: number;
  accountAge: number;  // Days
  
  // Trust tier
  tier: TrustTier;
  
  // History
  reputationHistory: ReputationEvent[];
}

enum TrustTier {
  UNTRUSTED = "untrusted",      // 0-199
  NEUTRAL = "neutral",          // 200-399
  RELIABLE = "reliable",        // 400-599
  TRUSTED = "trusted",          // 600-799
  EXEMPLARY = "exemplary"       // 800-1000
}
```

**Trust Score Calculation:**

```typescript
class PlayerTrustSystem {
  calculateTrustScore(playerId: PlayerId): number {
    const data = this.getPlayerData(playerId);
    
    // Trading reputation (0-200)
    const tradeScore = this.calculateTradeScore(data);
    
    // Quest completion rate (0-200)
    const questScore = data.questsCompleted / data.questsAccepted * 200;
    
    // Guild contribution (0-200)
    const guildScore = this.calculateGuildScore(data);
    
    // Community reports (0-200, negative impact)
    const reportScore = Math.max(0, 200 - (data.verifiedReports * 50));
    
    // Helpfulness (0-200)
    const helpScore = this.calculateHelpfulnessScore(data);
    
    // Total score
    const totalScore = tradeScore + questScore + guildScore + 
                       reportScore + helpScore;
    
    // Apply account age modifier (bonus for older accounts)
    const ageMod = Math.min(1.2, 1 + (data.accountAge / 365) * 0.1);
    
    return Math.min(1000, totalScore * ageMod);
  }
  
  private calculateTradeScore(data: PlayerData): number {
    if (data.totalTrades === 0) return 100; // Neutral start
    
    const successRate = data.successfulTrades / data.totalTrades;
    const baseScore = successRate * 200;
    
    // Bonus for high volume traders
    const volumeBonus = Math.min(20, data.totalTrades / 100);
    
    return Math.min(200, baseScore + volumeBonus);
  }
  
  private calculateGuildScore(data: PlayerData): number {
    if (!data.guildId) return 100; // Neutral if not in guild
    
    const guild = this.getGuild(data.guildId);
    const contribution = guild.getMemberContribution(data.playerId);
    
    // Score based on relative contribution
    const avgContribution = guild.averageMemberContribution;
    const relativeScore = (contribution / avgContribution) * 100;
    
    return Math.min(200, relativeScore);
  }
  
  private calculateHelpfulnessScore(data: PlayerData): number {
    let score = 0;
    
    // Mentoring new players
    score += data.mentoredPlayers * 10;
    
    // Helping with quests
    score += data.questsHelped * 2;
    
    // Positive community interactions
    score += data.positiveFeedback * 5;
    
    // Resource sharing
    score += data.resourcesShared / 100;
    
    return Math.min(200, score);
  }
}
```

#### 3. Guild Reputation

**Purpose:** Track guild standing within the community

```typescript
interface GuildReputation {
  guildId: GuildId;
  
  // Overall guild score
  reputationScore: number;
  
  // Component metrics
  questCompletionRate: number;
  eventParticipation: number;
  communityContribution: number;
  pvpRanking: number;
  craftingReputation: number;
  
  // Guild achievements
  achievements: string[];
  
  // Inter-guild relations
  alliances: Map<GuildId, number>;
  rivalries: Map<GuildId, number>;
}
```

### Reputation Impact on Economy

#### Market Access Tiers

```typescript
class MarketAccessSystem {
  getPlayerMarketAccess(playerId: PlayerId): MarketAccess {
    const trustScore = this.trustSystem.getTrustScore(playerId);
    
    return {
      canSell: trustScore.overallScore >= 200,
      canBuy: true,  // Always allowed
      
      // Listing limits
      maxListings: this.calculateMaxListings(trustScore),
      maxListingValue: this.calculateMaxValue(trustScore),
      
      // Fee modifiers
      listingFeeModifier: this.calculateFeeModifier(trustScore),
      
      // Special privileges
      canUsePremiumListings: trustScore.tier >= TrustTier.TRUSTED,
      canCreateAuctions: trustScore.tier >= TrustTier.RELIABLE,
      canDirectTrade: trustScore.overallScore >= 100
    };
  }
  
  private calculateMaxListings(trustScore: PlayerTrustScore): number {
    const baseListings = 10;
    const bonusPerTier = 5;
    
    switch (trustScore.tier) {
      case TrustTier.UNTRUSTED: return baseListings;
      case TrustTier.NEUTRAL: return baseListings + bonusPerTier;
      case TrustTier.RELIABLE: return baseListings + bonusPerTier * 2;
      case TrustTier.TRUSTED: return baseListings + bonusPerTier * 3;
      case TrustTier.EXEMPLARY: return baseListings + bonusPerTier * 5;
    }
  }
  
  private calculateFeeModifier(trustScore: PlayerTrustScore): number {
    // Higher trust = lower fees
    const baseFee = 1.0;
    const maxDiscount = 0.5;  // Up to 50% fee reduction
    
    const discount = (trustScore.overallScore / 1000) * maxDiscount;
    return baseFee - discount;
  }
}
```

#### Vendor Price Modifiers

```typescript
class VendorPricingSystem {
  calculatePrice(
    basePrice: number,
    playerId: PlayerId,
    vendorId: VendorId
  ): number {
    const vendor = this.getVendor(vendorId);
    
    if (vendor.factionId) {
      const reputation = this.reputationSystem.getFactionReputation(
        playerId, 
        vendor.factionId
      );
      
      return this.applyReputationModifier(basePrice, reputation);
    }
    
    return basePrice;
  }
  
  private applyReputationModifier(
    basePrice: number, 
    reputation: number
  ): number {
    let modifier = 1.0;
    
    if (reputation < -3000) {
      modifier = 1.5;  // 50% markup for hostile
    } else if (reputation < 0) {
      modifier = 1.2;  // 20% markup for unfriendly
    } else if (reputation >= 1000 && reputation < 3000) {
      modifier = 0.95;  // 5% discount for friendly
    } else if (reputation >= 3000 && reputation < 6000) {
      modifier = 0.90;  // 10% discount for honored
    } else if (reputation >= 6000 && reputation < 10000) {
      modifier = 0.85;  // 15% discount for revered
    } else if (reputation >= 10000) {
      modifier = 0.80;  // 20% discount for exalted
    }
    
    return Math.round(basePrice * modifier);
  }
}
```

## Reward Distribution System

### Reward Types and Balancing

#### Currency Rewards

```typescript
class CurrencyRewardSystem {
  calculateAchievementReward(achievement: Achievement): CurrencyReward {
    const baseReward = this.getBaseReward(achievement.tier);
    const categoryModifier = this.getCategoryModifier(achievement.category);
    const rarityBonus = this.getRarityBonus(achievement.rarity);
    
    const goldReward = Math.floor(
      baseReward * categoryModifier * rarityBonus
    );
    
    return {
      gold: goldReward,
      honorPoints: this.calculateHonorPoints(achievement),
      eventCurrency: this.calculateEventCurrency(achievement)
    };
  }
  
  private getBaseReward(tier: AchievementTier): number {
    switch (tier) {
      case AchievementTier.BRONZE: return 100;
      case AchievementTier.SILVER: return 500;
      case AchievementTier.GOLD: return 2000;
      case AchievementTier.PLATINUM: return 10000;
      case AchievementTier.LEGENDARY: return 50000;
    }
  }
  
  private getCategoryModifier(category: AchievementCategory): number {
    // Economic achievements give more currency
    // Combat achievements give less currency but more combat-related rewards
    switch (category) {
      case AchievementCategory.ECONOMIC: return 1.5;
      case AchievementCategory.COMBAT: return 0.8;
      case AchievementCategory.EXPLORATION: return 1.0;
      case AchievementCategory.SOCIAL: return 1.2;
      case AchievementCategory.CRAFTING: return 1.1;
      case AchievementCategory.COLLECTION: return 1.0;
      default: return 1.0;
    }
  }
  
  private getRarityBonus(rarity: AchievementRarity): number {
    switch (rarity) {
      case AchievementRarity.COMMON: return 1.0;
      case AchievementRarity.UNCOMMON: return 1.2;
      case AchievementRarity.RARE: return 1.5;
      case AchievementRarity.EPIC: return 2.0;
      case AchievementRarity.LEGENDARY: return 3.0;
    }
  }
}
```

#### Item Rewards

```typescript
interface ItemReward {
  itemId: string;
  quantity: number;
  quality?: ItemQuality;
  isBound: boolean;  // Bound to player or tradeable
}

class ItemRewardSystem {
  generateReward(achievement: Achievement): ItemReward | null {
    // Not all achievements give item rewards
    if (!achievement.itemRewardTable) return null;
    
    const roll = Math.random();
    const rewardTable = achievement.itemRewardTable;
    
    for (const entry of rewardTable.entries) {
      if (roll <= entry.chance) {
        return {
          itemId: entry.itemId,
          quantity: entry.quantity,
          quality: entry.quality,
          isBound: entry.isBound
        };
      }
    }
    
    return null;
  }
}
```

#### Title and Cosmetic Rewards

```typescript
interface TitleReward {
  titleId: string;
  name: string;
  displayFormat: string;  // e.g., "{{name}} the Mighty"
  rarity: TitleRarity;
  effects?: TitleEffect[];
}

enum TitleRarity {
  COMMON = "common",
  RARE = "rare",
  EPIC = "epic",
  LEGENDARY = "legendary"
}

interface TitleEffect {
  type: string;  // "stat_bonus", "visual_effect", "reputation_boost"
  value: number;
  target?: string;
}
```

### Anti-Exploit Mechanisms

Based on research from trust-player-created-quests-reputation-systems.md:

```typescript
class AntiExploitSystem {
  // Prevent achievement farming between alt accounts
  detectSuspiciousProgress(
    playerId: PlayerId,
    achievementId: string,
    context: any
  ): boolean {
    const history = this.getProgressHistory(playerId, achievementId);
    
    // Check for rapid progress
    if (this.isProgressTooFast(history)) {
      this.flagForReview(playerId, "rapid_achievement_progress");
      return true;
    }
    
    // Check for repeated interactions with same players
    if (this.detectCollusionPattern(playerId, context)) {
      this.flagForReview(playerId, "possible_achievement_collusion");
      return true;
    }
    
    // Check for unusual activity patterns
    if (this.detectBotBehavior(history)) {
      this.flagForReview(playerId, "bot_like_achievement_farming");
      return true;
    }
    
    return false;
  }
  
  // Diminishing returns for repetitive actions
  calculateDiminishingReturns(
    playerId: PlayerId,
    action: string,
    recentCount: number
  ): number {
    const baseValue = 1.0;
    const diminishingFactor = 0.95;
    
    // Each repeat within timeframe reduces effectiveness
    return baseValue * Math.pow(diminishingFactor, recentCount);
  }
  
  // Network analysis for reputation farming rings
  detectReputationFarmingRing(playerIds: PlayerId[]): boolean {
    const interactions = this.getInteractionNetwork(playerIds);
    
    // Check for closed loops of interactions
    const isClosedLoop = this.detectClosedLoop(interactions);
    
    // Check for disproportionate mutual benefit
    const isMutualInflation = this.detectMutualInflation(interactions);
    
    return isClosedLoop && isMutualInflation;
  }
}
```

## Integration with Existing Systems

### Quest System Integration

```typescript
class QuestAchievementIntegration {
  onQuestCompleted(event: QuestCompletedEvent): void {
    const player = this.getPlayer(event.playerId);
    const quest = this.getQuest(event.questId);
    
    // Update quest-related achievements
    this.achievementTracker.updateProgress(
      event.playerId,
      "quests_completed_total",
      { questId: quest.id, difficulty: quest.difficulty }
    );
    
    // Category-specific achievements
    if (quest.category === QuestCategory.FACTION) {
      this.achievementTracker.updateProgress(
        event.playerId,
        "faction_quests_completed",
        { factionId: quest.factionId }
      );
    }
    
    // Award reputation from quest
    if (quest.reputationReward) {
      this.reputationSystem.modifyReputation(
        event.playerId,
        quest.factionId,
        quest.reputationReward,
        `Quest: ${quest.name}`
      );
    }
  }
}
```

### Economy System Integration

```typescript
class EconomyAchievementIntegration {
  onTradeCompleted(event: TradeCompletedEvent): void {
    // Update trading achievements
    this.achievementTracker.updateProgress(
      event.sellerId,
      "trades_completed",
      { tradeValue: event.totalValue }
    );
    
    this.achievementTracker.updateProgress(
      event.buyerId,
      "trades_completed",
      { tradeValue: event.totalValue }
    );
    
    // Update trust scores
    this.trustSystem.recordSuccessfulTrade(
      event.sellerId,
      event.buyerId,
      event.totalValue
    );
    
    // Check for wealth accumulation achievements
    const sellerWealth = this.getPlayerWealth(event.sellerId);
    if (sellerWealth >= 100000) {
      this.achievementTracker.completeAchievement(
        event.sellerId,
        "wealth_tycoon"
      );
    }
  }
  
  onAuctionCompleted(event: AuctionCompletedEvent): void {
    // Update auction-specific achievements
    this.achievementTracker.updateProgress(
      event.sellerId,
      "auctions_completed",
      { finalPrice: event.finalPrice }
    );
  }
}
```

### Social System Integration

```typescript
class SocialAchievementIntegration {
  onGuildJoined(event: GuildJoinedEvent): void {
    this.achievementTracker.completeAchievement(
      event.playerId,
      "guild_member"
    );
    
    // Track guild membership duration for future achievements
    this.trackGuildMembership(event.playerId, event.guildId);
  }
  
  onPlayerHelped(event: PlayerHelpedEvent): void {
    // Update helpfulness score
    this.trustSystem.recordHelpfulAction(
      event.helperId,
      event.helpedPlayerId,
      event.actionType
    );
    
    // Update mentorship achievements
    if (event.actionType === HelpType.MENTORING) {
      this.achievementTracker.updateProgress(
        event.helperId,
        "mentor_achievement",
        { helpedPlayer: event.helpedPlayerId }
      );
    }
  }
}
```

## Database Schema

### Achievement Tables

```sql
-- Achievement definitions
CREATE TABLE achievements (
  id VARCHAR(64) PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  description TEXT,
  category VARCHAR(32) NOT NULL,
  tier INT NOT NULL,
  target_progress INT NOT NULL,
  is_hidden BOOLEAN DEFAULT FALSE,
  is_repeatable BOOLEAN DEFAULT FALSE,
  cooldown_days INT,
  rarity VARCHAR(32),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  INDEX idx_category (category),
  INDEX idx_tier (tier)
);

-- Achievement prerequisites
CREATE TABLE achievement_prerequisites (
  achievement_id VARCHAR(64),
  prerequisite_id VARCHAR(64),
  PRIMARY KEY (achievement_id, prerequisite_id),
  FOREIGN KEY (achievement_id) REFERENCES achievements(id),
  FOREIGN KEY (prerequisite_id) REFERENCES achievements(id)
);

-- Player achievement progress
CREATE TABLE player_achievements (
  player_id VARCHAR(64),
  achievement_id VARCHAR(64),
  current_progress INT DEFAULT 0,
  completed BOOLEAN DEFAULT FALSE,
  completed_at TIMESTAMP,
  PRIMARY KEY (player_id, achievement_id),
  FOREIGN KEY (achievement_id) REFERENCES achievements(id),
  INDEX idx_player_completed (player_id, completed),
  INDEX idx_completed_at (completed_at)
);

-- Achievement rewards
CREATE TABLE achievement_rewards (
  achievement_id VARCHAR(64),
  reward_type VARCHAR(32) NOT NULL,
  reward_amount INT,
  reward_item_id VARCHAR(64),
  reward_unlock_id VARCHAR(64),
  FOREIGN KEY (achievement_id) REFERENCES achievements(id),
  INDEX idx_achievement (achievement_id)
);
```

### Reputation Tables

```sql
-- Faction definitions
CREATE TABLE factions (
  id VARCHAR(64) PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  description TEXT,
  icon_url VARCHAR(512),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Faction relationships
CREATE TABLE faction_relationships (
  faction_id VARCHAR(64),
  related_faction_id VARCHAR(64),
  relationship_type VARCHAR(32) NOT NULL,  -- 'ALLIED', 'HOSTILE', 'NEUTRAL'
  influence_factor DECIMAL(3,2) DEFAULT 0.5,
  PRIMARY KEY (faction_id, related_faction_id),
  FOREIGN KEY (faction_id) REFERENCES factions(id),
  FOREIGN KEY (related_faction_id) REFERENCES factions(id)
);

-- Player faction reputation
CREATE TABLE player_faction_reputation (
  player_id VARCHAR(64),
  faction_id VARCHAR(64),
  reputation INT DEFAULT 0,
  last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (player_id, faction_id),
  FOREIGN KEY (faction_id) REFERENCES factions(id),
  INDEX idx_player (player_id),
  INDEX idx_reputation (reputation)
);

-- Reputation change log
CREATE TABLE reputation_history (
  id BIGINT AUTO_INCREMENT PRIMARY KEY,
  player_id VARCHAR(64) NOT NULL,
  faction_id VARCHAR(64) NOT NULL,
  change_amount INT NOT NULL,
  reason VARCHAR(512),
  timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (faction_id) REFERENCES factions(id),
  INDEX idx_player_faction (player_id, faction_id),
  INDEX idx_timestamp (timestamp)
);

-- Player trust scores
CREATE TABLE player_trust_scores (
  player_id VARCHAR(64) PRIMARY KEY,
  overall_score INT DEFAULT 0,
  trading_reputation INT DEFAULT 100,
  quest_completion_rate INT DEFAULT 100,
  guild_contribution INT DEFAULT 100,
  community_reports INT DEFAULT 200,
  helpfulness INT DEFAULT 0,
  total_trades INT DEFAULT 0,
  successful_trades INT DEFAULT 0,
  total_reports INT DEFAULT 0,
  verified_reports INT DEFAULT 0,
  trust_tier VARCHAR(32) DEFAULT 'NEUTRAL',
  last_calculated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  INDEX idx_overall_score (overall_score),
  INDEX idx_trust_tier (trust_tier)
);

-- Guild reputation
CREATE TABLE guild_reputation (
  guild_id VARCHAR(64) PRIMARY KEY,
  reputation_score INT DEFAULT 0,
  quest_completion_rate DECIMAL(5,2) DEFAULT 0,
  event_participation INT DEFAULT 0,
  community_contribution INT DEFAULT 0,
  pvp_ranking INT DEFAULT 0,
  crafting_reputation INT DEFAULT 0,
  last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  INDEX idx_reputation_score (reputation_score)
);
```

## API Endpoints

### Achievement Endpoints

```typescript
// GET /api/v1/achievements
// List all available achievements
interface GetAchievementsRequest {
  category?: AchievementCategory;
  tier?: AchievementTier;
  includeHidden?: boolean;
  page?: number;
  pageSize?: number;
}

interface GetAchievementsResponse {
  achievements: Achievement[];
  totalCount: number;
  page: number;
  pageSize: number;
}

// GET /api/v1/achievements/:achievementId
// Get specific achievement details
interface GetAchievementResponse {
  achievement: Achievement;
  globalCompletionRate: number;
  recentCompletions: PlayerAchievementCompletion[];
}

// GET /api/v1/players/:playerId/achievements
// Get player's achievement progress
interface GetPlayerAchievementsResponse {
  completed: Achievement[];
  inProgress: AchievementProgress[];
  locked: Achievement[];
  completionPercentage: number;
  achievementPoints: number;
}

// GET /api/v1/players/:playerId/achievements/:achievementId/progress
// Get detailed progress for specific achievement
interface GetAchievementProgressResponse {
  achievement: Achievement;
  currentProgress: number;
  targetProgress: number;
  progressPercentage: number;
  estimatedCompletion?: Date;
  recentActivity: ProgressEvent[];
}
```

### Reputation Endpoints

```typescript
// GET /api/v1/players/:playerId/reputation
// Get player's reputation summary
interface GetPlayerReputationResponse {
  factionReputation: FactionReputationEntry[];
  trustScore: PlayerTrustScore;
  reputationSummary: {
    highestFaction: FactionReputationEntry;
    lowestFaction: FactionReputationEntry;
    neutralFactions: number;
    totalReputation: number;
  };
}

// GET /api/v1/players/:playerId/reputation/faction/:factionId
// Get detailed faction reputation
interface GetFactionReputationResponse {
  faction: Faction;
  currentReputation: number;
  reputationLevel: string;
  nextLevelThreshold: number;
  progressToNextLevel: number;
  benefits: FactionBenefit[];
  history: ReputationEvent[];
}

// GET /api/v1/players/:playerId/trust-score
// Get player trust score details
interface GetTrustScoreResponse {
  trustScore: PlayerTrustScore;
  breakdown: {
    trading: TrustComponent;
    questing: TrustComponent;
    guild: TrustComponent;
    reports: TrustComponent;
    helpfulness: TrustComponent;
  };
  benefits: TrustBenefit[];
  penalties: TrustPenalty[];
}

// GET /api/v1/factions
// List all factions
interface GetFactionsResponse {
  factions: Faction[];
  playerReputation?: Map<FactionId, number>;
}
```

## Performance Considerations

### Caching Strategy

```typescript
class AchievementCacheManager {
  private cache: Redis;
  
  async getPlayerAchievements(playerId: PlayerId): Promise<Achievement[]> {
    const cacheKey = `player:${playerId}:achievements`;
    const cached = await this.cache.get(cacheKey);
    
    if (cached) {
      return JSON.parse(cached);
    }
    
    const achievements = await this.database.getPlayerAchievements(playerId);
    await this.cache.setex(
      cacheKey, 
      3600,  // 1 hour TTL
      JSON.stringify(achievements)
    );
    
    return achievements;
  }
  
  async invalidatePlayerCache(playerId: PlayerId): Promise<void> {
    const keys = [
      `player:${playerId}:achievements`,
      `player:${playerId}:reputation`,
      `player:${playerId}:trust`
    ];
    
    await this.cache.del(...keys);
  }
}
```

### Batch Processing

```typescript
class BatchAchievementProcessor {
  private updateQueue: Map<PlayerId, AchievementUpdate[]>;
  private processingInterval: number = 5000;  // 5 seconds
  
  queueUpdate(playerId: PlayerId, update: AchievementUpdate): void {
    if (!this.updateQueue.has(playerId)) {
      this.updateQueue.set(playerId, []);
    }
    
    this.updateQueue.get(playerId).push(update);
  }
  
  async processBatch(): Promise<void> {
    const updates = Array.from(this.updateQueue.entries());
    this.updateQueue.clear();
    
    // Process in parallel batches
    const batchSize = 100;
    for (let i = 0; i < updates.length; i += batchSize) {
      const batch = updates.slice(i, i + batchSize);
      await Promise.all(
        batch.map(([playerId, playerUpdates]) => 
          this.processPlayerUpdates(playerId, playerUpdates)
        )
      );
    }
  }
  
  private async processPlayerUpdates(
    playerId: PlayerId,
    updates: AchievementUpdate[]
  ): Promise<void> {
    // Aggregate updates
    const aggregated = this.aggregateUpdates(updates);
    
    // Single database transaction for all updates
    await this.database.transaction(async (tx) => {
      for (const [achievementId, progress] of aggregated) {
        await tx.updateAchievementProgress(
          playerId,
          achievementId,
          progress
        );
      }
    });
    
    // Invalidate cache
    await this.cacheManager.invalidatePlayerCache(playerId);
  }
}
```

## Testing Strategy

### Unit Tests

```typescript
describe('AchievementTracker', () => {
  it('should update progress on enemy killed event', () => {
    const tracker = new AchievementTracker(eventBus);
    const playerId = 'player123';
    
    eventBus.emit(GameEvent.ENEMY_KILLED, {
      playerId,
      enemyType: 'dragon',
      isBoss: true
    });
    
    const progress = tracker.getProgress(playerId, 'dragon_slayer');
    expect(progress.currentProgress).toBe(1);
  });
  
  it('should complete achievement and distribute rewards', async () => {
    const tracker = new AchievementTracker(eventBus);
    const playerId = 'player123';
    
    // Complete achievement
    await tracker.completeAchievement(playerId, 'first_kill');
    
    const player = await getPlayer(playerId);
    expect(player.achievements).toContain('first_kill');
    expect(player.gold).toBeGreaterThan(0);
  });
});

describe('ReputationSystem', () => {
  it('should spread reputation to allied factions', () => {
    const repSystem = new FactionReputationSystem();
    
    repSystem.modifyReputation('player123', 'faction_a', 1000, 'quest');
    
    const allyRep = repSystem.getReputation('player123', 'faction_b');
    expect(allyRep).toBeGreaterThan(0);
  });
  
  it('should update player access based on reputation', () => {
    const repSystem = new FactionReputationSystem();
    
    repSystem.modifyReputation('player123', 'faction_a', 3000, 'multiple_quests');
    
    const access = repSystem.getPlayerAccess('player123', 'faction_a');
    expect(access.vendorTier).toBe(VendorTier.ADVANCED);
  });
});
```

### Integration Tests

```typescript
describe('Achievement-Economy Integration', () => {
  it('should award currency on achievement completion', async () => {
    const initialGold = await getPlayerGold('player123');
    
    await completeAchievement('player123', 'merchant_achievement');
    
    const finalGold = await getPlayerGold('player123');
    expect(finalGold).toBeGreaterThan(initialGold);
  });
  
  it('should provide market access based on trust score', async () => {
    const player = await getPlayer('player123');
    player.trustScore.overallScore = 800;
    
    const access = await getMarketAccess('player123');
    expect(access.maxListings).toBeGreaterThan(10);
    expect(access.canUsePremiumListings).toBe(true);
  });
});
```

## Monitoring and Analytics

### Key Metrics

```typescript
interface AchievementMetrics {
  // Completion rates
  overallCompletionRate: number;
  completionRateByCategory: Map<AchievementCategory, number>;
  completionRateByTier: Map<AchievementTier, number>;
  
  // Player engagement
  averageAchievementsPerPlayer: number;
  achievementHunterPercentage: number;  // Players with >50 achievements
  
  // Time metrics
  averageTimeToComplete: Map<string, number>;
  fastestCompletions: AchievementCompletion[];
  
  // Reward distribution
  totalCurrencyAwarded: number;
  totalItemsAwarded: number;
  mostValuableRewards: Reward[];
}

interface ReputationMetrics {
  // Faction metrics
  averageReputationByFaction: Map<FactionId, number>;
  exaltedPlayerPercentage: number;
  
  // Trust metrics
  averageTrustScore: number;
  trustDistribution: Map<TrustTier, number>;
  
  // Trading metrics
  averageTradesPerPlayer: number;
  tradeSuccessRate: number;
  reportedPlayersPercentage: number;
}
```

## Future Enhancements

### Planned Features

1. **Dynamic Achievements**
   - Server-wide achievements (e.g., "First to defeat new raid boss")
   - Time-limited achievements during events
   - Competitive achievements with leaderboards

2. **Reputation Decay**
   - Inactive players lose reputation over time
   - Requires maintenance of relationships
   - Encourages consistent engagement

3. **Achievement Chains**
   - Series of connected achievements
   - Story-driven achievement progression
   - Unlockable achievement branches

4. **Social Reputation**
   - Player-to-player endorsements
   - Commendation system
   - Mentor/student reputation bonuses

5. **Cross-Server Reputation**
   - Global reputation tracking
   - Server transfer reputation preservation
   - Cross-server faction standings

## References

- [Game Programming Patterns - Observer Pattern](/research/literature/game-dev-analysis-game-programming-patterns.md)
- [Trust and Player-Created Quests Research](/research/topics/trust-player-created-quests-reputation-systems.md)
- [Cyberpunk RED Faction System](/research/literature/game-design-mechanics-analysis.md)
- [Quest System Design](/docs/systems/gameplay-systems.md)
- [Economy Systems](/docs/systems/economy-systems.md)

---

**Document Status:** Complete  
**Last Updated:** 2025-01-19  
**Next Review:** 2025-02-19
