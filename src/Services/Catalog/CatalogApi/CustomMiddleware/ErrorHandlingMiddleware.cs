using CatalogApi.Errors;
using CatalogApi.Exceptions;

namespace CatalogApi.CustomMiddleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = ex switch
            {
                ProductNotFoundException => new ApiResponse(404, ex.Message),
                NoChangesMadeException => new ApiResponse(200, ex.Message),
                KeyNotFoundException => new ApiResponse(404, ex.Message),
                UnauthorizedAccessException => new ApiResponse(401, "Unauthorized access."),
                _ => new ApiResponse(500, "An unexpected error occurred.")
            };

            context.Response.StatusCode = response.StatusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
    }

}
