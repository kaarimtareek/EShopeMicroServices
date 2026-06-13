using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Data;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

//Add services
var services = builder.Services;
var currentAssembly = typeof(Program).Assembly;
services.AddEndpointsApiExplorer();
services.AddCarter();
services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(currentAssembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
services.AddMarten(options => { options.Connection(builder.Configuration.GetConnectionString("Database")!); })
    .UseLightweightSessions();
if (builder.Environment.IsDevelopment())
    services.InitializeMartenWith<CatalogInitialData>();

services.AddValidatorsFromAssembly(currentAssembly);

services.AddExceptionHandler<CustomExceptionHandler>();

services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

//Configure Pipeline

app.MapCarter();
app.MapGet("/", () => "Welcome to the Catalog API!").WithName("Home").WithSummary("API Home")
    .WithDescription("Returns a welcome message for the Catalog API.");

app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();