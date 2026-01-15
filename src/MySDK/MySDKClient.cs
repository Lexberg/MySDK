using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace MySDK;

/// <summary>
/// Main SDK client implementation
/// </summary>
public class MySDKClient : IMySDKClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MySDKClient> _logger;
    private readonly MySDKOptions _options;
    private readonly JsonSerializerOptions _jsonOptions;

    public MySDKClient(HttpClient httpClient, MySDKOptions options, ILogger<MySDKClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        if (!string.IsNullOrEmpty(_options.BaseUrl))
        {
            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        }

        if (!string.IsNullOrEmpty(_options.ApiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_options.ApiKey}");
        }
    }

    public async Task<T?> GetAsync<T>(string resourceId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting resource: {ResourceId}", resourceId);
            var response = await _httpClient.GetAsync($"api/resources/{resourceId}", cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting resource: {ResourceId}", resourceId);
            throw;
        }
    }

    public async Task<TResponse?> CreateAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating resource");
            var response = await _httpClient.PostAsJsonAsync("api/resources", request, _jsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating resource");
            throw;
        }
    }

    public async Task<TResponse?> UpdateAsync<TRequest, TResponse>(string resourceId, TRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating resource: {ResourceId}", resourceId);
            var response = await _httpClient.PutAsJsonAsync($"api/resources/{resourceId}", request, _jsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating resource: {ResourceId}", resourceId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string resourceId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting resource: {ResourceId}", resourceId);
            var response = await _httpClient.DeleteAsync($"api/resources/{resourceId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting resource: {ResourceId}", resourceId);
            throw;
        }
    }
}
