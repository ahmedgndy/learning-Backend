using M04.MediaVersioningController.Data;
using M04.MediaVersioningController.Responses.V1;
using Microsoft.AspNetCore.Mvc;

namespace M04.MediaVersioningController.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
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

        Response.Headers["Deprecation"] = "true";
        Response.Headers["Sunset"] = "Wed, 31 Dec 2025 23:59:59 GMT";
        Response.Headers["Link"] = "</api/v2/products>; rel=\"successor-version\"";

        return Ok(ProductResponse.FromModel(product));
    }
}