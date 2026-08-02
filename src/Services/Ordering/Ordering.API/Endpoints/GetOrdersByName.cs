using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints;

public sealed record GetOrdersByNameRequest(string Name);

public sealed record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/by-name/{name}", async (string name, ISender sender) =>
            {
                var query = new GetOrdersByNameQuery(name);
                var result = await sender.Send(query);
                var response = new GetOrdersByNameResponse(result.Orders);
                return Results.Ok(response);
            })
            .WithName("GetOrdersByName")
            .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Gets orders by name.")
            .WithDescription("Gets orders by name. The name must be provided in the URL.");
    }
}