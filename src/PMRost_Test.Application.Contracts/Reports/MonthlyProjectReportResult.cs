
namespace PMRost_Test.Application.Contracts.Reports;

public sealed class MonthlyProjectReportResult
{
    public List<ProjectReportRow> Rows { get; set; } = new();
    public decimal GrandTotalHours { get; set; }
    public decimal GrandTotalCost { get; set; }
    public decimal GrandTotalBudget { get; set; }

    public decimal GrandBudgetUsagePercentage =>
        GrandTotalBudget > 0
            ? Math.Round(GrandTotalCost / GrandTotalBudget * 100m, 2)
            : 0m;
    public bool IsGrandOverrun =>
        GrandTotalBudget <= 0
            ? GrandTotalCost > 0
            : GrandBudgetUsagePercentage > 100m;

    public bool IsGrandRisk =>
        GrandTotalBudget > 0
        && GrandBudgetUsagePercentage > 80m
        && GrandBudgetUsagePercentage <= 100m;
}

public sealed class ProjectReportRow
{
    public Guid ProjectId { get; set; }
    public string ProjectNumber { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public decimal Budget { get; set; }

    public decimal TotalHours { get; set; }
    public decimal TotalCost { get; set; }

    public decimal BudgetUsagePercentage => Budget > 0 ? Math.Round((TotalCost / Budget) * 100, 2) : 0m;
    public bool IsOverrun =>
            Budget <= 0
                ? TotalCost > 0
                : BudgetUsagePercentage > 100m;

    public bool IsRisk =>
        Budget > 0
        && BudgetUsagePercentage > 80m
        && BudgetUsagePercentage <= 100m;
}
