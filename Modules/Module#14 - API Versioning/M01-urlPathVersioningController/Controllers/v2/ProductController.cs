
using System;
using System.Diagnostics.Contracts;
using System.Text;
using M01_urlPathVersioningController.Data;
using M01_urlPathVersioningController.Response.v2;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;

namespace M01_urlPathVersioningController.Controllers.V2;


[ApiController]
[ApiVersion("2.0")]
[Route("api/v{Version:apiVersion}/products")] //supported new version only


public class ProductController(ProductRepository repository) : ControllerBase
{
    [HttpGet("{productId}")]
    public ActionResult<ProductResponse> GetProduct(Guid productId)
    {

        var product = repository.GetProductById(productId);
        if (product is null)
            return NotFound();
        return Ok(ProductResponse.FromModel(product));
    }

}