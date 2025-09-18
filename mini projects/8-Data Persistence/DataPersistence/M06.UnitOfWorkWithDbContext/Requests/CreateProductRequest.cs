namespace M06.UnitOfWorkWithDbContext.Requests;

public class CreateProductRequest
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
}
