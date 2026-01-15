namespace MySDK.Models;

/// <summary>
/// Request for getting wine recommendations
/// </summary>
public class WineRecommendationRequest
{
    /// <summary>
    /// Type of wine (red, white, rosé, sparkling)
    /// </summary>
    public string? WineType { get; set; }
    
    /// <summary>
    /// Food to pair with (e.g., "fish", "meat", "pasta")
    /// </summary>
    public string? FoodPairing { get; set; }
    
    /// <summary>
    /// Country of origin
    /// </summary>
    public string? Country { get; set; }
    
    /// <summary>
    /// Maximum price
    /// </summary>
    public decimal? MaxPrice { get; set; }
    
    /// <summary>
    /// Minimum price
    /// </summary>
    public decimal? MinPrice { get; set; }
    
    /// <summary>
    /// Taste profile (dry, sweet, etc.)
    /// </summary>
    public string? Taste { get; set; }
    
    /// <summary>
    /// Number of recommendations to return
    /// </summary>
    public int Limit { get; set; } = 10;
}
