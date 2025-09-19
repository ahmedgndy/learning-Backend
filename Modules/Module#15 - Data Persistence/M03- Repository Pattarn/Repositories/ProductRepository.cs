
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
    public async Task<int> GetProductsCountAsync() => await _context.Products.CountAsync();
    public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.ToListAsync();

    public async Task<IEnumerable<Product>> GetProductsPageAsync(int page, int pageSize)
    {
        return await _context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();
    }

  public async Task<bool> ExistingByNameAsync(String name)
    {
        return await _context.Products.AnyAsync(p => name == p.Name);
    }
    public async Task<Product?> GetByIdAsync(Guid id) => await _context.Products.FindAsync(id);


    public async Task<bool> AddAsync(Product product)
    {
        _context.Products.Add(product);
        var rowEffected = await _context.SaveChangesAsync();
        return rowEffected > 1;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        var rowEffected = await _context.SaveChangesAsync();
        return rowEffected > 1;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is not null)
        {
            _context.Products.Remove(product);
            var rowEffected = await _context.SaveChangesAsync();
            return rowEffected > 1;

        }
        return false;

    }

    public async Task<bool> ExistsByIdAsync(Guid id) => await _context.Products.AnyAsync(p => p.Id == id);
    
}