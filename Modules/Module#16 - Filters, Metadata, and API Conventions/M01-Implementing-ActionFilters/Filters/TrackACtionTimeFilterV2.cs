using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace M01_Implementing_ActionFilters.Filters;

    class TrackACtionTimeFilterV2 : IAsyncActionFilter
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



public class AdminOnlyFilter : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var isAdmin = context.HttpContext.User.IsInRole("Admin");
        if (!isAdmin)
        {
            context.Result = new ForbidResult();

        }

        return Task.CompletedTask;

    }

}

public class Tenanvalidationfilter(IConfiguration config) : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var TenatId = context.HttpContext.Request.Headers["ApiKey"].ToString();
        var Apikey = context.HttpContext.Request.Headers["x-apkiey"].ToString();
        var UserId = config[$"Tenants:{TenatId}:{Apikey}"];

        if (String.IsNullOrWhiteSpace(UserId) || UserId != Apikey)
        {
            context.Result = new UnauthorizedResult();
            return;

        }
        await next();

    }


}

public class ElapsedTimeTrackerFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Items["StartedTime"] = DateTime.UtcNow;
        await next();
        var elapsed = (DateTime)context.HttpContext.Items["StartedTime"] - DateTime.UtcNow;


        context.HttpContext.Response.Headers.Append("x-Elapsed-Time", $"{elapsed.TotalMilliseconds} ms");
    }
}

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Title = "Internal Server Error",
            Detail = context.Exception.Message,
            Status = StatusCodes.Status500InternalServerError,
        };
        context.Result = new ObjectResult(details)
        {
            StatusCode = details.Status
        };
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}