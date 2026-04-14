using System.ComponentModel.DataAnnotations;

namespace MormorDagnysApi.DTOs.Suppliers;

public class PatchSupplierProductPriceDto
{
    [Range(0.01, double.MaxValue)]
    public decimal PricePerKg { get; set; }
}
