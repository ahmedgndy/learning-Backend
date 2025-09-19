
using M03__Repository_Pattern.Model;

namespace M03__Repository_Pattern.Interfaces;

public interface IProductRepository
{
    Task<bool> AddAsync(Product product, CancellationToken ef = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ef = default);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken ef = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ef = default);
    Task<bool> UpdateAsync(Product product, CancellationToken ef = default);
    Task<int> GetProductsCountAsync(CancellationToken ef);
    Task<IEnumerable<Product>> GetProductsPageAsync(int page, int pageSize , CancellationToken ef = default) ;
    Task<bool> ExistingByNameAsync(String name, CancellationToken ef = default);
    Task<bool>  ExistsByIdAsync(Guid id ,CancellationToken ef = default);
}