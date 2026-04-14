namespace MormorDagnysApi.DTOs.Suppliers;

public class GetSupplierDto : GetSuppliersDto
{
    public List<GetSupplierProductDto> Products { get; set; } = [];
}
