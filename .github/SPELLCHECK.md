# Spell Checking Guide

This repository uses [codespell](https://github.com/codespell-project/codespell) for automated spell checking in CI.

## Configuration Files

- **`.codespellrc`**: Main configuration file (skip patterns, quiet level)
- **`.codespell-ignore`**: Custom dictionary for project-specific terms

## Adding Words to Ignore List

If codespell flags a word that is correct (e.g., technical terms, proper nouns, game-specific jargon):

1. Add the word to `.codespell-ignore` (one word per line)
2. Group similar terms together with comments
3. Keep the list alphabetically organized within each section

Example:
```
# Game-specific terms
gameplay
metagame
wireframes
```

## Running Locally

Before pushing, you can check for spelling errors:

```bash
# Install codespell
pip install codespell

# Check all files
codespell

# Check specific files
codespell path/to/file.md

# Check with verbose output
codespell -v
```

## Common Game Development Terms Already Ignored

- `gameplay`, `metagame`, `routemap`
- `playtest`, `cooldown`, `wireframes`
- `unreal`, `ue` (Unreal Engine)
- `shard`, `durability`, `lifecycle`
- British English: `behaviour`, `colour`

## Best Practices

1. **Real typos**: Fix them in the source file
2. **Technical terms**: Add to `.codespell-ignore`
3. **Temporary words**: Use inline ignore comments when needed:
   ```markdown
   <!-- codespell:ignore specialword -->
   ```

## CI Integration

Codespell runs automatically on all PRs via `.github/workflows/quality.yml`.
