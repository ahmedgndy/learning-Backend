public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {

        var productApi = app.MapGroup("/api/products");

        productApi.MapGet("", GetProducts);
        productApi.MapGet("/{id}", GetProductById);

        productApi.MapPost("", () => Results.Created());
        productApi.MapPut("/{id}", (Guid id) => Results.NoContent());
        productApi.MapDelete("/{id}", (Guid id) => Results.NoContent());

        return productApi;
    }

    static IResult GetProducts()
    {
        return Results.Ok();
    }
    
    static IResult GetProductById(Guid id) =>   Results.Ok() ;
    
}