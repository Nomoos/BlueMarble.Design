# Elite Dangerous: Resource Distribution & Mining - Analysis for BlueMarble MMORPG

---
title: Elite Dangerous Resource Distribution & Mining Analysis
date: 2025-01-17
tags: [game-design, space-sim, resources, mining, economy, procedural, group-43]
status: complete
priority: medium
parent-research: research-assignment-group-43.md
---

**Source:** Elite Dangerous: Resource Distribution & Mining Mechanics  
**Developer:** Frontier Developments  
**Released:** 2014 (continuous updates)  
**Category:** GameDev-Design  
**Priority:** Medium  
**Status:** ✅ Complete  
**Lines:** 1000+  
**Related Sources:** No Man's Sky Proc-Gen, EVE Online Economy, X4 Foundations Economy

---

## Executive Summary

Elite Dangerous presents a full-scale galactic simulation with 400 billion star systems, each with procedurally-generated planetary bodies and resource distributions. This analysis examines ED's spatial resource distribution algorithms, mining mechanics, dynamic economy simulation, and supply/demand modeling to extract applicable patterns for BlueMarble's planet-scale resource systems.

**Key Takeaways for BlueMarble:**

- **Spatial Resource Distribution**: Galaxy-scale algorithms for realistic ore placement
- **Mining Gameplay Loop**: Engaging resource extraction mechanics
- **Dynamic Economy**: Supply and demand affect prices and availability
- **Resource Scarcity Design**: Tiered rarity creates exploration incentives
- **Material Sinks**: Module damage, synthesis, and engineering provide consumption
- **Hotspot Mechanics**: Concentrated resources reward knowledge and skill
- **Geological Realism**: Resource placement based on stellar/planetary formation
- **Market Simulation**: Trade routes and commodity flow between systems

**Relevance to BlueMarble:**

Elite's approach to distributing resources across a massive galaxy provides proven algorithms for BlueMarble's planet-scale distribution. Their mining gameplay loops, hotspot mechanics, and dynamic economy systems directly translate to geological simulation with appropriate scaling.

---

## Part I: Spatial Resource Distribution

### 1. Galactic Resource Distribution Algorithm

**Elite's Multi-Scale Distribution:**

```python
class EliteDangerousResourceDistribution:
    """Models Elite's galaxy-scale resource distribution"""
    
    def __init__(self):
        # Resource categories in Elite Dangerous
        self.resource_categories = {
            'core_minerals': [
                'Alexandrite', 'Benitoite', 'Grandidierite', 'Low Temperature Diamonds',
                'Monazite', 'Musgravite', 'Painite', 'Rhodplumsite', 'Serendibite', 'Void Opals'
            ],
            'laser_minerals': [
                'Platinum', 'Painite', 'Osmium', 'Gold', 'Palladium', 'Praseodymium',
                'Bromellite', 'Samarium', 'Alexandrite', 'Monazite'
            ],
            'common_minerals': [
                'Bauxite', 'Bertrandite', 'Coltan', 'Gallite', 'Indite', 'Lepidolite',
                'Rutile', 'Uraninite'
            ],
            'rare_materials': [
                'Antimony', 'Polonium', 'Ruthenium', 'Tellurium', 'Technetium', 'Yttrium'
            ]
        }
        
        # Distribution patterns based on stellar class
        self.stellar_class_resources = {
            'M_class': {  # Red dwarf stars
                'core_minerals': 0.5,
                'laser_minerals': 0.6,
                'common_minerals': 1.0,
                'rare_materials': 0.3
            },
            'K_class': {  # Orange dwarf stars
                'core_minerals': 0.7,
                'laser_minerals': 0.8,
                'common_minerals': 1.0,
                'rare_materials': 0.5
            },
            'G_class': {  # Yellow dwarf stars (like our Sun)
                'core_minerals': 1.0,
                'laser_minerals': 1.0,
                'common_minerals': 1.0,
                'rare_materials': 0.7
            },
            'F_class': {  # White stars
                'core_minerals': 1.2,
                'laser_minerals': 1.1,
                'common_minerals': 0.8,
                'rare_materials': 0.9
            },
            'A_class': {  # Blue-white stars
                'core_minerals': 1.5,
                'laser_minerals': 1.3,
                'common_minerals': 0.6,
                'rare_materials': 1.2
            }
        }
        
    def calculate_system_resources(self, star_class, distance_from_core, system_age):
        """Calculate resource availability for a star system"""
        
        # Base multipliers from stellar class
        base_multipliers = self.stellar_class_resources.get(star_class, self.stellar_class_resources['G_class'])
        
        # Distance from galactic core affects rarity
        # Core systems have more processed/rare materials
        core_distance_factor = 1.0 + (1.0 / (distance_from_core + 1.0)) * 0.5
        
        # System age affects resource depletion (in-game lore)
        age_factor = 1.0 - (system_age / 10000000000) * 0.2  # Slight depletion in old systems
        
        # Calculate final resource multipliers
        resource_multipliers = {}
        for category, base_mult in base_multipliers.items():
            # Rare materials more common near core
            if 'rare' in category:
                final_mult = base_mult * core_distance_factor * age_factor
            else:
                final_mult = base_mult * age_factor
                
            resource_multipliers[category] = final_mult
            
        return resource_multipliers
        
    def calculate_planetary_body_resources(self, body_type, composition, ring_type=None):
        """Calculate resources for specific planetary body"""
        
        body_resource_profiles = {
            'rocky': {
                'core_minerals': 0.8,
                'laser_minerals': 1.0,
                'common_minerals': 1.2,
                'rare_materials': 0.4
            },
            'metal_rich': {
                'core_minerals': 1.5,
                'laser_minerals': 2.0,
                'common_minerals': 0.8,
                'rare_materials': 1.0
            },
            'icy': {
                'core_minerals': 2.0,  # Core mining excellent in icy rings
                'laser_minerals': 0.6,
                'common_minerals': 0.5,
                'rare_materials': 0.3
            },
            'metallic': {
                'core_minerals': 1.0,
                'laser_minerals': 3.0,  # Best for laser mining
                'common_minerals': 0.6,
                'rare_materials': 0.8
            }
        }
        
        profile = body_resource_profiles.get(body_type, body_resource_profiles['rocky'])
        
        # Ring systems multiply resources
        if ring_type:
            ring_multipliers = {
                'metallic': 2.5,
                'metal_rich': 2.0,
                'icy': 1.8,
                'rocky': 1.2
            }
            ring_mult = ring_multipliers.get(ring_type, 1.0)
            profile = {k: v * ring_mult for k, v in profile.items()}
            
        return profile
        
    def generate_hotspot(self, resource_name, body_multipliers):
        """Generate a resource hotspot (concentrated region)"""
        
        # Hotspots provide 3-4x normal resources
        hotspot_multiplier = random.uniform(3.0, 4.0)
        
        # Hotspot radius (in Elite, about 20-50km)
        hotspot_radius_km = random.uniform(20, 50)
        
        return {
            'resource': resource_name,
            'multiplier': hotspot_multiplier,
            'radius_km': hotspot_radius_km,
            'center_coordinates': (
                random.uniform(-180, 180),  # Longitude
                random.uniform(-90, 90)     # Latitude
            ),
            'detection_method': 'Detailed Surface Scanner required',
            'value': 'HIGH (concentrated resource extraction)'
        }

# Example usage
elite_distribution = EliteDangerousResourceDistribution()

# Example: Sol system (G-class star)
sol_resources = elite_distribution.calculate_system_resources(
    star_class='G_class',
    distance_from_core=25000,  # Light years from galactic core
    system_age=4600000000      # Age of our Sun
)

print("Sol System Resource Distribution:")
for category, multiplier in sol_resources.items():
    print(f"  {category}: {multiplier:.2f}x")

# Example: Planetary ring resources
saturn_rings = elite_distribution.calculate_planetary_body_resources(
    body_type='icy',
    composition='water_ice',
    ring_type='icy'
)

print("\nSaturn-like Ring Resources:")
for category, multiplier in saturn_rings.items():
    print(f"  {category}: {multiplier:.2f}x")
```

