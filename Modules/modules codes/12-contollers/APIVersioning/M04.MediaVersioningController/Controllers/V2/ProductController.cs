using M04.MediaVersioningController.Data;
using M04.MediaVersioningController.Responses.V2;
using Microsoft.AspNetCore.Mvc;

namespace M04.MediaVersioningController.Controllers.V2;

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