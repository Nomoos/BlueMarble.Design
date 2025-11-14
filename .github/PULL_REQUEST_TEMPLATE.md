## Description

<!-- Provide a clear and concise description of the changes in this PR -->

## Type of Change

<!-- Mark the relevant option with an 'x' -->

- [ ] New design document
- [ ] Design document update
- [ ] New research note
- [ ] Research update
- [ ] Template addition/update
- [ ] Infrastructure/CI improvement
- [ ] Documentation fix
- [ ] Bug fix
- [ ] Code change (C# implementation)
- [ ] Other (please describe):

## Related Issues

<!-- Link to related issues using #issue-number -->

Closes #
Related to #

## Changes Made

<!-- List the specific changes made in this PR -->

- 
- 
- 

## Checklist

<!-- Mark completed items with an 'x' -->

### General
- [ ] I have followed the [CONTRIBUTING.md](../CONTRIBUTING.md) guidelines
- [ ] File naming follows kebab-case conventions
- [ ] Documents include front matter (if applicable)
- [ ] Documents are focused and appropriately sized (200-400 lines target)
- [ ] Cross-links to related documents are included
- [ ] I have updated related index files (if applicable)
- [ ] All markdown files pass linting checks
- [ ] Links are valid and not broken
- [ ] I have used appropriate templates from `/templates`
- [ ] I have self-reviewed my changes

### Architecture (for code changes only)
- [ ] I have read the [Layered Architecture ADR](../docs/architecture/adr-002-layered-architecture-conventions.md)
- [ ] All dependencies flow downward (no upward or circular dependencies)
- [ ] Namespaces correctly reflect architectural layer
- [ ] Interfaces are defined in the lowest applicable layer
- [ ] No code duplication across layers
- [ ] Naming conventions followed (see [Coding Guidelines](../docs/architecture/CODING_GUIDELINES.md))
- [ ] Architecture tests pass locally (if applicable)

## Additional Context

<!-- Add any other context about the PR here -->

## Screenshots (if applicable)

<!-- Add screenshots for visual changes (UI mockups, diagrams, etc.) -->

## Reviewers

<!-- Tag relevant reviewers based on the area of change -->
<!-- Example: @username -->

---

**For Reviewers:**

Please check:
- [ ] Changes align with design principles
- [ ] Documentation is clear and complete
- [ ] File organization is appropriate
- [ ] No sensitive information is included

**For Code Reviewers (C# changes):**

Please verify architecture compliance (see [Architecture Review Guide](../docs/architecture/ARCHITECTURE_REVIEW_GUIDE.md)):
- [ ] Layer boundaries respected (dependencies flow downward only)
- [ ] Namespace organization correct
- [ ] No code duplication across layers
- [ ] Naming conventions followed
- [ ] Interfaces in correct layer
- [ ] No leaky abstractions
