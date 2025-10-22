# Satisfactory: Factory Building Economy - Analysis for BlueMarble MMORPG

---
title: Satisfactory Factory Building and Production Chains Analysis
date: 2025-01-17
tags: [game-design, factory-building, production-chains, automation, economy, group-43]
status: complete
priority: medium
parent-research: research-assignment-group-43.md
---

**Source:** Satisfactory: Factory Building and Production Chains  
**Developer:** Coffee Stain Studios  
**Released:** Early Access 2019, Full Release 2024  
**Category:** GameDev-Design  
**Priority:** Medium  
**Status:** ✅ Complete  
**Lines:** 1000+  
**Related Sources:** Factorio Production, Minecraft Modded Tech, Dyson Sphere Program

---

## Executive Summary

Satisfactory is a first-person factory-building game where players extract resources from a planet and build increasingly complex production chains to manufacture parts for "Project Assembly." This analysis examines Satisfactory's resource node balance, production chain design, material transformation ratios, consumption rates, and player-driven optimization to extract applicable patterns for BlueMarble's crafting and material systems.

**Key Takeaways for BlueMarble:**

- **Production Chain Design**: Multi-tier material transformation creates depth
- **Resource Node Balance**: Fixed nodes with variable extraction rates
- **Input/Output Ratios**: Carefully tuned throughput mathematics
- **Building as Material Sink**: Construction consumes significant materials
- **Automation Systems**: Player-built infrastructure for resource processing
- **Optimization Gameplay**: Players solve logistical puzzles
- **Power Management**: Energy as limiting factor for production
- **Progression Gating**: Tech tiers unlock new production capabilities

**Relevance to BlueMarble:**

Satisfactory's production chain philosophy directly applies to BlueMarble's crafting systems. Their approach to resource node placement, material transformation ratios, and building construction costs provides proven frameworks for creating engaging economic gameplay loops.

---

## Part I: Resource Node System

### 1. Fixed Resource Nodes with Variable Extraction

**Satisfactory's Node Design:**

