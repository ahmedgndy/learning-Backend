using Asp.Versioning;
using M06.UrlQueryStringVersioningMinimal.Data;
using M06.UrlQueryStringVersioningMinimal.Endpoints.V1;
using M06.UrlQueryStringVersioningMinimal.Endpoints.V2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductRepository>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
});


var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .HasApiVersion(new ApiVersion(2))
    .ReportApiVersions()
    .Build();

app.MapProductEndpointsV1(apiVersionSet);
app.MapProductEndpointsV2(apiVersionSet);

app.Run();