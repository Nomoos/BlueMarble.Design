# BlueMarble - Accessibility Guidelines

**Version:** 1.0  
**Date:** 2025-11-09  
**Author:** BlueMarble UX Team  
**Status:** Active

## Executive Summary

BlueMarble is committed to creating an inclusive gaming experience accessible to all players, regardless of physical, cognitive, or sensory abilities. These guidelines ensure compliance with WCAG 2.1 Level AA standards while addressing game-specific accessibility needs identified through research on gender differences, motor control challenges, and cognitive load management.

### Core Accessibility Principles

1. **Perceivable** - Information and UI components must be presentable to users in ways they can perceive
2. **Operable** - UI components and navigation must be operable by all users
3. **Understandable** - Information and UI operation must be understandable
4. **Robust** - Content must be robust enough to work with current and future technologies

## Visual Accessibility

### Color and Contrast

#### WCAG 2.1 AA Compliance

**Text Contrast Requirements:**
- Normal text (< 18pt): Minimum 4.5:1 contrast ratio
- Large text (≥ 18pt or 14pt bold): Minimum 3:1 contrast ratio
- UI components and graphical objects: Minimum 3:1 contrast ratio
- Inactive/disabled elements: No minimum (but should be distinguishable)

**Implementation:**
```css
/* Primary text on light background */
color: #374151; /* Charcoal */
background: #FFFFFF; /* White */
/* Contrast ratio: 10.1:1 ✓ */

/* Secondary text on light background */
color: #9CA3AF; /* Medium Gray */
background: #FFFFFF; /* White */
/* Contrast ratio: 3.8:1 ✓ */

/* Danger/Warning text */
color: #DC2626; /* Warm Red */
background: #FFFFFF; /* White */
/* Contrast ratio: 5.9:1 ✓ */
```

#### Color Independence

**Critical Guideline:** Never rely on color alone to convey information.

**Required Implementation:**
- Use color + icon + text for all status indicators
- Provide patterns or textures in addition to color coding
- Include text labels for all color-coded elements
- Offer multiple ways to distinguish information

**Example - Resource Bars:**
```
❌ Bad:
[████████░░] (Red bar only)

✓ Good:
Health: [❤️████████░░] 80/100
Mana:   [💧████░░░░░░] 40/100
```

#### Colorblind Modes

**Supported Modes:**
- Protanopia (red-blind)
- Deuteranopia (green-blind)
- Tritanopia (blue-blind)
- Monochromacy (total color blindness)

**Implementation Strategy:**
- Provide alternative color palettes for each mode
- Test all UI elements with colorblind simulation tools
- Use patterns and shapes in addition to colors
- Ensure critical information uses high-contrast combinations

**Colorblind-Friendly Palette:**
```
Standard    Protanopia  Deuteranopia  Tritanopia
──────────  ──────────  ────────────  ──────────
#DC2626     #2563EB     #2563EB       #DC2626
#10B981     #F59E0B     #F59E0B       #10B981
#3B82F6     #DC2626     #DC2626       #F59E0B
```

### Text and Typography

#### Font Scaling

**Requirements:**
- Support text scaling from 100% to 200%
- Maintain layout integrity at all scaling levels
- Use relative units (em, rem) instead of absolute pixels
- Ensure no horizontal scrolling required at 200% zoom

**Implementation:**
```css
/* Base font size */
html {
  font-size: 16px; /* User can override */
}

/* Component text using relative units */
.ui-label {
  font-size: 1rem; /* 16px at base */
}

.ui-header {
  font-size: 1.5rem; /* 24px at base */
}

.ui-small {
  font-size: 0.875rem; /* 14px at base */
}
```

#### Font Selection

**Criteria:**
- High legibility at small sizes
- Clear distinction between similar characters (I, l, 1, O, 0)
- Support for extended character sets
- Optimized for screen rendering

**Recommended Fonts:**
- **Headings:** Roboto, Open Sans, Lato
- **Body Text:** Inter, Source Sans Pro, Noto Sans
- **Monospace:** Roboto Mono, Source Code Pro, JetBrains Mono

