

namespace PMRost_Test.Domain.TimeEntries.Services;

public interface ITimeEntryService
{
    public TimeEntryCreationResult Create(
        Employee employee,
        Project project,
        DateOnly date,
        decimal hours,
        string? comment,
        IReadOnlyCollection<TimeEntry> employeeEntriesOnSameDate,
        bool isPeriodClosed,
        string createdBy);

    public void RecalculateOvertimeForDay(IReadOnlyCollection<TimeEntry> employeeEntriesOnSameDate);
}
