using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrbansenseAPI.Domain.Exceptions;

namespace UrbansenseAPI.Infrastructure.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            WeatherNotFoundException  => (StatusCodes.Status404NotFound,              "Recurso não encontrado"),
            ExternalApiException      => (StatusCodes.Status502BadGateway,            "Serviço externo indisponível"),
            DomainException           => (StatusCodes.Status422UnprocessableEntity,   "Erro de negócio"),
            ArgumentException         => (StatusCodes.Status400BadRequest,            "Requisição inválida"),
            _                         => (StatusCodes.Status500InternalServerError,   "Erro interno do servidor")
        };

        logger.LogError(exception, "Exceção capturada: {Message}", exception.Message);

        var problem = new ProblemDetails
        {
            Status   = status,
            Title    = title,
            Detail   = exception.Message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
