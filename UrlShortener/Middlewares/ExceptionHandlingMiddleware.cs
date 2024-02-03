using System.Text.Json;

namespace UrlShortener.Middlewares;

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

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
            Logger.Log(exception.Message);

            // Customize the error response based on the exception
            var response = new { error = exception.Message };

            // Set the response status code and content type
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            // Serialize the response object to JSON
            var jsonResponse = JsonSerializer.Serialize(response);

            // Write the JSON response to the client
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}

