# CS 4320 OC2: Refactoring

## Build and Test Instructions

### Tools and dependencies
- .NET SDK 10.0 (developed with SDK 10.0.402) from https://dotnet.microsoft.com/download
- macOS, using the terminal in Visual Studio Code
- xUnit test framework (restored automatically by `dotnet test`; no manual installs)
- Git, for the commit history

### Commands

```
dotnet build
dotnet test
```

The first run downloads NuGet packages, so it may take a minute. A successful run ends with a "Test summary" line showing zero failures.

## Part 1
1. Comment and syntax errors: comment states "every ten comedy attendees," but the code divides by 5. Also, there is a stray semicolon at the end of the credits line.
2. One method for calculating, tracking, and formatting: can be broken into several smaller functions to make the code cleaner and less confusing.
3. Switch on play.Type: makes it difficult to add new play types, and the type is checked in two different places, so adding "historical" means finding and editing both.

## Part 2

### Git Log

1. Move large calculation into AmountFor
    Separate pricing from statement building so Print doesn't mix the two. Verified with dotnet test and 2/2 passing

2. Move volume credit calculation into VolumeCreditsFor
    Gives credit rules their own method away from the loop. Verified with d
    otnet test, 2/2 successes

3. Separate calculation from formatting
    Credit rules get their own method, and CreateStatementData does the math and RenderPlainText just formats. Verified with dotnet test, and 2/2 succeeded

4. add separate classes to perform the calculations
    Each play type has its own charge and credit rules, the type is checked in one method and a new type only needs a new class. Verified with dotnet test, 2/2 passed

5. Removes switch and type check from AmountFor and VolumeCreditsFor.
     Type is checked only in PerformanceCalculator.Create. Verified with dotnet test, 2/2 pass

6. Replace original with polymorphism, use the calculators in StatementPrinter

7. Add boundary tests for comedy charges with audience 20 and 21
    Covers >20 threshold and rounding. Verified with dotnet test and 4/4 now passing

8. add historical play type
    New HistoricalCalculator plus a case in PerformanceCalculator.Create. Tests cover the example in the assignment plus the boundary with an audience of 20 and 21. Verified with dotnet test and 7/7 succeeded

### Initial results
Command: `dotnet test` on the unmodified starter code, .NET 10.0.

```
Test summary: total: 2, failed: 0, succeeded: 2, skipped: 0, duration: 1.8s
```

### Final results
Command: `dotnet test`

```
Test summary: total: 7, failed: 0, succeeded: 7, skipped: 0, duration: 0.9s
Build succeeded in 2.0s
```


### New tests

## Part 3
- `ComedyAtAudience20HasNoExtraCharge`
  - Comedy at exactly 20 -> the extra $100 + $5/person applies only above 20. Expected-> $300 + $3 x 20 = $360.00; credits = 20/5 = 4.
- `ComedyAtAudience21AddsFlatFeeAndPerPersonCharge`
  - Comedy at 21 -> one person over the threshold triggers the flat fee. Expected -> $300 + $100 + $5 x 1 + $3 x 21 = $468.00; credits = 21/5 = 4 (rounded down)

These two tests passed against the original code (baseline commit `425de43`: total 4, failed 0) before refactoring and still pass after.

## Part 4 Tests
- `HistoricalAtAudience35MatchesExample`: $725.00 and 7 credits, the example from the assignment
- `HistoricalAtAudience20HasNoSurcharge`: boundary; $500.00 and 4 credits
- `HistoricalAtAudience21AddsSurchargeForOnePerson`: boundary; $515.00 and 4 credits
