
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
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

services.AddValidatorsFromAssembly(assembly);

services.AddExceptionHandler<CustomExceptionHandler>();

services.AddHealthChecks();
var app = builder.Build();

app.MapCarter();
app.MapGet("/", () => "Hello World!");

app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();