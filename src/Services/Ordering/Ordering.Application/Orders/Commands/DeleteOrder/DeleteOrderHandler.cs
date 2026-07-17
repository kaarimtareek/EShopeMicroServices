using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        //get order
        var orderId = OrderId.Of(request.OrderId);
        var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
        if (order == null)
        {
            return new DeleteOrderResult(false);
        }

        //delete
        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        //return result
        return new DeleteOrderResult(true);
    }
}