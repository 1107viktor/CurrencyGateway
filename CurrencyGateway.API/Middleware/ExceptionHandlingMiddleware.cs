using FluentValidation;

namespace CurrencyGateway.API.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            ILogger<ExceptionHandlingMiddleware> logger) =>
            _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error");
                await HandleValidationException(context, ex);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Application error");
                await HandleApplicationException(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception");
                await HandleGenericException(context, ex);
            }
        }

        private static async Task HandleValidationException(HttpContext context, ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            await context.Response.WriteAsJsonAsync(new
            {
                Title = "Validation Error",
                Status = context.Response.StatusCode,
                Errors = errors
            });
        }

        private static async Task HandleApplicationException(HttpContext context, ApplicationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsJsonAsync(new
            {
                Title = "Service Unavailable",
                Status = context.Response.StatusCode,
                Detail = ex.Message
            });
        }

        private static async Task HandleGenericException(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                Title = "Internal Server Error",
                Status = context.Response.StatusCode
            });
        }
    }
}
