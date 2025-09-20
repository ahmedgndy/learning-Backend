using M04.CancellationTokenBenefit.Interfaces;

namespace M04_unite_of_Work.interfaces ;

public interface IUnitOfWork : IDisposable
{
    public IProductRepository Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}