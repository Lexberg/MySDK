# MySDK

A boilerplate .NET SDK for building API client integrations.

## Features

- ✅ Type-safe HTTP client
- ✅ Dependency injection support
- ✅ Structured logging
- ✅ Configurable retry logic
- ✅ Async/await throughout
- ✅ Unit tests included

## Installation

```bash
dotnet add package MySDK
```

## Quick Start

### Configuration

```csharp
using MySDK;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddMySDK(options =>
{
    options.BaseUrl = "https://api.example.com";
    options.ApiKey = "your-api-key";
    options.TimeoutSeconds = 30;
    options.EnableRetry = true;
    options.MaxRetryAttempts = 3;
});

var serviceProvider = services.BuildServiceProvider();
var client = serviceProvider.GetRequiredService<IMySDKClient>();
```

### Usage

```csharp
// Get a resource
var resource = await client.GetAsync<MyResource>("resource-id");

// Create a resource
var newResource = await client.CreateAsync<CreateRequest, MyResource>(new CreateRequest 
{
    Name = "New Resource"
});

// Update a resource
var updated = await client.UpdateAsync<UpdateRequest, MyResource>("resource-id", new UpdateRequest 
{
    Name = "Updated Name"
});

// Delete a resource
var deleted = await client.DeleteAsync("resource-id");
```

## Building

```bash
dotnet build
```

## Testing

```bash
dotnet test
```

## License

MIT
