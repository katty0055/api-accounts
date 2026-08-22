using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Api.ErrorHandling;

/// <summary>
/// Manejador de excepciones "catch-all" que evita filtrar detalles internos (stack traces, mensajes de infraestructura)
/// hacia el cliente, devolviendo siempre un ProblemDetails consistente.
/// </summary>
public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Excepción no controlada en {Path}", httpContext.Request.Path);

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Ha ocurrido un error inesperado.",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Instance = httpContext.Request.Path,
            Detail = environment.IsDevelopment() ? exception.Message : "Contacte al administrador si el problema persiste."
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
