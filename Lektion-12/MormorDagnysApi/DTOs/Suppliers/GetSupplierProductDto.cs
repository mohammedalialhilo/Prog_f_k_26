namespace MormorDagnysApi.DTOs.Suppliers;

public class GetSupplierProductDto
{
    public int ProductId { get; set; }
    public string ArticleNumber { get; set; }
    public string Name { get; set; }
    public decimal PricePerKg { get; set; }
}
