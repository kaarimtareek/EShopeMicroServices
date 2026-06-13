using Catalog.API.Models;
using Marten;
using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync(cancellation))
            return;
        session.Store(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        return new List<Product>
        {
            new()
            {
                Name = "IPhone X",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Price = 999.99M,
                Categories = new List<string> { "Smartphones", "Electronics" },
                ImageFile = "https://example.com/images/iphone-x.jpg"
            },
            new()
            {
                Name = "Samsung Galaxy S10",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Price = 899.99M,
                Categories = new List<string> { "Smartphones", "Electronics" },
                ImageFile = "https://example.com/images/galaxy-s10.jpg"
            },
            new()
            {
                Name = "Google Pixel 4",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Price = 799.99M,
                Categories = new List<string> { "Smartphones", "Electronics" },
                ImageFile = "https://example.com/images/pixel-4.jpg"
            }
        };
    }
}