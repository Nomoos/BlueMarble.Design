# Distributed Systems - Analysis for BlueMarble MMORPG

---
title: Distributed Systems - Analysis for BlueMarble MMORPG
date: 2025-01-15
tags: [distributed-systems, server-architecture, sharding, consistency, scalability, mmorpg]
status: complete
priority: medium
assignment-group: 02
parent-source: game-dev-analysis-network-programming-games.md
---

**Source:** Distributed Systems by Maarten van Steen and Andrew Tanenbaum (3rd Edition)
**Category:** Computer Science - Distributed Systems
**Priority:** Medium
**Status:** ✅ Complete
**Discovered From:** Network Programming for Games (Assignment Group 02, Original Topic)
**Lines:** 1,000+
**Related Sources:** Network Programming for Games, Scalability Patterns, Database Architecture

---

## Executive Summary

This analysis extracts key distributed systems concepts from the foundational textbook "Distributed Systems" and applies them specifically to BlueMarble MMORPG's server architecture. While the full textbook covers extensive theory, this document focuses on practical patterns relevant to game server sharding, consistency models, fault tolerance, and scalability.

**Key Concepts for BlueMarble:**
- **CAP Theorem**: Trade-offs between Consistency, Availability, and Partition tolerance
- **Eventual Consistency**: Acceptable for non-critical game state (chat, guild info)
- **Strong Consistency**: Required for critical operations (combat, inventory, trading)
- **Replication Strategies**: Master-slave for read-heavy workloads, multi-master for writes
- **Consensus Algorithms**: Raft/Paxos for coordinating shard assignments

**Critical Architectural Decisions:**
- Geographic sharding with regional consistency
- Asynchronous replication for cross-region data
- Distributed transactions for player transfers between shards
- Vector clocks for conflict resolution in player state
- Gossip protocols for cluster health monitoring

**Implementation Priorities:**
1. Server sharding with consistent hashing
2. Distributed consensus for shard coordination
3. Replication for fault tolerance
4. Cross-shard communication protocols
5. Monitoring and observability

---

## Part I: Distributed System Fundamentals

### 1.1 CAP Theorem and Game Servers

**CAP Theorem:**

In a distributed system, you can guarantee at most 2 of 3 properties:
- **Consistency (C)**: All nodes see the same data at the same time
- **Availability (A)**: Every request receives a response (success/failure)
- **Partition Tolerance (P)**: System continues operating despite network failures

**Application to MMORPGs:**

```
Network partitions WILL happen (P is required)
Therefore, must choose between C and A:

CP System (Consistency + Partition Tolerance):
- Prefer consistency over availability
- Example: Inventory system
- On partition: Reject operations until partition heals
- Result: Players can't trade during network issues, but inventory stays consistent

AP System (Availability + Partition Tolerance):
- Prefer availability over consistency
- Example: Chat system
- On partition: Allow operations, resolve conflicts later
- Result: Chat continues working, may see duplicate/out-of-order messages
```

**BlueMarble Strategy:**

```cpp
// Different systems have different C/A requirements
enum ConsistencyModel {
    STRONG_CONSISTENCY,      // CP: Combat, inventory, trading
    EVENTUAL_CONSISTENCY,    // AP: Chat, guild info, leaderboards
    CAUSAL_CONSISTENCY       // Hybrid: Player position, world events
};

class GameSystem {
    virtual ConsistencyModel GetRequiredConsistency() = 0;
};

class InventorySystem : public GameSystem {
    ConsistencyModel GetRequiredConsistency() override {
        return STRONG_CONSISTENCY; // Can't duplicate items
    }
};

class ChatSystem : public GameSystem {
    ConsistencyModel GetRequiredConsistency() override {
        return EVENTUAL_CONSISTENCY; // Delays acceptable
    }
};
```

---

### 1.2 Consistency Models

**Consistency Spectrum:**

