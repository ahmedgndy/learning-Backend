namespace M05.UnitOfWork.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}