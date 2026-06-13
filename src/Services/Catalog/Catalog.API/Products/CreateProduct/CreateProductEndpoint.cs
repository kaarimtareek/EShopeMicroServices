namespace Catalog.API.Products.CreateProduct;

public sealed record CreateProductRequest(
    string Name,
    string Description,
    List<string> Categories,
    decimal Price,
    string ImageFile);

public sealed record ProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<ProductResponse>();
                return Results.Created($"/products/{result.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<ProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new product")
            .WithDescription("Creates a new product with the provided details and returns the created product's ID.");
       
    }
}