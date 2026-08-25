
using FluentValidation;
using Mediator;
using MongoDB.Driver;
using PMRost_Test.Common;
using PMRost_Test.DAL.Mongo;
using PMRost_Test.Domain.TimeEntries;
using PMRost_Test.Domain.TimeEntries.Services;

namespace PMRost_Test.Features.TimeEntries;

public sealed class UpdateTimeEntryCommandHandler : ICommandHandler<UpdateTimeEntryCommand>
{
    private readonly PMRostTestContextMongo _dbContext;
    private readonly ITimeEntryService _timeEntryService;
    private readonly IValidator<UpdateTimeEntryCommand> _validator;

    public UpdateTimeEntryCommandHandler(
        PMRostTestContextMongo dbContext,
        ITimeEntryService timeEntryService,
        IValidator<UpdateTimeEntryCommand> validator)
    {
        _dbContext = dbContext;
        _timeEntryService = timeEntryService;
        _validator = validator;
    }

    public async ValueTask<Unit> Handle(UpdateTimeEntryCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var entryStub = await _dbContext.TimeEntries.Find(x => x.Id == command.Id).FirstOrDefaultAsync(cancellationToken)
            ?? throw PMRostTestErrors.NotFound<TimeEntry>(command.Id);

        if (entryStub.Version != command.Version)
        {
            throw PMRostTestErrors.Validation("Требуется обновить данные");
        }

        var isPeriodClosed = await _dbContext.ClosedPeriods
            .Find(x => x.Year == entryStub.TimesheetDate.Year && x.Month == entryStub.TimesheetDate.Month)
            .AnyAsync(cancellationToken);

        if (isPeriodClosed)
        {
            throw PMRostTestErrors.Validation($"Период {entryStub.TimesheetDate:yyyy-MM} закрыт для редактирования бухгалтерией");;
        }

        var dayEntries = await _dbContext.TimeEntries
            .Find(x => x.EmployeeId == entryStub.EmployeeId && x.TimesheetDate == entryStub.TimesheetDate)
            .ToListAsync(cancellationToken);

        var entry = dayEntries.First(x => x.Id == command.Id);

        var originalVersions = dayEntries.ToDictionary(e => e.Id, e => e.Version);

        entry.Update(command.Hours, command.Comment);
        _timeEntryService.RecalculateOvertimeForDay(dayEntries);

        var changedEntries = dayEntries.Where(e => e.Version != originalVersions[e.Id]).ToList();

        var writes = changedEntries
        .Select(e => (WriteModel<TimeEntry>)new ReplaceOneModel<TimeEntry>(
            Builders<TimeEntry>.Filter.Where(x => x.Id == e.Id && x.Version == originalVersions[e.Id]),
            e))
        .ToList();

        var result = await _dbContext.TimeEntries.BulkWriteAsync(writes, cancellationToken: cancellationToken);

        if (result.MatchedCount != writes.Count)
        {
            throw PMRostTestErrors.Validation("Требуется обновить данные");
        }

        return Unit.Value;
    }
}

public record UpdateTimeEntryCommand(Guid Id,
    decimal Hours,
    string? Comment,
    int Version) : ICommand;