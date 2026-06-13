using Catalog.API.Models;
using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductByCategory;

public sealed record GetProductsByCategoryRequest(string Category);

public sealed record GetProductByCategoryResponse(IEnumerable<ProductDto> Products);

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var query = new GetProductByCategoryQuery(category);
                var result = await sender.Send(query);
                var dtos = result.Products.Select(p => p.Adapt<ProductDto>());
                return Results.Ok(new GetProductByCategoryResponse(dtos));
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .WithSummary("Retrieves products by category")
            .WithDescription("Retrieves a list of products that belong to a specific category.");
    }
}