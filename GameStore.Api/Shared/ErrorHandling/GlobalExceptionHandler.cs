using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace GameStore.Api.Shared.ErrorHandling
{
    public class GlobalExceptionHandler (ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken) 
        {
            var traceId = Activity.Current?.Id;
            logger.LogError(exception, "An error occurred while retrieving the game on machine {Machine} . Trace ID: {TraceId}", Environment.MachineName, traceId);
            //// Optionally log the exception here
             await Results.Problem(
                title: "An error occurred while processing your request.",
                detail: "Please try again later or contact support.",
                statusCode: StatusCodes.Status500InternalServerError,
                instance: traceId,
                extensions: new Dictionary<string, object?>
                {

                    ["traceId"] = traceId,
                    ["machineName"] = Environment.MachineName

                }

             ).ExecuteAsync(httpContext);

            return true;
        }
    }
}
