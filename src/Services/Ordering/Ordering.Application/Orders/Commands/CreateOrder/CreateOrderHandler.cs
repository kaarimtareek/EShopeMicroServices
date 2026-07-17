namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        //create order entity
        var order = CreateOrderEntity(request.Order);
        //save to db
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        //return result;
        return new CreateOrderResult(order.Id.Value);
    }

    private Order.Domain.Models.Order CreateOrderEntity(OrderDto orderDto)
    {
        var shippingAddressDto = orderDto.ShippingAddress;
        var shippingAddress = Address.Of(shippingAddressDto.FirstName,
            shippingAddressDto.LastName,
            shippingAddressDto.EmailAddress,
            shippingAddressDto.AddressLine,
            shippingAddressDto.Country,
            orderDto.ShippingAddress.State,
            orderDto.ShippingAddress.ZipCode);

        var billingAddressDto = orderDto.BillingAddress;
        var billingAddress = Address.Of(billingAddressDto.FirstName,
            billingAddressDto.LastName,
            billingAddressDto.EmailAddress,
            billingAddressDto.AddressLine,
            billingAddressDto.Country,
            orderDto.BillingAddress.State,
            orderDto.BillingAddress.ZipCode);

        var paymentDto = orderDto.Payment;
        var payment = Payment.Of(paymentDto.CardName, paymentDto.CardNumber, paymentDto.Expiration, paymentDto.Cvv,
            paymentDto.PaymentMethod);

        var order = Order.Domain.Models.Order.Create(OrderId.Of(Guid.NewGuid()), CustomerId.Of(Guid.NewGuid()),
            OrderName.Of(orderDto.OrderName), shippingAddress,
            billingAddress, payment);

        foreach (var orderItemDto in orderDto.OrderItems)
        {
            order.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Price, orderItemDto.Quantity);
        }

        return order;
    }
}