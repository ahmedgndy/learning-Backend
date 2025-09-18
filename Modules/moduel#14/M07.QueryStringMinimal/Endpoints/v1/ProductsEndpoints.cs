using Asp.Versioning;
using Asp.Versioning.Builder;
using M01_urlPathVersioningController.Data;
using M01_urlPathVersioningController.Response.v1;
using Microsoft.AspNetCore.Http.HttpResults;

namespace m06_UrlPathApiVersioningMinimalApi.Endpoints.v1;
public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpointsV1(this IEndpointRouteBuilder app , ApiVersionSet apiVersionSet)
    {
     
        var productApi = app
            .MapGroup("api/products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0)); //version 1 

        productApi.MapGet("{productId:guid}", GetProductById).WithName("GetProductByIdV1");

        return productApi;
    }

    private static Results<Ok<ProductResponse>, NotFound<String>> GetProductById(Guid productId, ProductRepository repository , HttpResponse response) {
        var product = repository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound("f");
        response.Headers["Deprecations"] = "true";
        return TypedResults.Ok(ProductResponse.FromModel(product));

    }
}