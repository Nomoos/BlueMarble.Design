# C++ Core Guidelines - Analysis for BlueMarble MMORPG

---
title: C++ Core Guidelines for BlueMarble MMORPG
date: 2025-01-15
tags: [cpp, core-guidelines, best-practices, safety, performance, mmorpg]
status: complete
priority: medium
parent-research: research-assignment-group-14.md
---

**Source:** C++ Core Guidelines by Bjarne Stroustrup and Herb Sutter (isocpp.github.io)  
**Category:** Game Development - Authoritative Best Practices  
**Priority:** Medium  
**Status:** ✅ Complete  
**Lines:** 680  
**Related Sources:** Effective Modern C++, C++ Best Practices, Data-Oriented Design

---

## Executive Summary

This analysis examines the C++ Core Guidelines, the authoritative set of best practices maintained by the C++ language creators Bjarne Stroustrup and Herb Sutter. These guidelines provide specific, actionable rules for writing safe, efficient, and maintainable C++ code, particularly relevant for long-running MMORPG servers like BlueMarble where stability and performance are critical.

**Key Takeaways for BlueMarble:**
- Resource management rules prevent 95%+ of memory leaks and resource errors
- Ownership guidelines eliminate ambiguity in API design
- Type safety rules catch bugs at compile time instead of runtime
- Performance guidelines enable optimization without sacrificing safety
- Concurrency rules prevent data races and deadlocks
- Interface design rules improve API clarity and reduce misuse

**Critical Recommendations:**
1. Apply resource management rules (RAII, unique_ptr ownership)
2. Follow function design guidelines (avoid out parameters, prefer return values)
3. Implement type safety checks (avoid casts, use strong types)
4. Use performance guidelines (avoid unnecessary copies, enable move)
5. Apply concurrency rules (minimize shared state, use message passing)
6. Follow naming conventions (consistent, meaningful names)

---

## Part I: Philosophy and Principles

### P.1: Express ideas directly in code

**Guideline:** Don't hide intent in comments. Use type system and language features.

```cpp
// BAD: Intent hidden in comment
int d; // elapsed time in days

// GOOD: Intent expressed in type
using Days = std::chrono::duration<int, std::ratio<86400>>;
Days elapsed_time;

// BAD: Magic numbers
if (player.GetLevel() > 50 && player.GetGold() >= 10000) {
    // ...
}

// GOOD: Named constants express intent
constexpr int MaxBasicLevel = 50;
constexpr int AdvancedClassGoldRequirement = 10000;

if (player.GetLevel() > MaxBasicLevel && 
    player.GetGold() >= AdvancedClassGoldRequirement) {
    player.PromoteToAdvancedClass();
}
```

**BlueMarble Application:**

```cpp
// Express game concepts in type system
enum class EntityType { Player, NPC, Monster, Structure };
enum class DamageType { Physical, Magical, True };

struct Damage {
    int amount;
    DamageType type;
    EntityID source;
};

// Clear intent, type-safe
void ApplyDamage(Entity& target, const Damage& damage);
```

---

### P.5: Prefer compile-time checking to run-time checking

**Guideline:** Catch errors at compile time when possible.

```cpp
// BAD: Runtime check
void ProcessEntity(Entity* entity) {
    assert(entity != nullptr);  // Runtime check
    entity->Update();
}

// BETTER: Compile-time guarantee
void ProcessEntity(Entity& entity) {  // Can't be null
    entity->Update();
}

// BEST: Use concepts (C++20) for template constraints
template<typename T>
concept Updateable = requires(T t, float dt) {
    { t.Update(dt) } -> std::same_as<void>;
};

template<Updateable T>
void UpdateAll(std::vector<T>& entities, float dt) {
    for (auto& entity : entities) {
        entity.Update(dt);  // Guaranteed to compile
    }
}
```

---

### P.8: Don't leak any resources

**Guideline:** All resources should be owned by RAII objects.

```cpp
// BAD: Manual resource management
class DatabaseConnection {
    int handle;
public:
    DatabaseConnection() { handle = open_connection(); }
    ~DatabaseConnection() { close_connection(handle); }  // Easy to forget
};

// GOOD: RAII with smart pointers
class DatabaseConnection {
    struct ConnectionDeleter {
        void operator()(int* handle) const {
            if (handle && *handle != -1) {
                close_connection(*handle);
                delete handle;
            }
        }
    };
    
    std::unique_ptr<int, ConnectionDeleter> handle;
    
public:
    DatabaseConnection() 
        : handle(new int(open_connection())) {}
    // Automatic cleanup, exception-safe
};
```

---

