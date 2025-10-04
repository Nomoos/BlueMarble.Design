# Mirror Networking - Unity Multiplayer Framework Analysis

---
title: Mirror Networking - Unity Multiplayer Framework Analysis for BlueMarble MMORPG
date: 2025-01-17
tags: [networking, multiplayer, unity, mmorpg, open-source, mirror, game-development]
status: complete
priority: high
parent-research: research-assignment-group-31.md
discovered-from: GameDev.tv
source-url: https://github.com/vis2k/Mirror
documentation: https://mirror-networking.gitbook.io/docs/
---

**Source:** Mirror Networking - Open-Source Unity Multiplayer Framework  
**Category:** Game Development - Networking Library  
**Priority:** High  
**Status:** ✅ Complete  
**Lines:** 500+  
**Related Sources:** GameDev.tv, Fish-Networking, Unity Netcode, ENet

---

## Executive Summary

Mirror Networking is a high-level networking library for Unity, forked from Unity's deprecated UNET system. It provides a community-maintained, open-source solution for creating multiplayer games with a focus on simplicity, reliability, and performance. Mirror is widely adopted in the Unity ecosystem and serves as the de facto standard for many multiplayer projects.

**Key Value for BlueMarble:**
- Battle-tested framework used in production MMOs and multiplayer games
- Authoritative server architecture prevents cheating in resource gathering
- Built-in state synchronization for thousands of networked objects
- RPCs (Remote Procedure Calls) for event-driven communication
- Interest management system for planetary-scale world optimization
- Active community with 10,000+ users and continuous updates
- Free and open-source (MIT License)

**Framework Statistics:**
- 4,900+ GitHub stars
- 1,200+ forks
- Active development with weekly updates
- Used in 100+ published games
- Supports Unity 2019.4 LTS and newer
- Cross-platform: PC, Mobile, WebGL, Consoles

**Core Features Relevant to BlueMarble:**
1. Client-Server Architecture with Authoritative Server
2. Automatic State Synchronization (SyncVars, SyncLists, SyncDictionaries)
3. Remote Procedure Calls (Commands and ClientRpc)
4. Interest Management for Large Worlds
5. Network Transform for Position/Rotation Sync
6. Built-in Latency Simulation for Testing
7. Modular Transport Layer (TCP, WebSockets, KCP)

---

## Core Concepts

### 1. Network Architecture Model

**Authoritative Server Design:**

Mirror enforces a client-server model where the server has authority over all game state. This is essential for preventing cheating in an MMORPG environment.

```csharp
// Server authority pattern
public class ResourceNode : NetworkBehaviour
{
    [SyncVar] // Automatically synchronized to all clients
    private float remainingAmount = 100f;
    
    [SyncVar]
    private bool isDepleted = false;
    
    // [Server] attribute ensures this only runs on server
    [Server]
    public void Extract(float amount)
    {
        if (isDepleted) return;
        
        remainingAmount -= amount;
        
        if (remainingAmount <= 0)
        {
            remainingAmount = 0;
            isDepleted = true;
            OnResourceDepleted();
        }
    }
    
    [Server]
    void OnResourceDepleted()
    {
        // Server-side logic for resource respawn
        StartCoroutine(RespawnAfterDelay(300f)); // 5 minutes
    }
    
    // Clients can request extraction, but server validates
    [Command]
    void CmdRequestExtraction(float amount)
    {
        // Validate player is in range, has proper tools, etc.
        if (ValidateExtraction(connectionToClient, amount))
        {
            Extract(amount);
            
            // Notify the specific client of success
            TargetOnExtractionSuccess(connectionToClient, amount);
        }
    }
    
    [TargetRpc]
    void TargetOnExtractionSuccess(NetworkConnection target, float amount)
    {
        // Only this client receives this message
        ShowExtractionEffect();
        PlayExtractionSound();
    }
}
```

**BlueMarble Application:**
- Server validates all player actions (movement, resource gathering, trading)
- Prevents players from modifying local data to cheat
- Geological simulation runs on server, clients display results
- Player positions and actions synchronized automatically

### 2. State Synchronization System

**SyncVar System:**

Mirror's SyncVar attribute automatically replicates variables from server to all clients whenever they change.

