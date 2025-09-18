using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Asp.Versioning.Builder;
using M01_urlPathVersioningController.Data;
using m06_UrlPathApiVersioningMinimalApi.Endpoints.v1;
using m06_UrlPathApiVersioningMinimalApi.Endpoints.v2;
using Microsoft.Extensions.Options;

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

//version set // for supporting versions in our abb

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .HasApiVersion(new ApiVersion(2))
    .ReportApiVersions()
    .Build();


app.MapProductEndpointsV1(versionSet);

app.MapProductEndpointsV2(versionSet);
app.Run();

