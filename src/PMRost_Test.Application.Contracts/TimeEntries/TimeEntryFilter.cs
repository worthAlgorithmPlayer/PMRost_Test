
namespace PMRost_Test.Application.Contracts.TimeEntries;

public sealed class TimeEntryFilter
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? ProjectId { get; set; }
    public int Skip { get; set; } = 0;
    public int Limit { get; set; } = 10;
}
