# Copilot Reviewer Guide

## Repository overview
- **Project**: **ClickHouse.Driver** is the official ADO.NET client for ClickHouse database. This is a **high-performance database client** where **stability, correctness, performance, and comprehensive testing are critical priorities**.

- **Tech stack**: C#/.NET solution targeting `net462`, `net48`, `netstandard2.1`, `net6.0`, `net8.0`, and `net9.0`. Tests run on `net6.0`, `net8.0`, `net9.0`; integration tests target `net9.0`. Benchmarks run on `net9.0`.

## Review priorities
1. **Correctness first**: the driver must faithfully serialize/deserialize ClickHouse types, honor feature flags, and keep multi-framework compatibility.
2. **Stability & backwards compatibility**: preserve behaviour across supported ClickHouse versions (`FeatureSwitch`, `ClickHouseFeatureMap`, `PublicAPI/*`). Watch for breaking API surface changes—the Roslyn Public API analyzer enforces shipped signatures.
3. **Performance**: avoid extra allocations or buffering (core code lives under `ClickHouse.Driver/ADO`, `Types`, `Utility`). Benchmarks should not regress.
4. **Testing discipline**: ensure permutations are covered (ADO provider, parameter binding, ORMs). If touching protocol parsing or feature detection, prefer adding/adjusting NUnit coverage or integration tests.
5. **Configuration compliance**: respect `.editorconfig`, StyleCop suppressions, nullable/project settings, and analyzer warnings.


## Project Structure

### Solution Layout
```
ClickHouse.Driver.sln                    # Main solution file
├── ClickHouse.Driver/                   # Main library (NuGet package)
├── ClickHouse.Driver.Tests/             # Unit tests (NUnit, net6.0/8.0/9.0)
├── ClickHouse.Driver.IntegrationTests/  # Integration tests (net9.0 only)
└── ClickHouse.Driver.Benchmark/         # BenchmarkDotNet performance tests
```

### Main Driver Structure (`ClickHouse.Driver/`)

**Core ADO.NET Implementation** (`ADO/`):
- `ClickHouseConnection.cs` - Main connection class
- `ClickHouseCommand.cs` - Command execution
- `ClickHouseDataSource.cs` - Connection pooling
- `ClickHouseConnectionStringBuilder.cs` - Connection string parsing
- `Readers/` - Data reader implementations
- `Parameters/` - Parameter handling
- `Adapters/` - DbProviderFactory support

**Type System** (`Types/`):
- 60+ type implementations for ClickHouse types
- `TypeConverter.cs` - Central type conversion logic (14KB, complex)
- Numeric types: Int8-Int256, UInt8-UInt256, Float32/64, Decimal32-256
- Date/Time types: Date, Date32, DateTime, DateTime64
- Complex types: Array, Tuple, Map, Nested, LowCardinality, Nullable
- Special types: UUID, IPv4/IPv6, Enum8/16, JSON, Dynamic, Variant
- `Grammar/` - Type parsing grammar

**Bulk Copy** (`Copy/`):
- `ClickHouseBulkCopy.cs` - High-performance bulk insert
- `Serializer/` - Binary serialization for bulk operations

**HTTP Layer** (`Http/`):
- HTTP client factories and handlers
- Connection pooling strategies

**Utilities** (`Utility/`):
- `SchemaDescriber.cs` - Schema metadata
- `ClickHouseFeatureMap.cs` - Version-based feature detection
- Extension methods for various types

**Other**:
- `Formats/` - Binary readers/writers, HTTP parameter formatting
- `Constraints/` - Constraint handling
- `DependencyInjection/` - DI extensions
- `Diagnostic/` - OpenTelemetry integration
- `Json/` - JSON type support
- `Numerics/` - ClickHouseDecimal implementation
- `PublicAPI/` - Public API surface tracking (required by analyzer)

### Configuration Files

**Build & Analysis**:
- `ClickHouse.Driver.csproj` - Multi-target project file
- `.editorconfig` - Code style rules (file-scoped namespaces, StyleCop suppressions)
- `analysis.yml` - .NET Code Analysis workflow

## Automation signals a reviewer should expect
- PRs run coverage, multi-version ClickHouse regression, .NET TF regression, integration tests, benchmarks, and CodeQL. Failures usually indicate:
  - Feature incompatibility with old ClickHouse versions.
  - Missing environment flags or analyzer baseline updates.
  - Public API changes requiring `PublicAPI/*.txt` edits.
  - Coverage drop (Codecov upload path: `ClickHouse.Driver.Tests/coverage.<tf>.opencover.xml`).

