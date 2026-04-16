using core.Entities;

namespace core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecificationParams args) : base(c =>
        (string.IsNullOrEmpty(args.Search) || c.ProductName.ToLower().Contains(args.Search.ToLower())) &&
        (string.IsNullOrWhiteSpace(args.ItemNumber) || (c.ItemNumber == args.ItemNumber)) &&
        (!args.Brands.Any() || args.Brands.Contains(c.Brand.ToLower())))
    {
        switch (args.Sort)
        {
            case "priceAsc":
                UserOrderByAscending(c => c.Price);
                break;
            case "priceDesc":
                UserOrderByDescending(c => c.Price);
                break;
            default:
                UserOrderByAscending(c => c.ProductName);
                break;
        }
    }

    /*public ProductSpecification(string? itemNumber, string? brand, string? search, string? sort) : base(c =>
        (string.IsNullOrEmpty(search) || c.ProductName.ToLower().Contains(search)) &&
        (string.IsNullOrWhiteSpace(itemNumber) || (c.ItemNumber == itemNumber)) &&
        (string.IsNullOrWhiteSpace(brand) || (c.Brand.ToLower() == brand.ToLower())))
    {
        switch (sort)
        {
            case "priceAsc":
                UserOrderByAscending(c => c.Price);
                break;
            case "priceDesc":
                UserOrderByDescending(c => c.Price);
                break;
            default:
                UserOrderByAscending(c => c.ProductName);
                break;
        }
    }*/
}
