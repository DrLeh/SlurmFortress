using SlurmFortress.Web.Api.Test.Factories;

namespace SlurmFortress.Web.Api.Test.Tests;

public class SwaggerIntegrationTest : IntegrationTestBase
{
    public SwaggerIntegrationTest(SlurmFortressWebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    [Fact]
    public async Task AddModel()
    {
        var client = GetHttpClient();
        var res = await client.GetAsync("/swagger/v1/swagger.json");
        res.IsSuccessStatusCode.Should().BeTrue();
    }
}
