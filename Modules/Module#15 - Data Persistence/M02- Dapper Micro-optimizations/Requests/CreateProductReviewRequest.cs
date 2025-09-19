namespace M02__Dapper_Micro_optimizations.Requests;

public class CreateProductReviewRequest
{
    public string? Reviewer { get; set; }
    public int Stars { get; set; }
}