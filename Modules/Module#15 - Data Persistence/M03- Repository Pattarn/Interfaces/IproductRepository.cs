
using M03__Repository_Pattern.Model;

namespace M03__Repository_Pattern.Interfaces;

public interface IProductRepository
{
    Task<bool> AddAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Product product);
    Task<int> GetProductsCountAsync();
    Task<IEnumerable<Product>> GetProductsPageAsync(int page, int pageSize);
    Task<bool> ExistingByNameAsync(String name);
    Task<bool>  ExistsByIdAsync(Guid id);
}