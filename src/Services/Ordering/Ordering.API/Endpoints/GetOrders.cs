using BuildingBlocks.Pagination;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints;

public sealed record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters] PaginationRequest request, [FromServices] ISender sender) =>
            {
                var query = new GetOrdersQuery(request);
                var result = await sender.Send(query);
                var response = new GetOrdersResponse(result.Orders);
                return Results.Ok(response);
            })
            .WithName("GetOrders")
            .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Gets orders with pagination.")
            .WithDescription(
                "Gets orders with pagination. The pagination parameters must be provided in the query string.");
    }
}