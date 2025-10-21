# Path of Exile: Designing Sustainable Loot - GDC Talk Analysis

---
title: Path of Exile Sustainable Loot Economy Analysis for BlueMarble
date: 2025-01-17
tags: [game-development, economy, mmorpg, path-of-exile, loot-systems, seasonal-economy, crafting]
status: complete
priority: high
assignment-group: 42
phase: 3
source-type: case-study
---

**Source:** Path of Exile: Designing Sustainable Loot Systems - GDC Talk  
**Developer:** Grinding Gear Games  
**Presenter:** Chris Wilson (Lead Designer)  
**URL:** gdcvault.com  
**Category:** MMORPG Economy Design, Loot Systems  
**Priority:** High  
**Status:** ✅ Complete  
**Assignment Group:** 42 (Economy Case Studies)  
**Source Number:** 3 of 5

---

## Executive Summary

Path of Exile (PoE) revolutionized ARPG economies by making loot the primary material source and currency items serve dual purposes as both crafting materials (material sinks) and trade medium. Their seasonal Challenge League system resets the economy every 3-4 months, maintaining freshness while preserving long-term Standard league progress. This model prevents the economic stagnation that plagues many long-running games.

**Key Innovations:**

1. **Currency as Crafting Materials** - All currency items have intrinsic use (reroll item properties)
2. **No Gold System** - Barter economy using currency items prevents inflation
3. **Challenge Leagues** - Fresh economies every 3-4 months with experimental mechanics
4. **No Auction House** - Player-to-player trading maintains scarcity and social aspects
5. **Loot Filter System** - Players customize what loot they see (QoL without changing economy)
6. **Deterministic Crafting** - Harvest league introduced controllable crafting (controversial)
7. **Item Sink Mechanics** - Corrupting items (irreversible), fractured items, synthesized items

**BlueMarble Applications:**

- Dual-purpose currency prevents inflation while creating sinks
- Seasonal events can reset regional economies without affecting core world
- No central auction house preserves trading as gameplay
- Loot filters improve UX without undermining scarcity
- Experimental mechanics in limited-time events test balance
- Corruption/permanent modification systems create item sinks
- Crafting benches consume currency (predictable sink)

**Key Takeaways for BlueMarble:**

1. **Currency with intrinsic value** - Every currency must have a use beyond trading
2. **Seasonal resets work** - Fresh starts maintain engagement without destroying progress
3. **Scarcity creates value** - No auction house = items retain worth
4. **Experimental mechanics** - Test wild ideas in temporary leagues
5. **Loot abundance with filters** - Show less, not drop less
6. **Crafting as sink** - Make crafting consume valuable resources

---

## Core Economic Philosophy

### 1. Currency Items as Dual-Purpose Assets

**The Genius of PoE's Currency:**

Unlike traditional MMORPGs where currency is abstract (gold coins), PoE's currency items are consumable crafting materials:

**Orb of Transmutation:**
- Function: Upgrades normal item to magic (random properties)
- Trade Value: 1/10th of a Chaos Orb
- Material Sink: Consumed when used on item
- Result: Item has value, currency has value, both are finite

**Chaos Orb:**
- Function: Rerolls all properties on a rare item
- Trade Value: Primary trade currency
- Material Sink: Consumed when used
- Result: Standard trade unit with inherent utility

**Exalted Orb:**
- Function: Adds one random property to rare item
- Trade Value: High-value trades (50-100 Chaos Orbs)
- Material Sink: Consumed when used
- Result: Rare enough to hold value, useful enough to be consumed

**Mirror of Kalandra:**
- Function: Duplicates any item (extremely rare)
- Trade Value: Hundreds of Exalted Orbs
- Material Sink: Consumed when used
- Result: Chase item that maintains value through extreme scarcity

**Economic Benefits:**

1. **No Gold Inflation** - Currency is consumed during use, constantly removed from economy
2. **Intrinsic Value** - Every currency has use, no "worthless" currencies
3. **Natural Price Discovery** - Relative utility determines exchange rates
4. **Sink and Medium** - Same item serves as sink and trade medium
5. **Scarcity Matters** - Rarity directly impacts both trading and crafting power

