using System.ComponentModel.DataAnnotations;

namespace MormorDagnysApi.DTOs.Suppliers;

public class PostSupplierProductDto
{
    [Required]
    public string ArticleNumber { get; set; }

    [Required]
    public string Name { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal PricePerKg { get; set; }
}
