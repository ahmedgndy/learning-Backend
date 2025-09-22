


using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace M02.DeveloperExceptionPage.Controllers;

public class ErrorEndpoints : ControllerBase
{
    [Route("/error")]

    public IActionResult HandelErrorProduction() => new ObjectResult(new
    {
        StatusCode = 500,
        Message = "Internal Server Error"
    });


    [Route("/error-Development")]
    public IActionResult HandelErrorDevelopment(IHostEnvironment host)
    {
        if (!host.IsDevelopment())
        {
            return NotFound();
        }
        var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
        
        return new ObjectResult(new
        {
            detail = exceptionHandler.Error.StackTrace,

            title = exceptionHandler.Error.Message

        });

     }
   
}