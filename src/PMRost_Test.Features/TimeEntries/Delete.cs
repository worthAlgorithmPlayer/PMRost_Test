
using Mediator;
using MongoDB.Driver;
using PMRost_Test.Common;
using PMRost_Test.DAL.Mongo;
using PMRost_Test.Domain.TimeEntries;
using PMRost_Test.Domain.TimeEntries.Services;

namespace PMRost_Test.Features.TimeEntries;

public sealed class DeleteTimeEntryCommandHandler : ICommandHandler<DeleteTimeEntryCommand>
{
    private readonly PMRostTestContextMongo _dbContext;
    private readonly ITimeEntryService _timeEntryService;
    public DeleteTimeEntryCommandHandler(PMRostTestContextMongo dbContext,
        ITimeEntryService timeEntryService)
    {
        _dbContext = dbContext;
        _timeEntryService = timeEntryService;
    }

    public async ValueTask<Unit> Handle(DeleteTimeEntryCommand command, CancellationToken cancellationToken)
    {
        var entry = await _dbContext.TimeEntries.Find(x => x.Id == command.Id).FirstOrDefaultAsync(cancellationToken)
            ?? throw PMRostTestErrors.NotFound<TimeEntry>(command.Id);

        var isPeriodClosed = await _dbContext.ClosedPeriods
            .Find(x => x.Year == entry.TimesheetDate.Year && x.Month == entry.TimesheetDate.Month)
            .AnyAsync(cancellationToken);

        if (isPeriodClosed)
        {
            throw PMRostTestErrors.Validation($"Период {entry.TimesheetDate:yyyy-MM} закрыт для редактирования бухгалтерией"); ;
        }

        var otherEntriesOnSameDate = await _dbContext.TimeEntries
            .Find(x => x.EmployeeId == entry.EmployeeId
                    && x.TimesheetDate == entry.TimesheetDate
                    && x.Id != command.Id)
            .ToListAsync(cancellationToken);

        var writes = new List<WriteModel<TimeEntry>>
        {
            new DeleteOneModel<TimeEntry>(Builders<TimeEntry>.Filter.Eq(x => x.Id, command.Id))
        };

        if (otherEntriesOnSameDate.Count > 0)
        {
            _timeEntryService.RecalculateOvertimeForDay(otherEntriesOnSameDate);

            writes.AddRange(otherEntriesOnSameDate.Select(remaining =>
                (WriteModel<TimeEntry>)new ReplaceOneModel<TimeEntry>(
                    Builders<TimeEntry>.Filter.Eq(x => x.Id, remaining.Id),
                    remaining)));
        }

        await _dbContext.TimeEntries.BulkWriteAsync(writes, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}

public record DeleteTimeEntryCommand(Guid Id) : ICommand;
