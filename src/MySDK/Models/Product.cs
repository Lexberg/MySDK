namespace MySDK.Models;

/// <summary>
/// Represents a wine product from Vinmonopolet
/// </summary>
public class Product
{
    public string? ProductId { get; set; }
    public string? ProductNumber { get; set; }
    public string? Name { get; set; }
    public string? ProductType { get; set; }
    public string? ProductSubtype { get; set; }
    public decimal Price { get; set; }
    public decimal Volume { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? Vintage { get; set; }
    public decimal? AlcoholContent { get; set; }
    public string? Color { get; set; }
    public string? Taste { get; set; }
    public string? Aroma { get; set; }
    public string? FoodPairing { get; set; }
    public string? Producer { get; set; }
    public string? Distributor { get; set; }
    public string? AssortmentGrade { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int? Stock { get; set; }
    public bool IsAvailable { get; set; }
}
