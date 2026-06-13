namespace Catalog.API.Models;

public class Product
{
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Categories { get; set; } = [];
    public decimal Price { get; set; }
    public string ImageFile { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public static Product Create(string name, string description, List<string> categories, decimal price, string imageFile)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Categories = categories,
            Price = price,
            ImageFile = imageFile,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}