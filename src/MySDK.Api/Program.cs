using MySDK;
using MySDK.Models;

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
