using PMRost_Test.Domain.Primitives.EntityTemplates;

namespace PMRost_Test.Domain.TimeEntries;
/// <summary>
/// Запись табеля
/// </summary>
public sealed class TimeEntry : DomainEntity
{
    public Guid EmployeeId { get; private set; }
    public Guid ProjectId { get; private set; }
    public DateOnly TimesheetDate { get; private set;  }
    public int Hours { get; set; }
    /// <summary>
    /// Актуальная на текущий момент ставка сотрудника
    /// </summary>
    public decimal RateApplied { get; private set; }
    public string? Comment { get; private set;  }
    public long Version { get; private set;  }
    /// <summary>
    /// Логический флаг, отвечающий за переработку
    /// </summary>
    public bool IsOvertime { get; private set; }
    public string CreatedBy { get; private set; } = default!;
    public DateTimeOffset CreatedAtUtc { get; private set; }

    private TimeEntry(Guid employeeId, Guid projectId, DateOnly timesheetDate,
        int hours, decimal rateApplied, string? comment,
        bool isOvertime, string createdBy)
    {
        EmployeeId = employeeId;
        ProjectId = projectId;
        TimesheetDate = timesheetDate;
        RateApplied = rateApplied;
        Comment = comment;
        IsOvertime = isOvertime;
        CreatedBy = createdBy;
        CreatedAtUtc = TimeProvider.System.GetUtcNow();
        Version = 1;
    }

    internal static TimeEntry Create(
        Guid employeeId, Guid projectId, DateOnly timesheetDate,
        int hours, decimal rateApplied, string? comment,
        bool isOvertime, string createdBy)
        => new(employeeId, projectId, timesheetDate, hours, rateApplied, comment, isOvertime, createdBy);
}