**BlueMarble Application:**

```python
class BlueMarbleResourceDistribution:
    """Adapted Elite Dangerous distribution for planet-scale"""
    
    def __init__(self):
        self.biome_resource_profiles = {
            'arctic': {
                'rare_earth_metals': 1.5,
                'ice_core_minerals': 3.0,
                'fossil_fuels': 0.5,
                'geothermal': 0.3
            },
            'volcanic': {
                'rare_earth_metals': 2.0,
                'sulfur_compounds': 3.0,
                'fossil_fuels': 0.2,
                'geothermal': 5.0
            },
            'ocean_floor': {
                'rare_earth_metals': 1.2,
                'manganese_nodules': 4.0,
                'fossil_fuels': 2.0,
                'geothermal': 1.5
            },
            'desert': {
                'rare_earth_metals': 0.8,
                'salt_deposits': 3.0,
                'fossil_fuels': 2.5,
                'geothermal': 1.0
            },
            'mountain': {
                'rare_earth_metals': 2.5,
                'precious_metals': 3.0,
                'fossil_fuels': 0.5,
                'geothermal': 0.8
            }
        }
        
    def calculate_location_resources(self, latitude, longitude, altitude, biome):
        """Calculate resources at specific location"""
        
        base_profile = self.biome_resource_profiles.get(biome, {})
        
        # Altitude affects some resources
        # Higher altitude = more exposed ancient rock = more minerals
        altitude_factor = 1.0 + (altitude / 8000) * 0.5  # Everest = 8,848m
        
        # Latitude affects geological processes
        # Tectonic activity higher at certain latitudes
        tectonic_factor = abs(math.sin(math.radians(latitude * 4))) * 0.5 + 0.75
        
        modified_profile = {}
        for resource, base_mult in base_profile.items():
            if 'metals' in resource or 'minerals' in resource:
                # Metals/minerals affected by both factors
                modified_profile[resource] = base_mult * altitude_factor * tectonic_factor
            else:
                # Other resources less affected
                modified_profile[resource] = base_mult * tectonic_factor
                
        return modified_profile
        
    def generate_resource_hotspot(self, resource_type, center_lat, center_lon, intensity='high'):
        """Generate a resource hotspot on the planet"""
        
        intensity_multipliers = {
            'low': (1.5, 2.0),
            'medium': (2.0, 3.0),
            'high': (3.0, 5.0),
            'extreme': (5.0, 10.0)
        }
        
        mult_range = intensity_multipliers.get(intensity, (2.0, 3.0))
        multiplier = random.uniform(*mult_range)
        
        # Hotspot radius in kilometers
        radius_km = random.uniform(10, 100)  # Varies by resource
        
        return {
            'resource_type': resource_type,
            'center': (center_lat, center_lon),
            'radius_km': radius_km,
            'multiplier': multiplier,
            'discovery_difficulty': 'high' if intensity == 'extreme' else 'medium',
            'depletion_rate': 0.01 if intensity == 'extreme' else 0.005  # % per extraction
        }
        
    def simulate_planet_resource_map(self, grid_resolution=100):
        """Generate resource distribution map for entire planet"""
        
        resource_map = {}
        
        for lat in range(-90, 91, 180 // grid_resolution):
            for lon in range(-180, 181, 360 // grid_resolution):
                # Determine biome at this location (simplified)
                biome = self._determine_biome(lat, lon)
                
                # Calculate resources
                altitude = random.gauss(500, 1000)  # Simplified altitude
                resources = self.calculate_location_resources(lat, lon, altitude, biome)
                
                resource_map[(lat, lon)] = {
                    'biome': biome,
                    'altitude': altitude,
                    'resources': resources
                }
                
        return resource_map
        
    def _determine_biome(self, lat, lon):
        """Simplified biome determination"""
        abs_lat = abs(lat)
        
        if abs_lat > 66:
            return 'arctic'
        elif abs_lat > 50:
            return 'mountain'
        elif abs_lat > 30:
            if abs(math.sin(math.radians(lon * 7))) > 0.5:
                return 'desert'
            else:
                return 'temperate'
        else:
            return 'tropical'

bluemarble_dist = BlueMarbleResourceDistribution()

# Generate hotspots
print("BlueMarble Resource Hotspot Examples:")
hotspots = [
    bluemarble_dist.generate_resource_hotspot('rare_earth_metals', 45.0, -110.0, 'high'),
    bluemarble_dist.generate_resource_hotspot('precious_metals', -30.0, 150.0, 'extreme'),
    bluemarble_dist.generate_resource_hotspot('geothermal', 65.0, -15.0, 'medium')
]

for i, hotspot in enumerate(hotspots, 1):
    print(f"\nHotspot {i}:")
    for key, value in hotspot.items():
        print(f"  {key}: {value}")
```

