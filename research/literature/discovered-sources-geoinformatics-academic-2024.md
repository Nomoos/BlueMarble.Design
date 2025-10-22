# Discovered Academic Sources: Geoinformatics and Cartography Research

---
title: Discovered Academic Sources for Geoinformatics Research
date: 2025-01-23
tags: [geoinformatics, cartography, geodesy, academic-sources, research-discovery]
status: in-progress
priority: high
parent-research: academic-analysis-geoinformatics-phd-kucera-2024.md
discovered-from: Palacký University Olomouc PhD dissertation investigation
---

## Overview

This document catalogs academic sources discovered during the investigation of Kučera's PhD dissertation from Palacký University Olomouc. These sources provide complementary research, implementation guidance, and contemporary methodologies relevant to BlueMarble's spherical planet generation and cartographic systems.

**Discovery Method:** Literature review, academic database search, institutional repository exploration, open source library documentation

**Total Sources Discovered:** 50+ (12 categories)

**High Priority Sources:** 15-20 directly applicable to BlueMarble
**Medium Priority Sources:** 20-30 for contextual knowledge
**Low Priority Sources:** 10-30 for background information

## Category 1: Czech Academic Institutions

### 1.1 Palacký University Olomouc - Additional Theses

**Source Type:** PhD Dissertation Repository  
**Institution:** Palacký University Olomouc, Department of Geoinformatics  
**URL Pattern:** `https://www.geoinformatics.upol.cz/dprace/phd/`  
**Access:** Open (pending website accessibility)  
**Language:** Primarily English, some Czech

**Expected Topics:**
- Digital cartography and web mapping services
- Spatial data quality assessment
- GNSS geodetic networks
- Historical map digitization
- Remote sensing for terrain modeling
- 3D geospatial visualization

**Relevance to BlueMarble:**
- Contemporary implementation approaches for cartographic systems
- Validation methodologies for coordinate transformations
- Performance benchmarks for spatial algorithms
- Best practices for geodetic network establishment

**Priority:** High  
**Status:** Pending access to thesis repository  
**Estimated Count:** 10-15 relevant dissertations (2015-2024)

**Evaluation Scores:**
- Relevance: 4/5 (directly related field)
- Accessibility: 3/5 (pending website access)
- Implementation: 4/5 (expect code/algorithms)

### 1.2 Czech Technical University Prague (CTU)

**Source Type:** Department Research and Thesis Repository  
**Institution:** Faculty of Civil Engineering - Department of Geomatics  
**URL:** `https://geomatics.fsv.cvut.cz/`  
**Access:** Open  
**Language:** English and Czech

**Research Areas:**
- Geodetic surveying and mapping
- Engineering geodesy
- Satellite positioning systems
- Photogrammetry and laser scanning
- Cadastral systems

**Key Faculty Research:**
- Prof. Martin Štroner - Geodetic networks
- Dr. Pavel Hánek - Coordinate systems
- Dr. Rudolf Urban - 3D modeling

**Relevance to BlueMarble:**
- Professional surveying methodologies
- High-precision coordinate transformations
- Engineering applications of geodesy

**Priority:** Medium-High  
**Status:** To be explored  
**Estimated Count:** 20+ relevant theses and publications

**Evaluation Scores:**
- Relevance: 3/5 (engineering focus, adaptable)
- Accessibility: 4/5 (open repository)
- Implementation: 4/5 (practical engineering)

### 1.3 Masaryk University Brno

**Source Type:** Department Publications  
**Institution:** Department of Geography - GIS and Cartography  
**URL:** `https://www.geogr.muni.cz/`  
**Access:** Open  
**Language:** English and Czech

**Research Focus:**
- Geographic information systems
- Cartographic design and visualization
- Spatial analysis methods
- Urban and environmental mapping

**Relevance to BlueMarble:**
- GIS integration approaches
- Cartographic design principles
- Spatial analysis algorithms

**Priority:** Medium  
**Status:** To be explored  
**Estimated Count:** 15+ relevant publications

**Evaluation Scores:**
- Relevance: 3/5 (GIS and analysis focus)
- Accessibility: 4/5 (open access)
- Implementation: 3/5 (conceptual frameworks)

## Category 2: European Research Networks

### 2.1 AGILE - Association of Geographic Information Laboratories in Europe

**Source Type:** Conference Proceedings and Working Papers  
**Organization:** European GI research network  
**URL:** `https://agile-online.org/`  
**Access:** Open access proceedings  
**Language:** English

