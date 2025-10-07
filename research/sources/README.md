# Research Sources

This directory contains bibliography, reading lists, and source documentation for BlueMarble research.

**Last Updated:** 2025-01-17  
**Total Sources Tracked:** 115+ (60 books + 13 survival collections + 20+ online + 21 scientific references)  
**Last Updated:** 2025-01-18  
**Total Sources Tracked:** 75+ (60 books + 13 survival collections + 20+ online + 2 game economy external)  
**Status:** Active - Comprehensive source tracking system

---

## Files

### Primary Documentation

- **SOURCE-SUMMARY.md**: 📊 **High-level summary table** with counts
  - Quick reference: downloaded, processed, and total counts by category
  - Progress metrics and completion rates
  - Storage requirements and sizes
  - Next steps and priorities
  
- **sources.bib**: Complete bibliography in BibTeX format
  - 64+ BibTeX entries covering all major sources
  - Categorized by topic (game programming, design theory, multiplayer, performance optimization, scientific references, etc.)
  - Includes survival knowledge collections and C# vs C++ performance books
  - Online resources, open source projects, and scientific reference materials
  
- **reading-list.md**: Curated reading list with priorities and status tracking
  - 112+ tracked items across all source types
  - Organized by priority (Critical, High, Medium, Low)
  - Tracks completion status (✓ completed, 🔍 in progress, ⏳ pending)
  - Cross-referenced with analysis documents
  - Includes new C# vs C++ performance optimization section
  
- **survival-sources.md**: Survival knowledge collection download sources
  - 13 major collections tracked
  - 10 downloaded and analyzed, 3 pending
  - Download links and magnet URIs
  - Size and format information (~440-450 GB total)
  - Extraction pipeline status
  - Legal and licensing considerations

- **game-economy-design-external.md**: 🆕 **External game economy design document**
  - Auction tiers (local to global) with fee structures
  - Transport mechanics with multiple vehicle types
  - Goods spoilage and preservation systems
  - Seasonal supply/demand cycles
  - Regional market specialization
  - Processed from external source (provided by @Nomoos)
  
- **discovery-sources-game-economy.md**: 🆕 **Catalogued source references**
  - 7 identified source documents (Sources 39, 58-63)
  - Priority ratings for investigation
  - Line-by-line reference tracking
  - Integration status with current implementation
  - Formulas and key data extracted
  
- **quotes.md**: Notable quotes and excerpts from sources

---

## Source Categories

### Game Development Books (60 sources)

#### Critical Priority (MMORPG Core)
- Multiplayer Game Programming
- Network Programming for Games
- Game Engine Architecture
- MMO Architecture references

#### High Priority (Core Systems)
- Game Programming Patterns
- Real-Time Rendering
- AI for Games
- Game design theory books
- Open source project documentation

#### Medium Priority (Supporting)
- **Performance Optimization (10 books)** - C# vs C++ performance, memory management, benchmarking
- Content creation tools
- Procedural generation
- Virtual economies
- Development process

#### Low Priority (Specialized)
- Platform-specific guides
- Case studies
- Specialized topics

### Survival Knowledge Collections (13 sources)

#### Completed Downloads (10)
1. OpenStreetMap Data (geographic)
2. Appropriate Technology Library (1,050+ ebooks)
3. Survivor Library Collection (historical tech)
4. Great Science Textbooks (88.9 GB)
5. Military Manuals (22,000+ documents)
6. Medical Textbooks Collection
7. Encyclopedia Collections
8. Communication Systems Documentation
9. CD3WD Collection
10. awesome-survival Repository Overview

#### Pending Downloads (3)
11. Energy Systems Collection
12. Historical Maps and Navigation Resources
13. Specialized Collections (Deep Web Sources)

### Online Resources (20+ sources)
- Documentation (Godot, Unity, Unreal)
- Open source projects (TrinityCore, CMaNGOS, etc.)
- Video tutorials and courses
- Community forums
- Industry resources (Gamasutra, GDC)

