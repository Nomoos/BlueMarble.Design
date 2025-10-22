---
title: Wildlife AI and Behavior Simulation Systems
date: 2025-01-17
tags: [research, survival, wildlife, ai, behavior-simulation]
status: complete
priority: Medium
phase: 2
group: 05
batch: 2
source_type: analysis
category: survival + gamedev-tech
estimated_effort: 5-7h
---

# Wildlife AI and Behavior Simulation Systems

**Document Type:** Research Analysis  
**Research Phase:** Phase 2, Group 05, Batch 2  
**Priority:** Medium  
**Category:** Survival + GameDev-Tech  
**Estimated Effort:** 5-7 hours

---

## Executive Summary

Realistic wildlife behavior is crucial for creating an immersive survival experience in BlueMarble. This research examines AI systems for simulating animal behavior, from basic needs (hunger, thirst, reproduction) to complex interactions (predator-prey dynamics, herd behavior, territoriality). The analysis focuses on creating believable, performance-efficient wildlife simulation at planet scale.

Key findings show that effective wildlife AI balances **behavioral realism** (natural-looking actions), **gameplay integration** (meaningful player interactions), and **performance** (handling thousands of animals). The recommended approach uses behavior trees for individual decision-making, flocking algorithms for group behavior, and simplified needs-based systems that create emergent complexity without excessive computation.

---

## Core Concepts and Analysis

### 1. Behavior Tree Architecture

```csharp
public abstract class BehaviorNode
{
    public enum NodeStatus
    {
        Success,
        Failure,
        Running
    }
    
    public abstract NodeStatus Tick(AnimalAI animal, float deltaTime);
}

public class SelectorNode : BehaviorNode
{
    private List<BehaviorNode> children;
    
    public override NodeStatus Tick(AnimalAI animal, float deltaTime)
    {
        foreach (var child in children)
        {
            var status = child.Tick(animal, deltaTime);
            if (status != NodeStatus.Failure)
                return status;
        }
        return NodeStatus.Failure;
    }
}

public class SequenceNode : BehaviorNode
{
    private List<BehaviorNode> children;
    
    public override NodeStatus Tick(AnimalAI animal, float deltaTime)
    {
        foreach (var child in children)
        {
            var status = child.Tick(animal, deltaTime);
            if (status != NodeStatus.Success)
                return status;
        }
        return NodeStatus.Success;
    }
}

// Example behavior: Deer
public class DeerBehaviorTree
{
    public static BehaviorNode CreateTree()
    {
        return new SelectorNode(new List<BehaviorNode>
        {
            // Priority 1: Flee from danger
            new SequenceNode(new List<BehaviorNode>
            {
                new DetectPredatorNode(),
                new FleeNode()
            }),
            
            // Priority 2: Satisfy hunger
            new SequenceNode(new List<BehaviorNode>
            {
                new IsHungryNode(),
                new FindFoodNode(),
                new MoveToFoodNode(),
                new EatNode()
            }),
            
            // Priority 3: Satisfy thirst
            new SequenceNode(new List<BehaviorNode>
            {
                new IsThirstyNode(),
                new FindWaterNode(),
                new MoveToWaterNode(),
                new DrinkNode()
            }),
            
            // Priority 4: Social behavior
            new SequenceNode(new List<BehaviorNode>
            {
                new FindHerdNode(),
                new MoveToHerdNode()
            }),
            
            // Default: Wander
            new WanderNode()
        });
    }
}
```

### 2. Needs-Based System

```csharp
public class AnimalNeeds
{
    public float Hunger { get; set; } = 0f;      // 0-100
    public float Thirst { get; set; } = 0f;      // 0-100
    public float Energy { get; set; } = 100f;    // 0-100
    public float Fear { get; set; } = 0f;        // 0-100
    public float Reproduction { get; set; } = 0f; // 0-100
    
    public void Update(float deltaTime)
    {
        // Needs increase over time
        Hunger += 1f * deltaTime;
        Thirst += 1.5f * deltaTime;  // Thirst increases faster
        Energy -= 0.5f * deltaTime;  // Energy depletes slowly
        
        // Fear decreases over time
        Fear = Math.Max(0, Fear - 2f * deltaTime);
        
        // Reproduction drive increases slowly
        Reproduction += 0.1f * deltaTime;
        
        // Clamp values
        Hunger = Math.Min(100, Hunger);
        Thirst = Math.Min(100, Thirst);
        Energy = Math.Max(0, Energy);
        Reproduction = Math.Min(100, Reproduction);
    }
    
    public float GetUrgency(NeedType need)
    {
        return need switch
        {
            NeedType.Hunger => Hunger / 100f,
            NeedType.Thirst => Thirst / 100f,
            NeedType.Rest => (100 - Energy) / 100f,
            NeedType.Safety => Fear / 100f,
            NeedType.Reproduction => Reproduction / 100f,
            _ => 0f
        };
    }
    
    public NeedType GetMostUrgentNeed()
    {
        var needs = new[]
        {
            (NeedType.Safety, GetUrgency(NeedType.Safety)),
            (NeedType.Thirst, GetUrgency(NeedType.Thirst)),
            (NeedType.Hunger, GetUrgency(NeedType.Hunger)),
            (NeedType.Rest, GetUrgency(NeedType.Rest)),
            (NeedType.Reproduction, GetUrgency(NeedType.Reproduction))
        };
        
        return needs.OrderByDescending(n => n.Item2).First().Item1;
    }
}
```

