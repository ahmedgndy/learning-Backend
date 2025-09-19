using M06.UnitOfWorkWithDbContext.Data;
using M06.UnitOfWorkWithDbContext.Endpoints;
using M06.UnitOfWorkWithDbContext.Interfaces;
using M06.UnitOfWorkWithDbContext.Repositories;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();

app.MapProductEndpoints();

app.Run();