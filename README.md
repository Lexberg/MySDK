# Vinmonopolet SDK

Et .NET SDK for integrering med Vinmonopolet sitt API. Gjør det enkelt å søke etter vinprodukter og få vinforslag.

Prosjektet består av:
- **MySDK** - Kjerne SDK-bibliotek for direkte integrering
- **MySDK.Api** - REST API wrapper for deployment til Render/Azure

## Funksjoner

- ✅ Søk i Vinmonopolets produktkatalog
- ✅ Få vinforslag basert på kriterier
- ✅ Hent produktdetaljer
- ✅ Hent sortimentskategorier
- ✅ Type-sikker HTTP-klient
- ✅ Dependency injection support
- ✅ Strukturert logging
### Som NuGet pakke (for direkte integrering)
```bash
dotnet add package VinmonopoletSDK
```

### Som REST API (deployed på Render)
Bruk den deployede APIen på: `https://your-app.onrender.com`

Se [API dokumentasjon](#api-endpoints) under.Installasjon

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

## API Endpoints

APIet eksponerer følgende endpoints:

- `GET /api/assortmentgrades` - Hent alle sortimentskategorier
- `GET /api/products` - Søk produkter (query params: query, productType, country, minPrice, maxPrice, pageSize, page)
- `GET /api/products/{id}` - Hent produkt etter ID
- `POST /api/recommendations` - Få vinforslag (body: WineRecommendationRequest JSON)

Swagger dokumentasjon er tilgjengelig på `/swagger` når APIet kjører.

## Kjør API lokalt

```bash
cd src/MySDK.Api
dotnet run
```

API vil være tilgjengelig på http://localhost:5000 og Swagger på http://localhost:5000/swagger

## Docker

### Bygg Docker image
```bash
docker build -t vinmonopolet-sdk .
```

### Kjør lokalt med Docker
```bash
docker run -p 8080:8080 -e VINMONOPOLET_API_KEY=din-api-nøkkel vinmonopolet-sdk
```

API vil være tilgjengelig på http://localhost:8080ker build -t vinmonopolet-sdk .
```

### Kjør lokalt med Docker
```bash
docker run -p 8080:8080 -e MySDK__ApiKey=din-api-nøkkel vinmonopolet-sdk
```

### Docker Compose
```bash
docker-compose up
```

## Deployment til Render

Se [RENDER_DEPLOY.md](RENDER_DEPLOY.md) for detaljert guide til deployment på Render.

**Rask start:**
1. Push koden til GitHub
2. Opprett ny Web Service på Render
3. Koble til GitHub repository
4. Velg "Docker" som environment
5. Legg til `VINMONOPOLET_API_KEY` environment variable
6. Deploy!

## Lisens

MIT
