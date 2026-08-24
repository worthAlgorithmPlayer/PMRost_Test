using PMRost_Test.Common.Exceptions;

namespace PMRost_Test.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is FluentValidation.ValidationException validationEx)
        {
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

        // Проверяем, реализует ли исключение наш интерфейс IErrorDescription
        if (exception is IErrorDescription error)
        {
            context.Response.StatusCode = error.StatusCode;

            var response = new
            {
                code = error.ErrorCode,
                message = exception.Message,
                details = error.ShortDescription
            };

            return context.Response.WriteAsJsonAsync(response);
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return context.Response.WriteAsJsonAsync(new { code = "internal_server_error", message = "Произошла внутренняя ошибка сервера" });
    }
}
