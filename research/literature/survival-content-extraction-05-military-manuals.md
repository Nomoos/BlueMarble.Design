# Content Extraction Guide 05: Military Manuals for Large-Scale Operations

---
title: Military Manuals Content Extraction
date: 2025-01-15
tags: [military, logistics, warfare, tactics, tier3-5]
status: completed
priority: medium
source: Military Manuals Collection (22,000+ documents)
---

## Executive Summary

The Military Manuals collection (22,000+ documents) provides authentic logistics, tactics, and organizational structures for BlueMarble's warfare systems.

**Target Content:**
- Logistics and supply chain mechanics
- Unit organization and command structures
- Tactical formations and doctrine
- Fortification and engineering

**Implementation Priority:** MEDIUM - Large-scale PvP and faction gameplay

## Source Overview

### Collection Scope

**Size:** 300+ GB of military field manuals including:
- US Army Field Manuals (FM series)
- Technical Manuals (TM series)
- Training circulars
- Historical doctrine documents

**Key Documents:**
- FM 4-0: Sustainment Operations
- FM 3-0: Operations
- FM 5-34: Engineer Field Data
- FM 21-76: Survival
- TM series: Equipment maintenance

## Content Extraction Strategy

### Category 1: Logistics Systems (Tier 3-5)

**Supply Chain Mechanics:**
```python
class SupplySystem:
    def __init__(self):
        self.supply_classes = {
            1: "Subsistence (food)",
            2: "Clothing and equipment",
            3: "Fuel and energy",
            4: "Construction materials",
            5: "Ammunition",
            6: "Personal items",
            9: "Repair parts"
        }
    
    def calculate_consumption(self, unit_size, days):
        """Calculate supply needs based on FM 4-0"""
        return {
            "food": unit_size * 3 * days,  # 3 meals/day
            "water": unit_size * 15 * days,  # 15L/person/day
            "fuel": unit_size * 0.5 * days,  # 0.5L/person/day
            "ammo": unit_size * 10 * days  # 10 rounds/day combat
        }
```

**Transport Capacity:**
- Horse cart: 500 kg
- Wagon: 1,500 kg
- Truck (Tier 5): 5,000 kg
- Supply convoy rules

### Category 2: Unit Organization (Tier 3-5)

**Command Structure:**
- Squad: 8-12 soldiers
- Platoon: 30-50 soldiers (3-4 squads)
- Company: 100-200 soldiers (3-4 platoons)
- Battalion: 400-1000 soldiers (4-6 companies)

**Command Span of Control:**
```csharp
public class CommandSystem
{
    public int GetMaxSubordinates(CommandLevel level, SituationComplexity complexity)
    {
        int baseSpan = level switch
        {
            CommandLevel.Squad => 3,         // Squad leader: 3 fireteams
            CommandLevel.Platoon => 4,       // Platoon leader: 4 squads
            CommandLevel.Company => 4,       // Company commander: 4 platoons
            CommandLevel.Battalion => 5,     // Battalion commander: 5 companies
            _ => 3
        };
        
        // Adjust for complexity
        return complexity switch
        {
            SituationComplexity.Low => baseSpan + 2,
            SituationComplexity.Moderate => baseSpan,
            SituationComplexity.High => baseSpan - 1,
            SituationComplexity.Extreme => baseSpan - 2,
            _ => baseSpan
        };
    }
}

public enum CommandLevel
{
    Team,
    Squad,
    Platoon,
    Company,
    Battalion,
    Regiment,
    Division
}

public enum SituationComplexity
{
    Low,        // Routine operations
    Moderate,   // Normal combat
    High,       // Complex environment
    Extreme     // Multi-threat, chaotic
}
```

### Category 3: Tactical Formations (Tier 3-4)

**Infantry Formations:**
- Line formation (maximum firepower)
- Column formation (rapid movement)
- Wedge formation (assault)
- Defensive positions

**Movement Techniques:**
- Traveling (low threat)
- Traveling overwatch (moderate threat)
- Bounding overwatch (high threat)

### Category 4: Fortification (Tier 2-5)

**Field Fortifications:**
- Fighting positions (1-2 person)
- Trench systems
- Bunkers and strongpoints
- Anti-vehicle obstacles
- Wire obstacles

## Game Integration

### Warfare Game Mechanics

**Player Roles:**
- Private: Individual combat
- Squad Leader: 8-12 NPCs/players
- Platoon Leader: 30-50 troops
- Company Commander: 100-200 troops

