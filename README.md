## Build and Test Instructions

### Tools and dependencies
- .NET SDK 10.0 (developed with SDK 10.0.402) from https://dotnet.microsoft.com/download
- macOS, using the terminal in Visual Studio Code
- xUnit test framework (restored automatically by `dotnet test`; no manual installs)
- Git, for the commit history

### Commands
From the repository root:

```
cd csharp
dotnet build
dotnet test
```

The first run downloads NuGet packages, so it may take a minute. A successful run ends with a "Test summary" line showing zero failures.

## Test Results

### Initial results (before any changes)
Command: `dotnet test` in the `csharp` folder, on the unmodified starter code, .NET 10.0.

```
Test summary: total: 2, failed: 0, succeeded: 2, skipped: 0, duration: 1.8s
```

### Final results (after refactoring, new tests, and the historical extension)
Command: `dotnet test`

```
[PASTE FINAL TEST SUMMARY HERE]
```

### New tests

**Tests for existing behavior (Part 3)**
- `[TestName1]`: [What input it uses, which rule or boundary it checks, and why the supplied tests did not cover it.]
- `[TestName2]`: [Same.]

These two tests passed against the original code before refactoring and still pass after.

**Tests for the historical play type (Part 4)**
- `[TestName3]`: [Example: audience of 35 gives $725 and 7 credits.]
- `[TestName4]`: [Boundary case, such as an audience of exactly 20 or 21.]
