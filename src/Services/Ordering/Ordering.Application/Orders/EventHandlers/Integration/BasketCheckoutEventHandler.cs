using BuildingBlocks.Messaging.Events;
using MassTransit;
using Order.Domain.Enums;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        logger.LogInformation("Basket checkout event received: {Event}", context.Message);

        // Here you can implement the logic to handle the basket checkout event.
        // For example, you might want to create an order based on the basket information.

        // Example: Send a command to create an order
        var createOrderCommand = MapToCreateOrderCommand(context.Message);
        var result = await sender.Send(createOrderCommand, context.CancellationToken);

        if (result.OrderId != Guid.Empty)
        {
            logger.LogInformation("Order created successfully for user {UserName}", context.Message.UserName);
        }
        else
        {
            logger.LogError("Failed to create order for user {UserName}", context.Message.UserName);
            // Optionally, you can throw an exception or handle the failure accordingly.
        }
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine,
            message.Country, message.State, message.ZipCode);
        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV,
            message.PaymentMethod);
        var orderId = Guid.NewGuid(); // Generate a new order ID
        var orderDto = new OrderDto(orderId, message.CustomerId, message.UserName, addressDto, addressDto, paymentDto,
            OrderStatus.Pending, [
                new OrderItemDto(orderId, new Guid("6b8a3ce0-9638-42dc-b047-2f37f491f9b3"), 2, 500),
                new OrderItemDto(orderId, new Guid("732f0d4b-90fd-4e03-b1f4-77d8a33caa5a"), 1, 400)
            ]);
        return new CreateOrderCommand(orderDto);
    }
}