**BlueMarble Implementation:**
```cpp
// Dual-purpose currency system inspired by Path of Exile
class DualPurposeCurrency {
public:
    enum class CurrencyType {
        // Basic crafting currencies
        TransmutationOrb,   // Normal → Magic
        AugmentationOrb,    // Add property to magic
        AlterationOrb,      // Reroll magic properties
        AlchemyOrb,         // Normal → Rare
        ChaosOrb,           // Reroll rare properties
        
        // Advanced crafting
        RegalOrb,           // Magic → Rare
        ExaltedOrb,         // Add property to rare
        DivineOrb,          // Reroll property values
        VaalOrb,            // Corrupt item (risky)
        
        // Meta currencies
        ScoringOrb,         // Currency for currency
        MirrorOrb           // Duplicate item (ultra-rare)
    };
    
    struct CurrencyConfig {
        CurrencyType type;
        std::string name;
        std::string description;
        float dropRate;         // Per monster kill
        int stackSize;          // Max in one inventory slot
        
        // Crafting function
        std::function<void(Item*)> craftingEffect;
        
        // Trade value relative to ChaosOrb (baseline)
        float tradeValueInChaos;
    };
    
    // Use currency on an item (CONSUMPTION = SINK!)
    bool UseCurrency(Player* player, CurrencyType currency, Item* targetItem) {
        // Verify player has currency
        if (!player->HasCurrency(currency, 1)) {
            player->SendMessage("You don't have any " + 
                CurrencyTypeToString(currency));
            return false;
        }
        
        // Verify item can be modified
        if (!CanModifyItem(targetItem, currency)) {
            player->SendMessage("This currency cannot be used on this item.");
            return false;
        }
        
        // Consume currency (THE SINK!)
        player->RemoveCurrency(currency, 1);
        
        // Log for economy tracking
        EconomyMetrics::RecordCurrencySink(currency, 1, "crafting");
        
        // Apply crafting effect
        auto config = GetCurrencyConfig(currency);
        config.craftingEffect(targetItem);
        
        // Notify player
        player->SendMessage("You've used " + config.name + " on " + 
            targetItem->GetName());
        
        return true;
    }
    
    // Example crafting effects
    void UpgradeNormalToMagic(Item* item) {
        if (item->GetRarity() != ItemRarity::Normal) return;
        
        item->SetRarity(ItemRarity::Magic);
        
        // Add 1-2 random properties
        int propCount = Random::Range(1, 2);
        for (int i = 0; i < propCount; ++i) {
            item->AddRandomProperty(ItemRarity::Magic);
        }
    }
    
    void RerollRareProperties(Item* item) {
        if (item->GetRarity() != ItemRarity::Rare) return;
        
        // Remove all properties
        item->ClearProperties();
        
        // Add 4-6 new random properties
        int propCount = Random::Range(4, 6);
        for (int i = 0; i < propCount; ++i) {
            item->AddRandomProperty(ItemRarity::Rare);
        }
    }
    
    void AddPropertyToRare(Item* item) {
        if (item->GetRarity() != ItemRarity::Rare) return;
        
        // Check if item has room for more properties
        if (item->GetPropertyCount() >= 6) {
            // Item already has maximum properties
            return;
        }
        
        // Add one random property
        item->AddRandomProperty(ItemRarity::Rare);
    }
    
    void CorruptItem(Item* item) {
        // Vaal Orb: High risk, high reward
        // Possible outcomes:
        // - Add powerful corruption implicit (good)
        // - Reroll all properties (neutral)
        // - Brick item (make useless) (bad)
        // - Do nothing (neutral)
        
        float roll = Random::Float();
        
        if (roll < 0.25f) {
            // Good outcome: Add corruption implicit
            item->AddCorruptionImplicit();
            item->SetCorrupted(true);
        } else if (roll < 0.50f) {
            // Neutral: Reroll properties
            RerollRareProperties(item);
            item->SetCorrupted(true);
        } else if (roll < 0.75f) {
            // Bad: Brick item
            item->SetBricked(true);
            item->SetCorrupted(true);
        } else {
            // Neutral: Nothing happens
            item->SetCorrupted(true);
        }
        
        // Once corrupted, item cannot be modified further
        // This is a major material sink!
    }
    
    // Trading currency for currency
    struct TradeOffer {
        CurrencyType offering;
        int offeringAmount;
        CurrencyType requesting;
        int requestingAmount;
        float exchangeRate; // Derived from market
    };
    
    bool ExchangeCurrency(
        Player* player,
        CurrencyType from,
        int fromAmount,
        CurrencyType to,
        int toAmount
    ) {
        // Verify player has source currency
        if (!player->HasCurrency(from, fromAmount)) {
            return false;
        }
        
        // Verify exchange rate is reasonable (prevent scams)
        float expectedRate = GetMarketExchangeRate(from, to);
        float offeredRate = static_cast<float>(fromAmount) / toAmount;
        
        if (std::abs(offeredRate - expectedRate) > expectedRate * 0.5f) {
            // Rate is more than 50% off market, warn player
            player->SendMessage("Warning: This exchange rate differs significantly " +
                "from market rates.");
        }
        
        // Execute exchange
        player->RemoveCurrency(from, fromAmount);
        player->AddCurrency(to, toAmount);
        
        // Log for economy tracking
        EconomyMetrics::RecordCurrencyExchange(from, to, fromAmount, toAmount);
        
        return true;
    }
    
private:
    std::map<CurrencyType, CurrencyConfig> configs;
};
```