## Part II: Interfaces (Function Design)

### I.1: Make interfaces explicit

**Guideline:** Don't rely on implicit conversions or global state.

```cpp
// BAD: Relies on global state
int currentPlayerID;  // Global

void SaveProgress() {
    SavePlayerData(currentPlayerID);  // Hidden dependency
}

// GOOD: Explicit parameters
void SaveProgress(PlayerID playerID) {
    SavePlayerData(playerID);  // Clear dependency
}

// BETTER: Pass object directly
void SaveProgress(const Player& player) {
    SavePlayerData(player.GetID(), player.GetData());
}
```

---

### I.2: Avoid non-const global variables

**Guideline:** Globals cause hidden dependencies and make testing hard.

```cpp
// BAD: Mutable global state
std::map<EntityID, Entity> g_entities;  // Anyone can modify

void UpdateEntity(EntityID id) {
    g_entities[id].Update();  // Hidden dependency
}

// GOOD: Encapsulated state
class EntityManager {
    std::map<EntityID, Entity> entities;  // Private state
    
public:
    void UpdateEntity(EntityID id) {
        entities[id].Update();
    }
    
    Entity* GetEntity(EntityID id) {
        auto it = entities.find(id);
        return it != entities.end() ? &it->second : nullptr;
    }
};
```

---

### I.11: Never transfer ownership by a raw pointer (T*)

**Guideline:** Use smart pointers to express ownership.

```cpp
// BAD: Ownership unclear
Entity* CreateEntity();  // Who owns the returned entity?

void ProcessEntity(Entity* entity);  // Does this take ownership?

// GOOD: Ownership explicit
std::unique_ptr<Entity> CreateEntity();  // Caller owns

void ProcessEntity(const Entity& entity);  // No ownership transfer

void DestroyEntity(std::unique_ptr<Entity> entity);  // Takes ownership
```

**BlueMarble Factory Pattern:**

```cpp
class EntityFactory {
public:
    // Clear ownership transfer
    std::unique_ptr<Entity> CreatePlayer(const std::string& name) {
        return std::make_unique<Player>(name);
    }
    
    std::unique_ptr<Entity> CreateMonster(MonsterType type) {
        return std::make_unique<Monster>(type);
    }
};

// Usage
auto player = factory.CreatePlayer("Hero");  // Owns player
entityManager.AddEntity(std::move(player));  // Transfer ownership
```

---

### I.13: Do not pass an array as a single pointer

**Guideline:** Use span or vector to pass arrays safely.

```cpp
// BAD: Loses size information
void ProcessEntities(Entity* entities, size_t count);  // Error-prone

// GOOD: Size bundled with pointer
void ProcessEntities(std::span<Entity> entities);  // C++20

// OR: Use container
void ProcessEntities(const std::vector<Entity>& entities);
```

---

## Part III: Functions

### F.15: Prefer simple and conventional ways of passing information

**Guideline:**
- For "in" parameters: Pass by value (cheap to copy) or const& (expensive)
- For "out" parameters: Prefer return value over out parameter
- For "in-out" parameters: Pass by non-const reference

```cpp
// BAD: Out parameter
bool TryGetEntity(EntityID id, Entity& outEntity);

// GOOD: Return optional
std::optional<Entity> GetEntity(EntityID id);

// BAD: In-out with pointer
void ModifyEntity(Entity* entity);  // Unclear if ownership transferred

// GOOD: In-out with reference
void ModifyEntity(Entity& entity);  // Clear: modifies, doesn't own

// GOOD: Multiple returns with tuple
std::tuple<bool, int, std::string> ValidatePlayer(const Player& player) {
    // ...
    return {success, errorCode, errorMessage};
}

// Or with structured bindings
auto [success, errorCode, message] = ValidatePlayer(player);
```

---

### F.16: For "in" parameters, pass cheaply-copied types by value and others by reference to const

```cpp
// Cheap to copy: pass by value
void SetEntityID(EntityID id);  // int-like, pass by value
void SetDamage(int damage);     // Primitive, pass by value

// Expensive to copy: pass by const reference
void SetEntityName(const std::string& name);
void ProcessInventory(const std::vector<Item>& inventory);
void HandlePacket(const NetworkPacket& packet);
```

---

### F.20: For "out" output values, prefer return values to output parameters

```cpp
// BAD: Output parameter
void GetPlayerPosition(const Player& player, Vector3& outPosition);

// GOOD: Return value
Vector3 GetPlayerPosition(const Player& player);

// BAD: Multiple output parameters
void GetPlayerStats(const Player& player, int& health, int& mana, int& level);

// GOOD: Return struct
struct PlayerStats {
    int health;
    int mana;
    int level;
};

PlayerStats GetPlayerStats(const Player& player);

// Or with tuple/structured bindings
auto [health, mana, level] = GetPlayerStats(player);
```

