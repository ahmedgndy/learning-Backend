using M06.UnitOfWorkWithDbContext.Data;
using M06.UnitOfWorkWithDbContext.Interfaces;
using M06.UnitOfWorkWithDbContext.Models;
using Microsoft.EntityFrameworkCore;

namespace M06.UnitOfWorkWithDbContext.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<int> GetProductsCountAsync(CancellationToken ct = default) =>
        await context.Products.CountAsync(ct);

    public async Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        return await context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken ct = default)
    {
        return await context.Products.FirstOrDefaultAsync(p => p.Id == productId, ct);
    }

    public async Task<List<ProductReview>> GetProductReviewsAsync(Guid productId, CancellationToken ct = default)
    {
        return await context.ProductReviews.Where(r => r.ProductId == productId).ToListAsync(ct);
    }

    public async Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId, CancellationToken ct = default)
    {
        return await context.ProductReviews
            .FirstOrDefaultAsync(r => r.ProductId == productId && r.Id == reviewId, ct);
    }

    public async Task<bool> AddProductAsync(Product product, CancellationToken ct = default)
    {
        context.Products.Add(product);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> AddProductReviewAsync(ProductReview review, CancellationToken ct = default)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == review.ProductId, ct);

        if (product is null)
            throw new InvalidOperationException();

        context.ProductReviews.Add(review);

        var reviews = await context.ProductReviews
                            .Where(pr => pr.ProductId == review.ProductId)
                            .ToListAsync(ct);

        product.AverageRating = (decimal)Math.Round(reviews.Average(pr => pr.Stars), 1, MidpointRounding.AwayFromZero);

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateProductAsync(Product updatedProduct, CancellationToken ct = default)
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == updatedProduct.Id, ct);
        if (existingProduct == null)
            return false;

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteProductAsync(Guid id, CancellationToken ct = default)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (product == null)
            return false;

        context.Products.Remove(product);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Products.AnyAsync(p => p.Id == id, ct);

    public async Task<bool> ExistsByNameAsync(string? name, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return await context.Products.AnyAsync(
            p => EF.Functions.Like(p.Name!.ToUpper(), name.ToUpper()), ct);
    }
}
