
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
[Route("api/products")]

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