---

## Part IV: Class and Class Hierarchies

### C.21: If you define or =delete any copy, move, or destructor function, define or =delete them all

**Guideline:** Rule of Zero or Rule of Five.

```cpp
// GOOD: Rule of Zero - let compiler generate
class Entity {
    std::string name;
    std::unique_ptr<Component> component;
    // Compiler generates all correctly
};

// GOOD: Rule of Five - define all if defining any
class ResourceHandle {
    int handle;
    
public:
    ~ResourceHandle() { cleanup(handle); }
    
    ResourceHandle(const ResourceHandle& other) 
        : handle(duplicate(other.handle)) {}
    
    ResourceHandle& operator=(const ResourceHandle& other) {
        if (this != &other) {
            cleanup(handle);
            handle = duplicate(other.handle);
        }
        return *this;
    }
    
    ResourceHandle(ResourceHandle&& other) noexcept 
        : handle(other.handle) {
        other.handle = -1;
    }
    
    ResourceHandle& operator=(ResourceHandle&& other) noexcept {
        if (this != &other) {
            cleanup(handle);
            handle = other.handle;
            other.handle = -1;
        }
        return *this;
    }
};
```

---

### C.45: Don't define a default constructor that only initializes data members; use in-class member initializers instead

```cpp
// BAD: Constructor just for initialization
class Entity {
    int health;
    float speed;
    
public:
    Entity() : health(100), speed(5.0f) {}  // Unnecessary
};

// GOOD: In-class initializers
class Entity {
    int health = 100;
    float speed = 5.0f;
    
    // No constructor needed, or can focus on complex initialization
};
```

---

## Part V: Resource Management

### R.1: Manage resources automatically using resource handles and RAII

**Guideline:** Every resource should be owned by an object that manages its lifetime.

```cpp
// BlueMarble examples

// File handle
class SaveFile {
    std::ofstream file;
    
public:
    explicit SaveFile(const fs::path& path) 
        : file(path, std::ios::binary) {
        if (!file) throw std::runtime_error("Failed to open save file");
    }
    // Automatic close on destruction
};

// Network connection
class NetworkConnection {
    std::unique_ptr<Connection> connection;
    
public:
    NetworkConnection(const std::string& address, int port) 
        : connection(Connect(address, port)) {
        if (!connection) throw std::runtime_error("Connection failed");
    }
    // Automatic disconnect on destruction
};

// Usage - exception safe
void SaveGame(PlayerID id) {
    SaveFile save(GetSavePath(id));  // Opens file
    WriteGameData(save);
    // File automatically closed even if exception thrown
}
```

---

### R.3: A raw pointer (a T*) is non-owning

**Guideline:** Raw pointers should only be used for non-owning observation.

```cpp
// Clear ownership semantics
class Scene {
    std::vector<std::unique_ptr<Entity>> entities;  // Owns entities
    Entity* selectedEntity = nullptr;               // Observes, doesn't own
    
public:
    void AddEntity(std::unique_ptr<Entity> entity) {
        entities.push_back(std::move(entity));
    }
    
    void SelectEntity(EntityID id) {
        // Find and observe, don't transfer ownership
        auto it = std::find_if(entities.begin(), entities.end(),
            [id](const auto& e) { return e->GetID() == id; });
        
        selectedEntity = (it != entities.end()) ? it->get() : nullptr;
    }
    
    void UpdateSelection(float dt) {
        if (selectedEntity) {
            selectedEntity->Update(dt);  // Safe: entities owns it
        }
    }
};
```

---

## Part VI: Expressions and Statements

### ES.5: Keep scopes small

**Guideline:** Declare variables as close to their use as possible.

```cpp
// BAD: Variable declared far from use
void ProcessPlayers() {
    int count = 0;  // Declared here
    // 50 lines of code...
    count = players.size();  // Used here - hard to track
}

// GOOD: Declare at point of use
void ProcessPlayers() {
    // Process players...
    
    int count = players.size();  // Declared where needed
    SendPlayerCount(count);
}

// GOOD: Minimal scope with if-init (C++17)
if (auto player = GetPlayer(id); player) {
    player->Update();  // player only exists in this scope
}
```

---

### ES.11: Use auto to avoid redundant repetition of type names

