using M03.RepositoryPattern.Data;
using M03.RepositoryPattern.Interfaces;
using M03.RepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;

namespace M03.RepositoryPattern.Repositories;



public class EFProductRepository(AppDbContext context) : IProductRepository
{

    public async Task<int> GetProductsCountAsync() =>
        await context.Products.CountAsync();

    public async Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10)
    {
        var products = await context.Products.Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

        return products;
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return null;

        return product;
    }

    public async Task<List<ProductReview>> GetProductReviewsAsync(Guid productId)
    {
        return await context.ProductReviews.Where(r => r.ProductId == productId).ToListAsync();
    }

    public async Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId)
    {
        return await context.ProductReviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.Id == reviewId);
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        context.Products.Add(product);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddProductReviewAsync(ProductReview review)
    {
        if (!await context.Products.AnyAsync(p => p.Id == review.ProductId))
            return false;

        context.ProductReviews.Add(review);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateProductAsync(Product updatedProduct)
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

        if (existingProduct == null)
            return false;

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
            return false;

        context.Products.Remove(product);

        return await context.SaveChangesAsync() > 0;
    }
    public async Task<bool> ExistsByIdAsync(Guid id)
        => await context.Products.AnyAsync(p => p.Id == id);

    public async Task<bool> ExistsByNameAsync(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return await context.Products.AnyAsync(p =>
        EF.Functions.Like(p.Name!.ToUpper(), name.ToUpper()));
    }
}