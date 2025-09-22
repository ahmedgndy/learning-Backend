using System.ComponentModel.DataAnnotations;
using System.Net;

namespace M01.DefaultBehaviour.Endpoints;

public static class ErrorEndpoints
{
    public static RouteGroupBuilder MapErrorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal-fake-errors");

        group.MapGet("/server-error", () =>
        {
            System.IO.File.ReadAllText(@"C:\Settings\UploadSettings.json"); // not exist
            Results.Created();
        });

        group.MapPost("/bad-request", () => Results.Problem(
            type: "http://example.com/prop/sku-required",
            title: HttpStatusCode.BadRequest.ToString(),
            statusCode: StatusCodes.Status400BadRequest,
            detail: "Product SKU is required"
        ));

        group.MapPost("/bad-request-no-body", () => Results.BadRequest());

        group.MapPost("/not-found", () => Results.Problem(
            type: "http://example.com/prop/product-not-found",
            title: HttpStatusCode.NotFound.ToString(),
            statusCode: StatusCodes.Status404NotFound,
            detail: "Product not found."
        ));

        group.MapPost("/unauthorized", () => Results.Unauthorized());

        group.MapPost("/conflict", () => Results.Problem(
            type: "http://example.com/prop/create-product-conflict",
            title: HttpStatusCode.Conflict.ToString(),
            statusCode: StatusCodes.Status409Conflict,
            detail: "This Product already exists."
        ));

        group.MapPost("/business-rule-error", () =>
        {
            throw new ValidationException("A discontinued product cannot be put on promotion.");
        });

        return group;
    }
}
