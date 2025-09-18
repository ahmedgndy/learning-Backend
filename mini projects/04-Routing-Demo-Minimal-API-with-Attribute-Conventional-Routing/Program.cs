var builder = WebApplication.CreateBuilder(args);

//Add all services to deal with the controller
builder.Services.AddControllers();

//Inject the monthConstraints
builder.Services.AddRouting(options =>
    options.ConstraintMap.Add("monthConstraints", typeof(MonthConstraints))
);

var app = builder.Build();

//use controllers 
app.MapControllers();
app.MapGet("generate/order/{id:int}", (int id, LinkGenerator link, HttpContext context) =>
{
    //updateOrder this is the endpoint
    // id the rout parameter 
    //context.Request.Scheme http / https 
    // context.Request.Host //localhost:5152
    var editUrl = link.GetUriByName("UpdateOrder" , new {id} ,context.Request.Scheme, context.Request.Host);    
    //order is retrieved 
    return Results.Ok(new
    {
        OrderId = id,
        ProductName = "Sample Product",
        Quantity = 1, 
        _links = new
        {
            self = new { href = context.Request.Path }, 
            edit = new { href = editUrl } 
        }
    });
});

app.MapPut("/order/{id:int}", (int id) =>
{
    //order is updated 

    return Results.Ok();
}).WithName("UpdateOrder");

//test app health 

app.MapGet("/health", () => Results.Ok("The app is healthy"));
app.Run();
