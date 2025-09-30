# Documentation Best Practices

This guide provides best practices for creating and maintaining high-quality documentation in the BlueMarble.Design repository.

## Content Quality

### Avoid Duplication

- **Check existing content**: Before creating new documentation, search for similar content that already exists
- **Link instead of copying**: When referencing information from another document, link to it rather than duplicating the content
- **Consolidate related information**: If multiple documents cover similar topics, consider consolidating them
- **Use templates consistently**: Templates help maintain consistency and reduce duplication

### Structure and Organization

- **Use clear headings**: Create a logical hierarchy with descriptive headings (H2, H3, H4)
- **Keep sections focused**: Each section should cover one main topic or concept
- **Avoid nested complexity**: Don't go deeper than 4 levels of headings (H1-H4)
- **Use numbered lists for steps**: When describing a process, use numbered lists to show sequence
- **Use bullet points for features**: When listing features or characteristics, use bullet points

### Writing Style

- **Write concisely**: Remove unnecessary words while maintaining clarity
- **Use active voice**: "Create a new file" instead of "A new file should be created"
- **Define technical terms**: Explain acronyms and technical terms on first use
- **Be consistent with terminology**: Use the same terms throughout documentation
- **Include examples**: Provide practical examples to illustrate concepts

## Formatting Standards

### Markdown Best Practices

- **Line length**: Keep lines under 120 characters for better readability
- **Blank lines**: Add blank lines around headings and between sections
- **Code blocks**: Always specify the language for syntax highlighting
  ```markdown
  \`\`\`python
  # Python code here
  \`\`\`
  ```
- **Lists**: Add blank lines before and after lists
- **Links**: Use descriptive link text, not "click here"
  - Good: `[Contributing Guidelines](CONTRIBUTING.md)`
  - Bad: `Click [here](CONTRIBUTING.md) for guidelines`

### Document Structure

Every major document should include:

1. **Title**: Clear, descriptive H1 heading
2. **Metadata**: Version, author, date, status (for formal documents)
3. **Overview/Introduction**: Brief explanation of the document's purpose
4. **Table of Contents**: For longer documents (optional for short ones)
5. **Main Content**: Well-organized sections with clear headings
6. **References**: Links to related documents or external resources
7. **Revision History**: Track major changes (for living documents)

Example structure:

```markdown
# Document Title

**Document Type:** Feature Specification  
**Version:** 1.0  
**Author:** Your Name  
**Date:** 2024-01-15  
**Status:** Draft

## Overview

Brief introduction to the document's purpose and scope.

## Table of Contents

1. [Section 1](#section-1)
2. [Section 2](#section-2)

## Section 1

Content here...

## Section 2

Content here...

## References

- [Related Document 1](path/to/doc1.md)
- [Related Document 2](path/to/doc2.md)

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2024-01-15 | Your Name | Initial version |
```

## File Naming

### Conventions

- **Use lowercase**: `player-progression-system.md` not `Player-Progression-System.md`
- **Use hyphens**: `combat-mechanics.md` not `combat_mechanics.md` or `combatmechanics.md`
- **Be descriptive**: Use clear, meaningful names that indicate content
- **Use prefixes**: For specific document types:
  - `gdd-` for Game Design Documents
  - `tdd-` for Technical Design Documents
  - `spec-` for Feature Specifications
  - `research-` for Research Reports

### Examples

- Good: `spec-guild-system.md`, `research-player-retention-2024.md`
- Bad: `guilds.md`, `research.md`, `System Design.md`

## Version Control

### Commit Messages

- **Be specific**: Describe what changed, not just "updated documentation"
- **Use imperative mood**: "Add combat system spec" not "Added combat system spec"
- **Reference issues**: Include issue numbers when applicable: "Fix broken links (#123)"
- **Keep it concise**: 50 characters or less for the subject line

Good examples:
```
Add initial guild system specification
Fix broken links in gameplay documentation
Update combat mechanics based on playtest feedback
```

Bad examples:
```
updates
fixed stuff
documentation changes
```

### Pull Requests

- **Create focused PRs**: One topic or feature per PR
- **Write clear descriptions**: Explain what changed and why
- **Request appropriate reviewers**: Tag team members with relevant expertise
- **Address feedback promptly**: Respond to comments and make requested changes
- **Keep PRs small**: Easier to review and merge

## Cross-Referencing

### Internal Links

- **Use relative paths**: `../gameplay/spec-combat.md` not absolute URLs
- **Check links regularly**: Use CI to validate links aren't broken
- **Link to specific sections**: Use anchors when referencing a specific section
  ```markdown
  [Combat Mechanics](../gameplay/spec-combat.md#damage-calculation)
  ```

### External Links

- **Use stable URLs**: Avoid linking to content that may move or disappear
- **Include access date**: For time-sensitive external references
- **Archive important references**: Consider saving copies of critical external resources

## Maintenance

### Regular Reviews

