using BuildingBlocks.Abstractions;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProductByCategory;

public sealed record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public sealed record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryQueryHandler(
    IDocumentSession session,
    ILogger<GetProductByCategoryQueryHandler> logger)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().Where(p => p.Categories.Contains(request.Category))
            .ToListAsync(cancellationToken);
        return new GetProductByCategoryResult(products);
    }
}