## Root directory quick index
- Solution: `ClickHouse.Driver.sln`
- Projects: `ClickHouse.Driver/`, `ClickHouse.Driver.Tests/`, `ClickHouse.Driver.IntegrationTests/`, `ClickHouse.Driver.Benchmark/`
- Docs: `README.md`, empty `CONTRIBUTING.md` placeholder.
- Config: `.editorconfig`, `.gitattributes`, `.gitignore`, `.vscode/tasks.json`
- GitHub automation: `.github/workflows/*.yml`, `.github/coverage-status.py`, `.github/dependabot.yml`
- Licensing: `LICENSE`
- Misc: `analysis.yml`, `data.bin`

## Reviewer guidance
- Trust the commands and structure above; only reach for additional searches if the information here is incomplete or conflicts with observed behaviour.
- When evaluating changes, verify:
  1. Target framework conditional logic (look for `#if` blocks or `TargetFramework.StartsWith('net4')` items).
  2. Connection/path changes maintain compatibility with `CLICKHOUSE_CONNECTION` defaults used in CI.
  3. Tests covering new behaviour exist (prefer NUnit cases in existing suites, integration cases when real server interactions change).
  4. Performance-sensitive paths (`Utility/`, `Types/`, `ADO/Readers/`) avoid allocations/regressions—benchmarks can confirm.
- Keep feedback focused on correctness, stability, performance, and test coverage.
- **Trust these instructions**: Only search the codebase if information here is incomplete or incorrect
- **Performance matters**: This is a database client - watch for allocations, boxing, unnecessary copies
- **Type safety**: ClickHouse has many specialized types - ensure correct mapping
- **Streaming**: Code should maintain streaming behavior, avoid buffering entire responses
- **Multi-version support**: Changes must work across .NET Framework 4.6.2 through .NET 9.0
- **ClickHouse versions**: Driver supports multiple ClickHouse server versions
- **Test coverage**: New features require tests; bug fixes should include regression tests

## Common Patterns & Conventions

### Code Style
- **Namespaces**: File-scoped (enforced as warning)
- **Indentation**: 4 spaces for C#, 2 spaces for .csproj
- **Line endings**: CRLF for C# files
- **Nullability**: Not enforced in main driver, enabled in integration tests

### Type System Patterns
- All types inherit from `ClickHouseType`
- Type parsing handled by grammar in `Types/Grammar/`
- Type conversion centralized in `TypeConverter.cs`
- Many types have TODO comments for future enhancements

### Async Patterns
- Extensive use of async/await for I/O operations
- `CancellationToken` support throughout

### Error Handling
- `ClickHouseServerException` for server errors
- Proper disposal patterns with `IDisposable`


## CI/CD Workflows

### GitHub Actions (`.github/workflows/`)

**tests.yml** - Main test workflow (runs on push/PR to `main`):
- **Coverage job**: Runs with coverage reporting to Codecov
- **ClickHouse regression**: Tests against versions 25.3, 25.7, 25.8, 25.9
- **.NET regression**: Tests on net6.0 and net9.0 frameworks
- **Integration tests**: Runs integration test suite
- **Windows tests**: Currently disabled (`if: false`)

**reusable.yml** - Reusable test workflow:
- Parameterized by framework (default: net9.0) and ClickHouse version (default: latest)
- Runs on Ubuntu 22.04 with 5-minute timeout
- Uses ClickHouse service container
- Uploads coverage to Codecov

**benchmark.yml** - Performance benchmarks (runs on push/PR to `main`):
- Runs short benchmark suite
- Posts results to GitHub Actions summary

**codeql.yml** - Security analysis (runs on push to `main`):
- CodeQL analysis for C#

**release.yml** - Manual release workflow:
- Builds on Windows
- Creates NuGet package
- Publishes to NuGet.org
- Creates GitHub release

**analysis.yml** - .NET Code Analysis (runs on push/PR to `master` branch):
- Note: Uses `master` branch, not `main` (potential inconsistency)
- Runs on Windows with .NET 6.x, 8.x, 9.x

### Validation Checklist

Before approving a PR, verify:

1. **All CI checks pass**: Tests, benchmarks, CodeQL
2. **Code coverage maintained**: Check Codecov report
3. **Public API changes documented**: Update `PublicAPI/*.txt` if needed
4. **Multi-framework compatibility**: Changes work on all target frameworks
5. **Performance impact**: Review benchmark results if available
6. **Breaking changes**: Flag any breaking changes clearly