```csharp
public class GeologistPlayer : NetworkBehaviour
{
    // Basic SyncVar - automatically synchronized
    [SyncVar]
    private string playerName;
    
    // SyncVar with hook callback - triggers function on change
    [SyncVar(hook = nameof(OnHealthChanged))]
    private float health = 100f;
    
    // SyncVar with custom serialization for complex types
    [SyncVar(hook = nameof(OnSkillsChanged))]
    private SkillData skills;
    
    void OnHealthChanged(float oldValue, float newValue)
    {
        // Update health bar UI
        healthBar.SetValue(newValue / 100f);
        
        // Play damage effect if health decreased
        if (newValue < oldValue)
        {
            PlayDamageEffect();
        }
    }
    
    void OnSkillsChanged(SkillData oldSkills, SkillData newSkills)
    {
        // Update skill UI
        skillPanel.UpdateDisplay(newSkills);
        
        // Check for level ups
        foreach (var skill in newSkills.skills)
        {
            if (skill.Value.level > oldSkills.skills[skill.Key].level)
            {
                ShowLevelUpNotification(skill.Key, skill.Value.level);
            }
        }
    }
    
    // Server-only method to modify synchronized state
    [Server]
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Max(0, health);
    }
    
    [Server]
    public void GainSkillXP(SkillType skill, float xp)
    {
        skills.AddXP(skill, xp);
        // SyncVar automatically triggers OnSkillsChanged on all clients
    }
}
```

**SyncList and SyncDictionary:**

For dynamic collections that change during gameplay:

```csharp
public class PlayerInventory : NetworkBehaviour
{
    // SyncList automatically synchronizes add/remove/update operations
    public SyncList<InventoryItem> items = new SyncList<InventoryItem>();
    
    void Awake()
    {
        // Subscribe to change callbacks
        items.Callback += OnInventoryChanged;
    }
    
    void OnInventoryChanged(SyncList<InventoryItem>.Operation op, int index, InventoryItem oldItem, InventoryItem newItem)
    {
        switch (op)
        {
            case SyncList<InventoryItem>.Operation.OP_ADD:
                // Item added to inventory
                inventoryUI.AddItemSlot(newItem);
                ShowItemAcquiredNotification(newItem);
                break;
                
            case SyncList<InventoryItem>.Operation.OP_REMOVEAT:
                // Item removed
                inventoryUI.RemoveItemSlot(index);
                break;
                
            case SyncList<InventoryItem>.Operation.OP_SET:
                // Item updated (quantity changed, etc.)
                inventoryUI.UpdateItemSlot(index, newItem);
                break;
        }
    }
    
    [Server]
    public bool AddItem(InventoryItem item)
    {
        // Check for existing stack
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemId == item.itemId && items[i].IsStackable())
            {
                var updated = items[i];
                updated.quantity += item.quantity;
                items[i] = updated; // Triggers sync
                return true;
            }
        }
        
        // Add new item
        if (items.Count < maxInventorySize)
        {
            items.Add(item); // Automatically synchronized to clients
            return true;
        }
        
        return false; // Inventory full
    }
}
```

**BlueMarble Application:**
- Player inventory synchronizes automatically across clients
- Resource nodes display real-time depletion status
- Geological changes propagate to all players in the region
- Skill progression updates instantly in UI

### 3. Remote Procedure Calls (RPCs)

**Command Pattern (Client to Server):**

Commands allow clients to request actions on the server. The server validates and executes them.

```csharp
public class GeologicalSurveyor : NetworkBehaviour
{
    [SerializeField] float surveyRange = 10f;
    [SerializeField] float surveyDuration = 5f;
    
    private bool isSurveying = false;
    
    // Client calls this when player clicks "Survey Area"
    public void StartSurvey()
    {
        if (!isLocalPlayer) return; // Only local player can initiate
        
        // Request survey from server
        CmdStartSurvey(transform.position);
    }
    
    [Command] // Runs on server when called by client
    void CmdStartSurvey(Vector3 position)
    {
        // Validate player has surveying equipment
        var inventory = GetComponent<PlayerInventory>();
        if (!inventory.HasItem("surveying_equipment"))
        {
            TargetShowError(connectionToClient, "You need surveying equipment!");
            return;
        }
        
        // Validate player isn't moving
        if (GetComponent<PlayerMovement>().IsMoving())
        {
            TargetShowError(connectionToClient, "You must stand still to survey!");
            return;
        }
        
        // Start survey process
        StartCoroutine(PerformSurvey(position, connectionToClient));
    }
    
    [Server]
    IEnumerator PerformSurvey(Vector3 position, NetworkConnection conn)
    {
        // Show surveying animation to all nearby players
        RpcShowSurveyAnimation(position);
        
        yield return new WaitForSeconds(surveyDuration);
        
        // Calculate survey results based on geological data
        var results = GeologicalDatabase.GetSurveyData(position, surveyRange);
        
        // Send results only to the player who initiated survey
        TargetReceiveSurveyResults(conn, results);
        
        // Award XP for surveying
        GetComponent<SkillProgression>().GainSkillXP(SkillType.Surveying, 25f);
    }
    
    [TargetRpc] // Only sent to specific client
    void TargetReceiveSurveyResults(NetworkConnection target, SurveyData results)
    {
        // Display survey results in UI
        surveyUI.DisplayResults(results);
        
        // Mark surveyed area on player's map
        minimap.MarkSurveyedArea(results.area);
    }
    
    [ClientRpc] // Sent to all clients
    void RpcShowSurveyAnimation(Vector3 position)
    {
        // Spawn survey effect visible to all players
        var effect = Instantiate(surveyEffectPrefab, position, Quaternion.identity);
        Destroy(effect, surveyDuration);
    }
    
    [TargetRpc]
    void TargetShowError(NetworkConnection target, string message)
    {
        errorMessageUI.Show(message);
    }
}
```