---

## Part II: Mining Gameplay Mechanics

### 2. Elite Dangerous Mining Loop

**Three Mining Types:**

```python
class EliteMiningMechanics:
    """Models Elite's three mining types"""
    
    def __init__(self):
        self.mining_types = {
            'laser_mining': {
                'description': 'Continuous beam mining from asteroids',
                'equipment': ['Mining Laser', 'Refinery', 'Collector Limpets'],
                'gameplay': 'Aim laser at asteroid, collect fragments',
                'skill_requirement': 'LOW',
                'rewards': 'Consistent, moderate value',
                'time_per_asteroid': '2-5 minutes',
                'engagement': 'Relaxing, meditative'
            },
            
            'core_mining': {
                'description': 'Explosive mining of asteroid cores',
                'equipment': ['Seismic Charge Launcher', 'Abrasion Blaster', 'Refinery', 'Collector Limpets'],
                'gameplay': 'Find core asteroids, place charges, detonate, collect',
                'skill_requirement': 'HIGH',
                'rewards': 'High value, rare materials',
                'time_per_asteroid': '10-15 minutes',
                'engagement': 'Exciting, puzzle-solving'
            },
            
            'surface_mining': {
                'description': 'Mining surface deposits with abrasion',
                'equipment': ['Abrasion Blaster', 'Collector Limpets'],
                'gameplay': 'Find surface deposits, blast them off',
                'skill_requirement': 'MEDIUM',
                'rewards': 'Quick, variable value',
                'time_per_asteroid': '1-2 minutes',
                'engagement': 'Fast-paced, opportunistic'
            }
        }
        
    def simulate_laser_mining(self, asteroid_quality, mining_laser_class):
        """Simulate laser mining session"""
        
        # Base yield per second of laser fire
        base_yield = 1.0
        
        # Laser class affects yield rate (Class 1-4)
        laser_multiplier = mining_laser_class * 0.5
        
        # Asteroid quality (0.0-1.0)
        quality_multiplier = asteroid_quality
        
        # Total yield per second
        yield_per_second = base_yield * laser_multiplier * quality_multiplier
        
        # Typical laser duration: 60-120 seconds per asteroid
        duration = random.randint(60, 120)
        
        # Calculate total fragments
        total_fragments = yield_per_second * duration
        
        # Refinery processes fragments into tons of material
        # 10 fragments = 1 ton (approximate)
        tons_mined = total_fragments / 10
        
        return {
            'mining_type': 'Laser Mining',
            'duration_seconds': duration,
            'fragments_generated': total_fragments,
            'tons_refined': tons_mined,
            'skill_factor': 'LOW (aim laser, collect)',
            'excitement': 'LOW (but relaxing)'
        }
        
    def simulate_core_mining(self, fissure_count, charge_placement_skill):
        """Simulate core mining with seismic charges"""
        
        # Core asteroids have fissures (weak points)
        # Typical: 3-6 fissures
        
        # Calculate optimal yield based on charge placement
        # Perfect placement: 100% yield
        # Poor placement: asteroid destroyed or low yield
        
        optimal_charge_strength = 1.0  # Target strength
        
        # Player skill affects charge placement (0.0-1.0)
        actual_strength = charge_placement_skill
        
        # Calculate detonation result
        if actual_strength < 0.7:
            result = 'FAILED (too weak, asteroid intact)'
            yield_multiplier = 0.0
        elif actual_strength > 1.3:
            result = 'FAILED (too strong, asteroid destroyed)'
            yield_multiplier = 0.0
        elif 0.95 <= actual_strength <= 1.05:
            result = 'PERFECT (optimal yield)'
            yield_multiplier = 1.5
        else:
            result = 'SUCCESS (good yield)'
            yield_multiplier = 1.0
            
        # Base core yield: 10-20 tons
        base_yield = random.uniform(10, 20)
        final_yield = base_yield * yield_multiplier
        
        return {
            'mining_type': 'Core Mining',
            'fissures_targeted': fissure_count,
            'detonation_result': result,
            'yield_multiplier': yield_multiplier,
            'tons_mined': final_yield,
            'skill_factor': 'HIGH (puzzle-solving)',
            'excitement': 'VERY HIGH (risk/reward)'
        }
        
    def calculate_mining_profitability(self, mining_type, time_invested_minutes, 
                                       skill_level, hotspot_bonus=1.0):
        """Calculate expected profit from mining session"""
        
        # Base credits per ton by resource type (Elite Dangerous averages)
        resource_values = {
            'platinum': 250000,      # Laser mining staple
            'painite': 350000,       # Laser mining high value
            'void_opals': 600000,    # Core mining high value
            'low_temp_diamonds': 500000,  # Core mining
            'osmium': 150000,        # Common laser
            'gold': 120000           # Common laser
        }
        
        if mining_type == 'laser_mining':
            # Consistent but moderate
            tons_per_hour = 30 * skill_level  # Skill affects efficiency
            typical_resource = 'platinum' if skill_level > 0.7 else 'osmium'
            value_per_ton = resource_values[typical_resource]
            
        elif mining_type == 'core_mining':
            # High risk, high reward
            if skill_level < 0.5:
                # Low skill = many failures
                tons_per_hour = 10
                typical_resource = 'low_temp_diamonds'
            else:
                # High skill = efficient
                tons_per_hour = 40
                typical_resource = 'void_opals'
            value_per_ton = resource_values[typical_resource]
            
        else:  # surface mining
            tons_per_hour = 20
            typical_resource = 'gold'
            value_per_ton = resource_values[typical_resource]
            
        # Apply hotspot bonus (3-4x in hotspots)
        tons_per_hour *= hotspot_bonus
        
        # Calculate for time invested
        tons_mined = (tons_per_hour / 60) * time_invested_minutes
        total_credits = tons_mined * value_per_ton
        credits_per_hour = (total_credits / time_invested_minutes) * 60
        
        return {
            'mining_type': mining_type,
            'time_invested': f'{time_invested_minutes} minutes',
            'tons_mined': tons_mined,
            'credits_earned': total_credits,
            'credits_per_hour': credits_per_hour,
            'hotspot_bonus_applied': f'{hotspot_bonus}x'
        }

# Example simulations
mining = EliteMiningMechanics()

print("Elite Dangerous Mining Simulation:")
print("=" * 70)

# Laser mining
laser_result = mining.simulate_laser_mining(asteroid_quality=0.8, mining_laser_class=3)
print("\nLaser Mining Session:")
for key, value in laser_result.items():
    print(f"  {key}: {value}")

# Core mining (skilled player)
core_result = mining.simulate_core_mining(fissure_count=4, charge_placement_skill=1.02)
print("\nCore Mining Session (Skilled Player):")
for key, value in core_result.items():
    print(f"  {key}: {value}")

# Profitability comparison
print("\n\nProfitability Analysis (1 hour session):")
print("=" * 70)

scenarios = [
    ('laser_mining', 60, 0.8, 1.0, 'Standard'),
    ('laser_mining', 60, 0.8, 3.5, 'Hotspot'),
    ('core_mining', 60, 0.9, 1.0, 'Standard'),
    ('core_mining', 60, 0.9, 3.5, 'Hotspot')
]

for mining_type, time, skill, hotspot, label in scenarios:
    result = mining.calculate_mining_profitability(mining_type, time, skill, hotspot)
    print(f"\n{mining_type.replace('_', ' ').title()} - {label}:")
    print(f"  Credits/hour: {result['credits_per_hour']:,.0f}")
    print(f"  Tons mined: {result['tons_mined']:.1f}")
```

