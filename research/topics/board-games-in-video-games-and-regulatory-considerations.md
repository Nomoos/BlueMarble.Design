# Board Games in Video Games and Regulatory Considerations

---
title: Board Games in Video Games and Regulatory Considerations
date: 2025-01-20
owner: @copilot
status: complete
tags: [game-design, mini-games, board-games, regulatory, cryptocurrency, gambling-law]
---

## Problem / Context

How can traditional board games (Chess, Shogi, Go, and similar abstract strategy games) be 
incorporated into video games as mini-games or social activities? What are the regulatory 
implications when these games involve cryptocurrency rewards (ETH/BTC)? This research 
examines game rules, implementation examples, and legal considerations.

- Traditional board games provide engaging social activities in MMORPGs
- In-game mini-games enhance player social interaction and downtime activities
- Cryptocurrency integration raises gambling and regulatory concerns
- Need to understand legal boundaries for reward systems

## Key Findings

- **Chess, Shogi, and Go in Games**: These abstract strategy games are commonly implemented as 
  tavern games, social activities, or strategic mini-games in RPGs and MMORPGs
  - Witcher 3's Gwent became so popular it spawned a standalone game
  - Final Fantasy XIV features Triple Triad, Mahjong, and Lord of Verminion
  - Red Dead Redemption 2 includes Poker, Blackjack, and Dominoes
  - Ancient board games provide authentic historical flavor in period games

- **Kivas/Mancala Games**: Lesser known in video games but excellent for implementation
  - Simple rules with deep strategic gameplay
  - Cultural authenticity for historical/cultural settings
  - Easy to implement with visual feedback
  - Short game sessions (5-15 minutes)

- **Regulatory Considerations**: Cryptocurrency rewards create significant legal complexity
  - Games of skill vs. games of chance have different legal classifications
  - Most jurisdictions regulate gambling involving real money or cryptocurrency
  - "Play-to-earn" models face increasing regulatory scrutiny
  - Implementation must carefully avoid gambling classifications

## Evidence

### Source 1: Board Games in Video Games

- **Examples of Implementation**:
  - The Witcher 3: Wild Hunt - Gwent (collectible card game)
  - Final Fantasy XIV - Triple Triad, Mahjong, Lord of Verminion
  - Kingdom Come: Deliverance - Dice games, Farkle
  - Assassin's Creed Valhalla - Orlog (dice game), Flyting
  - Red Dead Redemption 2 - Poker, Blackjack, Dominoes, Five Finger Fillet
  - Fable 2 & 3 - Pub Games
  - The Elder Scrolls Online - Tales of Tribute (deck-building card game)

- **Key Implementation Patterns**:
  - Tavern/social hub locations for player interaction
  - In-game currency rewards (not real money)
  - Collectible elements (cards, pieces) as progression system
  - Tournament structures for competitive play
  - Single-player against NPCs and multiplayer against players
  - Achievements and rankings for engagement

- **Relevance**: Demonstrates proven patterns for implementing traditional games within MMORPGs, 
  providing social content and alternate progression paths

### Source 2: Chess Rules and Implementation

- **Basic Rules**:
  - 8x8 board with alternating colored squares
  - 16 pieces per player: King, Queen, 2 Rooks, 2 Knights, 2 Bishops, 8 Pawns
  - Each piece type moves differently (Knights in L-shape, Bishops diagonally, etc.)
  - Objective: Checkmate opponent's King
  - Special moves: Castling, En passant, Pawn promotion

- **Video Game Implementations**:
  - Pure Chess (2012) - Photorealistic 3D chess
  - Chessmaster series - Tutorials and AI opponents
  - Chess.com - Online multiplayer with millions of players
  - Auto Chess genre - Chess-inspired tactical battlers (not traditional chess)

- **Implementation Complexity**: Medium
  - Clear rule set, well-documented
  - Complex AI requires significant development (minimax, alpha-beta pruning)
  - Can use existing chess engines (Stockfish, open-source)
  - Visual representation straightforward
  - Universal recognition - no tutorial needed for most players

- **Relevance**: Chess is universally recognized and can be implemented with existing open-source 
  engines, reducing development complexity while providing high-quality AI opponents

### Source 3: Shogi Rules and Implementation

- **Basic Rules**:
  - 9x9 board with 20 pieces per player
  - Similar to chess but pieces are not colored (orientation shows ownership)
  - Captured pieces can be "dropped" back on the board for the capturing player
  - Promotion zone: Last three ranks promote pieces
  - More complex than chess due to drop mechanic

