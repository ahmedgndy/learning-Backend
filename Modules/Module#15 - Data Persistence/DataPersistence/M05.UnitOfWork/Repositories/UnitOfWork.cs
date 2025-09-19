using M05.UnitOfWork.Data;
using M05.UnitOfWork.Interfaces;

namespace M05.UnitOfWork.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
{
    private IProductRepository? _productRepository;

    public IProductRepository Products => _productRepository ??= new ProductRepository(context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
