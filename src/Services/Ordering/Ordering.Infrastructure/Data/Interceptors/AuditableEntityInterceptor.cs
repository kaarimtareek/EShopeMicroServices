using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Order.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        UpdateEntities(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context is null) return;
        foreach (var entity in context.ChangeTracker.Entries<IEntity>())
        {
            if (entity.State is EntityState.Added)
            {
                //In production environment, we can extract the user id from the IHttpContextAccessor and assign it to created by.
                entity.Entity.CreatedBy = "System"; // Set the created by user
                entity.Entity.CreatedAt = DateTime.UtcNow; // Set the created at timestamp
                // Perform actions for added entities
            }

            if (entity.State is EntityState.Added or EntityState.Modified || entity.HasOwnedEntitiesChanged())
            {
                entity.Entity.LastModifiedBy = "System"; // Set the updated by user
                entity.Entity.LastModifiedAt = DateTime.UtcNow; // Set the updated at timestamp
                // Perform actions for modified entities
            }
        }
    }
}

public static class EntityExtensions
{
    public static bool HasOwnedEntitiesChanged(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry is { State: EntityState.Added or EntityState.Modified } && r.TargetEntry.Metadata.IsOwned());
    }
}