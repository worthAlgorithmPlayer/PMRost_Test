

namespace PMRost_Test.Domain.TimeEntries.Services;

internal interface ITimeEntryService
{
    public TimeEntry Create(
        Employee employee,
        Project project,
        DateOnly date,
        decimal hours,
        string? comment);
}
