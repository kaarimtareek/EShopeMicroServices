using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Order.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvent(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await DispatchDomainEvent(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async Task DispatchDomainEvent(DbContext? context, CancellationToken cancellationToken = default)
    {
        if (context is null) return;

        var aggregates = context.ChangeTracker.Entries<IAggregate>().Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity).ToList();
        if (aggregates.Count == 0) return;

        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ForEach(a => a.ClearDomainEvents());
        foreach (var domainEvent in domainEvents)
        {
            // Here you can dispatch the domain event to your event bus or mediator
            // For example, if you are using MediatR, you can do something like:
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}