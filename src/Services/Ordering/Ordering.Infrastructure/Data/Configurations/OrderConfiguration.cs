using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Enums;
using Order.Domain.Models;
using Order.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order.Domain.Models.Order>
{
    public void Configure(EntityTypeBuilder<Order.Domain.Models.Order> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion(
            id => id.Value,
            dbId => OrderId.Of(dbId));

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(c => c.CustomerId)
            .IsRequired();

        builder.HasMany<OrderItem>()
            .WithOne()
            .HasForeignKey(c => c.OrderId);

        builder.Property(x => x.Status).HasDefaultValue(OrderStatus.Draft).HasConversion(
            x => x.ToString(),
            x => Enum.Parse<OrderStatus>(x)
        );
        builder.Property(x => x.TotalPrice);
        
        builder.ComplexProperty(x => x.OrderName,
            nameBuilder =>
            {
                nameBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Order.Domain.Models.Order.OrderName))
                    .HasMaxLength(100).IsRequired();
            });

        builder.ComplexProperty(x => x.ShippingAddress,
            addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName).HasMaxLength(200).IsRequired();
                addressBuilder.Property(a => a.LastName).HasMaxLength(200).IsRequired();
                addressBuilder.Property(a => a.EmailAddress).HasMaxLength(100);
                addressBuilder.Property(a => a.AddressLine).HasMaxLength(180).IsRequired();
                addressBuilder.Property(a => a.Country).HasMaxLength(100);
                addressBuilder.Property(a => a.State).HasMaxLength(100);
                addressBuilder.Property(a => a.ZipCode).HasMaxLength(20);
            });
        builder.ComplexProperty(x => x.BillingAddress,
            addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName).HasMaxLength(200).IsRequired();
                addressBuilder.Property(a => a.LastName).HasMaxLength(200).IsRequired();
                addressBuilder.Property(a => a.EmailAddress).HasMaxLength(100);
                addressBuilder.Property(a => a.AddressLine).HasMaxLength(180).IsRequired();
                addressBuilder.Property(a => a.Country).HasMaxLength(100);
                addressBuilder.Property(a => a.State).HasMaxLength(100);
                addressBuilder.Property(a => a.ZipCode).HasMaxLength(20);
            });
        builder.ComplexProperty(x => x.Payment,
            paymentBuilder =>
            {
                paymentBuilder.Property(p => p.CardNumber).HasMaxLength(24).IsRequired();
                paymentBuilder.Property(p => p.CardName).HasMaxLength(50);
                paymentBuilder.Property(p => p.CardExpirationDate).HasMaxLength(10);
                paymentBuilder.Property(p => p.CVV).HasMaxLength(3).IsRequired();
                paymentBuilder.Property(p => p.PaymentMethod);
            });
    }
}