using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

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
//add auto migrations for development
if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}

app.Run();