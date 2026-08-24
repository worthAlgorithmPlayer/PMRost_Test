
using FluentValidation;
using Mediator;
using MongoDB.Driver;
using PMRost_Test.Common;
using PMRost_Test.DAL.Mongo;
using PMRost_Test.Domain;

namespace PMRost_Test.Features.ClosedPeriods;

public sealed class AddClosedPeriodCommandHandler : ICommandHandler<AddClosedPeriodCommand>
{
    private readonly PMRostTestContextMongo _dbContext;
    private readonly IValidator<AddClosedPeriodCommand> _validator;

    public AddClosedPeriodCommandHandler(PMRostTestContextMongo dbContext,
        IValidator<AddClosedPeriodCommand> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async ValueTask<Unit> Handle(AddClosedPeriodCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var closedPeriod = ClosedPeriod.Create(command.Year, command.Month);

        try
        {
            await _dbContext.ClosedPeriods.InsertOneAsync(closedPeriod, cancellationToken: cancellationToken);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            throw PMRostTestErrors.PeriodAlreadyClosed(command.Year, command.Month);
        }
        return Unit.Value;
    }
}

public record AddClosedPeriodCommand(int Year, int Month) : ICommand;