# Frontend Task Manager - UI Design Specification

**Document Type:** UI/UX Design Specification  
**Version:** 1.0  
**Author:** UI/UX Design Team  
**Date:** 2025-11-09  
**Status:** Draft  
**Related Specifications:**
- [Building and Construction Mechanics](../gameplay/mechanics/building-construction.md)
- [Mining Resource Extraction](../gameplay/mechanics/mining-resource-extraction.md)

## Overview

The Frontend Task Manager is a critical UI component that provides players with comprehensive visibility and control over all tasks within the BlueMarble game world. This includes construction tasks, resource extraction tasks, crafting tasks, and worker assignments. The Task Manager serves as the central hub for planning, monitoring, and optimizing player activities.

## Purpose and Goals

### Primary Goals
1. **Task Visibility**: Provide clear, at-a-glance view of all active and pending tasks
2. **Task Management**: Enable easy creation, modification, and cancellation of tasks
3. **Worker Assignment**: Facilitate efficient worker allocation to tasks
4. **Progress Tracking**: Display real-time task progress and completion estimates
5. **Priority Management**: Allow players to set and adjust task priorities
6. **Resource Planning**: Show resource requirements and availability for tasks

### User Experience Principles
- **Clarity**: Information should be immediately understandable
- **Efficiency**: Common actions should require minimal clicks
- **Feedback**: Provide immediate visual feedback for all actions
- **Flexibility**: Support multiple workflows and user preferences
- **Responsiveness**: Update in real-time as game state changes

## Component Architecture

### Main Components

#### 1. TaskManagerPanel
**Component Name:** `TaskManagerPanel`

**Purpose:** Primary container for all task management functionality

**Key Features:**
- Tabbed interface for different task categories
- Filtering and sorting capabilities
- Bulk operations support
- Quick actions toolbar
- Real-time status updates

**Interface Layout:**
```
┌─────────────────────────────────────────────────────────────┐
│ ⚙️ Task Manager                          🔍 Search  ⚙️ ✕    │
├─────────────────────────────────────────────────────────────┤
│ [All] [Construction] [Mining] [Crafting] [Agriculture]      │
├──────┬──────────────────────────────────────────────────────┤
│      │ Sort by: [Priority ▼]  Filter: [Active ▼]           │
│ Ctrl │ ☑️ Select All  📋 Queue  ⏸️ Pause  ❌ Cancel         │
│      ├──────────────────────────────────────────────────────┤
│ ▼ Q  │ Task Name            │ Progress │ Workers │ ETA      │
│  u   ├─────────────────────┼──────────┼─────────┼──────────┤
│  e   │🏗️ Build Cottage      │ ████░░░░ │ 3/5     │ 2h 15m  │
│  u   │⛏️ Mine Iron Ore      │ ██████░░ │ 2/2     │ 45m     │
│  e   │🔨 Craft Tools        │ ██░░░░░░ │ 1/1     │ 3h 30m  │
│      │⚡ Paused: Clear Site │ ░░░░░░░░ │ 0/4     │ Paused  │
│ ▼ P  ├─────────────────────┴──────────┴─────────┴──────────┤
│  r   │                                                       │
│  o   │ [Task details shown when selected]                  │
│  g   │                                                       │
│  r   │                                                       │
│  s   │                                                       │
└──────┴───────────────────────────────────────────────────────┘
```

#### 2. TaskCard
**Component Name:** `TaskCard`

**Purpose:** Display individual task information in a card format

**Properties:**
- Task name and type icon
- Progress bar with percentage
- Worker allocation status
- Resource requirements indicator
- Priority badge
- Time remaining estimate
- Status indicators (active, paused, blocked, completed)

**States:**
- Active (in progress)
- Queued (waiting to start)
- Paused (temporarily stopped)
- Blocked (waiting for resources/dependencies)
- Completed (finished)
- Failed (error state)

