namespace InvoiceHub.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(
                    ex,
                    "Error procesando {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var statusCode = exception switch
            {
                ArgumentException =>
                    StatusCodes.Status400BadRequest,

                KeyNotFoundException =>
                    StatusCodes.Status404NotFound,

                _ =>
                    StatusCodes.Status500InternalServerError
            };

            var message =
                statusCode == StatusCodes.Status500InternalServerError
                    ? "Ocurrió un error interno en el servidor."
                    : exception.Message;

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                message,
                statusCode
            });
        }
    }
}