

using M04.CancellationTokenBenefit.Data;
using M04.CancellationTokenBenefit.Interfaces;
using M04.CancellationTokenBenefit.Repositories;
using M04_unite_of_Work.interfaces;

namespace M04_unite_of_Work.Repositories;


public class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
{
    private IProductRepository? _productRepository;
    public IProductRepository Products =>  _productRepository ??= new ProductRepository(context);


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
    
   public void Dispose()
    {
        context.Dispose();
    }

}