using Microsoft.AspNetCore.Http.HttpResults;
using M02.Dapper.Models;
using M02.Dapper.Data;
using M02.Dapper.Responses;
using M02.Dapper.Requests;
using System.Threading.Tasks;

namespace M02.Dapper.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var productApi = app.MapGroup("/api/products");

        productApi.MapGet("", GetPaged);
        productApi.MapGet("{productId:guid}", GetProductById).WithName(nameof(GetProductById));
        productApi.MapPost("", CreateProduct);
        productApi.MapPost("{productId:guid}/reviews", CreateProductReview);
        productApi.MapPut("{productId:guid}", Put);
        productApi.MapDelete("{productId:guid}", Delete);

        return productApi;
    }

    private static async Task<IResult> GetPaged(
        ProductRepository repository,
        int page = 1,
        int pageSize = 10)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        int totalCount = await repository.GetProductsCountAsync();

        var products = await repository.GetProductsPageAsync(page, pageSize);

        var pagedResult = PagedResult<ProductResponse>.Create(
            ProductResponse.FromModels(products),
            totalCount,
            page,
            pageSize);

        return Results.Ok(pagedResult);
    }

    private static async Task<Results<Ok<ProductResponse>, NotFound<string>>> GetProductById(
        Guid productId,
        ProductRepository repository,
        bool includeReviews = false)
    {
        var product = await repository.GetProductByIdAsync(productId);

        if (product is null)
            return TypedResults.NotFound($"Product with Id '{productId}' not found");

        List<ProductReview>? reviews = null;

        if (includeReviews)
        {
            reviews = await repository.GetProductReviewsAsync(productId);
        }

        return TypedResults.Ok(ProductResponse.FromModel(product, reviews));
    }

    private static async Task<IResult> CreateProduct(CreateProductRequest request, ProductRepository repository)
    {
        if (await repository.ExistsByNameAsync(request.Name))
            return Results.Conflict($"A product with the name '{request.Name}' already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };

        await repository.AddProductAsync(product);

        return Results.CreatedAtRoute(routeName: nameof(GetProductById),
                              routeValues: new { productId = product.Id },
                              value: ProductResponse.FromModel(product));
    }

    private static async Task<IResult> CreateProductReview(
        Guid productId,
        CreateProductReviewRequest request,
        ProductRepository repository)
    {
        if (!await repository.ExistsByIdAsync(productId))
            return Results.NotFound($"Product with Id '{productId}' not found");

        var productReview = new ProductReview
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Reviewer = request.Reviewer,
            Stars = request.Stars
        };

        await repository.AddProductReviewAsync(productReview);

        return Results.Created(
                $"/api/products/{productId}/reviews/{productReview.Id}",
                ProductReviewResponse.FromModel(productReview)
        );
    }

    private static async Task<IResult> Put(Guid productId, UpdateProductRequest request, ProductRepository repository)
    {
        var product = await repository.GetProductByIdAsync(productId);

        if (product is null)
            return Results.NotFound($"Product with Id '{productId}' not found");

        product.Name = request.Name;
        product.Price = request.Price ?? 0;

        var succeeded = await repository.UpdateProductAsync(product);

        if (!succeeded)
            return Results.StatusCode(500);

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid productId, ProductRepository repository)
    {
        if (!await repository.ExistsByIdAsync(productId))
            return Results.NotFound($"Product with Id '{productId}' not found");

        var succeeded = await repository.DeleteProductAsync(productId);

        if (!succeeded)
            return Results.StatusCode(500);

        return Results.NoContent();
    }
}