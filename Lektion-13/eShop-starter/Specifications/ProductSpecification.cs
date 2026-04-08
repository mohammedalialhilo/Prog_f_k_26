using eShop.Entities;

namespace eShop.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string? itemNumber, string? brand, string? sort):base(
    c => (string.IsNullOrWhiteSpace(itemNumber) || (c.ItemNumber == itemNumber)) && 
    (string.IsNullOrWhiteSpace(brand) || (c.Brand.ToLower() == brand.ToLower())))
    {
        switch (sort )
        {
            case "priceAsc":
                UserOrderByAscending(c => c.Price);
                break;
            case "priceDesc":
                UserOrderByDecending(c => c.Price);
                break;
            default: 
                UserOrderByAscending(c => c.ProductName);
                break;
            
        }
    }

}
