var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<DBintilazer>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

using(var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DBintilazer>();
    dbInitializer.Initialize();
}
app.Run();

public class DBintilazer
{

private readonly ILogger<DBintilazer> _logger;

public DBintilazer(ILogger<DBintilazer> logger)
{
    _logger = logger;
}

    public void Initialize()
    {
        _logger.LogInformation("Database initialization logic goes here.");
        // Add your database initialization code here
    }
}