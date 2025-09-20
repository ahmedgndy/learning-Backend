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
