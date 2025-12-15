using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ExpenseTracker.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await WriteProblemDetailsAsync(context, ex, _env);
        }
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, Exception ex, IHostEnvironment env)
    {
        // If something wraps your real exception, unwrap it.
        var root = ex.GetBaseException();

        var (statusCode, title) = root switch
        {
            ArgumentException => ((int)HttpStatusCode.BadRequest, "Validation error"),
            InvalidOperationException => ((int)HttpStatusCode.Conflict, "Conflict"),
            _ => ((int)HttpStatusCode.InternalServerError, "Server error")
        };

        var detail =
            statusCode == 500
                ? (env.IsDevelopment()
                    ? $"{root.GetType().Name}: {root.Message}"
                    : "An unexpected error occurred.")
                : root.Message;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(problem);
    }
}
