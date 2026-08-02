using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints;

public sealed record UpdateOrderRequest(OrderDto Order);

public sealed record UpdateOrderResponse(bool Success);

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders",
                async ([FromBody] UpdateOrderRequest request, [FromServices] ISender sender) =>
                {
                    var command = request.Adapt<UpdateOrderCommand>();
                    var result = await sender.Send(command);
                    var response = new UpdateOrderResponse(result.IsSuccess);
                    return Results.Ok(response);
                })
            .WithName("UpdateOrder")
            .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Updates an existing order.")
            .WithDescription(
                "Updates an existing order. The request body must contain the updated order details.");
    }
}