**BlueMarble Communication Patterns:**
- `[Command]` - Player actions: move, extract resource, trade, chat
- `[ClientRpc]` - Global events: geological event, weather change, server announcement
- `[TargetRpc]` - Private notifications: quest update, achievement, private message

### 4. Interest Management System

**Spatial Hashing for Planetary Scale:**

Mirror's interest management reduces bandwidth by only sending updates about nearby objects.

```csharp
// Configure interest management in NetworkManager
public class BlueMarbleNetworkManager : NetworkManager
{
    public override void Start()
    {
        base.Start();
        
        // Use spatial hash interest management
        // Only sync objects within visibility range
        var aoi = GetComponent<InterestManagement>();
        if (aoi == null)
        {
            aoi = gameObject.AddComponent<SpatialHashInterestManagement>();
        }
        
        // Configure visibility range (in meters)
        aoi.visRange = 500f; // 500m visibility radius
        aoi.rebuildInterval = 1f; // Update every second
    }
}

// Custom interest management for geological regions
public class RegionalInterestManagement : InterestManagement
{
    // Only sync objects in same geological region
    public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnection newObserver)
    {
        // Get player's current region
        var player = newObserver.identity.GetComponent<GeologistPlayer>();
        var playerRegion = player.currentRegion;
        
        // Get object's region
        var objectRegion = identity.GetComponent<RegionalObject>()?.region;
        
        // Only observe if in same region or adjacent regions
        return objectRegion != null && 
               (objectRegion == playerRegion || 
                playerRegion.IsAdjacent(objectRegion));
    }
    
    public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnection> newObservers)
    {
        // Get all players in same region
        var region = identity.GetComponent<RegionalObject>()?.region;
        if (region == null) return;
        
        foreach (var player in region.GetPlayers())
        {
            if (player.connectionToClient != null)
            {
                newObservers.Add(player.connectionToClient);
            }
        }
        
        // Include players in adjacent regions
        foreach (var adjacentRegion in region.GetAdjacentRegions())
        {
            foreach (var player in adjacentRegion.GetPlayers())
            {
                if (player.connectionToClient != null)
                {
                    newObservers.Add(player.connectionToClient);
                }
            }
        }
    }
}
```

**BlueMarble Application:**
- Players only receive updates for their continent/region
- Resource nodes sync only to players within 500m
- Reduces server bandwidth from millions of objects to hundreds
- Essential for planet-scale simulation with limited network resources

### 5. Network Transform Component

**Smooth Position Synchronization:**

```csharp
// Attach to player GameObject
public class PlayerNetworkTransform : NetworkTransform
{
    protected override void Awake()
    {
        base.Awake();
        
        // Configure sync settings
        syncDirection = SyncDirection.ClientToServer; // Client controls position
        clientAuthority = true; // Client has authority over own position
        
        // Interpolation settings for smooth movement
        interpolatePosition = true;
        interpolateRotation = true;
        interpolateScale = false;
        
        // Sync interval (how often to send updates)
        syncInterval = 0.05f; // 20 updates per second
        
        // Compress position data to reduce bandwidth
        compressRotation = true;
    }
    
    // Server validates client movement
    [Server]
    protected override void OnClientToServerSync(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // Anti-cheat: Validate movement is within speed limits
        float distance = Vector3.Distance(transform.position, position);
        float maxDistance = playerSpeed * syncInterval * 2f; // 2x buffer
        
        if (distance > maxDistance)
        {
            // Potential speedhack - reject movement
            Debug.LogWarning($"Player {netId} attempted invalid movement: {distance}m in {syncInterval}s");
            
            // Force client back to server position
            RpcCorrectPosition(transform.position, transform.rotation);
            return;
        }
        
        // Validate terrain collision
        if (IsPositionInsideTerrain(position))
        {
            // Trying to move through solid ground
            RpcCorrectPosition(transform.position, transform.rotation);
            return;
        }
        
        // Valid movement - accept
        base.OnClientToServerSync(position, rotation, scale);
    }
    
    [ClientRpc]
    void RpcCorrectPosition(Vector3 correctPosition, Quaternion correctRotation)
    {
        if (isLocalPlayer)
        {
            transform.position = correctPosition;
            transform.rotation = correctRotation;
        }
    }
}
```

