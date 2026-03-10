using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.Entities;

public record SalesOrder
{
    public int SalesOrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    // Navigation property
    // [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}
