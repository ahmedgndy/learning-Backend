using System.Runtime.Serialization;
using M03__Repository_Pattern.Model;
using Microsoft.EntityFrameworkCore;

namespace M03__Repository_Pattern.Data.Configuration;


public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
    }


}