**BlueMarble Adaptation:**

```python
class BlueMebleMiningMechanics:
    """Adapted mining mechanics for BlueMarble"""
    
    def __init__(self):
        self.mining_types = {
            'surface_mining': {
                'tools': ['Pickaxe', 'Shovel', 'Hand Drill'],
                'tier': 'Basic',
                'skill_requirement': 'LOW',
                'yield': 'Low but consistent',
                'engagement': 'Entry-level, accessible'
            },
            
            'shaft_mining': {
                'tools': ['Drill', 'Explosives', 'Support Beams'],
                'tier': 'Intermediate',
                'skill_requirement': 'MEDIUM',
                'yield': 'Medium, reliable',
                'engagement': 'Infrastructure building'
            },
            
            'core_extraction': {
                'tools': ['Seismic Scanner', 'Precision Drill', 'Stabilizers'],
                'tier': 'Advanced',
                'skill_requirement': 'HIGH',
                'yield': 'High value, rare materials',
                'engagement': 'Puzzle-solving, exciting'
            }
        }
        
    def simulate_surface_mining(self, node_quality, tool_tier, player_skill):
        """Simulate surface mining"""
        
        base_yield = 10  # Units per node
        
        # Tool tier multiplier (1-5)
        tool_multiplier = 1.0 + (tool_tier - 1) * 0.2
        
        # Node quality (0.0-1.0)
        quality_multiplier = node_quality
        
        # Player skill (0.0-1.0)
        skill_multiplier = 0.5 + (player_skill * 0.5)
        
        total_yield = base_yield * tool_multiplier * quality_multiplier * skill_multiplier
        
        # Time required (inversely proportional to yield rate)
        time_minutes = 5 / tool_multiplier
        
        return {
            'yield_units': total_yield,
            'time_minutes': time_minutes,
            'efficiency': total_yield / time_minutes,
            'tool_durability_loss': 1  # 1% per node
        }
        
    def simulate_core_extraction(self, fissure_analysis_accuracy, drill_precision):
        """Simulate high-skill core extraction"""
        
        # Similar to Elite's core mining
        # Requires scanning to find core deposits
        # Precision drilling to extract without collapse
        
        if fissure_analysis_accuracy < 0.6:
            return {
                'result': 'FAILED (poor scan, missed core)',
                'yield_units': 0,
                'time_minutes': 15,
                'materials_wasted': 5
            }
            
        if drill_precision < 0.7:
            return {
                'result': 'PARTIAL (unstable extraction)',
                'yield_units': 50,
                'time_minutes': 20,
                'materials_wasted': 2
            }
            
        if drill_precision > 0.95:
            return {
                'result': 'PERFECT (optimal extraction)',
                'yield_units': 150,
                'time_minutes': 15,
                'materials_wasted': 0,
                'bonus_rare_materials': 10
            }
            
        return {
            'result': 'SUCCESS (stable extraction)',
            'yield_units': 100,
            'time_minutes': 18,
            'materials_wasted': 1
        }

bluemarble_mining = BlueMebleMiningMechanics()

# Simulate different skill levels
print("\nBlueMarble Mining Skill Progression:")
print("=" * 70)

for skill_level in [0.3, 0.6, 0.9]:
    result = bluemarble_mining.simulate_surface_mining(
        node_quality=0.8,
        tool_tier=3,
        player_skill=skill_level
    )
    print(f"\nSkill Level {skill_level:.1f}:")
    print(f"  Yield: {result['yield_units']:.1f} units")
    print(f"  Time: {result['time_minutes']:.1f} minutes")
    print(f"  Efficiency: {result['efficiency']:.2f} units/minute")
```

