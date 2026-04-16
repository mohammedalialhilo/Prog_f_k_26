namespace core.Specifications;

public class ProductSpecificationParams
{
    private List<string> _brand =[];
    public string? Sort { get; set; }
    public string? ItemNumber { get; set; }
    public string? Search { get; set; }

    public List<string> Brands
    {
        get => _brand;
        set
        {
            _brand =[.. value.SelectMany(c => c.ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries))];
        }
    }


}
