using M04.CancellationTokenBenefit.Data;
using M04.CancellationTokenBenefit.Endpoints;
using M04.CancellationTokenBenefit.Interfaces;
using M04.CancellationTokenBenefit.Repositories;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();

app.MapProductEndpoints();

app.Run();