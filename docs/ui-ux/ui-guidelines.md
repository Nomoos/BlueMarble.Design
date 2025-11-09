# BlueMarble - UI/UX Design Guidelines

**Version:** 1.0  
**Date:** 2025-09-29  
**Author:** BlueMarble UX Team

## Design Philosophy

BlueMarble's user interface prioritizes clarity, accessibility, and immersion. The UI should enhance the gaming experience without overwhelming players or obscuring the beautiful game world.

### Core Principles

#### Clarity First
- **Information Hierarchy:** Important information is prominently displayed
- **Visual Consistency:** Similar functions use similar visual treatments
- **Readable Typography:** Text is legible at all supported resolutions
- **Intuitive Icons:** Symbols convey meaning without requiring explanation

#### Accessibility
- **Color Accessibility:** Interface works for colorblind users
- **Text Scaling:** Support for different text sizes
- **High Contrast:** Alternative high-contrast mode available
- **Keyboard Navigation:** Full keyboard support for all functions

#### Immersion
- **Minimal Intrusion:** UI elements don't obstruct gameplay unnecessarily
- **Contextual Display:** Information appears when relevant
- **Visual Integration:** UI aesthetic matches game art style
- **Smooth Transitions:** Animations enhance rather than distract

## Visual Design Language

### Color Palette

