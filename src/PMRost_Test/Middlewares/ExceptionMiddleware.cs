using PMRost_Test.Common.Exceptions;
using PMRost_Test.Domain.TimeEntries.Services;

namespace PMRost_Test.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionMiddleware> logger)
    {
        context.Response.ContentType = "application/json";

        if (exception is FluentValidation.ValidationException validationEx)
        {
            logger.LogWarning("Ошибка валидации запроса {Path}: {Errors}",
                context.Request.Path,
                string.Join("; ", validationEx.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = validationEx.Errors.Select(e => new
            {
                field = e.PropertyName,
                message = e.ErrorMessage
            });

            var response = new
            {
                code = "validation_error",
                message = "Ошибка валидации входных данных",
                errors = errors
            };

            return context.Response.WriteAsJsonAsync(response);
        }

        if (exception is IErrorDescription error)
        {
            logger.LogWarning(exception, "Обработана ошибка приложения [{ErrorCode}]: {Message}", error.ErrorCode, exception.Message);

            context.Response.StatusCode = error.StatusCode;

            var response = new
            {
                code = error.ErrorCode,
                message = exception.Message,
                details = error.ShortDescription
            };

            return context.Response.WriteAsJsonAsync(response);
        }

        logger.LogError(exception, "Необработанное исключение при выполнении запроса {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return context.Response.WriteAsJsonAsync(new { code = "internal_server_error", message = "Произошла внутренняя ошибка сервера" });
    }
}
