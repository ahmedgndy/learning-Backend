

using Microsoft.AspNetCore.Mvc.Filters;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
class TrackACtionTimeFilter : Attribute,IAsyncActionFilter
{

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //before handler
        context.HttpContext.Items["ActionTimeStarted"] = DateTime.UtcNow;
        await next(); //handler implementation  

        //after Handler
        var startedTime = (DateTime)context.HttpContext.Items["ActionTimeStarted"]!;
        var elapsed = startedTime - DateTime.UtcNow;
        context.HttpContext.Response.Headers["TakenTime"] = $"{elapsed.TotalMilliseconds}";
    }
}