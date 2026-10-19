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

## Reflection
    I believe the refactoring that improved the program the most was the Polymorphism stage. The earlier steps with extraction and splitting made it easier to read, but overall, the steps were still scattered. Before the change was made, the play type was checked in the switch for charges and again in a separate statement for credits. After the change, each play type has its own calculator class with both its charge and credit rules, and the type is checked in only one place. Then, I did not have to remember every spot that worried about the different play types. 
    Refactoring and adding functionality are different because of their uses. Refactoring only changes the structure of the code while keeping the output the same, and the tests are what proved that. Through all of the refactoring steps, the expected output never changed, and I ran `dotnet test` after each one. Adding functionality changes what the overall program does, so it needs new tests for the new behaviors. Keeping them in separate commits made the difference visible in my history. The refactoring commits all say the tests are unchanged, and the historical commit is the first place that new functionality appears.
    The new design helped a lot when I added historical plays. It took one new class, very similar to the others, and add one new case in the PerformanceCalculator. I did not have to touch the tragedy or comedy code. The one thing I had to think about was credits. The base class gives one credit per person beyond 30, but the historical rule is only one credit per five people, so HistoricalCalculator overrides VolumeCredits without calling the base version. If I had copied the comedy approach, the 35 person example given in the assignment would have given 12 credits instead of 7.
    Many of the original maintainability issues were resolved. The one long method is now split into a calculation phase and a rendering phase. The scattered type checks are now down to one method. The "every ten" comment and the stray semicolon are now gone. Although the switch is still in place, the play type is now only checked once instead of in different places.

## AI Statement
I used Claude throughout the assignment to understand the starter code and Refactoring concepts, review my code for compile errors, set up .NET, suggest boundary test cases, and structure my README neatly.
It gave me ideas for the calculator classes, and I put them in my project, tested, and adjusted. I worked out the expected values for my new tests by hand from the rules, ran all the tests myself, and made the commits. I understand the final design and can explain each change. 

## Atribution
Based on the Theatrical Players Refactoring Kata by Emily Bache
(https://github.com/emilybache/Theatrical-Players-Refactoring-Kata),
Modified by Ava Layton, October 19, 2026 
Modifications: refactored `StatementPrinter` into separate calculation and rendering phases with per play type calculator classes, added boundary tests, and added a "historical" play type with tests.
