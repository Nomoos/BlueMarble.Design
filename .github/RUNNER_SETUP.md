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

#### Linux/macOS Setup
```bash
# Example for Ubuntu/Debian
sudo apt update
sudo apt install -y git curl python3 python3-pip nodejs npm

# Example for CentOS/RHEL
sudo yum install -y git curl python3 python3-pip nodejs npm

# Example for macOS (using Homebrew)
brew install git python3 node npm

# Verify installations
git --version
python3 --version
node --version
npm --version
```

#### Windows Setup

**Option 1: Using PowerShell (Recommended)**
```powershell
# Install Chocolatey package manager (if not already installed)
Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))

# Install required software using Chocolatey
choco install -y git python nodejs

# Verify installations
git --version
python --version
node --version
npm --version
```

**Option 2: Manual Installation**
1. Download and install [Git for Windows](https://git-scm.com/download/win)
2. Download and install [Python 3.8+](https://www.python.org/downloads/windows/)
3. Download and install [Node.js 18+](https://nodejs.org/en/download/)
4. Ensure all tools are added to your PATH environment variable

**Option 3: Using Windows Subsystem for Linux (WSL)**
```bash
# Install WSL2 with Ubuntu
wsl --install -d Ubuntu

# Then follow the Linux setup instructions above
```

### 2. Register the Runner

#### For All Operating Systems
1. Go to your repository Settings → Actions → Runners
2. Click "New self-hosted runner"
3. Select your operating system (Linux, macOS, or Windows)
4. Follow the platform-specific instructions below

#### Linux/macOS Registration
```bash
# Download and extract the runner
mkdir actions-runner && cd actions-runner
curl -o actions-runner-linux-x64-2.311.0.tar.gz -L https://github.com/actions/runner/releases/download/v2.311.0/actions-runner-linux-x64-2.311.0.tar.gz
tar xzf ./actions-runner-linux-x64-2.311.0.tar.gz

# Configure the runner
./config.sh --url https://github.com/YOUR_ORG/BlueMarble.Design --token YOUR_TOKEN

# Start the runner
./run.sh
```

#### Windows Registration (PowerShell)
```powershell
# Create a folder for the runner
mkdir actions-runner; cd actions-runner

# Download and extract the runner
Invoke-WebRequest -Uri https://github.com/actions/runner/releases/download/v2.311.0/actions-runner-win-x64-2.311.0.zip -OutFile actions-runner-win-x64-2.311.0.zip
Add-Type -AssemblyName System.IO.Compression.FileSystem ; [System.IO.Compression.ZipFile]::ExtractToDirectory("$PWD/actions-runner-win-x64-2.311.0.zip", "$PWD")

# Configure the runner
./config.cmd --url https://github.com/YOUR_ORG/BlueMarble.Design --token YOUR_TOKEN

# Start the runner
./run.cmd
```

#### Windows Service Installation (Optional)
```powershell
# Install as Windows service (run as Administrator)
./svc.sh install
./svc.sh start

# Check service status
./svc.sh status
```

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

**Cross-Platform Command Issues:**
- **Problem**: `find` command not available on Windows
- **Solution**: Our workflows now use Python fallbacks for file operations
- **Action**: Ensure Python 3.8+ is installed on Windows runners

**Path Separator Issues:**
- **Problem**: Unix forward slashes vs Windows backslashes
- **Solution**: Use relative paths and let Python handle normalization
- **Action**: Our workflows use `os.path.normpath()` for cross-platform paths

**Shell Compatibility:**
- **Problem**: Bash scripts failing on Windows PowerShell
- **Solution**: All workflow steps now explicitly use `shell: bash`
- **Action**: Git Bash is automatically available with Git for Windows

#### Windows-Specific Issues

**PowerShell Execution Policy:**
```powershell
# If scripts are blocked, update execution policy
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

**Path Issues:**
```powershell
# Add tools to PATH if not automatically detected
$env:PATH += ";C:\Program Files\Git\bin"
$env:PATH += ";C:\Program Files\nodejs"
$env:PATH += ";C:\Python39\Scripts"
```

**Python Command Variations:**
```bash
# Our workflows detect available Python command
# They try: python3, python, py -3
# Ensure at least one works on your system
```

#### Permission Issues
- **Linux/macOS**: Ensure runner user has necessary file system permissions
- **Windows**: Run PowerShell as Administrator for service installation
- **Docker**: Check if Docker requires specific user groups (if using Docker)

#### Network and Firewall Issues
- **GitHub Access**: Ensure outbound HTTPS (443) access to github.com
- **Package Downloads**: Allow access to package repositories (npmjs.org, pypi.org)
- **Windows Firewall**: May need to allow Git Bash and Node.js through firewall

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

## Windows-Specific Considerations

### Performance Tips
- **Use SSD storage** for better I/O performance
- **Allocate sufficient RAM** (minimum 8GB recommended for Windows)
- **Consider Windows Server** for dedicated runner machines
- **Use PowerShell Core** (pwsh) for better cross-platform compatibility

### Security Considerations
- **Windows Defender**: Add runner directory to exclusions for better performance
- **User Account Control (UAC)**: Consider running runner as service to avoid UAC prompts
- **Antivirus**: Exclude runner working directories from real-time scanning
- **Windows Updates**: Schedule updates during low-activity periods

### Monitoring on Windows
- **Event Viewer**: Check Windows Event Logs for runner service issues
- **Performance Monitor**: Track CPU, memory, and disk usage
- **Task Manager**: Monitor runner process and resource consumption
- **PowerShell Logs**: Enable PowerShell logging for debugging workflow issues

### Maintenance Tasks (Windows-Specific)
- **Weekly**: Check Windows Updates and install if needed
- **Weekly**: Clear runner working directories: `_work`, `_diag`
- **Monthly**: Update Git for Windows, Python, and Node.js
- **Monthly**: Check disk space and clean temporary files
- **Quarterly**: Review Windows security settings and runner permissions