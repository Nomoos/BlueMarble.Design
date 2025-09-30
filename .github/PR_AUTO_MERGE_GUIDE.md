# PR Auto-Merge from Main - User Guide

## Overview

The PR Auto-Merge automation helps keep pull requests up-to-date with the `main` branch by automatically merging the latest changes into outdated PRs. This reduces merge conflicts and ensures PRs always reflect the current state of the codebase.

## How It Works

### Automatic Detection

The workflow runs in the following scenarios:

1. **Scheduled**: Every 6 hours to check all open PRs
2. **On Push to Main**: Immediately after changes are merged to main
3. **On PR Events**: When a PR is opened, synchronized, or reopened
4. **Manual Trigger**: Can be manually triggered via GitHub Actions UI

### Merge Process

For each open pull request:

1. **Detection**: Check if the PR is behind the main branch
2. **Auto-Merge**: If outdated, automatically merge main into the PR branch
3. **Notification**: Post a comment on the PR to notify the author
4. **Conflict Handling**: If conflicts exist, notify the author with resolution instructions

## What Happens When Your PR Is Updated

### Successful Auto-Merge

When your PR is successfully updated, you'll receive a comment like this:

```
🤖 Automated PR Update

This PR was behind the `main` branch by X commit(s). 

I've automatically merged the latest changes from `main` into this PR branch to keep it up-to-date.

Next Steps:
- ✅ Review the changes to ensure compatibility
- ✅ Check that all tests pass
- ✅ Resolve any merge conflicts if they appear
```

**What to do:**
- Review the merge commit in your PR
- Ensure all CI checks pass
- Test your changes with the new main branch changes
- Resolve any issues that arise

### Merge Conflicts Detected

If automatic merging fails due to conflicts, you'll receive a comment with instructions:

```
🤖 PR Update Required - Manual Intervention Needed

This PR is behind the `main` branch by X commit(s), but automatic merging failed due to conflicts.

Action Required:
Please manually merge `main` into your branch or resolve conflicts
```

**What to do:**

#### Option 1: Use Command Line

```bash
# Update your local repository
git fetch origin

# Switch to your PR branch
git checkout your-branch-name

# Merge main into your branch
git merge origin/main

# Resolve any conflicts in your editor
# Look for conflict markers: <<<<<<< HEAD

# After resolving conflicts, commit and push
git add .
git commit -m "Merge main and resolve conflicts"
git push
```

#### Option 2: Use GitHub Web UI

1. Navigate to your PR on GitHub
2. Click the "Resolve conflicts" button if available
3. Use the web editor to resolve conflicts
4. Mark as resolved and commit

#### Option 3: Use GitHub Copilot

If you have GitHub Copilot enabled:

1. **In VS Code or Your IDE:**
   - Open the conflicted files
   - Copilot will highlight conflict markers
   - Ask Copilot: "Help me resolve this merge conflict"
   - Copilot will suggest resolutions based on context

2. **Using Copilot Chat:**
   - Open Copilot Chat in your IDE
   - Ask: "What's the best way to resolve these conflicts?"
   - Review and apply suggestions

3. **Using GitHub Copilot CLI:**
   ```bash
   # If you have GitHub Copilot CLI installed
   gh copilot suggest "resolve merge conflicts between my branch and main"
   ```

## Best Practices

### For PR Authors

1. **Keep PRs Small**: Smaller PRs are easier to keep updated and have fewer conflicts
2. **Merge Frequently**: Don't let your PR get too far behind main
3. **Respond Quickly**: Address auto-merge notifications promptly
4. **Test After Updates**: Always test your changes after main is merged in

### For Reviewers

1. **Review Promptly**: Quick reviews reduce the chance of PRs becoming outdated
2. **Check Merge Commits**: Verify that auto-merge didn't introduce issues
3. **Watch for Conflicts**: Monitor PRs that require manual conflict resolution

### For Team Leads

1. **Monitor Workflow**: Check the workflow logs for patterns
2. **Adjust Schedule**: Modify the cron schedule if needed (see Configuration section)
3. **Track Metrics**: Review the summary reports to identify problem areas

## Configuration

### Adjusting the Schedule

Edit `.github/workflows/pr-auto-merge-main.yml` to change when the workflow runs:

```yaml
schedule:
  - cron: '0 */6 * * *'  # Current: every 6 hours
  # Examples:
  # - cron: '0 */12 * * *'  # Every 12 hours
  # - cron: '0 0 * * *'     # Daily at midnight
  # - cron: '0 9,17 * * 1-5'  # Twice daily on weekdays
```

### Excluding Specific PRs

To prevent auto-merge on specific PRs, add the label `no-auto-merge` to the PR. 
(Note: This feature would require a small modification to the workflow)

### Permissions

The workflow requires these permissions:
- `contents: write` - To merge branches
- `pull-requests: write` - To update PRs and add comments
- `issues: write` - To post comments

## Troubleshooting

### Workflow Not Running

**Check:**
1. Workflow is enabled in repository settings
2. Self-hosted runner is online and connected
3. Required permissions are granted

### Auto-Merge Failing

**Common causes:**
1. **Conflicts**: Manual resolution required
2. **Protected branches**: Branch protection rules may prevent auto-updates
3. **Draft PRs**: Draft PRs are intentionally skipped

### Getting Help

1. Check workflow logs in the Actions tab
2. Review error messages in PR comments
3. Contact @Nomoos or the team for assistance
4. Refer to [CONTRIBUTING.md](../CONTRIBUTING.md) for general guidelines

## Examples

### Example 1: Successful Auto-Merge

```
Scenario: PR #42 is 3 commits behind main, no conflicts
Result: ✅ Main is automatically merged into PR
Action: Review the merge commit and continue development
```

### Example 2: Conflict Resolution

```
Scenario: PR #43 has conflicts with changes in main
Result: ⚠️ Author receives conflict notification
Action: Follow the instructions to manually resolve conflicts
```

### Example 3: Multiple Outdated PRs

```
Scenario: Main is updated with major changes, 5 PRs become outdated
Result: ✅ Workflow runs and updates all 5 PRs
Action: Each author reviews their updated PR
```

## Benefits

### For Developers

- ✅ **Less Manual Work**: No need to manually merge main into your branch
- ✅ **Fewer Surprises**: Conflicts are discovered early
- ✅ **Current Code**: Always working with the latest codebase
- ✅ **Better Testing**: CI runs against current main branch

### For the Team

- ✅ **Faster Merging**: PRs stay ready to merge
- ✅ **Fewer Blockers**: Outdated PRs don't block other work
- ✅ **Better Quality**: Issues are caught before final merge
- ✅ **Clear Communication**: Automated notifications keep everyone informed

### For the Project

- ✅ **Improved Velocity**: Less time spent on merge conflicts
- ✅ **Better Stability**: Integration issues caught earlier
- ✅ **Cleaner History**: Regular merges create clearer git history
- ✅ **Consistent Standards**: Automated process ensures consistency

## Related Documentation

- [CONTRIBUTING.md](../CONTRIBUTING.md) - General contribution guidelines
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Git Merge Documentation](https://git-scm.com/docs/git-merge)

## Feedback and Improvements

Have suggestions for improving this automation? 

1. Open an issue with the `enhancement` label
2. Propose changes in a PR
3. Discuss in team meetings

This automation is designed to evolve based on team needs and feedback.
