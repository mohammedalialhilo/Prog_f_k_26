namespace eShop.Entities;

public record OrderItem
{
    public int SalesOrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    // Navigation properties
    public SalesOrder SalesOrder { get; set; }
    public Product Product { get; set; }


}
