using Asp.Versioning;
using M01_urlPathVersioningController.Data;
using m06_UrlPathApiVersioningMinimalApi.Endpoints.v1;
using m06_UrlPathApiVersioningMinimalApi.Endpoints.v2;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1)) //1
    .HasApiVersion(new ApiVersion(2)) //2
    .ReportApiVersions() //reppoert for headers 
    .Build();

app.MapProductEndpointsV1(apiVersionSet);
app.MapProductEndpointsV2(apiVersionSet);

app.Run();
