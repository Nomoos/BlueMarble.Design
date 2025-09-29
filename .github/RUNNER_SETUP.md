# Self-Hosted Runner Configuration

This repository is configured to use self-hosted GitHub Actions runners exclusively. This ensures better security, control, and consistency for our CI/CD processes.

## Why Self-Hosted Runners?

- **Security**: Keep sensitive operations within our controlled environment
- **Consistency**: Ensure all builds run in the same environment regardless of GitHub's hosted runner availability
- **Performance**: Potentially faster builds with dedicated resources
- **Customization**: Install specific tools and configurations needed for our workflows
- **Cost Control**: Better cost predictability for CI/CD operations

## Runner Requirements

### Operating System Independence

All workflows are designed to be OS-independent and will work on:
- Linux (Ubuntu, CentOS, RHEL, etc.)
- macOS
- Windows

### Required Software

The self-hosted runners should have the following software installed:

#### Essential Tools
- Git (latest stable version)
- Python 3.8+ with pip
- Node.js 18+ with npm
- Docker (optional, for containerized builds)

#### Documentation Tools
- markdownlint-cli (installed via npm during workflow)
- PyYAML (installed via pip during workflow)

### Hardware Recommendations

- **Minimum**: 2 CPU cores, 4GB RAM, 20GB disk space
- **Recommended**: 4 CPU cores, 8GB RAM, 50GB disk space
- **Network**: Stable internet connection for GitHub API access

## Workflow Configuration

All workflows in this repository use `runs-on: self-hosted` to ensure they only execute on self-hosted runners. The workflows are designed to:

1. **Be OS-agnostic**: Use cross-platform commands and tools
2. **Install dependencies dynamically**: Don't assume pre-installed tools (except basics)
3. **Fail gracefully**: Provide clear error messages if requirements aren't met

## Available Workflows

### 1. CI - Documentation and Content Validation (`ci.yml`)
- Validates markdown files using markdownlint
- Checks repository structure
- Validates issue templates
- **Triggers**: Push to main/develop, Pull requests

### 2. Design Assets Validation (`design-validation.yml`)
- Validates design repository structure
- Checks for broken internal links
- Validates file naming conventions
- **Triggers**: Push/PR affecting docs, templates, assets, roadmap

### 3. Content Quality Check (`content-quality.yml`)
- Analyzes changed files for quality issues
- Validates YAML syntax
- Checks for sensitive information
- **Triggers**: Pull requests, manual dispatch

### 4. Issue and PR Automation (`issue-pr-automation.yml`)
- Validates issue template compliance
- Checks PR format and content
- Generates automation reports
- **Triggers**: Issues opened/edited, PRs opened/edited

### 5. Release and Deployment (`release-deployment.yml`)
- Prepares releases and generates changelogs
- Packages documentation
- Handles deployment processes
- **Triggers**: Tags, releases, manual dispatch

## Setting Up Self-Hosted Runners

### 1. Prepare the Runner Machine

```bash
# Example for Ubuntu/Debian
sudo apt update
sudo apt install -y git curl python3 python3-pip nodejs npm

# Verify installations
git --version
python3 --version
node --version
npm --version
```

### 2. Register the Runner

1. Go to your repository Settings → Actions → Runners
2. Click "New self-hosted runner"
3. Follow the instructions to download and configure the runner
4. Start the runner service

### 3. Configure Runner Labels (Optional)

You can add custom labels to runners for specific use cases:
- `documentation`: For documentation-focused tasks
- `design`: For design-related validations
- `fast`: For runners with better hardware

## Security Considerations

### Access Control
- Limit repository access to trusted contributors
- Use separate runners for different environments (staging vs production)
- Regularly update runner software and dependencies

### Secrets Management
- Store sensitive data in GitHub Secrets, not in workflow files
- Limit secret access to necessary workflows only
- Regularly rotate access tokens and keys

### Network Security
- Ensure runners are on secure networks
- Consider using VPN or private networks for sensitive operations
- Monitor runner activity and logs

## Troubleshooting

### Common Issues

#### Runner Not Picking Up Jobs
- Check if runner is online in GitHub Settings → Actions → Runners
- Verify runner labels match workflow requirements
- Check runner logs for connection issues

#### Workflow Failures
- Check runner has required software installed
- Verify network connectivity to GitHub and package repositories
- Check disk space and system resources

#### Permission Issues
- Ensure runner user has necessary file system permissions
- Check if Docker requires specific user groups (if using Docker)

### Getting Help

If you encounter issues with the self-hosted runner setup:

1. Check the runner logs on the machine
2. Review workflow run logs in GitHub Actions
3. Contact the DevOps team for infrastructure support
4. Review GitHub's self-hosted runner documentation

## Maintenance

### Regular Tasks
- Update runner software monthly
- Monitor disk space and clean up old artifacts
- Review and update workflow dependencies
- Test workflows after major system updates

### Monitoring
- Set up alerts for runner downtime
- Monitor resource usage
- Track workflow execution times and success rates

## Future Improvements

- Container-based workflows for better isolation
- Multi-platform builds if needed
- Integration with internal deployment systems
- Enhanced security scanning and validation