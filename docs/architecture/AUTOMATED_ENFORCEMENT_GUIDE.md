# Automated Enforcement Guide

This guide explains how to set up and use automated tools to enforce the layered architecture defined in [ADR-002](./adr-002-layered-architecture-conventions.md).

## Overview

Automated enforcement catches architectural violations before code review, saving time and ensuring consistency. We use three main approaches:

1. **.NET Analyzers**: Built-in code analysis
2. **Architecture Tests**: Unit tests that validate layer dependencies
3. **CI/CD Integration**: Automatic validation on every push

## .NET Analyzers

### Setup

Add to `Directory.Build.props` in repository root:

```xml
<Project>
  <PropertyGroup>
    <AnalysisLevel>latest</AnalysisLevel>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
    <EnableNETAnalyzers>true</EnableNETAnalyzers>
    <TreatWarningsAsErrors>false</TreatWarningsAsErrors>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.CodeAnalysis.NetAnalyzers" Version="8.0.0" />
  </ItemGroup>
</Project>
```

### Running Locally

```bash
# Build with analysis
dotnet build --configuration Release

# Run analyzers only
dotnet build /p:RunAnalyzers=true
```

## Architecture Tests

### Setup

Create a test project:

```bash
dotnet new xunit -n BlueMarble.Architecture.Tests -o tests/BlueMarble.Architecture.Tests
cd tests/BlueMarble.Architecture.Tests
dotnet add package NetArchTest.Rules
dotnet add reference ../../src/BlueMarble.SpatialData/BlueMarble.SpatialData.csproj
dotnet add reference ../../src/BlueMarble.World/BlueMarble.World.csproj
```

### Test Examples

Create `ArchitectureTests.cs`:

```csharp
using NetArchTest.Rules;
using Xunit;
using BlueMarble.SpatialData;
using BlueMarble.World;

namespace BlueMarble.Architecture.Tests
{
    public class LayerDependencyTests
    {
        [Fact]
        public void SpatialDataLayer_ShouldNotDependOn_WorldLayer()
        {
            var result = Types.InAssembly(typeof(DeltaPatchOctree).Assembly)
                .That().ResideInNamespace("BlueMarble.SpatialData")
                .ShouldNot().HaveDependencyOn("BlueMarble.World")
                .GetResult();

            Assert.True(result.IsSuccessful, 
                "SpatialData layer should not depend on World layer");
        }

        [Fact]
        public void UtilsLayer_ShouldNotDependOn_AnyHigherLayer()
        {
            var utilsAssembly = typeof(BlueMarble.Utils.Spatial.DistributedStorage.DistributedStorage).Assembly;
            
            var result = Types.InAssembly(utilsAssembly)
                .That().ResideInNamespace("BlueMarble.Utils")
                .ShouldNot().HaveDependencyOn("BlueMarble.SpatialData")
                .And().ShouldNot().HaveDependencyOn("BlueMarble.SpatialIndexing")
                .And().ShouldNot().HaveDependencyOn("BlueMarble.World")
                .And().ShouldNot().HaveDependencyOn("BlueMarble.Examples")
                .GetResult();

            Assert.True(result.IsSuccessful, 
                "Utils layer should not depend on any higher layers");
        }

        [Fact]
        public void WorldLayer_CanDependOn_LowerLayers()
        {
            var result = Types.InAssembly(typeof(CoordinateValidator).Assembly)
                .That().ResideInNamespace("BlueMarble.World")
                .Should().HaveDependencyOnAny("BlueMarble.SpatialData", "BlueMarble.Utils")
                .GetResult();

            // This test allows dependencies on lower layers
            // Failure here means World layer is completely isolated, which is unusual
            Assert.True(result.IsSuccessful || result.FailingTypes.Count() == 0);
        }

        [Fact]
        public void ExamplesLayer_CanDependOn_AllLowerLayers()
        {
            // This test verifies that the Examples layer is properly consuming lower layers
            var result = Types.InAssembly(typeof(BlueMarble.Examples.SpatialExample).Assembly)
                .That().ResideInNamespace("BlueMarble.Examples")
                .Should().HaveDependencyOnAny("BlueMarble.World", "BlueMarble.SpatialData", "BlueMarble.Utils")
                .GetResult();

            Assert.True(result.IsSuccessful || result.FailingTypes.Count() == 0);
        }
    }

    public class NamingConventionTests
    {
        [Fact]
        public void Interfaces_ShouldStartWithI()
        {
            var result = Types.InAssemblies(new[]
                {
                    typeof(DeltaPatchOctree).Assembly,
                    typeof(CoordinateValidator).Assembly
                })
                .That().AreInterfaces()
                .Should().HaveNameStartingWith("I")
                .GetResult();

            Assert.True(result.IsSuccessful, 
                "All interfaces should start with 'I'");
        }

        [Fact]
        public void Classes_ShouldNotHave_ManagerSuffix()
        {
            // Discourage generic "Manager" classes
            var result = Types.InAssemblies(new[]
                {
                    typeof(DeltaPatchOctree).Assembly,
                    typeof(CoordinateValidator).Assembly
                })
                .That().AreClasses()
                .And().DoNotHaveNameMatching(".*Base$")
                .ShouldNot().HaveNameEndingWith("Manager")
                .GetResult();

            Assert.True(result.IsSuccessful, 
                "Avoid generic 'Manager' suffix. Use more specific names.");
        }
    }

    public class AbstractionTests
    {
        [Fact]
        public void PublicClasses_InDataStructuresLayer_ShouldBeSealed_OrAbstract_OrHaveInterface()
        {
            var result = Types.InAssembly(typeof(DeltaPatchOctree).Assembly)
                .That().AreClasses()
                .And().ArePublic()
                .And().DoNotHaveNameMatching(".*Base$")
                .Should().BeSealed()
                .Or().BeAbstract()
                .GetResult();

            // This encourages use of interfaces and prevents uncontrolled inheritance
            // Failures indicate classes that might need an interface extracted
            // Note: This is a strong convention - adjust based on team preference
        }
    }
}
```