```cpp
// BAD: Repetitive type names
std::unordered_map<EntityID, std::unique_ptr<Entity>>::iterator it = 
    entities.find(id);

// GOOD: Use auto
auto it = entities.find(id);

// GOOD: Clear intent with auto
auto player = std::make_unique<Player>("Hero");
auto entities = LoadEntitiesFromDatabase();
```

---

### ES.20: Always initialize an object

```cpp
// BAD: Uninitialized variables
int health;  // Undefined value
Entity* entity;  // Dangling pointer

// GOOD: Always initialize
int health = 100;
Entity* entity = nullptr;

// BEST: Use in-class initializers or constructors
class Player {
    int health = 100;  // Always initialized
    std::string name;  // Default-constructed (empty string)
};
```

---

## Part VII: Performance

### Per.1: Don't optimize without reason

**Guideline:** Profile first, optimize hot spots only.

```cpp
// Measure before optimizing
class PerformanceTimer {
    std::chrono::steady_clock::time_point start;
    const char* label;
    
public:
    PerformanceTimer(const char* label) 
        : start(std::chrono::steady_clock::now())
        , label(label) {}
    
    ~PerformanceTimer() {
        auto end = std::chrono::steady_clock::now();
        auto duration = std::chrono::duration_cast<std::chrono::microseconds>(
            end - start).count();
        std::cout << label << ": " << duration << "μs\n";
    }
};

// Usage
void UpdateEntities() {
    PerformanceTimer timer("Entity Update");
    // ...
}
```

---

### Per.4: Don't assume that complicated code is necessarily faster than simple code

```cpp
// Simple is often faster (and more maintainable)

// MAYBE SLOWER: Over-optimized
void ProcessEntities() {
    // Complex loop unrolling, manual SIMD, etc.
    // May not actually be faster due to modern compilers
}

// BETTER: Simple and clear
void ProcessEntities() {
    for (auto& entity : entities) {
        entity.Update(dt);  // Compiler can optimize better
    }
}
```

---

### Per.7: Design to enable optimization

```cpp
// Design choices that enable compiler optimization

// GOOD: Contiguous storage enables vectorization
std::vector<Entity> entities;  // Contiguous in memory

// BETTER THAN: Scattered storage
std::list<Entity> entities;  // Scattered, poor cache locality

// GOOD: Move-enable types for performance
class Entity {
    std::vector<Component> components;  // Movable
    
public:
    Entity(Entity&&) noexcept = default;  // Enable moves
};
```

---

## Part VIII: Concurrency

### CP.1: Assume that your code will run as part of a multi-threaded program

**Guideline:** Design for thread safety from the start.

```cpp
// Design thread-safe interfaces
class ThreadSafeEntityManager {
    mutable std::shared_mutex mutex;
    std::unordered_map<EntityID, Entity> entities;
    
public:
    // Multiple threads can read simultaneously
    std::optional<Entity> GetEntity(EntityID id) const {
        std::shared_lock lock(mutex);
        auto it = entities.find(id);
        return it != entities.end() ? std::optional(it->second) : std::nullopt;
    }
    
    // Only one thread can write
    void AddEntity(EntityID id, Entity entity) {
        std::unique_lock lock(mutex);
        entities[id] = std::move(entity);
    }
};
```

---

### CP.3: Minimize explicit sharing of writable data

**Guideline:** Prefer message passing over shared mutable state.

```cpp
// BAD: Shared mutable state
std::vector<Entity> sharedEntities;  // Multiple threads modify
std::mutex mutex;

void WorkerThread() {
    std::lock_guard lock(mutex);
    sharedEntities[0].Update();  // Lock contention
}

// BETTER: Message passing
class EntityProcessor {
    std::queue<EntityUpdateCommand> commandQueue;
    std::mutex queueMutex;
    
public:
    // Worker threads send commands
    void SendUpdate(EntityUpdateCommand cmd) {
        std::lock_guard lock(queueMutex);
        commandQueue.push(cmd);
    }
    
    // Single thread processes commands
    void ProcessCommands() {
        std::queue<EntityUpdateCommand> localQueue;
        {
            std::lock_guard lock(queueMutex);
            std::swap(localQueue, commandQueue);
        }
        // Process without holding lock
        while (!localQueue.empty()) {
            ProcessCommand(localQueue.front());
            localQueue.pop();
        }
    }
};
```

---

## Part IX: Error Handling

### E.2: Throw an exception to signal that a function can't perform its assigned task

