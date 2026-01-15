# Vinmonopolet SDK

Et .NET SDK for integrering med Vinmonopolet sitt API. Gjør det enkelt å søke etter vinprodukter og få vinforslag.

## Funksjoner

- ✅ Søk i Vinmonopolets produktkatalog
- ✅ Få vinforslag basert på kriterier
- ✅ Hent produktdetaljer
- ✅ Hent sortimentskategorier
- ✅ Type-sikker HTTP-klient
- ✅ Dependency injection support
- ✅ Strukturert logging
- ✅ Async/await gjennom hele SDKen

## Installasjon

```bash
dotnet add package VinmonopoletSDK
```

## Kom i gang

### Konfigurering

Du trenger en API-nøkkel fra Vinmonopolet. Registrer deg på [Vinmonopolets utviklerportal](https://apis.vinmonopolet.no) for å få tilgang.

```csharp
using MySDK;
using MySDK.Models;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddMySDK(options =>
{
    options.ApiKey = "din-api-nøkkel";
    // BaseUrl er allerede satt til https://apis.vinmonopolet.no
});

var serviceProvider = services.BuildServiceProvider();
var client = serviceProvider.GetRequiredService<IMySDKClient>();
```

### Få vinforslag

```csharp
// Få vinforslag til middag
var recommendations = await client.GetWineRecommendationsAsync(new WineRecommendationRequest
{
    WineType = "red",
    FoodPairing = "beef",
    MaxPrice = 300,
    Limit = 5
});

foreach (var wine in recommendations)
{
    Console.WriteLine($"{wine.Name} - {wine.Price} kr");
    Console.WriteLine($"Fra: {wine.Country}, {wine.Region}");
    Console.WriteLine($"Passer til: {wine.FoodPairing}");
    Console.WriteLine();
}
```

### Søk etter produkter

```csharp
// Søk etter italiensk rødvin
var products = await client.GetProductsAsync(new ProductSearchRequest
{
    Query = "Barolo",
    Country = "Italia",
    ProductType = "Rødvin",
    MaxPrice = 500,
    PageSize = 10
});

Console.WriteLine($"Fant {products.TotalResults} produkter");
foreach (var product in products.Products)
{
    Console.WriteLine($"{product.Name} - {product.Price} kr");
}
```

### Hent produktdetaljer

```csharp
// Hent et spesifikt produkt
var product = await client.GetProductByIdAsync("12345");

if (product != null)
{
    Console.WriteLine($"Produkt: {product.Name}");
    Console.WriteLine($"Pris: {product.Price} kr");
    Console.WriteLine($"Alkohol: {product.AlcoholContent}%");
    Console.WriteLine($"Smak: {product.Taste}");
    Console.WriteLine($"Aroma: {product.Aroma}");
    Console.WriteLine($"Passer til: {product.FoodPairing}");
}
```

### Hent sortimentskategorier

```csharp
// Se alle tilgjengelige sortimentskategorier
var assortmentGrades = await client.GetAssortmentGradesAsync();

foreach (var grade in assortmentGrades.AssortmentGrades)
{
    Console.WriteLine($"{grade.Code}: {grade.Name}");
    Console.WriteLine($"  {grade.Description}");
}
```

## Eksempler på vinforslag

### Vin til fiskerett
```csharp
var fishWines = await client.GetWineRecommendationsAsync(new WineRecommendationRequest
{
    WineType = "white",
    FoodPairing = "fish",
    MaxPrice = 250,
    Taste = "dry",
    Limit = 5
});
```

### Italiensk rødvin til pasta
```csharp
var pastaWines = await client.GetWineRecommendationsAsync(new WineRecommendationRequest
{
    WineType = "red",
    Country = "Italia",
    FoodPairing = "pasta",
    MinPrice = 150,
    MaxPrice = 400,
    Limit = 10
});
```

### Musserende vin til fest
```csharp
var sparklingWines = await client.GetWineRecommendationsAsync(new WineRecommendationRequest
{
    WineType = "sparkling",
    MaxPrice = 300,
    Limit = 5
});
```

## Bygging

```bash
dotnet build
```

## Testing

```bash
dotnet test
```

## Lisens

MIT
