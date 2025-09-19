
using M05.UnitOfWork.Data;
using M05.UnitOfWork.Endpoints;
using M05.UnitOfWork.Interfaces;
using M05.UnitOfWork.Repositories;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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