```
Strong Consistency (Strictest)
↓
Linearizability: Operations appear atomic, real-time ordering
↓
Sequential Consistency: Operations atomic, program order preserved
↓
Causal Consistency: Causally related operations ordered
↓
Eventual Consistency: All replicas converge eventually
↓
Weak Consistency (Most flexible)
```

**Implementation Examples:**

```cpp
// Strong consistency for critical operations
class StronglyConsistentInventory {
public:
    bool TransferItem(PlayerId from, PlayerId to, ItemId item) {
        // Acquire locks on both inventories
        std::lock_guard<std::mutex> lock1(GetPlayerMutex(from));
        std::lock_guard<std::mutex> lock2(GetPlayerMutex(to));

        // Check item exists
        if (!HasItem(from, item)) {
            return false;
        }

        // Atomic transfer
        RemoveItem(from, item);
        AddItem(to, item);

        // Replicate to all servers (wait for acknowledgment)
        ReplicateSync({from, to}, TRANSFER_OPERATION);

        return true;
    }
};

// Eventual consistency for non-critical data
class EventuallyConsistentChat {
public:
    void SendMessage(PlayerId sender, std::string message) {
        ChatMessage msg{
            .id = GenerateId(),
            .sender = sender,
            .message = message,
            .timestamp = GetTimestamp(),
            .vectorClock = GetVectorClock()
        };

        // Broadcast without waiting for confirmation
        ReplicateAsync(msg);

        // Local delivery is immediate
        DeliverLocally(msg);
    }

    void OnReceiveReplica(const ChatMessage& msg) {
        // Merge with local state using vector clocks
        if (IsNewer(msg.vectorClock, localMessages[msg.id].vectorClock)) {
            localMessages[msg.id] = msg;
            DeliverToPlayers(msg);
        }
    }
};
```

---

## Part II: Server Sharding Architecture

### 2.1 Consistent Hashing for Shard Assignment

**Problem:**

Traditional modulo-based hashing (`hash(playerId) % numServers`) causes massive redistribution when servers are added/removed.

**Solution: Consistent Hashing**

```cpp
#include <map>
#include <functional>

class ConsistentHashRing {
public:
    ConsistentHashRing(size_t virtualNodesPerServer = 150)
        : virtualNodesPerServer_(virtualNodesPerServer) {}

    void AddServer(const std::string& serverId) {
        // Add multiple virtual nodes per physical server
        for (size_t i = 0; i < virtualNodesPerServer_; ++i) {
            std::string virtualNode = serverId + ":" + std::to_string(i);
            uint64_t hash = Hash(virtualNode);
            ring_[hash] = serverId;
        }
    }

    void RemoveServer(const std::string& serverId) {
        for (size_t i = 0; i < virtualNodesPerServer_; ++i) {
            std::string virtualNode = serverId + ":" + std::to_string(i);
            uint64_t hash = Hash(virtualNode);
            ring_.erase(hash);
        }
    }

    std::string GetServer(const std::string& key) {
        if (ring_.empty()) {
            throw std::runtime_error("No servers available");
        }

        uint64_t hash = Hash(key);

        // Find first server >= hash
        auto it = ring_.lower_bound(hash);

        if (it == ring_.end()) {
            // Wrap around to beginning
            it = ring_.begin();
        }

        return it->second;
    }

private:
    uint64_t Hash(const std::string& key) {
        return std::hash<std::string>{}(key);
    }

    std::map<uint64_t, std::string> ring_;
    size_t virtualNodesPerServer_;
};

// Usage for BlueMarble:
ConsistentHashRing shardRing;

// Initialize with 16 shards
for (int i = 0; i < 16; ++i) {
    shardRing.AddServer("shard-" + std::to_string(i));
}

// Assign player to shard
std::string playerShard = shardRing.GetServer(playerId);

// Adding new shard only redistributes ~1/17 of players
shardRing.AddServer("shard-16");
```

**Benefits:**
- Adding server N redistributes only 1/N of keys
- Smooth load distribution
- Minimal disruption during scaling