**Visual Design:**
```
┌─────────────────────────────────────────┐
│ 🏗️ Build Cottage                    ⭐  │
│ Priority: High                          │
├─────────────────────────────────────────┤
│ Progress: 45%  [████████░░░░░░░░]      │
│ Workers: 3/5 👷👷👷○○                   │
│ Resources: ✅ Wood  ✅ Stone  ⚠️ Tools  │
│ ETA: 2h 15m                             │
├─────────────────────────────────────────┤
│ Phase: Foundation                       │
│ Current: Pour footings (75%)            │
├─────────────────────────────────────────┤
│ [⏸️ Pause] [➕ Add Worker] [📋 Details]│
└─────────────────────────────────────────┘
```

#### 3. TaskDetailPanel
**Component Name:** `TaskDetailPanel`

**Purpose:** Display comprehensive information about a selected task

**Sections:**
1. **Task Information**
   - Task name and type
   - Creation date/time
   - Started date/time
   - Expected completion time
   - Priority level

2. **Requirements**
   - Material requirements with quantities
   - Tool requirements
   - Skill requirements
   - Worker requirements

3. **Progress Breakdown**
   - Phase-by-phase progress
   - Sub-task completion status
   - Milestone indicators

4. **Worker Assignment**
   - Assigned workers list
   - Worker skills and efficiency
   - Add/remove worker controls

5. **Dependencies**
   - Prerequisites (completed/pending)
   - Blocking tasks
   - Dependent tasks

6. **Resource Management**
   - Current resource availability
   - Resource consumption rate
   - Delivery status

**Layout:**
```
┌─────────────────────────────────────────┐
│ 🏗️ Build Cottage - Details              │
├─────────────────────────────────────────┤
│ Created: Nov 9, 2025 10:23 AM           │
│ Started: Nov 9, 2025 11:00 AM           │
│ Expected: Nov 9, 2025 2:15 PM           │
├─────────────────────────────────────────┤
│ Requirements:                           │
│ ✅ Wood: 500/500 units                  │
│ ✅ Stone: 200/200 units                 │
│ ⚠️ Iron Tools: 3/5 units                │
│ ✅ Skilled Builder: 1 assigned          │
├─────────────────────────────────────────┤
│ Progress Breakdown:                     │
│ ✅ Site Preparation (100%)              │
│ ✅ Foundation (100%)                    │
│ ⏳ Framework (45%) - In Progress        │
│ ○ Roofing (0%) - Pending                │
│ ○ Finishing (0%) - Pending              │
├─────────────────────────────────────────┤
│ Assigned Workers (3/5):                 │
│ 👷 John (Builder, Efficiency: 95%)     │
│ 👷 Maria (Laborer, Efficiency: 80%)    │
│ 👷 Tom (Laborer, Efficiency: 75%)      │
│ [➕ Assign Worker] [➖ Remove Worker]   │
├─────────────────────────────────────────┤
│ Dependencies:                           │
│ Prerequisites:                          │
│ ✅ Clear Site                           │
│ ✅ Gather Materials                     │
│                                         │
│ Blocks: None                            │
│ Dependents: Install Door (queued)       │
└─────────────────────────────────────────┘
```

#### 4. TaskCreationWizard
**Component Name:** `TaskCreationWizard`

**Purpose:** Guide players through task creation process

**Steps:**
1. **Task Type Selection**
   - Choose category (construction, mining, crafting, etc.)
   - Select specific task type

2. **Location Selection** (for location-based tasks)
   - Interactive map selection
   - Coordinate input
   - Suitability check

3. **Configuration**
   - Set parameters (size, quantity, quality)
   - Choose materials/resources
   - Set priority level

4. **Worker Assignment**
   - Select number of workers
   - Choose specific workers or auto-assign
   - Review skill requirements

5. **Review and Confirm**
   - Summary of all settings
   - Resource availability check
   - Estimated completion time
   - Confirm or go back to edit

