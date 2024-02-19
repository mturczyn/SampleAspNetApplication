using System.Net;

namespace Intrinsic.WebApi.ExampleApp.ProblemDetails;

public class ExceptionToProblemDetailsHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await httpContext.Response.WriteAsJsonAsync(new
        {
            Title = "ExceptionToProblemDetailsHandler: An error occurred",
            Detail = exception.Message,
            Type = exception.GetType().Name,
            Status = (int)HttpStatusCode.BadRequest
        }, cancellationToken: cancellationToken);

        return true;
    }
}
