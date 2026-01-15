namespace MySDK;

/// <summary>
/// Configuration options for the SDK
/// </summary>
public class MySDKOptions
{
    /// <summary>
    /// Base URL for the API
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// API key for authentication
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
    
    /// <summary>
    /// Enable retry logic
    /// </summary>
    public bool EnableRetry { get; set; } = true;
    
    /// <summary>
    /// Maximum retry attempts
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;
}
