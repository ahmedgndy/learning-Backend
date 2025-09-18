using M01.UrlPathVersioningController.Data;
using M01.UrlPathVersioningController.Responses.V2;
using Microsoft.AspNetCore.Mvc;

namespace M01.UrlPathVersioningController.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/products")]
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