```python
class SatisfactoryResourceNodes:
    """Models Satisfactory's resource node system"""
    
    def __init__(self):
        # Node purity levels
        self.node_purities = {
            'impure': {
                'base_yield': 30,  # items per minute
                'multiplier': 0.5,
                'rarity': 'common'
            },
            'normal': {
                'base_yield': 60,
                'multiplier': 1.0,
                'rarity': 'common'
            },
            'pure': {
                'base_yield': 120,
                'multiplier': 2.0,
                'rarity': 'rare'
            }
        }
        
        # Extractor tiers multiply yield
        self.extractor_tiers = {
            'mk1': {
                'multiplier': 1.0,
                'power_consumption': 5,  # MW
                'unlock_tier': 'Tier 0'
            },
            'mk2': {
                'multiplier': 2.0,
                'power_consumption': 12,
                'unlock_tier': 'Tier 3'
            },
            'mk3': {
                'multiplier': 4.0,
                'power_consumption': 30,
                'unlock_tier': 'Tier 7'
            }
        }
        
        # Overclock system (100%-250%)
        self.overclock_mechanics = {
            'min_clock': 1.0,    # 100%
            'max_clock': 2.5,    # 250%
            'power_formula': lambda clock_speed: clock_speed ** 1.6,  # Exponential power cost
        }
        
    def calculate_node_output(self, node_purity, extractor_tier, overclock=1.0):
        """Calculate actual output from a resource node"""
        
        node_data = self.node_purities[node_purity]
        extractor_data = self.extractor_tiers[extractor_tier]
        
        # Base yield * purity * extractor tier * overclock
        items_per_minute = (
            node_data['base_yield'] * 
            node_data['multiplier'] * 
            extractor_data['multiplier'] * 
            overclock
        )
        
        # Calculate power consumption with overclock
        base_power = extractor_data['power_consumption']
        power_multiplier = self.overclock_mechanics['power_formula'](overclock)
        total_power = base_power * power_multiplier
        
        return {
            'node_purity': node_purity,
            'extractor_tier': extractor_tier,
            'overclock_percentage': f'{overclock * 100:.0f}%',
            'items_per_minute': items_per_minute,
            'power_consumption_mw': total_power,
            'efficiency': items_per_minute / total_power  # items per MW
        }
        
    def optimize_power_efficiency(self, node_purity, extractor_tier):
        """Find optimal overclock for power efficiency"""
        
        efficiencies = []
        
        for overclock in [1.0, 1.25, 1.5, 1.75, 2.0, 2.5]:
            result = self.calculate_node_output(node_purity, extractor_tier, overclock)
            efficiencies.append({
                'overclock': overclock,
                'efficiency': result['efficiency'],
                'output': result['items_per_minute'],
                'power': result['power_consumption_mw']
            })
            
        # Sort by efficiency (highest first)
        efficiencies.sort(key=lambda x: x['efficiency'], reverse=True)
        
        return efficiencies
        
    def calculate_total_node_potential(self, planet_node_distribution):
        """Calculate total resource production potential"""
        
        total_output = {}
        
        for resource, nodes in planet_node_distribution.items():
            resource_total = 0
            
            for node_purity, count in nodes.items():
                # Assume all nodes have Mk3 extractors at 100% clock
                node_output = self.calculate_node_output(node_purity, 'mk3', 1.0)
                resource_total += node_output['items_per_minute'] * count
                
            total_output[resource] = resource_total
            
        return total_output

# Example usage
satisfactory_nodes = SatisfactoryResourceNodes()

print("Satisfactory Resource Node Analysis:")
print("=" * 70)

# Test different configurations
configs = [
    ('impure', 'mk1', 1.0),
    ('normal', 'mk1', 1.0),
    ('pure', 'mk1', 1.0),
    ('pure', 'mk3', 1.0),
    ('pure', 'mk3', 2.5),  # Max overclock
]

print("\nNode Output Configurations:")
for purity, extractor, overclock in configs:
    result = satisfactory_nodes.calculate_node_output(purity, extractor, overclock)
    print(f"\n{purity.title()} + {extractor.upper()} @ {overclock*100:.0f}%:")
    print(f"  Output: {result['items_per_minute']:.0f} items/min")
    print(f"  Power: {result['power_consumption_mw']:.1f} MW")
    print(f"  Efficiency: {result['efficiency']:.1f} items/MW")

# Optimization analysis
print("\n\nPower Efficiency Optimization (Pure Node + Mk3):")
print("=" * 70)
optimization = satisfactory_nodes.optimize_power_efficiency('pure', 'mk3')

print(f"{'Overclock':<12} {'Output/min':<12} {'Power (MW)':<12} {'Efficiency':<12}")
print("-" * 70)
for opt in optimization:
    print(f"{opt['overclock']*100:<12.0f} {opt['output']:<12.0f} {opt['power']:<12.1f} {opt['efficiency']:<12.2f}")

print("\n💡 KEY INSIGHT: 100% clock is most power-efficient!")
print("   Overclocking trades efficiency for throughput")
```

**BlueMarble Adaptation:**

```python
class BlueMarbleResourceNodes:
    """Adapted resource node system for BlueMarble"""
    
    def __init__(self):
        self.node_qualities = {
            'poor': {'yield_multiplier': 0.5, 'durability': 100},
            'average': {'yield_multiplier': 1.0, 'durability': 150},
            'rich': {'yield_multiplier': 2.0, 'durability': 200},
            'exceptional': {'yield_multiplier': 4.0, 'durability': 250}
        }
        
        self.extraction_tools = {
            'basic_pickaxe': {'efficiency': 1.0, 'durability_cost': 1},
            'drill': {'efficiency': 2.0, 'durability_cost': 0.5},
            'laser_drill': {'efficiency': 5.0, 'durability_cost': 0.2},
            'automated_extractor': {'efficiency': 10.0, 'durability_cost': 0.1}
        }
        
    def calculate_extraction_yield(self, node_quality, tool_type, player_skill=0.5):
        """Calculate yield from extracting a node"""
        
        node_data = self.node_qualities[node_quality]
        tool_data = self.extraction_tools[tool_type]
        
        base_yield = 10  # Units per extraction
        
        yield_amount = (
            base_yield * 
            node_data['yield_multiplier'] * 
            tool_data['efficiency'] * 
            (0.5 + player_skill * 0.5)  # Skill affects 50% of yield
        )
        
        # Node durability reduction
        durability_loss = tool_data['durability_cost']
        
        # Tool durability loss
        tool_durability_loss = 1  # 1% per use
        
        return {
            'yield_units': yield_amount,
            'node_durability_loss': durability_loss,
            'tool_durability_loss': tool_durability_loss,
            'extractions_until_node_depleted': node_data['durability'] / durability_loss
        }
        
    def calculate_node_lifetime_value(self, node_quality, tool_type):
        """Calculate total value from a node over its lifetime"""
        
        extraction = self.calculate_extraction_yield(node_quality, tool_type)
        
        total_extractions = extraction['extractions_until_node_depleted']
        total_yield = extraction['yield_units'] * total_extractions
        
        return {
            'node_quality': node_quality,
            'tool_type': tool_type,
            'total_extractions': total_extractions,
            'yield_per_extraction': extraction['yield_units'],
            'total_lifetime_yield': total_yield,
            'tool_replacements_needed': (total_extractions * extraction['tool_durability_loss']) / 100
        }

bluemarble_nodes = BlueMarbleResourceNodes()

print("\n\nBlueMarble Resource Node Design:")
print("=" * 70)

# Node lifetime analysis
for quality in ['poor', 'average', 'rich', 'exceptional']:
    for tool in ['basic_pickaxe', 'automated_extractor']:
        result = bluemarble_nodes.calculate_node_lifetime_value(quality, tool)
        print(f"\n{quality.title()} Node + {tool.replace('_', ' ').title()}:")
        print(f"  Total Extractions: {result['total_extractions']:.0f}")
        print(f"  Lifetime Yield: {result['total_lifetime_yield']:.0f} units")
        print(f"  Tools Needed: {result['tool_replacements_needed']:.1f}")
```

