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

    private StatementData CreateStatementData(Invoice invoice)
    {
        var summaries = new List<PerformanceSummary>();
        foreach (var perf in invoice.Performances)
        {
            var play = _plays[perf.PlayID];
            summaries.Add(new PerformanceSummary(
                play.Name,
                perf.Audience,
                AmountFor(perf, play),
                VolumeCreditsFor(perf, play)));
        }
        return new StatementData(
            invoice.Customer,
            summaries,
            summaries.Sum(p => p.Amount),
            summaries.Sum(p => p.VolumeCredits));
    }

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

    private int AmountFor(Performance perf, Play play)
    {
        var result = 0;
        switch(play.Type)
        {
            case "tragedy":
                result = 40000;
                if (perf.Audience > 30) {
                    result += 1000 * (perf.Audience - 30);
                }
                break;
            case "comedy":
                result = 30000;
                if (perf.Audience > 20) {
                    result += 10000 + 500 * (perf.Audience - 20);
                }
                result += 300 * perf.Audience;
                break;
            default:
                throw new Exception("unknown type: " + play.Type);
        }
        return result;
    }

    private int VolumeCreditsFor(Performance perf, Play play)
    {
        var result = 0;
        result += Math.Max(perf.Audience - 30, 0);
        if ("comedy" == play.Type) 
        {
            result += perf.Audience / 5;
        }
        return result;
    }
}
