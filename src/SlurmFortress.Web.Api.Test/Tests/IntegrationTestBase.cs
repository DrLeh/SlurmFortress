using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SlurmFortress.Web.Api.Test.Factories;
using System.Net.Http.Headers;

namespace SlurmFortress.Web.Api.Test.Tests;

[Trait("IntegrationTest", "true")]
public class IntegrationTestBase : IClassFixture<SlurmFortressWebApplicationFactory<Api.Startup>>
{
    private readonly SlurmFortressWebApplicationFactory<Api.Startup> _factory;

    protected IntegrationTestBase(SlurmFortressWebApplicationFactory<Api.Startup> factory)
    {
        _factory = factory;
        _config = new FileConfiguration();
    }

    private readonly FileConfiguration _config;

    protected virtual bool UseMockAuthentication => true;
    protected virtual bool UseAuthentication => !_config.SUTIsLocal;

    private HttpClient? _client;

    protected HttpClient GetHttpClient()
    {
        if (_client != null)
            return _client;
        //only use mock auth in local
        if (UseMockAuthentication && _config.SUTIsLocal)
            return _client = GetMockAuthenticatedHttpClient();
        else if (UseAuthentication)
            return _client = GetAuthenticatedHttpClient();
        return _client = GetUnauthenticatedHttpClient();
    }


    protected HttpClient GetMockAuthenticatedHttpClient()
    {
        if (_config.SUTIsLocal)
        {
            var http = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            http.BaseAddress = new Uri(http.BaseAddress!, "api");
            return http;
        }

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(_config.SystemUnderTest);
        return httpClient;
    }

    protected HttpClient GetAuthenticatedHttpClient()
    {
        var accessToken = ""; // insert code to get access token
                              //var accessToken = TestUserAuthenticator.GetTokens().access;
        var httpClient = new HttpClient();

        //if (_config.SUTIsLocal)
        //    httpClient = _factory.CreateClient();
        //else
        {

            var url = _config.SystemUnderTest ?? "http://zeal.local-asicentral.com/home";
            httpClient.BaseAddress = new Uri(url);
        }

        //legacy
        //httpClient.DefaultRequestHeaders.Authorization =
        //    new AuthenticationHeaderValue("AsiMemberAuth", $"guid=\"{accessToken}\"");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return httpClient;
    }

    protected HttpClient GetUnauthenticatedHttpClient()
    {
        var sut = _config.SystemUnderTest;
        if (_config.SUTIsLocal)
            return _factory.CreateClient();

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(sut);
        return httpClient;
    }
}