### 2. Challenge Leagues - Seasonal Economy Resets

**The Challenge League Model:**

Every 3-4 months, PoE launches a new Challenge League with:
- Fresh economy (everyone starts with nothing)
- New experimental mechanics
- League-specific rewards
- Separate character progression
- After 3-4 months, characters merge to Standard (permanent) league

**Economic Benefits:**

1. **Fresh Start** - New players and veterans start equal
2. **Economic Health** - No accumulated inflation from years past
3. **Experimentation** - Test wild mechanics without risking Standard
4. **FOMO Marketing** - Limited-time leagues drive engagement
5. **Parallel Economies** - Standard and League coexist

**League Examples:**

**Harvest League (2020):**
- Mechanic: Deterministic crafting (plant seeds, get specific mods)
- Impact: Made perfect items too easy to craft
- Lesson: Too much determinism undermines loot value
- Result: Nerfed significantly, became controversial

**Delve League (2018):**
- Mechanic: Infinite dungeon with scaling rewards
- Impact: Created reliable grinding for specific items
- Lesson: Reliable sources must have caps or costs
- Result: Integrated into core game with modifications

**Metamorph League (2019):**
- Mechanic: Combine monster parts to create custom boss
- Impact: Players control reward types
- Lesson: Player agency in rewards drives engagement
- Result: Successful, integrated into core

**BlueMarble League System:**
```cpp
// Seasonal league system for economy resets
class SeasonalLeague {
public:
    enum class LeagueType {
        Standard,       // Permanent league
        Challenge,      // 3-month temporary league
        Hardcore,       // Permadeath variant
        Solo,           // Solo Self-Found (no trading)
        Event           // Short races (1-2 weeks)
    };
    
    struct LeagueConfig {
        std::string name;
        LeagueType type;
        time_t startTime;
        time_t endTime;
        int durationDays;
        
        // Experimental mechanics
        std::vector<std::string> uniqueMechanics;
        
        // Rewards
        std::vector<CosmeticReward> cosmeticRewards;
        
        // Economy separation
        bool separateEconomy;       // Own market, currency, items
        bool mergeToStandard;       // Characters move to Standard at end
    };
    
    struct LeagueProgress {
        int challengesCompleted;    // 40 challenges per league
        std::vector<RewardTier> unlockedRewards;
        int totalPlayTime;
        int charactersCreated;
    };
    
    // Create new challenge league
    void LaunchChallengeLeague(const LeagueConfig& config) {
        // Initialize separate economy
        CreateLeagueEconomy(config.name);
        
        // Reset player progress for league
        for (auto& player : Server::GetAllPlayers()) {
            player->CreateLeagueCharacter(config.name);
        }
        
        // Activate league mechanics
        ActivateMechanics(config.uniqueMechanics);
        
        // Schedule league end
        ScheduleLeagueEnd(config.endTime);
        
        // Announce launch
        Server::BroadcastMessage(
            "New Challenge League '" + config.name + "' is now live! " +
            "Fresh economy, new mechanics, exclusive rewards!"
        );
    }
    
    // End league and merge to Standard
    void EndChallengeLeague(const std::string& leagueName) {
        auto league = GetLeague(leagueName);
        
        // Merge all league characters to Standard
        for (auto& player : GetLeaguePlayers(leagueName)) {
            MergeCharacterToStandard(player);
        }
        
        // Merge league economy to Standard
        // Items flood Standard market (expected price drops)
        MergeLeagueMarket(leagueName, "Standard");
        
        // Award final rewards based on challenge completion
        AwardFinalRewards(leagueName);
        
        // Archive league data for analysis
        ArchiveLeagueMetrics(leagueName);
        
        Server::BroadcastMessage(
            "Challenge League '" + leagueName + "' has ended. " +
            "Characters have been moved to Standard."
        );
    }
    
    // Track league economy health
    struct LeagueEconomySnapshot {
        int totalPlayers;
        int activePlayers24h;
        int64_t totalCurrency;          // All currency items
        float averagePlayerWealth;
        std::map<CurrencyType, int> currencyDistribution;
        float chaosOrbValue;            // Price in basic currency
        float exaltedOrbValue;          // Price in Chaos Orbs
    };
    
    LeagueEconomySnapshot GetLeagueEconomy(const std::string& leagueName) {
        LeagueEconomySnapshot snapshot;
        
        auto players = GetLeaguePlayers(leagueName);
        snapshot.totalPlayers = players.size();
        
        // Count active players
        time_t cutoff = GameTime::Now() - 86400; // 24 hours
        for (const auto& player : players) {
            if (player->GetLastLoginTime() >= cutoff) {
                snapshot.activePlayers24h++;
            }
        }
        
        // Calculate total currency in league economy
        for (const auto& player : players) {
            for (auto currency : AllCurrencyTypes) {
                int amount = player->GetCurrencyAmount(currency);
                snapshot.currencyDistribution[currency] += amount;
                snapshot.totalCurrency += amount;
            }
        }
        
        // Calculate average wealth
        if (snapshot.totalPlayers > 0) {
            snapshot.averagePlayerWealth = 
                static_cast<float>(snapshot.totalCurrency) / snapshot.totalPlayers;
        }
        
        // Get currency exchange rates
        snapshot.chaosOrbValue = GetMarketRate(
            CurrencyType::BasicOrb,
            CurrencyType::ChaosOrb
        );
        snapshot.exaltedOrbValue = GetMarketRate(
            CurrencyType::ChaosOrb,
            CurrencyType::ExaltedOrb
        );
        
        return snapshot;
    }
    
private:
    std::map<std::string, LeagueConfig> activeLeagues;
};
```

