using System.Data;
using Dapper;
using M02.Dapper.Models;

namespace M02.Dapper.Data;

public class ProductRepository(IDbConnection _db)
{

    public async Task<int> GetProductsCountAsync() =>
         await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Products");

    public async Task<List<Product>> GetProductsPageAsync(int page = 1, int pageSize = 10)
    {
        var result = await _db.QueryAsync<Product>(
           "SELECT * FROM Products LIMIT @Limit OFFSET @Offset",
           new { Limit = pageSize, Offset = (page - 1) * pageSize }
        );

        return result.ToList();
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _db.QuerySingleOrDefaultAsync<Product>(
            "SELECT * FROM Products WHERE Id = @Id", new { Id = productId.ToString() });
    }

    public async Task<List<ProductReview>> GetProductReviewsAsync(Guid productId)
    {
        var result = await _db.QueryAsync<ProductReview>(
            "SELECT * FROM ProductReviews WHERE ProductId = @ProductId",
            new { ProductId = productId.ToString() });
        return result.ToList();
    }

    public async Task<ProductReview?> GetReviewAsync(Guid productId, Guid reviewId)
    {
        return await _db.QuerySingleOrDefaultAsync<ProductReview>(
            "SELECT * FROM ProductReviews WHERE ProductId = @ProductId AND Id = @Id",
            new { ProductId = productId, Id = reviewId.ToString() });
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        var rows = await _db.ExecuteAsync(
            "INSERT INTO Products (Id, Name, Price) VALUES (@Id, @Name, @Price)",
            new { product.Id, product.Name, product.Price });
        return rows > 0;
    }

    public async Task<bool> AddProductReviewAsync(ProductReview review)
    {
        var exists = await _db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Products WHERE Id = @Id", new { Id = review.ProductId.ToString() });
        if (exists == 0) return false;

        var rows = await _db.ExecuteAsync("""
        INSERT INTO ProductReviews (Id, ProductId, Reviewer, Stars)
        VALUES (@Id, @ProductId, @Reviewer, @Stars)
        """,
            new
            {
                Id = review.Id.ToString(),
                ProductId = review.ProductId.ToString(),
                review.Reviewer,
                review.Stars
            });

        return rows > 0;
    }

    public async Task<bool> UpdateProductAsync(Product updatedProduct)
    {
        var rows = await _db.ExecuteAsync(
            "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id",
            new
            {
                Id = updatedProduct.Id.ToString(),
                updatedProduct.Name,
                updatedProduct.Price
            });

        return rows > 0;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var rows = await _db.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id.ToString() });
        await _db.ExecuteAsync("DELETE FROM ProductReviews WHERE ProductId = @Id", new { Id = id.ToString() });
        return rows > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _db.ExecuteScalarAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM Products WHERE Id = @Id)", new { Id = id.ToString() });
    }

    public async Task<bool> ExistsByNameAsync(string? name)
    {
        return await _db.ExecuteScalarAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM Products WHERE Name = @Name COLLATE NOCASE)", new { Name = name });
    }
}