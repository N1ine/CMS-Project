using System.Net;
using System.Text.Json;
using Domain.Exceptions;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        ILogger logger)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        string errorCode = "internal_error";

        switch (exception)
        {
            case ValidationException:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "validation_error";
                break;

            case EntityNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorCode = "not_found";
                break;

            case DomainException:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "domain_error";
                break;

            case FluentValidation.ValidationException fvEx:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "validation_error";

                var validationResponse = new
                {
                    error = errorCode,
                    message = "Validation failed",
                    details = fvEx.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(validationResponse));
                return;
        }

        logger.LogError(exception, "Unhandled exception");

        var response = new
        {
            error = errorCode,
            message = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}