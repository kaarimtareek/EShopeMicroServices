using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.DeleteBasket;

public sealed record DeleteBasketRequest(string UserName);

public sealed record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket", async ([FromBody] DeleteBasketRequest request, [FromServices] ISender sender) =>
            {
                var command = request.Adapt<DeleteBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            })
            .WithTags("Basket")
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Deletes the user's basket.")
            .WithDescription("Deletes the user's basket and all its items.");
    }
}