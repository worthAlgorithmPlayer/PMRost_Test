
using PMRost_Test.Common;

namespace PMRost_Test.Domain.TimeEntries.Services;

public sealed class TimeEntryService : ITimeEntryService
{
    public TimeEntryCreationResult Create(
        Employee employee,
        Project project,
        DateOnly date,
        decimal hours,
        string? comment,
        IReadOnlyCollection<TimeEntry> employeeEntriesOnSameDate,
        bool isPeriodClosed,
        string createdBy)
    {
        if (isPeriodClosed)
        {
            throw PMRostTestErrors.Validation($"Период {date:yyyy-MM} закрыт для редактирования бухгалтерией");
        }

        var halfHours = TimeAmount.ToHalfHours(hours);

        if (!project.IsDateWithinProjectPeriod(date))
        {
            throw PMRostTestErrors.Validation(
                $"Дата записи {date:yyyy-MM-dd} должна попадать в период проекта {project.Number} " +
                $"({project.StartDate:yyyy-MM-dd} — {(project.EndDate.HasValue ? project.EndDate.Value.ToString("yyyy-MM-dd") : "бессрочно")})");
        }

        var rate = employee.GetRateEffectiveOn(date)
            ?? throw PMRostTestErrors.Validation(
                $"У сотрудника {employee.FullName} нет ставки, действующей на {date:yyyy-MM-dd}. Запись создать нельзя.");

        var existingHalfHours = employeeEntriesOnSameDate.Sum(e => (int)e.HalfHours);
        var totalHalfHoursForDay = existingHalfHours + halfHours;
        if (totalHalfHoursForDay > TimeAmount.MaxHalfHoursPerDay)
        {
            throw PMRostTestErrors.Validation(
                $"Суммарные часы сотрудника {employee.FullName} за {date:yyyy-MM-dd} превысят {TimeAmount.ToHours(TimeAmount.MaxHalfHoursPerDay)} " +
                $"(уже внесено {TimeAmount.ToHours((short)existingHalfHours)}, добавляется {hours})");
        }

        var isOvertime = totalHalfHoursForDay > TimeAmount.OvertimeThresholdHalfHours;

        var entry = TimeEntry.Create(employee.Id, project.Id, date, halfHours, rate, comment, isOvertime, createdBy);

        var affectedEntries = new List<TimeEntry>();
        if (isOvertime)
        {
            foreach (var existing in employeeEntriesOnSameDate.Where(e => !e.IsOvertime))
            {
                existing.SetOvertime(true);
                affectedEntries.Add(existing);
            }
        }

        return new TimeEntryCreationResult(entry, affectedEntries);
    }

    /// <summary>
    /// Пересчёт флага переработки после удаления/изменения записи - вызывать из сервиса
    /// приложения после Delete/Update, передав оставшиеся записи дня.
    /// </summary>
    public void RecalculateOvertimeForDay(IReadOnlyCollection<TimeEntry> employeeEntriesOnSameDate)
    {
        var totalHalfHours = employeeEntriesOnSameDate.Sum(e => (int)e.HalfHours);
        var isOvertime = totalHalfHours > TimeAmount.OvertimeThresholdHalfHours;

        foreach (var entry in employeeEntriesOnSameDate)
        {
            entry.SetOvertime(isOvertime);
        }
    }
}