**Wizard Flow:**
```
Step 1: Task Type          Step 2: Location
┌──────────────────┐      ┌──────────────────┐
│ Select Type:     │  ➜   │ Select Location: │
│                  │      │                  │
│ ○ Construction   │      │   [Map View]     │
│ ● Mining         │      │                  │
│ ○ Crafting       │      │ Coordinates:     │
│ ○ Agriculture    │      │ X: [    ]        │
│                  │      │ Y: [    ]        │
│ [Next]  [Cancel] │      │ Z: [    ]        │
└──────────────────┘      │                  │
                          │ [Back]  [Next]   │
                          └──────────────────┘

Step 3: Configuration     Step 4: Workers
┌──────────────────┐      ┌──────────────────┐
│ Mining Depth:    │  ➜   │ Assign Workers:  │
│ [50] meters      │      │                  │
│                  │      │ Required: 2-4    │
│ Vein Type:       │      │ Available: 12    │
│ [Iron Ore ▼]     │      │                  │
│                  │      │ ☑️ Auto-assign   │
│ Priority:        │      │                  │
│ ● High           │      │ Or select:       │
│ ○ Medium         │      │ [ ] John         │
│ ○ Low            │      │ [ ] Maria        │
│                  │      │                  │
│ [Back]  [Next]   │      │ [Back]  [Next]   │
└──────────────────┘      └──────────────────┘

Step 5: Review
┌──────────────────┐
│ Confirm Task:    │
│                  │
│ Type: Mine Iron  │
│ Location: (...)  │
│ Depth: 50m       │
│ Workers: 3       │
│ Priority: High   │
│                  │
│ Resources Req:   │
│ ✅ Tools: 6      │
│ ✅ Support: 20   │
│                  │
│ ETA: 8 hours     │
│                  │
│ [Back] [Confirm] │
└──────────────────┘
```

#### 5. TaskQueueManager
**Component Name:** `TaskQueueManager`

**Purpose:** Manage task execution order and priorities

**Features:**
- Drag-and-drop task reordering
- Automatic priority-based sorting
- Queue visualization
- Dependency conflict detection
- Resource availability awareness

**Layout:**
```
┌─────────────────────────────────────────┐
│ Task Queue (8 tasks)                    │
├─────────────────────────────────────────┤
│ ⚙️ Sort: [Manual ▼]  📊 View: [List ▼] │
├─────────────────────────────────────────┤
│ ═ 1. Build Cottage (High)    [▲] [▼]   │
│ ═ 2. Mine Iron Ore (High)    [▲] [▼]   │
│ ═ 3. Craft Tools (Medium)    [▲] [▼]   │
│ ═ 4. Clear Forest (Low)      [▲] [▼]   │
│ ═ 5. Build Storage (Low)     [▲] [▼]   │
├─────────────────────────────────────────┤
│ Queued: 3 tasks waiting for resources   │
│ [Optimize Queue] [Clear Completed]      │
└─────────────────────────────────────────┘
```

#### 6. WorkerAssignmentPanel
**Component Name:** `WorkerAssignmentPanel`

**Purpose:** Manage worker allocation across all tasks

**Features:**
- View all workers and their current assignments
- Drag-and-drop worker reassignment
- Worker skill matching
- Efficiency calculations
- Fatigue/rest indicators

**Layout:**
```
┌─────────────────────────────────────────┐
│ Worker Assignments (12 workers)         │
├─────────────────────────────────────────┤
│ Worker       │ Task            │ Eff.  │
├──────────────┼─────────────────┼───────┤
│ 👷 John      │ Build Cottage   │ 95%   │
│ 👷 Maria     │ Build Cottage   │ 80%   │
│ 👷 Tom       │ Build Cottage   │ 75%   │
│ ⛏️ Sarah     │ Mine Iron Ore   │ 92%   │
│ ⛏️ Ahmed     │ Mine Iron Ore   │ 88%   │
│ 🔨 Lisa      │ Craft Tools     │ 85%   │
│ 💤 Pedro     │ (Resting)       │ --    │
│ ○ Anna       │ (Idle)          │ --    │
│ ○ Chen       │ (Idle)          │ --    │
├─────────────────────────────────────────┤
│ Idle: 3  Working: 6  Resting: 1  Total: 12 │
└─────────────────────────────────────────┘
```

## Functionality Specifications

### Task Operations

#### Create Task
**Trigger:** Click "New Task" button or use keyboard shortcut (Ctrl+N)
**Process:**
1. Open TaskCreationWizard
2. Guide user through configuration steps
3. Validate requirements and dependencies
4. Check resource availability
5. Create task in system
6. Add to queue
7. Display confirmation message