### Scientific Reference Sources (21 sources)
- **Biology and Organic Systems (5 sources)** - Decomposition, bacteria, organs, scavenger ecology
- **Engineering and Materials Science (5 sources)** - Structural mechanics, cast iron, iron-cementite diagrams
- **Physics and Chemistry (6 sources)** - Surface tension, redox, viscosity, fluid dynamics, gas laws
- **Atmospheric Science (5 sources)** - Atmosphere, magnetosphere, solar wind, weather systems, thermochemistry

**Application:** Foundational knowledge for realistic simulation systems including crafting, metallurgy, atmospheric simulation, structural engineering, and survival mechanics.
### External Game Design Documents (2 sources)

#### Processed Documents (2)
1. **Game Economy Design** - Auctions, transport, fees, spoilage, seasonality
   - Status: ✅ Processed and integrated
   - File: `game-economy-design-external.md`
   - Provider: @Nomoos (2025-01-18)
   - Topics: Auction tiers, transport mechanics, seasonal effects, spoilage
   - Integration: Aligned with extended auction system implementation

2. **Discovery Sources Catalog** - Referenced source materials
   - Status: ✅ Catalogued
   - File: `discovery-sources-game-economy.md`
   - Sources Identified: 7 (Sources 39, 58-63)
   - Priority: HIGH for Sources 58, 60, 39 (transport, seasons, tiers)
   - Next Step: Request access to referenced source documents

---

## Guidelines

### Bibliography (sources.bib)

Maintain a BibTeX file with all research sources using standard BibTeX format:

```bibtex
@book{key,
  title = {Title},
  author = {Author Name},
  publisher = {Publisher},
  isbn = {ISBN},
  year = {Year},
  note = {Relevance notes}
}
```

**Entry Types:**
- `@book` - Published books
- `@misc` - Online resources, documentation, collections
- `@article` - Academic papers (if added)

**Categories:**
- Game Programming and Architecture
- Game Design Theory
- AI and Algorithms
- Multiplayer and Online Games
- Procedural Generation
- Virtual Economies
- Online Resources and Documentation
- Open Source Projects
- Survival and Technology Resources
- Geographic and Map Data
- Scientific Reference Sources (Biology, Engineering, Physics, Atmospheric Science)

### Reading List (reading-list.md)

Organize reading materials by:

**Priority Levels:**
- **Critical**: Essential for core MMORPG functionality
- **High**: Core game systems and design
- **Medium**: Supporting features and tools
- **Low**: Platform-specific or specialized topics

**Status Tracking:**
- `[ ]` - Not yet analyzed
- `[x]` - Analysis completed with documentation
- `🔍` - Currently in progress
- `⏳` - Queued for analysis

**Information to Include:**
- Full title and author
- Brief description of relevance
- Cross-reference to analysis documents
- ISBN or URL when applicable

### Survival Sources (survival-sources.md)

Track survival knowledge collections with:

**Required Information:**
- Collection name and type
- Source repository or website
- Download method (direct, torrent, magnet link)
- Size and format
- Current status (downloaded, pending, analyzed)
- Analysis document reference
- Content categories and use cases

**Download Organization:**
```
~/bluemarble-sources/
├── openstreetmap/
├── appropriate-tech/
├── survivor-library/
└── [other collections]/
```

### Quotes (quotes.md)

Capture notable quotes with:

- Full citation (author, source, page)
- Context of the quote
- Relevance to BlueMarble systems
- Tags for categorization
- Cross-reference to related documents

---

## Usage Workflow

### Adding a New Source

1. **Identify Source**
   - Found during literature review
   - Referenced in existing documents
   - Discovered through research

2. **Add to Bibliography**
   ```bash
   # Edit sources.bib
   vim research/sources/sources.bib
   # Add BibTeX entry in appropriate category
   ```