---

## Part III: Dynamic Economy and Supply/Demand

### 3. Elite's Market Simulation

**Supply and Demand Modeling:**

```python
class EliteEconomySimulation:
    """Models Elite Dangerous' dynamic economy"""
    
    def __init__(self):
        self.commodities = {
            'gold': {'base_price': 9401, 'volatility': 0.15},
            'silver': {'base_price': 4775, 'volatility': 0.12},
            'platinum': {'base_price': 19279, 'volatility': 0.18},
            'palladium': {'base_price': 13298, 'volatility': 0.16}
        }
        
    def calculate_market_price(self, commodity, supply_level, demand_level, 
                                 system_state='normal'):
        """Calculate current market price based on supply/demand"""
        
        base_price = self.commodities[commodity]['base_price']
        volatility = self.commodities[commodity]['volatility']
        
        # Supply affects price inversely
        # Low supply = high price
        supply_factor = 2.0 - supply_level  # supply_level 0.0-1.0
        
        # Demand affects price directly
        # High demand = high price
        demand_factor = 0.5 + demand_level  # demand_level 0.0-1.0
        
        # System state affects prices
        state_multipliers = {
            'normal': 1.0,
            'boom': 1.2,
            'bust': 0.8,
            'war': 1.5,  # Demand for materials
            'famine': 2.0  # if food commodity
        }
        state_mult = state_multipliers.get(system_state, 1.0)
        
        # Calculate final price with some randomness
        price_multiplier = supply_factor * demand_factor * state_mult
        random_variation = random.uniform(1 - volatility, 1 + volatility)
        
        final_price = base_price * price_multiplier * random_variation
        
        return {
            'commodity': commodity,
            'base_price': base_price,
            'current_price': int(final_price),
            'price_change': f'{((final_price / base_price - 1) * 100):+.1f}%',
            'supply_level': supply_level,
            'demand_level': demand_level,
            'system_state': system_state
        }
        
    def simulate_trade_route(self, commodity, buy_system_params, sell_system_params, distance_ly):
        """Simulate a trade route between two systems"""
        
        # Calculate prices at both ends
        buy_price_data = self.calculate_market_price(commodity, **buy_system_params)
        sell_price_data = self.calculate_market_price(commodity, **sell_system_params)
        
        buy_price = buy_price_data['current_price']
        sell_price = sell_price_data['current_price']
        
        # Calculate profit per ton
        profit_per_ton = sell_price - buy_price
        
        # Distance affects profitability (time is money)
        # Typical jump range: 30-60 LY, each jump takes ~1 minute
        jumps_required = math.ceil(distance_ly / 40)
        time_minutes = jumps_required * 1.5  # Include fuel scooping time
        
        # Calculate profit per hour
        # Assume 200 ton cargo capacity (typical trading ship)
        tons_capacity = 200
        total_profit = profit_per_ton * tons_capacity
        profit_per_hour = (total_profit / time_minutes) * 60 if time_minutes > 0 else 0
        
        return {
            'commodity': commodity,
            'route': f'{buy_system_params.get("system_state", "System A")} → {sell_system_params.get("system_state", "System B")}',
            'distance_ly': distance_ly,
            'jumps_required': jumps_required,
            'time_minutes': time_minutes,
            'buy_price': buy_price,
            'sell_price': sell_price,
            'profit_per_ton': profit_per_ton,
            'total_profit_200t': total_profit,
            'profit_per_hour': profit_per_hour,
            'worthwhile': profit_per_hour > 10000000  # 10M credits/hour benchmark
        }
        
    def simulate_market_fluctuation(self, commodity, initial_supply, initial_demand, 
                                      player_trades_per_day, simulation_days=30):
        """Simulate how player trading affects market over time"""
        
        supply = initial_supply
        demand = initial_demand
        price_history = []
        
        for day in range(simulation_days):
            # Calculate current price
            price_data = self.calculate_market_price(
                commodity, 
                supply_level=supply,
                demand_level=demand
            )
            price_history.append(price_data['current_price'])
            
            # Player trades affect supply/demand
            # Buying increases demand, decreases supply at destination
            # Selling increases supply, decreases demand at source
            
            # Simulate player impact
            if player_trades_per_day > 1000:
                # Significant player impact
                supply += 0.01  # Supply increases from player sales
                demand -= 0.01  # Demand decreases from player purchases
            else:
                # Normal market regeneration
                # Markets slowly return to equilibrium
                supply += (0.5 - supply) * 0.1
                demand += (0.5 - demand) * 0.1
                
            # Clamp values
            supply = max(0.0, min(1.0, supply))
            demand = max(0.0, min(1.0, demand))
            
        return {
            'commodity': commodity,
            'days_simulated': simulation_days,
            'starting_price': price_history[0],
            'ending_price': price_history[-1],
            'price_change': f'{((price_history[-1] / price_history[0] - 1) * 100):+.1f}%',
            'peak_price': max(price_history),
            'lowest_price': min(price_history),
            'volatility': (max(price_history) - min(price_history)) / price_history[0]
        }

# Simulation examples
elite_economy = EliteEconomySimulation()

print("Elite Dangerous Economy Simulation:")
print("=" * 70)

# Example 1: Market price calculation
print("\nMarket Price Examples:")
scenarios = [
    ('platinum', 0.2, 0.8, 'boom'),    # Low supply, high demand, boom
    ('platinum', 0.8, 0.2, 'normal'),  # High supply, low demand, normal
    ('gold', 0.5, 0.5, 'war'),         # Balanced, war economy
]

for commodity, supply, demand, state in scenarios:
    price = elite_economy.calculate_market_price(commodity, supply, demand, state)
    print(f"\n{commodity.title()} - Supply:{supply:.1f} Demand:{demand:.1f} State:{state}")
    print(f"  Base: {price['base_price']:,} cr")
    print(f"  Current: {price['current_price']:,} cr")
    print(f"  Change: {price['price_change']}")

# Example 2: Trade route analysis
print("\n\nTrade Route Analysis:")
route = elite_economy.simulate_trade_route(
    'platinum',
    buy_system_params={'supply_level': 0.8, 'demand_level': 0.2, 'system_state': 'normal'},
    sell_system_params={'supply_level': 0.2, 'demand_level': 0.9, 'system_state': 'boom'},
    distance_ly=150
)

for key, value in route.items():
    print(f"  {key}: {value}")

# Example 3: Market fluctuation over time
print("\n\nMarket Fluctuation Simulation (30 days):")
fluctuation = elite_economy.simulate_market_fluctuation(
    'platinum',
    initial_supply=0.3,
    initial_demand=0.7,
    player_trades_per_day=500,
    simulation_days=30
)

for key, value in fluctuation.items():
    print(f"  {key}: {value}")
```

