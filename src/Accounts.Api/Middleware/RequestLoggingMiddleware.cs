using System.Diagnostics;
using FluentValidation;

namespace Accounts.Api.Middleware;

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app) =>
        app.Use(async (context, next) =>
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger("Accounts.Api.Middleware.RequestLogging");

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await next(context);
                stopwatch.Stop();

                // Forma correcta de loguear: placeholders con nombre (structured logging),
                // nunca interpolación de strings ($"..."). Así Seq puede indexar y filtrar
                // por cada propiedad (Method, Path, StatusCode, etc.) en vez de tratar todo
                // el mensaje como texto plano.
                logger.LogInformation(
                    "HTTP {Method} {Path} respondió {StatusCode} en {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
            catch (ValidationException ex)
            {
                stopwatch.Stop();

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                logger.LogWarning(
                    "HTTP {Method} {Path} falló por validación en {ElapsedMilliseconds} ms: {@ValidationErrors}",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds,
                    errors);

                throw;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                logger.LogError(
                    ex,
                    "HTTP {Method} {Path} lanzó una excepción no controlada en {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds);

                throw;
            }
        });
}
