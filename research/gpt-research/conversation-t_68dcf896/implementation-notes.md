# Implementation Notes: GitHub Labels Refactoring

## Overview

This document provides practical guidance for implementing the label refactoring
recommendations identified in the analysis. The implementation focuses on incremental
improvements to the existing label system without disrupting current workflows.

## Integration Points

### GitHub Configuration Files

**Files Affected**:
- `.github/labeler.yml` - Auto-labeling configuration
- `.github/workflows/autolabel.yml` - Auto-labeling workflow
- `.github/ISSUE_TEMPLATE/*.yml` - Issue template label assignments

**Changes Required**:
- Review and update label assignments in issue templates
- Add missing patterns to labeler.yml if needed
- Document label color scheme

### Documentation Files

**Files Affected**:
- `CONTRIBUTING.md` - Add label usage guidelines
- `README.md` - Reference label system if appropriate

**Changes Required**:
- Add section on label conventions
- Document when to use specific labels
- Provide examples of good labeling practices

## Implementation Phases

### Phase 1: Audit and Documentation (Immediate)

**Timeline**: 1-2 days

**Tasks**:
1. Create a comprehensive list of all labels currently in use
2. Document the purpose of each label
3. Identify unused or redundant labels
4. Create a label reference guide

**Validation**:
- All labels have clear definitions
- No duplicate or overlapping labels
- Label purposes are documented

### Phase 2: Label Cleanup (Short-term)

**Timeline**: 3-5 days

**Tasks**:
1. Remove or archive unused labels
2. Merge redundant labels
3. Ensure consistent naming (lowercase, hyphen-separated)
4. Apply color scheme to existing labels

**Validation**:
- Label list is streamlined
- All active issues and PRs maintain appropriate labels
- Color scheme is consistent

### Phase 3: Enhanced Auto-labeling (Medium-term)

**Timeline**: 1 week

**Tasks**:
1. Add labels for missing directories (e.g., assets)
2. Consider more specific labels for large directories
3. Test auto-labeling on sample PRs
4. Update documentation

**Example labeler.yml additions**:

```yaml
assets:
  - assets/**
  - assets/**/*

game-design:
  - research/game-design/**
  - research/game-design/**/*

spatial-data:
  - research/spatial-data-storage/**
  - research/spatial-data-storage/**/*

gpt-research:
  - research/gpt-research/**
  - research/gpt-research/**/*
```

**Validation**:
- Auto-labeling works for all major directories
- No conflicts between label rules
- Labels are applied consistently

### Phase 4: Status and Priority Labels (Optional, Long-term)

**Timeline**: 2-3 weeks

**Tasks**:
1. Define status label set (e.g., needs-review, in-progress, blocked)
2. Define priority label set (e.g., priority:high, priority:medium, priority:low)
3. Update issue templates to include these labels where appropriate
4. Train team on label usage
5. Consider automation for status changes

**Validation**:
- Status labels are consistently applied
- Priority labels help with triage
- Team understands label meanings

## Code Examples

### Example: Enhanced labeler.yml Configuration

```yaml
# Research areas
research:
  - research/**
  - research/**/*

game-design-research:
  - research/game-design/**
  - research/game-design/**/*

spatial-research:
  - research/spatial-data-storage/**
  - research/spatial-data-storage/**/*

gpt-research:
  - research/gpt-research/**
  - research/gpt-research/**/*

# Design areas
design:
  - design/**
  - design/**/*

# Infrastructure
roadmap:
  - roadmap/**
  - roadmap/**/*

templates:
  - templates/**
  - templates/**/*

# Documentation
documentation:
  - docs/**
  - docs/**/*
  - "*.md"
  - CONTRIBUTING.md
  - README.md
  - DOCUMENTATION_BEST_PRACTICES.md

# CI/CD and Infrastructure
ci:
  - .github/workflows/**
  - .github/workflows/**/*

infrastructure:
  - .github/**
  - scripts/**
  - "*.yml"
  - "*.yaml"
  - "*.json"

# Assets
assets:
  - assets/**
  - assets/**/*
```

### Example: Label Color Scheme

