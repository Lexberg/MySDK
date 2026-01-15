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
    var manifest = new McpManifest
    {
        Server = new ServerInfo
        {
            Name = "Vinmonopolet SDK",
            Description = "Tool for searching Norwegian wine catalog and getting wine recommendations from Vinmonopolet",
            Version = "1.0.0"
        },
        Tools = new List<Tool>
        {
            new Tool
            {
                Name = "search_wines",
                Description = "Search for wines in the Vinmonopolet catalog. Can filter by query text, product type, country, and price range.",
                InputSchema = new ToolInputSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, PropertySchema>
                    {
                        ["query"] = new PropertySchema { Type = "string", Description = "Search query (wine name, producer, etc.)" },
                        ["productType"] = new PropertySchema { Type = "string", Description = "Type of product (e.g., Rødvin, Hvitvin, Musserende)" },
                        ["country"] = new PropertySchema { Type = "string", Description = "Country of origin" },
                        ["minPrice"] = new PropertySchema { Type = "number", Description = "Minimum price in NOK" },
                        ["maxPrice"] = new PropertySchema { Type = "number", Description = "Maximum price in NOK" },
                        ["pageSize"] = new PropertySchema { Type = "number", Description = "Number of results to return", Default = 20 }
                    }
                }
            },
            new Tool
            {
                Name = "get_wine_recommendations",
                Description = "Get wine recommendations based on criteria like wine type, food pairing, taste profile, and price range.",
                InputSchema = new ToolInputSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, PropertySchema>
                    {
                        ["wineType"] = new PropertySchema { Type = "string", Description = "Type of wine (red, white, rosé, sparkling)" },
                        ["foodPairing"] = new PropertySchema { Type = "string", Description = "Food to pair with (e.g., fish, beef, pasta, cheese)" },
                        ["country"] = new PropertySchema { Type = "string", Description = "Country of origin" },
                        ["taste"] = new PropertySchema { Type = "string", Description = "Taste profile (dry, sweet, fruity, etc.)" },
                        ["minPrice"] = new PropertySchema { Type = "number", Description = "Minimum price in NOK" },
                        ["maxPrice"] = new PropertySchema { Type = "number", Description = "Maximum price in NOK" },
                        ["limit"] = new PropertySchema { Type = "number", Description = "Number of recommendations", Default = 10 }
                    }
                }
            },
            new Tool
            {
                Name = "get_product_details",
                Description = "Get detailed information about a specific wine product by ID.",
                InputSchema = new ToolInputSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, PropertySchema>
                    {
                        ["productId"] = new PropertySchema { Type = "string", Description = "The product ID" }
                    },
                    Required = new List<string> { "productId" }
                }
            }
        }
    };
    
    return Results.Ok(manifest);
})
.WithName("McpManifest")
.WithOpenApi()
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
