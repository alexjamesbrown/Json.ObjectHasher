# GitHub Actions CI/CD Workflow

This repository now includes a comprehensive GitHub Actions workflow for continuous integration and deployment.

## Workflow File

`.github/workflows/ci.yml` - Main CI/CD pipeline

## Triggers

The workflow runs automatically on:
- **Push** to `main`, `master`, or `develop` branches
- **Pull requests** targeting `main`, `master`, or `develop` branches
- **Manual trigger** via GitHub Actions UI (workflow_dispatch)

## Jobs

### 1. Build and Test
- Restores NuGet dependencies (with caching)
- Builds the solution in Release configuration
- Runs all unit tests with code coverage collection
- Uploads test results and coverage reports as artifacts
- Publishes build artifacts

**Artifacts:**
- `test-results` - Test results in .trx format (30-day retention)
- `code-coverage` - Code coverage in Cobertura XML format (30-day retention)
- `published-artifacts` - Published binaries (90-day retention)

### 2. Dependency Review (PRs only)
- Reviews dependency changes for security vulnerabilities
- Fails on moderate or higher severity issues
- Blocks GPL-3.0 and LGPL-3.0 licenses
- Posts summary comments on pull requests

### 3. CodeQL Security Analysis
- Performs static code analysis for security vulnerabilities
- Uses extended security and quality queries
- Scans C# code (excluding test directories)
- Uploads results to GitHub Security tab

### 4. Status Check
- Validates that all critical jobs succeeded
- Provides clear failure messages if any job fails

## Security Features

✅ **Action Pinning**: All actions pinned to specific versions
- `actions/checkout@v4`
- `actions/setup-dotnet@v4`
- `actions/upload-artifact@v4`
- `actions/cache@v4`
- `github/codeql-action@v3`
- `actions/dependency-review-action@v4`

✅ **Minimal Permissions**: Default `contents: read`, overridden only as needed per job

✅ **Concurrency Control**: 
- Cancels outdated PR builds to save resources
- Never cancels main/master/develop branch builds

✅ **Caching**: 
- NuGet packages cached between runs
- Cache keys based on project files and lock files

✅ **Security Scanning**:
- CodeQL static analysis
- Dependency vulnerability review
- License compliance checking

## Environment Variables

```yaml
DOTNET_VERSION: '8.0.x'
DOTNET_SKIP_FIRST_TIME_EXPERIENCE: true
DOTNET_NOLOGO: true
DOTNET_CLI_TELEMETRY_OPTOUT: true
```

## Viewing Results

1. **Workflow Runs**: Go to the "Actions" tab in GitHub
2. **Test Results**: Download artifacts from completed workflow runs
3. **Code Coverage**: Download coverage reports from artifacts
4. **Security Alerts**: Check the "Security" tab for CodeQL findings

## Local Validation

Before pushing changes, you can validate the workflow locally:

```bash
# Build and test locally (same as CI)
dotnet restore JsonObjectHasher.slnx
dotnet build JsonObjectHasher.slnx --configuration Release --no-restore
dotnet test JsonObjectHasher.slnx --configuration Release --no-build --verbosity normal
```

## Customization

To customize the workflow:
- **Change .NET version**: Modify `DOTNET_VERSION` in the `env` section
- **Add branches**: Update the `branches` lists under `on.push` and `on.pull_request`
- **Adjust retention**: Change `retention-days` in artifact upload steps
- **Modify severity**: Change `fail-on-severity` in dependency review

## Status Badge

Add this badge to your README.md to show workflow status:

```markdown
![CI/CD Pipeline](https://github.com/alexjamesbrown/Json.ObjectHasher/actions/workflows/ci.yml/badge.svg)
```

## Troubleshooting

If the workflow fails:
1. Check the workflow run logs in the Actions tab
2. Review failed job steps for error messages
3. Verify all dependencies are properly restored
4. Check for security vulnerabilities in dependencies
5. Review CodeQL findings in the Security tab
