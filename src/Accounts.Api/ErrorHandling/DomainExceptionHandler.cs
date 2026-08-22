using System.Net;
using Accounts.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Api.ErrorHandling;

/// <summary>
/// Traduce excepciones de dominio conocidas a códigos de estado HTTP semánticamente correctos
/// (404 para recursos no encontrados, 409 para conflictos), evitando que caigan en el 500 genérico.
/// </summary>
public class DomainExceptionHandler(ILogger<DomainExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            AccountNotFoundException => (HttpStatusCode.NotFound, "Recurso no encontrado."),
            DuplicateAccountNumberException => (HttpStatusCode.Conflict, "Conflicto con el estado actual del recurso."),
            DomainException => (HttpStatusCode.BadRequest, "Regla de negocio violada."),
            _ => ((HttpStatusCode?)null, (string?)null)
        };

        if (statusCode is null)
        {
            return false;
        }

        logger.LogWarning(exception, "Excepción de dominio en {Path}", httpContext.Request.Path);

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