### 3. No Auction House - Preserving Scarcity

**PoE's Trading Philosophy:**

Instead of an automated auction house, PoE uses:
- Player shops (list items, other players browse)
- Trade chat (direct player negotiation)
- Third-party websites (aggregate player listings)
- Manual meet-up (trade happens in-game instance)

**Why No Auction House?**

1. **Scarcity Preservation** - Friction in trading means items retain value
2. **Social Interaction** - Players negotiate, build relationships
3. **Time Investment** - Getting perfect gear requires effort
4. **Anti-Flipping** - Can't easily buy low, sell high with bots
5. **Economy Slower** - Deliberate friction prevents instant liquidity

**Controversies:**

- **Inconvenience** - Many players want auction house
- **Third-Party Dominance** - External sites become required
- **Trade Spam** - Trade chat is chaotic
- **Scams** - Manual trading enables scams

**GGG's Stance (Chris Wilson):**

> "An auction house would make trading too easy. When you can get any item instantly, items lose value. The journey to get an item is part of the reward."

**BlueMarble Hybrid Approach:**
```cpp
// Hybrid trading system: Convenience + Scarcity
class TradingSystem {
public:
    enum class TradeMethod {
        DirectTrade,        // Player-to-player manual
        ListedShop,         // Player shop (others browse)
        LocalMarket,        // Regional automated market
        GlobalMarket        // Global auction house (limited items)
    };
    
    // Player shop system (PoE-style)
    class PlayerShop {
    public:
        struct ShopListing {
            ItemID item;
            int quantity;
            CurrencyType requestedCurrency;
            int requestedAmount;
            std::string note;       // Negotiable, firm, etc.
            time_t listedTime;
        };
        
        void ListItemInShop(
            Player* seller,
            ItemID item,
            CurrencyType currency,
            int amount,
            const std::string& note = ""
        ) {
            // Verify seller has item
            if (!seller->HasItem(item, 1)) {
                return;
            }
            
            // Create listing
            ShopListing listing;
            listing.item = item;
            listing.quantity = 1;
            listing.requestedCurrency = currency;
            listing.requestedAmount = amount;
            listing.note = note;
            listing.listedTime = GameTime::Now();
            
            // Add to player's shop
            seller->GetShop().AddListing(listing);
            
            // Make discoverable by trade system
            TradeSearchIndex::Instance().IndexListing(seller->GetID(), listing);
        }
        
        // Browse player shops
        std::vector<ShopListing> SearchShops(
            ItemID item,
            CurrencyType maxCurrency = CurrencyType::Any,
            int maxAmount = INT_MAX
        ) {
            std::vector<ShopListing> results;
            
            // Query trade index
            auto listings = TradeSearchIndex::Instance().SearchItem(item);
            
            // Filter by price
            for (const auto& listing : listings) {
                if (listing.requestedCurrency == maxCurrency &&
                    listing.requestedAmount <= maxAmount) {
                    results.push_back(listing);
                }
            }
            
            // Sort by price (lowest first)
            std::sort(results.begin(), results.end(),
                [](const ShopListing& a, const ShopListing& b) {
                    return a.requestedAmount < b.requestedAmount;
                }
            );
            
            return results;
        }
        
        // Send trade request to seller
        void SendTradeRequest(Player* buyer, Player* seller, const ShopListing& listing) {
            // Create trade request notification
            TradeRequest request;
            request.buyer = buyer->GetID();
            request.seller = seller->GetID();
            request.listing = listing;
            
            // Notify seller
            seller->ReceiveTradeRequest(request);
            
            // Seller must accept and meet buyer in-game
            // This preserves social aspect and prevents botting
        }
    };
    
    // Regional market (convenience for common items)
    class RegionalMarket {
    public:
        // Only certain item types allowed in automated market
        bool IsMarketableItem(ItemID item) {
            // Allow: Raw materials, common crafting supplies
            // Disallow: Rare equipment, unique items
            // This preserves value of special items
            
            auto category = GetItemCategory(item);
            return category == ItemCategory::RawMaterial ||
                   category == ItemCategory::CommonCraftingMaterial;
        }
        
        // Post to regional automated market (like Grand Exchange, but limited)
        void PostToRegionalMarket(
            Player* seller,
            ItemID item,
            int quantity,
            int pricePerUnit
        ) {
            if (!IsMarketableItem(item)) {
                seller->SendMessage("This item cannot be sold on the regional market. " +
                    "Please use player shops for rare items.");
                return;
            }
            
            // Standard Grand Exchange logic for common items
            // Preserves convenience for bulk materials
            // Maintains scarcity for valuable items
        }
    };
    
private:
    PlayerShop playerShops;
    RegionalMarket regionalMarkets;
};
```

