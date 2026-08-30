using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
services.AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("ReverseProxy"));

services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
        // opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        // opt.QueueLimit = 2;
    });
    options.RejectionStatusCode = 429;
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.Headers.RetryAfter = "10";
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);
    };
});

var app = builder.Build();
app.UseRouting();
app.UseRateLimiter();
app.MapReverseProxy();
await app.RunAsync();