
using System;
using System.Diagnostics.Contracts;
using System.Text;
using M01_urlPathVersioningController.Data;
using M01_urlPathVersioningController.Response.v1;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;

namespace M01_urlPathVersioningController.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/products")] //old  //
[Route("api/v{Version:apiVersion}/products")] //support new 

public class ProductController(ProductRepository repository) : ControllerBase
{
    [HttpGet("{productId}")]
    public ActionResult<ProductResponse> GetProduct(Guid productId)
    {

        var product = repository.GetProductById(productId);
        if (product is null)
            return NotFound();

        Response.Headers["Deprecation"] = "true";

        // Tell clients when this API will be removed (RFC 8594 recommends a date)
        Response.Headers["Sunset"] = "Wed, 11 Nov 2025 23:59:59 GMT";

        // Example of pointing to the new API version using the Link header
        Response.Headers["Link"] = "</api/v2/products>; rel=\"successor-version\"";

        return Ok(ProductResponse.FromModel(product));
    }

}