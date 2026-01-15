using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MySDK.Tests;

public class MySDKClientTests
{
    private readonly Mock<ILogger<MySDKClient>> _loggerMock;
    private readonly MySDKOptions _options;

    public MySDKClientTests()
    {
        _loggerMock = new Mock<ILogger<MySDKClient>>();
        _options = new MySDKOptions
        {
            BaseUrl = "https://api.example.com",
            ApiKey = "test-key"
        };
    }

    [Fact]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new MySDKClient(null!, _options, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_WithNullOptions_ThrowsArgumentNullException()
    {
        // Arrange
        var httpClient = new HttpClient();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new MySDKClient(httpClient, null!, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        var httpClient = new HttpClient();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new MySDKClient(httpClient, _options, null!));
    }
}