### 3. Predator-Prey Dynamics

```csharp
public class PredatorPreySystem
{
    public struct HuntingAttempt
    {
        public AnimalAI Predator;
        public AnimalAI Prey;
        public float SuccessChance;
        public bool Success;
    }
    
    public HuntingAttempt AttemptHunt(AnimalAI predator, AnimalAI prey)
    {
        float baseSuccess = 0.3f;
        
        // Factors affecting success
        float distanceFactor = CalculateDistanceFactor(predator, prey);
        float healthFactor = predator.Health / 100f;
        float preyAwarenessFactor = prey.IsAwareOfThreat ? 0.5f : 1.0f;
        float terrainFactor = GetTerrainAdvantage(predator, prey);
        
        float successChance = baseSuccess * distanceFactor * healthFactor * 
                            preyAwarenessFactor * terrainFactor;
        
        bool success = Random.value < successChance;
        
        if (success)
        {
            prey.Health = 0;  // Kill prey
            predator.Needs.Hunger = 0;  // Satisfy hunger
        }
        
        return new HuntingAttempt
        {
            Predator = predator,
            Prey = prey,
            SuccessChance = successChance,
            Success = success
        };
    }
    
    private float CalculateDistanceFactor(AnimalAI predator, AnimalAI prey)
    {
        float distance = Vector3.Distance(predator.Position, prey.Position);
        float attackRange = predator.Species.AttackRange;
        
        if (distance > attackRange)
            return 0f;
        
        return 1.0f - (distance / attackRange);
    }
}
```

### 4. Herd/Flock Behavior

```csharp
public class FlockingSystem
{
    public struct FlockingForce
    {
        public Vector3 Separation;   // Avoid crowding
        public Vector3 Alignment;    // Match velocity
        public Vector3 Cohesion;     // Move toward center
    }
    
    public FlockingForce CalculateFlocking(
        AnimalAI animal,
        List<AnimalAI> nearbyAnimals,
        float perceptionRadius)
    {
        var force = new FlockingForce();
        int count = 0;
        
        Vector3 avgPosition = Vector3.zero;
        Vector3 avgVelocity = Vector3.zero;
        Vector3 separationForce = Vector3.zero;
        
        foreach (var other in nearbyAnimals)
        {
            if (other == animal || other.Species != animal.Species)
                continue;
            
            float distance = Vector3.Distance(animal.Position, other.Position);
            
            if (distance < perceptionRadius)
            {
                // Cohesion: average position
                avgPosition += other.Position;
                
                // Alignment: average velocity
                avgVelocity += other.Velocity;
                
                // Separation: avoid close neighbors
                if (distance < perceptionRadius * 0.3f)
                {
                    Vector3 diff = animal.Position - other.Position;
                    separationForce += diff / (distance * distance);
                }
                
                count++;
            }
        }
        
        if (count > 0)
        {
            // Cohesion: steer toward average position
            avgPosition /= count;
            force.Cohesion = (avgPosition - animal.Position).normalized;
            
            // Alignment: match average velocity
            avgVelocity /= count;
            force.Alignment = avgVelocity.normalized;
            
            // Separation: already calculated
            force.Separation = separationForce.normalized;
        }
        
        return force;
    }
    
    public Vector3 ApplyFlocking(
        AnimalAI animal,
        FlockingForce force,
        float separationWeight = 1.5f,
        float alignmentWeight = 1.0f,
        float cohesionWeight = 1.0f)
    {
        Vector3 steering = force.Separation * separationWeight +
                          force.Alignment * alignmentWeight +
                          force.Cohesion * cohesionWeight;
        
        return steering.normalized * animal.Species.MaxSpeed;
    }
}
```

### 5. Territory and Home Range