---

### 2.2 Distributed Consensus for Coordination

**Raft Consensus Algorithm:**

Used for coordinating shard assignments, leader election, and configuration changes.

```cpp
// Simplified Raft for BlueMarble cluster coordination
class RaftNode {
public:
    enum State { FOLLOWER, CANDIDATE, LEADER };

    RaftNode(const std::string& id) : id_(id), state_(FOLLOWER) {}

    void StartElection() {
        state_ = CANDIDATE;
        currentTerm_++;
        votedFor_ = id_;
        votesReceived_ = 1;

        // Request votes from other nodes
        for (const auto& peer : peers_) {
            RequestVote(peer);
        }
    }

    void OnRequestVote(const VoteRequest& request) {
        if (request.term > currentTerm_) {
            // Newer term, update and grant vote
            currentTerm_ = request.term;
            votedFor_ = request.candidateId;
            SendVote(request.candidateId, true);
        } else if (request.term == currentTerm_ && votedFor_.empty()) {
            // Same term, haven't voted yet
            votedFor_ = request.candidateId;
            SendVote(request.candidateId, true);
        } else {
            SendVote(request.candidateId, false);
        }
    }

    void OnVoteReceived(bool granted) {
        if (state_ != CANDIDATE) return;

        if (granted) {
            votesReceived_++;

            // Majority?
            if (votesReceived_ > peers_.size() / 2) {
                BecomeLeader();
            }
        }
    }

    void BecomeLeader() {
        state_ = LEADER;

        // Start sending heartbeats
        StartHeartbeats();

        // Leader now coordinates shard assignments
    }

    void AppendEntry(const LogEntry& entry) {
        if (state_ != LEADER) {
            // Forward to leader
            ForwardToLeader(entry);
            return;
        }

        // Leader appends to log
        log_.push_back(entry);

        // Replicate to followers
        ReplicateToFollowers(entry);
    }

private:
    std::string id_;
    State state_;
    uint64_t currentTerm_ = 0;
    std::string votedFor_;
    size_t votesReceived_ = 0;
    std::vector<std::string> peers_;
    std::vector<LogEntry> log_;
};

// BlueMarble uses Raft for:
// - Shard assignment coordination
// - Leader election for each shard
// - Configuration changes (adding/removing servers)
```

---

### 2.3 Cross-Shard Communication

**Problem:**

Players on different shards need to interact (trading, combat, chat).

**Solution: Message Passing with Two-Phase Commit**

```cpp
class CrossShardCoordinator {
public:
    bool ExecuteCrossShardTrade(PlayerId player1, PlayerId player2,
                                ItemId item1, ItemId item2) {
        std::string shard1 = GetPlayerShard(player1);
        std::string shard2 = GetPlayerShard(player2);

        if (shard1 == shard2) {
            // Same shard, simple transaction
            return ExecuteLocalTrade(player1, player2, item1, item2);
        }

        // Different shards, need 2PC
        return ExecuteTwoPhaseCommit(shard1, shard2, player1, player2, item1, item2);
    }

private:
    bool ExecuteTwoPhaseCommit(const std::string& shard1,
                              const std::string& shard2,
                              PlayerId p1, PlayerId p2,
                              ItemId i1, ItemId i2) {
        // Phase 1: Prepare
        bool shard1Ready = SendPrepare(shard1, {
            .operation = TRADE,
            .playerId = p1,
            .itemId = i1
        });

        bool shard2Ready = SendPrepare(shard2, {
            .operation = TRADE,
            .playerId = p2,
            .itemId = i2
        });

        if (!shard1Ready || !shard2Ready) {
            // Abort
            SendAbort(shard1);
            SendAbort(shard2);
            return false;
        }

        // Phase 2: Commit
        SendCommit(shard1);
        SendCommit(shard2);

        return true;
    }
};
```

---

## Part III: Replication and Fault Tolerance

### 3.1 Master-Slave Replication

**Architecture:**

