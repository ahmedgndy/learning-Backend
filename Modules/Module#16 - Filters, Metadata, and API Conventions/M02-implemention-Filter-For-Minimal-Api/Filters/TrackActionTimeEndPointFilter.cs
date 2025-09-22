
namespace M02_implemention_Filter_For_Minimal_Api.Filters;

public class TrackActionTimeEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var start = DateTime.UtcNow;
        context.HttpContext.Items["Started"] = start;
        
        var request = await next(context);

        var elapsed = (DateTime)context.HttpContext.Items["Started"] - DateTime.UtcNow;

        context.HttpContext.Response.Headers.Append("x-request-proccess-Time", $"{elapsed}");

        return request;
    }
}