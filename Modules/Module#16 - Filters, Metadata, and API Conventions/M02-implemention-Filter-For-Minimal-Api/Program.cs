using M02_implemention_Filter_For_Minimal_Api.Filters;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var productGroup = app.MapGroup("")
.AddEndpointFilter<TrackActionTimeEndpointFilter>(); //global  for all products  
; 
app.MapGet("/", () => "Hello World!")
.AddEndpointFilter<TrackActionTimeEndpointFilter>();

app.Run();
