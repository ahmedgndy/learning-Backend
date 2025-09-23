


using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace M02.DeveloperExceptionPage.Controllers;

public class ErrorEndpoints : ControllerBase
{
    [Route("/error")]

    public IActionResult HandelErrorProduction() {

        var problemDetails = new ProblemDetails{
    Type = "https://example.com/errors/internal-server-error", // a URI identifying the error type
    Title = "Internal Server Error",                           // short summary
    Status = StatusCodes.Status500InternalServerError,         // correct status code constant
    Detail = "An unexpected error occurred.",                  // human-readable details
    Instance = HttpContext.Request.Path                        // the request path that caused the error
};

return new ObjectResult(problemDetails)
{
    StatusCode = problemDetails.Status
};
    }
    


    [Route("/error-Development")]
    public IActionResult HandelErrorDevelopment(IHostEnvironment host)
    {
        if (!host.IsDevelopment())
        {
            return NotFound();
        }
        var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        var problemDetails = new ProblemDetails{
                Type = "https://example.com/errors/internal-server-error", // a URI identifying the error type
                Title = "Internal Server Error",                           // short summary
                Status = StatusCodes.Status500InternalServerError,         // correct status code constant
                 Detail = exceptionHandler.Error.StackTrace,                  // human-readable details
                Instance = HttpContext.Request.Path                        // the request path that caused the error
};
        
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };

     }
   
}