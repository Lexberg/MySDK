using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MySDK.Models;

namespace MySDK;

/// <summary>
/// Vinmonopolet SDK client implementation
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
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _options.ApiKey);
        }
    }

    public async Task<AssortmentGradesResponse?> GetAssortmentGradesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting assortment grades from Vinmonopolet");
            var response = await _httpClient.GetAsync("my-products/v1/assortmentgrades", cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AssortmentGradesResponse>(_jsonOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assortment grades");
            throw;
        }
    }

    public async Task<ProductsResponse?> GetProductsAsync(ProductSearchRequest? request = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var queryString = BuildQueryString(request);
            _logger.LogInformation("Getting products from Vinmonopolet with query: {Query}", queryString);
            
            var response = await _httpClient.GetAsync($"my-products/v1/products?{queryString}", cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductsResponse>(_jsonOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products");
            throw;
        }
    }

    public async Task<Product?> GetProductByIdAsync(string productId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting product: {ProductId}", productId);
            var response = await _httpClient.GetAsync($"my-products/v1/products/{productId}", cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>(_jsonOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product: {ProductId}", productId);
            throw;
        }
    }

    public async Task<List<Product>> GetWineRecommendationsAsync(WineRecommendationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting wine recommendations");
            
            var searchRequest = new ProductSearchRequest
            {
                ProductType = request.WineType,
                Country = request.Country,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                PageSize = request.Limit
            };

            var response = await GetProductsAsync(searchRequest, cancellationToken);
            var products = response?.Products ?? new List<Product>();

            // Filter by additional criteria
            if (!string.IsNullOrEmpty(request.FoodPairing))
            {
                products = products.Where(p => 
                    !string.IsNullOrEmpty(p.FoodPairing) && 
                    p.FoodPairing.Contains(request.FoodPairing, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(request.Taste))
            {
                products = products.Where(p => 
                    !string.IsNullOrEmpty(p.Taste) && 
                    p.Taste.Contains(request.Taste, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return products.Take(request.Limit).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting wine recommendations");
            throw;
        }
    }

    private string BuildQueryString(ProductSearchRequest? request)
    {
        if (request == null)
            return string.Empty;

        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(request.Query))
            queryParams.Add($"q={Uri.EscapeDataString(request.Query)}");
        
        if (!string.IsNullOrEmpty(request.ProductType))
            queryParams.Add($"productType={Uri.EscapeDataString(request.ProductType)}");
        
        if (!string.IsNullOrEmpty(request.Country))
            queryParams.Add($"country={Uri.EscapeDataString(request.Country)}");
        
        if (!string.IsNullOrEmpty(request.AssortmentGrade))
            queryParams.Add($"assortmentGrade={Uri.EscapeDataString(request.AssortmentGrade)}");
        
        if (request.MinPrice.HasValue)
            queryParams.Add($"minPrice={request.MinPrice.Value}");
        
        if (request.MaxPrice.HasValue)
            queryParams.Add($"maxPrice={request.MaxPrice.Value}");
        
        queryParams.Add($"pageSize={request.PageSize}");
        queryParams.Add($"page={request.Page}");

        return string.Join("&", queryParams);
    }
}
