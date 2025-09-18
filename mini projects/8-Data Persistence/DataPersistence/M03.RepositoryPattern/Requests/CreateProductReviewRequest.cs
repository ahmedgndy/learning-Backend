namespace M03.RepositoryPattern.Requests;

public class CreateProductReviewRequest
{
    public string? Reviewer { get; set; }
    public int Stars { get; set; }
}