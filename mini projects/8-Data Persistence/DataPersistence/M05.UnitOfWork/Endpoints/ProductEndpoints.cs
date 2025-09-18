using Microsoft.AspNetCore.Http.HttpResults;
using M05.UnitOfWork.Interfaces;
using M05.UnitOfWork.Responses;
using M05.UnitOfWork.Models;
using M05.UnitOfWork.Requests;

namespace M05.UnitOfWork.Endpoints;


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
        IUnitOfWork uow,
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        int totalCount = await uow.Products.GetProductsCountAsync(ct);
        var products = await uow.Products.GetProductsPageAsync(page, pageSize, ct);

        var pagedResult = PagedResult<ProductResponse>.Create(
            ProductResponse.FromModels(products),
            totalCount,
            page,
            pageSize);

        return Results.Ok(pagedResult);
    }

    private static async Task<Results<Ok<ProductResponse>, NotFound<string>>> GetProductById(
        Guid productId,
        IUnitOfWork uow,
        bool includeReviews = false,
        CancellationToken ct = default)
    {
        var product = await uow.Products.GetProductByIdAsync(productId, ct);

        if (product is null)
            return TypedResults.NotFound($"Product with Id '{productId}' not found");

        List<ProductReview>? reviews = null;

        if (includeReviews)
            reviews = await uow.Products.GetProductReviewsAsync(productId, ct);

        return TypedResults.Ok(ProductResponse.FromModel(product, reviews));
    }

    private static async Task<IResult> CreateProduct(
        CreateProductRequest request,
        IUnitOfWork uow,
        CancellationToken ct = default)
    {
        if (await uow.Products.ExistsByNameAsync(request.Name, ct))
            return Results.Conflict($"A product with the name '{request.Name}' already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };

        uow.Products.AddProduct(product);

        await uow.SaveChangesAsync(ct);

        return Results.CreatedAtRoute(
            routeName: nameof(GetProductById),
            routeValues: new { productId = product.Id },
            value: ProductResponse.FromModel(product));
    }

    private static async Task<IResult> CreateProductReview(
        Guid productId,
        CreateProductReviewRequest request,
        IUnitOfWork uow,
        CancellationToken ct = default)
    {
        if (!await uow.Products.ExistsByIdAsync(productId, ct))
            return Results.NotFound($"Product with Id '{productId}' not found");

        var productReview = new ProductReview
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Reviewer = request.Reviewer,
            Stars = request.Stars
        };

        await uow.Products.AddProductReviewAsync(productReview, ct);

        await uow.SaveChangesAsync(ct);

        return Results.Created(
            $"/api/products/{productId}/reviews/{productReview.Id}",
            ProductReviewResponse.FromModel(productReview));
    }

    private static async Task<IResult> Put(
        Guid productId,
        UpdateProductRequest request,
        IUnitOfWork uow,
        CancellationToken ct = default)
    {
        var product = await uow.Products.GetProductByIdAsync(productId, ct);

        if (product is null)
            return Results.NotFound($"Product with Id '{productId}' not found");

        product.Name = request.Name;
        product.Price = request.Price ?? 0;

        await uow.Products.UpdateProductAsync(product, ct);

        await uow.SaveChangesAsync(ct);

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(
        Guid productId,
        IUnitOfWork uow,
        CancellationToken ct = default)
    {
        if (!await uow.Products.ExistsByIdAsync(productId, ct))
            return Results.NotFound($"Product with Id '{productId}' not found");

        await uow.Products.DeleteProductAsync(productId, ct);

        await uow.SaveChangesAsync(ct);

        return Results.NoContent();
    }
}