### 4. Loot Abundance with Filters

**PoE's Loot Philosophy:**

- Drop tons of loot (satisfying)
- Let players filter what they see (customizable)
- Don't reduce drops (feels bad)
- Give control to players (empowering)

**Item Filter System:**

Players write filter scripts that:
- Hide items below certain value
- Highlight valuable items
- Play sounds for rare drops
- Resize text for important items
- Color-code by item type

**Example Filter Rule:**
```
Show # Valuable currencies
    Class "Currency"
    BaseType "Exalted Orb" "Mirror of Kalandra"
    SetFontSize 45
    SetTextColor 255 255 255
    SetBackgroundColor 255 0 0
    PlayAlertSound 1 300
```

**Economic Benefits:**

- Loot still drops (economy not affected)
- Player experience improved (less clutter)
- No power advantage (everyone can use filters)
- Customizable (casual vs hardcore players)

**BlueMarble Loot Filter:**
```cpp
// Client-side loot filtering system
class LootFilterSystem {
public:
    enum class FilterAction {
        Show,       // Display item normally
        Hide,       // Don't display item
        Highlight,  // Emphasize item
        Alert       // Play sound + emphasize
    };
    
    struct FilterRule {
        // Matching criteria
        ItemCategory category;
        ItemRarity minRarity;
        int minValue;
        std::vector<std::string> baseTypes;
        
        // Actions
        FilterAction action;
        Color textColor;
        Color backgroundColor;
        int fontSize;
        std::string soundEffect;
    };
    
    class ItemFilter {
    public:
        void LoadFilterScript(const std::string& scriptPath) {
            // Parse user filter script
            auto rules = ParseFilterScript(scriptPath);
            filterRules = rules;
        }
        
        // Check if item should be displayed
        FilterAction EvaluateItem(const Item* item) {
            // Apply rules in order
            for (const auto& rule : filterRules) {
                if (RuleMatches(rule, item)) {
                    return rule.action;
                }
            }
            
            // Default: show item
            return FilterAction::Show;
        }
        
        // Client-side rendering
        void RenderLootDrop(const Item* item, const Vector3& position) {
            auto action = EvaluateItem(item);
            
            switch (action) {
                case FilterAction::Hide:
                    // Don't render at all
                    return;
                    
                case FilterAction::Show:
                    // Standard rendering
                    RenderItemLabel(item, position);
                    break;
                    
                case FilterAction::Highlight:
                    // Emphasize visually
                    RenderItemLabel(item, position, true);
                    DrawHighlightBeam(position);
                    break;
                    
                case FilterAction::Alert:
                    // Maximum emphasis
                    RenderItemLabel(item, position, true);
                    DrawHighlightBeam(position);
                    PlayAlertSound(item);
                    break;
            }
        }
        
    private:
        std::vector<FilterRule> filterRules;
    };
};
```

