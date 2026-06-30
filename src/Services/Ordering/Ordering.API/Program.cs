using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//add services
var services = builder.Services;

services.AddApplicationServices();
services.AddInfrastructureServices(builder.Configuration);
services.AddApiServices();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
//config pipeline

app.UseApiServices();

app.Run();