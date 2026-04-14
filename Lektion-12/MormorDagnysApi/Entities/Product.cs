namespace MormorDagnysApi.Entities;

public class Product
{
    public int Id { get; set; }
    public string ArticleNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public List<SupplierProduct> SupplierProducts { get; set; } = new();
}