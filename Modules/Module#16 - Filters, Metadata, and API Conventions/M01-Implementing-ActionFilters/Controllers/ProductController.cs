using Microsoft.AspNetCore.Mvc;

namespace M01_Implementing_ActionFilters.Controllers;

[ApiController]
[Route("products")]
[TrackACtionTimeFilter]
public class ProductController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok( new[] { "keyboard[$52.99]", "Mouse, [$733,3]" });


}