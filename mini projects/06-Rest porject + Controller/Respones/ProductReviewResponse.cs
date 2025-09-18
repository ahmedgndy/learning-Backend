public class ProductReviewResponse
{
    public Guid ReviewId { get; set; }
    public Guid ProductId { get; set; }
    public string? Reviewer { get; set; }
    public int Stars { get; set; }

    private ProductReviewResponse() { }

    //mapping one review  
    public static ProductReviewResponse FromModel(ProductReview? review)
    {
        if (review is null)
            throw new ArgumentNullException(nameof(review), "Can Not Create A Review Response From A Null Review");

        return new ProductReviewResponse
        {
            ReviewId = review.Id,
            ProductId = review.ProductId,
            Reviewer = review.Reviewer,
            Stars = review.Stars,
        };
    }
    //mapping list of reviews  
   
     public static IEnumerable<ProductReviewResponse>  FromModel(IEnumerable<ProductReview> reviews)
    {
        if (reviews is null)
            throw new ArgumentNullException(nameof(reviews), "Can Not Create A Review Response From A Null Review");

        return reviews.Select(FromModel);
    }
}