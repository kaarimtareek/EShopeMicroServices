using BuildingBlocks.Abstractions;
using Catalog.API.Models;
using Marten;
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public sealed record GetProductsQuery(int pageNumber = 1, int pageSize = 10) : IQuery<GetProductsResult>;

public sealed record GetProductsResult(IPagedList<Product> Products);

public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
            .ToPagedListAsync(request.pageNumber, request.pageSize, cancellationToken);
        return new GetProductsResult(products);
    }
}