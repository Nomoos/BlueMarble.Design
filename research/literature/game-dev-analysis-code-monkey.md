---
title: Game Development Analysis - Code Monkey
date: 2025-01-17
tags: [game-development, unity, multiplayer, networking, rpg-systems, grid-systems, tutorials]
status: complete
priority: high
source: online-game-dev-resources.md
---

# Game Development Analysis: Code Monkey

## Executive Summary

**Creator:** Code Monkey  
**Platform:** YouTube (https://www.youtube.com/@CodeMonkeyUnity)  
**Primary Focus:** Unity game development, multiplayer networking, RPG systems  
**Content Type:** Practical tutorials with complete project walkthroughs  
**Experience Level:** Beginner to Advanced  
**Relevance to BlueMarble:** Critical - Covers multiplayer networking and RPG systems essential for MMORPG development

### Key Strengths

1. **Practical Implementation** - Complete, working projects from start to finish
2. **Multiplayer Focus** - Extensive coverage of networking and multiplayer systems
3. **Production Quality** - Clean code, professional practices, Unity best practices
4. **Regular Updates** - Active channel with current Unity versions and techniques
5. **Comprehensive Coverage** - From basics to advanced game systems

### Primary Topics Covered

- **Multiplayer Networking** - Netcode, Mirror, Photon, client-server architecture
- **Grid Systems** - Hex grids, square grids, pathfinding on grids
- **RPG Systems** - Inventory, dialogue, quests, combat, progression
- **Turn-Based Strategy** - Grid-based combat, AI opponents, game state management
- **UI Systems** - Health bars, inventory UI, tooltips, menus
- **Game Architecture** - Clean code patterns, state machines, event systems

### Applicability to BlueMarble MMORPG

Code Monkey's content directly addresses several critical challenges for BlueMarble:
- Multiplayer networking architecture for MMO-scale games
- Grid-based systems for tactical combat and world organization
- RPG progression and inventory systems
- Turn-based combat mechanics (alternative to real-time)
- Professional UI/UX implementation
- Scalable game architecture patterns

## Core Concepts

### 1. Multiplayer Networking

#### Unity Netcode for GameObjects

**Key Videos:**
- "Unity Multiplayer COMPLETE Course"
- "Netcode for GameObjects Tutorial"
- "Mirror Networking Tutorial Series"

**Core Concepts:**

**Client-Server Architecture:**
```csharp
using Unity.Netcode;

public class PlayerController : NetworkBehaviour {
    // NetworkVariable syncs from server to all clients
    private NetworkVariable<PlayerState> playerState = new NetworkVariable<PlayerState>();
    
    public override void OnNetworkSpawn() {
        if (IsOwner) {
            // Only the client who owns this can send commands
            SubscribeToInput();
        }
        
        if (IsServer) {
            // Server initializes state
            playerState.Value = new PlayerState {
                health = 100,
                position = transform.position
            };
        }
        
        // All clients subscribe to state changes
        playerState.OnValueChanged += OnPlayerStateChanged;
    }
    
    [ServerRpc]
    void MoveServerRpc(Vector3 direction) {
        // Client sends move command to server
        // Server validates and updates authoritative state
        if (CanMove(direction)) {
            transform.position += direction * moveSpeed * Time.deltaTime;
            
            // Update synced state
            var state = playerState.Value;
            state.position = transform.position;
            playerState.Value = state;
        }
    }
    
    [ClientRpc]
    void DamageClientRpc(int damage) {
        // Server tells all clients about damage
        PlayDamageEffect();
        UpdateHealthBar();
    }
    
    void OnPlayerStateChanged(PlayerState oldState, PlayerState newState) {
        // All clients react to state changes
        if (newState.health != oldState.health) {
            UpdateHealthDisplay(newState.health);
        }
    }
}

public struct PlayerState : INetworkSerializable {
    public int health;
    public Vector3 position;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref health);
        serializer.SerializeValue(ref position);
    }
}
```

**Network Variables vs RPCs:**
- **NetworkVariable**: Automatic state synchronization, efficient for frequently changing data
- **ServerRpc**: Client-to-server commands, for player actions
- **ClientRpc**: Server-to-client events, for game events affecting all players

**BlueMarble Application:**
- Player movement and position synchronization
- Combat damage and health updates
- Inventory changes
- Quest state updates
- World events (weather, day/night)

#### Lobby and Matchmaking

**Implementation Pattern:**
```csharp
public class LobbyManager : MonoBehaviour {
    public async void CreateLobby(string lobbyName, int maxPlayers) {
        try {
            CreateLobbyOptions options = new CreateLobbyOptions {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject> {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "Adventure") },
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, "MainWorld") }
                }
            };
            
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15f));
            
            // Start relay
            string relayJoinCode = await StartRelay(maxPlayers);
            
            // Update lobby with relay code
            await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, new UpdateLobbyOptions {
                Data = new Dictionary<string, DataObject> {
                    { "RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) }
                }
            });
            
        } catch (LobbyServiceException e) {
            Debug.LogError(e);
        }
    }
    
    public async void JoinLobby(string lobbyCode) {
        try {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions {
                Player = GetPlayer()
            };
            
            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
            
            string relayJoinCode = lobby.Data["RelayJoinCode"].Value;
            await JoinRelay(relayJoinCode);
            
        } catch (LobbyServiceException e) {
            Debug.LogError(e);
        }
    }
    
    private Player GetPlayer() {
        return new Player {
            Data = new Dictionary<string, PlayerDataObject> {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerPrefs.GetString("PlayerName")) },
                { "CharacterClass", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, selectedClass.ToString()) }
            }
        };
    }
    
    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float interval) {
        while (true) {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return new WaitForSeconds(interval);
        }
    }
}
```

**BlueMarble Application:**
- Party formation before entering dungeon instances
- PvP arena matchmaking
- Guild raid group assembly
- Server selection and world instances

### 2. Grid Systems

#### Hexagonal Grid Implementation

**Key Concepts:**

**Hex Grid Math:**
```csharp
public class HexGrid : MonoBehaviour {
    public enum HexOrientation { FlatTop, PointyTop }
    
    private HexOrientation orientation;
    private float hexSize;
    private Vector2 hexSpacing;
    
    public Vector3 HexToWorld(int q, int r) {
        if (orientation == HexOrientation.FlatTop) {
            float x = hexSize * (3f / 2f * q);
            float z = hexSize * (Mathf.Sqrt(3f) / 2f * q + Mathf.Sqrt(3f) * r);
            return new Vector3(x, 0, z);
        } else {
            float x = hexSize * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            float z = hexSize * (3f / 2f * r);
            return new Vector3(x, 0, z);
        }
    }
    
    public (int q, int r) WorldToHex(Vector3 worldPos) {
        if (orientation == HexOrientation.FlatTop) {
            float q = (2f / 3f * worldPos.x) / hexSize;
            float r = (-1f / 3f * worldPos.x + Mathf.Sqrt(3f) / 3f * worldPos.z) / hexSize;
            return HexRound(q, r);
        } else {
            float q = (Mathf.Sqrt(3f) / 3f * worldPos.x - 1f / 3f * worldPos.z) / hexSize;
            float r = (2f / 3f * worldPos.z) / hexSize;
            return HexRound(q, r);
        }
    }
    
    (int q, int r) HexRound(float q, float r) {
        float s = -q - r;
        
        int rq = Mathf.RoundToInt(q);
        int rr = Mathf.RoundToInt(r);
        int rs = Mathf.RoundToInt(s);
        
        float q_diff = Mathf.Abs(rq - q);
        float r_diff = Mathf.Abs(rr - r);
        float s_diff = Mathf.Abs(rs - s);
        
        if (q_diff > r_diff && q_diff > s_diff) {
            rq = -rr - rs;
        } else if (r_diff > s_diff) {
            rr = -rq - rs;
        }
        
        return (rq, rr);
    }
    
    public List<(int q, int r)> GetNeighbors(int q, int r) {
        List<(int, int)> directions;
        
        if (orientation == HexOrientation.FlatTop) {
            directions = new List<(int, int)> {
                (1, 0), (1, -1), (0, -1),
                (-1, 0), (-1, 1), (0, 1)
            };
        } else {
            directions = new List<(int, int)> {
                (1, 0), (0, -1), (-1, -1),
                (-1, 0), (0, 1), (1, 1)
            };
        }
        
        List<(int, int)> neighbors = new List<(int, int)>();
        foreach (var (dq, dr) in directions) {
            neighbors.Add((q + dq, r + dr));
        }
        
        return neighbors;
    }
    
    public int GetDistance(int q1, int r1, int q2, int r2) {
        return (Mathf.Abs(q1 - q2) + Mathf.Abs(q1 + r1 - q2 - r2) + Mathf.Abs(r1 - r2)) / 2;
    }
}
```

**Grid-Based Pathfinding:**
```csharp
public class GridPathfinding {
    public List<HexCell> FindPath(HexCell start, HexCell goal, HexGrid grid) {
        Dictionary<HexCell, HexCell> cameFrom = new Dictionary<HexCell, HexCell>();
        Dictionary<HexCell, int> costSoFar = new Dictionary<HexCell, int>();
        
        PriorityQueue<HexCell, int> frontier = new PriorityQueue<HexCell, int>();
        frontier.Enqueue(start, 0);
        
        cameFrom[start] = null;
        costSoFar[start] = 0;
        
        while (frontier.Count > 0) {
            HexCell current = frontier.Dequeue();
            
            if (current == goal) {
                break;
            }
            
            foreach (HexCell next in grid.GetNeighbors(current)) {
                if (!next.isWalkable) continue;
                
                int newCost = costSoFar[current] + next.movementCost;
                
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
                    costSoFar[next] = newCost;
                    int priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }
        
        return ReconstructPath(cameFrom, start, goal);
    }
    
    int Heuristic(HexCell a, HexCell b) {
        return Mathf.Abs(a.q - b.q) + Mathf.Abs(a.r - b.r);
    }
    
    List<HexCell> ReconstructPath(Dictionary<HexCell, HexCell> cameFrom, HexCell start, HexCell goal) {
        List<HexCell> path = new List<HexCell>();
        HexCell current = goal;
        
        while (current != start) {
            path.Add(current);
            current = cameFrom[current];
        }
        
        path.Reverse();
        return path;
    }
}
```

**BlueMarble Application:**
- Tactical turn-based combat zones
- City planning and building placement
- Resource node distribution visualization
- Territory control systems
- Strategic map views

#### Square Grid Systems

**Tilemap Integration:**
```csharp
public class GridBuildingSystem : MonoBehaviour {
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Grid grid;
    
    private Dictionary<Vector3Int, GridObject> gridObjects = new Dictionary<Vector3Int, GridObject>();
    
    public bool CanPlaceBuilding(Vector3 worldPosition, BuildingData building) {
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        
        // Check if all required cells are available
        for (int x = 0; x < building.width; x++) {
            for (int y = 0; y < building.height; y++) {
                Vector3Int checkPos = gridPosition + new Vector3Int(x, y, 0);
                
                if (gridObjects.ContainsKey(checkPos)) {
                    return false; // Cell occupied
                }
                
                if (!IsValidBuildLocation(checkPos)) {
                    return false; // Invalid terrain
                }
            }
        }
        
        return true;
    }
    
    public void PlaceBuilding(Vector3 worldPosition, BuildingData building) {
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        
        GridObject gridObject = new GridObject {
            position = gridPosition,
            buildingData = building,
            instanceId = System.Guid.NewGuid().ToString()
        };
        
        // Occupy all cells
        for (int x = 0; x < building.width; x++) {
            for (int y = 0; y < building.height; y++) {
                Vector3Int pos = gridPosition + new Vector3Int(x, y, 0);
                gridObjects[pos] = gridObject;
            }
        }
        
        // Instantiate visual
        GameObject buildingObj = Instantiate(building.prefab, grid.GetCellCenterWorld(gridPosition), Quaternion.identity);
        gridObject.gameObject = buildingObj;
    }
    
    public void RemoveBuilding(Vector3Int gridPosition) {
        if (!gridObjects.ContainsKey(gridPosition)) return;
        
        GridObject gridObject = gridObjects[gridPosition];
        
        // Remove from all occupied cells
        for (int x = 0; x < gridObject.buildingData.width; x++) {
            for (int y = 0; y < gridObject.buildingData.height; y++) {
                Vector3Int pos = gridPosition + new Vector3Int(x, y, 0);
                gridObjects.Remove(pos);
            }
        }
        
        // Destroy visual
        Destroy(gridObject.gameObject);
    }
}
```

**BlueMarble Application:**
- Player housing and city building
- Farm and resource production layouts
- Dungeon and instance design
- World map grid overlay
- Territory and zone boundaries

### 3. RPG Systems

#### Inventory System

**Data-Driven Architecture:**
```csharp
[CreateAssetMenu(fileName = "New Item", menuName = "RPG/Item")]
public class ItemSO : ScriptableObject {
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public int maxStackSize = 99;
    public bool isConsumable;
    public bool isEquippable;
    
    public ItemStats stats;
    public List<ItemEffect> effects;
    
    public virtual void Use(PlayerController player) {
        foreach (var effect in effects) {
            effect.Apply(player);
        }
    }
}

public class InventorySystem {
    public int inventorySize = 40;
    private List<InventorySlot> slots;
    
    public event Action<InventorySlot> OnSlotChanged;
    public event Action<ItemSO> OnItemAdded;
    public event Action<ItemSO> OnItemRemoved;
    
    public InventorySystem(int size) {
        inventorySize = size;
        slots = new List<InventorySlot>(size);
        
        for (int i = 0; i < size; i++) {
            slots.Add(new InventorySlot());
        }
    }
    
    public bool AddItem(ItemSO item, int amount = 1) {
        // Try to stack with existing item
        if (item.maxStackSize > 1) {
            foreach (var slot in slots) {
                if (slot.item == item && slot.amount < item.maxStackSize) {
                    int spaceInStack = item.maxStackSize - slot.amount;
                    int amountToAdd = Mathf.Min(spaceInStack, amount);
                    
                    slot.amount += amountToAdd;
                    amount -= amountToAdd;
                    
                    OnSlotChanged?.Invoke(slot);
                    
                    if (amount <= 0) {
                        OnItemAdded?.Invoke(item);
                        return true;
                    }
                }
            }
        }
        
        // Add to empty slot
        while (amount > 0) {
            InventorySlot emptySlot = FindEmptySlot();
            if (emptySlot == null) {
                return false; // Inventory full
            }
            
            int amountToAdd = Mathf.Min(item.maxStackSize, amount);
            emptySlot.item = item;
            emptySlot.amount = amountToAdd;
            amount -= amountToAdd;
            
            OnSlotChanged?.Invoke(emptySlot);
        }
        
        OnItemAdded?.Invoke(item);
        return true;
    }
    
    public bool RemoveItem(ItemSO item, int amount = 1) {
        int remainingToRemove = amount;
        
        for (int i = slots.Count - 1; i >= 0; i--) {
            if (slots[i].item == item) {
                if (slots[i].amount <= remainingToRemove) {
                    remainingToRemove -= slots[i].amount;
                    slots[i].Clear();
                    OnSlotChanged?.Invoke(slots[i]);
                } else {
                    slots[i].amount -= remainingToRemove;
                    remainingToRemove = 0;
                    OnSlotChanged?.Invoke(slots[i]);
                }
                
                if (remainingToRemove <= 0) {
                    OnItemRemoved?.Invoke(item);
                    return true;
                }
            }
        }
        
        return remainingToRemove <= 0;
    }
    
    public bool HasItem(ItemSO item, int amount = 1) {
        int total = 0;
        foreach (var slot in slots) {
            if (slot.item == item) {
                total += slot.amount;
            }
        }
        return total >= amount;
    }
    
    InventorySlot FindEmptySlot() {
        foreach (var slot in slots) {
            if (slot.item == null) {
                return slot;
            }
        }
        return null;
    }
}

public class InventorySlot {
    public ItemSO item;
    public int amount;
    
    public void Clear() {
        item = null;
        amount = 0;
    }
}
```

**BlueMarble Application:**
- Player inventory management
- Bank and storage systems
- Trading between players
- Loot distribution in parties
- Crafting material storage

#### Quest System

**Quest Architecture:**
```csharp
[CreateAssetMenu(fileName = "New Quest", menuName = "RPG/Quest")]
public class QuestSO : ScriptableObject {
    public string questId;
    public string questName;
    [TextArea(3, 10)]
    public string description;
    
    public List<QuestObjective> objectives;
    public List<QuestReward> rewards;
    public List<string> prerequisiteQuests;
    
    public int experienceReward;
    public int goldReward;
}

[System.Serializable]
public class QuestObjective {
    public enum ObjectiveType {
        KillEnemy,
        CollectItem,
        TalkToNPC,
        ReachLocation,
        Custom
    }
    
    public ObjectiveType type;
    public string targetId;
    public int requiredAmount;
    public int currentAmount;
    
    public bool IsComplete => currentAmount >= requiredAmount;
}

public class QuestManager : MonoBehaviour {
    private List<QuestSO> activeQuests = new List<QuestSO>();
    private List<QuestSO> completedQuests = new List<QuestSO>();
    
    public event Action<QuestSO> OnQuestAccepted;
    public event Action<QuestSO, QuestObjective> OnObjectiveUpdated;
    public event Action<QuestSO> OnQuestCompleted;
    
    public bool CanAcceptQuest(QuestSO quest) {
        // Check if already active or completed
        if (activeQuests.Contains(quest) || completedQuests.Contains(quest)) {
            return false;
        }
        
        // Check prerequisites
        foreach (string prereqId in quest.prerequisiteQuests) {
            if (!completedQuests.Exists(q => q.questId == prereqId)) {
                return false;
            }
        }
        
        return true;
    }
    
    public void AcceptQuest(QuestSO quest) {
        if (!CanAcceptQuest(quest)) return;
        
        activeQuests.Add(quest);
        OnQuestAccepted?.Invoke(quest);
        
        // Register objective trackers
        foreach (var objective in quest.objectives) {
            RegisterObjectiveTracker(quest, objective);
        }
    }
    
    public void UpdateObjective(QuestSO quest, string targetId, int amount = 1) {
        if (!activeQuests.Contains(quest)) return;
        
        foreach (var objective in quest.objectives) {
            if (objective.targetId == targetId && !objective.IsComplete) {
                objective.currentAmount = Mathf.Min(
                    objective.currentAmount + amount,
                    objective.requiredAmount
                );
                
                OnObjectiveUpdated?.Invoke(quest, objective);
                
                if (IsQuestComplete(quest)) {
                    CompleteQuest(quest);
                }
                
                break;
            }
        }
    }
    
    bool IsQuestComplete(QuestSO quest) {
        foreach (var objective in quest.objectives) {
            if (!objective.IsComplete) {
                return false;
            }
        }
        return true;
    }
    
    void CompleteQuest(QuestSO quest) {
        activeQuests.Remove(quest);
        completedQuests.Add(quest);
        
        // Give rewards
        GiveQuestRewards(quest);
        
        OnQuestCompleted?.Invoke(quest);
    }
    
    void GiveQuestRewards(QuestSO quest) {
        PlayerController player = GetComponent<PlayerController>();
        
        player.AddExperience(quest.experienceReward);
        player.AddGold(quest.goldReward);
        
        foreach (var reward in quest.rewards) {
            if (reward.item != null) {
                player.inventory.AddItem(reward.item, reward.amount);
            }
        }
    }
    
    void RegisterObjectiveTracker(QuestSO quest, QuestObjective objective) {
        switch (objective.type) {
            case QuestObjective.ObjectiveType.KillEnemy:
                GameEvents.OnEnemyKilled += (enemyId) => {
                    if (enemyId == objective.targetId) {
                        UpdateObjective(quest, enemyId, 1);
                    }
                };
                break;
                
            case QuestObjective.ObjectiveType.CollectItem:
                GameEvents.OnItemCollected += (itemId, amount) => {
                    if (itemId == objective.targetId) {
                        UpdateObjective(quest, itemId, amount);
                    }
                };
                break;
                
            // ... other objective types
        }
    }
}
```

**BlueMarble Application:**
- Main storyline quests
- Side quests and daily missions
- Guild quests and objectives
- Achievement tracking
- Tutorial progression

### 4. Turn-Based Combat System

**Combat State Machine:**
```csharp
public class TurnBasedCombat : MonoBehaviour {
    public enum CombatState {
        Start,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }
    
    private CombatState currentState;
    private List<CombatUnit> playerUnits;
    private List<CombatUnit> enemyUnits;
    private Queue<CombatUnit> turnOrder;
    
    public event Action<CombatState> OnStateChanged;
    public event Action<CombatUnit> OnTurnStart;
    public event Action<CombatUnit> OnTurnEnd;
    
    void StartCombat() {
        currentState = CombatState.Start;
        
        // Calculate turn order based on speed
        turnOrder = CalculateTurnOrder();
        
        StartNextTurn();
    }
    
    Queue<CombatUnit> CalculateTurnOrder() {
        List<CombatUnit> allUnits = new List<CombatUnit>();
        allUnits.AddRange(playerUnits);
        allUnits.AddRange(enemyUnits);
        
        // Sort by speed stat (highest first)
        allUnits.Sort((a, b) => b.stats.speed.CompareTo(a.stats.speed));
        
        return new Queue<CombatUnit>(allUnits);
    }
    
    void StartNextTurn() {
        if (CheckVictoryCondition()) {
            currentState = CombatState.Victory;
            OnStateChanged?.Invoke(currentState);
            return;
        }
        
        if (CheckDefeatCondition()) {
            currentState = CombatState.Defeat;
            OnStateChanged?.Invoke(currentState);
            return;
        }
        
        if (turnOrder.Count == 0) {
            // Round complete, recalculate turn order
            turnOrder = CalculateTurnOrder();
        }
        
        CombatUnit activeUnit = turnOrder.Dequeue();
        
        // Skip if unit is dead
        if (!activeUnit.isAlive) {
            StartNextTurn();
            return;
        }
        
        currentState = activeUnit.isPlayerControlled ? CombatState.PlayerTurn : CombatState.EnemyTurn;
        OnStateChanged?.Invoke(currentState);
        OnTurnStart?.Invoke(activeUnit);
        
        if (activeUnit.isPlayerControlled) {
            // Wait for player input
            EnablePlayerControls(activeUnit);
        } else {
            // AI takes action
            StartCoroutine(EnemyTurnCoroutine(activeUnit));
        }
    }
    
    IEnumerator EnemyTurnCoroutine(CombatUnit enemy) {
        yield return new WaitForSeconds(1f);
        
        // AI decision making
        CombatAction action = enemy.aiController.DecideAction(this);
        yield return StartCoroutine(ExecuteAction(enemy, action));
        
        EndTurn(enemy);
    }
    
    public void ExecutePlayerAction(CombatUnit actor, CombatAction action) {
        StartCoroutine(ExecuteActionCoroutine(actor, action));
    }
    
    IEnumerator ExecuteActionCoroutine(CombatUnit actor, CombatAction action) {
        yield return StartCoroutine(ExecuteAction(actor, action));
        EndTurn(actor);
    }
    
    IEnumerator ExecuteAction(CombatUnit actor, CombatAction action) {
        switch (action.type) {
            case ActionType.Attack:
                yield return StartCoroutine(PerformAttack(actor, action.target));
                break;
                
            case ActionType.Skill:
                yield return StartCoroutine(UseSkill(actor, action.skill, action.targets));
                break;
                
            case ActionType.Item:
                yield return StartCoroutine(UseItem(actor, action.item, action.target));
                break;
                
            case ActionType.Defend:
                actor.isDefending = true;
                break;
        }
    }
    
    IEnumerator PerformAttack(CombatUnit attacker, CombatUnit target) {
        int damage = CalculateDamage(attacker, target);
        target.TakeDamage(damage);
        
        // Play attack animation
        yield return StartCoroutine(attacker.PlayAttackAnimation());
        
        // Play hit effect
        yield return StartCoroutine(target.PlayHitEffect());
    }
    
    int CalculateDamage(CombatUnit attacker, CombatUnit target) {
        int baseDamage = attacker.stats.attack;
        int defense = target.isDefending ? target.stats.defense * 2 : target.stats.defense;
        
        int damage = Mathf.Max(1, baseDamage - defense);
        
        // Critical hit chance
        if (Random.Range(0f, 1f) < attacker.stats.critChance) {
            damage = (int)(damage * attacker.stats.critMultiplier);
        }
        
        return damage;
    }
    
    void EndTurn(CombatUnit unit) {
        unit.isDefending = false;
        OnTurnEnd?.Invoke(unit);
        
        StartNextTurn();
    }
    
    bool CheckVictoryCondition() {
        foreach (var enemy in enemyUnits) {
            if (enemy.isAlive) {
                return false;
            }
        }
        return true;
    }
    
    bool CheckDefeatCondition() {
        foreach (var player in playerUnits) {
            if (player.isAlive) {
                return false;
            }
        }
        return true;
    }
}

public class CombatUnit : MonoBehaviour {
    public UnitStats stats;
    public bool isPlayerControlled;
    public bool isAlive = true;
    public bool isDefending = false;
    
    public void TakeDamage(int damage) {
        stats.currentHealth -= damage;
        
        if (stats.currentHealth <= 0) {
            stats.currentHealth = 0;
            isAlive = false;
            Die();
        }
    }
    
    void Die() {
        // Play death animation
        // Drop loot if enemy
        // Remove from combat
    }
}
```

**BlueMarble Application:**
- Tactical dungeon encounters
- PvP arena battles
- Boss fights with phases
- Strategic guild battles
- Tutorial combat sequences

### 5. UI Systems

#### Damage Numbers and Floating Text

**Dynamic UI Elements:**
```csharp
public class DamageNumber : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private AnimationCurve alphaCurve;
    
    private float timer;
    private Vector3 initialPosition;
    
    public void Setup(int damage, Color color, bool isCritical) {
        text.text = damage.ToString();
        text.color = color;
        
        if (isCritical) {
            text.fontSize *= 1.5f;
            text.fontStyle = FontStyles.Bold;
        }
        
        initialPosition = transform.position;
        timer = 0f;
    }
    
    void Update() {
        timer += Time.deltaTime;
        float progress = timer / lifetime;
        
        // Move upward
        transform.position = initialPosition + Vector3.up * (moveSpeed * timer);
        
        // Animate scale
        float scale = scaleCurve.Evaluate(progress);
        transform.localScale = Vector3.one * scale;
        
        // Fade out
        float alpha = alphaCurve.Evaluate(progress);
        Color color = text.color;
        color.a = alpha;
        text.color = color;
        
        if (timer >= lifetime) {
            Destroy(gameObject);
        }
    }
}

public class DamageNumberSpawner : MonoBehaviour {
    [SerializeField] private DamageNumber damageNumberPrefab;
    [SerializeField] private Color normalDamageColor = Color.white;
    [SerializeField] private Color criticalDamageColor = Color.yellow;
    [SerializeField] private Color healColor = Color.green;
    
    public void SpawnDamageNumber(Vector3 worldPosition, int damage, bool isCritical) {
        DamageNumber damageNumber = Instantiate(damageNumberPrefab, worldPosition, Quaternion.identity);
        Color color = isCritical ? criticalDamageColor : normalDamageColor;
        damageNumber.Setup(damage, color, isCritical);
    }
    
    public void SpawnHealNumber(Vector3 worldPosition, int amount) {
        DamageNumber healNumber = Instantiate(damageNumberPrefab, worldPosition, Quaternion.identity);
        healNumber.Setup(amount, healColor, false);
    }
}
```

**BlueMarble Application:**
- Combat feedback (damage, healing, buffs)
- Quest notifications
- Loot pickup indicators
- Achievement popups
- Status effect notifications

### 6. Game Architecture Patterns

#### Event System

**Decoupled Communication:**
```csharp
public static class GameEvents {
    // Player events
    public static event Action<int> OnPlayerLevelUp;
    public static event Action<int> OnPlayerHealthChanged;
    public static event Action<ItemSO, int> OnItemCollected;
    
    // Combat events
    public static event Action<string> OnEnemyKilled;
    public static event Action<int, bool> OnDamageDealt;
    
    // Quest events
    public static event Action<QuestSO> OnQuestStarted;
    public static event Action<QuestSO> OnQuestCompleted;
    
    // World events
    public static event Action<WeatherType> OnWeatherChanged;
    public static event Action<float> OnTimeChanged;
    
    // Trigger events
    public static void TriggerPlayerLevelUp(int newLevel) {
        OnPlayerLevelUp?.Invoke(newLevel);
    }
    
    public static void TriggerEnemyKilled(string enemyId) {
        OnEnemyKilled?.Invoke(enemyId);
    }
    
    public static void TriggerItemCollected(ItemSO item, int amount) {
        OnItemCollected?.Invoke(item, amount);
    }
}

// Usage in different systems
public class QuestObjectiveTracker : MonoBehaviour {
    void OnEnable() {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnItemCollected += HandleItemCollected;
    }
    
    void OnDisable() {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnItemCollected -= HandleItemCollected;
    }
    
    void HandleEnemyKilled(string enemyId) {
        // Update quest objectives
    }
    
    void HandleItemCollected(ItemSO item, int amount) {
        // Update collection quests
    }
}
```

**BlueMarble Application:**
- Decouple game systems
- Achievement tracking
- Analytics and telemetry
- UI updates
- Audio triggers

## BlueMarble Application

### Critical Systems from Code Monkey

#### 1. Multiplayer Infrastructure

**Priority:** Critical  
**Estimated Effort:** 10-12 weeks

**Implementation Phases:**

**Phase 1: Core Networking (3-4 weeks)**
- Set up Unity Netcode for GameObjects
- Implement basic client-server architecture
- Create player connection/disconnection handling
- Build lobby system for party formation
- Test with 4-8 players

**Phase 2: State Synchronization (3-4 weeks)**
- Implement NetworkVariables for player state
- Create server-authoritative movement
- Build combat synchronization
- Add inventory state replication
- Optimize network traffic

**Phase 3: Matchmaking & Instances (2-3 weeks)**
- Implement dungeon instancing
- Create PvP arena matchmaking
- Build guild raid coordination
- Add server selection
- Test with 20+ concurrent players

**Phase 4: Optimization & Scaling (2 weeks)**
- Profile network performance
- Implement interest management
- Add client-side prediction
- Optimize bandwidth usage
- Load testing and stress testing

#### 2. Grid-Based Systems

**Priority:** High  
**Estimated Effort:** 6-8 weeks

**Implementation:**

**Hex Grid for Tactical Combat:**
- Implement hex grid math
- Create grid-based pathfinding
- Build turn-based combat on grid
- Add area of effect abilities
- Visual grid indicators

**Square Grid for Building:**
- Integrate with Unity Tilemap
- Create building placement system
- Implement collision detection
- Add structure templates
- Multi-tile building support

#### 3. RPG Core Systems

**Priority:** Critical  
**Estimated Effort:** 8-10 weeks

**Inventory System (2-3 weeks):**
- Item database with ScriptableObjects
- Grid-based inventory UI
- Stacking and splitting items
- Equipment slots
- Trading between players

**Quest System (3-4 weeks):**
- Quest database
- Objective tracking
- Multi-step quests
- Quest chains and prerequisites
- Reward distribution

**Combat System (3 weeks):**
- Turn-based combat framework
- Skill and ability system
- Status effects and buffs
- AI for enemy behavior
- Combat UI and feedback

## Implementation Recommendations

### Technology Stack

**For Multiplayer:**
- Unity Netcode for GameObjects (first-party, well-supported)
- Unity Gaming Services (Lobby, Relay for small groups)
- Custom dedicated servers for MMO-scale (beyond small groups)

**For UI:**
- Unity UI Toolkit (modern, performant)
- TextMeshPro for all text
- DOTween for animations

**For Data:**
- ScriptableObjects for game data
- JSON for save files
- SQL database for server-side persistence

### Learning Path

**Week 1-2: Multiplayer Fundamentals**
1. "Unity Multiplayer COMPLETE Course" (Code Monkey)
2. Build simple multiplayer test project
3. Implement basic client-server communication

**Week 3-4: Grid Systems**
1. "Hex Grid Tutorial Series"
2. "Grid Building System"
3. Implement pathfinding on grid

**Week 5-6: RPG Systems**
1. "Complete RPG Inventory System"
2. "Quest System Tutorial"
3. Build item and quest databases

**Week 7-8: Combat & Polish**
1. "Turn-Based Combat System"
2. "Damage Numbers & UI Feedback"
3. Integration and testing

### Code Quality Patterns

**Use Events for Decoupling:**
```csharp
// Bad - tight coupling
public class Enemy : MonoBehaviour {
    void Die() {
        FindObjectOfType<QuestManager>().UpdateKillObjective(enemyId);
        FindObjectOfType<PlayerController>().AddExperience(expReward);
        FindObjectOfType<UIManager>().ShowKillNotification(enemyName);
    }
}

// Good - decoupled via events
public class Enemy : MonoBehaviour {
    void Die() {
        GameEvents.TriggerEnemyKilled(enemyId, expReward, enemyName);
    }
}
```

**ScriptableObjects for Data:**
```csharp
// All game data as ScriptableObjects
// - Easy to create and edit in editor
// - No code changes for new content
// - Shareable between scenes
// - Version control friendly
```

**State Machines for Complex Behavior:**
```csharp
// Clear state transitions
// Easy to debug and visualize
// Prevents invalid states
// Supports hierarchical states
```

## References

### Primary Source

**YouTube Channel:** https://www.youtube.com/@CodeMonkeyUnity  
**Subscriber Count:** 700K+ (as of 2025)  
**Content Style:** Practical tutorials, complete projects  
**Update Frequency:** Weekly

### Key Playlists and Videos

**Multiplayer Networking:**
1. "Unity Multiplayer COMPLETE Course" (6+ hours)
2. "Netcode for GameObjects Tutorial Series"
3. "Mirror Networking Complete Guide"
4. "Photon PUN 2 Tutorial"
5. "Unity Relay & Lobby Services"

**Grid Systems:**
1. "Hex Grid Complete Tutorial"
2. "Grid Building System (Cities Skylines Style)"
3. "Tilemap Tutorial Series"
4. "Pathfinding on Grid"

**RPG Systems:**
1. "Complete RPG Inventory System"
2. "Quest System Tutorial"
3. "Dialogue System"
4. "Equipment & Stats System"
5. "Save & Load System"

**Turn-Based Combat:**
1. "Turn-Based Combat System (XCOM Style)"
2. "Grid-Based Tactics Game"
3. "Card Battle System"

**UI & Polish:**
1. "Damage Numbers Tutorial"
2. "Health Bar System"
3. "Tooltip System"
4. "Menu & UI Best Practices"

### Code Resources

**GitHub Repositories:**
- Many tutorials include downloadable projects
- Code samples available in video descriptions
- Active community discussions in comments

**Unity Asset Store:**
- Some tutorials based on free/paid assets
- Recommendations for production tools

### Complementary Resources

**Official Unity Documentation:**
- Netcode for GameObjects: https://docs-multiplayer.unity3d.com/
- Unity Gaming Services: https://unity.com/solutions/gaming-services

**Related Channels:**
- Brackeys (Unity basics)
- Sebastian Lague (advanced algorithms)
- GameDev.tv (complete courses)

## Related Research

### Within BlueMarble Repository

- [game-dev-analysis-sebastian-lague.md](./game-dev-analysis-sebastian-lague.md) - Procedural generation and algorithms
- [algorithm-analysis-marching-cubes.md](./algorithm-analysis-marching-cubes.md) - Terrain modification
- [code-analysis-sebastian-lague-github.md](./code-analysis-sebastian-lague-github.md) - Code implementations
- [online-game-dev-resources.md](./online-game-dev-resources.md) - Full resource catalog

### Cross-References

**Complementary Topics:**
- Assignment Group 2: "Game Engine Architecture" - System design patterns
- Assignment Group 5: "Multiplayer Game Programming" - Advanced networking
- Assignment Group 9: "Massively Multiplayer Game Development" - MMO-specific architecture

### Discovered Sources

**From Code Monkey Analysis:**
1. **Unity Netcode Documentation** - Official multiplayer framework docs
2. **Unity Gaming Services** - Lobby, Relay, Matchmaking services
3. **Photon PUN 2** - Alternative multiplayer solution
4. **Mirror Networking** - Open-source networking framework

### Future Research Directions

**Advanced Topics:**
1. **MMO Scaling** - Transition from small multiplayer to MMO scale
2. **Server Architecture** - Custom dedicated servers for large player counts
3. **Database Design** - Persistent player data and world state
4. **Anti-Cheat** - Server authority and validation
5. **Load Balancing** - Distributing players across servers

**Open Questions:**
- How to scale from 8-player instances to 100+ player zones?
- Database architecture for persistent MMO world?
- Client-side prediction vs server authority trade-offs?
- Bandwidth optimization for large player counts?

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Research Hours:** 4-6 hours  
**Lines:** 876  
**Next Steps:** Implement multiplayer prototype using Code Monkey's Netcode tutorials
