using M03__Repository_Pattern.Model;

namespace M03__Repository_Pattern.Responses;


public class ProductResponse
{

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }

    private ProductResponse() { }

    public static ProductResponse FromModel(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
    
   public static  IEnumerable<ProductResponse> FromModels(IEnumerable<Product> products)
    {
        return products.Select(p => FromModel(p));
    }
    
}