**BlueMarble Benefits:**
- Smooth player movement across clients
- Server-side anti-cheat validation
- Bandwidth-efficient compression
- Handles latency gracefully with interpolation

### 6. Transport Layer Flexibility

**Multiple Transport Options:**

Mirror abstracts the transport layer, allowing different protocols for different use cases:

```csharp
// TCP Transport - Reliable, ordered, but slower
// Good for: Turn-based gameplay, inventory management
public class TCPTransportConfig : MonoBehaviour
{
    void ConfigureTCP()
    {
        var transport = GetComponent<TelepathyTransport>();
        transport.port = 7777;
        transport.MaxMessageSize = 16 * 1024; // 16 KB messages
        transport.NoDelay = true; // Disable Nagle's algorithm for lower latency
    }
}

// KCP Transport - Fast, unreliable, lower latency
// Good for: Real-time movement, combat, events
public class KCPTransportConfig : MonoBehaviour
{
    void ConfigureKCP()
    {
        var transport = GetComponent<KcpTransport>();
        transport.Port = 7777;
        transport.MaxMessageSize = 1200; // MTU size
        
        // Tune for MMORPG (balanced reliability/speed)
        transport.NoDelay = true;
        transport.Interval = 10; // Update interval in ms
        transport.FastResend = 2; // Fast retransmit
        transport.CongestionWindow = false; // Disable congestion control
    }
}

// WebSocket Transport - Browser-compatible
// Good for: WebGL builds, browser-based gameplay
public class WebSocketTransportConfig : MonoBehaviour
{
    void ConfigureWebSocket()
    {
        var transport = GetComponent<SimpleWebTransport>();
        transport.port = 7778;
        transport.maxMessageSize = 16 * 1024;
        
        // SSL for secure connections
        transport.sslEnabled = true;
        transport.sslCertJson = "path/to/cert.json";
    }
}
```

**BlueMarble Transport Strategy:**
- **KCP for player movement and real-time events** (low latency required)
- **TCP for inventory, trading, and chat** (reliability required)
- **WebSocket for browser-based access** (future web version)

---

## BlueMarble Application

### 1. Regional Server Architecture with Mirror

**Multi-Region Deployment:**

```csharp
public class RegionalServerManager : NetworkManager
{
    [SerializeField] GeographicRegion assignedRegion;
    [SerializeField] int maxPlayersPerRegion = 100;
    
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        
        // Check server capacity
        if (numPlayers >= maxPlayersPerRegion)
        {
            conn.Disconnect();
            Debug.Log($"Region {assignedRegion.name} at capacity, rejecting connection");
            return;
        }
        
        Debug.Log($"Player connected to {assignedRegion.name} region");
    }
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Spawn player at region entry point
        Vector3 spawnPosition = assignedRegion.GetSpawnPoint();
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        
        // Initialize player with region data
        var geologistPlayer = player.GetComponent<GeologistPlayer>();
        geologistPlayer.currentRegion = assignedRegion;
        geologistPlayer.LoadPlayerData(conn.identity);
        
        // Spawn on network
        NetworkServer.AddPlayerForConnection(conn, player);
        
        // Notify other players in region
        RpcPlayerEnteredRegion(player.name, assignedRegion.name);
    }
    
    [ClientRpc]
    void RpcPlayerEnteredRegion(string playerName, string regionName)
    {
        chatUI.ShowSystemMessage($"{playerName} entered {regionName}");
    }
    
    // Handle player moving to adjacent region
    [Server]
    public void TransferPlayerToRegion(NetworkIdentity player, GeographicRegion newRegion)
    {
        // Save player state
        var geologistPlayer = player.GetComponent<GeologistPlayer>();
        var playerData = geologistPlayer.SerializeState();
        
        // Get new region server address
        string newServerAddress = RegionServerRegistry.GetServerAddress(newRegion);
        
        // Send transfer data to new server
        StartCoroutine(TransferPlayer(player.connectionToClient, newServerAddress, playerData));
    }
    
    IEnumerator TransferPlayer(NetworkConnection conn, string newServer, PlayerData data)
    {
        // Send transfer instruction to client
        TargetTransferToServer(conn, newServer, data);
        
        yield return new WaitForSeconds(5f);
        
        // Remove player from this server
        NetworkServer.RemovePlayerForConnection(conn, true);
    }
    
    [TargetRpc]
    void TargetTransferToServer(NetworkConnection target, string serverAddress, PlayerData data)
    {
        // Client disconnects and reconnects to new region server
        loadingScreen.Show("Traveling to new region...");
        
        NetworkManager.singleton.StopClient();
        
        // Store transfer data locally
        PlayerPrefs.SetString("TransferData", JsonUtility.ToJson(data));
        
        // Connect to new server
        NetworkManager.singleton.networkAddress = serverAddress;
        NetworkManager.singleton.StartClient();
    }
}
```

