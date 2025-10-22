# Auction Tier System Diagram

Visual representation of the three-tier auction system for BlueMarble.

## Tier Hierarchy

```text
┌────────────────────────────────────────────────────────────────────┐
│                        GLOBAL AUCTION HOUSES                        │
│                                                                      │
│  Location: Major Trade Capitals Only                                │
│  Commission: 7% base (up to 14% for experimental races)            │
│  Listing Fee: 2%                                                    │
│  Reach: Worldwide                                                   │
│  Transport: Long-distance, high cost, high risk                    │
│                                                                      │
│  ┌────────────────────────────────────────────────────────────┐   │
│  │ • Rare/luxury goods      • Strategic resources             │   │
│  │ • Intercontinental trade • Premium security available      │   │
│  └────────────────────────────────────────────────────────────┘   │
└────────────────────────────────────────────────────────────────────┘
                               ▲
                               │ Escalation
                               │
┌────────────────────────────────────────────────────────────────────┐
│                       REGIONAL AUCTION HOUSES                       │
│                                                                      │
│  Location: Major Cities                                             │
│  Commission: 3%                                                     │
│  Listing Fee: 1%                                                    │
│  Reach: Multiple Settlements                                        │
│  Transport: Medium distance, moderate cost/risk                    │
│                                                                      │
│  ┌────────────────────────────────────────────────────────────┐   │
│  │ • Regional specialization • Cross-settlement arbitrage     │   │
│  │ • Medium-value goods      • Inter-city trading             │   │
│  └────────────────────────────────────────────────────────────┘   │
└────────────────────────────────────────────────────────────────────┘
                               ▲
                               │ Escalation
                               │
┌────────────────────────────────────────────────────────────────────┐
│                        LOCAL AUCTION HOUSES                         │
│                                                                      │
│  Location: Most Settlements                                         │
│  Commission: 1.5%                                                   │
│  Listing Fee: 0.5%                                                  │
│  Reach: Local Community                                             │
│  Transport: None (same settlement)                                  │
│                                                                      │
│  ┌────────────────────────────────────────────────────────────┐   │
│  │ • Common goods           • Quick trades                     │   │
│  │ • Perishable items       • Player-to-player commerce        │   │
│  └────────────────────────────────────────────────────────────┘   │
└────────────────────────────────────────────────────────────────────┘
```

## Fee Comparison

```text
                 ┌────────────┬──────────┬──────────┬────────────┐
                 │ Local      │ Regional │ Global   │            │
─────────────────┼────────────┼──────────┼──────────┼────────────┤
Commission Rate  │   1.5%     │   3%     │   7%     │  ▲ Higher  │
Listing Fee      │   0.5%     │   1%     │   2%     │  │ Fees    │
Transport Fee    │   None     │ 0.001/km │ 0.002/km │            │
Guard Fee        │   None     │   0.5%   │   1%     │            │
─────────────────┼────────────┼──────────┼──────────┼────────────┤
Buyer Reach      │   Local    │ Regional │ Global   │  ▲ Greater │
Visibility       │  Low       │ Medium   │ High     │  │ Reach   │
Market Liquidity │  Low       │ Medium   │ High     │            │
─────────────────┴────────────┴──────────┴──────────┴────────────┘
```

## Transport Flow

```text
Seller (Node A)                                           Buyer (Node B)
     │                                                         │
     │ 1. List Item                                           │
     ▼                                                         │
┌─────────────┐                                               │
│  Local AH   │                                               │
│  (Node A)   │                                               │
└─────────────┘                                               │
     │                                                         │
     │ 2. Escalate to Regional/Global                         │
     ▼                                                         │
┌─────────────┐                                               │
│ Regional/   │                                               │
│ Global AH   │◀──────── 3. Search & Purchase ────────────────┤
└─────────────┘                                               │
     │                                                         │
     │ 4. Initiate Transport                                  │
     ▼                                                         │
┌─────────────────────────────────────────────────────────────┐
│                      TRANSPORT LAYER                         │
│  • Select Method (cart, ship, airship)                      │
│  • Choose Guard Level (none, basic, standard, premium)      │
│  • Calculate Cost & Risk                                    │
│  • Optional: Purchase Insurance                             │
└─────────────────────────────────────────────────────────────┘
     │                                                         │
     │ 5. Physical Movement (time-based)                      │
     ▼                                                         ▼
[Node A] ─────────────────────────────────────────────────▶ [Node B]
  Origin     Distance, Terrain, Weather, Risk              Destination
     │                                                         │
     │ 6. Item Arrives (or Lost if ambushed)                 │
     └─────────────────────────────────────────────────────────▶
                                                          Buyer receives item
```

## Regional Specialization Flow

```text
┌──────────────┐      Ore, Gems      ┌──────────────┐
│   Mountain   │◀────────────────────▶│   Coastal    │
│  Settlement  │      Fish, Salt      │  Settlement  │
└──────────────┘                      └──────────────┘
       ▲                                      ▲
       │                                      │
Timber, Furs                          Grain, Leather
       │                                      │
       ▼                                      ▼
┌──────────────┐      Tools, Metal   ┌──────────────┐
│    Forest    │◀────────────────────▶│   Farming    │
│  Settlement  │     Herbs, Food      │   Village    │
└──────────────┘                      └──────────────┘

Each settlement exports what it has in abundance
and imports what it lacks, creating natural trade routes.
```

## Seasonal Route Availability

```text
Season: Spring
┌────────┐       ┌────────┐       ┌────────┐
│ City A │───────│ City B │───────│ City C │
└────────┘       └────────┘       └────────┘
    │                │                 │
 Mountain      River Trade         Sea Route
  (Closed)       (OPEN)             (OPEN)

Season: Winter
┌────────┐       ┌────────┐       ┌────────┐
│ City A │  ╳    │ City B │  ╳    │ City C │
└────────┘       └────────┘       └────────┘
    │                │                 │
 Mountain      River Trade         Sea Route
  (Closed)      (FROZEN)           (FROZEN)
    │                │                 │
    └────────────────┴─────────────────┘
           Land Routes Only
```

## Related Documentation

- [Auction Economy Design](../../../design/auction-economy.md)
- [Marketplace System Architecture](README.md)