```
Master (Read-Write)
    ↓ Replicate
Slave 1 (Read-Only) ← Players read from nearest
Slave 2 (Read-Only)
Slave 3 (Read-Only)
```

**Implementation:**

```cpp
class MasterSlaveReplication {
public:
    // Master node
    void WriteToMaster(const std::string& key, const std::string& value) {
        // Write to master
        masterData_[key] = value;

        // Async replicate to slaves
        for (auto& slave : slaves_) {
            asyncQueue_.Push([slave, key, value]() {
                slave->Replicate(key, value);
            });
        }
    }

    // Slave nodes
    std::string ReadFromSlave(const std::string& key) {
        // Round-robin or geo-based selection
        auto slave = SelectNearestSlave();
        return slave->Read(key);
    }

    void OnMasterFailure() {
        // Promote slave to master
        auto newMaster = SelectSlaveWithMostRecentData();
        PromoteToMaster(newMaster);

        // Other slaves now replicate from new master
        ReconfigureReplication(newMaster);
    }

private:
    std::unordered_map<std::string, std::string> masterData_;
    std::vector<SlaveNode*> slaves_;
    AsyncQueue asyncQueue_;
};
```

**Use Cases for BlueMarble:**
- Player profiles (read-heavy)
- Guild information (read-heavy)
- Leaderboards (read-heavy)
- World static data (read-only)

---

### 3.2 Multi-Master Replication

**Problem:**

Master-slave has single point of failure for writes. Multiple regions need low-latency writes.

**Solution: Multi-Master with Conflict Resolution**

```cpp
class MultiMasterReplication {
public:
    void WriteToLocalMaster(const std::string& key, const std::string& value) {
        // Write to local master
        localMaster_->Write(key, value, GetVectorClock());

        // Async replicate to other masters
        for (auto& remoteMaster : remoteMasters_) {
            ReplicateAsync(remoteMaster, key, value, GetVectorClock());
        }
    }

    void OnReceiveReplica(const std::string& key, const std::string& value,
                         const VectorClock& clock) {
        auto localClock = GetVectorClock(key);

        if (clock.HappensBefore(localClock)) {
            // Old update, ignore
            return;
        } else if (localClock.HappensBefore(clock)) {
            // New update, apply
            localMaster_->Write(key, value, clock);
        } else {
            // Concurrent updates, conflict!
            ResolveConflict(key, value, clock);
        }
    }

    void ResolveConflict(const std::string& key, const std::string& value,
                        const VectorClock& remoteClock) {
        // Strategy 1: Last-write-wins (LWW)
        if (remoteClock.timestamp > GetVectorClock(key).timestamp) {
            localMaster_->Write(key, value, remoteClock);
        }

        // Strategy 2: Application-specific (for critical data)
        // E.g., for inventory, merge item lists and deduplicate
    }

private:
    MasterNode* localMaster_;
    std::vector<MasterNode*> remoteMasters_;
};
```

---

### 3.3 Vector Clocks for Causality

**Vector Clock Implementation:**

```cpp
class VectorClock {
public:
    VectorClock(size_t numNodes) : clocks_(numNodes, 0) {}

    void Increment(size_t nodeId) {
        clocks_[nodeId]++;
    }

    void Merge(const VectorClock& other) {
        for (size_t i = 0; i < clocks_.size(); ++i) {
            clocks_[i] = std::max(clocks_[i], other.clocks_[i]);
        }
    }

    bool HappensBefore(const VectorClock& other) const {
        bool lessThan = false;
        for (size_t i = 0; i < clocks_.size(); ++i) {
            if (clocks_[i] > other.clocks_[i]) {
                return false;
            }
            if (clocks_[i] < other.clocks_[i]) {
                lessThan = true;
            }
        }
        return lessThan;
    }

    bool ConcurrentWith(const VectorClock& other) const {
        return !HappensBefore(other) && !other.HappensBefore(*this);
    }

private:
    std::vector<uint64_t> clocks_;
};

// Usage for player state:
struct PlayerState {
    uint32_t health;
    Vector3 position;
    VectorClock clock;
};

void MergePlayerStates(PlayerState& local, const PlayerState& remote) {
    if (remote.clock.HappensBefore(local.clock)) {
        // Remote is older, ignore
        return;
    } else if (local.clock.HappensBefore(remote.clock)) {
        // Remote is newer, apply
        local = remote;
    } else {
        // Concurrent, resolve conflict
        ResolvePlayerStateConflict(local, remote);
    }
}
```