---

## Part IV: Material Sinks in Elite Dangerous

### 4. Consumption Mechanics

**Material Sinks in Elite:**

```python
class EliteMaterialSinks:
    """Models material consumption in Elite Dangerous"""
    
    def __init__(self):
        self.sink_categories = {
            'module_damage': {
                'frequency': 'Continuous (combat, accidents)',
                'materials_consumed': 'Hull/module repair materials',
                'significance': 'Primary sink for combat players',
                'volume': 'Medium (depends on playstyle)'
            },
            
            'synthesis': {
                'frequency': 'On-demand (player choice)',
                'materials_consumed': 'Raw materials for consumables',
                'significance': 'Exploration and combat support',
                'volume': 'Low to Medium'
            },
            
            'engineering': {
                'frequency': 'Progressive (one-time per module)',
                'materials_consumed': 'Materials + Data for upgrades',
                'significance': 'Major long-term sink',
                'volume': 'Very High (hundreds of materials)'
            },
            
            'tech_broker': {
                'frequency': 'One-time (unlock special items)',
                'materials_consumed': 'Rare materials + commodities',
                'significance': 'Special equipment unlocks',
                'volume': 'High (one-time dumps)'
            },
            
            'fleet_carrier': {
                'frequency': 'Weekly (upkeep costs)',
                'materials_consumed': 'Commodities for upkeep',
                'significance': 'End-game credit sink',
                'volume': 'Very High (billions of credits)'
            }
        }
        
    def calculate_engineering_costs(self, module_type, grade):
        """Calculate materials needed for engineering upgrade"""
        
        # Engineering has 5 grades per module
        # Each grade requires more materials
        
        base_materials_needed = {
            1: {'common': 3, 'uncommon': 0, 'rare': 0},
            2: {'common': 3, 'uncommon': 2, 'rare': 0},
            3: {'common': 3, 'uncommon': 3, 'rare': 1},
            4: {'common': 4, 'uncommon': 4, 'rare': 2},
            5: {'common': 5, 'uncommon': 5, 'rare': 3}
        }
        
        materials = base_materials_needed[grade]
        
        # Multiple rolls needed for good RNG (3-10 typically)
        avg_rolls_needed = 5
        
        total_materials = {
            k: v * avg_rolls_needed for k, v in materials.items()
        }
        
        return {
            'module': module_type,
            'grade': grade,
            'materials_per_roll': materials,
            'average_rolls_needed': avg_rolls_needed,
            'total_materials_expected': total_materials,
            'time_to_gather_hours': sum(total_materials.values()) * 0.5  # ~30min per material
        }
        
    def calculate_ship_engineering_total(self):
        """Calculate total materials to fully engineer a ship"""
        
        # Typical ship has ~15 engineerable modules
        modules = [
            'Frame Shift Drive',
            'Power Plant',
            'Thrusters',
            'Shields',
            'Power Distributor',
            'Sensors',
            'Life Support',
            'Fuel Scoop',
            'Weapons (x4)',
            'Shield Boosters (x3)'
        ]
        
        # Most players engineer to Grade 5
        total_common = 0
        total_uncommon = 0
        total_rare = 0
        
        for module in modules:
            costs = self.calculate_engineering_costs(module, grade=5)
            total_common += costs['total_materials_expected']['common']
            total_uncommon += costs['total_materials_expected']['uncommon']
            total_rare += costs['total_materials_expected']['rare']
            
        return {
            'modules_to_engineer': len(modules),
            'total_common_materials': total_common,
            'total_uncommon_materials': total_uncommon,
            'total_rare_materials': total_rare,
            'estimated_hours': (total_common * 0.5 + total_uncommon * 1.0 + total_rare * 2.0),
            'significance': 'MASSIVE material sink (hundreds of hours)'
        }

elite_sinks = EliteMaterialSinks()

print("\nElite Dangerous Material Sinks Analysis:")
print("=" * 70)

# Show all sink categories
print("\nMaterial Sink Categories:")
for sink_name, sink_data in elite_sinks.sink_categories.items():
    print(f"\n{sink_name.replace('_', ' ').title()}:")
    for key, value in sink_data.items():
        print(f"  {key}: {value}")

# Calculate engineering costs
print("\n\nEngineering Cost Example (Grade 5 FSD):")
fsd_costs = elite_sinks.calculate_engineering_costs('Frame Shift Drive', grade=5)
for key, value in fsd_costs.items():
    print(f"  {key}: {value}")

# Total ship engineering
print("\n\nFull Ship Engineering Cost:")
ship_total = elite_sinks.calculate_ship_engineering_total()
for key, value in ship_total.items():
    print(f"  {key}: {value}")
```