```csharp
public class TerritorialBehavior
{
    public struct Territory
    {
        public Vector3 Center;
        public float Radius;
        public AnimalAI Owner;
        public List<Vector3> PatrolPoints;
        public float DefenseAggression;  // 0-1
    }
    
    public bool IsInTerritory(Territory territory, Vector3 position)
    {
        return Vector3.Distance(territory.Center, position) <= territory.Radius;
    }
    
    public float GetTerritorialResponse(
        Territory territory,
        AnimalAI intruder)
    {
        if (!IsInTerritory(territory, intruder.Position))
            return 0f;
        
        // Distance from center affects response
        float distanceFromCenter = Vector3.Distance(
            territory.Center, 
            intruder.Position);
        float normalizedDist = distanceFromCenter / territory.Radius;
        
        // Stronger response near center
        float intensity = (1.0f - normalizedDist) * territory.DefenseAggression;
        
        // Species affects response
        if (intruder.Species == territory.Owner.Species)
            intensity *= 0.5f;  // Less aggressive to same species
        else if (intruder.IsPredator)
            intensity *= 1.5f;  // More aggressive to predators
        
        return intensity;
    }
    
    public Vector3 GetTerritorialAction(
        Territory territory,
        AnimalAI intruder)
    {
        float response = GetTerritorialResponse(territory, intruder);
        
        if (response > 0.7f)
        {
            // Attack intruder
            return (intruder.Position - territory.Owner.Position).normalized;
        }
        else if (response > 0.3f)
        {
            // Display aggression (move toward intruder)
            Vector3 toIntruder = intruder.Position - territory.Owner.Position;
            return toIntruder.normalized * 0.5f;
        }
        
        // Ignore
        return Vector3.zero;
    }
}
```

### 6. Migration Patterns

```csharp
public class MigrationSystem
{
    public struct MigrationRoute
    {
        public List<Vector3> Waypoints;
        public Season StartSeason;
        public Season EndSeason;
        public float TriggerTemperature;
    }
    
    private Dictionary<SpeciesType, List<MigrationRoute>> migrationRoutes;
    
    public bool ShouldMigrate(
        AnimalAI animal,
        Season currentSeason,
        float currentTemperature)
    {
        if (!animal.Species.IsMigratory)
            return false;
        
        var routes = GetMigrationRoutes(animal.Species.Type);
        
        foreach (var route in routes)
        {
            if (currentSeason == route.StartSeason)
                return true;
            
            if (currentTemperature < route.TriggerTemperature)
                return true;
        }
        
        return false;
    }
    
    public Vector3 GetMigrationDirection(
        AnimalAI animal,
        Season currentSeason)
    {
        var routes = GetMigrationRoutes(animal.Species.Type);
        
        foreach (var route in routes)
        {
            if (currentSeason == route.StartSeason)
            {
                // Find nearest waypoint
                Vector3 nearestWaypoint = route.Waypoints[0];
                float minDist = float.MaxValue;
                
                foreach (var waypoint in route.Waypoints)
                {
                    float dist = Vector3.Distance(animal.Position, waypoint);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearestWaypoint = waypoint;
                    }
                }
                
                return (nearestWaypoint - animal.Position).normalized;
            }
        }
        
        return Vector3.zero;
    }
}
```

---

## BlueMarble-Specific Recommendations

### 1. Performance Optimization

```csharp
public class WildlifePerformanceManager
{
    public enum UpdateFrequency
    {
        EveryFrame,    // Near player
        Every5Frames,  // Medium distance
        Every30Frames, // Far distance
        Every300Frames // Very far (statistical only)
    }
    
    public UpdateFrequency GetUpdateFrequency(AnimalAI animal, Vector3 playerPos)
    {
        float distance = Vector3.Distance(animal.Position, playerPos);
        
        if (distance < 100f)
            return UpdateFrequency.EveryFrame;
        else if (distance < 500f)
            return UpdateFrequency.Every5Frames;
        else if (distance < 2000f)
            return UpdateFrequency.Every30Frames;
        else
            return UpdateFrequency.Every300Frames;
    }
    
    private int frameCounter = 0;
    
    public void Update(List<AnimalAI> animals, Vector3 playerPos)
    {
        frameCounter++;
        
        foreach (var animal in animals)
        {
            var frequency = GetUpdateFrequency(animal, playerPos);
            
            bool shouldUpdate = frequency switch
            {
                UpdateFrequency.EveryFrame => true,
                UpdateFrequency.Every5Frames => frameCounter % 5 == 0,
                UpdateFrequency.Every30Frames => frameCounter % 30 == 0,
                UpdateFrequency.Every300Frames => frameCounter % 300 == 0,
                _ => false
            };
            
            if (shouldUpdate)
            {
                animal.UpdateBehavior(Time.deltaTime);
            }
        }
    }
}
```