- **Cultural Significance**:
  - Traditional Japanese board game with 500+ year history
  - Professional shogi players in Japan similar to professional chess players
  - Featured in Japanese media and culture

- **Video Game Examples**:
  - Yakuza series - Playable shogi mini-game
  - Animal Crossing - Shogi pieces as collectible furniture
  - Pokémon Conquest - Inspired by shogi mechanics
  - 81 Dojo - Online shogi platform

- **Implementation Complexity**: High
  - Less familiar to Western audiences (requires tutorial)
  - Drop mechanic adds strategic depth and complexity
  - AI development more complex than chess
  - Piece recognition requires clear visual design

- **Relevance**: Provides authentic Japanese cultural content, excellent for games with 
  Asian settings or themes, but requires more player education than chess

### Source 4: Go (Baduk/Weiqi) Rules and Implementation

- **Basic Rules**:
  - 19x19 grid (also 9x9 and 13x13 for beginners)
  - Players place stones on intersections, not squares
  - Capture enemy stones by surrounding them
  - Territory control - most territory wins
  - Extremely simple rules, extraordinarily deep strategy

- **Complexity Paradox**:
  - Simplest rules of major board games
  - Most complex strategic depth (more possible positions than atoms in universe)
  - AI was unable to beat top humans until AlphaGo in 2016
  - Learning curve is very steep for mastery

- **Video Game Examples**:
  - NetHack - Appears as decorative game boards
  - Kingdom of Loathing - Mini-game implementation
  - Hikaru no Go games - Anime-based go games
  - Online Go Server (OGS) - Web-based platform
  - Few major MMORPG implementations due to complexity

- **Implementation Complexity**: Extreme
  - Simple rules but complex scoring
  - Extremely difficult AI (requires modern machine learning)
  - Long game sessions (30+ minutes to hours)
  - High learning curve for players
  - Visual clarity important for board state

- **Relevance**: While culturally significant in East Asia, Go's complexity and time 
  requirements make it challenging for casual mini-game implementation. Better suited 
  for dedicated Go games or simplified variants.

### Source 5: Mancala/Kivas Rules and Implementation

- **Basic Rules** (Kalah variant):
  - Two rows of 6 pits with stores at each end
  - 4 stones in each pit at start
  - Players pick up all stones from one pit and distribute counter-clockwise
  - Capture opponent's stones under specific conditions
  - Player with most stones in their store wins

- **Game Variants**:
  - Kalah - Most common Western variant
  - Oware - West African variant with different rules
  - Congkak - Southeast Asian variant
  - Bao - Complex East African variant

- **Implementation Examples**:
  - Civilization VI - Mancala appears as city-state mini-game
  - Various mobile games and educational software
  - Rarely implemented in major MMORPGs

- **Implementation Complexity**: Low to Medium
  - Simple visual design (pits and stones)
  - Clear rule set and quick gameplay
  - AI can be implemented with moderate difficulty
  - Short game sessions (5-15 minutes)
  - Great for mobile and casual play

- **Cultural Significance**:
  - One of oldest games in human history (7000+ years)
  - Popular across Africa, Middle East, Asia
  - Provides authentic cultural representation

- **Relevance**: Excellent candidate for mini-game implementation due to simple rules, 
  quick gameplay, and cultural authenticity. Less well-known in Western markets but 
  easy to learn.

### Source 6: Regulatory Considerations - Gambling vs. Gaming

- **Legal Definitions**:
  - **Gambling**: Activity requiring three elements:
    1. Consideration (something of value wagered)
    2. Chance (outcome determined by randomness)
    3. Prize (reward of value)
  - **Games of Skill**: Outcomes determined primarily by player skill, not chance
  - **Games of Chance**: Outcomes determined primarily by randomness

- **Board Games Classification**:
  - Chess, Shogi, Go - Pure skill games (no randomness)
  - Backgammon - Mixed (dice = chance, strategy = skill)
  - Poker - Mixed (cards = chance, betting strategy = skill)
  - Mancala - Pure skill game (no randomness)

- **Key Legal Principle**: Pure skill games generally not classified as gambling, 
  even with entry fees and prizes, in most jurisdictions.

- **Precedents**:
  - Chess tournaments with entry fees and cash prizes are legal worldwide
  - Fantasy sports ruled "skill games" in many US jurisdictions
  - E-sports tournaments with cash prizes generally legal
  - "Skill game" classification varies by jurisdiction

- **Relevance**: Board games based purely on skill (Chess, Shogi, Go, Mancala) have 
  strongest legal standing for prize-based competitions, including cryptocurrency rewards.

