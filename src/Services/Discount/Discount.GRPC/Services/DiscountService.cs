using Discount.Grpc;
using Discount.GRPC.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : Grpc.DiscountService.DiscountServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.Where(c => c.ProductName == request.ProductName)
            .Select(c => new CouponModel
            {
                Id = c.Id,
                ProductName = c.ProductName,
                Description = c.Description,
                Amount = c.Amount,
            }).FirstOrDefaultAsync(context.CancellationToken) ?? new CouponModel()
        {
            Amount = 0,
            ProductName = "No Discount",
            Description = "No Coupon"
        };
        logger.LogInformation("Getting coupon {Id} for product {ProductName}", coupon.Id, coupon.ProductName);
        return coupon;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon;
        var entity = new Models.Coupon
        {
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = (int)coupon.Amount,
        };
        await dbContext.Coupons.AddAsync(entity, context.CancellationToken);
        await dbContext.SaveChangesAsync(context.CancellationToken);
        logger.LogInformation("Discount is successfully created. ProductName: {ProductName}", entity.ProductName);
        return new CouponModel()
        {
            Id = entity.Id,
            ProductName = entity.ProductName,
            Description = entity.Description,
            Amount = entity.Amount,
        };
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon;
        var entity = await dbContext.Coupons.Where(c => c.Id == coupon.Id)
            .FirstOrDefaultAsync(context.CancellationToken);
        if (entity == null) return null;
        entity.ProductName = coupon.ProductName;
        entity.Description = coupon.Description;
        entity.Amount = (int)coupon.Amount;
        await dbContext.SaveChangesAsync(context.CancellationToken);
        logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}", entity.ProductName);
        return new CouponModel()
        {
            Id = entity.Id,
            ProductName = entity.ProductName,
            Description = entity.Description,
            Amount = entity.Amount,
        };
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var productName = request.ProductName;
        var coupon = await dbContext.Coupons.Where(c => c.ProductName == productName)
            .FirstOrDefaultAsync(context.CancellationToken);
        if (coupon == null) return new DeleteDiscountResponse() { Success = false };
        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync(context.CancellationToken);
        logger.LogInformation("Discount is successfully deleted. ProductName: {ProductName}", productName);
        return new DeleteDiscountResponse() { Success = true };
    }
}