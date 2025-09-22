using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace M02.DeveloperExceptionPage.Contollers;

public class ErrorController : ControllerBase
{

    [Route("/error")]
    public IActionResult Error() =>
    new ObjectResult(new
    {
        StatusCode = 500,
        Message = "Internal Server Error!"
    });

    [Route("/error-development")]
    public IActionResult HandleErrorDevelopment(
    [FromServices] IHostEnvironment hostEnvironment)
    {
        if (!hostEnvironment.IsDevelopment())
        {
            return NotFound();
        }

        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        return new ObjectResult(new
        {
            detail = exceptionHandlerFeature.Error.StackTrace,
            title = exceptionHandlerFeature.Error.Message
        });
    }
}