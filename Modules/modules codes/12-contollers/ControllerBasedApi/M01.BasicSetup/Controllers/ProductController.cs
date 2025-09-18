using Microsoft.AspNetCore.Mvc;

namespace  M01.BasicSetup.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController() : ControllerBase
{     
    
    [HttpGet]
    public string Get()
    {
        return "Product #1, price $2.99";
    }
} 

