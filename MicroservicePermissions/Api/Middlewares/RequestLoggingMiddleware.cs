namespace MicroservicePermissions.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            // Nombre de la operación: método + ruta
            var operation = $"{request.Method} {request.Path}";

            _logger.LogInformation("Iniciando operación: {Operation}", operation);

            await _next(context);

            _logger.LogInformation("Finalizó operación: {Operation} con código {StatusCode}",
                operation, context.Response.StatusCode);
        }
    }
}
