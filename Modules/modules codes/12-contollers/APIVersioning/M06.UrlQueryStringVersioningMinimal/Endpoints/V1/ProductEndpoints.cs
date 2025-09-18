using Asp.Versioning;
using Asp.Versioning.Builder;
using M06.UrlQueryStringVersioningMinimal.Data;
using M06.UrlQueryStringVersioningMinimal.Responses.V1;
using Microsoft.AspNetCore.Http.HttpResults;

namespace M06.UrlQueryStringVersioningMinimal.Endpoints.V1;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpointsV1(this IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        var productApi = app
            .MapGroup("api/products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        productApi.MapGet("{productId:guid}", GetProductById)
            .WithName("GetProductByIdV1");

        return productApi;
    }

    private static Results<Ok<ProductResponse>, NotFound<string>> GetProductById(
        Guid productId,
        ProductRepository repository, HttpResponse response)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound($"Product with Id '{productId}' not found");

        response.Headers["Deprecation"] = "true";
        response.Headers["Sunset"] = "Wed, 31 Dec 2025 23:59:59 GMT";
        response.Headers["Link"] = "</api/v2/products>; rel=\"successor-version\"";

        return TypedResults.Ok(ProductResponse.FromModel(product));
    }
}