using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Models;
using Order.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class OderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => OrderItemId.Of(value));

        builder.Property(c => c.Quantity).IsRequired();
        builder.Property(c => c.Price).IsRequired();
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(c => c.ProductId);
    }
}