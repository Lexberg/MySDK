namespace MySDK;

/// <summary>
/// Configuration options for the Vinmonopolet SDK
/// </summary>
public class MySDKOptions
{
    /// <summary>
    /// Base URL for the Vinmonopolet API (default: https://apis.vinmonopolet.no)
    /// </summary>
    public string BaseUrl { get; set; } = "https://apis.vinmonopolet.no";
    
    /// <summary>
    /// API subscription key from Azure API Management (Ocp-Apim-Subscription-Key header)
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
