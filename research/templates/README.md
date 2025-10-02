# Research Templates

This directory contains templates for organizing research in BlueMarble's recursive, step-by-step structure.

## Available Templates

### [research-area-README-template.md](research-area-README-template.md)

Template for the main README.md in a research area directory. Use this when creating a new research area to provide overview and navigation.

**When to use**: Creating a new top-level research directory (e.g., `research/new-topic/`)

### [step-README-template.md](step-README-template.md)

Template for README.md files within step directories. Use this to document individual steps in the research process.

**When to use**: Creating step directories (e.g., `step-1-analysis/`, `step-2.1-sub-topic/`)

## Usage Instructions

### Creating a New Research Area

1. Create the research area directory: `research/[area-name]/`
2. Copy `research-area-README-template.md` to `research/[area-name]/README.md`
3. Fill in the template with your research area details
4. Create step directories as needed: `research/[area-name]/step-N-[name]/`
5. Add README.md to each step using `step-README-template.md`

### Adding Steps to Existing Research

1. Create step directory: `step-N-[name]/`
2. Copy `step-README-template.md` to `step-N-[name]/README.md`
3. Fill in step details, findings, and content
4. Update parent README.md to include the new step
5. Add navigation links to adjacent steps

### Creating Recursive Sub-steps

1. Within a step directory, create: `step-N.M-[name]/`
2. Copy `step-README-template.md` to the new sub-step
3. Fill in sub-step details
4. Update parent step README to list the sub-steps
5. Add "Parent" link in "Related Steps" section

## Template Customization

Feel free to adapt templates for your specific needs:

- Add sections relevant to your research type
- Remove optional sections that don't apply
- Maintain consistent structure across related steps
- Keep navigation links up to date

## Examples

See these research areas for examples of the structure in practice:

- [Spatial Data Storage](../spatial-data-storage/) - 5 main steps with flat structure
- [Game Design](../game-design/) - 4 main steps with recursive sub-steps in Step 2
- [Simple Example](examples/simple-research-example/) - Minimal template example (3 steps, flat)

## Related Documentation

- [RESEARCH_ORGANIZATION.md](../RESEARCH_ORGANIZATION.md) - Complete organizational guidelines
- [Research Index](../index.md) - Master index of all research
- [Main Research README](../README.md) - Research directory overview
