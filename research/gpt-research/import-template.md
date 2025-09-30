# GPT Research Import Template

Use this template when importing a new ChatGPT conversation for research purposes.

## conversation.md Template

```markdown
# GPT Conversation: [Brief Title]

**Conversation ID**: [ID from URL]  
**Date**: [Date of conversation]  
**URL**: [Full ChatGPT share link]  
**Related Issue**: [GitHub issue link]

## Conversation Transcript

[Paste the full conversation here, preserving formatting]

### User
[User message]

### Assistant
[Assistant response]

[Continue alternating User/Assistant throughout the conversation]

## Context

[Any additional context needed to understand the conversation]
```

## analysis.md Template

```markdown
# Analysis: [Research Topic Name]

## Executive Summary

[2-3 paragraph overview of key findings]

## Key Insights

### 1. [First Major Insight]

[Detailed explanation]

**Relevance to BlueMarble**: [How this applies]

### 2. [Second Major Insight]

[Detailed explanation]

**Relevance to BlueMarble**: [How this applies]

[Continue for additional insights]

## Technical Findings

### Algorithms & Data Structures

[Discussion of any algorithms, data structures, or technical approaches]

### Performance Considerations

[Any performance implications or optimization strategies]

### Trade-offs

[Discussion of design trade-offs and decision criteria]

## Recommendations

### Immediate Actions

- [ ] [Action item 1]
- [ ] [Action item 2]

### Future Considerations

- [ ] [Future consideration 1]
- [ ] [Future consideration 2]

## Integration with Existing Research

### Related Documentation

- [Link to related spatial-data-storage research]
- [Link to related game-design research]
- [Link to other relevant docs]

### Cross-References

[How this research connects to or extends existing research]

## Questions for Further Research

1. [Question 1]
2. [Question 2]

## References

[Any external references or resources mentioned in the conversation]
```

## implementation-notes.md Template (Optional)

```markdown
# Implementation Notes: [Topic]

## Overview

[Brief description of implementation guidance from the research]

## Integration Points

### Frontend

[Frontend integration details]

**Files Affected**:
- `Client/js/...`

**Changes Required**:
- [Change 1]
- [Change 2]

### Backend

[Backend integration details]

**Files Affected**:
- `Generator/...`

**Changes Required**:
- [Change 1]
- [Change 2]

## Code Examples

### Example 1: [Description]

```javascript
// JavaScript example if applicable
```

### Example 2: [Description]

```csharp
// C# example if applicable
```

## Testing Strategy

[How to test the implementation]

## Migration Path

[If this requires changes to existing systems, describe the migration approach]

## Performance Impact

[Expected performance impact and any benchmarking needs]

## Dependencies

[Any new dependencies or version requirements]

## Timeline

[Estimated implementation timeline]

## Risks & Mitigation

| Risk | Mitigation |
|------|------------|
| [Risk 1] | [How to mitigate] |
| [Risk 2] | [How to mitigate] |
```

## Quick Start Checklist

When importing a new GPT conversation:

- [ ] Create directory: `research/gpt-research/conversation-[short-id]/`
- [ ] Create `conversation.md` using template above
- [ ] Create `analysis.md` using template above
- [ ] Create `implementation-notes.md` if applicable
- [ ] Create/update `README.md` for the conversation
- [ ] Update `research/gpt-research/README.md` to list the new research
- [ ] Update main `research/README.md` if adding a new category
- [ ] Link from relevant existing documentation
- [ ] Add to `RESEARCH_ISSUES_SUMMARY.md` if applicable
- [ ] Close or update related GitHub issues

## File Naming Conventions

- Use lowercase with hyphens: `conversation-id-topic.md`
- Keep filenames descriptive but concise
- Use standard names: `README.md`, `conversation.md`, `analysis.md`, `implementation-notes.md`

## Markdown Guidelines

- Use proper heading hierarchy (# for title, ## for sections, ### for subsections)
- Include code blocks with language specifiers
- Use tables for structured data
- Add links to related documentation
- Include diagrams or images if helpful (store in `assets/` subdirectory)
