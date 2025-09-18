using M03.ContentNegotiation.Data;
using M03.ContentNegotiation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace M03.ContentNegotiation.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(ProductRepository repository) : ControllerBase
{
    [HttpGet("{productId}")]
    [Produces("application/json", "application/xml")]
    public ActionResult<ProductResponse> GetProduct(Guid productId)
    {
        var product = repository.GetProductById(productId);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(ProductResponse.FromModel(product));
    }

    [HttpGet("products-table")]
    [Produces("text/primitives-table")]
    public IActionResult GetProductsAsTextTable()
    {
        var products = repository.GetProductsPage(1, 100);
        return Ok(products);
    }
}
