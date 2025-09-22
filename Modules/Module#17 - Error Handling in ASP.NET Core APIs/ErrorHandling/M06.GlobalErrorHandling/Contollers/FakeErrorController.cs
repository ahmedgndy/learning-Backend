using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace M06.GlobalErrorHandling.Contollers;

[Route("api/controller-fake-errors")]
[ApiController]
public class FakeErrorController : ControllerBase
{
    [HttpGet("server-error")]
    public IActionResult ServerErrorExample()
    {
        System.IO.File.ReadAllText(@"C:\Settings\SomeSettings.json"); // not exist

        return Ok();
    }

    [HttpPost("bad-request")]
    public IActionResult BadRequestExample() => Problem(
        type: "http://example.com/prop/sku-required",
        title: HttpStatusCode.BadRequest.ToString(),
        statusCode: StatusCodes.Status400BadRequest,
        detail: "Product SKU is required"
    );


    [HttpPost("bad-request-no-body")]
    public IActionResult BadRequestExampleNoBody() => BadRequest();


    [HttpPost("not-found")]
    public IActionResult NotFoundExample() => Problem(
        type: "http://example.com/prop/product-not-found",
        title: HttpStatusCode.NotFound.ToString(),
        statusCode: StatusCodes.Status404NotFound,
        detail: "Product not found."
    );

    [HttpPost("unauthorized")]
    public IActionResult UnauthorizedExample() => Unauthorized();

    [HttpPost("conflict")]
    public IActionResult ConflictExample() => Problem(
        type: "http://example.com/prop/create-product-conflict",
        title: HttpStatusCode.Conflict.ToString(),
        statusCode: StatusCodes.Status409Conflict,
        detail: "This Product already exists."
    );


    [HttpPost("business-rule-error")]
    public IActionResult BusinessRuleExample() => throw new ValidationException("A discontinued product cannot be put on promotion.");


}