namespace Basket.API.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = null!;
    public List<ShoppingCartItem> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
    //Required For Mapping
    public ShoppingCart(){}
}