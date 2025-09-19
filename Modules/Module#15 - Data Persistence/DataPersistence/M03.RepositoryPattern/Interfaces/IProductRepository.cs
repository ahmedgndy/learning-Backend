using M03.RepositoryPattern.Models;

namespace M03.RepositoryPattern.Interfaces;

public interface IProductRepository
{
    Task<bool> AddProductAsync(Product product);
    Task<bool> AddProductReviewAsync(ProductReview review);
    Task<bool> DeleteProductAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
    Task<bool> ExistsByNameAsync(string? name);
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<List<ProductReview>> GetProductReviewsAsync(Guid productId);
    Task<int> GetProductsCountAsync();
    Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10);
    Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId);
    Task<bool> UpdateProductAsync(Product updatedProduct);
}