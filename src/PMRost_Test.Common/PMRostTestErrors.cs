
using PMRost_Test.Common.Exceptions;
using PMRost_Test.Domain.Primitives.EntityAnnotations;

namespace PMRost_Test.Common;

public static class PMRostTestErrors
{
    public static EntityNotFoundException NotFound<TEntity>(Guid id) where TEntity : IEntity =>
        new($"Сущность: {typeof(TEntity).Name} c id = {id} не найдена");

    public static EntityNotFoundException NotFound(string message) =>
        new(message);

    public static ValidationException Validation(string message) =>
        new(message);

    public static EntityAlreadyExistsException PeriodAlreadyClosed(int year, int month) =>
        new($"Закрытый период за {month}/{year} уже существует.");
}
