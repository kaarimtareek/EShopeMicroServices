namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
    : INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order updated with domain event {DomainEvent}", notification.GetType().Name);
        // Here you can implement any additional logic that should happen when an order is updated.
        return Task.CompletedTask;
    }
}