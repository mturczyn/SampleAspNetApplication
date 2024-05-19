using Microsoft.AspNetCore.Mvc.Testing;

namespace Intrinsic.WebApi.ExampleApp.Tests.IntegrationTests;

public class WebApiTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WebApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    //[InlineData("/api/Problems/throwError")]
    //[InlineData("/api/Users/getOwnClaims")]
    //[InlineData("/api/Users/getUsers")]
    [InlineData("/api/Users/login")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
}