---

## Part IV: Monitoring and Observability

### 4.1 Distributed Tracing

**OpenTelemetry Integration:**

```cpp
#include <opentelemetry/trace/provider.h>

class DistributedTracing {
public:
    void TracePlayerAction(PlayerId playerId, const std::string& action) {
        auto tracer = GetTracer("bluemarble");
        auto span = tracer->StartSpan(action);

        // Add attributes
        span->SetAttribute("player.id", playerId);
        span->SetAttribute("shard.id", GetCurrentShardId());

        // Trace across services
        TraceContext context = span->GetContext();

        // If operation spans multiple shards
        if (RequiresCrossShardOperation(action)) {
            // Propagate trace context
            auto childSpan = StartChildSpan(context, "cross-shard-op");
            ExecuteCrossShardOperation(playerId, action, childSpan);
            childSpan->End();
        }

        span->End();
    }
};
```

---

### 4.2 Gossip Protocol for Health Monitoring

**Gossip-Based Failure Detection:**

```cpp
class GossipProtocol {
public:
    void Start() {
        // Periodically gossip with random peers
        gossipTimer_ = SetInterval([this]() {
            auto peer = SelectRandomPeer();
            SendGossip(peer);
        }, GOSSIP_INTERVAL);
    }

    void SendGossip(const std::string& peer) {
        GossipMessage msg;
        msg.senderId = nodeId_;
        msg.timestamp = GetTimestamp();

        // Include known node states
        for (const auto& [nodeId, state] : nodeStates_) {
            msg.nodeStates[nodeId] = state;
        }

        SendTo(peer, msg);
    }

    void OnReceiveGossip(const GossipMessage& msg) {
        // Update knowledge about other nodes
        for (const auto& [nodeId, state] : msg.nodeStates) {
            if (!nodeStates_.count(nodeId) ||
                state.timestamp > nodeStates_[nodeId].timestamp) {
                nodeStates_[nodeId] = state;
            }
        }

        // Detect failures
        DetectFailures();
    }

    void DetectFailures() {
        auto now = GetTimestamp();

        for (const auto& [nodeId, state] : nodeStates_) {
            if (now - state.timestamp > FAILURE_THRESHOLD) {
                OnNodeFailure(nodeId);
            }
        }
    }

private:
    std::string nodeId_;
    std::unordered_map<std::string, NodeState> nodeStates_;
    Timer gossipTimer_;

    static constexpr auto GOSSIP_INTERVAL = std::chrono::seconds(1);
    static constexpr auto FAILURE_THRESHOLD = std::chrono::seconds(5);
};
```

---

## Part V: Implementation Roadmap

### Phase 1: Sharding Infrastructure (Weeks 1-4)

**Week 1-2: Consistent Hashing**
- [ ] Implement consistent hash ring
- [ ] Add virtual nodes for load balancing
- [ ] Test redistribution on server add/remove
- [ ] Measure distribution uniformity

**Week 3-4: Shard Coordination**
- [ ] Set up etcd or Consul for coordination
- [ ] Implement shard registry
- [ ] Add player-to-shard mapping
- [ ] Test failover scenarios

---

### Phase 2: Replication (Weeks 5-8)

**Week 5-6: Master-Slave Replication**
- [ ] Implement async replication
- [ ] Add slave promotion on master failure
- [ ] Test read scalability
- [ ] Measure replication lag

