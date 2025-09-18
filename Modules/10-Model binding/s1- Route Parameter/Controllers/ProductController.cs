using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class ProductController : ControllerBase
{
    [HttpGet("product-controller/{id:int}")]
    public IActionResult GetID(int id)
    {
        return Ok(id);
    }
}