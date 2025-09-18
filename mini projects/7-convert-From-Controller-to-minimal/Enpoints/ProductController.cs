
using System;
using System.Diagnostics.Contracts;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


public static class ProductEndpoints  
{

    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
    var group = app.MapGroup("/api/products");

        // OPTIONS
        group.MapMethods("",["OPTIONS"] ,OptionsProducts);

        // HEAD
        group.MapMethods("/{productId:guid}",["HEAD"], HeadProducts);

        // GET by Id (with optional query includeReview)
        group.MapGet("/{productId:guid}", GetProductById)
             .WithName(nameof(GetProductById)); // مهم علشان CreatedAtRoute

        // GET paged
        group.MapGet("/", GetPaged);

        // POST
        group.MapPost("/", CreateProduct);

        // PUT
        group.MapPut("/{productId:guid}", Put);

        // DELETE
        group.MapDelete("/{productId:guid}", Delete);

        // ProcessAsync → Accepted
        group.MapPost("/process", ProcessAsync);

        // CSV export
        group.MapGet("/export/csv", GetProductsCSV);

        // Physical file
        group.MapGet("/export/physical", GetPhysicalFile);

        // Redirect
        group.MapGet("/redirect", GetRedirection);

        // Temp endpoint
        group.MapGet("/temp-products", TempProducts);

        return group;
    }

    private static  IResult OptionsProducts(HttpResponse response)
    {
        response.Headers.Append("Allow", "GET ,HEAD ,POST, DELETE ,PATCH ,OPTIONS");
        return Results.NoContent();
    }

    private static IResult HeadProducts(ProductRepository repository ,Guid productId)
    {
        return repository.ExistsById(productId) ? Results.Ok() : Results.NotFound();
    }

    private static Results<Ok<ProductResponse>,NotFound<string>> GetProductById(ProductRepository repository ,Guid productId, bool includeReview = false)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound("the product not fout");

        List<ProductReview>? reviews = null;

        if (includeReview == true)
            reviews = repository.GetProductReviews(productId);

        var productResponse = ProductResponse.FromModel(product, reviews);
        return TypedResults.Ok(productResponse);
    }

    private static IResult GetPaged(
        ProductRepository repository,
        int page,
        int pageSize)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);


        var items = repository.GetProductsPage(page, pageSize);
        var totalCount = repository.GetProductsCount();
        var productResponse = ProductResponse.FromModel(items);

        var pageResponse = PageResponse<ProductResponse>.Create(
                                         productResponse
                                         , pageSize,
                                          totalCount,
                                           page);
        return Results.Ok(pageResponse);
    }

    private static IResult CreateProduct(ProductRepository repository , CreateProductRecord record)
    {

        if (repository.ExistsByName(record.Name))
            return Results.Conflict($"A Product with The name {record.Name} already Exist");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = record.Name,
            Price = record.Price
        };

        repository.AddProduct(product);

        return Results.CreatedAtRoute(routeName: nameof(GetProductById),
                     routeValues: new { productID = product.Id },
                     value: ProductResponse.FromModel(product));
    }

 

    private static IResult Put(ProductRepository repository  ,Guid productId, UpdateProductRecord record)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return Results.NotFound($"Product With Id {productId} not found");
        product.Name = record.Name;
        product.Price = record.Price;

        var isSucceed = repository.UpdateProduct(product);
        if (!isSucceed)
            //500 fail form server side not clint
            return Results.StatusCode(500);

        return Results.NoContent();
    }


    private static IResult Delete(ProductRepository repository, Guid productId)
    {
        if (!repository.ExistsById(productId))
            return Results.NotFound($"Not found Product With id : {productId}");

        var isDeleted = repository.DeleteProduct(productId);
        if (!isDeleted)
            return Results.StatusCode(500);
        return Results.NoContent();

    }


    private static IResult ProcessAsync()
    {
        var jopId = Guid.NewGuid();

        return Results.Accepted($"/api/products/status/{jopId}",
            new { jopId, status = "Processing" }
        );
    }


    private static IResult GetProductsCSV(ProductRepository repository)
    {
        var product = repository.GetProductsPage(1, 200);
        var cvsBuilder = new StringBuilder();

        cvsBuilder.AppendLine("ID,Name,Price");

        product.ForEach(p => cvsBuilder.AppendLine($"{p.Id},{p.Name},{p.Price}"));

        //convert string to array of bytes
        var fileBytes = Encoding.UTF32.GetBytes(cvsBuilder.ToString());

        return Results.File(fileBytes, "text/csv", "products_From_1_to_100.csv");

    }


    private static IResult GetPhysicalFile()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "File", "physicalFile.csv");

        return TypedResults.PhysicalFile(path, "text/csv", "products_From_1_to_100_Physical.csv");
    }


    private static IResult GetRedirection()
    {

        return Results.Redirect("/api/products/temp-products");

    }

    private static IResult TempProducts()
    {
        return Results.Ok(new {message ="you are in the temp endpoint . chill ."});
    }
}