```yaml
# Type Labels (Blues)
bug: d73a4a          # Red for bugs
chore: fef2c0       # Light yellow for chores
enhancement: a2eeef  # Light blue for enhancements

# Area Labels (Oranges/Yellows)
research: fbca04      # Yellow
design: f9d0c4       # Light orange
documentation: 0075ca # Blue

# Status Labels (Greens/Reds)
in-progress: 0e8a16  # Green
blocked: d93f0b      # Red
needs-review: fbca04 # Yellow

# Priority Labels (Gradient)
priority:high: d93f0b    # Red
priority:medium: fbca04  # Yellow
priority:low: 0e8a16    # Green

# Size Labels (Grays)
xs: ededed
s: d4d4d4
m: b8b8b8
l: 8a8a8a
xl: 6a6a6a
```

## Testing Strategy

### Manual Testing

1. **Create test PRs** that touch different areas
2. **Verify auto-labeling** applies correct labels
3. **Create test issues** from templates
4. **Verify label consistency** across issues and PRs

### Automated Testing

1. **Workflow validation**: Ensure autolabel.yml runs without errors
2. **Configuration validation**: Use YAML linters on labeler.yml
3. **Label coverage**: Create script to verify all directories are covered

### Testing Checklist

- [ ] Test PR touching only research files → gets `research` label
- [ ] Test PR touching only design files → gets `design` label
- [ ] Test PR touching documentation → gets `documentation` label
- [ ] Test PR with multiple areas → gets multiple labels
- [ ] Test issue creation from each template → correct labels applied
- [ ] Test PR size labeling for XS through XL changes

## Migration Path

### Step 1: Backup Current Configuration

```bash
# Create backup of current labels
gh label list --json name,color,description > .github/labels-backup.json
```

### Step 2: Document Current State

```bash
# Export current labeler.yml
cp .github/labeler.yml .github/labeler.yml.backup
```

### Step 3: Implement Changes Incrementally

- Make one change at a time
- Test each change in a draft PR
- Merge changes after validation

### Step 4: Monitor and Adjust

- Review label usage after 2 weeks
- Gather feedback from contributors
- Adjust labels based on actual usage patterns

## Performance Impact

**Expected Impact**: Minimal to none

- Label application is handled by GitHub Actions
- Additional labels don't significantly impact workflow performance
- More specific labels may improve developer productivity

## Dependencies

**Required**:
- GitHub Actions enabled
- `actions/labeler@v6` or higher
- `codelytv/pr-size-labeler@v1` or compatible version

**Optional**:
- GitHub CLI for label management
- YAML linters for configuration validation

## Timeline

### Immediate (Week 1)
- Document current labels
- Create label reference guide
- Identify cleanup opportunities

### Short-term (Weeks 2-3)
- Implement label cleanup
- Update labeler.yml with new patterns
- Update documentation

### Medium-term (Month 2)
- Add status/priority labels if needed
- Implement any new automation
- Train team on label usage

### Long-term (Ongoing)
- Monitor label effectiveness
- Adjust based on team feedback
- Maintain label documentation

## Risks & Mitigation

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Breaking existing workflows | Low | High | Test changes in draft PRs before merging |
| Label proliferation | Medium | Medium | Regular label audits, deprecate unused labels |
| Inconsistent label application | Medium | Low | Clear documentation and examples |
| Team confusion about new labels | Medium | Medium | Training session, written guidelines |
| Automation failures | Low | Medium | Monitor workflow runs, have rollback plan |

## Success Metrics

### Quantitative
- Reduction in unlabeled issues/PRs
- Increase in correctly labeled items
- Reduced time to label items manually

### Qualitative
- Improved issue discoverability
- Better project organization
- Positive team feedback

## Rollback Plan

If issues arise:

1. **Revert labeler.yml** to backup version
2. **Restore previous workflow** configuration
3. **Communicate changes** to team
4. **Document lessons learned** for future attempts

## Next Steps

1. Review this implementation plan with team
2. Get approval for Phase 1 changes
3. Create tracking issue for implementation
4. Begin Phase 1 audit and documentation
5. Schedule regular check-ins to assess progress
