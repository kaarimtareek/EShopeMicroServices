using System.ComponentModel.DataAnnotations;

namespace Discount.GRPC.Models;

public class Coupon
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string ProductName { get; set; } = null!;
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    public int Amount { get; set; }
}