using System;
public class ProductResponse
{
    public Guid ProductId { get; set; }
    public string? Name { get; set; }

    public decimal Price { get; set; }

    public List<ProductReviewResponse>? Reviews { get; set; } = new();


    private ProductResponse() { }

    //map one product 

    public static ProductResponse FromModel(Product product, IEnumerable<ProductReview>? reviews = null)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product), "Can Not Create A Product Response From A Null Product");

        var response = new ProductResponse
        {
            ProductId = product.Id,
            Name = product.Name,
            Price = product.Price
        };

        if (reviews is not null)
            response.Reviews = ProductReviewResponse.FromModel(reviews).ToList();

        return response;
    }

    //map list of  product 

    public static IEnumerable<ProductResponse> FromModel(IEnumerable<Product> products)
    {
        if (products is null)
            throw new ArgumentNullException(nameof(products), "Can Not Create A Product Response From A Null products");

        return products.Select(p => FromModel(p));
    }

}