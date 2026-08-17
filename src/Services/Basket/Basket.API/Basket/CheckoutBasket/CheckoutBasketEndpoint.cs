using System.Runtime.CompilerServices;
using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public sealed record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);

public sealed record CheckoutBasketResponse(bool IsSuccess);

public class CheckoutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout",
                async ([FromBody] CheckoutBasketRequest request, [FromServices] ISender sender) =>
                {
                    var command = new CheckoutBasketCommand(request.BasketCheckoutDto);
                    var result = await sender.Send(command);
                    var response = new CheckoutBasketResponse(result.IsSuccess);

                    return Results.Ok(response);
                })
            .WithName("CheckoutBasket")
            .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Checkout basket")
            .WithDescription("Checkout the items in the basket and create an order.");
    }
}