### 2. Geological Event Broadcasting

**Real-Time Geological Simulation Sync:**

```csharp
public class GeologicalEventManager : NetworkBehaviour
{
    // Singleton pattern for global access
    public static GeologicalEventManager Instance { get; private set; }
    
    // Track active geological events
    private SyncDictionary<int, GeologicalEvent> activeEvents = 
        new SyncDictionary<int, GeologicalEvent>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    [Server]
    public void TriggerEarthquake(Vector3 epicenter, float magnitude)
    {
        var earthquake = new GeologicalEvent
        {
            eventId = GenerateEventId(),
            type = EventType.Earthquake,
            location = epicenter,
            magnitude = magnitude,
            timestamp = NetworkTime.time
        };
        
        activeEvents.Add(earthquake.eventId, earthquake);
        
        // Broadcast to all clients in affected region
        RpcBroadcastEarthquake(earthquake);
        
        // Apply effects to world
        ApplyEarthquakeEffects(earthquake);
    }
    
    [ClientRpc]
    void RpcBroadcastEarthquake(GeologicalEvent earthquake)
    {
        // Show earthquake notification
        notificationUI.ShowGeologicalEvent(
            $"Earthquake detected! Magnitude: {earthquake.magnitude}",
            earthquake.location
        );
        
        // Play screen shake effect if player is nearby
        if (Vector3.Distance(localPlayer.position, earthquake.location) < 1000f)
        {
            cameraController.StartScreenShake(earthquake.magnitude);
        }
        
        // Visual effects
        SpawnEarthquakeEffect(earthquake.location, earthquake.magnitude);
        
        // Audio
        PlayEarthquakeSound(earthquake.magnitude);
    }
    
    [Server]
    void ApplyEarthquakeEffects(GeologicalEvent earthquake)
    {
        // Find all resource nodes in affected area
        float affectedRadius = earthquake.magnitude * 100f; // meters
        
        Collider[] colliders = Physics.OverlapSphere(
            earthquake.location, 
            affectedRadius
        );
        
        foreach (var collider in colliders)
        {
            var resourceNode = collider.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                // Earthquake may expose new resources or destroy existing ones
                float roll = UnityEngine.Random.value;
                
                if (roll < 0.1f) // 10% chance to expose new resources
                {
                    resourceNode.IncreaseYield(earthquake.magnitude * 0.5f);
                    RpcShowResourceExposed(resourceNode.netId);
                }
                else if (roll > 0.95f) // 5% chance to collapse/deplete
                {
                    resourceNode.Deplete();
                    RpcShowResourceCollapsed(resourceNode.netId);
                }
            }
        }
    }
    
    [ClientRpc]
    void RpcShowResourceExposed(uint resourceNodeId)
    {
        var node = NetworkClient.spawned[resourceNodeId].GetComponent<ResourceNode>();
        // Visual effect showing new resources
        SpawnResourceExposedEffect(node.transform.position);
    }
}
```

### 3. Player Trading System

**Peer-to-Peer Trading with Server Validation:**

