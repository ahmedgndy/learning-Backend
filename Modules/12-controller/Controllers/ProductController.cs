
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{

    [HttpGet("{id:int}")]
    public IActionResult getProduct(int id)
    {

        return Ok(new { name = "product1", id = id });
    }
}