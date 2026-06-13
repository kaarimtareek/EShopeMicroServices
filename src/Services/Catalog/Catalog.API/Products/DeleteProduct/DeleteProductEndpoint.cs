namespace Catalog.API.Products.DeleteProduct;

public sealed record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool success);
public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteProductCommand(id);
                var result = await sender.Send(command);
                return result.Success ? Results.Ok(new DeleteProductResponse(result.Success)) : Results.NotFound();
            })
            .WithName("DeleteProduct")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Deletes a product by ID")
            .WithDescription("Deletes a product from the catalog based on the provided product ID.");
    }
}