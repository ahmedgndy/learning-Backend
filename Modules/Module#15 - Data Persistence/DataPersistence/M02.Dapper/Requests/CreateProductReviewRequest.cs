namespace M02.Dapper.Requests;

public class CreateProductReviewRequest
{
    public string? Reviewer { get; set; }
    public int Stars { get; set; }
}