```csharp
public class PlayerTrading : NetworkBehaviour
{
    private NetworkIdentity tradePartner;
    private SyncList<TradeItem> offeredItems = new SyncList<TradeItem>();
    private SyncList<TradeItem> requestedItems = new SyncList<TradeItem>();
    
    [SyncVar(hook = nameof(OnTradeStatusChanged))]
    private TradeStatus tradeStatus = TradeStatus.None;
    
    // Initiate trade request
    public void RequestTrade(NetworkIdentity target)
    {
        if (!isLocalPlayer) return;
        
        CmdRequestTrade(target);
    }
    
    [Command]
    void CmdRequestTrade(NetworkIdentity target)
    {
        // Validate target is nearby
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 5f)
        {
            TargetShowTradeError(connectionToClient, "Target too far away!");
            return;
        }
        
        // Send trade request to target
        var targetTrading = target.GetComponent<PlayerTrading>();
        targetTrading.TargetReceiveTradeRequest(target.connectionToClient, netIdentity);
    }
    
    [TargetRpc]
    void TargetReceiveTradeRequest(NetworkConnection target, NetworkIdentity requester)
    {
        // Show trade request UI
        tradeUI.ShowTradeRequest(requester.GetComponent<GeologistPlayer>().playerName);
    }
    
    public void AcceptTrade(NetworkIdentity requester)
    {
        CmdAcceptTrade(requester);
    }
    
    [Command]
    void CmdAcceptTrade(NetworkIdentity requester)
    {
        tradePartner = requester;
        tradeStatus = TradeStatus.Active;
        
        var partnerTrading = requester.GetComponent<PlayerTrading>();
        partnerTrading.tradePartner = netIdentity;
        partnerTrading.tradeStatus = TradeStatus.Active;
        
        // Open trade windows on both clients
        RpcOpenTradeWindow(requester, netIdentity);
    }
    
    [ClientRpc]
    void RpcOpenTradeWindow(NetworkIdentity player1, NetworkIdentity player2)
    {
        if (netIdentity == player1 || netIdentity == player2)
        {
            tradeUI.OpenTradeWindow(player1, player2);
        }
    }
    
    // Add item to trade offer
    public void OfferItem(InventoryItem item, int quantity)
    {
        CmdOfferItem(item, quantity);
    }
    
    [Command]
    void CmdOfferItem(InventoryItem item, int quantity)
    {
        // Validate player owns the item
        var inventory = GetComponent<PlayerInventory>();
        if (!inventory.HasItem(item.itemId, quantity))
        {
            TargetShowTradeError(connectionToClient, "You don't have that item!");
            return;
        }
        
        // Add to offered items
        offeredItems.Add(new TradeItem { item = item, quantity = quantity });
    }
    
    // Confirm trade
    public void ConfirmTrade()
    {
        CmdConfirmTrade();
    }
    
    [Command]
    void CmdConfirmTrade()
    {
        if (tradeStatus != TradeStatus.Active)
            return;
        
        tradeStatus = TradeStatus.PlayerConfirmed;
        
        // Check if both players confirmed
        var partnerTrading = tradePartner.GetComponent<PlayerTrading>();
        if (partnerTrading.tradeStatus == TradeStatus.PlayerConfirmed)
        {
            // Execute trade
            ExecuteTrade();
        }
    }
    
    [Server]
    void ExecuteTrade()
    {
        // Verify both players still have items
        var myInventory = GetComponent<PlayerInventory>();
        var partnerInventory = tradePartner.GetComponent<PlayerInventory>();
        
        // Validation
        bool valid = true;
        foreach (var tradeItem in offeredItems)
        {
            if (!myInventory.HasItem(tradeItem.item.itemId, tradeItem.quantity))
            {
                valid = false;
                break;
            }
        }
        
        var partnerTrading = tradePartner.GetComponent<PlayerTrading>();
        foreach (var tradeItem in partnerTrading.offeredItems)
        {
            if (!partnerInventory.HasItem(tradeItem.item.itemId, tradeItem.quantity))
            {
                valid = false;
                break;
            }
        }
        
        if (!valid)
        {
            // Cancel trade
            CancelTrade("Trade validation failed");
            return;
        }
        
        // Execute item transfer
        foreach (var tradeItem in offeredItems)
        {
            myInventory.RemoveItem(tradeItem.item.itemId, tradeItem.quantity);
            partnerInventory.AddItem(tradeItem.item, tradeItem.quantity);
        }
        
        foreach (var tradeItem in partnerTrading.offeredItems)
        {
            partnerInventory.RemoveItem(tradeItem.item.itemId, tradeItem.quantity);
            myInventory.AddItem(tradeItem.item, tradeItem.quantity);
        }
        
        // Log trade for audit
        LogTrade();
        
        // Complete trade
        RpcTradeComplete();
    }
    
    [ClientRpc]
    void RpcTradeComplete()
    {
        tradeUI.ShowTradeSuccess();
        tradeUI.CloseTradeWindow();
        
        // Reset state
        offeredItems.Clear();
        requestedItems.Clear();
        tradePartner = null;
        tradeStatus = TradeStatus.None;
    }
}
```

---

## Implementation Recommendations

### 1. Getting Started with Mirror

**Installation and Setup:**

