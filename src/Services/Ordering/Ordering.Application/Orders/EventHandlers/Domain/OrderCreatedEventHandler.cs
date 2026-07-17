namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order created with domain event {DomainEvent}", notification.GetType().Name);
        // Here you can implement any additional logic that should happen when an order is created.
        return Task.CompletedTask;
    }
}