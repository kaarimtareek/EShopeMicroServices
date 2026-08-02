using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

//- Accepts a CreateOrderRequest Object.
//- Maps the request to a CreateOrderCommand.
//- Use MediatR to send the command to the corresponding handler.
//- Return a response with the created order's ID.

public sealed record CreateOrderRequest(OrderDto Order);

public sealed record CreateOrderResponse(Guid Id);

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async ([FromBody] CreateOrderRequest request, [FromServices] ISender sender) =>
            {
                var command = new CreateOrderCommand(request.Order);
                var result = await sender.Send(command);
                var response = new CreateOrderResponse(result.OrderId);
                return Results.Created($"/orders/{response.Id}", response);
            })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new order for a customer.")
            .WithDescription(
                "Creates a new order for a customer. The request body must contain the order details, including the customer ID and order items.");
    }
}