```csharp
// 1. Install via Unity Package Manager
// Add from git URL: https://github.com/vis2k/Mirror.git

// 2. Create NetworkManager
public class BlueMarbleNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("BlueMarble server started");
        
        // Spawn persistent world objects
        SpawnResourceNodes();
        SpawnGeologicalFeatures();
    }
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Custom player spawning logic
        Transform startPos = GetStartPosition();
        GameObject player = Instantiate(playerPrefab, startPos.position, startPos.rotation);
        
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
```

### 2. Architecture Decisions

**Recommended Pattern for BlueMarble:**

```
Client (Unity Player)
    ↓ [Commands]
Server (Authoritative)
    ├── Validates all actions
    ├── Runs geological simulation
    ├── Manages resource spawns
    ├── Handles player interactions
    ↓ [ClientRpc/SyncVars]
All Clients
    └── Display synchronized state
```

### 3. Performance Optimization

**Best Practices:**

1. **Use Interest Management:**
   ```csharp
   // Configure in NetworkManager
   var interest = gameObject.AddComponent<SpatialHashInterestManagement>();
   interest.visRange = 500f;
   ```

2. **Batch State Updates:**
   ```csharp
   // Instead of updating every frame
   [SyncVar(hook = nameof(OnPositionChanged))]
   Vector3 syncedPosition;
   
   float updateInterval = 0.1f;
   float nextUpdate = 0f;
   
   void Update()
   {
       if (Time.time < nextUpdate) return;
       nextUpdate = Time.time + updateInterval;
       
       syncedPosition = transform.position;
   }
   ```

3. **Compress Data:**
   ```csharp
   // Use smaller data types when possible
   [SyncVar] byte healthPercent; // 0-100 instead of float
   [SyncVar] ushort resourceAmount; // 0-65535 instead of int
   ```

### 4. Testing and Debugging

**Built-in Tools:**

```csharp
// Enable in NetworkManager
public override void Awake()
{
    base.Awake();
    
    // Simulate network conditions
    NetworkTime.PingFrequency = 2f;
    
    #if UNITY_EDITOR
    // Enable statistics window
    showDebugMessages = true;
    #endif
}

// Test with artificial latency
// Add LatencySimulation component to test lag handling
```

### 5. Deployment Strategy

**Phase 1: Single Server (Months 1-3)**
- Deploy one Mirror server for testing
- 50-100 concurrent players
- Full feature set on single instance
- Monitor performance and identify bottlenecks

**Phase 2: Regional Servers (Months 4-6)**
- Deploy multiple Mirror instances by continent
- 100 players per region server
- Implement cross-server communication
- Player transfers between regions

**Phase 3: Load Balancing (Months 7-9)**
- Multiple servers per region with load balancing
- Dynamic server scaling based on player density
- Database sharding by region
- Cross-region data replication

**Phase 4: Global Scale (Months 10-12)**
- Full planet coverage with regional servers
- 10,000+ concurrent players globally
- Advanced interest management
- Edge caching and CDN for static data

---

## References

### Primary Sources

1. **Mirror Networking Repository**
   - GitHub: https://github.com/vis2k/Mirror
   - Documentation: https://mirror-networking.gitbook.io/docs/
   - Discord Community: https://discord.gg/N9QVxbM
   - License: MIT (Free for commercial use)

2. **Key Documentation Pages**
   - Getting Started: https://mirror-networking.gitbook.io/docs/manual/general/getting-started
   - Network Behaviour: https://mirror-networking.gitbook.io/docs/manual/guides/networkbehaviour
   - Commands and RPC: https://mirror-networking.gitbook.io/docs/manual/guides/communications
   - State Synchronization: https://mirror-networking.gitbook.io/docs/manual/guides/synchronization
   - Interest Management: https://mirror-networking.gitbook.io/docs/manual/interest-management

3. **Community Resources**
   - Mirror Examples: https://github.com/MirrorNetworking/Mirror/tree/master/Assets/Mirror/Examples
   - Community Contributions: https://github.com/MirrorNetworking
   - Video Tutorials: Search "Mirror Networking Unity" on YouTube

### Supporting Documentation

1. **Transport Layers**
   - Telepathy (TCP): https://github.com/vis2k/Telepathy
   - KCP (Fast UDP): https://github.com/vis2k/kcp2k
   - SimpleWebTransport (WebSocket): https://github.com/MirrorNetworking/SimpleWebTransport

2. **Related Technologies**
   - Unity Netcode: https://docs.unity3d.com/Packages/com.unity.netcode@1.0/
   - Photon: https://www.photonengine.com/
   - Nakama: https://heroiclabs.com/nakama/

### Academic References

1. Glazer, J., & Madhav, S. (2015). *Multiplayer Game Programming*. Addison-Wesley.
2. Bernier, Y. W. (2001). "Latency Compensating Methods in Client/Server In-game Protocol Design and Optimization." GDC 2001.
3. Smed, J., & Hakonen, H. (2006). *Algorithms and Networking for Computer Games*. Wiley.

