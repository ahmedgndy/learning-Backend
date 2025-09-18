
using System.Data;
using Dapper;
using M03.RepositoryPattern.Data;
using M03.RepositoryPattern.Data.Handlers;
using M03.RepositoryPattern.Endpoints;
using M03.RepositoryPattern.Interfaces;
using M03.RepositoryPattern.Repositories;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<IProductRepository, DapperProductRepository>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

builder.Services.AddScoped<IDbConnection>(_ =>
    new SqliteConnection("Data Source=app.db"));

var app = builder.Build();

SqlMapper.AddTypeHandler(new GuidHandler());

app.MapProductEndpoints();

app.Run();