---

## Part II: Production Chain Design

### 2. Multi-Tier Material Transformation

**Satisfactory's Production Tiers:**

```python
class SatisfactoryProductionChains:
    """Models Satisfactory's production chain complexity"""
    
    def __init__(self):
        # Production recipes by tier
        self.recipes = {
            # Tier 0-1: Basic materials
            'iron_ingot': {
                'inputs': {'iron_ore': 30},  # per minute
                'outputs': {'iron_ingot': 30},
                'building': 'smelter',
                'tier': 0,
                'production_time': 2.0  # seconds per craft
            },
            
            'iron_plate': {
                'inputs': {'iron_ingot': 30},
                'outputs': {'iron_plate': 20},
                'building': 'constructor',
                'tier': 0
            },
            
            'iron_rod': {
                'inputs': {'iron_ingot': 15},
                'outputs': {'iron_rod': 15},
                'building': 'constructor',
                'tier': 0
            },
            
            # Tier 2: Intermediate parts
            'reinforced_iron_plate': {
                'inputs': {'iron_plate': 30, 'screw': 60},
                'outputs': {'reinforced_iron_plate': 5},
                'building': 'assembler',
                'tier': 2
            },
            
            'rotor': {
                'inputs': {'iron_rod': 20, 'screw': 100},
                'outputs': {'rotor': 4},
                'building': 'assembler',
                'tier': 2
            },
            
            # Tier 4: Advanced parts
            'modular_frame': {
                'inputs': {'reinforced_iron_plate': 3, 'iron_rod': 12},
                'outputs': {'modular_frame': 2},
                'building': 'assembler',
                'tier': 4
            },
            
            # Tier 6: Complex components
            'heavy_modular_frame': {
                'inputs': {'modular_frame': 10, 'steel_pipe': 30, 'encased_industrial_beam': 10, 'screw': 200},
                'outputs': {'heavy_modular_frame': 2},
                'building': 'manufacturer',
                'tier': 6
            }
        }
        
    def calculate_production_requirements(self, target_item, target_rate):
        """Calculate all inputs needed to produce target item at target rate"""
        
        recipe = self.recipes.get(target_item)
        if not recipe:
            return {}
            
        # Calculate how many buildings needed
        output_per_building = recipe['outputs'][target_item]
        buildings_needed = target_rate / output_per_building
        
        # Calculate input requirements
        requirements = {}
        for input_item, input_rate in recipe['inputs'].items():
            required_rate = input_rate * buildings_needed
            requirements[input_item] = required_rate
            
        return {
            'target': target_item,
            'target_rate': target_rate,
            'buildings_needed': buildings_needed,
            'building_type': recipe['building'],
            'inputs_required': requirements
        }
        
    def calculate_full_production_tree(self, target_item, target_rate, depth=0):
        """Recursively calculate full production tree"""
        
        recipe = self.recipes.get(target_item)
        if not recipe:
            # Base resource (ore)
            return {
                'item': target_item,
                'rate': target_rate,
                'type': 'raw_resource'
            }
            
        requirements = self.calculate_production_requirements(target_item, target_rate)
        
        # Recursively calculate sub-requirements
        sub_trees = {}
        for input_item, input_rate in requirements['inputs_required'].items():
            sub_trees[input_item] = self.calculate_full_production_tree(
                input_item, input_rate, depth + 1
            )
            
        return {
            'item': target_item,
            'rate': target_rate,
            'buildings': requirements['buildings_needed'],
            'building_type': requirements['building_type'],
            'inputs': sub_trees,
            'depth': depth
        }
        
    def calculate_ratios(self, item_a, item_b):
        """Calculate optimal production ratios between two items"""
        
        recipe_a = self.recipes.get(item_a)
        recipe_b = self.recipes.get(item_b)
        
        if not recipe_a or not recipe_b:
            return None
            
        # Find common inputs
        common_inputs = set(recipe_a['inputs'].keys()) & set(recipe_b['inputs'].keys())
        
        if common_inputs:
            ratios = {}
            for common_input in common_inputs:
                rate_a = recipe_a['inputs'][common_input] / recipe_a['outputs'][item_a]
                rate_b = recipe_b['inputs'][common_input] / recipe_b['outputs'][item_b]
                ratios[common_input] = rate_a / rate_b
            return ratios
        else:
            return {'note': 'No common inputs'}
            
    def optimize_production_line(self, available_resources, target_outputs):
        """Optimize production to maximize target outputs with available resources"""
        
        # Simplified linear programming problem
        # In real implementation, would use scipy.optimize or similar
        
        print("Production Line Optimization:")
        print("=" * 70)
        print(f"Available Resources: {available_resources}")
        print(f"Target Outputs: {target_outputs}")
        print("\nThis would require linear programming optimization...")
        print("Simplified: allocate resources based on priority")

# Example usage
production = SatisfactoryProductionChains()

print("\n\nSatisfactory Production Chain Analysis:")
print("=" * 70)

# Calculate requirements for complex item
print("\nProduction Requirements: 10 Heavy Modular Frames/min")
print("-" * 70)

tree = production.calculate_full_production_tree('heavy_modular_frame', 10)

def print_tree(tree, indent=0):
    """Print production tree"""
    prefix = "  " * indent
    if tree.get('type') == 'raw_resource':
        print(f"{prefix}📦 {tree['item']}: {tree['rate']:.1f}/min (raw)")
    else:
        print(f"{prefix}🏭 {tree['item']}: {tree['rate']:.1f}/min")
        print(f"{prefix}   Buildings: {tree['buildings']:.1f} {tree['building_type']}s")
        if tree.get('inputs'):
            for input_item, input_tree in tree['inputs'].items():
                print_tree(input_tree, indent + 1)

print_tree(tree)

# Calculate ratios
print("\n\nOptimal Production Ratios:")
print("-" * 70)
print("Iron Plate vs Iron Rod (both use iron ingots):")
ratios = production.calculate_ratios('iron_plate', 'iron_rod')
print(f"  Ratios: {ratios}")
```

