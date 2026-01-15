namespace MySDK.Models;

/// <summary>
/// Response containing products with pagination info
/// </summary>
public class ProductsResponse
{
    public List<Product> Products { get; set; } = new();
    public int TotalResults { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
}