**Supply Management:**
```json
{
  "company_logistics": {
    "personnel": 120,
    "daily_consumption": {
      "food": 360,
      "water": 1800,
      "ammunition": 1200
    },
    "supply_points_needed": 3,
    "transport_capacity": {
      "wagons": 5,
      "carrying_capacity_kg": 7500
    }
  }
}
```

## Extraction Methodology

### Step 1: Document Selection

Priority documents:
1. FM 4-0 (Sustainment) - Logistics mechanics
2. FM 3-0 (Operations) - Tactical doctrine
3. FM 5-34 (Engineer Field Data) - Fortifications
4. FM 7-8 (Infantry Platoon and Squad) - Unit tactics

### Step 2: Text Extraction

```python
import PyPDF2
import pdfplumber

def extract_text_from_pdf(pdf_path):
    """Extract text content from PDF"""
    with open(pdf_path, 'rb') as file:
        reader = PyPDF2.PdfReader(file)
        text = ""
        
        for page in reader.pages:
            text += page.extract_text()
    
    return text

def extract_tables_from_pdf(pdf_path):
    """Extract tables from PDF"""
    tables = []
    
    with pdfplumber.open(pdf_path) as pdf:
        for page in pdf.pages:
            page_tables = page.extract_tables()
            tables.extend(page_tables)
    
    return tables

# Example usage
fm_4_0_text = extract_text_from_pdf("pdfs/FM_4-0.pdf")
supply_tables = extract_tables_from_pdf("pdfs/FM_4-0.pdf")
```

### Step 3: Content Analysis

**Pattern Matching for Mechanics:**
```python
import re

def extract_supply_rates(text):
    """Extract supply consumption rates"""
    # Look for patterns like "X units per Y time"
    pattern = r"(\d+\.?\d*)\s*(\w+)\s+per\s+(\w+)"
    matches = re.findall(pattern, text)
    return matches

def extract_organization(text):
    """Extract unit organization structures"""
    # Look for tables of organization
    pattern = r"(Squad|Platoon|Company|Battalion)\s*:\s*(\d+)\s*(personnel|soldiers)"
    matches = re.findall(pattern, text, re.IGNORECASE)
    return matches
```

### Step 4: JSON Conversion

```python
import json

def convert_to_game_data(mechanics, category):
    """Convert extracted mechanics to game data format"""
    game_data = {
        "category": category,
        "mechanics": [],
        "metadata": {
            "source": "FM 4-0",
            "extraction_date": "2025-01-15",
            "game_tier": "3-5"
        }
    }
    
    for mechanic in mechanics:
        game_data["mechanics"].append({
            "name": extract_mechanic_name(mechanic),
            "description": mechanic,
            "requirements": extract_requirements(mechanic),
            "effects": extract_effects(mechanic)
        })
    
    return game_data

# Convert and save
logistics_data = convert_to_game_data(logistics_mechanics["systems"], "logistics")
with open("game_data_logistics.json", "w") as f:
    json.dump(logistics_data, f, indent=2)
```

## Deliverables

1. **mechanics_logistics.json** - Supply chain systems
2. **mechanics_organization.json** - Unit structures and command
3. **mechanics_tactics.json** - Formation and movement
4. **mechanics_fortification.json** - Defensive structures
5. **warfare_system_documentation.md** - Complete integration guide

## Success Metrics

- **Authenticity:** 100% based on real military doctrine
- **Balance:** Scaled appropriately for game
- **Complexity:** Manageable for players
- **Integration:** Works with existing systems

## Tools and Setup

### Required Tools

1. **PDF Readers and Extractors**
   - Adobe Acrobat Reader DC
   - PDFtk (PDF toolkit for command-line operations)
   - Tabula (for extracting tables from PDFs)

2. **Text Processing**
   - Python with PyPDF2 library
   - Regular expressions for pattern matching
   - Natural Language Processing (spaCy or NLTK)

3. **Data Organization**
   - Notion or Obsidian for knowledge management
   - Spreadsheet software for data organization
   - JSON/YAML editors for game data formats

4. **Version Control**
   - Git for tracking extraction progress
   - GitHub/GitLab for collaboration

---

**Document Status:** Complete
**Last Updated:** 2025-01-15
**Priority:** MEDIUM
**Estimated Time:** 8 weeks