**BlueMarble Production Chains:**

```python
class BlueMarbleProductionChains:
    """Adapted production chains for BlueMarble"""
    
    def __init__(self):
        self.recipes = {
            # Tier 1: Basic processing
            'refined_ore': {
                'inputs': {'raw_ore': 10},
                'outputs': {'refined_ore': 5},
                'facility': 'smelter',
                'time_seconds': 30,
                'skill_requirement': 'none'
            },
            
            'metal_bar': {
                'inputs': {'refined_ore': 5},
                'outputs': {'metal_bar': 3},
                'facility': 'forge',
                'time_seconds': 60,
                'skill_requirement': 'basic_metallurgy'
            },
            
            # Tier 2: Component crafting
            'basic_tool': {
                'inputs': {'metal_bar': 3, 'wood': 2},
                'outputs': {'basic_tool': 1},
                'facility': 'workshop',
                'time_seconds': 120,
                'skill_requirement': 'basic_crafting'
            },
            
            'reinforced_plate': {
                'inputs': {'metal_bar': 5, 'rivets': 10},
                'outputs': {'reinforced_plate': 1},
                'facility': 'assembly_table',
                'time_seconds': 90,
                'skill_requirement': 'advanced_crafting'
            },
            
            # Tier 3: Advanced items
            'advanced_tool': {
                'inputs': {'reinforced_plate': 2, 'metal_bar': 5, 'circuitry': 1},
                'outputs': {'advanced_tool': 1},
                'facility': 'advanced_workshop',
                'time_seconds': 300,
                'skill_requirement': 'expert_crafting'
            }
        }
        
    def calculate_crafting_time_efficiency(self, recipe_name, automation_level=1.0):
        """Calculate time efficiency with automation"""
        
        recipe = self.recipes[recipe_name]
        
        base_time = recipe['time_seconds']
        automated_time = base_time / automation_level
        
        # Calculate throughput
        output_amount = list(recipe['outputs'].values())[0]
        items_per_hour = (3600 / automated_time) * output_amount
        
        return {
            'recipe': recipe_name,
            'base_time': base_time,
            'automated_time': automated_time,
            'automation_level': automation_level,
            'items_per_hour': items_per_hour
        }
        
    def calculate_material_efficiency(self, recipe_name):
        """Calculate material input/output efficiency"""
        
        recipe = self.recipes[recipe_name]
        
        total_inputs = sum(recipe['inputs'].values())
        total_outputs = sum(recipe['outputs'].values())
        
        efficiency_ratio = total_outputs / total_inputs
        
        return {
            'recipe': recipe_name,
            'total_inputs': total_inputs,
            'total_outputs': total_outputs,
            'efficiency_ratio': efficiency_ratio,
            'material_loss': (1 - efficiency_ratio) * 100
        }

bluemarble_production = BlueMarbleProductionChains()

print("\n\nBlueMarble Production Chain Design:")
print("=" * 70)

# Time efficiency analysis
print("\nCrafting Time Efficiency (with automation):")
for recipe in ['refined_ore', 'metal_bar', 'advanced_tool']:
    for auto_level in [1.0, 2.0, 5.0]:
        result = bluemarble_production.calculate_crafting_time_efficiency(recipe, auto_level)
        print(f"\n{recipe.replace('_', ' ').title()} @ {auto_level}x automation:")
        print(f"  Time: {result['automated_time']:.0f}s per craft")
        print(f"  Throughput: {result['items_per_hour']:.1f} items/hour")

# Material efficiency
print("\n\nMaterial Efficiency Analysis:")
print("-" * 70)
for recipe in bluemarble_production.recipes.keys():
    efficiency = bluemarble_production.calculate_material_efficiency(recipe)
    print(f"\n{recipe.replace('_', ' ').title()}:")
    print(f"  Input → Output: {efficiency['total_inputs']} → {efficiency['total_outputs']}")
    print(f"  Efficiency: {efficiency['efficiency_ratio']:.2f}")
    print(f"  Loss: {efficiency['material_loss']:.1f}%")
```

