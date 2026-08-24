
using FluentValidation;
using Mediator;
using MongoDB.Driver;
using PMRost_Test.Common;
using PMRost_Test.DAL.Mongo;

namespace PMRost_Test.Features.ClosedPeriods;

public sealed class DeleteClosedPeriodCommandHandler : ICommandHandler<DeleteClosedPeriodCommand>
{
    private readonly PMRostTestContextMongo _dbContext;
    private readonly IValidator<DeleteClosedPeriodCommand> _validator;
    public DeleteClosedPeriodCommandHandler(PMRostTestContextMongo dbContext,
        IValidator<DeleteClosedPeriodCommand> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async ValueTask<Unit> Handle(DeleteClosedPeriodCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var result = await _dbContext.ClosedPeriods.DeleteOneAsync(
            x => x.Year == command.Year && x.Month == command.Month,
            cancellationToken: cancellationToken);

        if (result.DeletedCount == 0)
        {
            throw PMRostTestErrors.NotFound($"Период: {command.Year}, {command.Month}, не найден");
        }
        return Unit.Value;
    }
}

public record DeleteClosedPeriodCommand(int Year, int Month) : ICommand;