using Basket.API.Data;
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using JasperFx;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
//inject services
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddCarter();
services.AddMediatR(conifg =>
{
    conifg.RegisterServicesFromAssembly(assembly);
    conifg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    conifg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();


services.AddValidatorsFromAssembly(assembly);

services.AddExceptionHandler<CustomExceptionHandler>();

services.AddHealthChecks();

services.AddScoped<IBasketRepository, BasketRepository>();

var app = builder.Build();

app.MapCarter();
app.MapGet("/", () => "Hello World!");

app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();