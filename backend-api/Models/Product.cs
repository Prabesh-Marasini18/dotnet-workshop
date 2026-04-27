
namespace ProductAPI;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQty { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}