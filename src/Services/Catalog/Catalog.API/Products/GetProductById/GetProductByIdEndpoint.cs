using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductById;

public sealed record GetProductByIdRequest(Guid Id);

public sealed record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query);
                if (result.Product is null)
                    return Results.NotFound();
                var dto = result.Product.Adapt<ProductDto>();
                return Results.Ok(new GetProductByIdResponse(dto));
            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Retrieves a product by ID")
            .WithDescription("Retrieves the details of a specific product by its unique identifier.");
    }
}