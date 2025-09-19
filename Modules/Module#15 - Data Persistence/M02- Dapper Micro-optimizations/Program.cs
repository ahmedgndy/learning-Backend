using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using M02__Dapper_Micro_optimizations.Data;
using  M02.Dapper.Endpoints;


var builder = WebApplication.CreateBuilder(args);

// Register Dapper
//
builder.Services.AddScoped<IDbConnection>(_ => new SqliteConnection("Data Source=app.db"));

// Register repository as Scoped (not Singleton!)
builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Initialize database
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<IDbConnection>();
DatabaseInitializer.Initialize(db);

// Add custom type handlers
SqlMapper.AddTypeHandler(new GuidHandler());

app.MapProductEndpoints();
app.Run();