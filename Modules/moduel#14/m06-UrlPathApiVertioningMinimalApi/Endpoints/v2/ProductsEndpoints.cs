using Asp.Versioning;
using Asp.Versioning.Builder;
using M01_urlPathVersioningController.Data;
using M01_urlPathVersioningController.Response.v2;
using Microsoft.AspNetCore.Http.HttpResults;

namespace m06_UrlPathApiVersioningMinimalApi.Endpoints.v2;
public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpointsV2(this IEndpointRouteBuilder app , ApiVersionSet apiVersionSet)
    {

        var productApi = app
        .MapGroup("api/v{apiVersion:apiVersion}/products")
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(new ApiVersion(2))
        .WithName("GetProductByIdv2");
       

        productApi.MapGet("{productId:guid}", GetProductById).WithName(nameof(GetProductById));

        return productApi;
    }   

    private static Results<Ok<ProductResponse>, NotFound<String>> GetProductById(Guid productId, ProductRepository repository) {
        var product = repository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound("f");

        return TypedResults.Ok(ProductResponse.FromModel(product));

    }
}