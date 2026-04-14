namespace MormorDagnysApi.DTOs.Products;

public class GetProductSupplierDto
{
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public decimal PricePerKg { get; set; }
}
