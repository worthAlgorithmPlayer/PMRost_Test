
using PMRost_Test.Domain.Primitives.EntityTemplates;

namespace PMRost_Test.Domain;

public class TimesheetHistory : DomainEntity
{
    public Guid TimeEntryId { get; private set; }
    /// <summary>
    /// Старая версия
    /// </summary>
    public DateOnly PreviousTimesheetDate { get; private set; }
    public decimal PreviousHours { get; private set; }

    public string ChangedBy { get; private set; } = default!;
    public DateTimeOffset ChangedAtUtc { get; private set; }

    private TimesheetHistory(Guid timeEntryId, DateOnly previousDate, decimal previousHours, string changedBy)
    {
        TimeEntryId = timeEntryId;
        PreviousTimesheetDate = previousDate;
        PreviousHours = previousHours;
        ChangedBy = changedBy;
        ChangedAtUtc = DateTimeOffset.UtcNow;
    }
}