#### Line Height and Spacing

**Standards:**
- Line height: Minimum 1.5x font size for body text
- Paragraph spacing: Minimum 2x font size
- Letter spacing: Minimum 0.12x font size (adjustable)
- Word spacing: Minimum 0.16x font size (adjustable)

### Visual Indicators

#### Focus Indicators

**Requirements:**
- Visible focus indicator on all interactive elements
- Minimum 2px outline thickness
- High contrast ratio (minimum 3:1 against background)
- Non-reliant on color alone

**Implementation:**
```css
/* Keyboard focus indicator */
*:focus-visible {
  outline: 2px solid #3B82F6; /* Blue outline */
  outline-offset: 2px;
  box-shadow: 0 0 0 4px rgba(59, 130, 246, 0.2); /* Glow effect */
}

/* High contrast mode */
@media (prefers-contrast: high) {
  *:focus-visible {
    outline: 3px solid currentColor;
    outline-offset: 3px;
  }
}
```

#### Visual Feedback

**Required for:**
- Hover states on interactive elements
- Active/pressed states on buttons
- Loading/processing states
- Success/error/warning states
- Selection states

**Implementation Patterns:**
```css
/* Hover state */
.button:hover {
  background-color: #2563EB;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

/* Active state */
.button:active {
  transform: translateY(0);
  box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.1);
}

/* Loading state */
.button.loading {
  opacity: 0.6;
  cursor: wait;
  pointer-events: none;
}

.button.loading::after {
  content: "";
  animation: spin 1s linear infinite;
}
```

### Animations and Motion

#### Motion Reduction

**Respect User Preferences:**
```css
/* Reduced motion media query */
@media (prefers-reduced-motion: reduce) {
  * {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}
```

**Implementation Guidelines:**
- Provide toggle for disabling animations in settings
- Essential animations (loading indicators) should be simple
- Avoid parallax effects, excessive zoom, or rapid movements
- No flashing content faster than 3 times per second

## Motor Accessibility

### Input Target Sizes

#### Touch and Click Targets

**WCAG AAA Standard:**
- Minimum target size: 44x44 pixels (CSS pixels)
- Minimum spacing between targets: 8 pixels
- Larger targets preferred for critical actions: 60x60 pixels

**Implementation:**
```css
/* Standard interactive element */
.button, .link, .input {
  min-width: 44px;
  min-height: 44px;
  padding: 12px 16px;
}

/* Large touch target for primary actions */
.button-primary {
  min-width: 60px;
  min-height: 60px;
  padding: 16px 24px;
}

/* Ensure spacing between targets */
.button-group > * {
  margin: 8px;
}
```

#### Drag and Drop

**Accessibility Requirements:**
- Provide keyboard alternatives to all drag-drop operations
- Large, forgiving drop zones (minimum 60x60 pixels)
- Clear visual feedback during drag operations
- Snap-to-grid or magnetic assistance optional
- Undo capability for all drag-drop actions

**Alternative Methods:**
- Context menus for move/copy operations
- Keyboard shortcuts (Cut/Copy/Paste)
- Buttons for reordering items
- Form inputs for precise positioning

### Keyboard Navigation

#### Full Keyboard Support

**Requirements:**
- All functionality accessible via keyboard alone
- Logical tab order following visual layout
- Skip links for bypassing repeated content
- Visible focus indicators at all times
- No keyboard traps

**Navigation Patterns:**
```
Tab             - Move to next focusable element
Shift+Tab       - Move to previous focusable element
Enter/Space     - Activate buttons and controls
Arrow Keys      - Navigate within components (lists, menus)
Escape          - Close dialogs, cancel operations
Home/End        - Jump to beginning/end of lists
Page Up/Down    - Scroll content areas
```

**Implementation:**
```html
<!-- Skip link for keyboard users -->
<a href="#main-content" class="skip-link">Skip to main content</a>

<!-- Tab order and ARIA labels -->
<nav aria-label="Main navigation" role="navigation">
  <button tabindex="0" aria-label="Open menu">☰ Menu</button>
  <button tabindex="0" aria-label="Character">👤</button>
  <button tabindex="0" aria-label="Inventory">🎒</button>
</nav>
```

