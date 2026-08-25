
using FluentValidation;
using Mediator;
using MongoDB.Bson;
using MongoDB.Driver;
using PMRost_Test.Common;
using PMRost_Test.DAL.Mongo;
using PMRost_Test.Domain;
using PMRost_Test.Domain.TimeEntries;
using PMRost_Test.Domain.TimeEntries.Services;

namespace PMRost_Test.Features.TimeEntries;

public sealed class CreateTimeEntryCommandHandler : ICommandHandler<CreateTimeEntryCommand, Guid>
{
    private readonly PMRostTestContextMongo _dbContext;
    private readonly ITimeEntryService _timeEntryService;
    private readonly IValidator<CreateTimeEntryCommand> _validator;

    public CreateTimeEntryCommandHandler(
        PMRostTestContextMongo dbContext,
        ITimeEntryService timeEntryService,
        IValidator<CreateTimeEntryCommand> validator)
    {
        _dbContext = dbContext;
        _timeEntryService = timeEntryService;
        _validator = validator;
    }

    public async ValueTask<Guid> Handle(CreateTimeEntryCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var employee = await _dbContext.Employees.Find(x => x.Id == command.EmployeeId).FirstOrDefaultAsync(cancellationToken)
            ?? throw PMRostTestErrors.NotFound<Employee>(command.EmployeeId);

        var project = await _dbContext.Projects.Find(x => x.Id == command.ProjectId).FirstOrDefaultAsync(cancellationToken)
            ?? throw PMRostTestErrors.NotFound<Project>(command.ProjectId);

        var isPeriodClosed = await _dbContext.ClosedPeriods
            .Find(x => x.Year == command.TimesheetDate.Year && x.Month == command.TimesheetDate.Month)
            .AnyAsync(cancellationToken);

        var existingEntries = await _dbContext.TimeEntries
            .Find(x => x.EmployeeId == command.EmployeeId && x.TimesheetDate == command.TimesheetDate)
            .ToListAsync(cancellationToken);

        var rawBson = await _dbContext.Employees
            .Find(Builders<Employee>.Filter.Eq(x => x.Id, command.EmployeeId))
            .As<MongoDB.Bson.BsonDocument>()
            .FirstOrDefaultAsync(cancellationToken);

        var json = rawBson.ToJson(new MongoDB.Bson.IO.JsonWriterSettings { Indent = true });
        Console.WriteLine(json);

        var result = _timeEntryService.Create(
            employee,
            project,
            command.TimesheetDate,
            command.Hours,
            command.Comment,
            existingEntries,
            isPeriodClosed,
            createdBy: "SystemUser");

        var writes = new List<WriteModel<TimeEntry>>
        {
            new InsertOneModel<TimeEntry>(result.Entry)
        };

        foreach (var affected in result.EntriesMarkedOvertime)
        {
            var filter = Builders<TimeEntry>.Filter.Eq(x => x.Id, affected.Id);
            writes.Add(new ReplaceOneModel<TimeEntry>(filter, affected));
        }

        await _dbContext.TimeEntries.BulkWriteAsync(writes, cancellationToken: cancellationToken);

        return result.Entry.Id;
    }
}

public record CreateTimeEntryCommand(Guid EmployeeId,
    Guid ProjectId,
    DateOnly TimesheetDate,
    decimal Hours,
    string? Comment) : ICommand<Guid>;