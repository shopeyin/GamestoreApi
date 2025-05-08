using System.Diagnostics;

namespace GameStore.Api.Shared.Timing
{
    public class RequestTimingMiddleware (RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await next(context);
            stopwatch.Stop();

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            var path = context.Request.Path;
            var method = context.Request.Method;
            var statusCode = context.Response.StatusCode;
            var logMessage = $"[{DateTime.UtcNow}] {method} {path} responded {statusCode} in {elapsedMilliseconds}ms";

            logger.LogInformation($"[{DateTime.UtcNow}] {method} {path} responded {statusCode} in {elapsedMilliseconds}ms");
        }
    }
}