#### Keyboard Shortcuts

**Design Principles:**
- Use standard conventions (Ctrl+C, Ctrl+V, etc.)
- Provide customizable key bindings
- Display shortcuts in tooltips and help
- Avoid single-character shortcuts (conflicts with input fields)
- Use modifier keys (Ctrl, Alt, Shift) for custom shortcuts

**Recommended Shortcuts:**
```
I           - Open Inventory
C           - Open Character
M           - Open Map
K           - Open Skills
Q           - Open Quests
G           - Open Guild
Escape      - Close current window/menu
F1          - Open Help
F11         - Toggle Fullscreen
```

### Timing and Time Limits

#### No Time Pressure

**Critical for Accessibility:**
- No required actions under strict time limits for core gameplay
- Provide pause functionality in all scenarios
- Allow users to extend time limits or disable them
- Warning before time-sensitive events

**Routine System Benefits:**
- Players can plan actions without time pressure
- Automation reduces need for rapid responses
- Strategic gameplay over twitch reflexes
- Accessible to players with motor limitations

### Alternative Input Methods

#### Controller Support

**Requirements:**
- Full D-pad/analog stick navigation
- Button remapping capabilities
- Radial menus for complex selections
- Hold-to-confirm for destructive actions
- On-screen button prompts

**Adaptive Controllers:**
- Support for Xbox Adaptive Controller
- Support for custom button mappings
- One-handed play mode options
- Adjustable input sensitivity

## Cognitive Accessibility

### Clear Communication

#### Language and Instructions

**Guidelines:**
- Use plain, simple language
- Avoid jargon and technical terms (or provide glossary)
- Short sentences and paragraphs
- Active voice over passive voice
- Consistent terminology throughout

**Example:**
```
❌ Complex:
"Configure your automated resource extraction routine's 
conditional logic parameters to optimize yield efficiency 
relative to quality thresholds."

✓ Simple:
"Set up rules for automatic mining:
1. Choose when to mine (time or conditions)
2. Set minimum quality to accept
3. Choose how much to gather"
```

#### Error Messages

**Effective Error Communication:**
- Explain what went wrong in plain language
- Provide specific, actionable solutions
- Use consistent error formatting
- Don't blame the user
- Include error recovery options

**Example:**
```
❌ Bad:
"Error 0x8472: Operation failed"

✓ Good:
"Cannot save your routine"
"Reason: Not enough storage space"
[Options:]
- Delete old routines to make space
- Purchase more storage
- Save to different location
[Learn more about storage]
```

### Information Architecture

#### Consistent Navigation

**Requirements:**
- Predictable menu locations
- Consistent icon usage
- Breadcrumb navigation for complex menus
- Clear hierarchy of information
- Familiar design patterns

**Navigation Structure:**
```
Main HUD (Always visible)
├── Character (Top-left)
├── Inventory (Left panel)
├── Map (Top-right)
├── Quests (Right panel)
└── Chat (Bottom-left)

Submenus (Consistent pattern)
├── Skills → Character submenu
├── Equipment → Character submenu
├── Crafting → Inventory submenu
└── Filters → Inventory submenu
```

#### Progressive Disclosure

**Reduce Cognitive Load:**
- Show essential information first
- Provide "More details" options
- Use collapsible sections
- Implement tooltips for advanced features
- Template system for complex tasks

**Routine Builder Example:**
```
Simple Mode (Default)
┌─────────────────────────┐
│ Choose routine type:    │
│ ○ Daily Mining          │
│ ○ Market Trading        │
│ ○ Skill Training        │
│ [Create Routine]        │
│ [Advanced Mode]         │
└─────────────────────────┘

Advanced Mode (Optional)
┌─────────────────────────┐
│ Routine Configuration:  │
│ Triggers: [+]           │
│ Actions: [+]            │
│ Conditions: [+]         │
│ Variables: [+]          │
│ [Simple Mode] [Save]    │
└─────────────────────────┘
```

