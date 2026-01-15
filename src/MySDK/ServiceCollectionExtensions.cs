using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MySDK;

/// <summary>
/// Extension methods for registering the SDK with dependency injection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the MySDK client to the service collection
    /// </summary>
    public static IServiceCollection AddMySDK(this IServiceCollection services, Action<MySDKOptions> configureOptions)
    {
        var options = new MySDKOptions();
        configureOptions(options);
        
        services.AddSingleton(options);
        
        services.AddHttpClient<IMySDKClient, MySDKClient>(client =>
        {
            if (!string.IsNullOrEmpty(options.BaseUrl))
            {
                client.BaseAddress = new Uri(options.BaseUrl);
            }
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        });
        
        return services;
    }
}
