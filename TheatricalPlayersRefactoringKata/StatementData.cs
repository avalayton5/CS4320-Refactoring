using System.Collections.Generic;

namespace TheatricalPlayersRefactoringKata;

public record PerformanceSummary(string PlayName, int Audience, int Amount, int VolumeCredits);

//data holders passed from calculation to rendering 
public record StatementData(
    string Customer,
    List<PerformanceSummary> Performances,
    int TotalAmount,
    int TotalVolumeCredits);