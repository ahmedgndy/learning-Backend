using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace M01_Implementing_ActionFilters.Filters;

    public class EnvelopResultFilter : IAsyncResourceFilter
    {
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {

        if (context.Result is ObjectResult objectResult && objectResult is not null)
        {
            var wrapped = new
            {
                success = true,
                data = objectResult.Value
            };

            context.Result = new JsonResult(wrapped)
            {
                StatusCode = objectResult.StatusCode
            };
        }
        await next();   

    }
    }
