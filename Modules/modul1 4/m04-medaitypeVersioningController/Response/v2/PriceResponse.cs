using Microsoft.AspNetCore.Http.HttpResults;

namespace M01_urlPathVersioningController.Response.v2;

public sealed class PriceResponse
{

    public decimal Amount { get; set; }

    public string Currency { get; set; } 
}