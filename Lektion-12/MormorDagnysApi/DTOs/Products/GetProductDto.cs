namespace MormorDagnysApi.DTOs.Products;

public class GetProductDto : GetProductsDto
{
    public List<GetProductSupplierDto> Suppliers { get; set; } = [];
}
