namespace MySDK;

/// <summary>
/// Interface for the SDK client
/// </summary>
public interface IMySDKClient
{
    /// <summary>
    /// Gets a resource by ID
    /// </summary>
    Task<T?> GetAsync<T>(string resourceId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new resource
    /// </summary>
    Task<TResponse?> CreateAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing resource
    /// </summary>
    Task<TResponse?> UpdateAsync<TRequest, TResponse>(string resourceId, TRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a resource by ID
    /// </summary>
    Task<bool> DeleteAsync(string resourceId, CancellationToken cancellationToken = default);
}