**BlueMarble Material Sinks:**

```python
class BlueMambleMaterialSinks:
    """Adapted material sinks for BlueMarble"""
    
    def __init__(self):
        self.sinks = {
            'tool_degradation': {
                'frequency': 'Continuous (every use)',
                'consumption_rate': '5% durability per use',
                'repair_cost': '50% materials vs. crafting new',
                'significance': 'Primary continuous sink'
            },
            
            'experimental_research': {
                'frequency': 'On-demand (player choice)',
                'consumption_rate': 'Variable (10-100 materials)',
                'success_rate': '30-70%',
                'significance': 'High-risk, high-reward sink'
            },
            
            'building_construction': {
                'frequency': 'Progressive (infrastructure)',
                'consumption_rate': 'Large dumps (100-1000 materials)',
                'maintenance': 'Periodic repairs (5% materials/month)',
                'significance': 'Major milestone sink'
            },
            
            'enhancement_system': {
                'frequency': 'Progressive (incremental upgrades)',
                'consumption_rate': 'Increasing per tier',
                'success_mechanic': 'Chance-based, consume on attempt',
                'significance': 'Long-term progression sink'
            },
            
            'guild_projects': {
                'frequency': 'Event-based (community goals)',
                'consumption_rate': 'Massive (10,000+ materials)',
                'rewards': 'Shared benefits',
                'significance': 'Social/collaborative sink'
            }
        }
        
    def calculate_tool_lifecycle_cost(self, tool_value, uses_per_durability=20,
                                        usage_per_day=10, repair_rate=0.5):
        """Calculate material consumption from tool degradation"""
        
        # Tool lasts 20 uses on average
        days_until_replacement = uses_per_durability / usage_per_day
        
        # Players typically repair twice before replacing
        repairs_before_replacement = 2
        
        # Repair costs 50% of original materials
        repair_cost = tool_value * repair_rate
        
        # Total cost over tool lifecycle
        total_lifecycle_cost = tool_value + (repair_cost * repairs_before_replacement)
        
        # Materials consumed per day (amortized)
        materials_per_day = total_lifecycle_cost / (days_until_replacement * (1 + repairs_before_replacement))
        
        return {
            'tool_value': tool_value,
            'uses_per_durability': uses_per_durability,
            'days_until_replacement': days_until_replacement,
            'repairs_before_replacement': repairs_before_replacement,
            'repair_cost_each': repair_cost,
            'total_lifecycle_cost': total_lifecycle_cost,
            'materials_consumed_per_day': materials_per_day
        }

bluemarble_sinks = BlueMambleMaterialSinks()

print("\nBlueMarble Material Sinks Design:")
print("=" * 70)

# Tool lifecycle analysis
print("\nTool Lifecycle Cost Analysis (Tier 3 Pickaxe):")
tool_cost = bluemarble_sinks.calculate_tool_lifecycle_cost(
    tool_value=100,  # 100 iron
    uses_per_durability=20,
    usage_per_day=10
)

for key, value in tool_cost.items():
    print(f"  {key}: {value}")

# Calculate economy-wide sink rate
print("\n\nEconomy-Wide Material Sink Calculation:")
player_count = 10000
avg_materials_per_player_per_day = tool_cost['materials_consumed_per_day']
total_sink_per_day = player_count * avg_materials_per_player_per_day

print(f"  Players: {player_count:,}")
print(f"  Materials/player/day: {avg_materials_per_player_per_day:.1f}")
print(f"  Total sink/day: {total_sink_per_day:,.0f} materials")
print(f"  Total sink/month: {total_sink_per_day * 30:,.0f} materials")
```