### Memory and Attention Support

#### Context Preservation

**Implementation:**
- Save user's place when switching contexts
- Restore previous state when returning
- Highlight what's changed since last visit
- Provide history/undo functionality

#### Attention Management

**Reduce Distractions:**
- Focus mode to dim non-essential UI
- Customizable notification levels
- Quiet hours for non-urgent alerts
- Priority indicators for important information

**Example Settings:**
```
Notification Settings:
☑️ Critical alerts only (combat, death)
☑️ Important events (quest completion)
☐ Social notifications (friend online)
☐ Market updates (price changes)
☐ Guild messages

Focus Mode:
☑️ Enable during combat
☑️ Enable during crafting
☐ Enable during routine editing
Dim level: [━━━━●━━━] 50%
```

## Auditory Accessibility

### Visual Alternatives to Sound

**Requirements:**
- Visual indicators for all audio cues
- Closed captions for voice content
- Text transcripts for audio-only content
- Flash/vibration options for alerts

**Implementation:**
```
Sound Effect         Visual Indicator
──────────────────  ────────────────────────
Quest Complete      ✓ Screen border flash
Level Up            ⬆️ Rising text animation
Low Health          ❤️ Pulsing health bar
Message Received    💬 Chat icon badge
Resource Ready      📦 Notification banner
```

### Audio Settings

**Customization Options:**
- Independent volume controls (master, music, effects, voice)
- Mono audio option
- Subtitles/captions toggle
- Visual sound indicators toggle
- Audio description for cinematics (future)

## Screen Reader Support

### Semantic HTML and ARIA

**Requirements:**
- Use semantic HTML elements
- Provide ARIA labels for complex components
- ARIA live regions for dynamic content
- Proper heading hierarchy (H1→H2→H3)

**Implementation:**
```html
<!-- Semantic structure -->
<main id="main-content" role="main">
  <section aria-labelledby="inventory-heading">
    <h1 id="inventory-heading">Inventory</h1>
    
    <!-- Interactive elements with ARIA -->
    <button 
      aria-label="Use health potion"
      aria-describedby="potion-description">
      Use Potion
    </button>
    <span id="potion-description" class="sr-only">
      Restores 50 health points. 3 remaining in inventory.
    </span>
  </section>
  
  <!-- Live region for updates -->
  <div aria-live="polite" aria-atomic="true" class="sr-only">
    <span id="status-message"></span>
  </div>
</main>

<!-- Screen reader only text -->
<style>
.sr-only {
  position: absolute;
  width: 1px;
  height: 1px;
  padding: 0;
  margin: -1px;
  overflow: hidden;
  clip: rect(0, 0, 0, 0);
  white-space: nowrap;
  border-width: 0;
}
</style>
```

### Alternative Text

**Requirements:**
- Descriptive alt text for informative images
- Empty alt="" for decorative images
- Text alternatives for icons with meaning
- Complex image descriptions via aria-describedby

**Examples:**
```html
<!-- Informative image -->
<img src="iron-ore.png" alt="Iron ore resource icon">

<!-- Decorative image -->
<img src="border-decoration.png" alt="">

<!-- Icon button -->
<button aria-label="Close inventory window">
  <img src="close-icon.png" alt="">
</button>

<!-- Complex diagram -->
<figure aria-describedby="map-description">
  <img src="world-map.png" alt="World map showing player location">
  <figcaption id="map-description">
    Interactive map centered on coordinates 125.345°E, 23.567°N.
    Shows tropical rainforest biome with nearby settlements marked.
  </figcaption>
</figure>
```

## Game-Specific Accessibility Features

### Routine-Based Gameplay

**Accessibility Advantages:**
- Reduces motor skill requirements
- Eliminates time pressure
- Allows strategic planning over reflexes
- Supports offline progression
- Accommodates varied play schedules

**Implementation Guidelines:**
- Large, forgiving UI for routine builder
- Template system for common routines
- Plain language descriptions
- Visual workflow representation
- Community sharing of accessible routines

### Aiming and Precision Tasks

**Assist Options:**
Based on research in cursor accuracy and gender differences:

