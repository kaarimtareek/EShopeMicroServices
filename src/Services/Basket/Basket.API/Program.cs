var builder = WebApplication.CreateBuilder(args);

//inject services
var services = builder.Services;
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();