namespace MormorDagnysApi.Entities;

public class SupplierProduct
{
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public decimal PricePerKg { get; set; }
}
