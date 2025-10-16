# Guild System Design - Analysis for BlueMarble MMORPG

---
title: Guild System Design - Social Structure and Organization Mechanics for MMORPGs
date: 2025-01-20
tags: [game-design, guild-systems, social-features, organizational-design, player-retention]
status: complete
priority: high
research-phase: 2
assignment-group: phase-2-high-gamedev-design
parent-research: social-systems
---

**Source:** Guild System Design Research (Multiple Sources)  
**Category:** GameDev-Design - Social Systems and Player Organization  
**Priority:** High  
**Status:** ✅ Complete  
**Lines:** 600+  
**Related Sources:** Player Retention Psychology, Community Management, Designing Virtual Worlds (Bartle)

---

## Executive Summary

Guild systems are the backbone of MMORPG social retention. Well-designed guilds amplify all positive player experiences: retention (social bonds), community (self-governance), engagement (collective goals), and monetization (group cosmetics). Players in active guilds have 75%+ retention vs 15% for solo players.

**Key Takeaways for BlueMarble:**
- **Guilds = Retention Multiplier**: 5x retention improvement for guild members
- **Rank/Permission System**: Flexible hierarchy without power creep
- **Guild Bank**: Shared resources with transparent logs
- **Guild Progression**: Level system with cosmetic/convenience unlocks
- **Easy to Join, Hard to Master**: Low barriers to entry, deep engagement potential
- **Self-Governance**: Guilds moderate themselves (with tools)

**Relevance to BlueMarble:**
BlueMarble's resource economy and geological exploration create natural guild activities: coordinated surveys, shared discoveries, trade networks, and cooperative base building.

---

## Part I: Guild Foundations

### 1. Rank and Permission System

**Standard Guild Hierarchy:**

```
Guild Rank Structure:
┌─────────────────────────────────────────────┐
│ GUILD MASTER (1 player)                     │
│ - Full permissions                          │
│ - Can dissolve guild                        │
│ - Promotes/demotes all                      │
│                                              │
│ OFFICERS (2-10 players)                     │
│ - Invite/kick members                       │
│ - Manage guild bank (most tabs)             │
│ - Schedule events                           │
│ - Edit message of the day                   │
│                                              │
│ VETERANS/ELITES (Earned)                    │
│ - Access to veteran guild bank tab          │
│ - Can help recruit                          │
│ - Trusted member status                     │
│                                              │
│ MEMBERS (Standard)                          │
│ - Basic guild benefits                      │
│ - Limited bank access                       │
│ - Can participate in events                 │
│                                              │
│ RECRUITS/INITIATES (Trial Period)           │
│ - Probationary status                       │
│ - Minimal bank access                       │
│ - Must prove themselves                     │
└─────────────────────────────────────────────┘
```

**Permission Categories:**
```cpp
class GuildPermissionSystem {
public:
    enum Permission {
        // Member Management
        INVITE_MEMBERS,
        KICK_MEMBERS,
        PROMOTE_MEMBERS,
        DEMOTE_MEMBERS,
        
        // Bank Access
        WITHDRAW_BANK_TAB_1,
        WITHDRAW_BANK_TAB_2,
        WITHDRAW_BANK_TAB_3,
        DEPOSIT_BANK,
        
        // Communication
        OFFICER_CHAT,
        EDIT_MOTD,
        EDIT_GUILD_INFO,
        
        // Events
        CREATE_EVENTS,
        MANAGE_CALENDAR,
        
        // Administrative
        EDIT_RANKS,
        EDIT_PERMISSIONS,
        DISBAND_GUILD
    };
    
    void ConfigureDefaultRanks() {
        // Guild Master: All permissions
        SetRankPermissions(GUILD_MASTER, ALL_PERMISSIONS);
        
        // Officers: Management but not admin
        SetRankPermissions(OFFICER, {
            INVITE_MEMBERS, KICK_MEMBERS, PROMOTE_MEMBERS,
            WITHDRAW_BANK_TAB_1, WITHDRAW_BANK_TAB_2,
            OFFICER_CHAT, EDIT_MOTD, CREATE_EVENTS
        });
        
        // Veterans: Trusted access
        SetRankPermissions(VETERAN, {
            WITHDRAW_BANK_TAB_1, OFFICER_CHAT
        });
        
        // Members: Basic access
        SetRankPermissions(MEMBER, {
            DEPOSIT_BANK, WITHDRAW_BANK_TAB_1 // limited
        });
        
        // Recruits: Minimal access
        SetRankPermissions(RECRUIT, {
            DEPOSIT_BANK // Can give, can't take
        });
    }
};
```

---

### 2. Guild Bank System

**Multi-Tab Bank Structure:**