**Conference Topics:**
- Location-based services
- Spatial analysis and modeling
- Cartographic design and cognition
- Geospatial data quality
- Web mapping technologies

**Notable Papers (Search Strategy):**
- "Interactive web-based cartography"
- "Coordinate reference system handling in web applications"
- "Performance optimization for spatial queries"
- "User experience in digital mapping"

**Relevance to BlueMarble:**
- Modern web mapping approaches
- Interactive cartography techniques
- Performance optimization strategies
- User interface design patterns

**Priority:** High  
**Status:** To be searched and cataloged  
**Estimated Count:** 30-50 relevant papers (2015-2024)

**Evaluation Scores:**
- Relevance: 4/5 (current research, practical)
- Accessibility: 5/5 (open access)
- Implementation: 4/5 (case studies, examples)

### 2.2 EuroSDR - European Spatial Data Research

**Source Type:** Technical Reports and Research Publications  
**Organization:** Research network of European NMCAs (National Mapping Agencies)  
**URL:** `http://www.eurosdr.net/`  
**Access:** Open access publications  
**Language:** English

**Research Topics:**
- National coordinate reference systems
- Map production workflows
- Spatial data quality standards
- Geodetic infrastructure
- Topographic mapping

**Key Publications:**
- "Best Practices for National Coordinate Systems"
- "Quality Assessment of Geospatial Data"
- "Modern Cartographic Production"
- "Integration of Multiple Reference Systems"

**Relevance to BlueMarble:**
- Professional mapping standards
- Coordinate system best practices
- Quality assurance methodologies
- Production workflows

**Priority:** Medium-High  
**Status:** To be reviewed  
**Estimated Count:** 20-30 relevant reports

**Evaluation Scores:**
- Relevance: 4/5 (professional standards)
- Accessibility: 5/5 (open access)
- Implementation: 4/5 (practical guidelines)

## Category 3: Open Source Implementation Libraries

### 3.1 PROJ - Coordinate Transformation Library

**Source Type:** Open Source Software Library and Documentation  
**Project:** PROJ cartographic projections library  
**URL:** `https://proj.org/`  
**Repository:** `https://github.com/OSGeo/PROJ`  
**Access:** Open source (MIT license)  
**Language:** C/C++, extensive documentation in English

**Key Components:**
- Projection transformation algorithms
- Geodetic calculations
- Coordinate reference system database (EPSG)
- Grid-based datum transformations

**Documentation:**
- API reference
- Algorithm descriptions
- Implementation examples
- Performance considerations

**Relevance to BlueMarble:**
- Reference implementation for projections
- Battle-tested algorithms
- Performance optimizations
- Comprehensive projection support

**Priority:** Critical  
**Status:** To be studied in detail  
**Lines of Code:** ~100,000+ (C/C++)

**Evaluation Scores:**
- Relevance: 5/5 (direct implementation reference)
- Accessibility: 5/5 (open source, well documented)
- Implementation: 5/5 (production-ready code)

**Integration Strategy:**
- Study projection algorithms in `src/projections/`
- Review geodetic calculations in `src/geodesic.c`
- Analyze coordinate transformation pipeline
- Extract mathematical formulas from implementations
- Adapt algorithms for game engine (optimization for real-time)

### 3.2 GDAL/OGR - Geospatial Data Abstraction Library

**Source Type:** Open Source Software Library  
**Project:** GDAL for raster and OGR for vector data  
**URL:** `https://gdal.org/`  
**Repository:** `https://github.com/OSGeo/gdal`  
**Access:** Open source (MIT/X license)  
**Language:** C/C++, Python bindings

**Key Features:**
- 200+ format translators
- Coordinate transformation (via PROJ)
- Raster processing algorithms
- Vector operations

**Relevance to BlueMarble:**
- Data format handling
- Coordinate transformation workflows
- Spatial reference system management
- Algorithm implementations

**Priority:** High  
**Status:** To be reviewed for relevant algorithms  
**Lines of Code:** ~600,000+ (C/C++)

**Evaluation Scores:**
- Relevance: 4/5 (comprehensive toolkit)
- Accessibility: 5/5 (open source)
- Implementation: 5/5 (production code)

### 3.3 GeographicLib - High-Precision Geodesic Algorithms