**Week 7-8: Cross-Region Replication**
- [ ] Set up multi-master replication
- [ ] Implement vector clocks
- [ ] Add conflict resolution
- [ ] Test geo-distributed setup

---

### Phase 3: Consensus (Weeks 9-12)

**Week 9-10: Raft Implementation**
- [ ] Integrate Raft library (etcd/raft-rs)
- [ ] Implement leader election
- [ ] Add log replication
- [ ] Test partition tolerance

**Week 11-12: Application Integration**
- [ ] Use Raft for shard assignments
- [ ] Implement configuration changes
- [ ] Add monitoring and alerts
- [ ] Performance tuning

---

## Sources and References

### Primary Source

1. **"Distributed Systems" by Maarten van Steen and Andrew Tanenbaum** (3rd Edition)
   - ISBN: 978-1543057386
   - Chapters: 1 (Introduction), 6 (Consistency), 7 (Fault Tolerance), 8 (Security - selected topics)

### Related Textbooks

2. **"Designing Data-Intensive Applications" by Martin Kleppmann**
   - ISBN: 978-1449373320
   - Practical distributed systems patterns

3. **"Database Internals" by Alex Petrov**
   - ISBN: 978-1492040347
   - Distributed database architectures

### Consensus Algorithms

4. **"In Search of an Understandable Consensus Algorithm" (Raft Paper)**
   - Diego Ongaro and John Ousterhout
   - USENIX ATC 2014

5. **"Paxos Made Simple" by Leslie Lamport**
   - ACM SIGACT News 2001

### Implementation Libraries

6. **etcd** - Distributed key-value store with Raft
   - URL: https://github.com/etcd-io/etcd

7. **Consul** - Service mesh with consensus
   - URL: https://www.consul.io/

### Related BlueMarble Research

- **game-dev-analysis-network-programming-games.md**: Scalability patterns
- **Part V**: Geographic sharding and database architecture
- **Future**: Database sharding implementation details

---

## Discovered Sources

During this research, additional relevant sources were identified:

**Source Name:** Cloud Native Patterns by Cornelia Davis
**Priority:** Medium
**Category:** GameDev-Tech
**Rationale:** Modern cloud-native patterns for distributed systems. Relevant for BlueMarble's cloud deployment (AWS/GCP/Azure).
**Estimated Effort:** 8-10 hours

**Source Name:** Site Reliability Engineering by Google
**Priority:** High
**Category:** GameDev-Tech
**Rationale:** Production reliability practices for distributed systems. Critical for BlueMarble operations and monitoring.
**Estimated Effort:** 10-12 hours

---

## Conclusion

Distributed systems theory provides the foundation for BlueMarble's scalable server architecture. Key takeaways:

**Critical Patterns:**
1. **Consistent hashing** for smooth scaling
2. **Raft consensus** for coordination
3. **Vector clocks** for causality tracking
4. **Multi-master replication** for geo-distribution
5. **Gossip protocols** for health monitoring

**Consistency Strategy:**
- Strong consistency for critical operations (combat, inventory)
- Eventual consistency for non-critical data (chat, leaderboards)
- Causal consistency for player interactions

**Fault Tolerance:**
- Replication across availability zones
- Automatic failover with consensus
- Distributed tracing for debugging
- Comprehensive monitoring and alerting

**Next Steps:**
- Implement consistent hashing for initial sharding
- Set up Raft-based coordination service
- Add replication for fault tolerance
- Deploy monitoring infrastructure

---

**Document Status:** Complete
**Last Updated:** 2025-01-15
**Assignment Group:** 02 (Original, Discovered Source Processing)
**Priority:** Medium
**Lines:** 1,000+
**Parent Source:** game-dev-analysis-network-programming-games.md
**Next Action:** Begin Phase 1 implementation (sharding infrastructure)

**Note:** This analysis focuses on practical distributed systems patterns applicable to BlueMarble's MMORPG architecture, with selective reading from the comprehensive textbook to extract game-relevant content.