---

## Part III: Building Construction as Material Sink

### 3. Construction Costs and Infrastructure

**Satisfactory's Building System:**

```python
class SatisfactoryBuildingCosts:
    """Models construction costs in Satisfactory"""
    
    def __init__(self):
        self.buildings = {
            # Basic production
            'miner_mk1': {
                'cost': {'iron_plate': 10, 'cable': 10},
                'footprint': '8m x 8m',
                'power_consumption': 5,
                'function': 'Extract resources from nodes'
            },
            
            'smelter': {
                'cost': {'iron_rod': 5, 'wire': 8},
                'footprint': '6m x 9m',
                'power_consumption': 4,
                'function': 'Smelt ores into ingots'
            },
            
            'constructor': {
                'cost': {'reinforced_iron_plate': 2, 'cable': 10},
                'footprint': '8m x 10m',
                'power_consumption': 4,
                'function': 'Craft basic parts'
            },
            
            'assembler': {
                'cost': {'reinforced_iron_plate': 8, 'rotor': 4, 'cable': 10},
                'footprint': '10m x 15m',
                'power_consumption': 15,
                'function': 'Combine parts into components'
            },
            
            # Advanced production
            'manufacturer': {
                'cost': {'modular_frame': 10, 'rotor': 10, 'cable': 50},
                'footprint': '18m x 20m',
                'power_consumption': 55,
                'function': 'Complex multi-input crafting'
            },
            
            # Infrastructure
            'conveyor_belt_mk1': {
                'cost_per_meter': {'iron_plate': 0.1},
                'throughput': 60,  # items per minute
                'function': 'Transport items'
            },
            
            'power_pole': {
                'cost': {'iron_rod': 1, 'wire': 2},
                'range': '30m',
                'function': 'Distribute power'
            },
            
            'foundation': {
                'cost': {'concrete': 5},
                'size': '8m x 8m',
                'function': 'Building platform'
            }
        }
        
    def calculate_factory_construction_cost(self, factory_layout):
        """Calculate total materials needed for a factory"""
        
        total_costs = {}
        
        for building, count in factory_layout.items():
            building_data = self.buildings.get(building)
            if not building_data:
                continue
                
            cost_dict = building_data.get('cost') or building_data.get('cost_per_meter', {})
            
            for material, amount in cost_dict.items():
                total_costs[material] = total_costs.get(material, 0) + (amount * count)
                
        return total_costs
        
    def calculate_starter_factory_cost(self):
        """Calculate cost of typical starter factory"""
        
        starter_layout = {
            'miner_mk1': 3,        # 3 miners on 3 nodes
            'smelter': 6,          # 6 smelters to process 3 nodes
            'constructor': 4,      # 4 constructors for parts
            'assembler': 2,        # 2 assemblers for components
            'conveyor_belt_mk1': 200,  # 200 meters of belts
            'power_pole': 10,      # 10 power poles
            'foundation': 50       # 50 foundation pieces
        }
        
        costs = self.calculate_factory_construction_cost(starter_layout)
        
        return {
            'factory_type': 'Starter Factory',
            'buildings': starter_layout,
            'material_costs': costs,
            'estimated_build_time': '2-3 hours (manual)',
            'power_required': '100 MW (approximate)'
        }
        
    def calculate_mid_game_factory_cost(self):
        """Calculate cost of mid-game production facility"""
        
        mid_game_layout = {
            'miner_mk1': 10,
            'smelter': 20,
            'constructor': 15,
            'assembler': 10,
            'manufacturer': 5,
            'conveyor_belt_mk1': 1000,
            'power_pole': 50,
            'foundation': 500
        }
        
        costs = self.calculate_factory_construction_cost(mid_game_layout)
        
        return {
            'factory_type': 'Mid-Game Factory',
            'buildings': mid_game_layout,
            'material_costs': costs,
            'estimated_build_time': '10-15 hours',
            'power_required': '500 MW (approximate)'
        }

satisfactory_building = SatisfactoryBuildingCosts()

print("\n\nSatisfactory Building Cost Analysis:")
print("=" * 70)

# Starter factory
print("\nStarter Factory Construction Cost:")
starter = satisfactory_building.calculate_starter_factory_cost()
print(f"Buildings: {starter['buildings']}")
print(f"\nMaterial Costs:")
for material, amount in starter['material_costs'].items():
    print(f"  {material}: {amount:,.0f}")
print(f"\nBuild Time: {starter['estimated_build_time']}")
print(f"Power: {starter['power_required']}")

# Mid-game factory
print("\n\nMid-Game Factory Construction Cost:")
mid_game = satisfactory_building.calculate_mid_game_factory_cost()
print(f"\nMaterial Costs:")
for material, amount in mid_game['material_costs'].items():
    print(f"  {material}: {amount:,.0f}")
print(f"\nBuild Time: {mid_game['estimated_build_time']}")
print(f"Power: {mid_game['power_required']}")

print("\n💡 KEY INSIGHT: Building construction is a MASSIVE material sink!")
print("   Mid-game factories consume thousands of materials")
```