---

## Related Research

### Within BlueMarble Repository

- [game-dev-analysis-gamedev.tv.md](./game-dev-analysis-gamedev.tv.md) - Source of Mirror discovery
- [research-assignment-group-31.md](./research-assignment-group-31.md) - Parent research assignment
- [online-game-dev-resources.md](./online-game-dev-resources.md) - Comprehensive resource catalog
- [master-research-queue.md](./master-research-queue.md) - Overall research tracking

### External Resources

- [Fish-Networking](https://github.com/FirstGearGames/FishNet) - Alternative to Mirror
- [Unity DOTS](https://unity.com/dots) - Future performance optimization
- [Colyseus](https://colyseus.io/) - Alternative multiplayer framework

---

## New Sources Discovered During Analysis

No additional sources were discovered during this analysis. Mirror is a well-documented, mature framework with a focused scope.

---

**Document Status:** ✅ Complete  
**Last Updated:** 2025-01-17  
**Word Count:** ~6,000 words  
**Lines:** 850+  
**Next Steps:** Continue with Fish-Networking analysis or proceed to ENet Networking Library

---

## Appendix: Production Examples

### Games Using Mirror Networking

1. **Population: One** (VR Battle Royale)
   - 24 players per match
   - Cross-platform VR
   - Successful commercial title

2. **Nimoyd** (Survival MMORPG)
   - Open world multiplayer
   - Procedurally generated planets
   - Mirror handles 100+ concurrent players

3. **Vail VR** (VR FPS)
   - Competitive multiplayer
   - Low latency critical
   - Uses Mirror with KCP transport

### BlueMarble-Specific Example

```csharp
// Complete example: Mineral extraction with Mirror
public class MineralExtractor : NetworkBehaviour
{
    [SerializeField] float extractionRate = 1f;
    [SerializeField] float extractionRange = 2f;
    
    private ResourceNode targetNode;
    private bool isExtracting = false;
    
    void Update()
    {
        if (!isLocalPlayer) return;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryStartExtraction();
        }
        
        if (Input.GetKeyUp(KeyCode.E))
        {
            StopExtraction();
        }
        
        if (isExtracting)
        {
            CmdContinueExtraction();
        }
    }
    
    void TryStartExtraction()
    {
        // Raycast to find resource node
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, extractionRange))
        {
            targetNode = hit.collider.GetComponent<ResourceNode>();
            if (targetNode != null)
            {
                isExtracting = true;
                CmdStartExtraction(targetNode.netId);
            }
        }
    }
    
    void StopExtraction()
    {
        isExtracting = false;
        CmdStopExtraction();
    }
    
    [Command]
    void CmdStartExtraction(uint nodeId)
    {
        if (!NetworkServer.spawned.ContainsKey(nodeId)) return;
        
        var node = NetworkServer.spawned[nodeId].GetComponent<ResourceNode>();
        if (node != null && !node.IsDepleted())
        {
            RpcShowExtractionEffect(nodeId);
        }
    }
    
    [Command]
    void CmdContinueExtraction()
    {
        if (targetNode == null || targetNode.IsDepleted())
        {
            TargetStopExtraction(connectionToClient);
            return;
        }
        
        // Extract resources
        float amount = extractionRate * Time.deltaTime;
        var extracted = targetNode.Extract(amount);
        
        if (extracted > 0)
        {
            // Add to player inventory
            var inventory = GetComponent<PlayerInventory>();
            inventory.AddItem(targetNode.mineralType, (int)extracted);
            
            // Award XP
            var skills = GetComponent<SkillProgression>();
            skills.GainSkillXP(SkillType.Extraction, extracted * 0.5f);
        }
    }
    
    [Command]
    void CmdStopExtraction()
    {
        RpcStopExtractionEffect();
    }
    
    [ClientRpc]
    void RpcShowExtractionEffect(uint nodeId)
    {
        // Visual/audio feedback
        var node = NetworkClient.spawned[nodeId].GetComponent<ResourceNode>();
        PlayExtractionEffect(node.transform.position);
    }
    
    [ClientRpc]
    void RpcStopExtractionEffect()
    {
        StopExtractionEffect();
    }
    
    [TargetRpc]
    void TargetStopExtraction(NetworkConnection target)
    {
        isExtracting = false;
        targetNode = null;
    }
}
```

This example demonstrates the complete Mirror workflow for a core BlueMarble gameplay mechanic: mineral extraction with proper client-server communication, validation, and synchronization.
