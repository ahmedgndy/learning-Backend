using M05.UnitOfWork.Models;
using Microsoft.EntityFrameworkCore;

namespace M05.UnitOfWork.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductReview> ProductReviews => Set<ProductReview>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}