### Source 7: Cryptocurrency and Gaming Regulations

- **Current Regulatory Landscape** (as of 2024):
  - **European Union**: MiCA (Markets in Crypto-Assets) regulation requires licensing 
    for crypto services; games with crypto rewards may require licensing
  - **United States**: SEC treats many crypto tokens as securities; state-by-state 
    gambling laws apply; games may require money transmitter licenses
  - **China**: Cryptocurrency trading and gaming banned
  - **Japan**: Crypto exchanges must be licensed; gaming with crypto subject to gambling laws
  - **South Korea**: Strict crypto regulations; NFT games face regulatory scrutiny

- **Key Regulatory Concerns**:
  - **Money Laundering**: AML/KYC requirements for crypto transactions
  - **Securities Laws**: Tokens may be classified as securities requiring registration
  - **Gambling Laws**: Games with crypto rewards may trigger gambling regulations
  - **Consumer Protection**: Disclosure requirements, fair play guarantees
  - **Tax Reporting**: Players and platforms must report crypto income

- **Play-to-Earn Models Under Scrutiny**:
  - Axie Infinity investigated in multiple jurisdictions
  - Many P2E games have ceased operations due to regulatory pressure
  - "Gaming" vs "Gambling" distinction unclear with crypto rewards
  - NFT games face additional regulatory complexity

- **Relevance**: Crypto integration with games faces significant and evolving regulatory 
  challenges. Even skill-based games may require licenses, compliance programs, and 
  geographic restrictions.

### Source 8: Legal Frameworks for Skill-Based Gaming with Prizes

- **US Legal Framework**:
  - **Dominant Factor Test**: Determines if skill or chance is primary factor
  - **Material Element Test**: Whether chance plays a material role in outcome
  - **Any Chance Test**: Any element of chance makes it gambling (strictest, few states)
  
- **Skill Game Exceptions**:
  - Tournament formats with entry fees generally permitted if skill-based
  - Prize pools from entry fees allowed in most jurisdictions for skill games
  - Marketing and presentation matter (avoid "gambling" language)
  - Age restrictions often apply (18+ or 21+)

- **Best Practices for Compliance**:
  - Clear terms of service and prize structures
  - Verifiable skill-based mechanics (no randomness in core gameplay)
  - Age verification systems
  - Geo-blocking for restricted jurisdictions
  - Legal counsel review for target markets
  - Separate virtual currency from cryptocurrency
  - Optional entry without purchase (sweepstakes model)

- **Case Studies**:
  - **DraftKings/FanDuel**: Successfully defended fantasy sports as skill games
  - **Online Poker**: Mixed results, banned in some US states despite skill elements
  - **E-sports Tournaments**: Generally legal with proper structure
  - **Casual Game Competitions**: Many operate in legal gray areas

- **Relevance**: Skill-based board games have strong legal foundation, but cryptocurrency 
  integration requires careful structuring, legal review, and compliance systems to avoid 
  gambling classification and regulatory enforcement.

### Source 9: Implementation Models Without Gambling Issues

- **Model 1: In-Game Currency Only**
  - Players wager and win only in-game currency
  - No conversion to real money or cryptocurrency
  - Similar to chips in casual poker apps
  - Lowest regulatory risk
  - Example: Final Fantasy XIV's Triple Triad uses MGP (Manderville Gold Saucer Points)

- **Model 2: Free Entry Tournaments**
  - No entry fee or wager requirement
  - Prizes awarded from platform funds, not player contributions
  - Avoids "consideration" element of gambling
  - Higher operational costs for platform
  - Example: Many e-sports tournaments with open qualifiers

- **Model 3: Cosmetic Rewards Only**
  - Winners receive cosmetic items, not currency
  - Items are non-transferable and have no market value
  - Avoids "prize of value" element in many jurisdictions
  - Limited appeal for competitive players
  - Example: Overwatch competitive mode rewards

- **Model 4: Separate Tournament System**
  - Casual play with in-game currency in main game
  - Separate licensed tournament platform for real/crypto prizes
  - Clear separation between casual and competitive play
  - Requires tournament license in most jurisdictions
  - Example: Some poker platforms (PokerStars) separate play money and real money

- **Model 5: Skill-Based Rewards**
  - Rewards based on demonstrated skill level, not wagers
  - Achievement-based crypto rewards (complete tutorials, win X games)
  - Not tied to specific game outcomes or wagers
  - May still require crypto licensing but avoids gambling classification
  - Example: Some educational platforms that reward learning with crypto

