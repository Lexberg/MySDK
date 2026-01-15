namespace MySDK.Models;

/// <summary>
/// Request parameters for searching products
/// </summary>
public class ProductSearchRequest
{
    public string? Query { get; set; }
    public string? ProductType { get; set; }
    public string? Country { get; set; }
    public string? AssortmentGrade { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int PageSize { get; set; } = 20;
    public int Page { get; set; } = 1;
}