**BlueMarble Building Costs:**

```python
class BlueMarmbleBuildingCosts:
    """Adapted building costs for BlueMarble"""
    
    def __init__(self):
        self.structures = {
            'mining_shack': {
                'cost': {'wood': 50, 'stone': 100, 'iron': 20},
                'build_time_hours': 1,
                'function': 'Basic mining operation base'
            },
            
            'smeltery': {
                'cost': {'stone': 500, 'iron': 200, 'clay': 100},
                'build_time_hours': 4,
                'function': 'Ore processing facility'
            },
            
            'workshop': {
                'cost': {'wood': 200, 'stone': 300, 'iron': 150},
                'build_time_hours': 3,
                'function': 'Crafting facility'
            },
            
            'automated_extractor': {
                'cost': {'iron': 500, 'steel': 200, 'circuitry': 50},
                'build_time_hours': 8,
                'maintenance_cost_per_week': {'iron': 50, 'power_cells': 10},
                'function': 'Automated resource extraction'
            },
            
            'refinery': {
                'cost': {'steel': 1000, 'concrete': 500, 'circuitry': 200},
                'build_time_hours': 12,
                'maintenance_cost_per_week': {'steel': 100, 'power_cells': 50},
                'function': 'Advanced material processing'
            }
        }
        
    def calculate_infrastructure_investment(self, player_goal='mid_game'):
        """Calculate investment needed for infrastructure"""
        
        if player_goal == 'starter':
            buildings = {
                'mining_shack': 1,
                'smeltery': 1,
                'workshop': 1
            }
        elif player_goal == 'mid_game':
            buildings = {
                'mining_shack': 3,
                'smeltery': 2,
                'workshop': 2,
                'automated_extractor': 2
            }
        else:  # end_game
            buildings = {
                'mining_shack': 5,
                'smeltery': 4,
                'workshop': 4,
                'automated_extractor': 10,
                'refinery': 2
            }
            
        total_costs = {}
        total_build_time = 0
        
        for building, count in buildings.items():
            building_data = self.structures[building]
            
            # Add costs
            for material, amount in building_data['cost'].items():
                total_costs[material] = total_costs.get(material, 0) + (amount * count)
                
            # Add build time
            total_build_time += building_data['build_time_hours'] * count
            
        return {
            'goal': player_goal,
            'buildings': buildings,
            'material_costs': total_costs,
            'build_time_hours': total_build_time,
            'material_sink_significance': 'PRIMARY (thousands of materials consumed)'
        }

bluemarble_building = BlueMarmbleBuildingCosts()

print("\n\nBlueMarble Building Cost Design:")
print("=" * 70)

for goal in ['starter', 'mid_game', 'end_game']:
    infrastructure = bluemarble_building.calculate_infrastructure_investment(goal)
    print(f"\n{goal.upper().replace('_', ' ')} Infrastructure Investment:")
    print(f"Buildings: {infrastructure['buildings']}")
    print(f"Material Costs:")
    for material, amount in infrastructure['material_costs'].items():
        print(f"  {material}: {amount:,}")
    print(f"Build Time: {infrastructure['build_time_hours']} hours")
```

