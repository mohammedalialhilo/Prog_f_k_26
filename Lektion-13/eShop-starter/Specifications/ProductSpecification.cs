using eShop.Entities;

namespace eShop.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string? itemNumber, string? brand):base(
    c => (string.IsNullOrWhiteSpace(itemNumber) || (c.ItemNumber == itemNumber)) && 
    (string.IsNullOrWhiteSpace(brand) || (c.Brand.ToLower() == brand.ToLower()))) 
    {}

}
