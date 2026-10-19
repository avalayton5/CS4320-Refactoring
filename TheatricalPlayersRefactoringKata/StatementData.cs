using System.Collections.Generic;

namespace TheatricalPlayersRefactoringKata;

public record PerformanceSummary(string PlayName, int Audience, int Amount, int VolumeCredits);

public record StatementData(
    string Customer,
    List<PerformanceSummary> Performances,
    int TotalAmount,
    int TotalVolumeCredits);