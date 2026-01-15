namespace MySDK;

/// <summary>
/// Interface for the Vinmonopolet SDK client
/// </summary>
public interface IMySDKClient
{
    /// <summary>
    /// Gets all assortment grades (sortiment kategorier)
    /// </summary>
    Task<AssortmentGradesResponse?> GetAssortmentGradesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets wine products from the catalog
    /// </summary>
    Task<ProductsResponse?> GetProductsAsync(ProductSearchRequest? request = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a specific product by ID
    /// </summary>
    Task<Product?> GetProductByIdAsync(string productId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets wine recommendations based on criteria
    /// </summary>
    Task<List<Product>> GetWineRecommendationsAsync(WineRecommendationRequest request, CancellationToken cancellationToken = default);
}
