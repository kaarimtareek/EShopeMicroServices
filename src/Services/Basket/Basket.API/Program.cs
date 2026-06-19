using Basket.API.Data;
using BuildingBlocks.Exceptions.Handler;
using Discount.Grpc;
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

//Data services
services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis")!;
    opt.InstanceName = "Basket";
});

//Grpc services
services.AddGrpcClient<DiscountService.DiscountServiceClient>(options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });
//Validation services 
services.AddValidatorsFromAssembly(assembly);

services.AddExceptionHandler<CustomExceptionHandler>();

//Health check services
services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!, name: "PostgreSQL",
        tags: new[] { "db", "sql", "postgresql" })
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!, name: "Redis",
        tags: new[] { "db", "cache", "redis" });

services.AddScoped<IBasketRepository, BasketRepository>();
services.Decorate<IBasketRepository, CachedBasketRepository>();

var app = builder.Build();

app.MapCarter();
app.MapGet("/", () => "Hello World!");

app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();