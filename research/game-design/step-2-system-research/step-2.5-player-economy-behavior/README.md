# Step 2.5: Player Economy Behavior

## Overview

This research area focuses on understanding player economic behavior patterns in virtual economies, specifically how players make pricing decisions, value their time investment, and interact with market systems. Understanding these behaviors is critical for designing sustainable player-driven economies that respect player time while maintaining healthy competition.

## Research Content

- [Player Task Pricing Behavior Research](player-task-pricing-behavior-research.md) - Comprehensive analysis of whether players set higher rewards for time-consuming tasks or undercut market pricing

## Key Findings

### Player Pricing Behavior Patterns

- **Dual Behavior**: Players exhibit both time-valuing and undercutting behaviors depending on experience, context, and market knowledge
- **Experience Matters**: Veteran players (200+ hours) tend to value time appropriately, while new players often undercut due to lack of knowledge
- **Context Dependent**: High-skill items see premium pricing, while common items face aggressive undercutting
- **Market Structure Impact**: Quality differentiation, skill barriers, and market tools significantly influence pricing sustainability

### Critical Success Factors

- **Market Information Tools**: Price history, cost calculators, and profit tracking lead to more educated pricing
- **Quality-Based Segmentation**: Prevents direct competition between skill levels and supports premium pricing
- **Adequate Storage**: Reduces inventory pressure that drives destructive undercutting
- **Skill Barriers**: Specialization requirements enable sustainable pricing for specialists
- **Commission Systems**: Allow premium pricing for custom work and expertise

### Time Investment Recognition

- **Geological Depth Value**: Deep mining and exploration must be more rewarding per hour than surface activities
- **Skill Development Rewards**: Time invested in specialization should enable premium pricing
- **Quest Reward Scaling**: Rewards must reflect actual time investment plus reasonable profit margin
- **Opportunity Cost Awareness**: Experienced players calculate opportunity cost; game should help newer players learn this

## BlueMarble Integration Opportunities

### Quality-Based Market Design

Implement Wurm-style quality tiers that create distinct market segments:
- **Entry Market** (QL 20-40): High competition, learning space for new players
- **Standard Market** (QL 41-70): Main economy, competitive but profitable
- **Premium Market** (QL 71-90): Specialist domain with premium pricing
- **Master Market** (QL 91-100): Luxury goods, highest premiums

Benefits: Prevents race to bottom, rewards skill investment, creates multiple viable strategies

### Commission System Implementation

Design a request board system where:
- Players post custom item requests with budgets
- Crafters bid competitively or accept commissions
- Quality guarantees command premium pricing
- Reputation system enables trusted crafters to charge more
- Escrow system protects both parties

Benefits: Allows time-appropriate pricing, reduces inventory pressure, rewards expertise

### Market Tools and Education

Provide comprehensive economic tools:
- Price history charts and trend analysis
- Production cost calculators
- Profit margin tracking
- Opportunity cost comparison
- In-game tutorials on economic thinking

Benefits: Educates players, reduces uninformed undercutting, creates market efficiency

### Geological Value Recognition

Ensure time investment in geological exploration pays off:
- **Deep deposits**: Higher quality than surface deposits
- **Discovery bonuses**: First-finder rewards and reputation
- **Quality premiums**: Market naturally values high-quality materials
- **Processing efficiency**: High-quality materials yield better results
- **Exclusive access**: Skill barriers limit competition

Benefits: Encourages engagement with geological depth, respects exploration time

## Anti-Patterns to Avoid

### Destructive Design Choices

❌ **Short Listing Durations**: Creates urgency and panic selling (24-48 hours)  
✅ **Solution**: Long durations (30+ days) or no expiration

❌ **Limited Storage**: Forces inventory dumping at any price  
✅ **Solution**: Adequate bank/warehouse capacity for crafters

❌ **No Market Information**: Leads to uninformed, random pricing  
✅ **Solution**: Comprehensive price history and cost tools

❌ **Low Skill Barriers**: Everyone can make everything, oversupply  
✅ **Solution**: Specialization requirements and skill caps

❌ **No Quality Differentiation**: All items identical, pure price competition  
✅ **Solution**: Quality-based tiers with significant differences

❌ **Aggressive NPC Competition**: Undercuts player economy  
✅ **Solution**: NPCs provide price floors only, not ceiling competition

## Performance Targets

### Healthy Economy Metrics

```
Target Economic Health Indicators:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Profitability:
  ├── 70%+ of sales above (cost + time value)
  ├── Average markup on time-intensive items: 30%+
  └── Specialist premium: 50-100% above generalist pricing

Stability:
  ├── Daily price volatility: <15%
  ├── Market participation: 40%+ of players
  └── Repeat sellers: 60%+ monthly activity

Time Value Recognition:
  ├── Deep mining: 20%+ more gold/hour than surface
  ├── Specialist work: 50%+ premium over standard
  └── Commission pricing: 20-50% above open market
```

## Implementation Considerations

### Phase 1: Foundation (Alpha)
- Basic marketplace with price history
- Quality-based item differentiation
- Simple NPC price floors
- Adequate storage capacity
- Cost calculation tutorials

### Phase 2: Advanced Systems (Beta)
- Commission board implementation
- Crafter reputation system
- Dynamic quest rewards
- Market analytics dashboard
- Anti-manipulation safeguards

### Phase 3: Refinement (Launch)
- Advanced market analytics
- Regional market variations
- Guild economic systems
- Community pricing standards
- Long-term economic monitoring

## Related Research

### Upstream Dependencies
- [Step 2.2: Material Systems](../step-2.2-material-systems/) - Quality mechanics that enable market segmentation
- [Step 2.3: Crafting Systems](../step-2.3-crafting-systems/) - Time investment in crafting that needs appropriate reward

### Related Literature
- [Virtual Economies: Design and Analysis](../../../literature/game-dev-analysis-virtual-economies-design-and-analysis.md)
- [Virtual Worlds: Cyberian Frontier](../../../literature/game-dev-analysis-virtual-worlds-cyberian-frontier.md)
- [Wurm Online Material System](../step-2.2-material-systems/wurm-online-material-system-research.md)

### Downstream Impact
- Quest reward design must respect player time investment
- Marketplace features need price history and calculation tools
- Skill system should create meaningful specialization barriers
- Storage systems must support economic gameplay

## Summary

Player economic behavior research reveals that pricing decisions are not binary (time-value vs undercutting) but exist on a spectrum influenced by experience, market structure, and game design. By implementing quality differentiation, providing market tools, ensuring adequate storage, and creating skill barriers, BlueMarble can foster an economy where players naturally value their time investment while maintaining healthy competition.

The key insight: **The best economies don't force behavior - they create conditions where rational pricing emerges naturally.**

---

**Research Status:** Complete  
**Priority:** High  
**Applicability:** Direct impact on marketplace, quest rewards, crafting systems, and geological gameplay value
