

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PMRost_Test.Application.Contracts;
using PMRost_Test.Application.Contracts.Repositories;
using PMRost_Test.Application.Contracts.TimeEntries;
using PMRost_Test.DAL.Mongo;

namespace PMRost_Test.DAL.Repositories.Application;

internal sealed class TimeEntryReadRepository : ITimeEntryReadRepository
{
    private readonly PMRostTestContextMongo _dbContext;

    public TimeEntryReadRepository(PMRostTestContextMongo dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ModelPagedResult<TimeEntryModel>> GetAllAsync(TimeEntryFilter filter, CancellationToken cancellationToken) 
    {
        var query = _dbContext.TimeEntries.AsQueryable();

        if (filter.Year.HasValue && filter.Month.HasValue)
        {
            var startDate = new DateOnly(filter.Year.Value, filter.Month.Value, 1);
            var daysInMonth = DateTime.DaysInMonth(filter.Year.Value, filter.Month.Value);
            var endDate = new DateOnly(filter.Year.Value, filter.Month.Value, daysInMonth);

            query = query.Where(x => x.TimesheetDate >= startDate && x.TimesheetDate <= endDate);
        }

        if (filter.EmployeeId.HasValue)
        {
            query = query.Where(x => x.EmployeeId == filter.EmployeeId.Value);
        }

        if (filter.ProjectId.HasValue)
        {
            query = query.Where(x => x.ProjectId == filter.ProjectId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Join(_dbContext.Employees,
                timeEntry => timeEntry.EmployeeId,
                employee => employee.Id,
                (timeEntry, employee) => new { timeEntry, employee })
            .Join(_dbContext.Projects,
                combined => combined.timeEntry.ProjectId,
                project => project.Id,
                (combined, project) => new
                {
                    Id = combined.timeEntry.Id,
                    EmployeeName = combined.employee.FullName,
                    ProjectNumber = project.Number,
                    TimesheetDate = combined.timeEntry.TimesheetDate,
                    Comment = combined.timeEntry.Comment,
                    Hours = (decimal)combined.timeEntry.HalfHours / 2m,
                    Rate = combined.timeEntry.RateApplied,
                    Price = ((decimal)combined.timeEntry.HalfHours / 2m) * combined.timeEntry.RateApplied,
                    Version = combined.timeEntry.Version,
                    IsOvertime = combined.timeEntry.IsOvertime
                })
            .OrderByDescending(x => x.TimesheetDate)
            .Skip(filter.Skip)
            .Take(filter.Limit)
            .Select(x => new TimeEntryModel
            {
                Id = x.Id,
                EmployeeName = x.EmployeeName,
                ProjectNumber = x.ProjectNumber,
                TimeSheetDate = x.TimesheetDate,
                Comment = x.Comment,
                Hours = x.Hours,
                Rate = x.Rate,
                Price = x.Price,
                Version = x.Version,
                IsOvertime = x.IsOvertime
            })
            .ToListAsync(cancellationToken);

        var result = new ModelPagedResult<TimeEntryModel>
        {
            TotalCount = totalCount,
            Items = items,
        };

        return result;
    }
}
