
namespace PMRost_Test.Common.Exceptions;

public interface IErrorDescription
{
    string ErrorCode { get; }
    string ShortDescription { get; }
    int StatusCode { get; }
}

public abstract class ExceptionBase(string message, int statusCode = 400)
    : Exception(message), IErrorDescription
{
    public abstract string ErrorCode { get; }
    public virtual string ShortDescription => string.Empty;
    public int StatusCode { get; } = statusCode;
}

public sealed class EntityAlreadyExistsException(string message)
    : ExceptionBase(message, statusCode: 409)
{
    public override string ErrorCode => "entity_already_exists";
}

public sealed class EntityNotFoundException(string message)
    : ExceptionBase(message, statusCode: 404)
{
    public override string ErrorCode => "entity_not_found";
}

public sealed class ValidationException(string message)
    : ExceptionBase(message, statusCode: 400)
{
    public override string ErrorCode => "bad_request";
}