---

## Discovered Sources for Phase 4

1. **"No Man's Sky: Procedural Resource Distribution" - Hello Games**
   - Priority: High
   - Focus: Planet-scale procedural generation
   - Estimated: 5-6 hours

2. **"X4 Foundations: Dynamic Economy Simulation" - Egosoft**
   - Priority: High
   - Focus: Complex supply chain simulation
   - Estimated: 6-7 hours

3. **"Star Citizen: Mining Gameplay 2.0" - Cloud Imperium**
   - Priority: Medium
   - Focus: Advanced mining mechanics
   - Estimated: 4-5 hours

4. **"Dual Universe: Player-Driven Economy" - Novaquark**
   - Priority: Medium
   - Focus: MMO economic systems
   - Estimated: 5-6 hours

5. **"Deep Rock Galactic: Mining Mission Design" - Ghost Ship Games**
   - Priority: Medium
   - Focus: Cooperative mining gameplay
   - Estimated: 3-4 hours

---

## Cross-References

- **Group 41**: Virtual economy foundations
- **Group 42**: EVE Online economy (comparison)
- **Group 43 Source 1**: Game balance principles
- **Group 43 Source 2**: Diablo III RMAH (anti-patterns)
- **Group 43 Source 4**: Satisfactory (production comparison)

---

## Recommendations for BlueMarble

### Immediate Implementation (Phase 3)

1. **Spatial Resource Distribution**
   - Implement biome-based resource profiles
   - Create hotspot generation algorithm
   - Add geological realism (altitude, latitude factors)

2. **Mining Mechanics Tiers**
   - Surface mining (low skill, consistent)
   - Shaft mining (medium skill, infrastructure)
   - Core extraction (high skill, exciting)

3. **Resource Scarcity System**
   - Common, uncommon, rare, legendary tiers
   - Procedural quality variation within tiers
   - Discovery incentives for rare deposits

### High Priority (Phase 4)

4. **Dynamic Market Simulation**
   - Supply/demand price adjustments
   - Regional market variations
   - Trade route opportunities

5. **Hotspot System**
   - Concentrated resource regions (3-5x yield)
   - Discovery through exploration/scanning
   - Gradual depletion over time

6. **Material Sink Balance**
   - Tool degradation (5% per use)
   - Enhancement systems (chance-based consumption)
   - Building construction and maintenance

### Medium Priority (Phase 5+)

7. **Advanced Mining Gameplay**
   - Seismic scanning mini-game
   - Precision extraction challenges
   - Risk/reward for skilled players

8. **Economic Telemetry**
   - Track supply/demand in real-time
   - Auto-balance mechanisms
   - Market health monitoring

---

**Status:** ✅ Complete  
**Lines:** 1,100+  
**Analysis Date:** 2025-01-17  
**Next Source:** Satisfactory Factory Building Economy  
**Group:** 43 - Economy Design & Balance
