using System.Collections.Generic;
using Xunit;

namespace TheatricalPlayersRefactoringKata.Tests;

public class BoundaryTests
{
    // Builds a one performance invoice and returns the printed statement
    private static string PrintFor(string type, int audience)
    {
        var plays = new Dictionary<string, Play> { { "p1", new Play("Test Play", type) } };
        var invoice = new Invoice("Customer", new List<Performance> { new Performance("p1", audience) });
        return new StatementPrinter(plays).Print(invoice);
    }

    // Comedy at exactly 20 -> the extra $100 + $5/person applies only above 20
    // Expected-> $300 + $3 x 20 = $360.00; credits = 20/5 = 4.
    [Fact]
    public void ComedyAtAudience20HasNoExtraCharge()
    {
        var result = PrintFor("comedy", 20);
        Assert.Contains("$360.00", result);
        Assert.Contains("You earned 4 credits", result);
    }

    // Comedy at 21 -> one person over the threshold triggers the flat fee
    // Expected -> $300 + $100 + $5 x 1 + $3 x 21 = $468.00; credits = 21/5 = 4 (rounded down)
    [Fact]
    public void ComedyAtAudience21AddsFlatFeeAndPerPersonCharge()
    {
        var result = PrintFor("comedy", 21);
        Assert.Contains("$468.00", result);
        Assert.Contains("You earned 4 credits", result);
    }

    //35 people = $500 + $15 x 15 = $725, credits = 35/5 = 7
    [Fact]
    public void HistoricalAtAudience35MatchesExample()
    {
        var result = PrintFor("historical", 35);
        Assert.Contains("$725.00", result);
        Assert.Contains("You earned 7 credits", result);
    }

    // Exactly 20 has no per person surcharge, $500.00, credits = 4.
    [Fact]
    public void HistoricalAtAudience20HasNoSurcharge()
    {
        var result = PrintFor("historical", 20);
        Assert.Contains("$500.00", result);
        Assert.Contains("You earned 4 credits", result);
    }

    //21 adds $15 for the one person over, $515.00, credits = 21/5 = 4.
    [Fact]
    public void HistoricalAtAudience21AddsSurchargeForOnePerson()
    {
        var result = PrintFor("historical", 21);
        Assert.Contains("$515.00", result);
        Assert.Contains("You earned 4 credits", result);
    }
}