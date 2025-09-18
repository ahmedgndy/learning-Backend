
namespace  M01_urlPathVersioningController.Data;

using M01_urlPathVersioningController.Model;

public class ProductRepository
{
    private List<Product> _products =
  [
     new Product { Id = Guid.Parse("2779ee47-10b0-4bd7-8342-404006aa1392"), Name = "Soda", Price = 1.99m },
    ];




    public Product? GetProductById(Guid ProductId)
    {
        var product = _products.FirstOrDefault(p => p.Id == ProductId);

        if (product is null)
            return null;

        return product;

    }

}