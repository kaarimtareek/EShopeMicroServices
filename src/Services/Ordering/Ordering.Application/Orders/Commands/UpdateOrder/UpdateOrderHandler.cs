using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        //update order entity 
        var orderId = OrderId.Of(request.Order.Id);
        var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
        if (order == null)
        {
            return new UpdateOrderResult(false);
        }

        // Update order properties
        UpdateOrder(order, request.Order);

        //save to db
        await dbContext.SaveChangesAsync(cancellationToken);
        //return result
        return new UpdateOrderResult(true);
    }

    private void UpdateOrder(Order.Domain.Models.Order order, OrderDto orderDto)
    {
        // Update shipping address
        var shippingAddressDto = orderDto.ShippingAddress;
        var shippingAddress = Address.Of(shippingAddressDto.FirstName,
            shippingAddressDto.LastName,
            shippingAddressDto.EmailAddress,
            shippingAddressDto.AddressLine,
            shippingAddressDto.Country,
            orderDto.ShippingAddress.State,
            orderDto.ShippingAddress.ZipCode);

        // Update billing address
        var billingAddressDto = orderDto.BillingAddress;
        var billingAddress = Address.Of(billingAddressDto.FirstName,
            billingAddressDto.LastName,
            billingAddressDto.EmailAddress,
            billingAddressDto.AddressLine,
            billingAddressDto.Country,
            orderDto.BillingAddress.State,
            orderDto.BillingAddress.ZipCode);

        // Update payment
        var paymentDto = orderDto.Payment;
        var payment = Payment.Of(paymentDto.CardName, paymentDto.CardNumber, paymentDto.Expiration, paymentDto.Cvv,
            paymentDto.PaymentMethod);

        order.Update(OrderName.Of(orderDto.OrderName), shippingAddress, billingAddress, payment, orderDto.Status);
    }
}