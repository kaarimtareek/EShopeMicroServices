using Discount.GRPC.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Data;

public class DiscountContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = null!;

    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon()
            {
                Id = 1,
                ProductName = "IPhone X",
                Description = "Discount for IPhone X",
                Amount = 10,
            },
            new Coupon()
            {
                Id = 2,
                ProductName = "Samsung 10",
                Description = "Discount for Samsung 10",
                Amount = 20,
            }
        );
    }
}