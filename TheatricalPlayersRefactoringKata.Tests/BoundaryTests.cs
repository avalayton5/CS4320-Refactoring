using System.Collections.Generic;
using Xunit;

namespace TheatricalPlayersRefactoringKata.Tests;

public class BoundaryTests
{
    // Builds a one-performance invoice and returns the printed statement.
    private static string PrintFor(string type, int audience)
    {
        var plays = new Dictionary<string, Play> { { "p1", new Play("Test Play", type) } };
        var invoice = new Invoice("Customer", new List<Performance> { new Performance("p1", audience) });
        return new StatementPrinter(plays).Print(invoice);
    }

    // Comedy at exactly 20: the extra $100 + $5/person applies only ABOVE 20.
    // Expected: $300 + $3 x 20 = $360.00; credits = 20/5 = 4.
    [Fact]
    public void Comedy_AtAudience20_HasNoExtraCharge()
    {
        var result = PrintFor("comedy", 20);
        Assert.Contains("$360.00", result);
        Assert.Contains("You earned 4 credits", result);
    }

    // Comedy at 21: one person over the threshold triggers the flat fee.
    // Expected: $300 + $100 + $5 x 1 + $3 x 21 = $468.00; credits = 21/5 = 4 (rounded down).
    [Fact]
    public void Comedy_AtAudience21_AddsFlatFeeAndPerPersonCharge()
    {
        var result = PrintFor("comedy", 21);
        Assert.Contains("$468.00", result);
        Assert.Contains("You earned 4 credits", result);
    }
}