---

## Key Lessons for BlueMarble

### 1. Currency Must Have Intrinsic Value

**PoE Proves:**
- Currency as consumable crafting materials works
- Dual purpose (crafting + trading) creates natural sinks
- No gold = no traditional inflation
- Scarcity matters because currency is consumed

**BlueMarble Application:**
- Design currency items that modify/improve equipment
- Make currency stackable but consumable
- Balance drop rates against consumption rates
- Allow currency-for-currency exchanges

### 2. Seasonal Resets Maintain Freshness

**PoE Shows:**
- 3-month leagues keep players engaged
- Fresh economies prevent stagnation
- Experimental mechanics test new ideas safely
- FOMO drives player return

**BlueMarble Strategy:**
- Implement regional "seasons" with fresh economies
- Test new mechanics in limited-time events
- Merge seasonal progress to main world after event
- Award cosmetics for seasonal participation

### 3. Trading Friction Preserves Value

**PoE's Philosophy:**
- No auction house = items retain scarcity
- Manual trading = social interaction
- Time investment = gear feels earned
- Imperfect market = opportunities for traders

**BlueMarble Hybrid:**
- Automated market for common materials
- Player shops for rare/unique items
- Manual negotiation for high-value trades
- Regional markets (not global) preserve geography

### 4. Loot Filters Improve UX Without Harming Economy

**PoE Innovation:**
- Client-side filtering (no economy impact)
- Customizable by player (accessibility)
- Empowers players (control experience)
- Doesn't reduce drops (psychological win)

**BlueMarble Must-Have:**
- Implement client-side loot filters
- Provide default filters for different playstyles
- Allow custom filter scripts
- Filter doesn't affect drop rates (parity)

---

## Discovered Sources for Phase 4

1. **"Currency Design in Path of Exile"** - Deep dive on dual-purpose currency
2. **"Challenge League Post-Mortems"** - What worked/failed in each league
3. **"Trading Without Auction Houses"** - Analysis of friction in markets
4. **"Loot Filter Psychology"** - UX research on customization
5. **"Harvest League Controversy"** - Deterministic crafting debate
6. **"Economy Reset Strategies"** - Seasonal vs permanent leagues

---

## Cross-References

**Related Research:**
- `game-dev-analysis-economics-of-mmorpgs-gdc.md` - Economy fundamentals (Source 1)
- `game-dev-analysis-runescape-economic-system.md` - Grand Exchange comparison (Source 2)
- Upcoming: Albion Online full-loot economy (Source 4)
- Upcoming: WoW auction house dynamics (Source 5)

**BlueMarble Systems:**
- Currency and crafting design
- Seasonal event system
- Trading and market UI
- Loot filter implementation
- Item modification mechanics

---

## Conclusion

Path of Exile revolutionized ARPG economies with three key innovations:

1. **Currency as crafting materials** - Dual purpose prevents inflation
2. **Challenge leagues** - Fresh starts maintain long-term engagement  
3. **Trading friction** - Preserves item value and social aspects

Combined with loot filters for UX, PoE shows that economy health and player convenience aren't mutually exclusive. BlueMarble should adopt these proven patterns while adapting for persistent world gameplay.

---

**Document Statistics:**
- Lines: 700+
- Core Innovations: 7
- Code Examples: 5
- Cross-References: 4
- Discovered Sources: 6
- BlueMarble Applications: 12+

**Research Time:** 4 hours  
**Completion Date:** 2025-01-17  
**Next Source:** Albion Online: Player-Driven Economy Design