**Level 1: Target Highlighting**
- Visual indicators for aimable targets
- No mechanical assistance
- Reduces visual search time

**Level 2: Sticky Targeting**
- Slight cursor magnetism near targets
- 10-15% easier to hit
- Reduces fine motor requirements

**Level 3: Aim Slowdown**
- Cursor slows when over target
- Easier fine adjustments
- Player maintains full control

**Level 4: Snap-to-Target**
- Button press snaps to nearest target
- Player times the action
- Removes spatial aiming requirement

**Level 5: Auto-Aim**
- System handles all aiming
- Player initiates action only
- Fully accessible option

**Implementation:**
```javascript
class AimingAssistSystem {
  constructor(playerPreferences) {
    this.assistLevel = playerPreferences.aimAssistLevel;
    this.showAssistIndicator = playerPreferences.showAssistIndicator;
  }
  
  updateCursor(cursorPos, availableTargets) {
    switch(this.assistLevel) {
      case AssistLevel.None:
        return cursorPos; // No modification
        
      case AssistLevel.Sticky:
        return this.applyStickyTargeting(cursorPos, availableTargets);
        
      case AssistLevel.Slowdown:
        this.adjustCursorSpeed(cursorPos, availableTargets);
        return cursorPos;
        
      case AssistLevel.SnapTo:
        return this.snapToNearestTarget(cursorPos, availableTargets);
        
      case AssistLevel.Auto:
        return this.selectBestTarget(availableTargets);
    }
  }
  
  // Minimal reward penalty for using assists
  getRewardMultiplier() {
    const penalties = {
      [AssistLevel.None]: 1.0,
      [AssistLevel.Sticky]: 0.95,
      [AssistLevel.Slowdown]: 0.95,
      [AssistLevel.SnapTo]: 0.90,
      [AssistLevel.Auto]: 0.80
    };
    return penalties[this.assistLevel] || 1.0;
  }
}
```

### Multiple Valid Paths

**Design Principle:**
- Never require specific motor skills for core progression
- Provide alternative methods for all activities
- Validate all playstyles equally

**Examples:**
```
Resource Extraction Options:
1. Real-time aiming (for skilled players)
   - Manual control, highest potential yield
   - Bonus for precision
   
2. Scheduled routines (planning-based)
   - Automated execution, reliable yield
   - No time pressure
   
3. Area selection (simplified)
   - Mark region, automatic extraction
   - Minimal interaction required
   
4. Delegation (management)
   - Hire workers, assign tasks
   - Strategic oversight
```

## Testing and Validation

### Automated Testing

**Tools and Checks:**
- Lighthouse accessibility audit
- axe DevTools for WCAG compliance
- Pa11y for continuous integration
- Color contrast analyzers
- Keyboard navigation testing

**Required Tests:**
```javascript
// Example automated accessibility tests
describe('Accessibility Tests', () => {
  test('All interactive elements have accessible names', () => {
    const buttons = document.querySelectorAll('button');
    buttons.forEach(button => {
      expect(
        button.getAttribute('aria-label') ||
        button.textContent.trim()
      ).toBeTruthy();
    });
  });
  
  test('Color contrast meets WCAG AA standards', () => {
    const textElements = document.querySelectorAll('p, span, label');
    textElements.forEach(element => {
      const contrast = getContrastRatio(element);
      expect(contrast).toBeGreaterThanOrEqual(4.5);
    });
  });
  
  test('Focus indicators are visible', () => {
    const focusableElements = document.querySelectorAll(
      'a, button, input, select, textarea, [tabindex]:not([tabindex="-1"])'
    );
    focusableElements.forEach(element => {
      element.focus();
      const outlineWidth = getComputedStyle(element).outlineWidth;
      expect(parseInt(outlineWidth)).toBeGreaterThanOrEqual(2);
    });
  });
});
```

### Manual Testing

**User Testing Protocol:**
- Test with diverse user groups
- Include users with disabilities
- Test with assistive technologies (screen readers, etc.)
- Document pain points and barriers
- Iterate based on feedback