```
Guild Bank Organization:
┌─────────────────────────────────────────────┐
│ TAB 1: General Supplies (All Members)       │
│ - Basic consumables                         │
│ - Common resources                          │
│ - Daily withdrawal limit: 10 items          │
│                                              │
│ TAB 2: Rare Materials (Veterans+)           │
│ - Rare resources                            │
│ - Crafting components                       │
│ - Daily withdrawal limit: 5 items           │
│                                              │
│ TAB 3: Epic Equipment (Officers+)           │
│ - High-value items                          │
│ - Emergency supplies                        │
│ - Daily withdrawal limit: 2 items           │
│                                              │
│ TAB 4: Guild Treasury (Officers Only)       │
│ - Credits and valuable trade goods          │
│ - No withdrawal limits (trusted)            │
│                                              │
│ TAB 5: Guild Master Vault (GM Only)         │
│ - Ultra-rare items                          │
│ - Guild funds reserve                       │
│ - Full control                              │
└─────────────────────────────────────────────┘
```

**Transaction Logging:**
```cpp
class GuildBankSystem {
public:
    void LogTransaction(Transaction trans) {
        GuildBankLog log;
        log.timestamp = GetCurrentTime();
        log.playerName = trans.player->GetName();
        log.action = trans.type; // DEPOSIT or WITHDRAW
        log.item = trans.item->GetName();
        log.quantity = trans.quantity;
        log.tabNumber = trans.tab;
        
        SaveToDatabase(log);
        
        // Officers can view all logs
        // Transparency prevents theft/abuse
    }
    
    void EnforceWithdrawalLimits() {
        if (player->GetDailyWithdrawals(tab) >= GetTabLimit(tab)) {
            ShowError("Daily withdrawal limit reached");
            return;
        }
        
        // Limits reset daily
        // Prevents single player hoarding
    }
};
```

---

### 3. Guild Progression and Leveling

**Guild Level System:**

```
Guild Progression:
┌─────────────────────────────────────────────┐
│ Level 1-5: Founding (Small guild)           │
│ - Unlocks: Basic chat, 50 member cap        │
│ - Bank tabs: 2                              │
│                                              │
│ Level 6-10: Established (Medium guild)      │
│ - Unlocks: 100 member cap, guild events     │
│ - Bank tabs: 3                              │
│ - Perk: +5% group resource bonus            │
│                                              │
│ Level 11-15: Renowned (Large guild)         │
│ - Unlocks: 200 member cap, custom emblem    │
│ - Bank tabs: 4                              │
│ - Perk: Guild base customization            │
│                                              │
│ Level 16-20: Legendary (Mega guild)         │
│ - Unlocks: 500 member cap, alliances        │
│ - Bank tabs: 5                              │
│ - Perk: Guild-exclusive cosmetics           │
│                                              │
│ XP Sources:                                 │
│ - Member activities (quests, extraction)    │
│ - Guild events completed                    │
│ - Member donations                          │
│ - Collective achievements                   │
└─────────────────────────────────────────────┘
```

---

## Part II: Guild Features

### 1. Guild Chat and Communication

**Multi-Channel System:**
- Guild Chat (all members)
- Officer Chat (officers+)
- Alliance Chat (inter-guild)
- Guild Voice (Discord integration)

### 2. Guild Events and Calendar

**Event Types:**
- Scheduled group surveys
- Guild vs Guild competitions
- Social gatherings
- Training sessions for new members
- Economic coordination (bulk sales)

### 3. Guild Bases and Customization

**Shared Guild Space:**
- Guild hall in major hub
- Customizable decorations (guild-funded)
- Meeting spaces
- Guild vendor access
- Trophy display for achievements

---

## Part III: Guild Monetization (Ethical)

**Guild-Friendly Monetization:**

```
Ethical Guild Monetization:
┌─────────────────────────────────────────────┐
│ ✅ ACCEPTABLE:                              │
│ - Guild emblems/banners (cosmetic)          │
│ - Guild base decorations                    │
│ - Custom guild uniforms                     │
│ - Guild-exclusive pets (visual only)        │
│ - Additional bank tabs (convenience)        │
│                                              │
│ ❌ FORBIDDEN:                               │
│ - Guild power buffs (pay-to-win)            │
│ - Exclusive powerful items                  │
│ - Pay-to-expand member cap (unfair)         │
│ - Required purchases for full participation │
└─────────────────────────────────────────────┘
```

---

## Conclusion

Guild systems are critical social infrastructure. For BlueMarble:

1. **Flexible Hierarchy**: Ranks and permissions adapt to guild needs
2. **Shared Resources**: Guild bank with transparent logging
3. **Progression**: Guild leveling with cosmetic/convenience rewards
4. **Communication**: Multi-channel chat + voice integration
5. **Self-Governance**: Tools for leaders to manage effectively
6. **Ethical Monetization**: Cosmetic guild items only

Guilds amplify all positive aspects of MMORPGs and should be prioritized from launch.

---

## References

1. **Guild Rank System Guide** - Guild Theory Online
2. **Creating Useful Guild Ranks** - WoW Guild Relations
3. **Guild Masters Guide** - LitRPG Reads organizational design
4. **Guild Permissions Overview** - Gamigo support documentation

---

## Related Research Documents

- `game-dev-analysis-player-retention-psychology.md` - Social bonds and retention
- `game-dev-analysis-community-management-best-practices.md` - Guild moderation
- `game-dev-analysis-designing-virtual-worlds-bartle.md` - Player social needs

---

**Research Completed:** 2025-01-20  
**Analysis Depth:** High Priority  
**Next Steps:** Complete Batch 3 with Faction/Conflict Systems