#### Primary Colors
- **Deep Blue (#1E3A8A):** Primary brand color for headers and accents
- **Golden Yellow (#F59E0B):** Important actions and highlights
- **Forest Green (#065F46):** Positive actions, confirmations, success states
- **Warm Red (#DC2626):** Warnings, errors, and destructive actions

#### Neutral Colors
- **Charcoal (#374151):** Primary text and interface elements
- **Light Gray (#F3F4F6):** Background areas and subtle elements
- **Medium Gray (#9CA3AF):** Secondary text and inactive elements
- **Pure White (#FFFFFF):** High contrast text and highlights

#### Functional Colors
- **Health Red (#EF4444):** Health bars and damage indicators
- **Mana Blue (#3B82F6):** Mana bars and magical effects
- **Experience Purple (#8B5CF6):** Experience bars and progression
- **Stamina Orange (#F97316):** Stamina bars and physical abilities

### Typography

#### Font Hierarchy
- **Primary Font:** Custom fantasy-themed font for headings and titles
- **Secondary Font:** Clean sans-serif for body text and UI labels
- **Monospace Font:** Fixed-width font for numbers and data displays

#### Text Sizes
- **H1 Headers:** 24px - Major section titles
- **H2 Headers:** 20px - Subsection titles
- **H3 Headers:** 18px - Component titles
- **Body Text:** 16px - Standard interface text
- **Small Text:** 14px - Secondary information
- **Tiny Text:** 12px - Tooltips and fine print

### Visual Elements

#### Buttons
- **Primary Buttons:** Bold, high-contrast for main actions
- **Secondary Buttons:** Subtle styling for supporting actions
- **Icon Buttons:** Square buttons with symbolic icons
- **Text Links:** Underlined text for navigation links

#### Input Fields
- **Text Inputs:** Clear borders with focus states
- **Dropdowns:** Expandable menus with scrolling support
- **Checkboxes:** Square selection boxes with clear states
- **Radio Buttons:** Circular selection for exclusive choices

#### Containers
- **Panels:** Rounded corner containers for grouped content
- **Windows:** Draggable interfaces with title bars
- **Tooltips:** Contextual information overlays
- **Modals:** Full-screen overlays for important interactions

## Layout Principles

### Screen Organization

#### Grid System
- **12-Column Grid:** Flexible layout system for different screen sizes
- **Responsive Breakpoints:** Tablet and mobile-friendly adaptations
- **Consistent Spacing:** 8px base unit for margins and padding
- **Alignment:** Left-aligned text, centered important elements

#### Information Architecture
```
[Top Bar: Navigation, Chat, System]
[Left Panel: Character, Inventory, Skills]
[Center: Game World View]
[Right Panel: Map, Quest Log, Social]
[Bottom Bar: Abilities, Chat Input]
```

### Component Spacing
- **Tight Spacing:** 4px between related elements
- **Standard Spacing:** 8px between component groups
- **Loose Spacing:** 16px between major sections
- **Section Breaks:** 24px between distinct content areas

## Interface Components

### HUD Elements

#### Health and Resource Bars
- **Position:** Top-left corner, always visible
- **Visual Design:** Gradient fills with smooth animations
- **Information:** Current/max values displayed as text
- **Color Coding:** Red for health, blue for mana, orange for stamina

#### Minimap
- **Position:** Top-right corner
- **Functionality:** Player position, nearby players, objectives
- **Interaction:** Click to open full map
- **Customization:** Toggle different information layers

#### Chat Interface
- **Position:** Bottom-left, expandable
- **Multiple Channels:** Tabbed interface for different chat types
- **Input Field:** Expandable text input with autocomplete
- **History:** Scrollable message history with timestamps

#### Action Bars
- **Position:** Bottom center of screen
- **Customization:** Drag-and-drop ability assignment
- **Visual Feedback:** Cooldown animations and availability states
- **Hotkeys:** Keyboard shortcuts displayed on abilities

### Windows and Panels

#### Character Sheet
- **Sections:** Stats, equipment, skills, achievements
- **Equipment Viewer:** Visual representation of equipped items
- **Stat Display:** Primary and secondary attributes with explanations
- **Progression Tracking:** Experience bars and level information

#### Inventory
- **Grid Layout:** Sortable item grid with categories
- **Item Information:** Detailed tooltips on hover
- **Quick Actions:** Right-click context menus
- **Search Function:** Filter items by name or type

#### Quest Log
- **Quest Categories:** Active, completed, available quests
- **Progress Tracking:** Objective completion status
- **Map Integration:** Click to show quest locations
- **Reward Preview:** Show quest rewards before acceptance

#### Social Panel
- **Friends List:** Online status and quick communication
- **Guild Interface:** Member list, guild bank, events
- **Group Management:** Party invites and composition
- **LFG System:** Looking for group matchmaking

### Menu Systems

#### Main Menu
- **Background:** Atmospheric game world scene
- **Options:** Continue, New Character, Settings, Exit
- **News Integration:** Latest game updates and events
- **Account Info:** Player name and subscription status

#### Settings Menu
- **Categories:** Graphics, Audio, Controls, Gameplay
- **Graphics Options:** Resolution, quality presets, advanced settings
- **Audio Controls:** Master volume, music, effects, voice
- **Control Mapping:** Customizable key bindings
- **Accessibility:** Color blind options, text scaling, contrast

## Interaction Design

### Navigation Patterns

#### Primary Navigation
- **Tab System:** Switch between major interface sections
- **Breadcrumbs:** Show current location in complex menus
- **Back Button:** Return to previous interface state
- **Shortcuts:** Quick access to frequently used functions

#### Context Menus
- **Right-Click Menus:** Context-sensitive action lists
- **Item Actions:** Use, trade, examine, destroy options
- **Player Actions:** Invite, trade, whisper, ignore options
- **World Interactions:** Harvest, examine, interact options

### Feedback Systems

#### Visual Feedback
- **Hover States:** Subtle highlighting on interactive elements
- **Click Feedback:** Brief animation or color change on activation
- **Loading States:** Progress indicators for longer operations
- **Status Indicators:** Icons showing current character states

#### Audio Feedback
- **Button Sounds:** Subtle audio cues for interactions
- **Notification Sounds:** Alerts for important events
- **Error Sounds:** Audio feedback for invalid actions
- **Success Sounds:** Positive reinforcement for achievements

### Error Handling

#### Error Messages
- **Clear Language:** Explain what went wrong in simple terms
- **Actionable Solutions:** Tell users how to fix the problem
- **Non-Blocking:** Don't halt gameplay for minor issues
- **Dismissible:** Allow users to close error messages

#### Validation
- **Real-time Validation:** Immediate feedback for form inputs
- **Character Limits:** Show remaining characters for text fields
- **Invalid States:** Clear indication of what needs to be fixed
- **Confirmation Dialogs:** Prevent accidental destructive actions

## Responsive Design

### Multi-Platform Considerations

#### Desktop (Primary)
- **Resolution:** 1920x1080 minimum, 4K support
- **Input:** Mouse and keyboard optimization
- **Screen Real Estate:** Maximum information density
- **Window Management:** Multiple resizable interface panels

#### Tablet (Future)
- **Touch Interface:** Larger buttons and touch-friendly controls
- **Simplified Layout:** Reduced complexity for smaller screens
- **Gesture Support:** Swipe and pinch interactions
- **Portrait/Landscape:** Adaptive layouts for orientation

#### Mobile (Future)
- **Minimal Interface:** Essential information only
- **Large Touch Targets:** Finger-friendly button sizes
- **Simplified Navigation:** Reduced menu complexity
- **Battery Optimization:** Efficient rendering and animations

### Scaling Strategies
- **UI Scaling:** Percentage-based scaling for different resolutions
- **Dynamic Layout:** Adaptive component positioning
- **Font Scaling:** Maintain readability across screen sizes
- **Icon Scaling:** Vector graphics for crisp rendering

## Accessibility Standards

> **📖 Comprehensive Guide:** See [Accessibility Guidelines](accessibility-guidelines.md) for complete accessibility standards, implementation details, and WCAG 2.1 AA compliance requirements.

### Visual Accessibility

#### Color and Contrast
- **Text Contrast:** Minimum 4.5:1 ratio for normal text, 3:1 for large text (WCAG AA)
- **UI Component Contrast:** Minimum 3:1 for interactive elements and graphics
- **Color Independence:** Never use color alone to convey information
  - Always combine color with icons, text, or patterns
  - Example: Status indicators use ✓/⚠/✗ + color + text
- **Colorblind Support:** 
  - Test all interfaces with colorblind simulation tools
  - Provide alternative colorblind-friendly palettes
  - Support for protanopia, deuteranopia, tritanopia, and monochromacy

#### Typography and Text
- **Font Scaling:** Support 100% to 200% text scaling without loss of functionality
- **Legible Fonts:** High readability at all sizes with clear character distinction
- **Line Height:** Minimum 1.5x font size for body text
- **Text Spacing:** Adjustable letter spacing (0.12x) and word spacing (0.16x)

#### Visual Feedback
- **Focus Indicators:** 
  - Minimum 2px outline thickness
  - High contrast (3:1 minimum against background)
  - Visible on all interactive elements
  - Non-reliant on color alone
- **Hover States:** Clear visual change on interactive elements
- **Loading States:** Visible progress indicators for operations
- **Status Indicators:** Use icon + color + text for all states

#### Motion and Animations
- **Motion Reduction:** Respect `prefers-reduced-motion` user preference
- **Animation Control:** Toggle to disable non-essential animations
- **No Flashing:** Avoid content flashing more than 3 times per second
- **Smooth Transitions:** Gentle, predictable animations (or none if reduced motion enabled)

### Motor Accessibility

#### Input Targets
- **Touch Target Size:** Minimum 44x44 pixels (WCAG AAA standard)
- **Target Spacing:** Minimum 8px between adjacent interactive elements
- **Large Targets for Critical Actions:** 60x60 pixels for primary buttons
- **Forgiving Hit Areas:** Generous clickable regions, especially for small UI elements

#### Keyboard Navigation
- **Full Keyboard Support:** All functionality accessible via keyboard alone
- **Logical Tab Order:** Follow visual layout and user expectations
- **Skip Links:** Bypass repeated content blocks
- **No Keyboard Traps:** Users can navigate away from all components
- **Visible Focus:** Clear focus indicator at all times
- **Standard Shortcuts:** Support common keyboard conventions (Ctrl+C, Ctrl+V, etc.)

#### Drag and Drop
- **Keyboard Alternatives:** Provide non-drag methods for all drag operations
- **Large Drop Zones:** Minimum 60x60 pixel drop targets
- **Magnetic Assistance:** Optional snap-to-grid or magnetic drop zones
- **Clear Feedback:** Visual indicators during drag operations
- **Undo Support:** Easy recovery from drag-drop mistakes

#### Timing and Time Limits
- **No Time Pressure:** Avoid strict time limits for core gameplay activities
- **Pause Functionality:** Allow pausing in all scenarios
- **Adjustable Limits:** Option to extend or disable time limits
- **Advance Warning:** Alert users before time-sensitive events

### Cognitive Accessibility

#### Clear Communication
- **Plain Language:** Simple, direct communication avoiding jargon
- **Short Sentences:** Concise text with clear structure
- **Consistent Terminology:** Use same words for same concepts throughout
- **Active Voice:** Prefer active over passive constructions
- **Glossary Support:** Define technical terms when necessary

#### Information Architecture
- **Predictable Navigation:** Consistent menu locations and behaviors
- **Clear Hierarchy:** Logical organization of information
- **Breadcrumbs:** Show current location in complex menu structures
- **Familiar Patterns:** Use well-known UI conventions

#### Error Handling
- **Clear Error Messages:** Explain what went wrong in plain language
- **Actionable Solutions:** Tell users how to fix problems
- **Helpful Validation:** Real-time feedback for form inputs
- **Error Prevention:** Design to minimize user mistakes
- **Recovery Options:** Easy undo and correction capabilities

#### Cognitive Load Management
- **Progressive Disclosure:** Show essential information first, details on demand
- **Chunking:** Break complex tasks into manageable steps
- **Focus Mode:** Option to dim non-essential UI elements
- **Context Preservation:** Save user's place when switching tasks
- **Memory Aids:** History, favorites, and recently used items

### Auditory Accessibility

#### Visual Alternatives
- **Sound Indicators:** Visual representation of all audio cues
- **Captions and Subtitles:** Text alternatives for voice content
- **Flash/Vibration:** Alternative alert methods for sound notifications
- **Sound Legend:** Documentation of what each sound means

#### Audio Controls
- **Independent Volume:** Separate controls for music, effects, voice, master
- **Mono Audio:** Option for mono sound output
- **Visual Sound Indicators:** Toggle for on-screen sound effect indicators

### Screen Reader Support

#### Semantic Structure
- **Semantic HTML:** Use proper HTML5 elements (nav, main, section, etc.)
- **ARIA Labels:** Provide accessible names for complex components
- **Heading Hierarchy:** Logical structure (H1 → H2 → H3)
- **Live Regions:** ARIA live for dynamic content updates
- **Alternative Text:** Descriptive alt text for informative images

### Game-Specific Accessibility

#### Routine-Based Gameplay
- **Reduced Motor Requirements:** Routine system minimizes cursor precision needs
- **No Time Pressure:** Plan activities without real-time pressure
- **Strategic Over Reflexive:** Rewards planning and knowledge over twitch skills
- **Template System:** Pre-built routines reduce setup complexity

#### Aiming and Precision Assistance
- **Multiple Assist Levels:** From subtle help to full auto-aim
  - Level 1: Target highlighting (visual aid only)
  - Level 2: Sticky targeting (slight magnetism)
  - Level 3: Aim slowdown (easier fine adjustment)
  - Level 4: Snap-to-target (removes spatial requirement)
  - Level 5: Auto-aim (full automation)
- **Minimal Penalties:** Small reward reduction for using assists (5-20%)
- **Player Choice:** Assistants are optional and customizable

#### Multiple Progression Paths
- **Valid Alternatives:** Never require specific motor skills for core progression
- **Varied Playstyles:** Support real-time action, planning, management, social approaches
- **Delegation Options:** Hire workers/NPCs for activities requiring precision
- **Technology Progression:** Better tools reduce skill requirements

### Accessibility Testing Requirements

#### Automated Testing
- **WCAG Compliance:** Run Lighthouse, axe DevTools, Pa11y
- **Color Contrast:** Verify all text meets 4.5:1 minimum ratio
- **Keyboard Navigation:** Test all functionality keyboard-only
- **Screen Reader:** Test with NVDA, JAWS, or VoiceOver

#### Manual Testing
- **Diverse User Groups:** Include users with various disabilities
- **Assistive Technology:** Test with screen readers, alternative input devices
- **Multiple Scenarios:** Test common user journeys
- **Feedback Collection:** Document pain points and barriers

#### Compliance Checklist
- ☐ WCAG 2.1 Level AA compliance
- ☐ All text meets contrast requirements
- ☐ Color not sole information carrier
- ☐ All targets meet size requirements
- ☐ Full keyboard navigation
- ☐ Visible focus indicators
- ☐ Screen reader compatible
- ☐ Motion reduction supported
- ☐ Multiple input methods supported
- ☐ Clear error messages and help

## Performance Guidelines

### Rendering Optimization
- **UI Draw Calls:** Minimize texture changes and draw calls
- **Animation Performance:** Use hardware acceleration when possible
- **Memory Management:** Efficient texture and object pooling
- **Frame Rate:** Maintain 60fps for UI animations

### Loading Optimization
- **Progressive Loading:** Show content as it becomes available
- **Perceived Performance:** Use skeleton screens and progress indicators
- **Caching Strategy:** Cache frequently accessed UI elements
- **Asset Optimization:** Compressed textures and efficient formats

## Testing and Validation

### Usability Testing
- **User Testing:** Regular sessions with target audience
- **A/B Testing:** Compare interface variations
- **Analytics:** Track user behavior and pain points
- **Feedback Collection:** In-game feedback tools

### Quality Assurance
- **Cross-Platform Testing:** Verify functionality across platforms
- **Accessibility Testing:** Automated and manual accessibility checks
- **Performance Testing:** Load testing for UI responsiveness
- **Regression Testing:** Ensure new changes don't break existing functionality

---

*These UI/UX guidelines should be regularly updated based on user feedback and evolving accessibility standards. The goal is to create an interface that serves all players effectively.*