**Diversity Checklist:**
- ☐ Users who rely on keyboard navigation
- ☐ Users who rely on screen readers
- ☐ Users with colorblindness
- ☐ Users with motor impairments
- ☐ Users with cognitive disabilities
- ☐ Older users (50+ age group)
- ☐ Users with limited gaming experience
- ☐ Users on varied devices (desktop, tablet, mobile)

### Accessibility Audit Checklist

**Visual:**
- ☐ All text meets contrast requirements (4.5:1 minimum)
- ☐ Color is not the only means of conveying information
- ☐ Text can be resized to 200% without loss of functionality
- ☐ High contrast mode available
- ☐ Colorblind modes available and functional

**Motor:**
- ☐ All touch/click targets are minimum 44x44 pixels
- ☐ Full keyboard navigation implemented
- ☐ Visible focus indicators on all interactive elements
- ☐ No keyboard traps
- ☐ Drag-and-drop has keyboard alternatives

**Cognitive:**
- ☐ Clear, simple language used throughout
- ☐ Consistent navigation patterns
- ☐ Error messages are helpful and actionable
- ☐ Progressive disclosure of complexity
- ☐ Help documentation available

**Auditory:**
- ☐ Visual alternatives for all audio cues
- ☐ Captions available for voice content
- ☐ Independent audio controls

**Screen Reader:**
- ☐ Semantic HTML structure
- ☐ ARIA labels for complex components
- ☐ Alt text for all meaningful images
- ☐ Proper heading hierarchy
- ☐ Live regions for dynamic content

## Success Metrics

### Quantitative Metrics

**Target Goals:**
- WCAG 2.1 Level AA compliance: 100%
- Lighthouse accessibility score: ≥95
- Keyboard navigation coverage: 100%
- Color contrast compliance: 100%
- Touch target size compliance: 100%

### Qualitative Metrics

**User Satisfaction:**
- Accessibility feature satisfaction: ≥85%
- "Easy to use" rating: ≥80%
- "Can play despite motor limitations": ≥75%
- Frustration with accessibility: <5%

### Diversity Metrics

**Player Demographics:**
- Gender diversity: Target 40-45% women (vs. <25% typical MMORPG)
- Age diversity: Target 30%+ players aged 40+
- Accessibility feature usage: Target 10%+ of players
- Early abandonment rate: Target <15% (vs. 40%+ typical F2P MMORPG)

## Resources and References

### Standards and Guidelines

- [WCAG 2.1](https://www.w3.org/WAI/WCAG21/quickref/) - Web Content Accessibility Guidelines
- [ARIA Authoring Practices](https://www.w3.org/WAI/ARIA/apg/) - Accessible Rich Internet Applications
- [Game Accessibility Guidelines](http://gameaccessibilityguidelines.com/) - Game-specific accessibility standards
- [Xbox Accessibility Guidelines](https://www.xbox.com/en-US/community/for-everyone/accessibility) - Console gaming accessibility

### Tools

- [Lighthouse](https://developers.google.com/web/tools/lighthouse) - Automated accessibility auditing
- [axe DevTools](https://www.deque.com/axe/devtools/) - Accessibility testing extension
- [WAVE](https://wave.webaim.org/) - Web accessibility evaluation tool
- [Color Contrast Analyzer](https://www.tpgi.com/color-contrast-checker/) - WCAG contrast checking
- [NVDA](https://www.nvaccess.org/) - Free screen reader for testing

### Related BlueMarble Documentation

- [UI Guidelines](ui-guidelines.md) - Core UI design principles
- [Cursor Accuracy Research](../../research/literature/game-dev-research-cursor-accuracy-gender-accessibility.md) - Gender differences in gaming
- [Aiming Mechanics Research](../../research/literature/game-dev-analysis-gender-differences-aiming-mechanics.md) - Motor control and task management

---

**Document Version:** 1.0  
**Last Updated:** 2025-11-09  
**Review Cycle:** Quarterly  
**Next Review:** 2026-02-09

*These guidelines are living documents and should be updated based on user feedback, emerging standards, and technological advances.*
