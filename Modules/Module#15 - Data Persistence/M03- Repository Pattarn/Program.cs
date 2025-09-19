
using M03__Repository_Pattern.Data.Configuration;
using M03__Repository_Pattern.Interfaces;
using M03__Repository_Pattern.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Registraion services

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source= app.db")
);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
