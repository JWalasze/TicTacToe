namespace webapi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlingMiddleware(RequestDelegate requestDelegate, ILoggerFactory loggerFactory)
    {
        _next = requestDelegate;
        _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            _logger.LogError(ex.ToString());
            await httpContext.Response.WriteAsync(ex.ToString());
        }
    }
}