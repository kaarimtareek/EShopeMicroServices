using Catalog.API.Models;
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public sealed record GetProductsRequest(int? pageNumber = 1, int? pageSize = 10);

public sealed record GetProductsResponse(IPagedList<Product> Products);

public record ProductDto(
    string Id,
    string Name,
    string Description,
    List<string> Categories,
    decimal Price,
    string ImageFile,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = new GetProductsQuery(request.pageNumber ?? 1, request.pageSize ?? 10);
                var result = await sender.Send(query);
                var products = result.Products;
                return Results.Ok(new GetProductsResponse(products));
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .WithSummary("Retrieves all products")
            .WithDescription("Retrieves a list of all products in the catalog.");
    }
}