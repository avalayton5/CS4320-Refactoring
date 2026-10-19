using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TheatricalPlayersRefactoringKata;

public class StatementPrinter(Dictionary<string, Play> plays)
{
    private readonly Dictionary<string, Play> _plays = plays;
    
    public string Print(Invoice invoice)
    {
        var data = CreateStatementData(invoice);
        return RenderPlainText(data);
    }

    //all calculation, no formatting, builds data that any renderer can use
    private StatementData CreateStatementData(Invoice invoice)
    {
        var summaries = new List<PerformanceSummary>();
        foreach (var perf in invoice.Performances)
        {
            var calculator = PerformanceCalculator.Create(perf, _plays[perf.PlayID]);
            summaries.Add(new PerformanceSummary(
                calculator.Play.Name,
                perf.Audience,
                calculator.Amount,
                calculator.VolumeCredits));
        }
        return new StatementData(
            invoice.Customer,
            summaries,
            summaries.Sum(p => p.Amount),
            summaries.Sum(p => p.VolumeCredits));
    }

    //formatting only, no calculations
    private static string RenderPlainText(StatementData data)
    {
        var cultureInfo = new CultureInfo("en-US");
        var result = $"Statement for {data.Customer}\n";
        foreach (var perf in data.Performances)
        {
            result += $"  {perf.PlayName}: {(perf.Amount / 100m).ToString("C", cultureInfo)} ({perf.Audience} seats)\n";
        }
        result += $"Amount owed is {(data.TotalAmount / 100m).ToString("C", cultureInfo)}\n";
        result += $"You earned {data.TotalVolumeCredits} credits\n";
        return result;
    }
}