3. **Add to Reading List**
   ```bash
   # Edit reading-list.md
   vim research/sources/reading-list.md
   # Add under appropriate priority section
   ```

4. **For Survival Collections**
   ```bash
   # Edit survival-sources.md
   vim research/sources/survival-sources.md
   # Add download information and tracking details
   ```

5. **Create Analysis Document** (if analyzing)
   ```bash
   # Create in research/literature/
   vim research/literature/[topic]-analysis.md
   # Follow analysis document template
   ```

### Tracking Analysis Progress

1. **Mark as In Progress**
   - Update reading-list.md status to 🔍
   - Note start date in analysis document

2. **Complete Analysis**
   - Mark as `[x]` in reading-list.md
   - Update master-research-queue.md
   - Cross-reference analysis document

3. **Extract Quotes**
   - Add relevant quotes to quotes.md
   - Include full citation and context

---

## Cross-Linking

Link sources to relevant documentation:

- **Research Literature**: `research/literature/` - Detailed analysis documents
- **Master Queue**: `research/literature/master-research-queue.md` - Overall tracking
- **Topics**: `research/topics/` - Game system topics
- **Experiments**: `research/experiments/` - Prototypes and tests
- **Assignment Groups**: `research/literature/research-assignment-group-*.md` - Team assignments

---

## Verification and Quality

### Source Verification Checklist

- [ ] BibTeX entry added to sources.bib
- [ ] Reading list entry with priority
- [ ] Download source documented (if applicable)
- [ ] Legal/licensing status verified
- [ ] Cross-references added
- [ ] Analysis document created (if completed)
- [ ] Master queue updated

### Quality Standards

**Bibliography Entries:**
- Complete metadata (author, title, publisher, year, ISBN)
- Proper BibTeX formatting
- Categorized correctly
- Relevant notes included

**Reading List Entries:**
- Clear priority assignment
- Brief but informative description
- Status accurately reflected
- Cross-references included

**Survival Source Entries:**
- Complete download information
- Size and format documented
- Current status tracked
- Legal considerations noted

---

## Statistics

### Current Status

**Books and Technical Resources:**
- Total: 50
- Critical Priority: 8
- High Priority: 21
- Medium Priority: 15
- Low Priority: 6
- Completed Analysis: 4

**Survival Collections:**
- Total: 13
- Downloaded: 10
- Pending: 3
- Total Size: ~350+ GB
- Analyzed: 10

**Online Resources:**
- Documentation: 5
- Open Source Projects: 10
- Communities: 5
- Video Courses: 5

**Scientific Reference Sources:**
- Total: 21
- Biology and Organic Systems: 5
- Engineering and Materials Science: 5
- Physics and Chemistry: 6
- Atmospheric Science: 5
- Status: Catalogued, pending integration into game systems

---

## Next Steps

### Immediate Actions

1. **Download Pending Collections**
   - Energy Systems Collection (high priority)
   - Historical Maps and Navigation (high priority)
   - Specialized Collections (medium priority)

2. **Critical Reading**
   - Begin multiplayer architecture books
   - Focus on networking and scalability
   - Document patterns and best practices

3. **Analysis Pipeline**
   - Create extraction guides for pending collections
   - Update master queue with progress
   - Cross-reference findings across sources

### Long-term Maintenance

1. **Regular Updates**
   - Add newly discovered sources
   - Update analysis status
   - Track completion progress

2. **Quality Review**
   - Verify all citations
   - Check for broken links
   - Update download sources

3. **Integration**
   - Map sources to game systems
   - Identify implementation priorities
   - Create development roadmap

---

## Related Documents

- `research/literature/master-research-queue.md` - Master tracking document
- `research/literature/README.md` - Literature directory overview
- `research/literature/*-analysis.md` - Individual source analyses
- `research/literature/survival-content-extraction-*.md` - Extraction guides

---

**Maintained By:** Game Design Research Team  
**Contact:** See CONTRIBUTING.md for contribution guidelines
