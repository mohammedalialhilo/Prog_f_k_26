namespace core.Specifications;

public class ProductSpecificationParams
{
    private const int MAX_PAGE_SIZE = 50;
    private int _pageSize = 30;

    private List<string> _brand =[];

    public int PageNumber {get; set;} = 1;
    public int PageSize 
    { 
        get => _pageSize; 
        set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
    }
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
