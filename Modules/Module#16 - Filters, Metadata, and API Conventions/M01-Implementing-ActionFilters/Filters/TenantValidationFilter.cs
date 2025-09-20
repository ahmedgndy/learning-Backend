

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class TenantValidationFilter(IConfiguration config) : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var tenantID = context.HttpContext.Request.Headers["TenatID"].ToString();
        var apiKey = context.HttpContext.Request.Headers["x-api-key"].ToString();

        var expectedKey = config[$"Tenants:{tenantID}:apiKey"];

        if (String.IsNullOrEmpty(expectedKey) || expectedKey != apiKey)
        {
            context.Result = new UnauthorizedResult();
            return;

        }
        await next();
    }
}