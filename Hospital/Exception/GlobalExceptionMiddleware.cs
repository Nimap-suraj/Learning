using System.Net;

public class GlobalExceptionMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware>? _logger;
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next,ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); // Call next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError;
        var response = new
        {
            success = true,
            Message = ex.Message,
            Detail = "An unexpected error occurred. Please contact support.",
            ErrorCode = "ERR-500"
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return  context.Response.WriteAsJsonAsync(response);
    }

}
