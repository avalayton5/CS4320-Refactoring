using System;

namespace TheatricalPlayersRefactoringKata;

//each play type gets its own subclass holding its charge and credit rules
public abstract class PerformanceCalculator(Performance perf, Play play)
{
    protected Performance Perf {get; } = perf;
    public Play Play {get;} = play;

    public abstract int Amount {get;}

    //default credit rule
    //subclasses override this when their rule is different
    public virtual int VolumeCredits => Math.Max(Perf.Audience - 30, 0);

    //only place that checks the play type
    //adding a new type adds a new subclass and one case here
    public static PerformanceCalculator Create(Performance perf, Play play)
    {
        switch(play.Type)
        {
            case "tragedy":
                return new TragedyCalculator(perf, play);
            case "comedy":
                return new ComedyCalculator(perf, play);
            case "historical":
                return new HistoricalCalculator(perf, play);
            default:
                throw new Exception("unknown type: " + play.Type);
        }
    }
}

//$400 base, plus $10 per attendee beyond 30
public class TragedyCalculator(Performance perf, Play play) : PerformanceCalculator(perf, play)
{
    public override int Amount
    {
        get
        {
            var result = 40000;
            if(Perf.Audience > 30)
            {
                result += 1000 * (Perf.Audience - 30);
            }
            return result;
        }
    }
}

//$300 base, plus a flat $100 and $5 per attendee beyond 20, plus $3 per attendee regardless of size
public class ComedyCalculator(Performance perf, Play play) : PerformanceCalculator(perf, play)
{
    public override int Amount
    {
        get
        {
            var result = 30000;
            if(Perf.Audience > 20)
            {
                result += 10000 + 500 * (Perf.Audience - 20);
            }
            result += 300 * Perf.Audience;
            return result;
        }
    }
    public override int VolumeCredits => base.VolumeCredits + Perf.Audience / 5;
}

// $500 base, plus $15 per attendee beyond 20
public class HistoricalCalculator(Performance perf, Play play) : PerformanceCalculator(perf, play)
{
    public override int Amount
    {
        get
        {
            var result = 50000;
            if (Perf.Audience > 20)
            {
                result += 1500 * (Perf.Audience - 20);
            }
            return result;
        }
    }

    public override int VolumeCredits => Perf.Audience / 5;
}