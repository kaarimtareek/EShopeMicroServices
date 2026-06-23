var builder = WebApplication.CreateBuilder(args);

//add services
var services = builder.Services;


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
//config pipeline

app.Run();