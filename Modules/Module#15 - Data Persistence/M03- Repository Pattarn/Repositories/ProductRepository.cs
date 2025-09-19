
using M03__Repository_Pattern.Data.Configuration;
using M03__Repository_Pattern.Interfaces;
using M03__Repository_Pattern.Model;
using Microsoft.EntityFrameworkCore;

namespace M03__Repository_Pattern.Repositories;


public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {

        _context = context;
    }
    public async Task<int> GetProductsCountAsync(CancellationToken ef ) => await _context.Products.CountAsync(ef);
    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ef) => await _context.Products.ToListAsync(ef);

    public async Task<IEnumerable<Product>> GetProductsPageAsync(int page, int pageSize , CancellationToken ef)
    {
        return await _context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync(ef);
    }

  public async Task<bool> ExistingByNameAsync(String name , CancellationToken ef)
    {
        return await _context.Products.AnyAsync(p => name == p.Name , ef);
    }
    public async Task<Product?> GetByIdAsync(Guid id,CancellationToken ef) => await _context.Products.FindAsync(id,ef);


    public async Task<bool> AddAsync(Product product,CancellationToken ef)
    {
        _context.Products.Add(product);
        var rowEffected = await _context.SaveChangesAsync(ef);
        return rowEffected > 1;
    }

    public async Task<bool> UpdateAsync(Product product, CancellationToken ef)
    {
        _context.Products.Update(product);
        var rowEffected = await _context.SaveChangesAsync(ef);
        return rowEffected > 1;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ef)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is not null)
        {
            _context.Products.Remove(product);
            var rowEffected = await _context.SaveChangesAsync(ef);
            return rowEffected > 1;

        }
        return false;

    }

    public async Task<bool> ExistsByIdAsync(Guid id,CancellationToken ef) => await _context.Products.AnyAsync(p => p.Id == id, ef);


}