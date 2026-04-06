
namespace ProductAPI;

public class Supplier
{
   public int Id { get; set; }
   public string Name { get; set; } = string.Empty;
   public string Email { get; set; } = string.Empty;
   public string? Phone { get; set; }

   // One Supplier supplies many Products (1-to-M)
   public ICollection<Product> Products { get; set; } = new List<Product>();
}