#### Edit Task
**Trigger:** Select task and click "Edit" or double-click task
**Process:**
1. Open task details in edit mode
2. Allow modification of:
   - Priority
   - Worker assignment
   - Parameters (where applicable)
3. Validate changes
4. Update task
5. Refresh display

#### Pause/Resume Task
**Trigger:** Click pause/resume button on task
**Process:**
1. Change task state
2. Deallocate/reallocate workers
3. Update queue
4. Log event
5. Update UI

#### Cancel Task
**Trigger:** Click cancel button with confirmation
**Process:**
1. Show confirmation dialog
2. If confirmed:
   - Stop task execution
   - Return workers to idle
   - Return consumed resources (if recoverable)
   - Update dependencies
   - Remove from queue
   - Log cancellation

### Filtering and Sorting

#### Filter Options
- **By Status:** All, Active, Queued, Paused, Blocked, Completed
- **By Type:** Construction, Mining, Crafting, Agriculture, Other
- **By Priority:** High, Medium, Low
- **By Worker:** Show tasks for specific worker
- **By Location:** Show tasks in specific area
- **By Time:** Created today, this week, etc.

#### Sort Options
- **Priority:** High to Low, Low to High
- **Progress:** Most complete, Least complete
- **Time:** Soonest completion, Latest completion
- **Creation Date:** Newest first, Oldest first
- **Alphabetical:** A-Z, Z-A
- **Manual:** Custom drag-and-drop order

### Real-time Updates

#### Update Triggers
- Task progress changes
- Worker assignment changes
- Resource availability changes
- Task status changes
- New task created
- Task completed
- Task failed/blocked

#### Update Behavior
- Smooth animations for progress bars
- Highlight changed items (fade effect)
- Toast notifications for important events
- Sound effects for completion (optional)
- Badge counts for different states

## User Interactions

### Keyboard Shortcuts
- `Ctrl+N`: Create new task
- `Ctrl+F`: Focus search box
- `Space`: Pause/Resume selected task
- `Delete`: Cancel selected task (with confirmation)
- `Ctrl+A`: Select all tasks
- `Escape`: Clear selection/Close dialogs
- `F5`: Refresh task list
- `Ctrl+↑/↓`: Move selected task in queue

### Mouse Interactions
- **Single Click:** Select task
- **Double Click:** Open task details
- **Right Click:** Show context menu
- **Drag:** Reorder tasks in queue
- **Drag Worker to Task:** Assign worker
- **Hover:** Show tooltip with quick info

### Touch Interactions (Mobile)
- **Tap:** Select task
- **Double Tap:** Open task details
- **Long Press:** Show context menu
- **Swipe Left:** Quick cancel (with undo)
- **Swipe Right:** Quick pause/resume
- **Pinch:** Zoom in/out on queue view

## Visual Design

### Color Coding

