# Research Organization Structure

This document describes the recursive, step-by-step organizational structure for BlueMarble research.

## Overview

Research in BlueMarble is organized in a recursive hierarchy where:

1. Each research area starts with an **overview** that lists the main research steps
2. Each step can be broken down into **sub-steps** with dedicated folders
3. Sub-steps can themselves contain **further sub-steps**, recursively
4. Each level contains files with detailed documentation

## Structure Pattern

```
research/
├── [research-area]/              # Top-level research area
│   ├── README.md                 # Overview with step-by-step outline
│   ├── step-1-[name]/           # First major step
│   │   ├── README.md            # Step overview and sub-steps
│   │   ├── analysis.md          # Detailed analysis
│   │   ├── findings.md          # Research findings
│   │   └── step-1.1-[name]/    # Recursive sub-step (if needed)
│   │       ├── README.md        # Sub-step details
│   │       └── ...              # Additional files
│   ├── step-2-[name]/           # Second major step
│   │   ├── README.md
│   │   └── ...
│   └── step-3-[name]/           # Third major step
│       └── ...
```

## Benefits

### 1. Clear Navigation
- Researchers can quickly understand the overall structure
- Step numbering provides clear progression
- READMEs at each level guide understanding

### 2. Recursive Depth
- Simple topics can remain flat
- Complex topics can be broken down as deeply as needed
- Each level maintains the same organizational pattern

### 3. Progressive Detail
- High-level overview in top README
- Increasing detail as you navigate deeper
- Self-contained documentation at each level

### 4. Flexible Expansion
- New steps can be added without restructuring
- Sub-steps can be added to any step as research grows
- Pattern scales from simple to complex research

## Implementation Guidelines

### Creating a New Research Area

1. Create the research area directory
2. Add README.md with overview and step list
3. Create step-N-[name] folders for major steps
4. Add README.md in each step folder
5. Include detailed research files in each step

### Organizing Existing Research

1. Identify the main research steps
2. Create step folders for each major phase
3. Move related files into appropriate steps
4. Update README.md files with navigation
5. Add cross-references between steps

### Adding Sub-steps

When a step becomes complex:

1. Create step-N.M-[name] folders within the step
2. Add README.md explaining sub-step breakdown
3. Distribute files to appropriate sub-steps
4. Update parent README.md with sub-step outline

### Naming Conventions

- **Step folders**: `step-N-[descriptive-name]/`
- **Sub-step folders**: `step-N.M-[descriptive-name]/`
- **Further nesting**: `step-N.M.P-[descriptive-name]/`
- Use kebab-case for names
- Keep names concise but descriptive

## Examples

### Simple Structure (Flat)

```
research/simple-topic/
├── README.md          # Overview with 3 steps
├── step-1-foundation/
│   ├── README.md
│   └── analysis.md
├── step-2-implementation/
│   ├── README.md
│   └── design.md
└── step-3-validation/
    ├── README.md
    └── results.md
```

### Complex Structure (Recursive)

```
research/complex-topic/
├── README.md                      # Overview with main steps
├── step-1-research-phase/
│   ├── README.md                  # Step overview with sub-steps
│   ├── step-1.1-analysis/
│   │   ├── README.md
│   │   ├── data.md
│   │   └── findings.md
│   ├── step-1.2-comparison/
│   │   ├── README.md
│   │   ├── metrics.md
│   │   └── step-1.2.1-detailed-metrics/
│   │       ├── README.md
│   │       └── benchmarks.md
│   └── step-1.3-recommendations/
│       └── README.md
├── step-2-design-phase/
│   └── ...
└── step-3-implementation-phase/
    └── ...
```

## README Template

Each README.md should follow this pattern:

```markdown
# [Step Name]

## Overview
Brief description of this research step.

## Steps
1. **[Step 1 Name]** - Brief description
2. **[Step 2 Name]** - Brief description
3. **[Step 3 Name]** - Brief description

## Research Content
- [Document Name](file.md) - Description
- [Another Document](other.md) - Description

## Related Steps
- Previous: [Step N-1](../step-N-1-name/)
- Next: [Step N+1](../step-N+1-name/)

## Summary
Key findings and takeaways from this step.
```

## Integration with Existing Structure

This organizational pattern complements existing research structures:

- **Topics** (`topics/`): Small, focused notes remain flat
- **Research Areas** (e.g., `spatial-data-storage/`): Large areas use step structure
- **GPT Research** (`gpt-research/`): Can organize conversations into steps
- **Literature** (`literature/`): Individual reviews remain focused
- **Experiments** (`experiments/`): Can organize by experimental phases

## Migration Strategy

For existing research areas:

1. **Phase 1**: Add step overview to README.md
2. **Phase 2**: Create step-N folders for major phases
3. **Phase 3**: Organize existing files into steps
4. **Phase 4**: Add sub-steps where complexity warrants
5. **Phase 5**: Update cross-references and navigation

## Best Practices

### Do's
- ✅ Keep README.md files concise with clear step outlines
- ✅ Use consistent naming across all levels
- ✅ Add navigation links between related steps
- ✅ Include summary sections highlighting key findings
- ✅ Break complex steps into sub-steps when needed

### Don'ts
- ❌ Don't create unnecessary nesting (keep it as flat as possible)
- ❌ Don't duplicate content across multiple steps
- ❌ Don't create steps with only one or two files
- ❌ Don't break natural groupings for the sake of structure
- ❌ Don't forget to update parent README when adding sub-steps

## Version History

- 2025-01-19: Initial organizational structure document created
