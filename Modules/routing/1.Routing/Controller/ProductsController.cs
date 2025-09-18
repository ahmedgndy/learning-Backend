using Microsoft.AspNetCore.Mvc;

namespace Routing.Controller;

[ApiController]
[Route("Products")]// ../Products

public class ProductsController : ControllerBase
{   
    // ../products/all
    [HttpGet("all")] //get request for products
    public IActionResult GetProducts()
    { 

        return Ok(new [] { "Product1", "Product2", "Product3" });
    }
    
}