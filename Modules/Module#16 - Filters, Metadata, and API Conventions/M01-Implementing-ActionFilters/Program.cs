
using M01_Implementing_ActionFilters.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
    options.Filters.Add<TrackACtionTimeFilterV2>()
);

var app = builder.Build();

app.MapControllers();
app.Run();