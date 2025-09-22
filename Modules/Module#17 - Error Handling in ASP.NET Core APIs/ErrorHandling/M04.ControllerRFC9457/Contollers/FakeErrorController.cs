using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace M03.RFC9457Controller.Contollers;

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
        title: "Bad Request",
        statusCode: StatusCodes.Status400BadRequest,
        detail: "Product SKU is required"
    );


    [HttpPost("not-found")]
    public IActionResult NotFoundExample() => Problem(
        type: "http://example.com/prop/product-not-found",
        title: "Bad Request",
        statusCode: StatusCodes.Status404NotFound,
        detail: "Product not found."
    );

    [HttpPost("unauthorized")]
    public IActionResult UnauthorizedExample() => Unauthorized(new ProblemDetails
    {
        Title = "You are not authorized"
    });

    [HttpPost("conflict")]
    public IActionResult ConflictExample() => Problem(
        type: "http://example.com/prop/product-not-found",
        title: "Bad Request",
        statusCode: StatusCodes.Status409Conflict,
        detail: "This Product already exists."
    );


    [HttpPost("business-rule-error")]
    public IActionResult BusinessRuleExample() => throw new ValidationException("A discontinued product cannot be put on promotion.");


}