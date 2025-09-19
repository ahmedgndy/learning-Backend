using M01.EFCoreCodeFirst.Requests;
using M03__Repository_Pattern.Interfaces;
using M03__Repository_Pattern.Model;
using M03__Repository_Pattern.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace M03__Repository_Pattern.Endpoints;


public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var productApi = app.MapGroup("/api/products");

        productApi.MapGet("", GetPaged);
        productApi.MapGet("{productId:guid}", GetProductById).WithName(nameof(GetProductById));
        productApi.MapPost("", CreateProduct);

        productApi.MapPut("{productId:guid}", Put);
        productApi.MapDelete("{productId:guid}", Delete);

        return productApi;
    }

    private static async Task<IResult> GetPaged(
       IProductRepository repository,
        int page = 1,
        int pageSize = 10,
        CancellationToken ef = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        int totalCount = await repository.GetProductsCountAsync(ef);

        var products = await repository.GetProductsPageAsync(page, pageSize);

        var pagedResult = PagedResult<ProductResponse>.Create(
            items: ProductResponse.FromModels(products),
            totalCount,
            page,
            pageSize);

        return Results.Ok(pagedResult);
    }

    private static async Task<Results<Ok<ProductResponse>, NotFound<string>>> GetProductById(Guid productId,
                                                                                             IProductRepository repository,
                                                                                             bool includeReviews = false ,
                                                                                              CancellationToken ef = default)
    {
        var product = await repository.GetByIdAsync(productId , ef);

        if (product is null)
            return TypedResults.NotFound($"Product with Id '{productId}' not found");


        

        return TypedResults.Ok(ProductResponse.FromModel(product));
    }

    private static async Task<IResult> CreateProduct(CreateProductRequest request, IProductRepository repository , CancellationToken ef = default)
    {
        if (await repository.ExistingByNameAsync(request.Name , ef))
            return Results.Conflict($"A product with the name '{request.Name}' already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };

        await repository.AddAsync(product);

        return Results.CreatedAtRoute(routeName: nameof(GetProductById),
                              routeValues: new { productId = product.Id },
                              value: ProductResponse.FromModel(product));
    }

    

    private static async Task<IResult> Put(Guid productId, UpdateProductRequest request, IProductRepository repository , CancellationToken ef =default)
    {
        var product = await repository.GetByIdAsync(productId, ef );

        if (product is null)
            return Results.NotFound($"Product with Id '{productId}' not found");

        product.Name = request.Name;
        product.Price = request.Price ?? 0;

        var succeeded = await repository.UpdateAsync(product, ef);

        if (!succeeded)
            return Results.StatusCode(500);

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid productId, IProductRepository repository , CancellationToken ef = default)
    {
        if (!await repository.ExistsByIdAsync(productId , ef ))
            return Results.NotFound($"Product with Id '{productId}' not found");

        var succeeded = await repository.DeleteAsync(productId);

        if (!succeeded)
            return Results.StatusCode(500);

        return Results.NoContent();
    }
}