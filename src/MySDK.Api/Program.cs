using MySDK;
using MySDK.Models;
using MySDK.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Vinmonopolet SDK
builder.Services.AddMySDK(options =>
{
    options.ApiKey = builder.Configuration["MySDK:ApiKey"] 
        ?? builder.Configuration["VINMONOPOLET_API_KEY"] 
        ?? throw new InvalidOperationException("API key not configured");
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

// API Endpoints
app.MapGet("/", () => Results.Redirect("/swagger"));

// MCP Manifest endpoint for tool discovery
app.MapGet("/.well-known/mcp.json", () =>
{
    var manifest = new
    {
        openapi = "3.1.0",
        info = new
        {
            title = "Vinmonopolet SDK",
            description = "Tool for searching Norwegian wine catalog and getting wine recommendations from Vinmonopolet",
            version = "1.0.0"
        },
        servers = new[]
        {
            new { url = "https://mysdk-ccqv.onrender.com" }
        },
        paths = new Dictionary<string, object>
        {
            ["/api/products"] = new
            {
                get = new
                {
                    operationId = "search_wines",
                    summary = "Search for wines in the Vinmonopolet catalog",
                    parameters = new[]
                    {
                        new { name = "query", @in = "query", schema = new { type = "string" }, description = "Search query (wine name, producer, etc.)" },
                        new { name = "productType", @in = "query", schema = new { type = "string" }, description = "Type of product (e.g., Rødvin, Hvitvin)" },
                        new { name = "country", @in = "query", schema = new { type = "string" }, description = "Country of origin" },
                        new { name = "minPrice", @in = "query", schema = new { type = "number" }, description = "Minimum price in NOK" },
                        new { name = "maxPrice", @in = "query", schema = new { type = "number" }, description = "Maximum price in NOK" },
                        new { name = "pageSize", @in = "query", schema = new { type = "integer", @default = 20 }, description = "Number of results" }
                    }
                }
            },
            ["/api/recommendations"] = new
            {
                post = new
                {
                    operationId = "get_wine_recommendations",
                    summary = "Get wine recommendations based on criteria",
                    requestBody = new
                    {
                        required = true,
                        content = new
                        {
                            applicationJson = new
                            {
                                schema = new
                                {
                                    type = "object",
                                    properties = new Dictionary<string, object>
                                    {
                                        ["wineType"] = new { type = "string", description = "Type of wine (red, white, rosé, sparkling)" },
                                        ["foodPairing"] = new { type = "string", description = "Food to pair with (e.g., fish, beef, pasta)" },
                                        ["country"] = new { type = "string", description = "Country of origin" },
                                        ["taste"] = new { type = "string", description = "Taste profile (dry, sweet, etc.)" },
                                        ["minPrice"] = new { type = "number", description = "Minimum price in NOK" },
                                        ["maxPrice"] = new { type = "number", description = "Maximum price in NOK" },
                                        ["limit"] = new { type = "integer", @default = 10, description = "Number of recommendations" }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            ["/api/products/{id}"] = new
            {
                get = new
                {
                    operationId = "get_product_details",
                    summary = "Get detailed information about a specific wine product",
                    parameters = new[]
                    {
                        new { name = "id", @in = "path", required = true, schema = new { type = "string" }, description = "Product ID" }
                    }
                }
            }
        }
    };
    
    return Results.Ok(manifest);
})
.WithName("McpManifest")
.ExcludeFromDescription();

app.MapGet("/api/assortmentgrades", async (IMySDKClient client, CancellationToken ct) =>
{
    var grades = await client.GetAssortmentGradesAsync(ct);
    return Results.Ok(grades);
})
.WithName("GetAssortmentGrades")
.WithOpenApi();

app.MapGet("/api/products", async (
    IMySDKClient client,
    string? query,
    string? productType,
    string? country,
    decimal? minPrice,
    decimal? maxPrice,
    int pageSize = 20,
    int page = 1,
    CancellationToken ct = default) =>
{
    var request = new ProductSearchRequest
    {
        Query = query,
        ProductType = productType,
        Country = country,
        MinPrice = minPrice,
        MaxPrice = maxPrice,
        PageSize = pageSize,
        Page = page
    };
    
    var products = await client.GetProductsAsync(request, ct);
    return Results.Ok(products);
})
.WithName("GetProducts")
.WithOpenApi();

app.MapGet("/api/products/{id}", async (IMySDKClient client, string id, CancellationToken ct) =>
{
    var product = await client.GetProductByIdAsync(id, ct);
    return product != null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById")
.WithOpenApi();

app.MapPost("/api/recommendations", async (IMySDKClient client, WineRecommendationRequest request, CancellationToken ct) =>
{
    var recommendations = await client.GetWineRecommendationsAsync(request, ct);
    return Results.Ok(recommendations);
})
.WithName("GetWineRecommendations")
.WithOpenApi();

app.Run();
