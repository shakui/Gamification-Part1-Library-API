using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Errors;

/// <summary>
/// Превращает исключения библиотеки в единообразные ответы ProblemDetails:
/// KeyNotFoundException → 404, ArgumentException → 400, всё остальное → 500.
/// </summary>
public sealed class DomainExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetails;
    private readonly ILogger<DomainExceptionHandler> _logger;

    /// <summary>Создаёт обработчик.</summary>
    public DomainExceptionHandler(IProblemDetailsService problemDetails, ILogger<DomainExceptionHandler> logger)
    {
        _problemDetails = problemDetails;
        _logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
                                                CancellationToken cancellationToken)
    {
        var (status, title, detail) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", (string?)null)
        };

        if (status == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Необработанная ошибка");
        else
            _logger.LogWarning("Ошибка запроса {Status}: {Message}", status, exception.Message);

        httpContext.Response.StatusCode = status;
        return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails { Status = status, Title = title, Detail = detail }
        });
    }
}
