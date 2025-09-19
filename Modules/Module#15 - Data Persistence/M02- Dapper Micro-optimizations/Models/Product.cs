namespace M02__Dapper_Micro_optimizations.Models;

public class Product
{
    public Guid Id{ get; set; }

    public string? Name { get; set; }
    public decimal Price { get; set; }

    public List<ProductReview> ProductReviews { get; set; } = [];

}