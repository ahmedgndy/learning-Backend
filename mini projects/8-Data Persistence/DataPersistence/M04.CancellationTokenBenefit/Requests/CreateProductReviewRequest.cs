namespace M04.CancellationTokenBenefit.Requests;

public class CreateProductReviewRequest
{
    public string? Reviewer { get; set; }
    public int Stars { get; set; }
}