---

## Part IV: Optimization Gameplay

### 4. Player-Driven Efficiency Optimization

**The Optimization Loop:**

```python
class SatisfactoryOptimizationGameplay:
    """Models the optimization gameplay loop"""
    
    def __init__(self):
        self.optimization_targets = {
            'maximize_throughput': 'Produce as much as possible',
            'minimize_power': 'Achieve target with least power',
            'balance_production': 'Match input/output ratios perfectly',
            'minimize_footprint': 'Compact factory design',
            'aesthetic_design': 'Beautiful, organized layouts'
        }
        
    def calculate_throughput_optimization(self, production_line):
        """Calculate how to maximize throughput"""
        
        # Identify bottlenecks
        bottlenecks = []
        
        for stage, rate in production_line.items():
            if rate < max(production_line.values()):
                bottlenecks.append({
                    'stage': stage,
                    'rate': rate,
                    'deficit': max(production_line.values()) - rate,
                    'recommendation': 'Add more machines or overclock'
                })
                
        return {
            'current_throughput': min(production_line.values()),
            'potential_throughput': max(production_line.values()),
            'efficiency': (min(production_line.values()) / max(production_line.values())) * 100,
            'bottlenecks': bottlenecks
        }
        
    def calculate_power_optimization(self, machines_and_power):
        """Optimize for power efficiency"""
        
        # Calculate current power usage
        total_power = sum(machines_and_power.values())
        
        # Find opportunities to underclock (reduce power)
        opportunities = []
        
        for machine, power in machines_and_power.items():
            # Underclocking to 50% saves significant power
            # Power = clock_speed^1.6
            underclock_power = power * (0.5 ** 1.6)
            power_saved = power - underclock_power
            
            opportunities.append({
                'machine': machine,
                'current_power': power,
                'underclocked_power': underclock_power,
                'power_saved': power_saved,
                'output_reduction': '50%',
                'recommendation': 'Underclock if excess capacity'
            })
            
        return {
            'total_power': total_power,
            'optimization_opportunities': opportunities
        }
        
    def calculate_perfect_ratios(self, recipe_chain):
        """Calculate perfect machine ratios for no waste"""
        
        print("Perfect Ratio Calculation:")
        print("=" * 70)
        print("To achieve perfect balance with no surplus/deficit:")
        print("- Calculate LCM of all production rates")
        print("- Determine machine counts that match perfectly")
        print("\nExample: Iron Plate → Reinforced Iron Plate")
        print("  Iron Plate: 20/min per Constructor")
        print("  Reinforced needs 30 plates/min")
        print("  Ratio: 3 Constructors : 2 Reinforced assemblers")
        print("  (3 * 20 = 60 plates, 2 * 30 = 60 plates consumed)")

optimization = SatisfactoryOptimizationGameplay()

# Example throughput analysis
print("\n\nThroughput Optimization Example:")
print("=" * 70)

production_line = {
    'mining': 120,      # items/min
    'smelting': 100,    # items/min (bottleneck!)
    'constructing': 80,  # items/min (bottleneck!)
    'assembling': 120   # items/min
}

throughput_analysis = optimization.calculate_throughput_optimization(production_line)
print(f"Current Throughput: {throughput_analysis['current_throughput']} items/min")
print(f"Potential: {throughput_analysis['potential_throughput']} items/min")
print(f"Efficiency: {throughput_analysis['efficiency']:.1f}%")
print("\nBottlenecks Identified:")
for bottleneck in throughput_analysis['bottlenecks']:
    print(f"  {bottleneck['stage']}: {bottleneck['rate']}/min")
    print(f"    Deficit: {bottleneck['deficit']}/min")
    print(f"    Fix: {bottleneck['recommendation']}")

# Perfect ratios
optimization.calculate_perfect_ratios(None)
```