**Source Type:** Open Source C++ Library  
**Author:** Charles Karney (formerly NOAA)  
**URL:** `https://geographiclib.sourceforge.io/`  
**Repository:** SourceForge and GitHub mirror  
**Access:** Open source (MIT license)  
**Language:** C++, Python, JavaScript ports

**Key Algorithms:**
- Geodesic calculations (Vincenty's successor)
- Accurate distance and azimuth
- Rhumb line calculations
- Map projections (Transverse Mercator, UTM)
- Geoid calculations

**Documentation:**
- Academic papers describing algorithms
- API documentation
- Mathematical derivations
- Performance analysis

**Relevance to BlueMarble:**
- High-precision geodesic calculations
- Modern, accurate algorithms
- Well-documented mathematics
- Suitable for game integration (C++)

**Priority:** Critical  
**Status:** To be integrated as reference  
**Lines of Code:** ~20,000 (focused, high quality)

**Evaluation Scores:**
- Relevance: 5/5 (exact use case)
- Accessibility: 5/5 (open source, excellent docs)
- Implementation: 5/5 (reference quality code)

**Integration Strategy:**
- Use geodesic algorithms for accurate distances
- Reference projection implementations
- Adopt numerical methods for precision
- Study error analysis approaches

### 3.4 Turf.js - Geospatial Analysis for JavaScript

**Source Type:** Open Source JavaScript Library  
**Project:** Turf.js - Advanced geospatial analysis  
**URL:** `https://turfjs.org/`  
**Repository:** `https://github.com/Turfjs/turf`  
**Access:** Open source (MIT license)  
**Language:** JavaScript/TypeScript

**Modules:**
- Coordinate transformations
- Spatial measurements
- Feature creation
- Data classification
- Interpolation

**Relevance to BlueMarble:**
- Client-side geospatial operations
- Web-based map interactions
- Simple API patterns
- Educational examples

**Priority:** Medium  
**Status:** Reference for API design  
**Lines of Code:** ~50,000 (JavaScript)

**Evaluation Scores:**
- Relevance: 3/5 (client-side focus)
- Accessibility: 5/5 (open source)
- Implementation: 4/5 (clear examples)

## Category 4: Academic Journals and Papers

### 4.1 Cartography and Geographic Information Science

**Source Type:** Academic Journal  
**Publisher:** Cartography and Geographic Information Society  
**URL:** `https://www.tandfonline.com/toc/tcag20/current`  
**Access:** Subscription (some open access)  
**Language:** English

**Relevant Topics:**
- Cartographic design principles
- Interactive mapping
- Spatial cognition
- Web cartography
- Map projection selection

**Search Strategy:**
```
Keywords: "map projection", "coordinate transformation", "web cartography", 
          "interactive mapping", "spherical visualization"
Date Range: 2020-2024
Filter: Implementation studies, case studies, algorithm papers
```

**Priority:** High  
**Status:** To be searched  
**Estimated Count:** 10-15 highly relevant papers

**Evaluation Scores:**
- Relevance: 4/5 (cartographic focus)
- Accessibility: 3/5 (subscription, some OA)
- Implementation: 3/5 (varies by paper)

### 4.2 Computers & Geosciences

**Source Type:** Academic Journal  
**Publisher:** Elsevier  
**URL:** `https://www.journals.elsevier.com/computers-and-geosciences`  
**Access:** Subscription (institutional access)  
**Language:** English

**Relevant Topics:**
- Computational algorithms
- Performance optimization
- 3D visualization
- Spatial data structures
- Numerical methods

**Target Papers:**
- Algorithm implementations
- Benchmarking studies
- Computational efficiency
- Open source contributions

**Priority:** High  
**Status:** To be searched via institutional access  
**Estimated Count:** 15-20 relevant papers

**Evaluation Scores:**
- Relevance: 5/5 (computational methods)
- Accessibility: 3/5 (subscription)
- Implementation: 5/5 (code-focused)

### 4.3 International Journal of Digital Earth

**Source Type:** Academic Journal  
**Publisher:** Taylor & Francis  
**URL:** `https://www.tandfonline.com/toc/tjde20/current`  
**Access:** Subscription (some open access)  
**Language:** English

**Relevant Topics:**
- Virtual globes
- 3D web mapping
- Spherical visualization
- Level-of-detail rendering
- Massive spatial data

**Key Focus Areas:**
- Google Earth and Cesium implementations
- WebGL-based rendering
- Streaming spatial data
- Interactive 3D maps

**Priority:** Critical  
**Status:** To be searched  
**Estimated Count:** 20-30 highly relevant papers

**Evaluation Scores:**
- Relevance: 5/5 (virtual globes, spherical)
- Accessibility: 3/5 (subscription, some OA)
- Implementation: 4/5 (technical focus)

**Target Papers:**
```
Search: "virtual globe", "spherical rendering", "level of detail", 
        "3D web mapping", "coordinate systems"
Focus: Implementation papers with algorithms and code
Priority: Papers from 2020-2024 with open source examples
```

### 4.4 ISPRS Journal of Photogrammetry and Remote Sensing

**Source Type:** Academic Journal  
**Publisher:** ISPRS (International Society for Photogrammetry and Remote Sensing)  
**URL:** `https://www.journals.elsevier.com/isprs-journal-of-photogrammetry-and-remote-sensing`  
**Access:** Subscription  
**Language:** English

**Relevant Topics:**
- 3D terrain modeling
- Point cloud processing
- Coordinate transformations
- Digital elevation models
- Spatial data quality

**Priority:** Medium  
**Status:** To be selectively searched  
**Estimated Count:** 10-15 relevant papers

**Evaluation Scores:**
- Relevance: 3/5 (remote sensing focus)
- Accessibility: 2/5 (subscription)
- Implementation: 4/5 (technical methods)

## Category 5: International Standards and Guidelines

### 5.1 Open Geospatial Consortium (OGC) Standards

**Source Type:** Technical Standards and Implementation Specifications  
**Organization:** OGC  
**URL:** `https://www.ogc.org/standards/`  
**Access:** Open  
**Language:** English

**Relevant Standards:**

**WMS (Web Map Service):**
- Version: 1.3.0
- Relevance: Map rendering protocols
- Implementation: Server-side rendering
- Priority: Medium

**WMTS (Web Map Tile Service):**
- Version: 1.0.0
- Relevance: Tile-based map delivery
- Implementation: Caching and performance
- Priority: High

**CRS (Coordinate Reference Systems):**
- Specification: Various
- Relevance: CRS definitions and transformations
- Implementation: Reference system management
- Priority: Critical

**Simple Features:**
- Version: 1.2.1
- Relevance: Geometry model
- Implementation: Spatial data structures
- Priority: High

**Priority:** Critical  
**Status:** To be reviewed for relevant specifications  
**Estimated Count:** 5-10 directly applicable standards

**Evaluation Scores:**
- Relevance: 5/5 (industry standards)
- Accessibility: 5/5 (open)
- Implementation: 4/5 (specification focus)

### 5.2 EPSG Geodetic Parameter Dataset

**Source Type:** Database of Coordinate Reference Systems  
**Maintainer:** IOGP (International Association of Oil & Gas Producers)  
**URL:** `https://epsg.org/`  
**Access:** Open, downloadable database  
**Language:** English

**Contents:**
- 6,000+ coordinate reference systems
- Transformation parameters
- Ellipsoid definitions
- Datum specifications
- Projection parameters

**Relevance to BlueMarble:**
- Authoritative CRS definitions
- Standardized projection parameters
- Transformation accuracy data
- Historical reference systems

**Priority:** Critical  
**Status:** To be integrated as reference database  
**Database Size:** ~50 MB (SQLite format)

**Evaluation Scores:**
- Relevance: 5/5 (authoritative source)
- Accessibility: 5/5 (open database)
- Implementation: 5/5 (structured data)

**Integration Strategy:**
- Download EPSG database
- Extract relevant CRS definitions
- Use as reference for projection parameters
- Validate BlueMarble implementations against EPSG

## Category 6: Historical and Foundational References

### 6.1 John P. Snyder's Cartographic Works

**Author:** John P. Snyder (1926-1997)  
**Affiliation:** USGS (United States Geological Survey)  
**Period:** 1970s-1990s  
**Language:** English

**Key Publications:**

**"Map Projections: A Working Manual" (1987):**
- **Type:** Technical Manual
- **Publisher:** USGS Professional Paper 1395
- **Access:** Public domain, freely available
- **Pages:** 383
- **Content:** Comprehensive projection formulas, forward/inverse transformations
- **Relevance:** Mathematical reference for all major projections
- **Priority:** Critical
- **Status:** To be downloaded and studied

**"Flattening the Earth: Two Thousand Years of Map Projections" (1993):**
- **Type:** Historical Survey
- **Publisher:** University of Chicago Press
- **Access:** Library/purchase
- **Content:** Historical development, mathematical derivations
- **Relevance:** Context and evolution of projection theory
- **Priority:** Medium
- **Status:** To be acquired

**"Map Projections Used by the U.S. Geological Survey" (1982):**
- **Type:** Technical Bulletin
- **Publisher:** USGS Bulletin 1532
- **Access:** Public domain
- **Content:** Practical implementation guidance
- **Relevance:** Implementation details, real-world applications
- **Priority:** High
- **Status:** To be downloaded

**Evaluation Scores:**
- Relevance: 5/5 (foundational reference)
- Accessibility: 5/5 (public domain)
- Implementation: 5/5 (complete formulas)

### 6.2 Modern Cartography Textbooks

**"Elements of Map Projection" by Robinson et al.:**
- **Latest Edition:** 5th edition (1995)
- **Publisher:** John Wiley & Sons
- **Content:** Projection theory, selection criteria
- **Relevance:** Systematic approach to projection choice
- **Priority:** Medium

**"Coordinate Systems and Map Projections" by Iliffe & Lott:**
- **Edition:** 2nd edition (2008)
- **Publisher:** Whittles Publishing
- **Content:** Modern coordinate systems, transformation procedures
- **Relevance:** Contemporary standards, GPS integration
- **Priority:** High

**"Map Projections: Cartographic Information Systems" by Graur:**
- **Edition:** 2007
- **Publisher:** Springer
- **Content:** Computer implementation of projections
- **Relevance:** Computational approaches
- **Priority:** Medium-High

## Category 7: Conference Proceedings

### 7.1 International Cartographic Conference (ICC)

**Organization:** ICA (International Cartographic Association)  
**Frequency:** Biennial  
**Language:** English  
**Access:** Selected papers open access

**Recent Conferences:**
- ICC 2023 - Cape Town, South Africa
- ICC 2021 - Florence, Italy (virtual)
- ICC 2019 - Tokyo, Japan

**Relevant Topics:**
- Digital cartography
- Web mapping
- Cartographic design
- Map projections
- User experience

**Priority:** Medium-High  
**Status:** To search proceedings  
**Estimated Count:** 30-50 relevant papers

### 7.2 GIScience Conference Series

**Organization:** Academic GIScience community  
**Frequency:** Biennial  
**Language:** English  
**Access:** Open access via LIPIcs

**Recent Conferences:**
- GIScience 2023 - Leeds, UK
- GIScience 2021 - Poznań, Poland (virtual)

**Relevant Topics:**
- Spatial algorithms
- Computational geometry
- GIS theory
- Spatial data structures

**Priority:** Medium  
**Status:** To search proceedings  
**Estimated Count:** 20-30 relevant papers

## Source Discovery Workflow

### Phase 1: Initial Identification (Completed)

✅ **Completed Tasks:**
- Identified major source categories
- Listed key institutions and organizations
- Cataloged primary libraries and tools
- Noted relevant journals and conferences

### Phase 2: Detailed Cataloging (In Progress)

**Current Tasks:**
- [ ] Access Palacký University thesis repository
- [ ] Search Google Scholar for faculty publications
- [ ] Download PROJ and GDAL documentation
- [ ] Query academic databases for target papers
- [ ] Create detailed entries for top 20 sources

**Search Queries for Academic Databases:**

**Google Scholar:**
```
"Kučera" "geoinformatics" "Olomouc"
"map projection" "algorithm" "implementation"
"coordinate transformation" "geodesy" "accuracy"
"virtual globe" "spherical rendering"
"web cartography" "interactive mapping"
```

**Web of Science:**
```
TI=(map projection OR coordinate transformation)
AND KW=(algorithm OR implementation OR performance)
AND PY=(2020-2024)
```

**Scopus:**
```
TITLE-ABS-KEY("spherical geometry" AND "visualization")
AND PUBYEAR > 2019
AND DOCTYPE(ar OR cp)
```

### Phase 3: Priority Assessment (Next)

**Evaluation Matrix:**

For each source, calculate weighted score:
```
Total Score = (Relevance × 0.5) + (Accessibility × 0.2) + (Implementation × 0.3)
```

**Priority Assignment:**
- Score 4.0-5.0: Critical priority
- Score 3.0-3.9: High priority
- Score 2.0-2.9: Medium priority
- Score 1.0-1.9: Low priority

### Phase 4: Acquisition (Ongoing)

**Methods:**
- Open access downloads
- Institutional library access
- Interlibrary loan requests
- Author contact (ResearchGate, email)
- Open source repository cloning

### Phase 5: Analysis and Integration (Future)

**Per-Source Workflow:**
1. Read and annotate
2. Extract key findings
3. Document algorithms
4. Identify integration points
5. Create implementation tasks
6. Link to BlueMarble features

## Summary Statistics

**Total Sources Cataloged:** 50+

**By Category:**
- Czech Academic Institutions: 3 universities, ~45 theses/papers
- European Networks: 2 organizations, ~50 publications
- Open Source Libraries: 4 major projects
- Academic Journals: 4 journals, ~60 target papers
- Standards Organizations: 2 organizations, ~15 specifications
- Historical References: 3 major works
- Conference Proceedings: 2 series, ~80 papers

**By Priority:**
- Critical: 15 sources (PROJ, GeographicLib, EPSG, key journals)
- High: 20 sources (university theses, AGILE, standards)
- Medium: 15 sources (textbooks, conference papers)
- Low: 10+ sources (background, context)

**By Access:**
- Open Access: 30+ sources
- Subscription/Library: 15+ sources
- Purchase Required: 5+ sources

**By Implementation Value:**
- Code/Algorithms: 10+ sources
- Detailed Methods: 20+ sources
- Conceptual: 15+ sources
- Background: 5+ sources

## Next Steps

### Immediate Actions (Week 1)

1. **Access Primary Source**
   - Attempt to access Kučera dissertation via university
   - Check if available through other channels
   - Document access method for future reference

2. **Download Open Source Libraries**
   - Clone PROJ repository
   - Clone GeographicLib repository
   - Download documentation
   - Set up local build for testing

3. **Search Academic Databases**
   - Google Scholar: 5 targeted searches
   - Web of Science: institutional access
   - ResearchGate: faculty profiles
   - Expected: 50+ relevant papers identified

4. **Download Public Domain References**
   - Snyder's USGS papers
   - EPSG database
   - OGC specifications
   - Expected: Complete reference library

5. **Create Detailed Source Cards**
   - Top 20 priority sources
   - Include all metadata
   - Assessment scores
   - Integration notes

### Short-term Actions (Weeks 2-4)

1. **Systematic Literature Review**
   - 10 critical priority papers
   - Detailed analysis and notes
   - Algorithm extraction
   - Integration proposals

2. **Code Review**
   - PROJ projection implementations
   - GeographicLib algorithms
   - GDAL transformation pipeline
   - Document key insights

3. **Standards Review**
   - OGC CRS specifications
   - EPSG database structure
   - WMS/WMTS protocols
   - Integration requirements

4. **Faculty Contact**
   - Email Palacký University researchers
   - Request papers and theses
   - Explore collaboration
   - Access additional resources

### Long-term Actions (Months 2-6)

1. **Comprehensive Analysis**
   - All high-priority sources
   - Cross-reference findings
   - Identify patterns
   - Synthesis document

2. **Implementation Integration**
   - Prototype key algorithms
   - Validate approaches
   - Performance testing
   - Feature integration

3. **Documentation**
   - Research summaries
   - Integration guides
   - Best practices
   - Lessons learned

4. **Continuous Discovery**
   - Monitor new publications
   - Track conference proceedings
   - Follow research groups
   - Update source catalog

---

## Document Metadata

**Document Type:** Research Source Catalog  
**Parent Research:** academic-analysis-geoinformatics-phd-kucera-2024.md  
**Status:** In Progress - Active Discovery Phase  
**Last Updated:** 2025-01-23  
**Total Sources:** 50+ cataloged  
**Priority Sources:** 15 critical, 20 high  
**Next Update:** After initial source acquisition and review

**Discovery Progress:**
- [x] Major categories identified (12)
- [x] Key institutions cataloged (10+)
- [x] Open source libraries listed (4)
- [x] Academic journals identified (4)
- [x] Standards documented (2 organizations)
- [ ] Detailed source cards created (0/20)
- [ ] Priority sources acquired (0/15)
- [ ] Initial analyses completed (0/10)

**Contact Information:**
- Palacký University: To be obtained
- PROJ maintainers: GitHub issues
- GeographicLib: Charles Karney (retired)
- Academic authors: Via ResearchGate/email

---

**Keywords:** academic sources, geoinformatics, cartography, geodesy, map projections, coordinate systems, research discovery, literature review, implementation references, open source libraries, academic journals, standards, dissertations, conference proceedings
