namespace Catalog.API.Products.UpdateProduct;

public sealed record UpdateProductRequest(
    Guid Id,
    string Name,
    string Description,
    List<string> Categories,
    decimal Price,
    string ImageFile);

public sealed record UpdateProductResponse(Guid Id);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id}", async (Guid id, UpdateProductRequest request, ISender sender) =>
            {
                UpdateProductCommand command = new UpdateProductCommand(
                    Id: id,
                    Name: request.Name,
                    Description: request.Description,
                    Categories: request.Categories,
                    Price: request.Price,
                    ImageFile: request.ImageFile);
                var result = await sender.Send(command);

                if (result.Product is null)
                {
                    return Results.NotFound($"Product with ID {id} not found.");
                }

                var response = new UpdateProductResponse(result.Product.Id);
                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Updates an existing product")
            .WithDescription(
                "Updates the details of an existing product identified by its ID. The request body must contain the updated product details, and the ID in the URL must match the ID in the request body.");
    }
}