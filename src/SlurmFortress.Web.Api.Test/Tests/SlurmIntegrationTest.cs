using Refit;
using SlurmFortress.Web.Api.Test.Factories;

namespace SlurmFortress.Web.Api.Test.Tests.Client;

/// <summary>
/// The same tests can be ran using the client app via ASI.Services.SlurmFortress
/// </summary>
public class SlurmIntegrationTest : IntegrationTestBase
{
    public SlurmIntegrationTest(SlurmFortressWebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    private SlurmsClient GetClient()
    {
        var refitClient = RestService.For<ISlurmsApi>(GetHttpClient());
        var client = new SlurmsClient(refitClient);
        return client;
    }

    [Fact]
    public async Task Client_Add_Test()
    {
        var client = GetClient();
        var res = await client.AddSlurmAsync(new SlurmView
        {
        });

        res.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Client_Add_Get_Test()
    {
        var client = GetClient();
        var res = await client.AddSlurmAsync(new SlurmView
        {
        });

        res.Id.Should().NotBe(Guid.Empty);

        var getRes = await client.GetSlurmAsync(res.Id);
    }

    [Fact]
    public async Task Client_Add_NoName_BadRequest_Test()
    {
        var client = GetClient();

        await Assert.ThrowsAsync<SlurmFortressBadRequestException>(async () => await client.AddSlurmAsync(new SlurmView
        {
        }));
    }
}
