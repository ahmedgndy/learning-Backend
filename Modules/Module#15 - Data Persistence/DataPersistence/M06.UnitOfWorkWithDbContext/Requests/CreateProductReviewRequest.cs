namespace M06.UnitOfWorkWithDbContext.Requests;

public class CreateProductReviewRequest
{
    public string? Reviewer { get; set; }
    public int Stars { get; set; }
}