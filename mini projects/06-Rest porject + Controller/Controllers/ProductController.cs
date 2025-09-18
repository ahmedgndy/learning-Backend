
using System;
using System.Diagnostics.Contracts;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]

public class ProductController(ProductRepository repository) : ControllerBase
{
    [HttpOptions]
    public IActionResult OptionsProducts()
    {
        Response.Headers.Append("Allow", "GET ,HEAD ,POST, DELETE ,PATCH ,OPTIONS");
        return NoContent();
    }

    [HttpHead("{productId:Guid}")]
    public IActionResult HeadProducts(Guid productId)
    {
        return repository.ExistsById(productId) ? Ok() : NotFound();
    }

    [HttpGet("{productId:Guid}", Name = "GetProductById")]
[Produces("application/json" ,"application/xml")]// just support json and xml
    public ActionResult<ProductResponse> GetProductById(Guid productId, bool includeReview = false)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return NotFound();

        List<ProductReview>? reviews = null;

        if (includeReview == true)
            reviews = repository.GetProductReviews(productId);

        var productResponse = ProductResponse.FromModel(product, reviews);
        return productResponse;
    }

    [HttpGet]

    public IActionResult GetPaged(int page, int pageSize)
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
        return Ok(pageResponse);
    }

    [HttpPost]
    public IActionResult CreateProduct(CreateProductRecord record)
    {

        if (repository.ExistsByName(record.Name))
            return Conflict($"A Product with The name {record.Name} already Exist");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = record.Name,
            Price = record.Price
        };

        repository.AddProduct(product);

        return CreatedAtRoute(routeName: nameof(GetProductById),
                     routeValues: new { productID = product.Id },
                     value: ProductResponse.FromModel(product));
    }

    [HttpPut("{productId:guid}")]

    public IActionResult Put(Guid productId, UpdateProductRecord record)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return NotFound($"Product With Id {productId} not found");
        product.Name = record.Name;
        product.Price = record.Price;

        var isSucceed = repository.UpdateProduct(product);
        if (!isSucceed)
            //500 fail form server side not clint
            return StatusCode(500, $"Fail to update Product With id {productId}");

        return NoContent();
    }

    [HttpDelete("{productId:guid}")]

    public IActionResult Delete(Guid productId)
    {
        if (!repository.ExistsById(productId))
            return NotFound($"Not found Product With id : {productId}");

        var isDeleted = repository.DeleteProduct(productId);
        if (!isDeleted)
            return StatusCode(500, "Failed To Delete product");
        return NoContent();

    }

    [HttpPost("process")]

    public IActionResult ProcessAsync()
    {
        var jopId = Guid.NewGuid();

        return Accepted($"/api/products/status/{jopId}",
            new { jopId, status = "Processing" }
        );
    }

    [HttpGet("csv")]

    public IActionResult GetProductsCSV()
    {
        var product = repository.GetProductsPage(1, 200);
        var cvsBuilder = new StringBuilder();

        cvsBuilder.AppendLine("ID,Name,Price");

        product.ForEach(p => cvsBuilder.AppendLine($"{p.Id},{p.Name},{p.Price}"));

        //convert string to array of bytes
        var fileBytes = Encoding.UTF32.GetBytes(cvsBuilder.ToString());

        return File(fileBytes, "text/csv", "products_From_1_to_100.csv");


    }

    [HttpGet("physical")]

    public IActionResult GetPhysicalFile()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "File", "physicalFile.csv");

        return PhysicalFile(path, "text/csv", "products_From_1_to_100_Physical.csv");
    }

    [HttpGet("product-legacy")]

    public IActionResult GetRedirection()
    {

        return Redirect("/api/products/temp-products");

    }

    [HttpGet("temp-products")]
    public IActionResult TempProducts()
    {
        return Ok(new {message ="you are in the temp endpoint . chill ."});
    }
}