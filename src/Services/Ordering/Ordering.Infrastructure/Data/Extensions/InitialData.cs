using Order.Domain.Models;
using Order.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Extensions;

internal static class InitialData
{
    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            new() { Id = CustomerId.Of(Guid.Parse("dac1bfcc-d416-481e-bee4-0386333a0355")), Name = "John Doe", Email = "john.doe@example.com" },
            new() { Id = CustomerId.Of(Guid.Parse("01548010-ee1c-4921-9573-ffa7f4d00376")), Name = "Jane Smith", Email = "jane.smith@example.com" }
        };

    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            new() { Id = ProductId.Of(Guid.Parse("6b8a3ce0-9638-42dc-b047-2f37f491f9b3")), Name = "Product A", Price = 10.0m },
            new() { Id = ProductId.Of(Guid.Parse("732f0d4b-90fd-4e03-b1f4-77d8a33caa5a")), Name = "Product B", Price = 20.0m },
            new() { Id = ProductId.Of(Guid.Parse("ace3e3f2-c03f-4dbb-bc26-8636b2a758a2")), Name = "Product C", Price = 30.0m },
            new() { Id = ProductId.Of(Guid.Parse("7082e391-06b1-4900-9f27-bf7a5cb854a4")), Name = "Product D", Price = 40.0m },
        };

    public static IEnumerable<Order.Domain.Models.Order> Orders
    {
        get
        {
            var address1 = Address.Of("first name", "lastname", "email@test.com", "123 Main St", "City A", "State A",
                "12345");
            var address2 = Address.Of("first name", "lastname", "email2@test.com", "123 st", "Country B", "City b",
                "123");

            var payment1 = Payment.Of("John Doe", "4111111111111111", "12/25", "123", 1);
            var payment2 = Payment.Of("Jane Smith", "4222222222222222", "11/24", "456", 2);

            var order1 = Order.Domain.Models.Order.Create(
                OrderId.Of(Guid.NewGuid()),
                Customers.First().Id,
                OrderName.Of("Order 1"), address1, address2, payment1);
            order1.Add(Products.First().Id, 2, 3);
            var order2 = Order.Domain.Models.Order.Create(
                OrderId.Of(Guid.NewGuid()),
                Customers.Last().Id,
                OrderName.Of("Order 2"), address2, address1, payment2);
            order2.Add(Products.Last().Id, 1, 4);
            return new List<Order.Domain.Models.Order> { order1, order2 };
        }
    }
}