- **Schedule reviews**: Quarterly reviews of key documentation
- **Update outdated content**: Remove or archive obsolete information
- **Fix broken links**: Use CI tools to identify and fix broken links
- **Validate formatting**: Run linters regularly to catch formatting issues

### Archiving

- **Move deprecated docs**: Place old versions in `/archive` directory
- **Add deprecation notices**: Clearly mark deprecated content
- **Maintain redirects**: Update links that point to archived content
- **Keep revision history**: Document what was archived and why

## Quality Checks

### Before Committing

Run these checks before committing documentation:

1. **Spell check**: Use a spell checker on your content
2. **Link validation**: Ensure all internal links work
3. **Format validation**: Run markdownlint to catch formatting issues
   ```bash
   markdownlint --config .markdownlint.json **/*.md
   ```
4. **Read aloud**: Read your content aloud to catch awkward phrasing
5. **Check examples**: Verify that code examples and commands work

### Linting Configuration

The repository uses `.markdownlint.json` for formatting rules:

```json
{
  "default": true,
  "MD013": {
    "line_length": 120
  },
  "MD033": false,
  "MD041": false
}
```

Key rules:
- Line length limited to 120 characters
- HTML allowed (MD033 disabled)
- First line doesn't need to be H1 (MD041 disabled)

## Templates

### Using Templates

- **Always start with a template**: Use appropriate template from `/templates` directory
- **Fill all sections**: Don't leave template sections empty
- **Adapt as needed**: Modify templates to fit your specific needs
- **Propose improvements**: Suggest template changes that would benefit everyone

### Creating Templates

When creating new templates:

1. **Identify common patterns**: Look for repeated structures across documents
2. **Make them flexible**: Templates should guide, not restrict
3. **Include examples**: Show how to fill each section
4. **Document usage**: Explain when and how to use the template
5. **Get feedback**: Have others review before adding to repository

## Accessibility

### Writing for All Users

- **Use clear language**: Avoid jargon when possible
- **Provide context**: Don't assume prior knowledge
- **Use descriptive headings**: Headings should convey meaning
- **Add alt text**: Describe images and diagrams
- **Consider screen readers**: Use semantic markdown structures

### Inclusive Documentation

- **Use inclusive language**: Avoid gender-specific terms when not necessary
- **Consider international users**: Avoid idioms and culturally-specific references
- **Provide translations**: Consider key documents in multiple languages (future)
- **Support multiple formats**: Some users may prefer different formats

## Copilot and AI-Assisted Documentation

### After Copilot Runs

When documentation is generated or modified by GitHub Copilot:

1. **Review thoroughly**: Don't blindly accept AI-generated content
2. **Check for duplication**: AI may recreate existing content
3. **Verify accuracy**: Ensure technical details are correct
4. **Format properly**: Apply markdown linting and formatting rules
5. **Add human context**: Include rationale and decision-making context
6. **Test examples**: Verify that code examples and commands work

### Saving Copilot-Generated Content

- **Document the source**: Note when content was AI-assisted
- **Refine and edit**: Improve AI-generated text for clarity and accuracy
- **Merge with existing docs**: Integrate with existing documentation rather than creating duplicates
- **Follow conventions**: Ensure AI-generated content follows repository standards

## Issue Completion

### Documenting Work

When completing issues:

1. **Use the completion template**: Follow `templates/issue-completion-template.md`
2. **Tag the issue creator**: Use @username to notify the creator
3. **Summarize changes**: Provide clear list of what was modified/added
4. **Explain reasoning**: Document why changes were made
5. **Note follow-ups**: Mention any related work or dependencies

Example:
```markdown
## Issue Completion Summary

@Nomoos

### Changes Made
- Updated documentation best practices guide
- Fixed duplicate content in USAGE_EXAMPLES.md
- Improved CI workflow error handling

### Reasoning & Context
- Best practices guide helps maintain documentation quality
- Removed duplicate "Scenario 3" to improve clarity
- CI workflows now provide better error messages for debugging

### Additional Notes
- Consider adding automated duplication detection in future
- May want to expand best practices based on team feedback

Issue completed and ready for review.
```

## Tools and Automation

### Recommended Tools

- **Markdown editors**: VS Code with Markdown extensions
- **Linters**: markdownlint-cli for format checking
- **Link checkers**: Built into CI workflows
- **Spell checkers**: Browser extensions or IDE plugins

### CI Integration

The repository includes automated checks:

- **Markdown linting**: Catches formatting issues
- **Link validation**: Identifies broken internal links
- **YAML validation**: Ensures workflow and template files are valid
- **Content quality**: Scans for potential issues

## Getting Help

If you're unsure about documentation standards:

1. **Review existing docs**: Look at similar documents for examples
2. **Check templates**: Use appropriate templates as starting points
3. **Ask for review**: Request feedback on your draft
4. **Consult this guide**: Refer back to these best practices
5. **Suggest improvements**: Propose changes to improve standards

---

**Last Updated:** 2024-12-29  
**Maintainers:** BlueMarble.Design Documentation Team

For questions or suggestions, create an issue or reach out to the documentation team.