---

## Discovered Sources for Phase 4

1. **"Factorio: Production Optimization and Ratios" - Wube Software**
   - Priority: High
   - Focus: Mathematical optimization gameplay
   - Estimated: 5-6 hours

2. **"Minecraft Modded: Industrial Tech Mods" - Community**
   - Priority: High
   - Focus: Tech progression and automation
   - Estimated: 6-8 hours

3. **"Dyson Sphere Program: Planetary-Scale Logistics" - Youthcat Studio**
   - Priority: High
   - Focus: Multi-planet resource management
   - Estimated: 5-6 hours

4. **"Oxygen Not Included: Resource Management" - Klei Entertainment**
   - Priority: Medium
   - Focus: Closed-loop resource systems
   - Estimated: 4-5 hours

5. **"Rimworld: Production Chains and Workshop" - Ludeon Studios**
   - Priority: Medium
   - Focus: Crafting progression and colonist management
   - Estimated: 4-5 hours

---

## Cross-References

- **Group 41**: Economy foundations
- **Group 42**: MMO case studies
- **Group 43 Source 1**: Game balance (resource balance)
- **Group 43 Source 2**: Diablo III (material sinks)
- **Group 43 Source 3**: Elite Dangerous (spatial distribution)

---

## Recommendations for BlueMarble

### Immediate Implementation (Phase 3)

1. **Resource Node System**
   - Fixed node locations with quality tiers
   - Tool efficiency affects extraction rate
   - Node depletion over time (durability system)

2. **Production Chains**
   - 3-4 tier material transformation
   - Clear input/output ratios
   - Skill requirements for advanced recipes

3. **Building Construction Costs**
   - Significant material investment
   - Infrastructure as major material sink
   - Maintenance costs for upkeep

### High Priority (Phase 4)

4. **Automation Systems**
   - Manual → Semi-automated → Fully automated
   - Power management mechanics
   - Efficiency scaling with automation

5. **Optimization Gameplay**
   - Clear bottleneck identification
   - Ratio calculators (player tools)
   - Throughput optimization challenges

6. **Production Facilities**
   - Tiered crafting stations
   - Capacity/throughput upgrades
   - Specialization options

### Medium Priority (Phase 5+)

7. **Advanced Logistics**
   - Resource transportation systems
   - Storage management
   - Supply chain optimization

8. **Power System**
   - Energy generation/consumption
   - Power as limiting factor
   - Efficiency considerations

---

**Status:** ✅ Complete  
**Lines:** 1,100+  
**Analysis Date:** 2025-01-17  
**Next:** Batch Summary Document  
**Group:** 43 - Economy Design & Balance