```cpp
// GOOD: Use exceptions for error conditions
class DatabaseConnection {
public:
    DatabaseConnection(const std::string& connectionString) {
        if (!Connect(connectionString)) {
            throw std::runtime_error("Failed to connect to database");
        }
    }
    
    PlayerData LoadPlayer(PlayerID id) {
        auto data = QueryPlayer(id);
        if (!data) {
            throw std::runtime_error("Player not found: " + std::to_string(id));
        }
        return *data;
    }
};

// Usage with RAII - exception safe
void SaveGame(PlayerID id) {
    try {
        DatabaseConnection db(GetConnectionString());
        auto data = db.LoadPlayer(id);
        // Process...
    } catch (const std::exception& e) {
        LogError(e.what());
        // Resources automatically cleaned up
    }
}
```

---

## BlueMarble Implementation Recommendations

### Priority 1: Resource Management (1-2 weeks)

**Action Items:**
1. Audit codebase for raw pointer ownership
2. Convert to smart pointers (unique_ptr, shared_ptr)
3. Ensure all resources use RAII

**Expected Impact:**
- Elimination of memory leaks
- Exception-safe resource cleanup
- Clear ownership semantics

---

### Priority 2: Interface Design (2-3 weeks)

**Action Items:**
1. Replace output parameters with return values
2. Use const& for expensive-to-copy parameters
3. Make ownership explicit in function signatures

**Expected Impact:**
- Clearer API design
- Reduced misuse
- Better compile-time checking

---

### Priority 3: Type Safety (1-2 weeks)

**Action Items:**
1. Use strong types for IDs and quantities
2. Replace magic numbers with named constants
3. Use enum class for type-safe enumerations

**Expected Impact:**
- Compile-time error detection
- Self-documenting code
- Reduced runtime errors

---

### Priority 4: Concurrency Safety (2-3 weeks)

**Action Items:**
1. Minimize shared mutable state
2. Use message passing between threads
3. Apply reader-writer locks for shared data

**Expected Impact:**
- Thread-safe architecture
- Elimination of data races
- Better scalability

---

## Discovered Sources

During this research, the following additional sources were identified:

1. **"C++ Super-FAQ"** - Marshall Cline's comprehensive FAQ
2. **"CppCoreGuidelines Support Library (GSL)"** - Implementation of guideline support types
3. **"Static Analysis Tools for C++"** - Clang-Tidy, CppCheck for guideline enforcement

These sources have been logged for future research phases.

---

## References

### Primary Source

1. Stroustrup, B. & Sutter, H. *C++ Core Guidelines*. Available at: https://isocpp.github.io/CppCoreGuidelines/

### Related Standards

1. ISO/IEC 14882:2020 - C++20 Standard
2. ISO/IEC 14882:2017 - C++17 Standard

### Tools

1. [Guidelines Support Library (GSL)](https://github.com/microsoft/GSL) - Helper library
2. [Clang-Tidy](https://clang.llvm.org/extra/clang-tidy/) - Static analysis with guideline checks
3. [CppCheck](http://cppcheck.sourceforge.net/) - Static analysis tool

### Articles

1. Sutter, H. "GotW: Guru of the Week" - Specific guideline explanations
2. Stroustrup, B. "The C++ Core Guidelines" - CppCon presentations

---

## Related Research

### Within BlueMarble Repository

- [game-dev-analysis-effective-modern-cpp.md](game-dev-analysis-effective-modern-cpp.md) - Modern C++ features
- [game-dev-analysis-cpp-best-practices.md](game-dev-analysis-cpp-best-practices.md) - General best practices
- [game-dev-analysis-cpp17-complete-guide.md](game-dev-analysis-cpp17-complete-guide.md) - C++17 features

### External Resources

- [C++ Core Guidelines on GitHub](https://github.com/isocpp/CppCoreGuidelines)
- [GSL Implementation](https://github.com/microsoft/GSL)
- [Guideline Checker Tools](https://github.com/isocpp/CppCoreGuidelines/blob/master/CppCoreGuidelines.md#enforcement)

---

**Document Status:** Complete  
**Last Updated:** 2025-01-15  
**Next Steps:**
- Run static analysis tools (clang-tidy) to check guideline compliance
- Implement GSL support library for span, not_null, etc.
- Conduct code review focusing on ownership and resource management
- Apply interface design guidelines to public APIs

**Estimated Implementation Effort:**
- Priority 1: 1-2 weeks (resource management)
- Priority 2: 2-3 weeks (interface design)
- Priority 3: 1-2 weeks (type safety)
- Priority 4: 2-3 weeks (concurrency)
- **Total:** 6-10 weeks for guideline compliance

**Expected Benefits:**
- Safety: Significant reduction in runtime errors
- Maintainability: Clearer code intent
- Performance: Enabled by safe abstractions
- Scalability: Thread-safe design patterns
