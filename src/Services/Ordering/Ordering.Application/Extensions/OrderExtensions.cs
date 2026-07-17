namespace Ordering.Application.Extensions;

public static class OrderExtensions
{
    public static IEnumerable<OrderDto> ToOrderDtos(this IEnumerable<Order.Domain.Models.Order> orders)
    {
        if (orders == null || !orders.Any())
            return [];

        return orders.Select(order => new OrderDto
        (Id: order.Id.Value,
            CustomerId: order.CustomerId.Value,
            OrderName: order.OrderName.Value,
            ShippingAddress: new AddressDto
            (FirstName: order.ShippingAddress.FirstName,
                LastName: order.ShippingAddress.LastName,
                EmailAddress: order.ShippingAddress.EmailAddress,
                AddressLine: order.ShippingAddress.AddressLine,
                Country: order.ShippingAddress.Country,
                State: order.ShippingAddress.State,
                ZipCode: order.ShippingAddress.ZipCode
            ),
            BillingAddress: new AddressDto
            (FirstName: order.BillingAddress.FirstName,
                LastName: order.BillingAddress.LastName,
                EmailAddress: order.BillingAddress.EmailAddress,
                AddressLine: order.BillingAddress.AddressLine,
                Country: order.BillingAddress.Country,
                State: order.BillingAddress.State,
                ZipCode: order.BillingAddress.ZipCode
            ),
            Payment: new PaymentDto
            (CardName: order.Payment.CardName,
                CardNumber: order.Payment.CardNumber,
                Expiration: order.Payment.CardExpirationDate,
                Cvv: order.Payment.CVV,
                PaymentMethod: order.Payment.PaymentMethod
            ),
            Status: order.Status,
            OrderItems: order.OrderItems.Select(item => new OrderItemDto
            (Id: item.Id.Value,
                ProductId: item.ProductId.Value,
                Price: item.Price,
                Quantity: item.Quantity
            )).ToList()
        ));
    }
}