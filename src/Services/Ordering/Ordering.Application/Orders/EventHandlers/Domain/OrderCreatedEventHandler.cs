using MassTransit;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler(
    IPublishEndpoint publishEndpoint,
    IFeatureManager featureManager,
    ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order created with domain event {DomainEvent}", domainEvent.GetType().Name);
        // Here you can implement any additional logic that should happen when an order is created.
        if (await featureManager.IsEnabledAsync("EnabledOrderProcessing"))
        {
            logger.LogInformation(
                "Order processing feature is enabled. Proceeding with order processing for OrderId: {OrderId}",
                domainEvent.Order.Id);
            var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }
        else
        {
            logger.LogInformation(
                "Order processing feature is disabled. Skipping order processing for OrderId: {OrderId}",
                domainEvent.Order.Id);
        }
    }
}