using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
    {
        private readonly ILogger _logger = logger;
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid().ToString();
                _logger.LogError(ex, $"{errorId}:{ex.Message}");

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "https://http.cat/500", // HTTP Cats URL for 500 Internal Server Error
                    Title = "Internal Server Error",
                    Detail = "Something went wrong! We are looking into resolving this.",
                    Instance = $"urn:uuid:{errorId}"
                };

                var json = JsonSerializer.Serialize(error);
                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