### Running Tests

```bash
# Run architecture tests
dotnet test tests/BlueMarble.Architecture.Tests/

# Run with detailed output
dotnet test tests/BlueMarble.Architecture.Tests/ --logger "console;verbosity=detailed"
```

## CI/CD Integration

### GitHub Actions Workflow

Create `.github/workflows/architecture-validation.yml`:

```yaml
name: Architecture Validation

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  architecture-tests:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Run Architecture Tests
      run: dotnet test tests/BlueMarble.Architecture.Tests/ --no-build --configuration Release --logger "trx;LogFileName=architecture-results.trx"
    
    - name: Upload Test Results
      if: always()
      uses: actions/upload-artifact@v3
      with:
        name: architecture-test-results
        path: '**/architecture-results.trx'
    
    - name: Code Analysis
      run: dotnet build --configuration Release /p:TreatWarningsAsErrors=true /p:RunAnalyzers=true

  dependency-check:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Check for circular dependencies
      run: |
        echo "Checking for circular dependencies..."
        dotnet list package --include-transitive > dependencies.txt
        # Add custom script to detect circular dependencies if needed
```

### Pre-commit Hook

Create `.git/hooks/pre-commit` (optional local enforcement):

```bash
#!/bin/bash

echo "Running architecture tests..."
dotnet test tests/BlueMarble.Architecture.Tests/ --no-build --configuration Debug

if [ $? -ne 0 ]; then
    echo "❌ Architecture tests failed. Commit blocked."
    echo "Fix architectural violations before committing."
    exit 1
fi

echo "✅ Architecture tests passed."
exit 0
```

Make executable:
```bash
chmod +x .git/hooks/pre-commit
```

## NsDepCop (Optional Advanced Tool)

### Installation

```xml
<ItemGroup>
  <PackageReference Include="NsDepCop" Version="2.3.0" PrivateAssets="all" />
</ItemGroup>
```

### Configuration

Create `config.nsdepcop` in project root:

```xml
<?xml version="1.0" encoding="utf-8"?>
<NsDepCopConfig>
  <Allowed From="BlueMarble.World" To="BlueMarble.SpatialData" />
  <Allowed From="BlueMarble.World" To="BlueMarble.SpatialIndexing" />
  <Allowed From="BlueMarble.World" To="BlueMarble.Utils.*" />
  
  <Allowed From="BlueMarble.SpatialData" To="BlueMarble.Utils.*" />
  <Allowed From="BlueMarble.SpatialIndexing" To="BlueMarble.Utils.*" />
  
  <Allowed From="BlueMarble.Examples" To="*" />
  
  <Disallowed From="BlueMarble.SpatialData" To="BlueMarble.World" />
  <Disallowed From="BlueMarble.SpatialData" To="BlueMarble.Examples" />
  <Disallowed From="BlueMarble.SpatialIndexing" To="BlueMarble.World" />
  <Disallowed From="BlueMarble.Utils.*" To="BlueMarble.*" />
</NsDepCopConfig>
```

## Troubleshooting

### Test Failures

**Issue**: Architecture tests failing unexpectedly

**Solutions**:
1. Check if new dependencies were added
2. Review recent namespace changes
3. Verify project references in `.csproj` files
4. Run `dotnet clean` and rebuild

### False Positives

**Issue**: Tests flag valid patterns

**Solutions**:
1. Review if the pattern truly follows architecture
2. Update test to allow specific valid pattern
3. Add exception for specific case with clear comment
4. Document exception in ADR-002

### Performance Issues

**Issue**: Architecture tests slow down CI/CD

**Solutions**:
1. Run architecture tests in parallel with unit tests
2. Cache dependencies in CI/CD
3. Run architecture tests only on PR, not every commit
4. Optimize test selection (test only changed layers)

## Maintenance

### Updating Tests

When architecture evolves:

1. Update ADR-002 first
2. Update architecture tests to match
3. Update this guide
4. Communicate changes to team
5. Provide grace period for adoption

### Adding New Layer

When adding a new layer:

1. Document in ADR-002
2. Add corresponding architecture tests
3. Update CI/CD configuration
4. Update NsDepCop config (if used)
5. Train team on new layer

## Related Documents

- [ADR-002: Layered Architecture](./adr-002-layered-architecture-conventions.md)
- [Coding Guidelines](./CODING_GUIDELINES.md)
- [Architecture Review Guide](./ARCHITECTURE_REVIEW_GUIDE.md)