### 2. Spawning System

```csharp
public class WildlifeSpawningSystem
{
    public struct SpawnRule
    {
        public SpeciesType Species;
        public BiomeType Biome;
        public float MinPopulation;  // Per km²
        public float MaxPopulation;
        public float SpawnProbability;
    }
    
    public void SpawnWildlife(
        Vector3 region Center,
        float regionRadius,
        BiomeType biome)
    {
        var rules = GetSpawnRules(biome);
        
        foreach (var rule in rules)
        {
            float currentPopulation = CountAnimals(
                regionCenter, 
                regionRadius, 
                rule.Species);
            
            float desiredPopulation = Random.Range(
                rule.MinPopulation, 
                rule.MaxPopulation);
            
            int toSpawn = (int)(desiredPopulation - currentPopulation);
            
            for (int i = 0; i < toSpawn; i++)
            {
                if (Random.value < rule.SpawnProbability)
                {
                    Vector3 spawnPos = GetRandomPositionInRegion(
                        regionCenter, 
                        regionRadius);
                    SpawnAnimal(rule.Species, spawnPos);
                }
            }
        }
    }
    
    private Vector3 GetRandomPositionInRegion(Vector3 center, float radius)
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return center + new Vector3(randomCircle.x, 0, randomCircle.y);
    }
}
```

### 3. Player Interaction

```csharp
public class WildlifePlayerInteraction
{
    public enum InteractionType
    {
        Neutral,
        Curious,
        Fearful,
        Aggressive,
        Defensive
    }
    
    public InteractionType DetermineInteraction(
        AnimalAI animal,
        Player player)
    {
        float distance = Vector3.Distance(animal.Position, player.Position);
        
        // Predators may be aggressive
        if (animal.IsPredator && animal.Needs.Hunger > 70f)
        {
            if (distance < animal.Species.AggroRange)
                return InteractionType.Aggressive;
        }
        
        // Most animals are fearful of humans
        if (animal.IsHerbivore)
        {
            if (distance < animal.Species.FleeDistance)
                return InteractionType.Fearful;
            else if (distance < animal.Species.AlertDistance)
                return InteractionType.Curious;
        }
        
        // Animals with young are defensive
        if (animal.HasOffspring && distance < 20f)
        {
            return InteractionType.Defensive;
        }
        
        return InteractionType.Neutral;
    }
    
    public void ReactToPlayer(AnimalAI animal, Player player)
    {
        var interaction = DetermineInteraction(animal, player);
        
        switch (interaction)
        {
            case InteractionType.Fearful:
                Vector3 fleeDirection = (animal.Position - player.Position).normalized;
                animal.SetBehavior(new FleeBehavior(fleeDirection));
                break;
                
            case InteractionType.Aggressive:
                animal.SetTarget(player);
                animal.SetBehavior(new AttackBehavior());
                break;
                
            case InteractionType.Defensive:
                if (Vector3.Distance(animal.Position, player.Position) < 10f)
                {
                    animal.SetBehavior(new ChargeWarningBehavior());
                }
                break;
                
            case InteractionType.Curious:
                animal.SetBehavior(new ObserveBehavior(player.Position));
                break;
        }
    }
}
```

---

## Implementation Roadmap

### Phase 1: Core AI (Week 1-2)
1. Behavior tree framework
2. Basic needs system
3. Simple movement/pathfinding
4. Spawning system

### Phase 2: Species Behaviors (Week 3)
1. Herbivore behaviors
2. Predator behaviors
3. Predator-prey interactions
4. Basic animations

### Phase 3: Social Behaviors (Week 4)
1. Flocking/herding
2. Pack hunting
3. Territorial behavior
4. Mating/reproduction

### Phase 4: Optimization (Week 5)
1. LOD system
2. Update frequency scaling
3. Spatial partitioning
4. Performance profiling

---

## References and Cross-Links

### Related Research
- `survival-analysis-biome-generation-ecosystems.md` - Animal distribution
- `survival-analysis-day-night-cycle-implementation.md` - Diurnal behaviors
- `game-dev-analysis-ai-for-games-3rd-edition.md` - AI techniques

### External Resources
- Behavior tree implementations
- Flocking algorithms (Boids)
- Game AI Pro series
- Real-world animal behavior studies

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Next Steps:** Implement behavior tree and needs system  
**Related Issues:** Phase 2 Group 05 research assignment