#### Task Status Colors
- **Active:** Green (#4CAF50)
- **Queued:** Blue (#2196F3)
- **Paused:** Orange (#FF9800)
- **Blocked:** Red (#F44336)
- **Completed:** Gray (#9E9E9E)

#### Priority Colors
- **High:** Red badge
- **Medium:** Yellow badge
- **Low:** Green badge

#### Resource Status Colors
- **Available:** Green checkmark
- **Partial:** Yellow warning
- **Missing:** Red X

### Icons
- 🏗️ Construction tasks
- ⛏️ Mining tasks
- 🔨 Crafting tasks
- 🌾 Agriculture tasks
- 👷 Workers
- ⭐ High priority
- ⚠️ Warning/blocked
- ✅ Complete/available
- ❌ Cancel/missing

### Typography
- **Task Names:** 14pt Bold
- **Status Text:** 12pt Regular
- **Details:** 11pt Regular
- **Headers:** 16pt Bold
- **Timestamps:** 10pt Regular

## Performance Requirements

### Response Time
- Task list load: <500ms for 100 tasks
- Filter/sort operation: <200ms
- Task creation: <1s
- Real-time update: <100ms latency

### Scalability
- Support up to 1000 tasks in queue
- Handle 100+ concurrent workers
- Maintain 60fps animations
- Efficient memory usage (<50MB)

### Reliability
- Auto-save task changes
- Recover from connection loss
- Handle concurrent modifications
- Validate all inputs

## Accessibility

### Screen Reader Support
- All interactive elements labeled
- Status announcements for changes
- Keyboard navigation fully supported
- ARIA landmarks properly set

### Visual Accessibility
- Minimum contrast ratio 4.5:1
- Color-blind friendly palette
- Resizable text (up to 200%)
- Focus indicators visible

### Motor Accessibility
- Large click targets (44x44px minimum)
- No time-sensitive actions
- Alternative to drag-and-drop
- Keyboard shortcuts for all actions

## Technical Specifications

### Component Technology
- Framework: React 18+
- State Management: Redux Toolkit
- UI Library: Material-UI v5
- Real-time: WebSocket connection
- Testing: Jest + React Testing Library

### API Integration
```typescript
interface TaskManagerAPI {
  // Task operations
  getTasks(filter?: TaskFilter): Promise<Task[]>
  createTask(task: TaskCreate): Promise<Task>
  updateTask(id: string, updates: TaskUpdate): Promise<Task>
  deleteTask(id: string): Promise<void>
  pauseTask(id: string): Promise<Task>
  resumeTask(id: string): Promise<Task>
  
  // Worker operations
  getWorkers(): Promise<Worker[]>
  assignWorker(taskId: string, workerId: string): Promise<void>
  unassignWorker(taskId: string, workerId: string): Promise<void>
  
  // Queue operations
  getQueue(): Promise<Task[]>
  reorderQueue(taskIds: string[]): Promise<void>
  optimizeQueue(): Promise<Task[]>
}
```

### Data Models
```typescript
interface Task {
  id: string
  name: string
  type: TaskType
  status: TaskStatus
  priority: Priority
  progress: number // 0-100
  createdAt: Date
  startedAt?: Date
  estimatedCompletion?: Date
  location?: Coordinate3D
  requirements: ResourceRequirement[]
  assignedWorkers: Worker[]
  dependencies: string[] // task IDs
  phases: TaskPhase[]
}

interface Worker {
  id: string
  name: string
  skills: Skill[]
  currentTask?: string
  efficiency: number // 0-100
  status: WorkerStatus // working, idle, resting
  fatigue: number // 0-100
}

interface TaskPhase {
  name: string
  progress: number
  subtasks: SubTask[]
  status: PhaseStatus
}
```

## Error Handling

### Error Scenarios
1. **Insufficient Resources**
   - Display clear message
   - Show what's missing
   - Suggest alternatives

2. **Worker Unavailable**
   - Indicate why worker can't be assigned
   - Show worker's current task
   - Suggest other workers

3. **Dependency Conflict**
   - Explain the conflict
   - Show affected tasks
   - Suggest resolution

4. **Network Error**
   - Show offline indicator
   - Queue changes locally
   - Sync when connection restored

### Error Messages
- Clear, non-technical language
- Specific problem description
- Actionable solutions
- No blame on user

## Future Enhancements

### Planned Features
1. **Task Templates**
   - Save common task configurations
   - Quick creation from templates

2. **Batch Operations**
   - Create multiple similar tasks
   - Bulk worker assignment

3. **Analytics Dashboard**
   - Task completion metrics
   - Worker efficiency trends
   - Resource usage analytics

4. **Smart Scheduling**
   - AI-powered task optimization
   - Automatic worker assignment
   - Resource availability prediction

5. **Mobile App**
   - Native iOS/Android apps
   - Offline support
   - Push notifications

## Related Documentation

- [QA Test Plan: Frontend Task Manager](qa-test-plan-frontend-task-manager.md)
- [Building and Construction Mechanics](../gameplay/mechanics/building-construction.md)
- [Mining Resource Extraction](../gameplay/mechanics/mining-resource-extraction.md)
- [UI Guidelines](ui-guidelines.md)

---

**Document Owner:** UI/UX Design Team  
**Last Updated:** 2025-11-09  
**Next Review:** 2025-12-09
