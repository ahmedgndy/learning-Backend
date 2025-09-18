using M02.UrlQueryStringVersioningController.Data;
using M02.UrlQueryStringVersioningController.Responses.V2;
using Microsoft.AspNetCore.Mvc;

namespace M02.UrlQueryStringVersioningController.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/products")]
public class ProductController(ProductRepository repository) : ControllerBase
{
    [HttpGet("{productId}")]
    public ActionResult<ProductResponse> GetProduct(Guid productId)
    {
        var product = repository.GetProductById(productId);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(ProductResponse.FromModel(product));
    }
}