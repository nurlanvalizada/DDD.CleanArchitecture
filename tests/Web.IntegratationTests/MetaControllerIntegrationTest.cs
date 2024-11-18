using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests;

public class MetaControllerIntegrationTest(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Info_RetrunAssemblyInfo()
    {
        var httpResponse = await _client.GetAsync("/info");

        httpResponse.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        var responseString = await httpResponse.Content.ReadAsStringAsync();
        Assert.Contains("Version: ", responseString);
        Assert.Contains("Last Updated: ", responseString);

    }
}