
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PMRost_Test.Application.Contracts.Reports;
using PMRost_Test.Common;
using PMRost_Test.Domain;
using PMRost_Test.Domain.TimeEntries;

namespace PMRost_Test.Features.Reports;

public sealed class GetMonthlyProjectReportQueryHandler : IQueryHandler<GetMonthlyProjectReportQuery, MonthlyProjectReportResult>
{
    private readonly IMongoDatabase _db;
    private readonly ILogger<GetMonthlyProjectReportQueryHandler> _logger;
    private readonly IValidator<GetMonthlyProjectReportQuery> _validator;
    public GetMonthlyProjectReportQueryHandler(IMongoDatabase db,
        ILogger<GetMonthlyProjectReportQueryHandler> logger,
        IValidator<GetMonthlyProjectReportQuery> validator)
    {
        _db = db;
        _logger = logger;
        _validator = validator;
    }
    public async ValueTask<MonthlyProjectReportResult> Handle(GetMonthlyProjectReportQuery query, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);

        var startDate = new DateOnly(query.Year, query.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(query.Year, query.Month);
        var endDate = new DateOnly(query.Year, query.Month, daysInMonth);

        var monthEntries = await _db.GetCollection<TimeEntry>("time_entries")
            .AsQueryable()
            .Where(e => e.TimesheetDate >= startDate && e.TimesheetDate <= endDate)
            .ToListAsync(cancellationToken);

        if (monthEntries.Count == 0)
        {
            return new MonthlyProjectReportResult();
        }

        var employeeIds = monthEntries.Select(e => e.EmployeeId).Distinct().ToList();
        var projectIds = monthEntries.Select(e => e.ProjectId).Distinct().ToList();

        var employees = await _db.GetCollection<Employee>("employees")
            .Find(e => employeeIds.Contains(e.Id))
            .ToListAsync(cancellationToken);

        var projects = await _db.GetCollection<Project>("projects")
            .Find(p => projectIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        var employeesDict = employees.ToDictionary(e => e.Id);
        var projectsDict = projects.ToDictionary(p => p.Id);

        var rowsMap = new Dictionary<Guid, ProjectReportRow>();

        foreach (var entry in monthEntries)
        {
            if (!projectsDict.TryGetValue(entry.ProjectId, out var project))
            {
                _logger.LogWarning(
                    "Time entry {EntryId} references unknown project {ProjectId}, skipped from report",
                    entry.Id, entry.ProjectId);
                continue;
            }

            if (!employeesDict.TryGetValue(entry.EmployeeId, out var employee))
            {
                throw PMRostTestErrors.NotFound<Employee>(entry.EmployeeId);
            }

            var rate = employee.GetRateEffectiveOn(entry.TimesheetDate);
            if (rate == null)
            {
                throw PMRostTestErrors.Validation(
                    $"Не найдена ставка для пользователя: '{employee.Id}' на дату: {entry.TimesheetDate}.");
            }

            var hours = (decimal)entry.HalfHours / 2m;
            var cost = hours * rate.Value;

            if (!rowsMap.TryGetValue(entry.ProjectId, out var row))
            {
                row = new ProjectReportRow
                {
                    ProjectId = project.Id,
                    ProjectNumber = project.Number,
                    ProjectName = project.Name,
                    Budget = project.Budget,
                    TotalHours = 0m,
                    TotalCost = 0m
                };
                rowsMap[project.Id] = row;
            }

            row.TotalHours += hours;
            row.TotalCost += cost;
        }

        var rows = rowsMap.Values
            .OrderBy(r => r.ProjectNumber)
            .ToList();

        return new MonthlyProjectReportResult
        {
            Rows = rows,
            GrandTotalHours = rows.Sum(r => r.TotalHours),
            GrandTotalCost = rows.Sum(r => r.TotalCost),
            GrandTotalBudget = rows.Sum(r => r.Budget)
        };
    }
}

public record GetMonthlyProjectReportQuery(int Year, int Month) : IQuery<MonthlyProjectReportResult>;
