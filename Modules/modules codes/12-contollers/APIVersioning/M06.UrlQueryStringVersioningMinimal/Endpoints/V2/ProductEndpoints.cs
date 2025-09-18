using Asp.Versioning;
using Asp.Versioning.Builder;
using M06.UrlQueryStringVersioningMinimal.Data;
using M06.UrlQueryStringVersioningMinimal.Responses.V2;
using Microsoft.AspNetCore.Http.HttpResults;

namespace M06.UrlQueryStringVersioningMinimal.Endpoints.V2;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpointsV2(this IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        var productApi = app
                .MapGroup("api/products")
                .WithApiVersionSet(apiVersionSet)
                .HasApiVersion(new ApiVersion(2, 0));

        productApi.MapGet("{productId:guid}", GetProductById)
            .WithName("GetProductByIdV2");

        return productApi;
    }

    private static Results<Ok<ProductResponse>, NotFound<string>> GetProductById(
        Guid productId,
        ProductRepository repository)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound($"Product with Id '{productId}' not found");

        return TypedResults.Ok(ProductResponse.FromModel(product));
    }
}