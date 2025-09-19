using M01.EFCoreCodeFirst.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
}
);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