- **Relevance**: Multiple implementation models allow board games in MMORPGs while 
  managing regulatory risk. In-game currency models have lowest risk; cryptocurrency 
  integration requires careful legal structuring.

## Implications for Design

- **Implementation Strategy for BlueMarble**:
  - Board games as tavern/social activities enhance world immersion
  - Start with in-game currency only to avoid regulatory complexity
  - Chess and Mancala recommended as first implementations (low complexity, clear skill-based)
  - Use existing open-source engines (Stockfish for chess) to reduce development time
  - Implement single-player vs. NPCs first, then add player-vs-player
  - Consider Shogi for authentic cultural content in appropriate settings

- **Cryptocurrency Integration Recommendations**:
  - **Do Not** integrate direct ETH/BTC wagering on game outcomes
  - Risk: Gambling classification in most jurisdictions
  - Risk: Requires gaming licenses, AML/KYC compliance, geographic restrictions
  - Risk: Ongoing regulatory scrutiny and potential enforcement actions
  
- **Alternative Crypto Integration Models**:
  - **Achievement Rewards**: Award crypto for reaching milestones (win 100 games, reach rating)
  - **Tournament System**: Separate licensed platform for crypto tournaments with proper compliance
  - **NFT Pieces**: Cosmetic chess/board game pieces as collectible NFTs (separate from wagering)
  - **Season Rewards**: Top players in seasonal ladders receive crypto prizes (not per-game wagering)

- **Risk Mitigation**:
  - Consult gaming law attorneys before any crypto integration
  - Implement geo-blocking for restricted jurisdictions
  - Clear terms of service stating games are skill-based
  - Age verification for any prize competitions
  - Separate systems for casual play vs. competitive tournaments
  - Monitor regulatory developments in target markets

- **Design Priorities**:
  1. Implement board games with in-game currency only (Phase 1)
  2. Build player base and engagement with mini-games
  3. Evaluate regulatory landscape for target markets
  4. If crypto integration desired, pursue licensed tournament model (Phase 2)
  5. Keep casual and competitive systems clearly separated

## Open Questions / Next Steps

### Open Questions

- What is BlueMarble's target geographic market? Regulations vary significantly by region.
- What is the appetite for regulatory compliance costs (licensing, legal, KYC/AML systems)?
- Are there existing partnerships with licensed gaming platforms for tournaments?
- What is the primary goal: player engagement (social mini-games) or monetization (crypto prizes)?
- Should board games be authentic recreations or original variants designed for the game world?

### Next Steps

- [ ] Review BlueMarble's target market and applicable gaming regulations
- [ ] Conduct legal review of proposed mini-game systems with gaming law attorney
- [ ] Evaluate open-source board game engines for integration (Stockfish, etc.)
- [ ] Design UI/UX for board game integration in tavern/social spaces
- [ ] Create technical specification for board game system architecture
- [ ] Prototype chess implementation with in-game currency
- [ ] Test player engagement with board game mini-games in alpha
- [ ] Evaluate tournament system vendors if crypto integration desired
- [ ] Document compliance requirements for any cryptocurrency features
- [ ] Create geo-blocking and age verification systems if needed

## Related Documents

- [Game Design Document](../../docs/core/game-design-document.md)
- [Economy Systems](../../docs/systems/economy-systems.md)
- [Social Interaction System Summary](../../SOCIAL_INTERACTION_SYSTEM_SUMMARY.md)
- [Game Design Roles and Types](game-design-roles-and-types.md)

## References

### Board Games in Video Games
- The Witcher 3: Wild Hunt - Gwent implementation
- Final Fantasy XIV - Triple Triad, Mahjong mini-games
- Red Dead Redemption 2 - Tavern games
- The Elder Scrolls Online - Tales of Tribute

### Chess Resources
- Chess Programming Wiki: https://www.chessprogramming.org/
- Stockfish Engine (open-source): https://stockfishchess.org/
- Lichess (open-source platform): https://lichess.org/

### Regulatory Resources
- MiCA Regulation (EU): https://www.esma.europa.eu/esmas-activities/digital-finance-and-innovation/markets-crypto-assets-regulation-mica
- SEC Cryptocurrency Guidance (US): https://www.sec.gov/cryptocurrency
- FATF Guidance on Virtual Assets: https://www.fatf-gafi.org/
- Gaming Law Masters: Various gaming law publications

### Game Design Theory
- Costikyan, G. "Uncertainty in Games" (MIT Press)
- Salen, K. & Zimmerman, E. "Rules of Play" (MIT Press)
- Schell, J. "The Art of Game Design" (CRC Press)
