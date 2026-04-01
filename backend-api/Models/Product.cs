
namespace ProductAPI;

public class Category
{
    public int Id{get;set;}
    public string? Name {get;set;}
    public ICollection<Product>? Products {get;set;}
}

public class Supplier
{
    public int Id{get;set;}

    public string? Name{get;set;}
    public string? Email{get;set;}
    public string? Phone{get;set;}
    public ICollection<Product>? Products{get;set;}


}
public class Product
{
    public int Id{get;set;}
    
    public string? Name {get;set;}

    public string? SKU{get;set;}
    public decimal Price {get;set;}
    public int Stock{get;set;}
    public int SupplierId{get;set;}
    public Supplier? Supplier{get;set;}

    public string? Description {get;set;}
    public int CategoryId{get;set;}

    public Category? Category {get;set;}
    public string? ImageUrl {get;set;}
    public ICollection<OrderItem>? OrderItems{get;set;}

}

public class Customer
{
    public int Id{get;set;}

    public string? FirstName{get;set;}
    public string? LastName{get;set;}
    public string? Email{get;set;}
    public string? Phone{get;set;}

    public ICollection<Order>? Orders{get;set;}
}

public class Order
{
    public int Id{get;set;}

    public DateTime OrderDate{get;set;}
    public string?  Status{get;set;}
    public int CustomerId{get;set;}

    public Customer? Customer{get;set;}
    public ICollection<OrderItem>? OrderItems{get;set;}

}

public class OrderItem
{
        public int Id{get;set;}

    public int ProductId{get;set;}
    public int Quantity{get;set;}
    public int UnitPrice{get;set;}
    public int OrderId{get;set;}

    public Order? Order{get;set;}
    public Product? Product{get;set;}


}